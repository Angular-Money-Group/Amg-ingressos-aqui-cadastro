using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Utils;

namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class TransactionService : ITransactionService
    {
        private ITransactionRepository _transactionRepository;
        private MessageReturn _messageReturn;

        public TransactionService(
            ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> GetByIdAsync(string idTransaction)
        {
            try
            {
                idTransaction.ValidateIdMongo("Transação");

                _messageReturn.Data = await _transactionRepository.GetById(idTransaction);

            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (GetByIdTransactionException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }    
    }
}