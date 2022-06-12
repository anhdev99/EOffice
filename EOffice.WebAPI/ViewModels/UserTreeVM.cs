using EOffice.WebAPI.Models;

namespace EOffice.WebAPI.ViewModels
{
    public class UserTreeVM
    {
        public UserTreeVM(UserTreeVM model)
        {
            this.Id = model.Id;
            this.Label = model.Label;
        }
        public UserTreeVM(User model)
        {
            this.Id = model.Id;
            this.Label = model.FullName;
            this.Selected = false;
            this.Opened = false;
        }
        public string Id { get; set; }
        public string Label { get; set; }
        

        public bool Selected { get; set; } = false;
        public bool Opened { get; set; } = false;
    }
}