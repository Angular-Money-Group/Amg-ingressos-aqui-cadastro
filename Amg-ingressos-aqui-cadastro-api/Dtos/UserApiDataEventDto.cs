namespace Amg_ingressos_aqui_cadastro_api.Dtos
{
    public class UserApiDataEventDto
    {
        public UserApiDataEventDto()
        {
            IdEvent = string.Empty;
            IdUser = string.Empty;
        }

        public string IdEvent { get; set; }
        public string IdUser { get; set; }
    }
}