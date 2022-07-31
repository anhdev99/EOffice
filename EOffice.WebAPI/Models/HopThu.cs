using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace EOffice.WebAPI.Models
{
    public class HopThu: Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string TieuDe { get; set; }
        public List<UserShort> NguoiNhans { get; set; }
        public List<UserShort> Cc { get; set; }
        public string NoiDung { get; set; }
        public List<FileShort> Files{ get; set; }
        [BsonIgnore]
        public List<FileShort> UploadFiles{ get; set; }
        public UserShort NguoiGui { get; set; }
        public DateTime NgayGui { get; set; }
        public UserShort NguoiNhan { get; set; }
        public DateTime NgayNhan { get; set; }
        public bool DaXem { get; set; } = false;
    }
}