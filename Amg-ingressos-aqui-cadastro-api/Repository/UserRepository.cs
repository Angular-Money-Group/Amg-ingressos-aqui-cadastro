using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Infra;
using MongoDB.Driver;
using Amg_ingressos_aqui_cadastro_api.Enum;
using System;
using MongoDB.Bson;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class UserRepository<T> : IUserRepository
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserRepository(IDbConnection<User> dbConnection)
        {
            this._userCollection = dbConnection.GetConnection("user");
        }

        public async Task<object> Save<T>(User userComplet)
        {
            try
            {
                await this._userCollection.InsertOneAsync(userComplet);

                if (userComplet.Id is null)
                    throw new SaveUserException("Erro ao salvar usuario");

                return userComplet.Id;
            }
            catch (SaveUserException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DoesValueExistsOnField<T>(string fieldName, object value)
        {
            try
            {
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

        public async Task<User> FindByField<T>(string fieldName, object value)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(fieldName, value);
                var user = await _userCollection.Find(filter).FirstOrDefaultAsync();
                if (user is not null)
                    return user;
                else
                    throw new UserNotFound("Usuário não encontrado por " + fieldName + ".");
            }
            catch (UserNotFound ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> UpdateUser<T>(object id, User userModel)
        {
            try
            {
                var update = Builders<User>.Update
                    .Set(userMongo => userMongo.Name, userModel.Name)
                    .Set(userMongo => userMongo.DocumentId, userModel.DocumentId)
                    .Set(userMongo => userMongo.Address, userModel.Address)
                    .Set(userMongo => userMongo.Contact, userModel.Contact)
                    .Set(userMongo => userMongo.UserConfirmation, userModel.UserConfirmation)
                    .Set(userMongo => userMongo.Password, userModel.Password);

                var filter = Builders<User>.Filter.Eq(userMongo => userMongo.Id, userModel.Id);

                UpdateResult updateResult = await _userCollection.UpdateOneAsync(filter, update);
                if (updateResult.ModifiedCount > 0)
                {
                    // The data was successfully updated
                    return updateResult;
                }
                else
                {
                    throw new UpdateUserException("Erro ao atualizar usuario.");
                }
            }
            catch (UpdateUserException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> Delete<T>(object id)
        {
            try
            {
                var user = await _userCollection
                    .Find(Builders<User>.Filter.Eq(userMongo => userMongo.Id, id))
                    .FirstOrDefaultAsync();

                if (user != null)
                {
                    if (user.Status == StatusUserEnum.Inactive)
                    {
                        var result = await _userCollection.UpdateOneAsync(
                            Builders<User>.Filter.Eq(userMongo => userMongo.Id, id),
                            Builders<User>.Update.Set(
                                userMongo => userMongo.Status,
                                StatusUserEnum.Active
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
                                StatusUserEnum.Inactive
                            )
                        );
                        return "Usuário Deletado.";
                    }
                }
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

        public async Task<List<User>> Get<T>(FiltersUser? filterOptions)
        {
            try
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
                    throw new GetAllUserException("Usuarios não encontrados");
                }

                return pResults;
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

        public async Task<object> UpdatePasswordUser<T>(string id, string password)
        {
            try
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
                    throw new UpdateUserException("Nova senha não pode ser igual a antiga.");
                }
                else
                {
                    throw new UpdateUserException("Erro ao atualizar usuario.");
                }
            }
            catch (UpdateUserException ex)
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
