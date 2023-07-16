using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Infra;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class AssociateColabOrganizerRepository: IAssociateColabOrganizerRepository
    {
        private readonly IMongoCollection<AssociateColabOrganizer> _associateCollection;
        public AssociateColabOrganizerRepository(IDbConnection<AssociateColabOrganizer> dbconnectionIten)
        {
            _associateCollection = dbconnectionIten.GetConnection("organizer_colaborator");
        }
        public async Task<object> AssociateColabAsync(AssociateColabOrganizer associateColab)
        {
            try
            {
                await _associateCollection.InsertOneAsync(associateColab);
                return associateColab;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<object> FindAllColabsOfProducer<T>(string idProducer) {
            try {

                var filter = Builders<AssociateColabOrganizer>.Filter.Eq("idUserOrganizer", idProducer);
                var producerColabs = await _associateCollection.Find(filter)
                                                .ToListAsync() ?? 
                                                throw new ProducerColabNotFound("Este produtor ainda não cadastrou nenhum colaborador...");
                return producerColabs;
                    
            }
            catch (ProducerColabNotFound ex) {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}