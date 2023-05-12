namespace Amg_ingressos_aqui_cadastro_api.Model {
    public class Address
    {
        /// <sumary>
        /// Cep residencia 
        /// </sumary>
        public string cep { get; set; }

        /// <sumary>
        /// Endereço residencia 
        /// </sumary>
        public string address { get; set; }

        /// <sumary>
        /// Número da residencia
        /// </sumary>
        public string houserNumber { get; set; }

        /// <sumary> 
        /// Complemento 
        /// </sumary>
        public string complement { get; set; }

        /// <sumary>
        /// Bairro de residencia 
        /// </sumary>
        public string neighborhood { get; set; }

        ///<sumary> 
        /// Cidade de residencia 
        /// </sumary>
        public string city { get; set; }

        /// <sumary>
        /// Estado de residencia
        /// </sumary>
        public string state { get; set; }
    }
}
