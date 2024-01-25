using Amg_ingressos_aqui_cadastro_api.Exceptions;
using System.Text.RegularExpressions;

namespace Amg_ingressos_aqui_cadastro_api.Utils
{
    public static class ExtensionMethods
    {
        public static void ValidateIdMongo(this string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new IdMongoException("Id é Obrigatório.");
            else if (id.Length < 24)
                throw new IdMongoException("Id é obrigatório e está menor que 24 digitos.");
        }
        
        public static void ValidateIdUserFormat(this string idUser) {
            try {
                idUser.ValidateIdMongo();
            } catch (IdMongoException ex) {
                throw new RuleException("Em IdUser: " + ex.Message);
            }
        }
        
        public static void ValidateDocumentIdFormat(this string documentId) {
            if (string.IsNullOrEmpty(documentId))
                throw new RuleException("Documento de Identificação é Obrigatório.");
            var DocumentIdLength = documentId.Length;
            if (!((DocumentIdLength == 11) || (DocumentIdLength == 13)))
                throw new RuleException("Formato de CPF/CNPJ inválido.");
        }

        public static bool ValidateTextFormat(this string str ) {
            return Regex.IsMatch(str, @"^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$");
        }

        public static bool ValidateFullNameFormat(this string str ) {
            return Regex.IsMatch(str, @"^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+[ ]{1}[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$");
        }

        public static bool ValidateEmailFormat(this string email ) {
            return Regex.IsMatch(email, @"^[A-Za-z0-9+_.-]+[@]{1}[A-Za-z0-9-]+[.]{1}[A-Za-z.]+$");
        }

        public static bool ValidateSimpleTextFormat(this string str ) {
            return Regex.IsMatch(str, @"^[a-zA-Z0-9 ,.-]+$");
        }

        public static bool ValidateStrongPasswordFormat(this string passwd ) {
            return Regex.IsMatch(passwd, @"^[a-zA-Z0-9!#$%&*+-?@_]+$") &&
            Regex.IsMatch(passwd, @"[a-z]+") &&
            Regex.IsMatch(passwd, @"[A-Z]+") &&
            Regex.IsMatch(passwd, @"[0-9]+") &&
            Regex.IsMatch(passwd, @"[!#$%&*+-?@_]+");
        }

        public static bool ValidateCompanyNameFormat(this string str ) {
            return Regex.IsMatch(str, @"^[a-zA-Z ,&$@.'-]+$");
        }
        
        public static bool ValidateNumbersWithHyphen(this string str ) {
            return Regex.IsMatch(str, @"^[0-9 -]+$");
        }
        
        public static bool ValidateNumbersAndLetters(this string str ) {
            return Regex.IsMatch(str, @"^[0-9a-zA-Z]+$");
        }
        
        public static bool ValidateOnlyNumbers(this string str ) {
            return Regex.IsMatch(str, @"^[0-9]+$");
        }
        
        public static void ValidateCpfFormat(this string cpf) {
            if(string.IsNullOrEmpty(cpf))
                throw new RuleException("Documento de CPF é Obrigatório.");
            cpf = string.Join("", cpf.ToCharArray().Where(Char.IsDigit));
            if(cpf.Length != 11)
                throw new RuleException("Formato de Documento de CPF inválido.");
        }
        
        public static bool IsNullOrEmpty(this object obj)
        {
            if (obj == null)
            {
                return true;
            }

            if (obj is string str && string.IsNullOrEmpty(str))
            {
                return true;
            }

            return false;
        }

        public static void ValidateObjectEnumType(this System.Enum TEnum, System.Enum classObjectEnum) {
            if (classObjectEnum.IsNullOrEmpty())
                throw new RuleException();
            if(!classObjectEnum.Equals(TEnum))
                throw new RuleException();
        }
    }
}