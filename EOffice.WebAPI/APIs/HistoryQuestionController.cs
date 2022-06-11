using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EOffice.WebAPI.Exceptions;
using EOffice.WebAPI.Helpers;
using EOffice.WebAPI.Interfaces;
using EOffice.WebAPI.Models;
using EOffice.WebAPI.Services;
using EResultResponse = EOffice.WebAPI.Helpers.EResultResponse;

namespace EOffice.WebAPI.APIs
{
    [Route("api/v1/[controller]")]
    public class HistoryQuestionController : ControllerBase
    {
        private HistoryQuestionService _questionService;
        public HistoryQuestionController(HistoryQuestionService questionService)
        {
            _questionService = questionService;
        }
        
        [HttpGet]
        [Route("get-by-question-id/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var response = await _questionService.GetHistoryByQuestionId(id);

                return Ok(
                    new ResultResponse<List<HistoryQuestion>>()
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