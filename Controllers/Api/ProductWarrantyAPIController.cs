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
    public class ProductWarrantyAPIController : ControllerBase, IProductWarrantyAPIController
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

        // POST: api/ProductWarrantyAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductWarrantyData>> RegisterNewProductWarranty(ProductWarrantyData productWarrantyData)
        {
            if (await IsProductWarrantyValid(productWarrantyData))
            {
                productWarrantyData.WarrantyDate = DateTime.Now.AddYears(5);
                await _repository.InsertAsync(productWarrantyData);
            }
            else
            {
                return NoContent();
            }

            return CreatedAtAction("GetProductWarrantyData", new { id = productWarrantyData.ProdWarrantyId }, productWarrantyData);
        }

        // PUT: api/ProductWarrantyAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> ExtendRegistedProductWarranty(int id, ProductWarrantyData productWarrantyData)
        {

            if (id != productWarrantyData.CustomerId)
            {
                return BadRequest();
            }

            try
            {
                var prodWarrantyData = await _repository.GetBySerialNumberAsync(productWarrantyData.ProductSerialNumber);//validate serial number exists
                
                if (prodWarrantyData!=null && productWarrantyData.ProductSerialNumber == prodWarrantyData.FirstOrDefault().ProductSerialNumber) {
                    productWarrantyData.WarrantyDate = productWarrantyData.WarrantyDate.AddYears(2);
                    await _repository.UpdateAsync(productWarrantyData);

                    return CreatedAtAction("GetProductWarrantyData", new { id = productWarrantyData.ProdWarrantyId }, productWarrantyData);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await WarrantyItemIsExists(id))
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

        private async Task<bool> WarrantyItemIsExists(int id)
        {
            return await _repository.IsExistsAsync(id);
        }
    }
}
