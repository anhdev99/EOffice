using MongoDB.Bson.Serialization.Attributes;

namespace EOffice.WebAPI.Models
{
    public class Notify : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Read { get; set; } = false;
        public string RecipientId { get; set; }
        public string Recipient { get; set; }
        public string SenderId { get; set; }
        public string Sender { get; set; }
        public string Url { get; set; }
    }
}