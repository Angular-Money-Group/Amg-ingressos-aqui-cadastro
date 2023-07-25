using Amg_ingressos_aqui_cadastro_api.Enum;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_cadastro_api.Model {
    public class User 
    {
        public User() {
            this.Id = null;
            this.Name = null;
            this.DocumentId = null;
            this.Status = null;
            this.Type = null;
            this.Address = null;
            this.Contact = null;
            this.UserConfirmation = null;
            this.Password = null;
            this.IdAssociate = null;
        }
        
        public User(User user) {
            this.Id = user.Id;
            this.Name = user.Name;
            this.DocumentId = user.DocumentId;
            this.Status = user.Status;
            this.Type = user.Type;
            this.Address = user.Address;
            this.Contact = user.Contact;
            this.UserConfirmation = user.UserConfirmation;
            this.Password = user.Password;
        }
        
        public User(string? id, string? name, string? documentId, TypeStatusEnum? status, TypeUserEnum? type, Address? address,
                    Contact? contact, UserConfirmation? userConfirmation, string? password) {
            this.Id = id;
            this.Name = name;
            this.DocumentId = documentId;
            this.Status = status;
            this.Type = type;
            this.Address = address;
            this.Contact = contact;
            this.UserConfirmation = userConfirmation;
            this.Password = password;
        }

        /// <summary>
        /// Id do usuário
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string? Id { get; set; }
        /// <summary>
        /// name
        /// </summary>
        [Required]
        [BsonElement("Name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <sumary>
        /// Documento identificação
        /// </sumary>
        [Required]
        [BsonElement("DocumentId")]
        [JsonPropertyName("documentId")]
        public string? DocumentId { get; set; }

        /// <sumary>
        /// Status
        /// </sumary>
        [BsonElement("Status")]
        [JsonPropertyName("status")]
        public TypeStatusEnum? Status { get; set; }

        /// <summary>
        /// Tipo do usuário
        /// </summary>
        [Required]
        [BsonElement("Type")]
        [JsonPropertyName("type")]
        public TypeUserEnum? Type { get; set; }

        /// <summary>
        /// Endereço do usuário
        /// </summary>
        [BsonElement("Address")]
        [JsonPropertyName("address")]
        public Address? Address { get; set; }

        /// <summary>
        /// Contato do usuário
        /// </summary>
        [Required]
        [BsonElement("Contact")]
        [JsonPropertyName("contact")]
        public Contact? Contact { get; set; }

        /// <summary>
        /// Confirmação do usuário
        /// </summary>
        [BsonElement("UserConfirmation")]
        [JsonPropertyName("userConfirmation")]
        public UserConfirmation? UserConfirmation { get; set; }

        /// <summary>
        /// Senha de acesso
        /// </summary>
        [Required]
        [BsonElement("Password")]
        [JsonPropertyName("password")]
        public string? Password { get; set; }

        /// <summary>
        /// Senha de acesso
        /// </summary>
        [BsonElement("idAssociate")]
        [JsonPropertyName("idAssociate")]
        public string? IdAssociate { get; set; }
    }
}
