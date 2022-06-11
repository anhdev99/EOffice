using MongoDB.Bson.Serialization.Attributes;

namespace EOffice.WebAPI.Models
{
    public class ChucVu : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Ten { get; set; }
        public string MoTa { get; set; }
        public int ThuTu { get; set; }
    }
    public class ChucVuShort
    {
        public string Id { get; set; }
        public string Ten { get; set; }
        // public string MoTa { get; set; }
        // public string ThuTu { get; set; }
    }
}