using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMVC.Data;
using SalesWebMVC.Models;

namespace SalesWebMVC.Services
{
    public class SellerService
    {
        // to prevent dependency from being changed
        private readonly SalesWebMVCContext _context;

        public SellerService(SalesWebMVCContext context)
        {
            _context = context;
        }

        // To return a list of all sellers
        // Synchronous operation (wait until it is done)
        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }

        // It will insert a new seller in the database
        public void Insert(Seller obj)
        {
            _context.Add(obj);
            _context.SaveChanges();
        }
    }
}
