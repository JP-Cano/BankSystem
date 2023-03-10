using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DrivenAdapters.Mongo.Entities.Base
{
    /// <summary>
    /// Entidad base Mongo
    /// </summary>
    public class EntityBase
    {
        /// <summary>
        /// Id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}