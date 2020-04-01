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
        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        // It will insert a new seller in the database
        public async Task InsertAsync(Seller obj)
        {
            // To search for the first department that exists in the BD
            // It will prevent errors from happening due to lack of the department field when creating the seller
            // obj.Department = _context.Department.First();
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            // Include - joins the tables to obtain the department
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            var obj = await _context.Seller.FindAsync(id);
            _context.Seller.Remove(obj);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Seller seller)
        {
            // Only update if there is already an id equal to the one provided

            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == seller.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id not found");
            }
            try
            {
                _context.Update(seller);
                await _context.SaveChangesAsync();
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
