using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Infra;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class UserRepository<T> : IUserRepository
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserRepository(IDbConnection<User> dbConnection) {
            this._userCollection = dbConnection.GetConnection("user");
        }
        
        public async Task<object> Save<T>(User userComplet) {
            try {
                await this._userCollection.InsertOneAsync(userComplet);

                if (userComplet.Id is null)
                    throw new SaveUserException("Erro ao salvar usuario");

                return userComplet.Id;
            }
            catch (SaveUserException ex) {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DoesValueExistsOnField<T>(string fieldName, object value) {
            try {
                var filter = Builders<User>.Filter.Eq(fieldName, value);
                var user = await _userCollection.Find(filter).FirstOrDefaultAsync();
                if (user is null)
                    return false;
                return true;
            }   
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> FindByField<T>(string fieldName, object value) {
            try {

                var filter = Builders<User>.Filter.Eq(fieldName, value);
                var user = await _userCollection.Find(filter).FirstOrDefaultAsync();
                if (user is not null)
                    return user;
                else
                    throw new UserNotFound("Usuário não encontrado por " + fieldName + ".");
            }
            catch (UserNotFound ex) {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> UpdateUser<T>(object id, User userModel) {
            try {
                var update = Builders<User>.Update
                   .Set(userMongo => userMongo.Name, userModel.Name)
                   .Set(userMongo => userMongo.DocumentId, userModel.DocumentId)
                   .Set(userMongo => userMongo.Address, userModel.Address)
                   .Set(userMongo => userMongo.Contact, userModel.Contact)
                   .Set(userMongo => userMongo.UserConfirmation, userModel.UserConfirmation)
                   .Set(userMongo => userMongo.Password, userModel.Password);

                var filter = Builders<User>.Filter
                    .Eq(userMongo => userMongo.Id, userModel.Id);

                UpdateResult updateResult = await _userCollection.UpdateOneAsync(filter, update);
                if (updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)
                {
                    // The data was successfully updated
                    return "Usuário Atualizado.";
                }
                else
                {
                    throw new UpdateUserException("Erro ao atualizar usuario.");
                }
            }
            catch (UpdateUserException ex) {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public async Task<object> Delete<T>(object id) {
            try
            {
                var result = await _userCollection.DeleteOneAsync(x => x.Id == id as string);
                if (result.DeletedCount >= 1)
                    return "Usuário Deletado.";
                else
                    throw new DeleteUserException("Usuário não encontrado.");
            }
            catch (DeleteUserException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<User>> GetAll<T>(string email) {
            try
            {
                var builder = Builders<User>.Filter;
                var filter = builder.Empty;

                if (!string.IsNullOrWhiteSpace(email))
                    filter &= builder.Eq(x => x.Contact.Email, email);

                var result = await _userCollection.Find(filter).ToListAsync();

                if (!result.Any())
                    throw new GetAllUserException("Usuários não encontrados");

                return result;

                //List<User> result = await _userCollection.Find(_ => true).ToListAsync();
                //if (!result.Any())
                    //throw new GetAllUserException("Usuários não encontrados");

                //return result;
            }
            catch (GetAllUserException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}