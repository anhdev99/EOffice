using System.Collections.Generic;
using EOffice.WebAPI.Models;

namespace EOffice.WebAPI.ViewModels
{
    public class MenuTreeVM
    {
        public MenuTreeVM(MenuTreeVM model)
        {
            this.Id = model.Id;
            this.Label = model.Text;
            this.Text = model.Text;
            this.Link = model.Link;
            this.Icon = model.Icon;
        }
        public MenuTreeVM(Menu model)
        {
            this.Id = model.Id;
            this.Label = model.Ten;
            this.Text = model.Ten;
            this.Link = model.Path ?? "";
            this.Icon = model.Icon ?? "";
            this.ParentId = model.ParentId;
        }
        public string Id { get; set; }
        public string Label { get; set; }
        public string Text { get; set; }
        public List<MenuTreeVM> Children { get; set; }
        public List<MenuTreeVM> SubItems { get; set; } = new List<MenuTreeVM>();
        public State State { get; set; } = new State();
        public bool Opened { get; set; } = false;
        public string ParentId { get; set; }= "";
        public string Link { get; set; } = "";
        public string Icon { get; set; } = "";
        public bool Selected { get; set; } = false;
    }
    public class State
    {
        public bool Expanded { get; set; } = false;
    }
}