using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_cadastro_api.Repository.Querys
{
    public static class QuerysMongo
    {
        public const string GetTransactionQuery = @"{
                                $lookup: {
                                    from: 'transactionIten',
                                    'let': { transactionId : { '$toString': '$_id' }},
                                    pipeline: [
                                        {
                                            $match: {
                                                $expr: {
                                                    $eq: [
                                                        '$IdTransaction',
                                                        '$$transactionId'
                                                    ]
                                                }
                                            }
                                        }
                                    ],
                                    as: 'transactionItens'
                                }
                            }";
    }
}