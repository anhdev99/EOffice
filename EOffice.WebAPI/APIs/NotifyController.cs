using System;
using System.Threading.Tasks;
using EOffice.WebAPI.Helpers;
using EOffice.WebAPI.Interfaces;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOffice.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class NotifyController : ControllerBase
    {
        private readonly INotifyService _notifyService;

        public NotifyController(INotifyService notifyService)
        {
            this._notifyService = notifyService;
        }
        
        [HttpPost]
        [Route("get-paging-params")]
        public async Task<IActionResult> GetPagingParam([FromBody] NotifyParam param)
        {
            ResultResponse<PagingModel<User>> resultResponse = new ResultResponse<PagingModel<User>>();
            try
            {
                var data = await  _notifyService.GetPaging(param);
                return Ok(data);
            }
            catch (Exception ex)
            {
                resultResponse.ResultCode = EResultResponse.ERROR.ToString();
                resultResponse.ResultString = "Lỗi." + ex.Message;
                return Ok(resultResponse);
            }
        }
        
        [HttpGet]
        [Route("get-by-id/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            ResultResponse<CoQuan> resultResponse = new ResultResponse<CoQuan>();
            try
            {
                if (id == default)
                {
                    resultResponse.ResultCode = EResultResponse.FAIL.ToString();
                    resultResponse.ResultString = "Dữ liệu không được trống.";
                    return Ok(resultResponse);
                }

                var data = await _notifyService.GetById(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                resultResponse.ResultCode = EResultResponse.ERROR.ToString();
                resultResponse.ResultString = "Lỗi." + ex.Message;
                return Ok(resultResponse);
            }
        }

        [HttpGet]
        [Route("get-list-notify")]
        public async Task<IActionResult> GetListNotify()
        {
            ResultResponse<Notify> resultResponse = new ResultResponse<Notify>();
            try
            {
                var data = await _notifyService.GetListNotify();
                return Ok(data);
            }
            catch (Exception ex)
            {
                resultResponse.ResultCode = EResultResponse.ERROR.ToString();
                resultResponse.ResultString = "Lỗi." + ex.Message;
                return Ok(resultResponse);
            }
        }
        
        [HttpPost]
        [Route("change-status")]
        public async Task<IActionResult> ChangeStatus([FromBody] NotifyParam model)
        {
            try
            {
                var data = await _notifyService.ChangeStatus(model.Id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(
                    new ResultResponse<String>()
                    {
                        ResultCode = EResultResponse.ERROR.ToString(),
                        ResultString = "Lỗi",
                        Data = $"{ex.Message}"
                    }
                );
                
            }
        }
        
    }
}