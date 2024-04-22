using Microsoft.EntityFrameworkCore;
using Quipu_Task.Helpers;
using Quipu_Task.Models;

namespace Quipu_Task.Service
{
    public interface IClientService 
    {
        ClientInfo GetClientInfo(int? Id);
        IEnumerable<ClientInfo> GetAllClients();
        ClientInfo Create(ClientInfo user);
        ClientInfo Update(ClientInfo user);
        void Delete(ClientInfo user);
        bool ClientInfoExists(int id);
       
    }

    public class ClientService : IClientService
    {
        private DataContext _context;

        public ClientService(DataContext context)
        {
            _context = context;
        }

        public bool ClientInfoExists(int id)
        {
            return _context.clientInfo.Any(e => e.ClientId == id);
        }

        public ClientInfo Create(ClientInfo user)
        {
            _context.Add(user);
            _context.SaveChanges();

            return user;
        }



        public void Delete(ClientInfo user)
        {
            _context.clientInfo.Remove(user);
            _context.SaveChanges();
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

        public ClientInfo Update(ClientInfo user)
        {
            _context.Update(user);
            _context.SaveChanges();

            return user;
        }
    }
}
