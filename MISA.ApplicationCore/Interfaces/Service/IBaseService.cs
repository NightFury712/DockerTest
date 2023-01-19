using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.ApplicationCore.Entities;

namespace MISA.ApplicationCore.Interfaces.Service
{
    public interface IBaseService<MISAEntity>
    {
        /// <summary>
        /// Lấy toàn bộ thực thể
        /// </summary>
        /// <returns>Danh sách thực thể</returns>
        /// Author: HHDang(30/7/2021)
        IEnumerable<MISAEntity> GetEntities();

        /// <summary>
        /// Lấy thực thể theo id
        /// </summary>
        /// <param name="entityId">id của thực thể</param>
        /// <returns>Thực thể theo id tìm kiếm</returns>
        /// Author: HHDang (30/7/2021)
        MISAEntity GetEntityById(Guid entityId);

        /// <summary>
        /// Thực hiện validate để thêm mới bản ghi
        /// </summary>
        /// <param name="entity">Thông tin bản ghi</param>
        /// <returns>Kết quả và thông điệp</returns>
        /// Author: HHDang (30/7/2021)
        ServiceResult Save(MISAEntity entity);

        /// <summary>
        /// Thực hiện validate để cập nhật bản ghi
        /// </summary>
        /// <param name="entity">Thông tin bản ghi cập nhật</param>
        /// <returns>Kết quả và thông điệp</returns>
        /// Author: HHDang (30/7/2021)
        ServiceResult Update(MISAEntity entity);

        /// <summary>
        /// Thực hiện xóa thực thể
        /// </summary>
        /// <param name="entityId">Id của thực thể</param>
        /// <returns>Kết quả và thông điệp</returns>
        /// Author: HHDang (31/7/2021)
        ServiceResult Delete(Guid entityId);
    }
}
