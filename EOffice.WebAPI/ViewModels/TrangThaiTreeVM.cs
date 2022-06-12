using EOffice.WebAPI.Models;

namespace EOffice.WebAPI.ViewModels
{
    public class TrangThaiTreeVM
    {
        public TrangThaiTreeVM(TrangThaiTreeVM model)
        {
            this.Id = model.Id;
            this.Label = model.Label;
        }
        public TrangThaiTreeVM(TrangThai model)
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
    }
}