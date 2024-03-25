using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_cadastro_api.Model
{
    public class AssociateUserApiDataEvent
    {
        public AssociateUserApiDataEvent()
        {
            Id = string.Empty;
            IdUser = string.Empty;
            IdEvent = string.Empty;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; }

        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdUser { get; set; }

        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdEvent { get; set; }
    }
}