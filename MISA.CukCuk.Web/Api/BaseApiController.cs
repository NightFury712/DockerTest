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
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseApiController<MISAEntity> : ControllerBase
    {
        IBaseService<MISAEntity> _baseService;
        protected ServiceResult _serviceResult;
        string _entityName = string.Empty;
        public BaseApiController(IBaseService<MISAEntity> baseService)
        {
            _baseService = baseService;
            _entityName = typeof(MISAEntity).Name;
            _serviceResult = new ServiceResult();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var employees = _baseService.GetEntities();
                if (employees != null)
                {
                    if (employees.Count() > 0)
                    {
                        return Ok(employees);
                    }
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, InitExceptionResult(ex));

            }

        }

        [HttpGet("{entityId}")]
        public IActionResult GetEmployeeById(Guid entityId)
        {
            try
            {
                var employee = _baseService.GetEntityById(entityId);
                if (employee != null)
                {
                    return Ok(employee);
                } else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, InitExceptionResult(ex));
            }
        }

        [HttpPost]
        public IActionResult Post(MISAEntity employee)
        {
            try
            {
                _serviceResult = _baseService.Save(employee);
                if (_serviceResult.MISACode == MISACode.Created && (int)_serviceResult.Data > 0)
                {
                    return Created("Create successfully! ", _serviceResult);
                }
                else if(_serviceResult.MISACode == MISACode.NotValid)
                {
                    return Ok(_serviceResult);
                } else
                {
                    _serviceResult.MISACode = MISACode.Exception;
                    _serviceResult.Messenger = MISA.ApplicationCore.Properties.Resources.ErrorException;
                    return StatusCode(500, _serviceResult);
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, InitExceptionResult(ex));
            }
        }

        [HttpPut("{entityId}")]
        public IActionResult Put([FromBody]MISAEntity entity, Guid entityId)
        {
            try
            {
                entity.GetType().GetProperty($"{_entityName}Id").SetValue(entity, entityId);
                _serviceResult = _baseService.Update(entity);
                return StatusCode(200, _serviceResult);
            }
            catch (Exception ex)
            {

                return StatusCode(500, InitExceptionResult(ex));
            }
        }

        [HttpDelete("{entityId}")]
        public IActionResult Delete(Guid entityId)
        {
            try
            {
                _serviceResult = _baseService.Delete(entityId);

                if (_serviceResult.MISACode == MISACode.Ok)
                {
                    return Ok(_serviceResult);
                }
                else
                {
                    return Ok(_serviceResult);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, InitExceptionResult(ex));
                //return NoContent();
            }
        }

        protected ServiceResult InitExceptionResult(Exception ex)
        {
            
            _serviceResult.MISACode = MISACode.Exception;
            _serviceResult.Messenger = MISA.ApplicationCore.Properties.Resources.ErrorException;
            _serviceResult.Data = ex.Message;
            return _serviceResult;
        }
    }
}
