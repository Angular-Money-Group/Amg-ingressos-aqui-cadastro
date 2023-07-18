using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;

namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class AssociateService : IAssociateService
    {
        private MessageReturn _messageReturn;
        private IAssociateColabOrganizerRepository _associateColabOrganizerRepository;
        private IAssociateColabEventRepository _associateColabEventRepository;

        public AssociateService(
            IAssociateColabOrganizerRepository associateColabOrganizerRepository,
            IAssociateColabEventRepository associateColabEventRepository)
        {
            _associateColabOrganizerRepository = associateColabOrganizerRepository;
            _associateColabEventRepository = associateColabEventRepository;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> AssociateColabEventAsync(AssociateColabEvent collaboratorEvent)
        {
            try
            {
                _messageReturn.Data = await _associateColabEventRepository.AssociateCollaboratorEventAsync(collaboratorEvent);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> AssociateColabOrganizerAsync(string idUserOrganizer, string idUserColaborator)
        {
            try
            {
                _messageReturn.Data = await _associateColabOrganizerRepository
                .AssociateColabAsync(new AssociateColabOrganizer()
                {
                    IdUserColaborator = idUserColaborator,
                    IdUserOrganizer = idUserOrganizer
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> DeleteAssociateColabEventAsync(string idAssociate)
        {
            try
            {
                _messageReturn.Data = await _associateColabEventRepository
                                        .DeleteAssociateCollaboratorEventAsync(idAssociate);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> DeleteAssociateColabOrganizerAsync(string idAssociate)
        {
             try
            {
                _messageReturn.Data = await _associateColabOrganizerRepository
                                        .DeleteAssociateColabAsync(idAssociate);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }
    }
}