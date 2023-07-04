using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Utils;
using System;
using System.Text.RegularExpressions;

namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class EventColabService : IEventColabService
    {
        private IEventColabRepository _eventColabRepository;
        private IUserService _userService;
        private MessageReturn? _messageReturn;

        public EventColabService(IEventColabRepository eventColabRepository, IUserService userService)
        {
            this._eventColabRepository = eventColabRepository;
            this._userService = userService;
        }
        
        public async Task<MessageReturn> GetAllColabsOfEventAsync(string idEvent)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                var result = await _eventColabRepository.GetAllEventColabs<EventColab>();

                List<EventColabDTO> list = new List<EventColabDTO>();
                foreach (EventColab eventColab in result) {
                    list.Add(new EventColabDTO(eventColab));
                }
                _messageReturn.Data = list;
            }
            catch (GetAllEventColabException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _messageReturn;
        }
        
        public async Task<MessageReturn> SaveAsync(EventColabDTO eventColabSaveDTO) {
            this._messageReturn = new MessageReturn();
            try
            {
                // controlar o fluxo para nao permitir que o colab entre aqui de primeira
                EventColab eventColab = eventColabSaveDTO.makeEventColabSave();       
                
                var id = await _eventColabRepository.Save<EventColab>(eventColab);
                _messageReturn.Data = id;
            }
            catch (UserNotFound ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (EmptyFieldsException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (InvalidFormatException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (SaveEventColabException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }

        public async Task<bool> DoesIdExists(string idEventColab) {
            this._messageReturn = new MessageReturn();
            try {
                return await _eventColabRepository.DoesValueExistsOnField<EventColab>("Id", idEventColab);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MessageReturn> DeleteAsync(string id) {
            this._messageReturn = new MessageReturn();
            try
            {
                id.ValidateIdMongo();
                
                if (!await DoesIdExists(id))
                    throw new EventColabNotFound("Id de eventXcolab não encontrada.");

                _messageReturn.Data = await _eventColabRepository.Delete<EventColab>(id) as string;
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (EventColabNotFound ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (DeleteEventColabException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _messageReturn;
        }
    }
}