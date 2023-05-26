namespace Amg_ingressos_aqui_cadastro_api.Model {
    public class Address
    {
        /// <summary>
        /// Cep da residencia 
        /// </summary>
        public string Cep { get; set; }

        /// <summary>
        /// Endereço da residencia 
        /// </summary>
        public string AddressDescription { get; set; }

        /// <summary>
        /// Número da residencia
        /// </summary>
        public string Number { get; set; }

        /// <summary> 
        /// Complemento 
        /// </summary>
        public string Complement { get; set; }

        /// <summary> 
        /// Ponto de referencia 
        /// </summary>
        public string ReferencePoint { get; set; }

        /// <summary>
        /// Bairro de residencia 
        /// </summary>
        public string Neighborhood { get; set; }

        /// <summary> 
        /// Cidade de residencia 
        /// </summary>
        public string City { get; set; }

       /// <summary>
       /// Estado de residencia 
       /// </summary>
        public string State { get; set; }
    }
}
