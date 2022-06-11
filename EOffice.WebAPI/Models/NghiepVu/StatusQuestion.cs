using MongoDB.Bson.Serialization.Attributes;

namespace EOffice.WebAPI.Models
{
    public class StatusQuestion
    {
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        
        [BsonIgnore]
        public string QuestionId { get; set; }
    }

    public class HistoryAction
    {
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
    }
}