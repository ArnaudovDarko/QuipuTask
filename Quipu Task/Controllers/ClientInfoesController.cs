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
using NuGet.Protocol;
using Quipu_Task.Helpers;
using Quipu_Task.Models;

namespace Quipu_Task.Controllers
{
    public class ClientInfoesController : Controller
    {
        private readonly DataContext _context;
        private IWebHostEnvironment _environment;

        public ClientInfoesController(DataContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index(string sortOrder)
        {

            ViewData["FirstNameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "firstname_desc" : "";
            ViewData["DateBirthSortParam"] = sortOrder == "DateBirth" ? "datebirth_desc" : "DateBirth";
            var clients = from s in _context.clientInfo
                           select s;
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

        //public async Task<IActionResult> Upload()
        //{
        //    ClientInfo clients = new ClientInfo();

        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(string.Concat(this._environment.WebRootPath, "/clients.xml"));

        //    XmlNodeList clientNodes = doc.SelectNodes("/Clients/Client");
        //    if (clientNodes != null)
        //    {
        //        foreach (XmlNode clientNode in clientNodes)
        //        {
        //            XmlClientsImport client = new XmlClientsImport();
        //            client.ID = int.Parse(clientNode.Attributes["ID"].Value);
        //            client.Name = clientNode.SelectSingleNode("Name").InnerText;

        //            XmlNodeList addressNodes = clientNode.SelectNodes("Address");
        //            if (addressNodes != null)
        //            {
        //                client.Addresses = new List<string>();
        //                foreach (XmlNode addressNode in addressNodes)
        //                {
        //                    client.Addresses.Add(addressNode.InnerText);
        //                }
        //            }

        //            client.BirthDate = DateOnly.Parse(clientNode.SelectSingleNode("BirthDate").InnerText);

        //            //clients.ClientId = client.ID;
        //            clients.FirstName = client.Name;
        //            clients.HomeAddress = client.Addresses[0];
        //            clients.DateBirth = client.BirthDate;


        //            _context.Add(clients);
        //            await _context.SaveChangesAsync();
        //        }
        //    }

        //    return View(await _context.clientInfo.ToListAsync());
        //}
       
        public IActionResult ExportCSV()
        {

            var sortOrder = TempData["Value"];
            if (sortOrder != null)
            {
                TempData["Value"] = "";
            }

            var clients = from s in _context.clientInfo
                          select s;

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

            var clientInfo = await _context.clientInfo
                .FirstOrDefaultAsync(m => m.ClientId == id);
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
                _context.Add(clientInfo);
                await _context.SaveChangesAsync();
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

            var clientInfo = await _context.clientInfo.FindAsync(id);
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
                try
                {
                    _context.Update(clientInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientInfoExists(clientInfo.ClientId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(clientInfo);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientInfo = await _context.clientInfo
                .FirstOrDefaultAsync(m => m.ClientId == id);
            if (clientInfo == null)
            {
                return NotFound();
            }

            return View(clientInfo);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clientInfo = await _context.clientInfo.FindAsync(id);
            if (clientInfo != null)
            {
                _context.clientInfo.Remove(clientInfo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientInfoExists(int id)
        {
            return _context.clientInfo.Any(e => e.ClientId == id);
        }
    }
}
