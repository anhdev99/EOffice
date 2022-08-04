using System.Collections.Generic;

namespace EOffice.WebAPI.Models
{
    public class SignDigital
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string TypeKySo { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public int Page { get; set; }
        public string Image { get; set; }
        public dynamic Payload { get; set; }
        public dynamic File { get; set; }
        
        public string FontFamily { get; set; }
        public int LineHeight { get; set; }
        public List<string> Lines { get; set; }
        public int Size { get; set; }
        public string Text { get; set; }
    }
}