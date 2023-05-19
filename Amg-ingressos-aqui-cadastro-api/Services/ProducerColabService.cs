using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Utils;
using System;
using System.Text.RegularExpressions;

namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class ProducerColabService : IProducerColabService
    {
        private IProducerColabRepository _producerColabRepository;
        private IUserService _userService;
        private MessageReturn? _messageReturn;

        public ProducerColabService(IProducerColabRepository producerColabRepository, IUserService userService)
        {
            this._producerColabRepository = producerColabRepository;
            this._userService = userService;
        }
        
        public async Task<MessageReturn> GetAllProducerColabsAsync()
        {
            this._messageReturn = new MessageReturn();
            try
            {
                var result = await _producerColabRepository.GetAllProducerColabs<ProducerColab>();

                List<ProducerColabDTO> list = new List<ProducerColabDTO>();
                foreach (ProducerColab producerColab in result) {
                    list.Add(new ProducerColabDTO(producerColab));
                }
                _messageReturn.Data = list;
            }
            catch (GetAllProducerColabException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _messageReturn;
        }
        
        public async Task<MessageReturn> FindByIdAsync(string idProducerColab)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                idProducerColab.ValidateIdMongo();

                ProducerColab producerColab = await _producerColabRepository.FindByField<ProducerColab>("Id", idProducerColab);
                _messageReturn.Data = new ProducerColabDTO(producerColab);

            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (ProducerColabNotFound ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }
        
        public async Task<MessageReturn> GetAllColabsOfProducerAsync(string idProducer)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                idProducer.ValidateIdMongo();

                ProducerColab producerColab = await _producerColabRepository.FindByField<ProducerColab>("IdProducer", idProducer);
                _messageReturn.Data = new ProducerColabDTO(producerColab);

            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (ProducerColabNotFound ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }
        
        public async Task<MessageReturn> SaveAsync(ProducerColabDTO producerColabSaveDTO) {
            this._messageReturn = new MessageReturn();
            try
            {
                // controlar o fluxo para nao permitir que o colab entre aqui de primeira
                ProducerColab producerColab = producerColabSaveDTO.makeProducerColabSave();       
                
                var id = await _producerColabRepository.Save<ProducerColab>(producerColab);
                _messageReturn.Data = id;
            }
            catch (UserNotFound ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (EmptyFieldsException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (InvalidFormatException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (SaveProducerColabException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }
        public async Task<MessageReturn> RegisterColabAsync(string idProducer, UserDTO colab) {
            this._messageReturn = new MessageReturn();
            try
            {
                ProducerColabDTO.ValidateIdProducerFormat(idProducer);
                colab.Type = TypeUserEnum.Colab;
                User colabUser = colab.makeUserSave();

                _messageReturn = await _userService.FindByIdAsync(TypeUserEnum.Producer, idProducer);
                if(!_messageReturn.hasRunnedSuccessfully()) {
                    throw new UserNotFound("Id de Producer não encontrado.");
                }

                // UserDTO producer = _messageReturn.Data as UserDTO;
                // if (producer.Type != TypeUserEnum.Producer) {
                //     throw new GetByIdUserException("O id de usuário não corresponde ao de um Produtor.");
                // }
               
                _messageReturn = await _userService.SaveAsync(colab);
                if(!_messageReturn.hasRunnedSuccessfully()) {
                    throw new SaveUserException("Nao foi possivel salvar o Colaborador no banco");
                }

                colabUser.Id = _messageReturn.Data as string;
                ProducerColabDTO producerColabDTO = new ProducerColabDTO(idProducer, colabUser.Id);

                _messageReturn = await SaveAsync(producerColabDTO);
                if(!_messageReturn.hasRunnedSuccessfully()) {
                    string ids = producerColabDTO.IdProducer + '\t' + producerColabDTO.IdColab;
                    throw new SaveProducerColabException("Nao foi possivel salvar a relacao ColaboradorXProdutor no banco.\n" + ids);
                }
                
            // dar rollback caso nao tenha adicionado idColab na tabela colab x producer

                _messageReturn.Data = colabUser.Id;
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (InvalidFormatException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (EmptyFieldsException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (UserNotFound ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (GetByIdUserException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (SaveUserException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = "Erro ao salvar colaborador no banco";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _messageReturn;
        }

        public async Task<bool> DoesIdExists(string idProducerColab) {
            this._messageReturn = new MessageReturn();
            try {
                return await _producerColabRepository.DoesValueExistsOnField<ProducerColab>("Id", idProducerColab);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MessageReturn> DeleteAsync(string id) {
            this._messageReturn = new MessageReturn();
            try
            {
                id.ValidateIdMongo();
                
                if (!await DoesIdExists(id))
                    throw new ProducerColabNotFound("Id de producerXcolab não encontrada.");

                _messageReturn.Data = await _producerColabRepository.Delete<ProducerColab>(id) as string;
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (ProducerColabNotFound ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (DeleteProducerColabException ex)
            {
                _messageReturn.Data = null;
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