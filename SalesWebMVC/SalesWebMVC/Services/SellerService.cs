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
            // To search for the first department that exists in the BD
            // It will prevent errors from happening due to lack of the department field when creating the seller
            obj.Department = _context.Department.First();
            _context.Add(obj);
            _context.SaveChanges();
        }
    }
}
