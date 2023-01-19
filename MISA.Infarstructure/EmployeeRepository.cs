using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Infarstructure;
using MySqlConnector;

namespace MISA.Infarstructure
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        #region Constructor
        public EmployeeRepository(IConfiguration configuration):base(configuration)
        {

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
                
            // Khởi tạo thông tin phân trang 
            var parameter = new DynamicParameters();
            var input = employeeFilter == null ? string.Empty : employeeFilter;
            parameter.Add("@PageSize", pageSize, direction: ParameterDirection.Input);
            parameter.Add("@PageIndex", pageIndex * pageSize, direction: ParameterDirection.Input);
            parameter.Add("@EmployeeFilter", input, direction: ParameterDirection.Input);
            parameter.Add("@DepartmentId", departmentId, DbType.String, direction: ParameterDirection.Input);
            parameter.Add("@PositionId", positionId, DbType.String, direction: ParameterDirection.Input);
            parameter.Add("@TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameter.Add("@TotalPage", dbType: DbType.Int32, direction: ParameterDirection.Output);

            // Thực hiện truy vấn dữ liệu
            var employees = _dbConnection.Query<Employee>("Proc_GetEmployeesFilterPaging", 
                parameter, 
                commandType: CommandType.StoredProcedure);

            // Trả về dữ liệu
            // <param name="TotalPage">Tổng số trang</param>
            // <param name="TotalRecord">Tổng số bản ghi</param>
            // <param name="Data">Danh sách nhân viên</param>

            var obj = new
            {
                TotalPage = parameter.Get<int>("TotalPage"),
                TotalRecord = parameter.Get<int>("TotalRecord"),
                Data = employees
            };
                
            
            return obj;
        }

        /// <summary>
        /// Lấy mã nhân viên mới 
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// Author: HHDang (30/7/2021)
        public string GetNewEmployeeCode()
        {
            var newEmployeeCode = _dbConnection.Query<string>("Proc_GetNewEmployeeCode", commandType: CommandType.StoredProcedure);
            
            if(newEmployeeCode != null)
            {
                return newEmployeeCode.FirstOrDefault();
            }
            return string.Empty;
        }

        /// <summary>
        /// Phương thức xóa nhiều nhân viên
        /// </summary>
        /// <param name="employeeDeleteList">Danh sách nhân viên</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Author: HHDang (11/8/2021)
        public int Deletes(IEnumerable<Guid> employeeDeleteList)
        {
            var rowEffects = 0;
            _dbConnection.Open();
            using(var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    var parameter = new DynamicParameters();
                    foreach (var employee in employeeDeleteList)
                    {
                        parameter.Add("@EmployeeId", employee, DbType.String);
                        var result = _dbConnection.Execute("Proc_DeleteEmployeeById",
                            parameter,
                            commandType: CommandType.StoredProcedure,
                            transaction: transaction);
                        if(result > 0)
                        {
                            rowEffects += result;
                        } else
                        {
                            transaction.Rollback();
                            return 0;
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
                
            }
            return rowEffects;
        }

        #endregion
    }
}
