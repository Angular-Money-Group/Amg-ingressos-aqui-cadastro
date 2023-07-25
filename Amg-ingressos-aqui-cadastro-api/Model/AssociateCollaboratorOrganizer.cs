using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_cadastro_api.Model
{
    public class AssociateCollaboratorOrganizer 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string IdUserOrganizer { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string IdUserCollaborator { get; set; }
    }
}