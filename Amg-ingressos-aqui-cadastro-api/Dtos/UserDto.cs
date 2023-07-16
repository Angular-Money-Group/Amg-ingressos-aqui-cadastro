
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Utils;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Dtos
{
    public class UserDTO : User
    {
        // CONSTRUCTORS
        public UserDTO()
        {
            this.Id = null;
            this.Name = null;
            this.DocumentId = null;
            this.Status = null;
            this.Type = null;
            this.Address = null;
            this.Contact = null;
            this.UserConfirmation = null;
            this.Password = null;
        }

        public UserDTO(System.Enum TEnum, UserDTO userDTO)
        {
            ValidateUserType(TEnum, userDTO.Type);
            this.Id = userDTO.Id;
            this.Name = userDTO.Name;
            this.DocumentId = userDTO.DocumentId;
            this.Status = userDTO.Status;
            this.Type = userDTO.Type;
            this.Address = userDTO.Address;
            this.Contact = userDTO.Contact;
            this.UserConfirmation = userDTO.UserConfirmation;
            this.Password = userDTO.Password;
        }

        public UserDTO(System.Enum TEnum, User user)
        : base(user)
        {
            ValidateUserType(TEnum, user.Type);
        }

        public UserDTO(User user)
        : base(user)
        {
        }

        // USER FACTORY FUNCTIONS
        public User makeUser()
        {
            return new User(this.Id, this.Name, this.DocumentId, this.Status, this.Type, this.Address,
                    this.Contact, this.UserConfirmation, this.Password);
        }

        public User makeUserSave()
        {
            if (this.Id is not null)
                this.Id = null;
            this.Status = TypeStatusEnum.Active;

            this.ValidateBasicUserFormat();
            switch (this.Type)
            {
                //case TypeUserEnum.Colab:
                //    this.ValidateColabFormat();
                //    break;
                case TypeUserEnum.Admin:
                    this.ValidateAdminFormat();
                    break;
                case TypeUserEnum.Organizer:
                    this.ValidateProducerFormat();
                    break;
                case TypeUserEnum.Customer:
                    this.ValidateCustomerFormat();
                    break;
            }
            return this.makeUser();
        }

        public User makeUserUpdate()
        {
            this.Id.ValidateIdMongo();
            //this.ValidateStatusFormat();

            this.ValidateBasicUserFormat();
            switch (this.Type)
            {
                case TypeUserEnum.Admin:
                    this.ValidateAdminFormat();
                    break;
                case TypeUserEnum.Organizer:
                    this.ValidateProducerFormat();
                    break;
                case TypeUserEnum.Customer:
                    this.ValidateCustomerFormat();
                    break;
            }
            return this.makeUser();
        }

        public void ValidateBasicUserFormat()
        {

            this.ValidateNameFormat();
            this.validatePasswordFormat();
            this.ValidateAdressFormat();
            this.validateConctact();
            this.validateUserConfirmation();
        }
        public void ValidateAdminFormat()
        {
            ValidateUserType(TypeUserEnum.Admin, Type);
            this.ValidateDocumentIdFormat();
        }
        public void ValidateCustomerFormat()
        {
            ValidateUserType(TypeUserEnum.Customer, this.Type);
            this.ValidateCpfFormat();
        }
        public void ValidateProducerFormat()
        {
            ValidateUserType(TypeUserEnum.Organizer, this.Type);
            this.ValidateCnpjFormat();
        }

        /*public void ValidateColabFormat()
        {
            if (this.Contact is null)
                throw new EmptyFieldsException("Contato é Obrigatório.");
            ValidateEmailFormat(this.Contact.Email);
            this.ValidateCpfFormat();
            this.Address = null;
            this.Contact.PhoneNumber = null;
            this.UserConfirmation = null;
            ValidateUserType(TypeUserEnum.Colab, this.Type);
        }*/

        // VALIDATES
        // STATIC FUNCTIONS
        public static void ValidateEmailFormat(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new EmptyFieldsException("Email é Obrigatório.");
            if (!email.ValidateEmailFormat())
                throw new InvalidFormatException("Formato de email inválido.");
        }

        public static void ValidatePhoneNumberFormat(string? phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                throw new EmptyFieldsException("Telefone de Contato é Obrigatório.");
            phoneNumber = string.Join("", phoneNumber.ToCharArray().Where(Char.IsDigit));
            if (string.IsNullOrEmpty(phoneNumber))
                throw new EmptyFieldsException("Formato de Telefone de Contato inválido.");
        }

        public static void ValidateUserType(System.Enum typeExpected, System.Enum userType)
        {
            try
            {
                userType.ValidateObjectEnumType(typeExpected);
            }
            catch (EmptyFieldsException)
            {
                throw new EmptyFieldsException("Tipo de Usuario é obrigatório.");
            }
            catch (InvalidUserTypeException)
            {
                throw new InvalidUserTypeException("Tipo de Usuário não corresponde ao esperado");
            }
        }

        // PUBLIC FUNCTIONS
        public void ValidateNameFormat()
        {
            if (string.IsNullOrEmpty(this.Name))
                throw new EmptyFieldsException("Nome é Obrigatório.");
            if (!this.Name.ValidateTextFormat())
                throw new InvalidFormatException("Formato de nome inválido.");
        }

        public void ValidateStatusFormat()
        {
            if (this.Status is null)
                throw new EmptyFieldsException("Status de Usuário é Obrigatório.");
        }

        public void ValidateDocumentIdFormat()
        {
            if (string.IsNullOrEmpty(this.DocumentId))
                throw new EmptyFieldsException("Documento de Identificação é Obrigatório.");
            this.DocumentId = string.Join("", this.DocumentId.ToCharArray().Where(Char.IsDigit));
            if (this.DocumentId.Length != 11 && this.DocumentId.Length != 13)
                throw new EmptyFieldsException("Formato de Documento de Identificação inválido.");
        }

        public void ValidateCpfFormat()
        {
            this.DocumentId.ValidateCpfFormat();
        }

        public void ValidateCnpjFormat()
        {
            if (string.IsNullOrEmpty(this.DocumentId))
                throw new EmptyFieldsException("Documento de CNPJ é Obrigatório.");
            this.DocumentId = string.Join("", this.DocumentId.ToCharArray().Where(Char.IsDigit));
            if (this.DocumentId.Length < 11 && this.DocumentId.Length > 14)
                throw new EmptyFieldsException("Formato de Documento de CNPJ inválido.");
        }

        public void ValidateAdressFormat()
        {
            if (this.Address is not null)
            {
                //throw new EmptyFieldsException("Endereço é Obrigatório.");

                if (string.IsNullOrEmpty(this.Address.Cep))
                    throw new EmptyFieldsException("Em Endereço, CEP é Obrigatório.");
                this.Address.Cep = string.Join("", this.Address.Cep.ToCharArray().Where(Char.IsDigit));
                if (this.Address.Cep.Length != 8)
                    throw new InvalidFormatException("Em Endereço, formato de CEP inválido.");

                if (string.IsNullOrEmpty(this.Address.AddressDescription))
                    throw new EmptyFieldsException("Em Endereço, Logradouro é Obrigatório.");
                if (!this.Address.AddressDescription.ValidateTextFormat())
                    throw new InvalidFormatException("Em de Endereço, formato de Logradouro inválido.");

                if (string.IsNullOrEmpty(this.Address.Number))
                    throw new EmptyFieldsException("Em Endereço, Número é Obrigatório.");
                if (!this.Address.Number.ValidateSimpleTextFormat())
                    throw new InvalidFormatException("Em de Endereço, formato de Número inválido.");

                if (string.IsNullOrEmpty(this.Address.Neighborhood))
                    throw new EmptyFieldsException("Em Endereço, Bairro é Obrigatório.");
                if (!this.Address.Neighborhood.ValidateTextFormat())
                    throw new InvalidFormatException("Em Endereço, formato de Bairro inválido.");

                if (string.IsNullOrEmpty(this.Address.Complement))
                    throw new EmptyFieldsException("Em Endereço, Complemento é Obrigatório.");
                if (!this.Address.Complement.ValidateTextFormat())
                    throw new InvalidFormatException("Em Endereço, formato de Complemento inválido.");

                // Aceita Ponto de Referência vazio, já que não está na aplicação.
                if (!string.IsNullOrEmpty(this.Address.ReferencePoint) && !(this.Address.ReferencePoint.ValidateTextFormat()))
                    throw new InvalidFormatException("Em Endereço Formato de Ponto de Referência inválido.");

                if (string.IsNullOrEmpty(this.Address.City))
                    throw new EmptyFieldsException("Em endereço, Cidade é Obrigatório.");
                if (!this.Address.City.ValidateTextFormat())
                    throw new InvalidFormatException("Uma tentativa de cadastro pode ter vindo de fora. [User.Adress.City]");

                if (string.IsNullOrEmpty(this.Address.State))
                    throw new EmptyFieldsException("Em endereço, Estado é Obrigatório.");
                if (!this.Address.State.ValidateTextFormat())
                    throw new InvalidFormatException("Uma tentativa de cadastro pode ter vindo de fora. [User.Adress.State]");
            }
        }

        public void validateConctact()
        {
            if (this.Contact is null)
                throw new EmptyFieldsException("Contato é Obrigatório.");
            ValidateEmailFormat(this.Contact.Email);
            ValidatePhoneNumberFormat(this.Contact.PhoneNumber);
        }

        public void validateUserConfirmation()
        {
            if (this.UserConfirmation is null)
                throw new EmptyFieldsException("UserConfirmation é Obrigatório.");
            if (string.IsNullOrEmpty(this.UserConfirmation.EmailConfirmationCode))
                throw new EmptyFieldsException("Código de Confirmação de Email é Obrigatório.");
            if (!this.UserConfirmation.EmailConfirmationExpirationDate.HasValue)
                throw new EmptyFieldsException("Data de Expiração de Código de Confirmação de Email é Obrigatório.");
            if (!this.UserConfirmation.EmailVerified.HasValue)
                throw new EmptyFieldsException("Status de Verificação de Email é Obrigatório.");
            if (!this.UserConfirmation.PhoneVerified.HasValue)
                throw new EmptyFieldsException("Status de Verificação de Telefone é Obrigatório.");
        }

        public void validatePasswordFormat()
        {
            if (string.IsNullOrEmpty(this.Password))
                throw new EmptyFieldsException("Senha é Obrigatório.");
            if (this.Password.Length < 8 || this.Password.Length > 16)
                throw new InvalidFormatException("Formato de Senha inválido, mínimo de 8, máximo de 16 caracteres.");
            if (!this.Password.ValidateStrongPasswordFormat())
                throw new InvalidFormatException("Senha deve conter letra minúscula e maiúscula, número e caractere especial(!#$%&*+-?@_) apenas.");
        }
    }
}