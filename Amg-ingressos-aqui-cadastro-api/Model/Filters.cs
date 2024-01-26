using Amg_ingressos_aqui_cadastro_api.Enum;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_cadastro_api.Model
{
    public class FiltersUser
    {
        public FiltersUser()
        {
            Email = string.Empty;
            Name = string.Empty;
            PhoneNumber = string.Empty;
        }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("type")]
        public TypeUser Type { get; set; }
    }
}