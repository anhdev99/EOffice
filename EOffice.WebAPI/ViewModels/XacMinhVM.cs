using System.Collections.Generic;
using EOffice.WebAPI.Models;

namespace EOffice.WebAPI.ViewModels
{
    public class XacMinhVM
    {
        public User User { get; set; }
        public List<FileShort> UploadFiles { get; set; }
    }
}