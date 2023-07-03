using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public static bool ValidateTextFormat(this string str ) {
            return Regex.IsMatch(str, @"^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$");
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
            return !Regex.IsMatch(str, @"^[a-zA-Z ,&$@.'-]+$");
        }
        
        public static bool ValidateBankAgencyFormat(this string str ) {
            return !Regex.IsMatch(str, @"^[0-9 -]+$");
        }
    }
}