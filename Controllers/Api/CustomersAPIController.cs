using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WarrantyRegistrationApp.Models;
using WarrantyRegistrationApp.Repository;

namespace WarrantyRegistrationApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersAPIController : ControllerBase
    {
        private readonly IRepository<Customer> _repository;

        public CustomersAPIController(IRepository<Customer> repository)
        {
            _repository = repository;
        }

        // GET: api/CustomersAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _repository.GetAllAsync();
        }

        // GET: api/CustomersAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _repository.GetByIDAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }




        //    // PUT: api/CustomersAPI/5
        //    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            try
            {
                await _repository.UpdateAsync(customer);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await CustomerExists(id))
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

        // POST: api/CustomersAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            await _repository.InsertAsync(customer);
            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/CustomersAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _repository.GetByIDAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _repository.Delete(customer);

            return NoContent();
        }

        private async Task<bool> CustomerExists(int id)
        {
            return await _repository.IsExistsAsync(id);
        }
    }
}
