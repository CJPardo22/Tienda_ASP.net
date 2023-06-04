using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ventas1.Models;

namespace Ventas1.Controllers
{
    public class FacturasController : Controller
    {
        private readonly Venta1Context _context;

        public FacturasController(Venta1Context context)
        {
            _context = context;
        }

        // GET: Facturas
        public async Task<IActionResult> Index()
        {
            var venta1Context = _context.Facturas.Include(f => f.CedulaNavigation).Include(f => f.ProductoNavigation);
            return View(await venta1Context.ToListAsync());
        }

        // GET: Facturas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Facturas == null)
            {
                return NotFound();
            }

            var factura = await _context.Facturas
                .Include(f => f.CedulaNavigation)
                .Include(f => f.ProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdFactura == id);
            if (factura == null)
            {
                return NotFound();
            }

            return View(factura);
        }

        // GET: Facturas/Create
        public IActionResult Create()
        {
            ViewData["Cedula"] = new SelectList(_context.Clientes, "Cedula","NombreCedula");
            ViewData["Producto"] = new SelectList(_context.Productos, "Codigo", "NombreProducto");
            return View();
        }

        // POST: Facturas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFactura,Fecha,Cantidad,Total,Cedula,Producto")] Factura factura)
        {
            if (ModelState.IsValid)
            {
                // Obtener el producto desde la base de datos
                var producto = await _context.Productos.FindAsync(factura.Producto);
               
                if (producto == null)
                {
                    return NotFound();
                }

                // Verificar si hay suficiente cantidad de productos disponibles
                if (factura.Cantidad > producto.Cantidad)
                {
                    ModelState.AddModelError("Cantidad", "No hay suficientes productos disponibles.");
                    ViewData["Cedula"] = new SelectList(_context.Clientes, "Cedula", "Cedula", factura.Cedula);
                    ViewData["Producto"] = new SelectList(_context.Productos, "Codigo", "Codigo", factura.Producto);
                    return View(factura);
                }

                // Calcular el total multiplicando el precio por la cantidad
                factura.Total = producto.Precio * factura.Cantidad;

                // Actualizar la cantidad de productos disponibles
                producto.Cantidad -= factura.Cantidad;

                // Agregar la factura al contexto y guardar en la base de datos
                _context.Add(factura);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["Cedula"] = new SelectList(_context.Clientes, "Cedula", "Cedula", factura.Cedula);
            ViewData["Producto"] = new SelectList(_context.Productos, "Codigo", "Codigo", factura.Producto);
            return View(factura);

        }

        public IActionResult GetPrecioProducto(int productoId)
        {
            var producto = _context.Productos.FirstOrDefault(p => p.Codigo == productoId);
            if (producto != null)
            {
                return Ok(producto.Precio);
            }
            return NotFound();
        }

        // GET: Facturas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Facturas == null)
            {
                return NotFound();
            }

            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null)
            {
                return NotFound();
            }
            ViewData["Cedula"] = new SelectList(_context.Clientes, "Cedula", "Cedula", factura.Cedula);
            ViewData["Producto"] = new SelectList(_context.Productos, "Codigo", "Codigo", factura.Producto);
            return View(factura);
        }

        // POST: Facturas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdFactura,Fecha,Cantidad,Total,Cedula,Producto")] Factura factura)
        {
            if (id != factura.IdFactura)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(factura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacturaExists(factura.IdFactura))
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
            ViewData["Cedula"] = new SelectList(_context.Clientes, "Cedula", "Cedula", factura.Cedula);
            ViewData["Producto"] = new SelectList(_context.Productos, "Codigo", "Codigo", factura.Producto);
            return View(factura);
        }

        // GET: Facturas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Facturas == null)
            {
                return NotFound();
            }

            var factura = await _context.Facturas
                .Include(f => f.CedulaNavigation)
                .Include(f => f.ProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdFactura == id);
            if (factura == null)
            {
                return NotFound();
            }

            return View(factura);
        }

        // POST: Facturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Facturas == null)
            {
                return Problem("Entity set 'Venta1Context.Facturas'  is null.");
            }
            var factura = await _context.Facturas.FindAsync(id);
            if (factura != null)
            {
                _context.Facturas.Remove(factura);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacturaExists(int id)
        {
          return (_context.Facturas?.Any(e => e.IdFactura == id)).GetValueOrDefault();
        }
    }
}
