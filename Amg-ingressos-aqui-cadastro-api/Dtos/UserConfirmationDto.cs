using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_cadastro_api.Dtos
{
    public class UserConfirmationDto
    {
        /// <sumary>
        /// codeConfirmation
        /// </sumary>
        [JsonPropertyName("codeConfirmation")]
        public string CodeConfirmation {get;set;}

        /// <sumary>
        /// userType
        /// </sumary>
        [JsonPropertyName("userType")]
        public string UserType {get;set;}
    }
}