using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Infra;
using MongoDB.Driver;
using Amg_ingressos_aqui_cadastro_api.Enum;
using MongoDB.Bson;
using Amg_ingressos_aqui_cadastro_api.Exceptions;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserRepository(IDbConnection dbConnection)
        {
            _userCollection = dbConnection.GetConnection<User>("user");
        }

        public async Task<User> Save(User user)
        {
            await _userCollection.InsertOneAsync(user);

            if (user.Id is null)
                throw new RuleException("Erro ao salvar usuario");

            return user;
        }

        public async Task<bool> DoesValueExistsOnField(string fieldName, object value)
        {
            var filter = Builders<User>.Filter.Eq(fieldName, value);
            var user = await _userCollection.Find(filter).FirstOrDefaultAsync();
            if (user is null)
                return false;
            return true;
        }

        public async Task<T> GetUser<T>(string id)
        {
            var filter = Builders<User>.Filter.Eq("Id", id);
            var user = await _userCollection
                                .Find(filter)
                                .As<T>()
                                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<T> GetByField<T>(string fieldName, object value)
        {
            var filter = Builders<User>.Filter.Eq(fieldName, value);
            var user = await _userCollection
                                .Find(filter)
                                .As<T>()
                                .FirstOrDefaultAsync();
            if (user == null)
                throw new RuleException("Usuário não encontrado por " + fieldName + ".");

            return user;
        }


        public async Task<User> UpdateUser(string id, User user)
        {
            //Monta lista de campos, que serão atualizado - Set do update
            var updateDefination = new List<UpdateDefinition<User>>();

            if (!string.IsNullOrEmpty(user.Name)) updateDefination.Add(Builders<User>.Update.Set(userMongo => userMongo.Name, user.Name));
            if (!string.IsNullOrEmpty(user.DocumentId)) updateDefination.Add(Builders<User>.Update.Set(userMongo => userMongo.DocumentId, user.DocumentId));

            if (user.Address != null) updateDefination.Add(Builders<User>.Update.Set(userMongo => userMongo.Address, user.Address));
            if (user.Contact != null) updateDefination.Add(Builders<User>.Update.Set(userMongo => userMongo.Contact, user.Contact));

            if (!string.IsNullOrEmpty(user.Password))
            {
                updateDefination.Add(Builders<User>.Update.Set(userMongo => userMongo.Password, user.Password));
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
                // The data was successfully updated
                return user;
            else
                throw new RuleException("Erro ao atualizar usuario.");
        }

        public async Task<bool> Delete(object id)
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
                    if (result.IsAcknowledged && result.ModifiedCount > 0)
                        // The data was successfully updated
                        return true;
                    else
                        throw new RuleException("Erro ao atualizar usuario.");
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
                    if (result.IsAcknowledged && result.ModifiedCount > 0)
                        // The data was successfully updated
                        return true;
                    else
                        throw new RuleException("Erro ao atualizar usuario.");
                }
            }
            else
                throw new RuleException("Usuário não encontrado.");
        }

        public async Task<List<T>> Get<T>(FiltersUser? filters)
        {
            var filtersOptions = new List<FilterDefinition<User>> { Builders<User>.Filter.Empty };

            if (filters != null)
            {
                if (!string.IsNullOrEmpty(filters.Name))
                {
                    filtersOptions.Add(
                        Builders<User>.Filter.Regex("Name",
                            new BsonRegularExpression(filters.Name, "i")
                        )
                    );
                }

                if (!string.IsNullOrEmpty(filters.Email))
                {
                    filtersOptions.Add(
                        Builders<User>.Filter.Regex("Contact.Email",
                            new BsonRegularExpression(filters.Email, "i")
                        )
                    );
                }

                if (!string.IsNullOrEmpty(filters.PhoneNumber))
                {
                    filtersOptions.Add(
                        Builders<User>.Filter.Regex("Contact.PhoneNumber",
                            new BsonRegularExpression(filters.PhoneNumber, "i")
                        )
                    );
                }

                filtersOptions.Add(Builders<User>.Filter.Eq("Type", filters.Type));
            }

            var filter = Builders<User>.Filter.And(filtersOptions);
            var pResults = await _userCollection
                                .Aggregate()
                                .Match(filter)
                                .As<T>()
                                .ToListAsync();

            return pResults;
        }

        public async Task<bool> UpdatePasswordUser(string id, string password)
        {
            var update = Builders<User>.Update.Set(userMongo => userMongo.Password, password);
            var filter = Builders<User>.Filter.Eq(userMongo => userMongo.Id, id);

            UpdateResult updateResult = await _userCollection.UpdateOneAsync(filter, update);
            if (updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)
                // The data was successfully updated
                return true;
            else if (updateResult.ModifiedCount == 0 && updateResult.MatchedCount > 0)
                throw new RuleException("Nova senha não pode ser igual a antiga.");
            else
                throw new RuleException("Erro ao atualizar usuario.");
        }
    }
}