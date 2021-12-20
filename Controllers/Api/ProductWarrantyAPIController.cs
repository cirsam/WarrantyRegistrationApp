using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarrantyRegistrationApp.Models;

namespace WarrantyRegistrationApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductWarrantyAPIController : ControllerBase
    {
        private readonly WarrantyDataContext _context;

        public ProductWarrantyAPIController(WarrantyDataContext context)
        {
            _context = context;
        }

        // GET: api/ProductWarrantyAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductWarrantyData>>> GetProductWarrantyDatas()
        {
            return await _context.ProductWarrantyDatas.ToListAsync();
        }

        // GET: api/ProductWarrantyAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductWarrantyData>> GetProductWarrantyData(int id)
        {
            var productWarrantyData = await _context.ProductWarrantyDatas.FindAsync(id);

            if (productWarrantyData == null)
            {
                return NotFound();
            }

            return productWarrantyData;
        }

        // PUT: api/ProductWarrantyAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductWarrantyData(int id, ProductWarrantyData productWarrantyData)
        {
            if (id != productWarrantyData.ProdWarrantyId)
            {
                return BadRequest();
            }

            _context.Entry(productWarrantyData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductWarrantyDataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductWarrantyAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductWarrantyData>> PostProductWarrantyData(ProductWarrantyData productWarrantyData)
        {
            _context.ProductWarrantyDatas.Add(productWarrantyData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductWarrantyData", new { id = productWarrantyData.ProdWarrantyId }, productWarrantyData);
        }

        // DELETE: api/ProductWarrantyAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductWarrantyData(int id)
        {
            var productWarrantyData = await _context.ProductWarrantyDatas.FindAsync(id);
            if (productWarrantyData == null)
            {
                return NotFound();
            }

            _context.ProductWarrantyDatas.Remove(productWarrantyData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductWarrantyDataExists(int id)
        {
            return _context.ProductWarrantyDatas.Any(e => e.ProdWarrantyId == id);
        }
    }
}
