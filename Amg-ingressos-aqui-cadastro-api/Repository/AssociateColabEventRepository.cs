using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_cadastro_api.Infra;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class AssociateColabEventRepository : IAssociateColabEventRepository
    {

        private readonly IMongoCollection<AssociateColabEvent> _associateCollection;
        public AssociateColabEventRepository(IDbConnection<AssociateColabEvent> dbconnectionIten)
        {
            _associateCollection = dbconnectionIten.GetConnection("event_collaborator");
        }
        public async Task<Object> AssociateColabEventAsync(AssociateColabEvent associateColab)
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

        public async Task<object> FindAllColabsOfEvent<T>(string idEvent)
        {
           try {

                var filter = Builders<AssociateColabEvent>.Filter.Eq("idEvent", idEvent);
                var eventCollaborator = await _associateCollection.Find(filter)
                                                .ToListAsync();
                return eventCollaborator;
                    
            }
            catch (Exception ex) {
                throw ex;
            }
        }
    }
}