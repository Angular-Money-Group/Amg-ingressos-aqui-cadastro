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
        private readonly MessageReturn _messageReturn;
        private readonly ILogger<EventColabService> _logger;

        public EventColabService(
            IEventColabRepository eventColabRepository,
            IUserService userService,
            ILogger<EventColabService> Logger)
        {
            _eventColabRepository = eventColabRepository;
            _userService = userService;
            _logger = Logger;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> CheckAllColabsOfEventAsync(string idEvent, List<string> idColabsOfProducer)
        {
            //fazer do postgres para o mongo
            try
            {
                List<string> idColabsOfEvent = await _eventColabRepository.FindAllColabsOfEvent(idEvent);
                List<User> colabsEvent = new List<User>();

                foreach (string idColab in idColabsOfProducer)
                {
                    User colab = _userService.GetByIdAsync(idColab).Result.ToObject<User>();
                    if (idColabsOfEvent.Contains(idColab))
                        colabsEvent.Add(colab);
                }
                _messageReturn.Data = colabsEvent;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(CheckAllColabsOfEventAsync), ex));
                throw;
            }
        }

        public async Task<MessageReturn> SaveAsync(EventCollaboratorDto eventCollaborator)
        {
            try
            {
                // controlar o fluxo para nao permitir que o colab entre aqui de primeira
                EventColab eventColab = eventCollaborator.MakeEventColabSave();
                var id = await _eventColabRepository.Save(eventColab);
                _messageReturn.Data = id;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(SaveAsync), ex));
                throw;
            }
        }

        public async Task<bool> DoesIdExists(string idEventColab)
        {
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
            try
            {
                id.ValidateIdMongo();

                if (!await DoesIdExists(id))
                    throw new RuleException("Id de eventXcolab não encontrada.");

                if (!await _eventColabRepository.Delete(id))
                    throw new RuleException("Relacao não deletada.");

                _messageReturn.Data = "ok";
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(DeleteAsync), ex));
                throw;
            }
        }
    }
}