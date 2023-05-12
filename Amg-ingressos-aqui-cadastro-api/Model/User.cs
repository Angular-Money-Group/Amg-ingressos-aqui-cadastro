using Amg_ingressos_aqui_cadastro_api.Enum;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Amg_ingressos_aqui_cadastro_api.Model {
    public class User 
    {

        /// <sumary>
        /// Nome usuário
        /// </sumary>
        public string name { get; set; }

        /// <sumary>
        /// Documento identificação
        /// </sumary>
        public string documentid { get; set; }

        /// <sumary>
        /// Estatus
        /// </sumary>
        public TypeStatusUserEnum Status { get; set; }

    }
}
