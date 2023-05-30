using Amg_ingressos_aqui_cadastro_api.Enum;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using System.Text.RegularExpressions;

namespace Amg_ingressos_aqui_cadastro_api.Model {
    public class User 
    {
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
        public StatusUserEnum? Status { get; set; }

        /// <summary>
        /// Endereço do usuário
        /// </summary>
        [Required]
        public TypeUserEnum? Type { get; set; }

        /// <summary>
        /// Endereço do usuário
        /// </summary>
        [Required]
        public Address? Address { get; set; }

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

        // STATIC FUNCTIONS
        public static void ValidateEmailFormat(string email) {
            if (string.IsNullOrEmpty(email))
                throw new UserEmptyFieldsException("Email é Obrigatório.");
            if (!Regex.IsMatch(email, @"^[A-Za-z0-9+_.-]+[@]{1}[A-Za-z0-9-]+[.]{1}[A-Za-z.]+$"))
                throw new InvalidFormatException("Formato de email inválido.");
        }

        // PUBLIC FUNCTIONS
        public void ValidateNameFormat() {
            if (string.IsNullOrEmpty(this.Name))
                throw new UserEmptyFieldsException("Nome é Obrigatório.");
            if (!Regex.IsMatch(this.Name, @"^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$"))
                throw new InvalidFormatException("Formato de nome inválido.");
        }

        public void ValidateDocumentIdFormat() {
            if (string.IsNullOrEmpty(this.DocumentId))
                throw new UserEmptyFieldsException("Documento de Identificação é Obrigatório.");
            this.DocumentId = string.Join("", this.DocumentId.ToCharArray().Where(Char.IsDigit));
            var DocumentIdLength = this.DocumentId.Length;
            if (!((DocumentIdLength == 11) || (DocumentIdLength == 13))) {
                throw new InvalidFormatException("Formato de CPF/CNPJ inválido.");
            }
        }

        // aqui eu preciso validar se o numero eh de 0 a 2?
        public void ValidateStatusUserEnumFormat() {
            if (this.Status is null)
                throw new UserEmptyFieldsException("Status de Usuário é Obrigatório.");
        }

        // aqui eu preciso validar se o numero eh de 0 a 2?
        public void ValidateTypeUserEnumFormat() {
            if (this.Type is null)
                throw new UserEmptyFieldsException("Tipo de Usuário é Obrigatório.");
            if ((this.Type == TypeUserEnum.Admin || this.Type == TypeUserEnum.Customer) && this.DocumentId.Length != 11)
                throw new InvalidFormatException("Tipo de Usuário não corresponde com Documento de Identificação.");
            if (this.Type == TypeUserEnum.Producer && this.DocumentId.Length != 13)
                throw new InvalidFormatException("Tipo de Usuário não corresponde com Documento de Identificação.");
        }

        public void ValidateAdressFormat() {
            if (this.Address is null)
                throw new UserEmptyFieldsException("Endereço é Obrigatório.");

            if (string.IsNullOrEmpty(this.Address.Cep))
                throw new UserEmptyFieldsException("CEP é Obrigatório.");
            this.Address.Cep = string.Join("", this.Address.Cep.ToCharArray().Where(Char.IsDigit));
            if(this.Address.Cep.Length != 8){
                throw new InvalidFormatException("Complemento de Endereço, formato de CEP inválido.");
            }

            if (string.IsNullOrEmpty(this.Address.AddressDescription))
                throw new UserEmptyFieldsException("Logradouro do Endereço é Obrigatório.");
            if (!Regex.IsMatch(this.Address.AddressDescription, @"^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$")) {
                throw new InvalidFormatException("Complemento de Endereço, formato de Logradouro inválido.");
            }

            if (string.IsNullOrEmpty(this.Address.Number))
                throw new UserEmptyFieldsException("Número Endereço é Obrigatório.");
            if (!Regex.IsMatch(this.Address.Number, @"^[a-zA-Z0-9 ,.-]+$")) {
                throw new InvalidFormatException("Complemento de Endereço, formato de Número inválido.");
            }

                
            if (string.IsNullOrEmpty(this.Address.Neighborhood))
                throw new UserEmptyFieldsException("Vizinhança é Obrigatório.");
            if (!Regex.IsMatch(this.Address.Neighborhood, @"^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$")) {
                throw new InvalidFormatException("Complemento de Endereço, formato de Bairro inválido.");
            }

            if (string.IsNullOrEmpty(this.Address.Complement))
                throw new UserEmptyFieldsException("Complemento é Obrigatório.");
            if (!Regex.IsMatch(this.Address.Complement, @"^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$")) {
                throw new InvalidFormatException("Formato de Complemento de Endereço inválido.");
            }

            if (string.IsNullOrEmpty(this.Address.ReferencePoint))
                throw new UserEmptyFieldsException("Ponto de referência é Obrigatório.");
            if (!Regex.IsMatch(this.Address.ReferencePoint, @"^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$")) {
                throw new InvalidFormatException("Formato de Ponto de Referência de Endereço inválido.");
            }

            if (string.IsNullOrEmpty(this.Address.City))
                throw new UserEmptyFieldsException("Em endereço, Cidade é Obrigatório.");

            if (string.IsNullOrEmpty(this.Address.State))
                throw new UserEmptyFieldsException("Em endereço, Estado é Obrigatório.");
        }

        public void validateConctact() {
            if (this.Contact is null)
                throw new UserEmptyFieldsException("Contato é Obrigatório.");
            ValidateEmailFormat(this.Contact.Email);
            this.Contact.PhoneNumber = string.Join("", this.Contact.PhoneNumber.ToCharArray().Where(Char.IsDigit));
            if(string.IsNullOrEmpty(this.Contact.PhoneNumber))
                throw new UserEmptyFieldsException("Telefone de Contato é Obrigatório.");
        }

        public void validateUserConfirmation () {
            if (this.UserConfirmation is null)
                throw new UserEmptyFieldsException("UserConfirmation é Obrigatório.");
            if (string.IsNullOrEmpty(this.UserConfirmation.EmailConfirmationCode))
                throw new UserEmptyFieldsException("Código de Confirmação de Email é Obrigatório.");
            if (!this.UserConfirmation.EmailConfirmationExpirationDate.HasValue)
                throw new UserEmptyFieldsException("Data de Expiração de Código de Confirmação de Email é Obrigatório.");
            if (!this.UserConfirmation.EmailVerified.HasValue)
                throw new UserEmptyFieldsException("Status de Verificação de Email é Obrigatório.");
            if (!this.UserConfirmation.PhoneVerified.HasValue)
                throw new UserEmptyFieldsException("Status de Verificação de Telefone é Obrigatório.");
        }
        
        public void validatePasswordFormat() {
            if (string.IsNullOrEmpty(this.Password))
                throw new UserEmptyFieldsException("Senha é Obrigatório.");
            if (this.Password.Length < 8 || this.Password.Length > 16)
                throw new InvalidFormatException("Formato de Senha inválido, mínimo de 8, máximo de 16 caracteres.");
            if (!Regex.IsMatch(this.Password, @"^[a-zA-Z0-9!#$%&*+-?@_]+$"))
                throw new InvalidFormatException("Formato de Senha inválido, caracteres impróprios.");
            if (!Regex.IsMatch(this.Password, @"[a-z]+"))
                throw new InvalidFormatException("Formato de Senha inválido, deve conter letra minúscula.");
            if (!Regex.IsMatch(this.Password, @"[A-Z]+"))
                throw new InvalidFormatException("Formato de Senha inválido, deve conter letra maiúscula.");
            if (!Regex.IsMatch(this.Password, @"[0-9]+"))
                throw new InvalidFormatException("Formato de Senha inválido, deve conter número.");
            if (!Regex.IsMatch(this.Password, @"[!#$%&*+-?@_]+"))
                throw new InvalidFormatException("Formato de Senha inválido, deve conter caractere especial.");
        }
    }
}
