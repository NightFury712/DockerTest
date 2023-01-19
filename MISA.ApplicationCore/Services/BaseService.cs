using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.ApplicationCore.Const;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Enums;
using MISA.ApplicationCore.Interfaces.Infarstructure;
using MISA.ApplicationCore.Interfaces.Service;

namespace MISA.ApplicationCore.Services
{
    public class BaseService<MISAEntity> : IBaseService<MISAEntity> where MISAEntity : BaseEntity
    {
        #region Declare
        IBaseRepository<MISAEntity> _baseRepository;
        protected ServiceResult serviceResult;
        private bool isValid;
        #endregion

        #region Constructor
        public BaseService(IBaseRepository<MISAEntity> baseRepository)
        {
            _baseRepository = baseRepository;
            serviceResult = new ServiceResult();
            serviceResult.MISACode = MISACode.Ok;
            isValid = true;
        }
        #endregion

        #region Method

        /// <summary>
        /// Lấy danh sách thực thể
        /// </summary>
        /// <returns>Danh sách thực thể</returns>
        /// Author: HHDang (26/7/2021)
        public IEnumerable<MISAEntity> GetEntities()
        {
            return _baseRepository.GetEntities();
        }

        /// <summary>
        /// Lấy thông tin thực thể theo id
        /// </summary>
        /// <param name="entityId">Id của thực thể</param>
        /// <returns>Thông tin thực thể</returns>
        /// Author: HHDang (26/7/2021)
        public MISAEntity GetEntityById(Guid entityId)
        {
            var entity = _baseRepository.GetEntityById(entityId);
            return entity;
        }

        /// <summary>
        /// Thêm mới thực thể
        /// </summary>
        /// <param name="entity">Obj chứa thông tin thực thể thêm mới</param>
        /// <returns>Số cột bị ảnh hưởng</returns>
        /// Author: HHDang (26/7/2021)
        public virtual ServiceResult Save(MISAEntity entity)
        {
            entity.EntityState = EntityState.AddNew;

            isValid = Validate(entity);
            // Thêm mới dữ liệu khi đã hợp lệ:
            if(isValid)
            {
                isValid = ValidateCustom(entity);
            }
            if(isValid)
            { 
                var rowAffects = _baseRepository.Save(entity);
                if(rowAffects >= 1)
                {
                    serviceResult.MISACode = MISACode.Created;
                    serviceResult.Messenger = Properties.Resources.SR_Success_Add;
                    serviceResult.Data = rowAffects;
                } else
                {
                    serviceResult.MISACode = MISACode.Exception;
                    serviceResult.Messenger = Properties.Resources.SR_Fail_Add;
                    serviceResult.Data = rowAffects;
                }
            }
            return serviceResult;
        }

        /// <summary>
        /// Cập nhật thông tin thực thể
        /// </summary>
        /// <param name="entity">Object chứa thông tin thực thể được cập nhật</param>
        /// <returns>Số cột bị ảnh hưởng</returns>
        /// Author: HHDang (21/7/2021)
        public ServiceResult Update(MISAEntity entity)
        {
            entity.EntityState = EntityState.Update;
            isValid = Validate(entity);
            if(isValid)
            {
                isValid = ValidateCustom(entity);
            }
            if(isValid)
            {
                var rowAffects = _baseRepository.Update(entity);
                if (rowAffects >= 1)
                {
                    serviceResult.MISACode = MISACode.Ok;
                    serviceResult.Messenger = Properties.Resources.SR_Success_Update;
                    serviceResult.Data = rowAffects;
                }
                else
                {
                    serviceResult.MISACode = MISACode.Exception;
                    serviceResult.Messenger = Properties.Resources.SR_Fail_Update;
                    serviceResult.Data = rowAffects;
                }
            }
            return serviceResult;
        }

        /// <summary>
        /// Xóa thực thể 
        /// </summary>
        /// <param name="entityId">Id thực thể cần xóa</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Author: HHDang (21/7/2021)
        public ServiceResult Delete(Guid entityId)
        {
            var rowAffects = _baseRepository.Delete(entityId);
            if (rowAffects >= 1)
            {
                serviceResult.MISACode = MISACode.Ok; 
                serviceResult.Messenger = Properties.Resources.SR_Success_Delete;
                serviceResult.Data = rowAffects;
                return serviceResult;
            }
            else
            {
                serviceResult.MISACode = MISACode.BadRequest;
                serviceResult.Messenger = Properties.Resources.SR_Fail_Delete;
                serviceResult.Data = rowAffects;
                return serviceResult;
            }

        }

        /// <summary>
        /// Phương thức để validate dữ liệu về thực thể
        /// </summary>
        /// <param name="entity">Thông tin thực thể</param>
        /// <returns>Kết quả thông tin có hợp lệ hay không</returns>
        /// Author: HHDang (30/7/2021)
        private bool Validate(MISAEntity entity)
        {
            // Đọc các Property:
            var properties = entity.GetType().GetProperties();
            foreach (var property in properties)
            {
                /// Lấy giá trị của property hiện tại
                var propertyValue = property.GetValue(entity);

                /// Lấy tên hiển thị của property
                var displayName = string.Empty;
                DisplayNameAttribute dp = property.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().SingleOrDefault();
                if (dp != null)
                {
                    displayName = dp.DisplayName;
                }

                if (property.IsDefined(typeof(Required), false))
                {
                    // Check bắt buộc nhập:
                    var messenger = string.Format(Properties.Resources.SR_Fail_Validate_Required, displayName);
                    if (propertyValue == null || string.IsNullOrEmpty(propertyValue.ToString()))
                    {
                        var msg = new
                        {
                            devMsg = new { fieldName = property.Name, msg = messenger },
                            userMsg = messenger,
                            Code = MISAConst.Required
                        };
                        serviceResult.MISACode = MISACode.NotValid;
                        serviceResult.Messenger = messenger;
                        serviceResult.Data = msg;
                        return false;
                    } 
                }

                if(property.IsDefined(typeof(CheckDuplicate), false))
                {
                    // Check trùng dữ liệu:
                    var entityDuplicate = _baseRepository.GetEntityByProperty(entity, property);
                    var messenger = string.Format(Properties.Resources.SR_Fail_Validate_CheckDuplicate, displayName);
                    if (entityDuplicate != null)
                    {
                        var msg = new
                        {
                            devMsg = new { fieldName = property.Name, msg = messenger },
                            userMsg = messenger,
                            Code = MISAConst.Duplicate
                        };
                        serviceResult.MISACode = MISACode.NotValid;
                        serviceResult.Messenger = messenger;
                        serviceResult.Data = msg;
                        return false;
                    }
                }

                if (property.IsDefined(typeof(MaxLength), false))
                {
                    // Lấy độ dài đã khai báo:
                    var atributeMaxLength = property.GetCustomAttributes(typeof(MaxLength), true)[0];
                    var length = (atributeMaxLength as MaxLength).Value;
                    //var messenger = (atributeMaxLength as MaxLength).ErrorMsg;
                    var messenger = string.Format(Properties.Resources.SR_Fail_Validate_MaxLength, displayName, length);

                    if ( propertyValue.ToString().Trim().Length > length)
                    {
                        var msg = new
                        {
                            devMsg = new { fieldName = property.Name, msg = messenger },
                            userMsg = messenger,
                            Code = MISAConst.OverLength
                        };
                        serviceResult.MISACode = MISACode.NotValid;
                        serviceResult.Messenger = messenger;
                        serviceResult.Data = msg;
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Phương thức validate riêng cho các class con kế thừa
        /// </summary>
        /// <param name="entity">Thông tin về thực thể</param>
        /// <returns>True or false</returns>
        /// Author: HHDang (30/7/2021)
        protected virtual bool ValidateCustom(MISAEntity entity)
        {
            return true;
        }
        #endregion
    }
}
