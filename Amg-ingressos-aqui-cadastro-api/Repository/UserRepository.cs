using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_cadastro_api.Infra;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class UserRepository<T> : IUserRepository
    {
        private readonly IMongoCollection<User> _userCollection;

        /* public UserRepository(IDbConnection<User> dbConnection, string modelName) {
            this._userCollection = dbConnection.GetConnection(modelName);
        } */

        public UserRepository(IDbConnection<User> dbConnection) {
            this._userCollection = dbConnection.GetConnection("User");
        }
        
        public async Task<object> Save<T>(object userComplet) {
            try {
                await this._userCollection.InsertOneAsync(userComplet as User);

                if ((userComplet as User).Id is null)
                    throw new SaveUserException("Erro ao salvar usuario");

                return (userComplet as User).Id;
            }
            catch (SaveUserException ex) {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<object>> GetAllUsers<T>() {
            try
            {
                var result = await _userCollection.Find(_ => true).ToListAsync();
                if (!result.Any())
                    throw new GetAllUserException("Usuários não encontrados");

                return result;
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

        public async Task<bool> DoesValueExistsOnField<T>(string fieldName, T value) {
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

        public async Task<object> FindByField<T>(string fieldName, string value) {
            try {

                var filter = Builders<User>.Filter.Eq(fieldName, value);
                var user = await _userCollection.Find(filter).FirstOrDefaultAsync();
                if (user is not null)
                    return user;
                else
                    throw new UserNotFound("Usuario nao encontrado por " + fieldName + ".");
            }
            catch (UserNotFound ex) {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<object> UpdateUser<T>(object id, object userComplet) {
            try {
                var filter = Builders<User>.Filter
                .Eq(r => r.Id, id);
                ReplaceOneResult result = await this._userCollection.ReplaceOneAsync(filter, userComplet as User);
                if (result.ModifiedCount > 0 || result.MatchedCount > 0)
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
        //  public async Task<object> removeValueFromArrayField<T>(object id, object fieldName, object idValueToRemove) {

        //     var filter = Builders<T>.Filter.Eq("_id", id);
        //     var update = Builders<T>.Update.Pull((FieldDefinition<T>)fieldName, idValueToRemove);
        //     var options = new FindOneAndUpdateOptions<T> { ReturnDocument = ReturnDocument.After };

        //     var result = await _userCollection.FindOneAndUpdateAsync(filter, update, options);

        //     return result;
        // }
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
    }
}