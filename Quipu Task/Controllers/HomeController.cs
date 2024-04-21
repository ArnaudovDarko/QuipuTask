using Microsoft.AspNetCore.Mvc;
using Quipu_Task.Models;
using System.Diagnostics;

namespace Quipu_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
     

        public HomeController()
        {
           
        }

        public IActionResult Index()
        {
            return Ok();
        }

        

       
    }
}
