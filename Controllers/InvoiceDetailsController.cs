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
    public class InvoiceDetailsController : Controller
    {
        private readonly AsoftContext _context;

        public InvoiceDetailsController(AsoftContext context)
        {
            _context = context;
        }
        // GET: InvoiceDetails/Create
        [HttpGet]
        public async Task<IActionResult> Create(string invoiceId)
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            ViewData["InvoiceId"] = invoiceId;
            return View();
        }

        // POST: InvoiceDetails/Create
        [HttpPost]
        
        public async Task<IActionResult> Create([Bind("InvoiceId", "ProductId", "Quantity", "TotalPrice")]
                                                InvoiceDetail invoiceDetail)
        {
            try
            {
                var check = _context.InvoiceDetails.FirstOrDefault(idl => idl.ProductId.Equals(invoiceDetail.ProductId) &&
                                                                        idl.InvoiceId.Equals(invoiceDetail.InvoiceId));
                if (check != null)
                {
                    check.Quantity += invoiceDetail.Quantity;
                    check.TotalPrice = invoiceDetail.TotalPrice;
                    _context.Update(check);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Invoices", new { id = invoiceDetail.InvoiceId });
                }
                _context.Add(invoiceDetail);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return RedirectToAction("Details", "Invoices", new { id = invoiceDetail.InvoiceId });
        }
        public async Task<bool> CreateNew([Bind("InvoiceId", "ProductId", "Quantity", "TotalPrice")]
                                           InvoiceDetail invoiceDetail)
        {
            try
            {
                var check = _context.InvoiceDetails.FirstOrDefault(idl => idl.ProductId.Equals(invoiceDetail.ProductId) &&
                                                                        idl.InvoiceId.Equals(invoiceDetail.InvoiceId));
                if (check != null)
                {
                    check.Quantity += invoiceDetail.Quantity;
                    _context.Update(check);
                    await _context.SaveChangesAsync();
                    return true;
                }
                _context.Add(invoiceDetail);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }
        // GET: InvoiceDetails/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var invoiceDetail = await _context.InvoiceDetails
                                              .Include(idl => idl.Product)
                                              .FirstOrDefaultAsync(idl => idl.InvoiceDetailId.Equals(id));
            if (invoiceDetail == null)
            {
                return NotFound();
            }
            return View(invoiceDetail);
        }


        // POST: InvoiceDetails/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (int id, int quantity)
        {
            if (id == null || quantity == 0)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var invoiceDetail = await _context.InvoiceDetails
                                              .Include(idl => idl.Product)
                                              .FirstOrDefaultAsync(idl => idl.InvoiceDetailId.Equals(id));
                    if (invoiceDetail == null)
                    {
                        return NotFound();
                    }
                    invoiceDetail.Quantity = quantity;
                    invoiceDetail.TotalPrice = quantity * invoiceDetail.Product.Price;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Invoices", new { id = invoiceDetail.InvoiceId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceDetailExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return NotFound();
        }

        // GET: /InvoiceDetails/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var invoiceDetail = await _context.InvoiceDetails
                                              .Include(idl => idl.Product)
                                              .FirstOrDefaultAsync(idl => idl.InvoiceDetailId.Equals(id));
            if (invoiceDetail == null)
            {
                return NotFound();
            }
            return View(invoiceDetail);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoiceDetail = await _context.InvoiceDetails.FindAsync(id);
            if (invoiceDetail != null)
            {
                _context.InvoiceDetails.Remove(invoiceDetail);
            }
            await _context.SaveChangesAsync();
            var invoice = await _context.Invoices.FindAsync(invoiceDetail.InvoiceId);
            if (invoice.InvoiceDetails.Count == 0)
            {
                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Invoices");
            }
            return RedirectToAction("Details", "Invoices", new { id = invoiceDetail.InvoiceId });
        }

        private bool InvoiceDetailExists(int id)
        {
            return _context.InvoiceDetails.Any(e => e.InvoiceDetailId == id);
        }
    }
}
