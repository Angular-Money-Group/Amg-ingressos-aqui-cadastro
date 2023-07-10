using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<MessageReturn> AssociateColabEventAsync(AssociateColabEvent colabEvent)
        {
            try
            {
                _messageReturn.Data = await _associateColabEventRepository.AssociateColabEventAsync(colabEvent);
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
                    idUserColaborator = idUserColaborator,
                    idUserOrganizer = idUserOrganizer
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }
    }
}