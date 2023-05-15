using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_cadastro_api.Infra;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class UserRepository<T> : IUserRepository
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserRepository(IDbConnection<User> dbConnection, string modelName) {
            this._userCollection = dbConnection.GetConnection(modelName);
        }

        public UserRepository(IDbConnection<User> dbConnection) {
            this._userCollection = dbConnection.GetConnection("User");
        }
        public async Task<object> Save<T>(object userComplet) {
            try {
                await this._userCollection.InsertOneAsync(userComplet as User);
                return (userComplet as User).Id;
            }
            catch (SaveUserException ex) {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        /* Task<object> FindById<T>(object id) {

        } */
        public async Task<object> UpdateUser<T>(object id, object userComplet) {
            try {
                var filter = Builders<User>.Filter
                .Eq(r => r.Id, id);
                await this._userCollection.ReplaceOneAsync(filter, userComplet as User);
                return "Usuário Atualizado.";
            }
            catch (UpdateUserException ex) {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        /* Task<object> removeValueFromArrayField<T>(object id, string fieldname, object IdValueToRemove) {

        }
        Task<object> Delete<T>(object id) {

        }
        Task<IEnumerable<object>> GetAllEvents<T>() {

        } */
    }
}