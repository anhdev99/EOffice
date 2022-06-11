using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EOffice.WebAPI.Exceptions;
using EOffice.WebAPI.Helpers;
using EOffice.WebAPI.Interfaces;
using EOffice.WebAPI.Models;
using EResultResponse = EOffice.WebAPI.Helpers.EResultResponse;

namespace EOffice.WebAPI.APIs
{
    [Route("api/v1/[controller]")]
    // [Authorize]
    public class EnumController : ControllerBase
    {
        private IDefineStatusService _defineStatusService;

        public  EnumController(IDefineStatusService enumService)
        {
            _defineStatusService = enumService;
        }


        [HttpGet]
        [Route("get-status-question")]
        public async Task<IActionResult> GetStatusQuestion()
        {
            try
            {
                var response = await _defineStatusService.GetStatusQuestion();

                return Ok(
                    new ResultResponse<List<StatusQuestion>>()
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
