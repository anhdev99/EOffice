using System.Collections.Generic;
using System.Threading.Tasks;
using EOffice.WebAPI.Exceptions;
using EOffice.WebAPI.Helpers;
using EOffice.WebAPI.Interfaces;
using EOffice.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using EResultResponse = EOffice.WebAPI.Helpers.EResultResponse;

namespace EOffice.WebAPI.APIs
{
    [Route("api/v1/[controller]")]
    // [Authorize]
    public class EnumController : ControllerBase
    {
        private IEnumService _enumService;

        public  EnumController(IEnumService enumService)
        {
            _enumService = enumService;
            
        }
        
        [HttpGet]
        [Route("get-muc-do")]
        public async Task<IActionResult> GetMucDo()
        {
            try
            {
                var response = await _enumService.GetMucDo();

                return Ok(
                    new ResultResponse<List<EnumModel>>()
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
