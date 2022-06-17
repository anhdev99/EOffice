using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace EOffice.WebAPI.Models
{
    public class VanBanDen: Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public int Version { get; set; }
        public int Number { get; set; }
        public string LoaiVanBan { get; set; }
        public string LoaiVanBanTen { get; set; }
        public string TrangThai { get; set; }
        public string TrangThaiTen { get; set; }
        public string SoLuuCV { get; set; }
        public string SoVBDi { get; set; }
        public DateTime? NgayNhap { get; set; }
        public DateTime? NgayTraLoi { get; set; }
        public string TraLoiCVSo { get; set; }
        public string SoBan { get; set; }
        public string TrichYeu { get; set; }
        public string DonViSoan { get; set; }
        public string DonViSoanTen { get; set; }
        public string CanBoSoan { get; set; }
        public string CanBoSoanTen { get; set; }
        public string HinhThucGui { get; set; }
        public string HinhThucGuiTen { get; set; }
        public string LinhVuc { get; set; }
        public string LinhVucTen { get; set; }
        public string MucDoBaoMat { get; set; }
        public string MucDoBaoMatTen { get; set; }
        public string MucDoTinhChat { get; set; }
        public string MucDoTinhChatTen { get; set; }
        public string HoSoDonVi { get; set; }
        public string HoSoDonViTen { get; set; }
        public string NoiLuuTru { get; set; }
        public string CoQuanNhan { get; set; }
        public string CoQuanNhanTen { get; set; }
        public string KhoiCoQuanNhan { get; set; }
        public string KhoiCoQuanNhanTen { get; set; }
        public FileShort File { get; set; }
        [BsonIgnore]
        public FileShort UploadFiles{ get; set; }
        public List<NguoiPhanCong> NguoiPhanCong { get; set; }
    }
}