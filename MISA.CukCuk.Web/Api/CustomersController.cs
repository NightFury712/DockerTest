using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Enums;
using MISA.ApplicationCore.Interfaces;
using MISA.ApplicationCore.Interfaces.Service;

namespace MISA.CukCuk.Web.Api
{
    [ApiController]
    public class CustomersController : BaseApiController<Customer>
    {
        ICustomerService _customerService;
        public CustomersController(ICustomerService customerService):base(customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("customerFilter")]
        public IActionResult Filter(int pageSize, int pageIndex, string customerFilter, Guid? customerGroupId)
        {
            try
            {
                var customers = _customerService.GetCustomerPaging(pageSize, pageIndex, customerFilter, customerGroupId);
                
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, InitExceptionResult(ex));
            }
            
        }

        [HttpGet("CustomerGroups")]
        public IActionResult GetCustomerGroups()
        {
            try
            {
                var customerGroups = _customerService.GetCustomerGroups();
                if (customerGroups.Count() > 0)
                {
                    return Ok(customerGroups);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, InitExceptionResult(ex));
            }
            
        }

        [HttpGet("CustomerGroups/{customerGroupId}")]

        public IActionResult GetCustomerGroupById(Guid customerGroupId)
        {
            try
            {
                var customerGroup = _customerService.GetCustomerGroupById(customerGroupId);

                return Ok(customerGroup);
            }
            catch (Exception ex)
            {
                return StatusCode(500, InitExceptionResult(ex));
            }
        }
    }


}
