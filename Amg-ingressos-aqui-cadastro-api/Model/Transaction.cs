using System.ComponentModel.DataAnnotations;
using Amg_ingressos_aqui_cadastro_api.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_cadastro_api.Model
{
    public class Transaction
    {
        /// <summary>
        /// Id mongo
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Id Usuario
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdPerson { get; set; }

        /// <summary>
        /// Taxa de Compra
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// Valor total sem desconto ou taxas
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Desconto
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// Url de Retorno Transacao
        /// </summary>
        public string ReturnUrl { get; set; }
        

    }
}