using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_cadastro_api.Model {
    public class Address
    {
        /// <summary>
        /// Cep da residencia 
        /// </summary>
        [BsonElement("Cep")]
        [JsonPropertyName("Cep")]
        public string Cep { get; set; }

        /// <summary>
        /// Endereço da residencia 
        /// </summary>
        [BsonElement("AddressDescription")]
        [JsonPropertyName("AddressDescription")]
        public string AddressDescription { get; set; }

        /// <summary>
        /// Número da residencia
        /// </summary>
        [BsonElement("Number")]
        [JsonPropertyName("Number")]
        public string Number { get; set; }

        /// <summary> 
        /// Complemento 
        /// </summary>
        [BsonElement("Complement")]
        [JsonPropertyName("Complement")]
        public string Complement { get; set; }

        /// <summary> 
        /// Ponto de referencia 
        /// </summary>
        [BsonElement("ReferencePoint")]
        [JsonPropertyName("ReferencePoint")]
        public string? ReferencePoint { get; set; }

        /// <summary>
        /// Bairro de residencia 
        /// </summary>
        [BsonElement("Neighborhood")]
        [JsonPropertyName("Neighborhood")]
        public string Neighborhood { get; set; }

        /// <summary> 
        /// Cidade de residencia 
        /// </summary>
        [BsonElement("City")]
        [JsonPropertyName("City")]
        public string City { get; set; }

       /// <summary>
       /// Estado de residencia 
       /// </summary>
        [BsonElement("State")]
        [JsonPropertyName("State")]
        public string State { get; set; }
    }
}
