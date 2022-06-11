using System.Collections.Generic;
using EOffice.WebAPI.Models;

namespace EOffice.WebAPI.ViewModels
{
    public class NotifyVM
    {
        public long Number { get; set; }
        public List<Notify> ListNotify { get; set; }
    }
}