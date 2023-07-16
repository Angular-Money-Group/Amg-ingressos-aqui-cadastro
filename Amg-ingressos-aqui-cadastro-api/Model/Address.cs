using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_cadastro_api.Model {
    public class Address
    {
        /// <summary>
        /// Cep da residencia 
        /// </summary>
        [BsonElement("Cep")]
        [JsonPropertyName("cep")]
        public string? Cep { get; set; }

        /// <summary>
        /// Endereço da residencia 
        /// </summary>
        [BsonElement("AddressDescription")]
        [JsonPropertyName("addressDescription")]
        public string? AddressDescription { get; set; }

        /// <summary>
        /// Número da residencia
        /// </summary>
        [BsonElement("Number")]
        [JsonPropertyName("number")]
        public string? Number { get; set; }

        /// <summary> 
        /// Complemento 
        /// </summary>
        [BsonElement("Complement")]
        [JsonPropertyName("complement")]
        public string? Complement { get; set; }

        /// <summary> 
        /// Ponto de referencia 
        /// </summary>
        [BsonElement("ReferencePoint")]
        [JsonPropertyName("referencePoint")]
        public string? ReferencePoint { get; set; }

        /// <summary>
        /// Bairro de residencia 
        /// </summary>
        [BsonElement("Neighborhood")]
        [JsonPropertyName("neighborhood")]
        public string? Neighborhood { get; set; }

        /// <summary> 
        /// Cidade de residencia 
        /// </summary>
        [BsonElement("City")]
        [JsonPropertyName("city")]
        public string? City { get; set; }

       /// <summary>
       /// Estado de residencia 
       /// </summary>
        [BsonElement("State")]
        [JsonPropertyName("state")]
        public string? State { get; set; }
    }
}
