using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EOffice.WebAPI.Exceptions;
using EOffice.WebAPI.Helpers;
using EOffice.WebAPI.Interfaces;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using EResultResponse = EOffice.WebAPI.Helpers.EResultResponse;

namespace EOffice.WebAPI.APIs
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class VanBanDiController : ControllerBase
    {
        private IVanBanDiService _vanBanDiService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public VanBanDiController(IVanBanDiService vanBanDiService, IWebHostEnvironment hostingEnvironment)
        {
            _vanBanDiService = vanBanDiService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] VanBanDi model)
        {
            try
            {
                var response = await _vanBanDiService.Create(model);
                return Ok(
                    new ResultResponse<VanBanDi>()
                        .WithData(response)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage(DefaultMessage.CREATE_SUCCESS)
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
        
        [HttpPost]
        [Route("assign-or-reject")]
        public async Task<IActionResult> AssignOrReject([FromBody] PhanCongKySo model)
        {
            try
            {
                var uploadDirecotroy = "files/";
                var uploadPath = Path.Combine(_hostingEnvironment.ContentRootPath, uploadDirecotroy);

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);
                var dateTime = DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss");
                var path = Path.Combine(uploadDirecotroy, dateTime);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                
                var response = await _vanBanDiService.AssignOrReject(model, path);
                return Ok(
                    new ResultResponse<VanBanDi>()
                        .WithData(response)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage("Ký số thành công")
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

        
        [HttpPost]
        [Route("them-nguoi-ky-so")]
        public async Task<IActionResult> ThemNguoiKySo([FromBody] PhanCongKySo model)
        {
            try
            {
                var response = await _vanBanDiService.AssignSign(model);
                return Ok(
                    new ResultResponse<VanBanDi>()
                        .WithData(response)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage("Thêm người tham gia ký số.")
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
        
        [HttpPost]
        [Route("xoa-nguoi-ky-so")]
        public async Task<IActionResult> XoaNguoiKySo([FromBody] PhanCongKySo model)
        {
            try
            {
                var response = await _vanBanDiService.RemoveAssignSign(model);
                return Ok(
                    new ResultResponse<VanBanDi>()
                        .WithData(response)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage("Xóa người tham gia ký số!")
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

    
        [HttpGet]
        [Route("get-phancongkyso-by-vanbanid/{id}")]
        public async Task<IActionResult> GetPhanCongKySoByVanBanId(string id)
        {
            try
            {
                var response = await _vanBanDiService.GetPhanCongKySoByVanBanId(id);
                return Ok(
                    new ResultResponse<List<PhanCongKySo>>()
                        .WithData(response)
                    .WithCode(EResultResponse.SUCCESS.ToString())
                    .WithMessage(DefaultMessage.GET_DATA_SUCCESS)
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

        
        [HttpPost]
        [Route("phan-cong")]
        public async Task<IActionResult> PhanCong([FromBody] VanBanDi model)
        {
            try
            {
                var response = await _vanBanDiService.PhanCong(model);
                return Ok(
                    new ResultResponse<VanBanDi>()
                        .WithData(response)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage(DefaultMessage.CREATE_SUCCESS)
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

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] VanBanDi model)
        {
            try
            {
                var response = await _vanBanDiService.Update(model);

                return Ok(
                    new ResultResponse<VanBanDi>()
                        .WithData(response)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage(DefaultMessage.UPDATE_SUCCESS)
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

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _vanBanDiService.Delete(id);

                return Ok(
                    new ResultMessageResponse()
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage(DefaultMessage.DELETE_SUCCESS)
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

        [HttpGet]
        [Route("get-by-id/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var response = await _vanBanDiService.GetById(id);

                return Ok(
                    new ResultResponse<VanBanDi>()
                        .WithData(response)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage(DefaultMessage.GET_DATA_SUCCESS)
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

        // [HttpGet]
        // [Route("get-phancongkyso-by-vanbanid/{id}")]
        // public async Task<IActionResult> RemoveAssignSign(PhanCongKySo model)
        // {
        //     try
        //     {
        //         var response = await _vanBanDiService.RemoveAssignSign(model);
        //
        //         return Ok(
        //             new ResultResponse<VanBanDi>()
        //                 .WithData(response)
        //                 .WithCode(EResultResponse.SUCCESS.ToString())
        //                 .WithMessage("Xóa ký số thành công")
        //         );
        //     }
        //     catch (ResponseMessageException ex)
        //     {
        //         return Ok(
        //             new ResultMessageResponse().WithCode(ex.ResultCode)
        //                 .WithMessage(ex.ResultString)
        //         );
        //     }
        // }
        //
        //
        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _vanBanDiService.Get();

                return Ok(
                    new ResultResponse<List<VanBanDi>>()
                        .WithData(response)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage(DefaultMessage.GET_DATA_SUCCESS)
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

        [HttpPost]
        [Route("get-paging-params")]
        public async Task<IActionResult> GetPagingParam([FromBody] VanBanDiParam param)
        {
            try
            {
                var response = await _vanBanDiService.GetPaging(param);

                return Ok(
                    new ResultResponse<PagingModel<VanBanDi>>()
                        .WithData(response)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage(DefaultMessage.GET_DATA_SUCCESS)
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
        
        [HttpPost]
        [Route("get-paging-params-user")]
        public async Task<IActionResult> GetPagingParamUser([FromBody] VanBanDiParam param)
        {
            try
            {
                var response = await _vanBanDiService.GetPagingUser(param);

                return Ok(
                    new ResultResponse<PagingModel<VanBanDi>>()
                        .WithData(response)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage(DefaultMessage.GET_DATA_SUCCESS)
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