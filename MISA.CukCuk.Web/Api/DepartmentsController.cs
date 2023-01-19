using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.CukCuk.Web.Api
{
    [ApiController]
    public class DepartmentsController : BaseApiController<Department>
    {
        IBaseService<Department> _baseService;

        #region Constructor
        public DepartmentsController(IBaseService<Department> baseService):base(baseService)
        {
            _baseService = baseService;
        }
        #endregion

    }
}
