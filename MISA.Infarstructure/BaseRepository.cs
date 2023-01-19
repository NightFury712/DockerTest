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
using MISA.ApplicationCore.Enums;
using MISA.ApplicationCore.Interfaces.Infarstructure;
using MySqlConnector;

namespace MISA.Infarstructure
{
    public class BaseRepository<MISAEntity> : IBaseRepository<MISAEntity>, IDisposable where MISAEntity : BaseEntity
    {
        #region DECLARE
        private readonly IConfiguration _configuration;
        string _connectionString = string.Empty;
        protected IDbConnection _dbConnection = null;
        protected string _tableName;
        #endregion

        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("MISACukCukConnectionString");
            _dbConnection = new MySqlConnection(_connectionString);
            _tableName = typeof(MISAEntity).Name;
        }

        /// <summary>
        /// Lấy toàn bộ thực thể
        /// </summary>
        /// <returns>Danh sách thực thể</returns>
        /// Author: HHDang(30/6/2021)
        public IEnumerable<MISAEntity> GetEntities()
        {
            try
            {
                // Khởi tạo các commandText:
                var entities = _dbConnection.Query<MISAEntity>($"Proc_Get{_tableName}s", commandType: CommandType.StoredProcedure);

                // Trả về dữ liệu:
                return entities;
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// Lấy thực thể theo id
        /// </summary>
        /// <param name="entityId">id của thực thể</param>
        /// <returns>Thực thể theo id tìm kiếm</returns>
        /// Author: HHDang (30/7/2021)
        public MISAEntity GetEntityById(Guid entityId)
        {
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add($"@{_tableName}Id", entityId, DbType.String);
                var entity = _dbConnection.Query<MISAEntity>($"Proc_Get{_tableName}ById", parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
                return entity;
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// Kiểm tra trùng lặp thực thể
        /// </summary>
        /// <param name="entity">thông tin thực thể</param>
        /// <param name="property">Thông tin thuộc tính của thực thể</param>
        /// <returns>Thông tin thực thể</returns>
        /// Author: HHDang (06/08/2021)
        public MISAEntity GetEntityByProperty(MISAEntity entity, PropertyInfo property)
        {
            // Tạo dynamic pârameters
            var parameter = new DynamicParameters();
            var propertyName = property.Name;
            var propertyValue = property.GetValue(entity);
            var keyValue = entity.GetType().GetProperty($"{_tableName}Id").GetValue(entity);
            parameter.Add("@propertyName", propertyValue, DbType.String);
            parameter.Add("@propertyId", keyValue, DbType.String);

            // Tạo mới commandText
            var query = string.Empty;
            if (entity.EntityState == EntityState.AddNew)
            {
                query = $"SELECT * from View_{_tableName} WHERE {propertyName} = @propertyName";
            }
            else if (entity.EntityState == EntityState.Update)
            {
                query = $"SELECT * from View_{_tableName} WHERE {propertyName} = @propertyName AND {_tableName}Id <> @propertyId";
            }
            // Thực thi commandText
            var data = _dbConnection.Query<MISAEntity>(query, param: parameter, commandType: CommandType.Text).FirstOrDefault();
            // Trả về dữ liệu:
            return data;
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="entity">Thông tin bản ghi</param>
        /// <returns>Số cột bị ảnh hưởng</returns>
        /// Author: HHDang (30/7/2021)
        public int Save(MISAEntity entity)
        {
            var rowAffects = 0;
            _dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    // Xử lý các kiểu dữ liệu (mapping dataType):
                    var parameters = MappingDbtype(entity);
                    // Thực thi commandText
                    rowAffects = _dbConnection.Execute($"Proc_Insert{_tableName}",
                        parameters,
                        commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    if(rowAffects > 0)
                    {
                        transaction.Commit();
                    } else
                    {
                        transaction.Rollback();
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            // Trả về kết quả (Số bản ghi thêm mới được)
            return rowAffects;
        }

        /// <summary>
        /// Cập nhật bản ghi
        /// </summary>
        /// <param name="entity">Thông tin bản ghi cập nhật</param>
        /// <returns>Số bản ghi cập nhật được</returns>
        /// Author: HHDang (30/7/2021)
        public int Update(MISAEntity entity)
        {

            var rowAffects = 0;
            _dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    // Xử lý các kiểu dữ liệu (mapping dataType):
                    var parameters = MappingDbtype(entity);
                    // Thực thi commandText
                    rowAffects = _dbConnection.Execute($"Proc_Update{_tableName}",
                        parameters,
                        commandType: CommandType.StoredProcedure
                        , transaction: transaction);
                    if(rowAffects > 0)
                    {
                        transaction.Commit();
                    } else
                    {
                        transaction.Rollback();
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

            }
            // Trả về kết quả (Số bản ghi cập nhật được)
            return rowAffects;


        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="entityId">Id của bản ghi cần xóa</param>
        /// <returns>Số cột bị ảnh hưởng</returns>
        /// Author: HHDang (30/7/2021)
        public int Delete(Guid entityId)
        {
            var rowEffects = 0;
            _dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    var parameter = new DynamicParameters();
                    parameter.Add($"@{_tableName}Id", entityId, DbType.String);

                    rowEffects = _dbConnection.Execute($"Proc_Delete{_tableName}ById",
                        parameter,
                        commandType: CommandType.StoredProcedure,
                        transaction: transaction);
                    if(rowEffects > 0)
                    {
                        transaction.Commit();
                    } else
                    {
                        transaction.Rollback();
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

            }
            return rowEffects;
        }

        /// <summary>
        /// Mapping dữ liệu
        /// </summary>
        /// <param name="entity">Thông tin về thực thể</param>
        /// <returns>parameter mang thông tin thực thể</returns>
        /// Author: HHDang(30/7/2021)
        private DynamicParameters MappingDbtype(MISAEntity entity)
        {
            var properties = entity.GetType().GetProperties();
            var parameters = new DynamicParameters();

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(entity);
                var propertyType = property.PropertyType;
                if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                {
                    parameters.Add($"@{propertyName}", propertyValue, DbType.String);
                }
                else if (propertyType == typeof(bool) || propertyType == typeof(bool?))
                {
                    var dbValue = (bool)propertyValue == true ? 1 : 0;
                    parameters.Add($"@{propertyName}", dbValue, DbType.Int32);
                }
                else
                {
                    parameters.Add($"@{propertyName}", propertyValue);
                }
            }
            return parameters;
        }
        /// <summary>
        /// Đóng kết nối đến database
        /// </summary>
        /// Author: HHDang (06/08/2021)
        public void Dispose()
        {
            if (_dbConnection.State == ConnectionState.Open)
            {
                _dbConnection.Close();
            }
        }
    }
}
