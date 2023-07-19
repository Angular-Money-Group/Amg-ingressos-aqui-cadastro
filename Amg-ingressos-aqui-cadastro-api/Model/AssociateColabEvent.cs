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
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string? Id { get; set; }
        public string IdEvent { get; set; }
        public string IdUserColaborator { get; set; }
        public string IdUserOrganizer { get; set; }
    }
}