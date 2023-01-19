using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interfaces.Infarstructure
{
    public interface IBaseRepository<MISAEntity>
    {

        #region Methods
        /// <summary>
        /// Lấy danh sách thực thể
        /// </summary>
        /// <returns>Danh sách thực thể</returns>
        /// Author: HHDang (26/7/2021)
        IEnumerable<MISAEntity> GetEntities();
        /// <summary>
        /// Lấy thông tin thực thể theo id
        /// </summary>
        /// <param name="entityId">Id của thực thể</param>
        /// <returns>Thông tin thực thể</returns>
        /// Author: HHDang (26/7/2021)
        MISAEntity GetEntityById(Guid entityId);
        /// <summary>
        /// Thêm mới thực thể
        /// </summary>
        /// <param name="entity">Obj chứa thông tin thực thể thêm mới</param>
        /// <returns>Số cột bị ảnh hưởng</returns>
        /// Author: HHDang (26/7/2021)
        int Save(MISAEntity entity);
        /// <summary>
        /// Cập nhật thông tin thực thể
        /// </summary>
        /// <param name="entity">Object chứa thông tin thực thể được cập nhật</param>
        /// <returns>Số cột bị ảnh hưởng</returns>
        /// Author: HHDang (21/7/2021)
        int Update(MISAEntity entity);
        /// <summary>
        /// Xóa thực thể 
        /// </summary>
        /// <param name="entityId">Id thực thể cần xóa</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Author: HHDang (21/7/2021)
        int Delete(Guid entityId);

        /// <summary>
        /// Kiểm tra trùng lặp thực thể
        /// </summary>
        /// <param name="entity">thông tin thực thể</param>
        /// <param name="property">Thông tin thuộc tính của thực thể</param>
        /// <returns>Thông tin thực thể</returns>
        /// Author: HHDang (06/08/2021)
        MISAEntity GetEntityByProperty(MISAEntity entity, PropertyInfo property);
    }
    #endregion
}

