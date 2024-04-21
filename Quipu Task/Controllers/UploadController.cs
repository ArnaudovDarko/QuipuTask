using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quipu_Task.Helpers;
using Quipu_Task.Models;
using System.Text;
using System.Web;
using System.Xml;



namespace Quipu_Task.Controllers
{
    public class UploadController : Controller
    {
        private readonly DataContext _context;
        private IWebHostEnvironment _environment;

        public UploadController (DataContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        [HttpGet]
        public IActionResult UploadFile() 
        {
            return View();
        }


        [HttpPost]
        public ActionResult UploadFile(IFormFile file)
        {

            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }



           
          
            return View();
        }
    }
}
