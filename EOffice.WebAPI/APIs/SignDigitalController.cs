using System;
using System.IO;
using System.Threading.Tasks;
using EOffice.WebAPI.Exceptions;
using EOffice.WebAPI.Helpers;
using EOffice.WebAPI.Interfaces;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Services.SignDigital;
using EOffice.WebAPI.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EResultResponse = EOffice.WebAPI.Helpers.EResultResponse;

namespace EOffice.WebAPI.APIs
{
    [Route("api/v1/[controller]")]
    public class SignDigitalController : ControllerBase
    {
        private IVanBanDiService _vanBanDiService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public SignDigitalController(IVanBanDiService vanBanDiService, IWebHostEnvironment hostingEnvironment)
        {
            _vanBanDiService = vanBanDiService;
            _hostingEnvironment = hostingEnvironment;
        }
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
        
        [HttpPost]
        [Route("xac-thuc")]
        public async Task<IActionResult> XacMinh([FromBody] XacMinhVM model)
        {
            try
            {
                await _vanBanDiService.XacThuc(model);
                return Ok(
                    new ResultMessageResponse()
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage("Xác thực thành công!")
                );
            }
            catch (ResponseMessageException ex)
            {
                return Ok(
                    new ResultMessageResponse().WithCode(ex.ResultCode)
                        .WithMessage(ex.ResultString)
                );
            }
        }
    }
}