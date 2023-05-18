using Amg_ingressos_aqui_cadastro_api.Enum;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Amg_ingressos_aqui_cadastro_api.Model {
    public class User {

        /// <summary>
        /// Nome do usuário
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        /// <summary>
        /// name
        /// </summary>
        public string Name { get; set; }
        /// <sumary>
        /// Documento identificação
        /// </sumary>
        public string DocumentId { get; set; }
        /// <sumary>
        /// Estatus
        /// </sumary>
        [Required]
        public TypeStatusUserEnum? Status { get; set; }
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