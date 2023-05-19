using Microsoft.VisualBasic;

namespace Amg_ingressos_aqui_cadastro_api.Model {
    public class UserConfirmation 
    {
        /// <summary>
        /// Confirmação de e-mail
        /// </summary>        
        public bool EmailConfirmationCode { get; set; }
        public DateTime EmailConfirmationExpirationDate { get; set; }

        /// <summary> 
        /// Confirmação do número 
        /// </summary>
        public bool PhoneVerified { get; set; } = false;
    }
}
