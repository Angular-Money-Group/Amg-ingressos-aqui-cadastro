using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Amg_ingressos_aqui_cadastro_api.Model 
{
    public class ContaRecebimento 
    {
        // <summary>
        /// Nome do uruário
        /// </summary>
        public string? fullName { get; set; }

        // <summary>
        /// Nome do uruário
        /// </summary>
        public string? bank { get; set; }


        // <summary>
        /// Nome do uruário
        /// </summary>
        public string? bankAgency { get; set; }

        // <summary>
        /// Nome do uruário
        /// </summary>
        public string? bankAccount { get; set; }


        // <summary>
        /// Nome do uruário
        /// </summary>
        public string? bankDigit { get; set; }

    }

    
}
