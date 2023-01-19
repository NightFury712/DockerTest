using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Microsoft.Extensions.Configuration;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Infarstructure;

namespace MISA.Infarstructure
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        #region Constructor
        public CustomerRepository(IConfiguration configuration):base(configuration)
        {

        }

        #endregion

        #region Method

        /// <summary>
        /// Lấy mã khách hàng mới 
        /// </summary>
        /// <returns>Mã khách hàng mới</returns>
        /// Author: HHDang (4/8/2021)
        public string GetNewCustomerCode()
        {
            try
            {
                var newCustomerCode = string.Empty;
                var data = _dbConnection.Query<string>("Proc_GetNewCustomerCode", commandType: CommandType.StoredProcedure);
                if(data != null)
                {
                    newCustomerCode = data.FirstOrDefault();
                }
                return newCustomerCode;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Phương thức phân trang cho thực thể khách hàng
        /// </summary>
        /// <param name="pageSize"> Số lượng bản ghi mỗi trang </param>
        /// <param name="pageIndex"> Số Trang </param>
        /// <param name="employeeFilter"> Thông tin tìm kiếm </param>
        /// <param name="departmentId">id phòng ban</param>
        /// <param name="positionId">id vị trí</param>
        /// <returns> Danh sách khách hàng </returns>
        /// Author: HHDang (30/7/2021)
        public object GetCustomerPaging(int pageSize, int pageIndex, string customerFilter, Guid? customerGroupId)
        {
            
            var parameter = new DynamicParameters();
            var input = customerFilter == null ? string.Empty : customerFilter;
            parameter.Add("@PageSize", pageSize, direction: ParameterDirection.Input);
            parameter.Add("@PageIndex", pageIndex * pageSize, direction: ParameterDirection.Input);
            parameter.Add("@CustomerFilter", input, direction: ParameterDirection.Input);
            parameter.Add("@CustomerGroupId", customerGroupId, DbType.String, direction: ParameterDirection.Input);
            parameter.Add("@TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameter.Add("@TotalPage", dbType: DbType.Int32, direction: ParameterDirection.Output);

            // Thực hiện truy vấn dữ liệu
            var employees = _dbConnection.Query<Customer>("Proc_GetCustomersFilterPaging", parameter, commandType: CommandType.StoredProcedure);

            // Trả về dữ liệu
            // <param name="TotalPage">Tổng số trang</param>
            // <param name="TotalRecord">Tổng số bản ghi</param>
            // <param name="Data">Danh sách khách hàng</param>

            return new
            {
                TotalPage = parameter.Get<int>("TotalPage"),
                TotalRecord = parameter.Get<int>("TotalRecord"),
                Data = employees
            };
        }

        /// <summary>
        /// Lấy danh sách nhóm khách hàng
        /// </summary>
        /// <returns>Danh sách nhóm khách hàng</returns>
        /// Author: HHDang (1/8/2021)
        public IEnumerable<CustomerGroup> GetCustomerGroups()
        {
            var customerGroup = _dbConnection.Query<CustomerGroup>("Proc_GetCustomerGroups", commandType: CommandType.StoredProcedure);
            return customerGroup;
        }

        /// <summary>
        /// Lấy nhóm khách hàng theo id
        /// </summary>
        /// <param name="CustomerGroupId">Id nhóm khách hàng</param>
        /// <returns>Nhóm khách hàng tìm kiếm theo id</returns>
        /// Author: HHDang (1/8/2021)
        public CustomerGroup GetCustomerGroupById(Guid customerGroupId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@CustomerGroupId", customerGroupId, DbType.String);

            var custommerGroup = _dbConnection.Query<CustomerGroup>("Proc_GetCustomerGroupById", parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return custommerGroup;
        }

        #endregion
    }
}
