using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Services.Exceptions;

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
            // obj.Department = _context.Department.First();
            _context.Add(obj);
            _context.SaveChanges();
        }

        public Seller FindById(int id)
        {
            // Include - joins the tables to obtain the department
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id);
        }

        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id);
            _context.Seller.Remove(obj);
            _context.SaveChanges();
        }

        public void Update(Seller seller)
        {
            // Only update if there is already an id equal to the one provided
            if (!_context.Seller.Any(x => x.Id == seller.Id))
            {
                throw new NotFoundException("Id not found");
            }
            try
            {
                _context.Update(seller);
                _context.SaveChanges();
            }
            // I am intercepting an exception from the data access level 
            // and re-launching that exception but at the service level.
            // Very important to segregate the layers.
            // The controller(SellersControler) will only have to deal with exceptions from the service layer
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }

           
        }
    }
}
