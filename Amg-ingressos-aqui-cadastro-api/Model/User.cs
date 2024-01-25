using Amg_ingressos_aqui_cadastro_api.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_cadastro_api.Model
{
    [BsonIgnoreExtraElements]
    public class User
    {
        public User()
        {
            Id = null;
            Name = null;
            DocumentId = null;
            Status = null;
            Type = null;
            Address = null;
            Contact = null;
            UserConfirmation = null;
            Password = null;
            IdAssociate = null;
        }

        public User(User user)
        {
            Id = user.Id;
            Name = user.Name;
            DocumentId = user.DocumentId;
            Status = user.Status;
            Type = user.Type;
            Address = user.Address;
            Contact = user.Contact;
            UserConfirmation = user.UserConfirmation;
            Password = user.Password;
        }

        public User(string? id, string? name, string? documentId, TypeStatusEnum? status, TypeUserEnum? type, Address? address,
                    Contact? contact, UserConfirmation? userConfirmation, string? password)
        {
            Id = id;
            Name = name;
            DocumentId = documentId;
            Status = status;
            Type = type;
            Address = address;
            Contact = contact;
            UserConfirmation = userConfirmation;
            Password = password;
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
        [BsonElement("Name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <sumary>
        /// Documento identificação
        /// </sumary>
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
        [BsonElement("Password")]
        [JsonPropertyName("password")]
        public string? Password { get; set; }

        /// <summary>
        /// Senha de acesso
        /// </summary>
        [BsonElement("idAssociate")]
        [JsonPropertyName("idAssociate")]
        public string? IdAssociate { get; set; }

        /// <summary>
        /// Senha de acesso
        /// </summary>
        [BsonElement("updatedAt")]
        [JsonPropertyName("updatedAt")]
        public DateTime? updatedAt { get; set; }

        /// <summary>
        /// Atualização do usuário
        /// </summary>
        [BsonElement("UpdateAt")]
        [JsonPropertyName("UpdateAt")]
        public DateTime? UpdateAt { get; set; }

        /// <summary>
        /// Atualização do usuário
        /// </summary>
        [JsonPropertyName("sex")]
        public string Sex { get; set; }

        /// <summary>
        /// Atualização do usuário
        /// </summary>
        [JsonPropertyName("birthDate")]
        public string BirthDate { get; set; }
    }
}
