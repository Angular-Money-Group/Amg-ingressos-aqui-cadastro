using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Utils;
using Amg_ingressos_aqui_cadastro_api.Consts;
using Amg_ingressos_aqui_cadastro_api.Exceptions;

namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class EventColabService : IEventColabService
    {
        private readonly IEventColabRepository _eventColabRepository;
        private readonly IUserService _userService;
        private MessageReturn? _messageReturn;
        private readonly ILogger<EventColabService> _logger;

        public EventColabService(IEventColabRepository eventColabRepository,
        IUserService userService,
        ILogger<EventColabService> Logger)
        {
            _eventColabRepository = eventColabRepository;
            _userService = userService;
            _logger = Logger;
        }

        public async Task<MessageReturn> CheckAllColabsOfEventAsync(string idEvent, List<string> idColabsOfProducer)
        {
            //fazer do postgres para o mongo
            /*this._messageReturn = new MessageReturn();
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
            }*/
            return _messageReturn;
        }

        public async Task<MessageReturn> SaveAsync(EventCollaboratorDto eventColabSaveDTO)
        {
            _messageReturn = new MessageReturn();
            try
            {
                // controlar o fluxo para nao permitir que o colab entre aqui de primeira
                EventColab eventColab = eventColabSaveDTO.MakeEventColabSave();

                var id = await _eventColabRepository.Save(eventColab);
                _messageReturn.Data = id;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(SaveAsync), ex));
                throw;
            }

            return _messageReturn;
        }

        public async Task<bool> DoesIdExists(string idEventColab)
        {
            _messageReturn = new MessageReturn();
            try
            {
                return await _eventColabRepository.DoesValueExistsOnField<EventColab>("Id", idEventColab);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(DoesIdExists), ex));
                throw;
            }
        }

        public async Task<MessageReturn> DeleteAsync(string id)
        {
            _messageReturn = new MessageReturn();
            try
            {
                id.ValidateIdMongo();

                if (!await DoesIdExists(id))
                    throw new RuleException("Id de eventXcolab não encontrada.");

                if (!await _eventColabRepository.Delete(id))
                    throw new RuleException("Relacao não deletada.");

                _messageReturn.Data = "ok";
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(DeleteAsync), ex));
                throw;
            }
            return _messageReturn;
        }
    }
}