using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace EOffice.WebAPI.Models
{
    public class LichCongTac: Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public string ThoiGian { get; set; }
        public User ChuTri { get; set; }
        public string MauSac { get; set; }
        public string DiaDiem { get; set; }
        public string TieuDe { get; set; }
        public string GhiChu { get; set; }
        public List<User> ThanhPhanThamDu { get; set; }
    }

    //public class ThanhPhanThamDu
    //{
        //public string UserId { get; set; }
        //public string Ten { get; set; }
        //public string ChucVu { get; set; }
    //}
}