using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.ApplicationCore.Entities;

namespace MISA.ApplicationCore.Interfaces.Infarstructure
{
    /// <summary>
    /// Interfaces danh sách nhân viên
    /// </summary>
    /// Author: HHDang (26/7/2021)
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
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
        object GetEmployeePaging(int pageSize, int pageIndex, string employeeFilter, Guid? departmentId, Guid? positionId);
        /// <summary>
        /// Lấy mã nhân viên mới 
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// Author: HHDang (30/7/2021)
        string GetNewEmployeeCode();
        /// <summary>
        /// Phương thức xóa nhiều nhân viên
        /// </summary>
        /// <param name="employeeDeleteList">Danh sách nhân viên</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Author: HHDang (11/8/2021)
        int Deletes(IEnumerable<Guid> employeeDeleteList);
    }
    #endregion
}
