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
        public string SoVBDen{ get; set; }
        public DateTime? NgayNhap { get; set; }
        public DateTime? NgayNhan { get; set; }
        public DateTime? NgayBanHanh { get; set; }
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
        
        public string CoQuanGui { get; set; }
        public string CoQuanGuiTen { get; set; }
        public string KhoiCoQuanGui { get; set; }
        public string KhoiCoQuanGuiTen { get; set; }
        public string HanXuLy { get; set; }
        public string CongVanChiDoc { get; set; }
        public string BanChinh { get; set; }
        public string HienThiThongBao { get; set; }
        public string NguoiKy { get; set; }
        public FileShort File { get; set; }
        [BsonIgnore]
        public FileShort UploadFiles{ get; set; }
        public List<NguoiPhanCong> NguoiPhanCong { get; set; }
        
        public ButPhe ButPhe { get; set; }
        public List<DonViXuLy> DonViXuLy { get; set; }
        
        public List<PhanCong> PhanCong { get; set; }

    }

    public class ButPhe
    {
        public string NoiDungButPhe { get; set; }
        public string NguoiButPhe { get; set; }
        public string NgayButPhe { get; set; }
        public FileShort File { get; set; }
        [BsonIgnore]
        public FileShort UploadFiles{ get; set; }
        public string MucDoQuanTrong { get; set; }
        public string NguoiPhuTrach { get; set; }
        public string NguoiChuTri { get; set; }
        public string NguoiPhoiHopXuLy { get; set; }
        public string DonViXuLy { get; set; }
        public string DonViPhoiHop { get; set; }
        public string NguoiXemDeBiet { get; set; }
    }

    public class DonViXuLy
    {
        public string NguoiPhuTrach { get; set; }
        public string NguoiChuTri { get; set; }
        public string NguoiPhoiHopXuLy { get; set; }
        public string NguoiXemDeBiet { get; set; }
        public string DonViNhanXuLy { get; set; }
        public string DonViPhoiHop { get; set; }
        public string TrangThaiDuyetVB { get; set; }
        public string NguoiDuyetVB { get; set; }
        public DateTime?NgayDuyetVB { get; set; }
        public string GhiChu { get; set; }
    }

    public class PhanCong
    {
        public string YKienChiDao { get; set; }
        public string NguoiButPhe { get; set; }
        public string NguoiNhanXuLy { get; set; }
        public FileShort File { get; set; }
        [BsonIgnore]
        public FileShort UploadFiles{ get; set; }
    }
}