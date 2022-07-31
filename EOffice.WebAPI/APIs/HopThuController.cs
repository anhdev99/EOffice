using System.Threading.Tasks;
using EOffice.WebAPI.Exceptions;
using EOffice.WebAPI.Helpers;
using EOffice.WebAPI.Interfaces;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EResultResponse = EOffice.WebAPI.Helpers.EResultResponse;

namespace EOffice.WebAPI.APIs
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class HopThuController : ControllerBase
    {
        private IHopThuService _hopThuService;

        public HopThuController(IHopThuService hopThuService)
        {
            _hopThuService = hopThuService;
        }

        [HttpPost]
        [Route("create-send")]
        public async Task<IActionResult> Create([FromBody] HopThu model)
        {
            try
            {
                var response = await _hopThuService.Create(model);
                return Ok(
                    new ResultResponse<dynamic>()
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
        [Route("get-paging-params")]
        public async Task<IActionResult> GetPagingParam([FromBody] HopThuParam param)
        {
            try
            {
                var response = await _hopThuService.GetPaging(param);

                return Ok(
                    new ResultResponse<dynamic>()
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