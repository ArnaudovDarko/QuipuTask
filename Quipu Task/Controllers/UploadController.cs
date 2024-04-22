﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quipu_Task.Helpers;
using Quipu_Task.Models;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;



namespace Quipu_Task.Controllers
{
    public class UploadController : Controller
    {
        private readonly DataContext _context;
        private IWebHostEnvironment _environment;

        public UploadController(DataContext context, IWebHostEnvironment environment)
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
            if (file == null || file.Length <= 0)
            {
                ViewBag.Message = "Внесете фајл";
                return View();
            }
            var clients = new List<ClientInfo>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {            
                XDocument xmlDoc = XDocument.Load(reader);
                foreach (XElement clientElement in xmlDoc.Descendants("Client"))
                {
                    var client = new ClientInfo
                    {
                        FirstName = clientElement.Element("Name")?.Value,
                        LastName = "", 
                        Email = "", 
                        HomeAddress = clientElement.Element("Address")?.Element("HomeAddress")?.Value,
                        HomeAddress2 = clientElement.Element("Address")?.Element("HomeAddress2")?.Value,
                        DateBirth = DateOnly.Parse(clientElement.Element("BirthDate")?.Value)
                    };

                    clients.Add(client);
                    _context.Add(client);
                    _context.SaveChanges();
                }
            }          
            return View(clients);
        }
    }
}
