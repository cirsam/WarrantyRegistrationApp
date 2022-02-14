using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarrantyRegistrationApp.Models;

namespace WarrantyRegistrationApp.Controllers.Api
{
    public interface IProductWarrantyAPIController
    {
        Task<string> IsProductWarrantyValid(ProductWarrantyData productWarrantyData);
    }
}
