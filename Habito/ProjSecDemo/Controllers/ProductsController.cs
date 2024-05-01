using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjSecDemo.Data;
using ProjSecDemo.Data.Migrations;
using ProjSecDemo.Models;

namespace ProjSecDemo.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin, Manager_Pantalon, Manager_Veste, Manager_Sous-vetêments")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Titre,Description,Fabricant,InfoSup,Prix_Vente,Type")] Data.Product product)
        {
            if (ModelState.IsValid)
            {
                if ((User.IsInRole("Manager_Pantalon") && product.Type == "Pantalon") ||
         (User.IsInRole("Manager_Veste") && product.Type == "Veste") ||
         (User.IsInRole("Manager_Sous-vetêments") && product.Type == "Sous-vetêments") ||
         (User.IsInRole("Admin")))
                {
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // If user doesn't have necessary roles, return 403 Forbidden
                    return Forbid();
                }
            }
            return View(product);
        }

        // GET: Products/Edit/5
        
        public async Task<IActionResult> Edit(int? id)
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

            var productEdit = new ProductEditViewModel()
            {
                id = product.id,
                Titre = product.Titre,
                Description = product.Description,
                Fabricant = product.Fabricant,
                InfoSup = product.InfoSup,
                Prix_Vente = product.Prix_Vente,
                Type = product.Type,
                DroitEdit = User.Identity?.IsAuthenticated ?? false
            };
            var isConnected = false;
            if (User.Identity != null)
            {
                isConnected = User.Identity.IsAuthenticated;
            }
            if ((User.IsInRole("Manager_Pantalon") && product.Type == "Pantalon") ||
        (User.IsInRole("Manager_Veste") && product.Type == "Veste") ||
        (User.IsInRole("Manager_Sous-vetêments") && product.Type == "Sous-vetêments") ||
        (User.IsInRole("Admin")))
            { return View(productEdit); }
             return Forbid();
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Titre,Description,Fabricant,InfoSup,Prix_Vente,Type")] Data.Product product)
        {

                
            if (id != product.id)
            {
                return NotFound();
            }
         

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.id == id);
            if (product == null)
            {
                return NotFound();
            }

            if ((User.IsInRole("Manager_Pantalon") && product.Type == "Pantalon") ||
        (User.IsInRole("Manager_Veste") && product.Type == "Veste") ||
        (User.IsInRole("Manager_Sous-vetêments") && product.Type == "Sous-vetêments") ||
        (User.IsInRole("Admin")))
            {
                return View(product);
            }
            else
            {
                // If user doesn't have necessary roles, return 403 Forbidden
                return Forbid();
            }
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                if ((User.IsInRole("Manager_Pantalon") && product.Type == "Pantalon") ||
         (User.IsInRole("Manager_Veste") && product.Type == "Veste") ||
         (User.IsInRole("Manager_Sous-vetêments") && product.Type == "Sous-vetêments") ||
         (User.IsInRole("Admin")))
                {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        else
        {
            // If user doesn't have necessary roles, return 403 Forbidden
            return Forbid();
        }
            }

            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.id == id);
        }
    }
}
