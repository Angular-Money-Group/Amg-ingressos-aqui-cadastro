using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_cadastro_api.Model
{
    public class AssociateColabEvent
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id;
        public string idEvent { get; set; }
        public string idUserColaborator { get; set; }
        public string idUserOrganizer { get; set; }
    }
}