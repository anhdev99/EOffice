using MongoDB.Bson.Serialization.Attributes;

namespace EOffice.WebAPI.Models
{
    public class LoaiVanBan : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Code { get; set; }
        public string Ten { get; set; }
        public int ThuTu { get; set; }
    }
}