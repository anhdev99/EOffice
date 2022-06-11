using System.ComponentModel.DataAnnotations;

namespace EOffice.WebAPI.Models
{
    public enum DefineStatus
    {
        [Display(Name = "KHÔNG TIẾP NHẬN")]
        KHONGTIEPNHAN,
        [Display(Name = "CHỜ DUYỆT")]
        CHODUYET,
        [Display(Name = "VỪA TIẾP NHẬN")]
        VUATIEPNHAN,
        [Display(Name = "ĐANG XỬ LÝ")]
        DANGXULY,
        [Display(Name = "ĐÃ XỬ LÝ XONG")]
        DAXULYXONG,
        [Display(Name = "ĐÃ TRẢ LỜI NGƯỜI DÂN")]
        DATRALOINGUOIDAN,
    }
}
