using System.Collections.Generic;
using EOffice.WebAPI.Models;
using Spire.Pdf.Exporting.XPS.Schema;

namespace EOffice.WebAPI.ViewModels
{
    public class LichCongTacVM
    {
        public string NgayXepLich { get; set; }
        public List<CongViec> CongViecs { get; set; }
    }
}