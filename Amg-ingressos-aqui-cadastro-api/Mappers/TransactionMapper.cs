using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;
using AutoMapper;

namespace Amg_ingressos_aqui_cadastro_api.Mappers
{
    public static class TransactionItemMapper
    {
        public static Transaction StageTicketDataDtoToTransaction(this TransactionDto stageTicket)
        {
            return new Transaction()
            {
                Id = stageTicket.Id,
            };
        }
    }
}