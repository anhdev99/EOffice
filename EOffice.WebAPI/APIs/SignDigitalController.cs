using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EOffice.WebAPI.Data;
using EOffice.WebAPI.Exceptions;
using EOffice.WebAPI.Helpers;
using EOffice.WebAPI.Interfaces;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Services.SignDigital;
using EOffice.WebAPI.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using EResultResponse = EOffice.WebAPI.Helpers.EResultResponse;

namespace EOffice.WebAPI.APIs
{
    [Route("api/v1/[controller]")]
    public class SignDigitalController : ControllerBase
    {
        private IVanBanDiService _vanBanDiService;
        private IFileService _fileService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private DataContext _context;
        public SignDigitalController(IFileService fileService, IVanBanDiService vanBanDiService, IWebHostEnvironment hostingEnvironment, DataContext context)
        {
            _vanBanDiService = vanBanDiService;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _fileService = fileService;
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
            string vanBanDiId = data["vanBanDiId"];
            byte[] fileInput = null;
            using (var ms = new MemoryStream())
            {
                fileUpload.CopyTo(ms);
                fileInput = ms.ToArray();
                string s = Convert.ToBase64String(fileInput);
                // act on the Base64 data
            }
            ResponseMessage result = SmartCA.getSignFile(user, pass, content, fileName, fileInput, pageNumber, xPosition, yPosition);

            if (result.Content != null)
            {
                var uploadDirecotroy = "files/";
                var uploadPath = Path.Combine(_hostingEnvironment.ContentRootPath, uploadDirecotroy);

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);
              
                var dateTime = DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss");
                var path = Path.Combine(_hostingEnvironment.ContentRootPath, uploadDirecotroy, dateTime);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var newFileName = Guid.NewGuid().ToString() +"." + fileName.Split(".")[1];
                var relativePath = Path.Combine("", dateTime, newFileName);
                var filePath = Path.Combine(uploadDirecotroy, relativePath);

                using (System.IO.FileStream stream = System.IO.File.Create(filePath ))
                {
                    System.Byte[] byteArray = result.Content as byte[];
                     stream.Write(byteArray, 0, byteArray.Length);
                }
                
                 var result1 =  _fileService.SaveFileAsync(filePath, fileName, newFileName,  fileName.Split(".")[1], result.Content.ToString().Length);

                Task.WhenAll(result1);
                 var vanBanDi = _context.VanBanDi.Find(x => x.Id == vanBanDiId).FirstOrDefault();
                 if (vanBanDi != default)
                 {
                     if (vanBanDi.FilePDF == default)
                         vanBanDi.FilePDF = new List<FileShort>();
                     vanBanDi.FilePDF.Add(new FileShort(){Ext = result1.Result.Ext,FileId = result1.Result.Id, FileName = result1.Result.FileName,});
                     
                     ReplaceOneResult actionResult
                         =  _context.VanBanDi.ReplaceOne(x => x.Id.Equals(vanBanDi.Id)
                             , vanBanDi
                             , new ReplaceOptions { IsUpsert = true });
                     var result2 = actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
                     if (!result2)
                     {
                         
                     }
                 }
            }
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