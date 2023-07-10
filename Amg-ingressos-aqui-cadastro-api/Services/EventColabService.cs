using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Utils;
using System;
using System.Text.RegularExpressions;
using Amg_ingressos_aqui_cadastro_api.Model.Querys;

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
        
        public async Task<MessageReturn> CheckAllColabsOfEventAsync(string idEvent, List<string> idColabsOfProducer)
        {
            //fazer do postgres para o mongo
            this._messageReturn = new MessageReturn();
            try
            {
                var result = await _eventColabRepository.FindAllColabsOfEvent<EventColab>(idEvent);

                List<string> idColabsOfEvent = await _eventColabRepository.FindAllColabsOfEvent<EventColab>(idEvent);
                List<GetColabsEvent> colabsEvent = new List<GetColabsEvent>();
                
                foreach (string idColab in idColabsOfProducer) {
                    UserDTO colab = (await _userService.FindByIdAsync(idColab)).Data as UserDTO;
                    if(idColabsOfEvent.Contains(idColab))
                        colabsEvent.Add(new GetColabsEvent(colab, true));
                    else
                        colabsEvent.Add(new GetColabsEvent(colab, false));
                }
                _messageReturn.Data = colabsEvent;
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

        public async Task<MessageReturn> FindEventColabAsync(string idEvent, string idColab) {
            this._messageReturn = new MessageReturn();
            try {
                EventColabDTO.ValidateIdEventFormat(idEvent);
                EventColabDTO.ValidateIdColabFormat(idColab);

                EventColab eventColab = await _eventColabRepository.FindEventColab<EventColab>(idEvent, idColab);
                _messageReturn.Data = eventColab;
            }
            catch (IdMongoException ex) {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (EventColabNotFound ex) {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> IfEventPasswordMatchReturnUser(string idEvent, string typeUser, string email, string password)
        {
            try
            {
                this._messageReturn = await _userService.GetAsync(email, typeUser);
                if (!this._messageReturn.hasRunnedSuccessfully())
                    throw new InvalidLoginCredentials("Email e/ou senha incorretos");
                foreach (UserDTO user in (this._messageReturn.Data as List<UserDTO>)) {
                    if (user.Password.Equals(password)){
                        this._messageReturn = await FindEventColabAsync(idEvent, user.Id);
                        if (this._messageReturn.hasRunnedSuccessfully())
                        {
                            this._messageReturn.Data = user;
                            return this._messageReturn;
                        }
                    }
                }
                throw new InvalidLoginCredentials("Email e/ou senha incorretos");
            }
            catch (InvalidLoginCredentials ex)
            {
                this._messageReturn.Data = null;
                this._messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return this._messageReturn;
        }
    }
}