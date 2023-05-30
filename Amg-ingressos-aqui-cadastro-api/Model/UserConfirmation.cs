using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace Amg_ingressos_aqui_cadastro_api.Model {
    public class UserConfirmation 
    {
        /// <summary>
        /// Confirmação de e-mail
        /// </summary>
        [Required]
        public string? EmailConfirmationCode { get; set; }
        [Required]
        public DateTime? EmailConfirmationExpirationDate { get; set; }

        /// <summary> 
        /// Confirmação do número 
        /// </summary>
        [Required]
        public bool? EmailVerified { get; set; } = false;
        public bool? PhoneVerified { get; set; } = false;
    }
}
