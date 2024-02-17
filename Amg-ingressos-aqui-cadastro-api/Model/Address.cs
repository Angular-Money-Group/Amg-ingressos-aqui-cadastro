using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Utils;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_cadastro_api.Model
{
    public class Address
    {
        public Address()
        {
            Cep = string.Empty;
            AddressDescription = string.Empty;
            Number = string.Empty;
            Neighborhood = string.Empty;
            Complement = string.Empty;
            ReferencePoint = string.Empty;
            City = string.Empty;
            State = string.Empty;
        }

        /// <summary>
        /// Cep da residencia 
        /// </summary>
        [BsonElement("Cep")]
        [JsonPropertyName("cep")]
        public string Cep { get; set; }

        /// <summary>
        /// Endereço da residencia 
        /// </summary>
        [BsonElement("AddressDescription")]
        [JsonPropertyName("addressDescription")]
        public string AddressDescription { get; set; }

        /// <summary>
        /// Número da residencia
        /// </summary>
        [BsonElement("Number")]
        [JsonPropertyName("number")]
        public string Number { get; set; }

        /// <summary> 
        /// Complemento 
        /// </summary>
        [BsonElement("Complement")]
        [JsonPropertyName("complement")]
        public string Complement { get; set; }

        /// <summary> 
        /// Ponto de referencia 
        /// </summary>
        [BsonElement("ReferencePoint")]
        [JsonPropertyName("referencePoint")]
        public string ReferencePoint { get; set; }

        /// <summary>
        /// Bairro de residencia 
        /// </summary>
        [BsonElement("Neighborhood")]
        [JsonPropertyName("neighborhood")]
        public string Neighborhood { get; set; }

        /// <summary> 
        /// Cidade de residencia 
        /// </summary>
        [BsonElement("City")]
        [JsonPropertyName("city")]
        public string City { get; set; }

        /// <summary>
        /// Estado de residencia 
        /// </summary>
        [BsonElement("State")]
        [JsonPropertyName("state")]
        public string State { get; set; }

        public void ValidateAdressFormat()
        {

            if (string.IsNullOrEmpty(this.Cep))
                throw new RuleException("Em Endereço, CEP é Obrigatório.");
            this.Cep = string.Join("", this.Cep.ToCharArray().Where(Char.IsDigit));
            if (this.Cep.Length != 8)
                throw new RuleException("Em Endereço, formato de CEP inválido.");

            if (string.IsNullOrEmpty(this.AddressDescription))
                throw new RuleException("Em Endereço, Logradouro é Obrigatório.");
            if (!this.AddressDescription.ValidateTextFormat())
                throw new RuleException("Em de Endereço, formato de Logradouro inválido.");

            if (string.IsNullOrEmpty(this.Number))
                throw new RuleException("Em Endereço, Número é Obrigatório.");
            if (!this.Number.ValidateSimpleTextFormat())
                throw new RuleException("Em de Endereço, formato de Número inválido.");

            if (string.IsNullOrEmpty(this.Neighborhood))
                throw new RuleException("Em Endereço, Bairro é Obrigatório.");
            if (!this.Neighborhood.ValidateTextFormat())
                throw new RuleException("Em Endereço, formato de Bairro inválido.");

            if (string.IsNullOrEmpty(this.City))
                throw new RuleException("Em endereço, Cidade é Obrigatório.");
            if (!this.City.ValidateTextFormat())
                throw new RuleException("Uma tentativa de cadastro pode ter vindo de fora. [User.Adress.City]");

            if (string.IsNullOrEmpty(this.State))
                throw new RuleException("Em endereço, Estado é Obrigatório.");
            if (!this.State.ValidateTextFormat())
                throw new RuleException("Uma tentativa de cadastro pode ter vindo de fora. [User.Adress.State]");
        }
    }
}
