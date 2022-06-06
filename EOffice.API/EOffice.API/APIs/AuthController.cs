using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOffice.API.APIs
{
	[Route("api/v1/[controller]")]
	public class AuthController : ControllerBase
	{

		public AuthController()
		{
		}
		[HttpPost]
		[Route("Login")]
		public async Task<IActionResult> Login()
        {
			return Ok("login");
		}
	}
}

