using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_cadastro_api.Consts
{
    public static class StatusCallbackCielo
    {
        public const string NotAllowed = "Não Autorizada";
        public const string ExpiredCard = "Cartão Expirado";
        public const string BlockedCard = "Cartão Bloqueado";
        public const string TimeOut = "Time Out";
        public const string CanceledCard = "Cartão Cancelado";
        public const string CreditCardProblems = "Problemas com o Cartão de Crédito";
        public const string OperationSuccessfulTimeout = "Operation Successful / Time Out";
        public const string SuccessfullyPerformedOperation = "Operação realizada com sucesso";
        
    }
}