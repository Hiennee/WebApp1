using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using WebApp1.Models;

namespace WebApp1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AsoftContext _context;

        public ProductsController(AsoftContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (ProductExists(product.ProductId))
                {
                    return RedirectToAction(nameof(Index));
                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        public bool CreateNew([Bind("ProductId, ProductName, Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (!ProductExists(product.ProductId))
                {
                    _context.Add(product);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<bool> Edit([Bind("ProductId,ProductName,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<bool> DeleteConfirmed([Bind("id")] string id)
        {
            var product = await _context.Products.Include(p => p.InvoiceDetails).FirstOrDefaultAsync(p => p.ProductId.Equals(id));
            if (product != null)
            {
                if (product.InvoiceDetails.Any())
                {
                    ViewData["ErrorMessage"] = "Không thể xóa sản phẩm đã tồn tại trong các hóa đơn";
                    return false;
                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public bool ProductExists(string id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
        [HttpGet]
        public double GetProductPrice(string id)
        {
            return _context.Products.Find(id).Price;
        }
        [HttpGet]
        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
