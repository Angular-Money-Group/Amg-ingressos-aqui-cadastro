namespace Amg_ingressos_aqui_cadastro_api.Consts
{
    public static class Settings
    {
        //Services
        public readonly static string EmailServiceApi = "http://api.ingressosaqui.com:3011/";
        public readonly static string UriEmailVerifyAccount = "v1/notification/verifyAccount";
        public readonly static string UriEmailConfirmedAccount = "v1/notification/emailConfirmed";
        public readonly static string UriEmailLoginCollaborator = "v1/notification/loginCollaboratorCredential";
        public readonly static string UriTicketSuport = "v1/notification/ticketSupport";

        public readonly static string EventServiceApi = "http://api.ingressosaqui.com:3002/";
        public readonly static string UriGetByIdEvent = "v1/events/";

        //configs login
        public readonly static string UrlLoginCollaborator = "https://qrcode.ingressosaqui.com/auth?idEvento=";

        //Notification configs
        public readonly static string Sender = "suporte@ingressosaqui.com";
        public readonly static string SubjectCredentialsEvent = "Credenciais de Acesso ao Evento";
        public readonly static string ToSupport  = "augustopires@angularmoneygroup.com.br";
        public readonly static string SubjectComfirmationAccount  = "Confirmação de Conta";
        public readonly static string SubjectComfirmateAccount  = "Conta confirmada.";
        
        

        //Encrypt / decrypt
        public readonly static string keyEncrypt  = "b14ca5898a4e4133bbce2ea2315a2023";
        
    }
}