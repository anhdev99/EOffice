using System.Linq;
using System.Threading.Tasks;
using EOffice.WebAPI.Extensions;
using EOffice.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EOffice.WebAPI.APIs
{
    [Route("api/v1/[controller]")]
    public class TestController : ControllerBase
    {
        public TestController()
        {
            
        }

        // [HttpGet]
        // [Route("enum")]
        // public async Task<IActionResult> TestEnumGetDisplay()
        // {
        //     var data = EnumExtensions.GetValue(EHinhThucGui, "Thap");
        //     return Ok(new {data,data1}); 
        // }
    }
}