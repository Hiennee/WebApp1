using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp1.Models;

namespace WebApp1.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly AsoftContext _context;

        public InvoicesController(AsoftContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var invoiceList = _context.Invoices.Include(i => i.Customer);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerName");
            return View(await invoiceList.ToListAsync());
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.InvoiceDetails)
                .ThenInclude(id => id.Product)
                .FirstOrDefaultAsync(m => m.InvoiceId == id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["ProductList"] = new SelectList(_context.Products.ToList(), "ProductId", "ProductName");
            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerName");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<bool> Create([Bind("InvoiceId,CustomerId,InvoiceDate")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                if (InvoiceExists(invoice.InvoiceId))
                {
                    return false;
                }
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        [HttpPost]
        public bool CreateNew([Bind("InvoiceId,CustomerId,InvoiceDate")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                if (!CustomerExists(invoice.Customer.CustomerId))
                {
                    _context.Add(invoice);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<bool> Edit([Bind("InvoiceId,CustomerId,InvoiceDate")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    invoice.InvoiceDetails = new List<InvoiceDetail>();
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.InvoiceId))
                    {
                        return false;
                    }
                    else
                    {
                        throw;
                    }
                }
                return true;
            }
            return false;
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(m => m.InvoiceId == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<bool> DeleteConfirmed([Bind("id")] string id)
        {
            var invoice = await _context.Invoices.Include(i => i.InvoiceDetails).FirstOrDefaultAsync(i => i.InvoiceId.Equals(id));
            if (invoice != null && !invoice.InvoiceDetails.Any())
            {
                _context.Invoices.Remove(invoice);
            }
            else
            {
                return false;
            }
            await _context.SaveChangesAsync();
            return true;
        }
        public bool CustomerExists(string id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
        public bool InvoiceExists(string id)
        {
            return _context.Invoices.Any(e => e.InvoiceId == id);
        }
        [HttpGet("[controller]/getInvDel/{id}")]
        public async Task<List<InvoiceDetail>> GetInvoiceDetailsOfInvoice(string id)
        {
            return await _context.InvoiceDetails.Where(idl => idl.InvoiceId.Equals(id))
                                                .Include(idl => idl.Product)
                                                .Select(idl => new InvoiceDetail
                                                {
                                                    InvoiceDetailId = idl.InvoiceDetailId,
                                                    InvoiceId = idl.InvoiceId,
                                                    ProductId = idl.ProductId,
                                                    Quantity = idl.Quantity,
                                                    TotalPrice = idl.TotalPrice,
                                                    Product = idl.Product,
                                                })
                                                .ToListAsync();
        }
    }
}
