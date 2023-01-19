using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Const
{
    public class MISAConst
    {
        /// <summary>
        /// Trường này không được phép để trống
        /// </summary>
        public const string Required = "MISA-001";

        /// <summary>
        /// Trường này đã bị lặp lại
        /// </summary>
        public const string Duplicate = "MISA-002";

        /// <summary>
        /// Trường này có độ dài không hợp lệ
        /// </summary>
        public const string OverLength = "MISA-003";

        /// <summary>
        /// Trường này không hợp lệ
        /// </summary>
        public const string NotValid = "MISA-004";

    }
}
