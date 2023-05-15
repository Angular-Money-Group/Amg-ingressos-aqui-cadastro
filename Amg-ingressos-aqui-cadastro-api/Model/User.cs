using Amg_ingressos_aqui_cadastro_api.Enum;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Amg_ingressos_aqui_cadastro_api.Model {
    public class User 
    {
        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Documento de identificação
        /// </summary>
        public string Documentid { get; set; }

        /// <summary>
        /// Status 
        /// </summary>
        public TypeStatusUserEnum Status { get; set; }

        /// <summary>
        /// Endereço do usuário
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// Contato do usuário
        /// </summary>
        public Contact Contact { get; set; }

        /// <summary>
        /// Confirmação do usuário
        /// </summary>
        public UserConfirmation UserConfirmation { get; set; }

        /// <summary>
        /// Senha de acesso
        /// </summary>
        public string Password { get; set; }
    }



}
