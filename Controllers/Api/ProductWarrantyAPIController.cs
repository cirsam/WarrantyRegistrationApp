using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarrantyRegistrationApp.Models;
using WarrantyRegistrationApp.Repository;

namespace WarrantyRegistrationApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductWarrantyAPIController : ControllerBase
    {
        private readonly IRepository<ProductWarrantyData> _repository;
        private readonly IRepository<Customer> _repositoryCustomer;
        private readonly IRepository<Product> _repositoryProduct;

        public ProductWarrantyAPIController(IRepository<ProductWarrantyData> repository, IRepository<Customer> repositoryCustomer, IRepository<Product> repositoryProduct)
        {
            _repository = repository;
            _repositoryCustomer = repositoryCustomer;
            _repositoryProduct = repositoryProduct;
        }

        // GET: api/ProductWarrantyAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductWarrantyData>>> GetProductWarrantyDatas()
        {
            return await _repository.GetAllAsync();
        }


        // GET: api/ProductWarrantyAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductWarrantyData>> GetProductWarrantyData(int id)
        {
            var productWarrantyData = await _repository.GetByIDAsync(id);

            if (productWarrantyData == null)
            {
                return NotFound();
            }

            return productWarrantyData;
        }

        public async Task<bool> IsProductWarrantyValid(ProductWarrantyData productWarrantyData)
        {
            var productData = await _repositoryProduct.GetBySerialNumberAsync(productWarrantyData.ProductSerialNumber);//validate serial number
            var customerData = await _repositoryCustomer.GetByIDAsync(productWarrantyData.CustomerId);//validate customer ID
            var prodWarrantyData = await _repository.GetBySerialNumberAsync(productWarrantyData.ProductSerialNumber);//validate unique serial number

            if (productData != null && customerData != null && prodWarrantyData == null)
            {
                return true;
            }

            //if it gets here it mean the product is not warrantable
            return false;
        }

        //// PUT: api/ProductWarrantyAPI/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutProductWarrantyData(int id, ProductWarrantyData productWarrantyData)
        //{
        //    if (id != productWarrantyData.ProdWarrantyId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(productWarrantyData).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProductWarrantyDataExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/ProductWarrantyAPI
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<ProductWarrantyData>> PostProductWarrantyData(ProductWarrantyData productWarrantyData)
        //{
        //    _context.ProductWarrantyDatas.Add(productWarrantyData);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetProductWarrantyData", new { id = productWarrantyData.ProdWarrantyId }, productWarrantyData);
        //}

        //// DELETE: api/ProductWarrantyAPI/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteProductWarrantyData(int id)
        //{
        //    var productWarrantyData = await _context.ProductWarrantyDatas.FindAsync(id);
        //    if (productWarrantyData == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.ProductWarrantyDatas.Remove(productWarrantyData);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool ProductWarrantyDataExists(int id)
        //{
        //    return _context.ProductWarrantyDatas.Any(e => e.ProdWarrantyId == id);
        //}
    }
}
