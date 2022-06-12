using System.Collections.Generic;
using EOffice.WebAPI.Models;

namespace EOffice.WebAPI.ViewModels
{
    public class LoaiVanBanTreeVM
    {

        public LoaiVanBanTreeVM(LoaiVanBanTreeVM model)
        {
            this.Id = model.Id;
            this.Label = model.Label;
        }
        public LoaiVanBanTreeVM(LoaiVanBan model)
        {
            this.Id = model.Id;
            this.Label = model.Ten;
            this.Selected = false;
            this.Opened = false;
        }
        public string Id { get; set; }
        public string Label { get; set; }
        

        public bool Selected { get; set; } = false;
        public bool Opened { get; set; } = false;
        
        public List<LoaiVanBanTreeVM> Children { get; set; }
    }
}