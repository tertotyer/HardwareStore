using HardwareStore.Data;
using HardwareStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace HardwareStore.Services
{
    public class GetDataService
    {
        private readonly ApplicationDbContext _context;

        public GetDataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Entity> GetEntities()
        {
            var data = _context.Entity.ToList();
            return data;
        }
    }
}
