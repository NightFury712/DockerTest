using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.ApplicationCore.Enums;

namespace MISA.ApplicationCore.Entities
{
    

    public class BaseEntity
    {
        public EntityState EntityState { get; set; } = EntityState.AddNew;
        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Thời gian thay đổi
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
        /// <summary>
        /// Người thau đổi
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}
