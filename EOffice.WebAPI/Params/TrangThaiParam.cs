using System.Collections.Generic;
using EOffice.WebAPI.Models;

namespace EOffice.WebAPI.Params
{
    public class TrangThaiParam : PagingParam
    {
        public string LoaiTrangThai { get; set; }
        public TrangThaiShort CurrentTrangThai { get; set; }
        public TrangThaiShort NewTrangThai { get; set; }
        public string VanBanDiId { get; set; }
        public string NoiDung { get; set; }
        public string UserName { get; set; }
        public UserShort LanhDaoDonVi { get; set; }
        public List<DonVi> DonVi { get; set; }
    }
}