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
        public LoaiVanBan LoaiVanBan { get; set; }
        public string LoaiVanBanTen { get; set; }
        public TrangThai TrangThai { get; set; }
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
        public DonVi DonViSoan { get; set; }
        public string DonViSoanTen { get; set; }
        public User CanBoSoan { get; set; }
        public string CanBoSoanTen { get; set; }
        public HinhThucGui HinhThucNhan { get; set; }
        public string HinhThucNhanTen { get; set; }
        public HinhThucGui HinhThucGui { get; set; }
        public string HinhThucGuiTen { get; set; }
        public LinhVuc LinhVuc { get; set; }
        public string LinhVucTen { get; set; }
        public EnumModel MucDoBaoMat { get; set; }
        public string MucDoBaoMatTen { get; set; }
        public EnumModel MucDoTinhChat { get; set; }
        public string MucDoTinhChatTen { get; set; }
        public HoSoDonVi HoSoDonVi { get; set; }
        public string HoSoDonViTen { get; set; }
        public string NoiLuuTru { get; set; }
        public CoQuan CoQuanNhan { get; set; }
        public string CoQuanNhanTen { get; set; }
        
        public KhoiCoQuan KhoiCoQuanNhan { get; set; }
        public string KhoiCoQuanNhanTen { get; set; }
        
        public CoQuan CoQuanGui { get; set; }
        public string CoQuanGuiTen { get; set; }
        public KhoiCoQuan KhoiCoQuanGui { get; set; }
        public string KhoiCoQuanGuiTen { get; set; }
        public DateTime? HanXuLy { get; set; }
        public bool CongVanChiDoc { get; set; }
        public bool BanChinh { get; set; }
        public bool HienThiThongBao { get; set; }
        public User NguoiKy { get; set; }
        public DateTime? NgayKy { get; set; }
        public FileShort File { get; set; }
        [BsonIgnore]
        public FileShort UploadFiles{ get; set; }
        public List<NguoiPhanCong> NguoiPhanCong { get; set; }
        
        //public ButPhe ButPhe { get; set; }
        //public List<DonViXuLy> DonViXuLy { get; set; }
        
        //public List<PhanCong> PhanCong { get; set; }

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