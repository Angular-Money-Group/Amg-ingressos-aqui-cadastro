using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_cadastro_api.Model
{
    public class AssociateCollaboratorEvent
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string? Id { get; set; }
        
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdEvent { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string IdUserCollaborator { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string IdUserOrganizer { get; set; }
    }
}