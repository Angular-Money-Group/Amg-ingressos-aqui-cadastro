// using Amg_ingressos_aqui_cadastro_api.Dtos;
// using Amg_ingressos_aqui_cadastro_api.Exceptions;
// using Amg_ingressos_aqui_cadastro_api.Model;
// using Amg_ingressos_aqui_cadastro_api.Enum;
// using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
// using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
// using Amg_ingressos_aqui_cadastro_api.Utils;
// using System;
// using System.Text.RegularExpressions;

// namespace Amg_ingressos_aqui_cadastro_api.Services
// {
//     public class EventColabService : IEventColabService
//     {
//         private IEventColabRepository _eventColabRepository;
//         private IUserService _userService;
//         private MessageReturn? _messageReturn;

//         public EventColabService(IEventColabRepository eventColabRepository, IUserService userService)
//         {
//             this._eventColabRepository = eventColabRepository;
//             this._userService = userService;
//         }
        
//         public async Task<MessageReturn> GetAllEventColabsAsync()
//         {
//             this._messageReturn = new MessageReturn();
//             try
//             {
//                 var result = await _eventColabRepository.GetAllEventColabs<EventColab>();

//                 List<EventColabDTO> list = new List<EventColabDTO>();
//                 foreach (EventColab eventColab in result) {
//                     list.Add(new EventColabDTO(eventColab));
//                 }
//                 _messageReturn.Data = list;
//             }
//             catch (GetAllEventColabException ex)
//             {
//                 _messageReturn.Data = null;
//                 _messageReturn.Message = ex.Message;
//             }
//             catch (Exception ex)
//             {
//                 throw ex;
//             }
//             return _messageReturn;
//         }
        
//         public async Task<MessageReturn> FindByIdAsync(string idEventColab)
//         {
//             this._messageReturn = new MessageReturn();
//             try
//             {
//                 idEventColab.ValidateIdMongo();

//                 EventColab eventColab = await _eventColabRepository.FindByField<EventColab>("Id", idEventColab);
//                 _messageReturn.Data = new EventColabDTO(eventColab);

//             }
//             catch (IdMongoException ex)
//             {
//                 _messageReturn.Data = null;
//                 _messageReturn.Message = ex.Message;
//             }
//             catch (EventColabNotFound ex)
//             {
//                 _messageReturn.Data = null;
//                 _messageReturn.Message = ex.Message;
//             }
//             catch (Exception ex)
//             {
//                 throw ex;
//             }

//             return _messageReturn;
//         }
        
//         public async Task<MessageReturn> GetAllColabsOfEventAsync(string idEvent)
//         {
//             this._messageReturn = new MessageReturn();
//             try
//             {
//                 idEvent.ValidateIdMongo();

//                 EventColab eventColab = await _eventColabRepository.FindByField<EventColab>("IdEvent", idEvent);
//                 _messageReturn.Data = new EventColabDTO(eventColab);

//             }
//             catch (IdMongoException ex)
//             {
//                 _messageReturn.Data = null;
//                 _messageReturn.Message = ex.Message;
//             }
//             catch (EventColabNotFound ex)
//             {
//                 _messageReturn.Data = null;
//                 _messageReturn.Message = ex.Message;
//             }
//             catch (Exception ex)
//             {
//                 throw ex;
//             }

//             return _messageReturn;
//         }
        
//         public async Task<MessageReturn> SaveAsync(EventColabDTO eventColabSaveDTO) {
//             this._messageReturn = new MessageReturn();
//             try
//             {
//                 // controlar o fluxo para nao permitir que o colab entre aqui de primeira
//                 EventColab eventColab = eventColabSaveDTO.makeEventColabSave();       
                
//                 var id = await _eventColabRepository.Save<EventColab>(eventColab);
//                 _messageReturn.Data = id;
//             }
//             catch (UserNotFound ex)
//             {
//                 _messageReturn.Data = null;
//                 _messageReturn.Message = ex.Message;
//             }
//             catch (EmptyFieldsException ex)
//             {
//                 _messageReturn.Data = null;
//                 _messageReturn.Message = ex.Message;
//             }
//             catch (InvalidFormatException ex)
//             {
//                 _messageReturn.Data = null;
//                 _messageReturn.Message = ex.Message;
//             }
//             catch (SaveEventColabException ex)
//             {
//                 _messageReturn.Data = null;
//                 _messageReturn.Message = ex.Message;
//             }
//             catch (Exception ex)
//             {
//                 throw ex;
//             }

//             return _messageReturn;
//         }
//         public async Task<MessageReturn> RegisterColabAsync(string idEvent, UserDTO colab) {
//             this._messageReturn = new MessageReturn();
//             try
//             {
//                 EventColabDTO.ValidateIdEventFormat(idEvent);
//                 colab.Type = TypeUserEnum.Colab;
//                 User colabUser = colab.makeUserSave();

//                 _messageReturn = await _userService.FindByIdAsync(TypeUserEnum.Event, idEvent);
//                 if(!_messageReturn.hasRunnedSuccessfully()) {
//                     throw new UserNotFound("Id de Event não encontrado.");
//                 }

//                 // UserDTO event = _messageReturn.Data as UserDTO;
//                 // if (event.Type != TypeUserEnum.Event) {
//                 //     throw new GetByIdUserException("O id de usuário não corresponde ao de um Produtor.");
//                 // }
               
//                 _messageReturn = await _userService.SaveAsync(colab);
//                 if(!_messageReturn.hasRunnedSuccessfully()) {
//                     throw new SaveUserException("Nao foi possivel salvar o Colaborador no banco");
//                 }

//                 colabUser.Id = _messageReturn.Data as string;
//                 EventColabDTO eventColabDTO = new EventColabDTO(idEvent, colabUser.Id);

//                 _messageReturn = await SaveAsync(eventColabDTO);
//                 if(!_messageReturn.hasRunnedSuccessfully()) {
//                     string ids = eventColabDTO.IdEvent + '\t' + eventColabDTO.IdColab;
//                     throw new SaveEventColabException("Nao foi possivel salvar a relacao ColaboradorXProdutor no banco.\n" + ids);
//                 }
                
//             // dar rollback caso nao tenha adicionado idColab na tabela colab x event

//                 _messageReturn.Data = colabUser.Id;
//             }
//             catch (IdMongoException ex)
//             {
//                 _messageReturn.Data = null;
//                 _messageReturn.Message = ex.Message;
//             }
//             catch (InvalidFormatException ex)
//             {
//                 _messageReturn.Data = null;
//                 _messageReturn.Message = ex.Message;
//             }
//             catch (EmptyFieldsException ex)
//             {
//                 _messageReturn.Data = null;
//                 _messageReturn.Message = ex.Message;
//             }
//             catch (UserNotFound ex)
//             {
//                 _messageReturn.Data = null;
//                 _messageReturn.Message = ex.Message;
//             }
//             catch (GetByIdUserException ex)
//             {
//                 _messageReturn.Data = null;
//                 _messageReturn.Message = ex.Message;
//             }
//             catch (SaveUserException ex)
//             {
//                 _messageReturn.Data = null;
//                 _messageReturn.Message = "Erro ao salvar colaborador no banco";
//             }
//             catch (Exception ex)
//             {
//                 throw ex;
//             }
//             return _messageReturn;
//         }

//         public async Task<bool> DoesIdExists(string idEventColab) {
//             this._messageReturn = new MessageReturn();
//             try {
//                 return await _eventColabRepository.DoesValueExistsOnField<EventColab>("Id", idEventColab);
//             }
//             catch (Exception ex)
//             {
//                 throw ex;
//             }
//         }

//         public async Task<MessageReturn> DeleteAsync(string id) {
//             this._messageReturn = new MessageReturn();
//             try
//             {
//                 id.ValidateIdMongo();
                
//                 if (!await DoesIdExists(id))
//                     throw new EventColabNotFound("Id de eventXcolab não encontrada.");

//                 _messageReturn.Data = await _eventColabRepository.Delete<EventColab>(id) as string;
//             }
//             catch (IdMongoException ex)
//             {
//                 _messageReturn.Data = null;
//                 _messageReturn.Message = ex.Message;
//             }
//             catch (EventColabNotFound ex)
//             {
//                 _messageReturn.Data = null;
//                 _messageReturn.Message = ex.Message;
//             }
//             catch (DeleteEventColabException ex)
//             {
//                 _messageReturn.Data = null;
//                 _messageReturn.Message = ex.Message;
//             }
//             catch (Exception ex)
//             {
//                 throw ex;
//             }
//             return _messageReturn;
//         }
//     }
// }