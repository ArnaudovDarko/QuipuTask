using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NuGet.Protocol;
using Quipu_Task.Helpers;
using Quipu_Task.Models;
using Quipu_Task.Service;

namespace Quipu_Task.Controllers
{
    public class ClientInfoesController : Controller
    {

        private IWebHostEnvironment _environment;
        private IClientService _ClientService;

        public ClientInfoesController( IWebHostEnvironment environment, IClientService clientService)
        {
            _environment = environment;
            _ClientService = clientService; 
        }

        public async Task<IActionResult> Index(string sortOrder)
        {

            ViewData["FirstNameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "firstname_desc" : "";
            ViewData["DateBirthSortParam"] = sortOrder == "DateBirth" ? "datebirth_desc" : "DateBirth";
            var clients = _ClientService.GetAllClients();
            switch (sortOrder)
            {
                case "firstname_desc":
                    clients = clients.OrderByDescending(s => s.FirstName);
                    TempData["Value"] = "firstname_desc";
                    break;
                case "DateBirth":
                    clients = clients.OrderBy(s => s.DateBirth);
                    TempData["Value"] = "DateBirth";
                    break;
                case "datebirth_desc":
                    clients = clients.OrderByDescending(c => c.DateBirth);
                    TempData["Value"] = "datebirth_desc";
                    break;
                default:
                    clients = clients.OrderBy(c => c.FirstName);
                    break;
            }
            return View(clients);
        }
        public IActionResult ExportCSV()
        {

            var sortOrder = TempData["Value"];
            if (sortOrder != null)
            {
                TempData["Value"] = "";
            }

            var clients = _ClientService.GetAllClients();

            switch (sortOrder)
            {
                case "firstname_desc":
                    clients = clients.OrderByDescending(s => s.FirstName);
                    break;
                case "DateBirth":
                    clients = clients.OrderBy(s => s.DateBirth);
                    break;
                case "datebirth_desc":
                    clients = clients.OrderByDescending(c => c.DateBirth);
                    break;
                default:
                    clients = clients.OrderBy(c => c.FirstName);
                    break;
            }  
            string jsonString = JsonSerializer.Serialize(clients);
            var fileName = "Clients.txt";
            var mimeType = "text/plain";
            var fileBytes = Encoding.ASCII.GetBytes(jsonString);
            return new FileContentResult(fileBytes, mimeType)
            {
                FileDownloadName = fileName
            };
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientInfo = _ClientService.GetClientInfo(id);

            if (clientInfo == null)
            {
                return NotFound();
            }

            return View(clientInfo);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientId,FirstName,LastName,Email,HomeAddress,HomeAddress2,DateBirth")] ClientInfo clientInfo)
        {
            if (ModelState.IsValid)
            {
                _ClientService.Create(clientInfo);
                return RedirectToAction(nameof(Index));
            }
            return View(clientInfo);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientInfo = _ClientService.GetClientInfo(id);
            if (clientInfo == null)
            {
                return NotFound();
            }
            return View(clientInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClientId,FirstName,LastName,Email,HomeAddress,HomeAddress2,DateBirth")] ClientInfo clientInfo)
        {
            if (id != clientInfo.ClientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_ClientService.ClientInfoExists(id))
                {
                    _ClientService.Update(clientInfo);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(clientInfo);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientInfo = _ClientService.GetClientInfo(id);
            if (clientInfo == null)
            {
                return NotFound();
            }

            return View(clientInfo);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var clientInfo =  _ClientService.GetClientInfo(id);
            if (clientInfo != null)
            {
                _ClientService.Delete(clientInfo);
            }            
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("api/getclientcount")]
        public async Task<IActionResult> GetUsersCount()
        {
            try
            {
                var userCount = _ClientService.GetAllClients().Count();
                return Ok(new { Count = userCount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
      
    }
}
