using Amg_ingressos_aqui_cadastro_api.Model;
using System.Text.Json.Serialization;
using Amg_ingressos_aqui_cadastro_api.Enum;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_cadastro_api.Dtos
{
    [BsonIgnoreExtraElements]
    public class UserDto
    {
        /// <summary>
        /// Id do usuário
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <sumary>
        /// Documento identificação
        /// </sumary>
        [JsonPropertyName("documentId")]
        public string DocumentId { get; set; }

        /// <sumary>
        /// Status
        /// </sumary>
        [JsonPropertyName("status")]
        public TypeStatusEnum Status { get; set; }

        /// <summary>
        /// Tipo do usuário
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Endereço do usuário
        /// </summary>
        [JsonPropertyName("address")]
        public Address Address { get; set; }

        /// <summary>
        /// Contato do usuário
        /// </summary>
        [JsonPropertyName("contact")]
        public Contact Contact { get; set; }

        /// <summary>
        /// Confirmação do usuário
        /// </summary>
        [JsonPropertyName("userConfirmation")]
        public UserConfirmation UserConfirmation { get; set; }

        /// <summary>
        /// Senha de acesso
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; set; }

        /// <summary>
        /// Senha de acesso
        /// </summary>
        [JsonPropertyName("idAssociate")]
        public string IdAssociate { get; set; }

        /// <summary>
        /// Senha de acesso
        /// </summary>
        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

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

        // CONSTRUCTORS
        public UserDto()
        {
            Id = string.Empty;
            Name = string.Empty;
            DocumentId = string.Empty;
            Type = string.Empty;
            Address = new Address();
            Contact = new Contact();
            UserConfirmation = new UserConfirmation();
            Password = string.Empty;
            IdAssociate = string.Empty;
            Sex = string.Empty;
            BirthDate = string.Empty;
        }

        public User DtoToModel()
        {
            TypeUserEnum type = (TypeUserEnum)System.Enum.Parse(typeof(TypeUserEnum), Type, true);
            return new User()
            {
                Address = this.Address,
                BirthDate = this.BirthDate,
                Contact = this.Contact,
                DocumentId = this.DocumentId,
                Id = this.Id,
                IdAssociate = this.IdAssociate,
                Name = this.Name,
                Password = this.Password,
                Sex = this.Sex,
                Status = this.Status,
                Type = type,
                UpdatedAt = this.UpdatedAt,
                UserConfirmation = this.UserConfirmation
            };
        }
    }
}