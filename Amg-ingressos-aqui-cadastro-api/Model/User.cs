using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Utils;
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
            Id = string.Empty;
            Name = string.Empty;
            DocumentId = string.Empty;
            Address = new Address();
            Contact = new Contact();
            UserConfirmation = new UserConfirmation();
            Password = string.Empty;
            IdAssociate = string.Empty;
            Sex = string.Empty;
            BirthDate = string.Empty;
        }

        /// <summary>
        /// Id do usuário
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; }
        /// <summary>
        /// name
        /// </summary>
        [BsonElement("Name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <sumary>
        /// Documento identificação
        /// </sumary>
        [BsonElement("DocumentId")]
        [JsonPropertyName("documentId")]
        public string DocumentId { get; set; }

        /// <sumary>
        /// Status
        /// </sumary>
        [BsonElement("Status")]
        [JsonPropertyName("status")]
        public TypeStatus Status { get; set; }

        /// <summary>
        /// Tipo do usuário
        /// </summary>
        [BsonElement("Type")]
        [JsonPropertyName("type")]
        public TypeUser Type { get; set; }

        /// <summary>
        /// Endereço do usuário
        /// </summary>
        [BsonElement("Address")]
        [JsonPropertyName("address")]
        public Address Address { get; set; }

        /// <summary>
        /// Contato do usuário
        /// </summary>
        [BsonElement("Contact")]
        [JsonPropertyName("contact")]
        public Contact Contact { get; set; }

        /// <summary>
        /// Confirmação do usuário
        /// </summary>
        [BsonElement("UserConfirmation")]
        [JsonPropertyName("userConfirmation")]
        public UserConfirmation UserConfirmation { get; set; }

        /// <summary>
        /// Senha de acesso
        /// </summary>
        [BsonElement("Password")]
        [JsonPropertyName("password")]
        public string Password { get; set; }

        /// <summary>
        /// Senha de acesso
        /// </summary>
        [BsonElement("idAssociate")]
        [JsonPropertyName("idAssociate")]
        public string IdAssociate { get; set; }

        /// <summary>
        /// Senha de acesso
        /// </summary>
        [BsonElement("updatedAt")]
        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Atualização do usuário
        /// </summary>
        [BsonElement("UpdateAt")]
        [JsonPropertyName("UpdateAt")]
        public DateTime UpdateAt { get; set; }

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



        public User makeUserSave()
        {
            if (Id is not null)
                Id = null;

            Status = TypeStatus.Active;

            ValidateBasicUserFormat();
            //TypeUserEnum type = (TypeUserEnum)System.Enum.Parse(typeof(TypeUserEnum), Type, true);
            switch (this.Type)
            {
                case TypeUser.Admin:
                    ValidateAdminFormat();
                    break;
                case TypeUser.Organizer:
                    ValidateProducerFormat();
                    break;
                case TypeUser.Customer:
                    ValidateCustomerFormat();
                    break;
            }

            return new User();
        }

        public User makeUserUpdate()
        {
            Id.ValidateIdMongo();

            ValidateBasicUserUpdateFormat();
            switch (this.Type)
            {
                case TypeUser.Admin:
                    ValidateAdminFormat();
                    break;
                case TypeUser.Organizer:
                    ValidateProducerFormat();
                    break;
                case TypeUser.Customer:
                    ValidateCustomerFormat();
                    break;
            }
            return new User();
        }

        public void ValidateBasicUserFormat()
        {

            ValidateNameFormat();
            validatePasswordFormat();
            validateConctact();
        }

        public void ValidateBasicUserUpdateFormat()
        {
            ValidateNameFormat();
            if (Password is not null)
            {
                validatePasswordFormat();
            }
        }
        public void ValidateAdminFormat()
        {
            //TypeUserEnum type = (TypeUserEnum)System.Enum.Parse(typeof(TypeUserEnum), Type, true);
            ValidateUserType(TypeUser.Admin, this.Type);
            ValidateDocumentIdFormat();
        }
        public void ValidateCustomerFormat()
        {
            //TypeUserEnum type = (TypeUserEnum)System.Enum.Parse(typeof(TypeUserEnum), Type, true);
            ValidateUserType(TypeUser.Customer, this.Type);
            ValidateCpfFormat();
        }
        public void ValidateProducerFormat()
        {
            //TypeUserEnum type = (TypeUserEnum)System.Enum.Parse(typeof(TypeUserEnum), Type, true);
            ValidateUserType(TypeUser.Organizer, this.Type);
            ValidateCnpjFormat();
        }
        
        public static void ValidateEmailFormat(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new RuleException("Email é Obrigatório.");
            if (!email.ValidateEmailFormat())
                throw new RuleException("Formato de email inválido.");
        }

        public static void ValidatePhoneNumberFormat(string? phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                throw new RuleException("Telefone de Contato é Obrigatório.");
            phoneNumber = string.Join("", phoneNumber.ToCharArray().Where(Char.IsDigit));
            if (string.IsNullOrEmpty(phoneNumber))
                throw new RuleException("Formato de Telefone de Contato inválido.");
        }

        public static void ValidateUserType(System.Enum typeExpected, System.Enum userType)
        {
            try
            {
                userType.ValidateObjectEnumType(typeExpected);
            }
            catch (RuleException)
            {
                throw new RuleException("Tipo de Usuario é obrigatório.");
            }
        }

        // PUBLIC FUNCTIONS
        public void ValidateNameFormat()
        {
            if (string.IsNullOrEmpty(Name))
                throw new RuleException("Nome é Obrigatório.");
            if (!Name.ValidateTextFormat())
                throw new RuleException("Formato de nome inválido.");
        }

        public void ValidateDocumentIdFormat()
        {
            if (string.IsNullOrEmpty(DocumentId))
                throw new RuleException("Documento de Identificação é Obrigatório.");
            DocumentId = string.Join("", DocumentId.ToCharArray().Where(Char.IsDigit));
            if (DocumentId.Length != 11 && DocumentId.Length != 13)
                throw new RuleException("Formato de Documento de Identificação inválido.");
        }

        public void ValidateCpfFormat()
        {
            DocumentId.ValidateCpfFormat();
        }

        public void ValidateCnpjFormat()
        {
            if (string.IsNullOrEmpty(DocumentId))
                throw new RuleException("Documento de CNPJ é Obrigatório.");
            DocumentId = string.Join("", DocumentId.ToCharArray().Where(Char.IsDigit));
            if (DocumentId.Length < 11 && DocumentId.Length > 14)
                throw new RuleException("Formato de Documento de CNPJ inválido.");
        }

        public void ValidateAdressFormat()
        {
            if (Address is not null)
            {

                if (string.IsNullOrEmpty(Address.Cep))
                    throw new RuleException("Em Endereço, CEP é Obrigatório.");
                Address.Cep = string.Join("", Address.Cep.ToCharArray().Where(Char.IsDigit));
                if (Address.Cep.Length != 8)
                    throw new RuleException("Em Endereço, formato de CEP inválido.");

                if (string.IsNullOrEmpty(Address.AddressDescription))
                    throw new RuleException("Em Endereço, Logradouro é Obrigatório.");
                if (!Address.AddressDescription.ValidateTextFormat())
                    throw new RuleException("Em de Endereço, formato de Logradouro inválido.");

                if (string.IsNullOrEmpty(Address.Number))
                    throw new RuleException("Em Endereço, Número é Obrigatório.");
                if (!Address.Number.ValidateSimpleTextFormat())
                    throw new RuleException("Em de Endereço, formato de Número inválido.");

                if (string.IsNullOrEmpty(Address.Neighborhood))
                    throw new RuleException("Em Endereço, Bairro é Obrigatório.");
                if (!Address.Neighborhood.ValidateTextFormat())
                    throw new RuleException("Em Endereço, formato de Bairro inválido.");

                if (string.IsNullOrEmpty(Address.City))
                    throw new RuleException("Em endereço, Cidade é Obrigatório.");
                if (!Address.City.ValidateTextFormat())
                    throw new RuleException("Uma tentativa de cadastro pode ter vindo de fora. [User.Adress.City]");

                if (string.IsNullOrEmpty(Address.State))
                    throw new RuleException("Em endereço, Estado é Obrigatório.");
                if (!Address.State.ValidateTextFormat())
                    throw new RuleException("Uma tentativa de cadastro pode ter vindo de fora. [User.Adress.State]");
            }
        }

        public void validateConctact()
        {
            if (Contact is null)
                throw new RuleException("Contato é Obrigatório.");
            ValidateEmailFormat(Contact.Email);
        }

        public void validateUserConfirmation()
        {
            if (UserConfirmation is null)
                throw new RuleException("UserConfirmation é Obrigatório.");
            if (string.IsNullOrEmpty(UserConfirmation.EmailConfirmationCode))
                throw new RuleException("Código de Confirmação de Email é Obrigatório.");
            if (UserConfirmation.EmailConfirmationExpirationDate == DateTime.MinValue)
                throw new RuleException("Data de Expiração de Código de Confirmação de Email é Obrigatório.");
            if (!UserConfirmation.EmailVerified)
                throw new RuleException("Status de Verificação de Email é Obrigatório.");
            if (!UserConfirmation.PhoneVerified)
                throw new RuleException("Status de Verificação de Telefone é Obrigatório.");
        }

        public void validatePasswordFormat()
        {
            if (string.IsNullOrEmpty(Password))
                throw new RuleException("Senha é Obrigatório.");
            if (Password.Length < 8 || Password.Length > 16)
                throw new RuleException("Formato de Senha inválido, mínimo de 8, máximo de 16 caracteres.");
            if (!Password.ValidateStrongPasswordFormat())
                throw new RuleException("Senha deve conter letra minúscula e maiúscula, número e caractere especial(!#$%&*+-?@_) apenas.");
        }
    }
}
