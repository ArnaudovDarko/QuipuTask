﻿using Microsoft.EntityFrameworkCore;
using Quipu_Task.Helpers;
using Quipu_Task.Models;

namespace Quipu_Task.Service
{
    public interface IClientService 
    {
        ClientInfo GetClientInfo(int? Id);

        IEnumerable<ClientInfo> GetAllClients();
    }

    public class ClientService : IClientService
    {
        private DataContext _context;

        public ClientService(DataContext context)
        {
            _context = context;
        }



        public IEnumerable<ClientInfo> GetAllClients()
        {
            return from s in _context.clientInfo
                   select s;
        }

        public  ClientInfo GetClientInfo(int? Id)
        {
            return _context.clientInfo.FirstOrDefault(m => m.ClientId == Id);
        }
    }
}