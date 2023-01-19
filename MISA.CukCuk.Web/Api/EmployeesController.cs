using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using MySqlConnector;
using System.Data;
using Microsoft.Extensions.Configuration;
using MISA.ApplicationCore.Interfaces;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Enums;
using MISA.ApplicationCore.Interfaces.Service;

namespace MISA.CukCuk.Web.Api
{

    [ApiController]
    public class EmployeesController : BaseApiController<Employee>
    {
        IEmployeeService _employeeService;

        #region Constructor
        public EmployeesController(IEmployeeService employeeService):base(employeeService)
        {
            _employeeService = employeeService;
        }
        #endregion

        [HttpGet("employeeFilter")]
        public IActionResult Filter(int pageSize, int pageIndex, string employeeFilter, Guid? departmentId, Guid? positionId)
        {
            try
            {
                var employees = _employeeService.GetEmployeePaging(pageSize, pageIndex, employeeFilter, departmentId, positionId);

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, InitExceptionResult(ex));
            }
            
        }

        [HttpGet("newEmployeeCode")]
        public IActionResult GetNewEmployeeCode()
        {
            try
            {
                return Ok(_employeeService.GetNewEmployeeCode());

            }
            catch (Exception ex)
            {
                return StatusCode(500, InitExceptionResult(ex));
            }
        }

        [HttpPost("deletes")]
        public IActionResult Deletes(IEnumerable<Guid> employeeDeleteList)
        {
            try
            {
                _serviceResult = _employeeService.Deletes(employeeDeleteList);
                return Ok(_serviceResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, InitExceptionResult(ex));
            }
        }
    }
}
