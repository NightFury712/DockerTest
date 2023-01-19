using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Entities
{
    public class Position : BaseEntity
    {
        #region Properties
        /// <summary>
        /// Id vị trí
        /// </summary>
        [PrimaryKey]
        public Guid PositionId { get; set; }
        /// <summary>
        /// Mã vị trí
        /// </summary>
        [CheckDuplicate]
        [Required]
        public string PositionCode { get; set; }
        /// <summary>
        /// Tên vị trí
        /// </summary>
        [Required]
        public string PositionName { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
        #endregion
    }
}
