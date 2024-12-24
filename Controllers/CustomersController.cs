using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Model.Strings;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using WebApp1.Models;

namespace WebApp1.Controllers
{
    public class CustomersController : Controller
    {
        private readonly AsoftContext _context;

        public CustomersController(AsoftContext context)
        {
            _context = context;
        }
        [HttpGet("exists/{id}")]
        public bool CustomerExists(string id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,CustomerName,Phone")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (!CustomerExists(customer.CustomerId))
                {
                    _context.Add(customer);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public bool CreateNew([Bind("CustomerId,CustomerName,Phone")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (!CustomerExists(customer.CustomerId))
                {
                    _context.Add(customer);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public bool Edit([Bind("CustomerId,CustomerName,Phone")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                    return false;
                }
            }
            TempData["ErrorMessage"] = "Tên / SĐT mới không hợp lệ";
            return false;
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<bool> DeleteConfirmed(string id)
        {
            var customer = await _context.Customers.Include(c => c.Invoices).FirstOrDefaultAsync(c => c.CustomerId.Equals(id));
            if (customer != null)
            {
                if (customer.Invoices.Any())
                {
                    TempData["ErrorMessage"] = "Không thể xóa khách hàng đã có hóa đơn";
                    return false;
                }
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
