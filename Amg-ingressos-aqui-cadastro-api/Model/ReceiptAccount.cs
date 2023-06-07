using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Amg_ingressos_aqui_cadastro_api.Model 
{
    public class ReceiptAccount 
    {
        /// <summary>
        /// Nome do uruário
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Instituição Bancária
        /// </summary>
        public string? Bank { get; set; }
        
        /// <summary>
        /// Agência Bancária 
        /// </summary>
        public string? BankAgency { get; set; }

        /// <summary>
        /// Conta Bancária
        /// </summary>
        public string? BankAccount { get; set; }

        /// <summary>
        /// Dígito da Conta 
        /// </summary>
        public string? BankDigit { get; set; }
    }
}
