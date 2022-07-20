using System;
using System.IO;
using EOffice.WebAPI.Services.SignDigital;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EOffice.WebAPI.APIs
{
    [Route("api/v1/[controller]")]
    public class SignDigitalController : ControllerBase
    {
        [HttpPost]
        public ResponseMessage Pdf(IFormCollection data, IFormFile fileUpload)
        {
            string user = data["user"];
            string pass = data["pass"];
            string content = data["content"];
            string fileName = data["fileName"];
            string xPosition = data["xPosition"];
            string yPosition = data["yPosition"];
            string pageNumber = data["pageNumber"];
            byte[] fileInput = null;
            using (var ms = new MemoryStream())
            {
                fileUpload.CopyTo(ms);
                fileInput = ms.ToArray();
                string s = Convert.ToBase64String(fileInput);
                // act on the Base64 data
            }
            ResponseMessage result = SmartCA.getSignFile(user, pass, content, fileName, fileInput, pageNumber, xPosition, yPosition);
            return result;
        }
    }
}