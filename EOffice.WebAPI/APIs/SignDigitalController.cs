using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using MongoDB.Driver.Linq;
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
                     var newTrangThai = IAsyncCursorSourceExtensions.FirstOrDefault(_context.TrangThai.AsQueryable()
                             .Where(x => x.Code.ToUpper() == DefaultRoleCode.HOAN_THANH_KY_SO.ToUpper()).Select(x =>
                                 new TrangThaiShort()
                                 {
                                     Id = x.Id,
                                     Code = x.Code,
                                     Ten = x.Ten,
                                     BgColor = x.BgColor,
                                     Color = x.BgColor
                                 }));
                     vanBanDi.TrangThai = newTrangThai;
                     vanBanDi.Ower = vanBanDi.GetOwerWithRole(DefaultRoleCode.VAN_THU_TRUONG);
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
[Route("ThietLapKySo")]
        public ResponseMessage ThietLapKySo(IFormCollection data, IFormFile fileUpload)
        {
            try
            {
                string fileName = data["fileName"];
                string path = data["path"];
                string vanBanDiId = data["vanBanDiId"];
                byte[] fileInput = null;
                var detectIdFile = path.Split("/");
                var idFile = detectIdFile[detectIdFile.Length - 1];
                var file = _context.Files.Find(x => x.Id == idFile).FirstOrDefault();
                if (file == default)
                {
                    return new ResponseMessage()
                    {
                        ResponseID = Guid.NewGuid(),
                        ResponseCode = 0,
                        ResponseContent = "Thiết lập ký số không thành công"
                    };
                }

                var uploadDirecotroy = "files/";
                var uploadPath = Path.Combine(_hostingEnvironment.ContentRootPath, uploadDirecotroy);

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);
              
                var dateTime = DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss");
                var path1 = Path.Combine(_hostingEnvironment.ContentRootPath, uploadDirecotroy, dateTime);
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(path1);
                }
                var newFileName = Guid.NewGuid().ToString() +"." + fileName.Split(".")[1];
                var relativePath = Path.Combine("", dateTime, newFileName);
                var filePath = Path.Combine(uploadDirecotroy, relativePath);

                using (System.IO.FileStream stream = System.IO.File.Create(filePath ))
                {
                    fileUpload.CopyTo(stream);
                }

                file.SaveName = newFileName;
                file.Path = filePath;
                file.FileName = $"[da_thiet_lap_chu_ky]{fileName}";
                ReplaceOneResult actionResult
                    =  _context.Files.ReplaceOne(x => x.Id.Equals(file.Id)
                        , file
                        , new ReplaceOptions { IsUpsert = true });
                var result2 = actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
                if (!result2)
                {
                    return new ResponseMessage()
                    {
                        ResponseID = Guid.NewGuid(),
                        ResponseCode = 0,
                        ResponseContent = "Thiết lập ký số không thành công"
                    }; 
                }

                var vanBanDi = _context.VanBanDi.Find(x => x.Id == vanBanDiId).FirstOrDefault();
                if (vanBanDi != default)
                {
                    var indexFile = -1;
                    if (vanBanDi.FilePDF != null)
                    {
                         indexFile = vanBanDi.FilePDF.FindIndex(x => x.FileId == file.Id);
                        if (indexFile != -1)
                        {
                            vanBanDi.FilePDF[indexFile] = new FileShort()
                            {
                                FileId = file.Id,
                                FileName = file.FileName,
                                Ext = file.Ext
                            };
                        }
                    }
                    else
                    {
                        if (vanBanDi.File != null)
                        {
                            indexFile = vanBanDi.File.FindIndex(x => x.FileId == file.Id);
                            if (indexFile != -1)
                            {
                                vanBanDi.File[indexFile] = new FileShort()
                                {
                                    FileId = file.Id,
                                    FileName = file.FileName,
                                    Ext = file.Ext
                                };
                            }
                        }

                    }

                    if (indexFile != -1)
                    {
                        var newTrangThai = IAsyncCursorSourceExtensions.FirstOrDefault(_context.TrangThai.AsQueryable()
                            .Where(x => x.Code.ToUpper() == DefaultRoleCode.KY_SO_PHAP_LY_THIETLAP.ToUpper()).Select(x =>
                                new TrangThaiShort()
                                {
                                    Id = x.Id,
                                    Code = x.Code,
                                    Ten = x.Ten,
                                    BgColor = x.BgColor,
                                    Color = x.BgColor
                                }));
                        vanBanDi.TrangThai = newTrangThai;
                        vanBanDi.Ower = vanBanDi.GetOwerWithRole(DefaultRoleCode.HIEU_TRUONG);
                        ReplaceOneResult actionResult1
                            =  _context.VanBanDi.ReplaceOne(x => x.Id.Equals(vanBanDi.Id)
                                , vanBanDi
                                , new ReplaceOptions { IsUpsert = true });
                        var result3 = actionResult1.IsAcknowledged && actionResult1.ModifiedCount > 0;
                        if (!result3)
                        {
                            return new ResponseMessage()
                            {
                                ResponseID = Guid.NewGuid(),
                                ResponseCode = 0,
                                ResponseContent = "Thiết lập ký số không thành công"
                            };
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ResponseMessage()
                {
                    ResponseID = Guid.NewGuid(),
                    ResponseCode = 0,
                    ResponseContent = $"Thiết lập ký số không thành công, {e.Message}"
                };
            }
            return new ResponseMessage()
            {
                ResponseID = Guid.NewGuid(),
                ResponseCode = 0,
                ResponseContent = $"Thiết lập ký số không thành công"
            };
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