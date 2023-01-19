using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Const
{
    public class MISARegex
    {
        /// <summary>
        /// Regex kiểm tra Email hợp lệ
        /// </summary>
        /// Author: HHDang (5/8/2021)
        private Regex EmailRegex;

        /// <summary>
        /// Regex kiểm tra số điện thoại hợp lệ
        /// </summary>
        /// Author: HHDang (5/8/2021)
        private Regex PhoneNumberRegex;

        private Regex NumberRegex;

        public MISARegex()
        {
            this.SetEmailRegex();
            this.SetPhoneNumberRegex();
            this.SetNumberRegex();
        }

        #region Method
        /// <summary>
        /// Khởi tạo regex cho email
        /// </summary>
        /// Author: HHDang (5/8/2021)
        private void SetEmailRegex()
        {
            Regex rx = new Regex(@"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$"
                , RegexOptions.IgnoreCase);
            this.EmailRegex = rx;
        }

        /// <summary>
        /// Khởi tạo regex cho số điện thoại
        /// </summary>
        /// Author: HHDang (5/8/2021)
        private void SetPhoneNumberRegex()
        {
            Regex rx = new Regex(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$", RegexOptions.IgnoreCase);
            this.PhoneNumberRegex = rx;
        }

        private void SetNumberRegex()
        {
            Regex rx = new Regex(@"^\d+$", RegexOptions.IgnoreCase);
            this.NumberRegex = rx;
        }

        /// <summary>
        /// Lấy ra regex cho email
        /// </summary>
        /// <returns>regex cho email</returns>
        /// Author: HHDang (5/8/2021)
        public Regex GetEmailRegex()
        {
            return this.EmailRegex;
        }
        /// <summary>
        /// Lấy ra regex cho số điện thoại
        /// </summary>
        /// <returns>regex cho số điện thoại</returns>
        /// Author: HHDang (5/8/2021)
        public Regex GetPhoneNumberRegex()
        {
            return this.PhoneNumberRegex;
        }

        public Regex GetNumberRegex()
        {
            return this.NumberRegex;
        }

        #endregion
    }
}
