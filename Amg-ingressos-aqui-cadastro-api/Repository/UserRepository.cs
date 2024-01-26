using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Infra;
using MongoDB.Driver;
using Amg_ingressos_aqui_cadastro_api.Enum;
using MongoDB.Bson;
using Amg_ingressos_aqui_cadastro_api.Exceptions;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class UserRepository<T> : IUserRepository
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserRepository(IDbConnection dbConnection)
        {
            _userCollection = dbConnection.GetConnection<User>("user");
        }

        public async Task<object> Save<T>(User userComplet)
        {

            await _userCollection.InsertOneAsync(userComplet);

            if (userComplet.Id is null)
                throw new RuleException("Erro ao salvar usuario");

            return userComplet.Id;

        }

        public async Task<bool> DoesValueExistsOnField<T>(string fieldName, object value)
        {

            var filter = Builders<User>.Filter.Eq(fieldName, value);
            var user = await _userCollection.Find(filter).FirstOrDefaultAsync();
            if (user is null)
                return false;
            return true;

        }

        public async Task<User> GetUser(string id)
        {

            var filter = Builders<User>.Filter.Eq("Id", id);
            var user = await _userCollection.Find(filter).FirstOrDefaultAsync();

            return user;

        }

        public async Task<User> FindByField<T>(string fieldName, object value)
        {

            var filter = Builders<User>.Filter.Eq(fieldName, value);
            var user = await _userCollection.Find(filter).FirstOrDefaultAsync();
            if (user is not null)
                return user;
            else
                throw new RuleException("Usuário não encontrado por " + fieldName + ".");

        }

        public async Task<object> UpdateUser<T>(string id, User userModel)
        {

            //Monta lista de campos, que serão atualizado - Set do update
            var updateDefination = new List<UpdateDefinition<User>>();

            if (!string.IsNullOrEmpty(userModel.Name)) updateDefination.Add(Builders<User>.Update.Set(userMongo => userMongo.Name, userModel.Name));
            if (!string.IsNullOrEmpty(userModel.DocumentId)) updateDefination.Add(Builders<User>.Update.Set(userMongo => userMongo.DocumentId, userModel.DocumentId));

            if (userModel.Address != null) updateDefination.Add(Builders<User>.Update.Set(userMongo => userMongo.Address, userModel.Address));
            if (userModel.Contact != null) updateDefination.Add(Builders<User>.Update.Set(userMongo => userMongo.Contact, userModel.Contact));

            if (!string.IsNullOrEmpty(userModel.Password))
            {
                updateDefination.Add(Builders<User>.Update.Set(userMongo => userMongo.Password, userModel.Password));
            }

            //Where do update
            var filter = Builders<User>.Filter.Eq(userMongo => userMongo.Id, id);

            //Prepara o objeto para atualização
            var combinedUpdate = Builders<User>.Update.Combine(updateDefination);

            //Realiza o update
            UpdateResult updateResult = await _userCollection.UpdateOneAsync(filter, combinedUpdate, new UpdateOptions() { });

            //Se comando executado com sucesso e
            //encontrou o usuario (id) na collection (MatchedCount > 0) e
            //atualizou (ModifiedCount > 0) ou não atualiza um a linha (no put, veio o mesmo registro que ja estava no banco de dados)
            //retorna sucesso no comando update
            if (updateResult.IsAcknowledged && updateResult.MatchedCount > 0 && updateResult.ModifiedCount >= 0)
            {
                // The data was successfully updated
                return updateResult;
            }
            else
            {
                throw new RuleException("Erro ao atualizar usuario.");
            }

        }

        public async Task<object> Delete<T>(object id)
        {

            var user = await _userCollection
                .Find(Builders<User>.Filter.Eq(userMongo => userMongo.Id, id))
                .FirstOrDefaultAsync();

            if (user != null)
            {
                if (user.Status == TypeStatus.Inactive)
                {
                    var result = await _userCollection.UpdateOneAsync(
                        Builders<User>.Filter.Eq(userMongo => userMongo.Id, id),
                        Builders<User>.Update.Set(
                            userMongo => userMongo.Status,
                            TypeStatus.Active
                        )
                    );
                    return "Usuário Reativado.";
                }
                else
                {
                    var result = await _userCollection.UpdateOneAsync(
                        Builders<User>.Filter.Eq(userMongo => userMongo.Id, id),
                        Builders<User>.Update.Set(
                            userMongo => userMongo.Status,
                            TypeStatus.Inactive
                        )
                    );
                    return "Usuário Deletado.";
                }
            }
            else
                throw new RuleException("Usuário não encontrado.");

        }

        public async Task<List<User>> Get<T>(FiltersUser? filterOptions)
        {

            var filters = new List<FilterDefinition<User>> { Builders<User>.Filter.Empty };

            if (filterOptions != null)
            {
                if (!string.IsNullOrEmpty(filterOptions.Name))
                {
                    filters.Add(
                        Builders<User>.Filter.Regex(
                            g => g.Name,
                            new BsonRegularExpression(filterOptions.Name, "i")
                        )
                    );
                }

                if (!string.IsNullOrEmpty(filterOptions.Email))
                {
                    filters.Add(
                        Builders<User>.Filter.Regex(
                            g => g.Contact!.Email,
                            new BsonRegularExpression(filterOptions.Email, "i")
                        )
                    );
                }

                if (!string.IsNullOrEmpty(filterOptions.PhoneNumber))
                {
                    filters.Add(
                        Builders<User>.Filter.Regex(
                            g => g.Contact!.PhoneNumber,
                            new BsonRegularExpression(filterOptions.PhoneNumber, "i")
                        )
                    );
                }

                if (filterOptions.Type != null)
                {
                    filters.Add(Builders<User>.Filter.Eq(g => g.Type, filterOptions.Type));
                }
            }

            var filter = Builders<User>.Filter.And(filters);
            var pResults = _userCollection.Find(filter).ToList();

            if (pResults.Count == 0)
            {
                throw new RuleException("Usuarios não encontrados");
            }

            return pResults;

        }

        public async Task<object> UpdatePasswordUser<T>(string id, string password)
        {

            var update = Builders<User>.Update.Set(userMongo => userMongo.Password, password);
            var filter = Builders<User>.Filter.Eq(userMongo => userMongo.Id, id);

            UpdateResult updateResult = await _userCollection.UpdateOneAsync(filter, update);
            if (updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)
            {
                // The data was successfully updated
                return "Usuário Atualizado.";
            }
            else if (updateResult.ModifiedCount == 0 && updateResult.MatchedCount > 0)
            {
                throw new RuleException("Nova senha não pode ser igual a antiga.");
            }
            else
            {
                throw new RuleException("Erro ao atualizar usuario.");
            }

        }

        public async Task<User> FindByGenericField<T>(string fieldName, object value)
        {
            var filter = Builders<User>.Filter.Eq(fieldName, value);
            var user = await _userCollection.Find(filter).FirstOrDefaultAsync();
            if (user is not null)
                return user;
            else
                return null;

        }
    }
}
