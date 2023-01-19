using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.ApplicationCore.Enums;

namespace MISA.ApplicationCore.Entities
{
    public class ServiceResult
    {
        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// Thông báo về kết quả
        /// </summary>
        public string Messenger { get; set; }
        /// <summary>
        /// Mã của kết quả
        /// </summary>
        public MISACode MISACode { get; set; }

    }
}
