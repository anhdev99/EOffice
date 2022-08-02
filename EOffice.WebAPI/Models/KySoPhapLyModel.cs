namespace EOffice.WebAPI.Models
{
    public class KySoPhapLyModel
    {
        public string Id { get; set; }
        public FileShort File { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Px { get; set; }
        public int Py { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Page { get; set; }
        public string Image { get; set; }
    }
}