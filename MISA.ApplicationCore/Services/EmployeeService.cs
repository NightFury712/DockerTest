using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Enums;
using MISA.ApplicationCore.Interfaces.Service;
using MISA.ApplicationCore.Interfaces.Infarstructure;
using System.ComponentModel;
using MISA.ApplicationCore.Const;
using System.Reflection;

namespace MISA.ApplicationCore.Services
{
    /// <summary>
    /// Lớp xử lý nghiệp vụ cho thực thể nhân viên
    /// </summary>
    /// Author: HHDang (30/7/2021)
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        IEmployeeRepository _employeeRepository;
        MISARegex regex;

        #region Constructor
        public EmployeeService(IEmployeeRepository employeeRepository) : base(employeeRepository)
        {
            _employeeRepository = employeeRepository;
            regex = new MISARegex();
        }

        #endregion
        #region Method

        /// <summary>
        /// Phương thức phân trang cho thực thể nhân viên
        /// </summary>
        /// <param name="pageSize"> Số lượng bản ghi mỗi trang </param>
        /// <param name="pageIndex"> Số Trang </param>
        /// <param name="employeeFilter"> Thông tin tìm kiếm </param>
        /// <param name="departmentId">id phòng ban</param>
        /// <param name="positionId">id vị trí</param>
        /// <returns> Danh sách nhân viên </returns>
        /// Author: HHDang (30/7/2021)
        public object GetEmployeePaging(int pageSize, int pageIndex, string employeeFilter, Guid? departmentId, Guid? positionId)
        {
            return _employeeRepository.GetEmployeePaging(pageSize, pageIndex, employeeFilter, departmentId, positionId);
        }

        /// <summary>
        /// Lấy mã nhân viên mới 
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// Author: HHDang (30/7/2021)
        public string GetNewEmployeeCode()
        {
            return _employeeRepository.GetNewEmployeeCode();
        }

        /// <summary>
        /// Phương thức xóa nhiều nhân viên
        /// </summary>
        /// <param name="employeeDeleteList">Danh sách nhân viên</param>
        /// <returns>Kết quả và thông điệp</returns>
        /// Author: HHDang (11/8/2021)
        public ServiceResult Deletes(IEnumerable<Guid> employeeDeleteList)
        {
            var rowAffects = _employeeRepository.Deletes(employeeDeleteList);
            if (rowAffects >= 1)
            {
                serviceResult.MISACode = MISACode.Ok;
                serviceResult.Messenger = Properties.Resources.SR_Success_Delete;
                serviceResult.Data = rowAffects;
                return serviceResult;
            }
            else
            {
                serviceResult.MISACode = MISACode.Exception;
                serviceResult.Messenger = Properties.Resources.SR_Fail_Delete;
                serviceResult.Data = rowAffects;
                return serviceResult;
            }
        }

        /// <summary>
        /// Phương thức validate riêng cho các class con kế thừa
        /// </summary>
        /// <param name="entity">Thông tin về thực thể</param>
        /// <returns>True or false</returns>
        /// Author: HHDang (30/7/2021)
        protected override bool ValidateCustom(Employee employee)
        {
            var properties = employee.GetType().GetProperties();

            foreach (var property in properties)
            {
                // Lấy giá trị của property hiện tại
                var propertyValue = property.GetValue(employee);
                /// Lấy tên hiển thị của property
                var displayName = string.Empty;
                DisplayNameAttribute dp = property.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().SingleOrDefault();
                if (dp != null)
                {
                    displayName = dp.DisplayName;
                }

                // Kiểm tra email hợp lệ
                if (property.IsDefined(typeof(ValidateEmail), false))
                {
                    // Lấy ra regex cho email
                    var emailRegex = regex.GetEmailRegex();

                    if (!emailRegex.IsMatch(propertyValue.ToString()))
                    {
                        SetServiceResult(property, displayName);
                        return false;
                    }
                }

                // Kiểm tra số điện thoại hợp lệ
                if (property.IsDefined(typeof(ValidatePhoneNumber), false))
                {
                    // Lấy ra regex cho số điện thoại
                    var phoneNumberRegex = regex.GetPhoneNumberRegex();
                    if (!phoneNumberRegex.IsMatch(propertyValue.ToString()))
                    {
                        SetServiceResult(property, displayName);
                        return false;
                    }
                }

                // Kiểm tra số CMT, mã số thuế hợp lệ
                if (property.IsDefined(typeof(ValidateNumber), false))
                {
                    if (String.IsNullOrEmpty(propertyValue.ToString()))
                    {
                        continue;
                    }
                    // Lấy ra regex kiểm tra số
                    var numberRegex = regex.GetNumberRegex();
                    if (!numberRegex.IsMatch(propertyValue.ToString()))
                    {
                        SetServiceResult(property, displayName);
                        return false;
                    }
                }
            }
            return true;
        }

        private void SetServiceResult(PropertyInfo property, string displayName)
        {
            var msg = new
            {
                devMsg = new { fieldName = property.Name, msg = string.Format(Properties.Resources.SR_Fail_Validate_NotValid, displayName) },
                userMsg = string.Format(Properties.Resources.SR_Fail_Validate_NotValid, displayName),
                Code = MISAConst.NotValid
            };
            serviceResult.MISACode = MISACode.NotValid;
            serviceResult.Messenger = string.Format(Properties.Resources.SR_Fail_Validate_NotValid, displayName);
            serviceResult.Data = msg;
        }

        #endregion
    }
}
