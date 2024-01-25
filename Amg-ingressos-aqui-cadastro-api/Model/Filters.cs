using Amg_ingressos_aqui_cadastro_api.Enum;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_cadastro_api.Model
{
    public class FiltersUser
    {
        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("phoneNumber")]
        public string? PhoneNumber { get; set; }

        [JsonProperty("type")]
        public TypeUserEnum? Type { get; set; }
    }
}
