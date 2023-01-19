using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MISA.ApplicationCore.Const;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Enums;
using MISA.ApplicationCore.Interfaces.Infarstructure;
using MISA.ApplicationCore.Interfaces.Service;

namespace MISA.ApplicationCore.Services
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        ICustomerRepository _customerRepository;
        MISARegex regex;

        #region Constructor
        public CustomerService(ICustomerRepository customerRepository):base(customerRepository)
        {
            _customerRepository = customerRepository;
            regex = new MISARegex();
        }
        #endregion

        #region Method

        public object GetCustomerPaging(int pageSize, int pageIndex, string customerFilter, Guid? customerGroupId)
        {
            return _customerRepository.GetCustomerPaging(pageSize, pageIndex, customerFilter, customerGroupId);
        }

        public IEnumerable<CustomerGroup> GetCustomerGroups()
        {
            return _customerRepository.GetCustomerGroups();
        }

        public CustomerGroup GetCustomerGroupById(Guid customerGroupId)
        {
            return _customerRepository.GetCustomerGroupById(customerGroupId);
        }

        protected override bool ValidateCustom(Customer customer)
        {
            var properties = customer.GetType().GetProperties();


            foreach (var property in properties)
            {
                // Lấy giá trị của property hiện tại
                var propertyValue = property.GetValue(customer);

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

                    // Email không hợp lệ sẽ trả về false
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

                    // Số điện thoại không hợp lệ sẽ trả về false
                    if (!phoneNumberRegex.IsMatch(propertyValue.ToString()))
                    {
                        SetServiceResult(property, displayName);
                        return false;
                    }
                }

                // Kiểm tra số mã số công ty thuế hợp lệ
                if (property.IsDefined(typeof(ValidateNumber), false))
                {
                    // Lấy ra regex kiểm tra số
                    var numberRegex = regex.GetNumberRegex();

                    // Giá trị không phải là số sẽ trả về false
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
