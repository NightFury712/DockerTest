using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.ApplicationCore.Entities;

namespace MISA.ApplicationCore.Interfaces.Service
{
    public interface ICustomerService : IBaseService<Customer>
    {

        /// <summary>
        /// Phương thức phân trang cho thực thể khách hàng
        /// </summary>
        /// <param name="pageSize"> Số lượng bản ghi mỗi trang </param>
        /// <param name="pageIndex"> Số Trang </param>
        /// <param name="employeeFilter"> Thông tin tìm kiếm </param>
        /// <param name="customerGroupId">id nhóm khách hàng</param>
        /// <returns> Danh sách khách hàng </returns>
        /// Author: HHDang (30/7/2021)
        object GetCustomerPaging(int pageSize, int pageIndex, string customerFilter, Guid? customerGroupId);

        /// <summary>
        /// Lấy danh sách nhóm khách hàng
        /// </summary>
        /// <returns>Danh sách nhóm khách hàng</returns>
        /// Author: HHDang (1/8/2021)
        IEnumerable<CustomerGroup> GetCustomerGroups();

        /// <summary>
        /// Lấy nhóm khách hàng theo id
        /// </summary>
        /// <param name="CustomerGroupId">Id nhóm khách hàng</param>
        /// <returns>Nhóm khách hàng tìm kiếm theo id</returns>
        /// Author: HHDang (1/8/2021)
        CustomerGroup GetCustomerGroupById(Guid customerGroupId);
    }
}
