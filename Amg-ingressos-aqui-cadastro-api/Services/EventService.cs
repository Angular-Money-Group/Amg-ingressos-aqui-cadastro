using Amg_ingressos_aqui_cadastro_api.Consts;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class EventService : IEventService
    {
       private readonly MessageReturn _messageReturn;
        private readonly HttpClient _HttpClient;
        private readonly ILogger<EventService> _logger;

        public EventService(ILogger<EventService> logger)
        {
            _HttpClient = new HttpClient();
            _messageReturn = new MessageReturn();
            _logger = logger;
        }

        public async Task<MessageReturn> GetById(string idEvent)
        {
            try
            {
                var url = Settings.EventServiceApi;
                var uri = Settings.UriGetByIdEvent+idEvent;

                var response = await _HttpClient.GetAsync(url + uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    _messageReturn.Data = JsonConvert.DeserializeObject<Event>(jsonContent) ?? new Event();
                }
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(GetById), ex));
                throw;
            }
        } 
    }
}