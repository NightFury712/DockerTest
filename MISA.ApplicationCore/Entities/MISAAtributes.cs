using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Entities
{
    /// <summary>
    /// Atribute dùng để kiểm tra trường bắt buộc nhập
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Required : Attribute
    {

    }
    /// <summary>
    /// Atribute dùng để kiểm tra trường bị lặp lại
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CheckDuplicate : Attribute
    {

    }
    /// <summary>
    /// Atribute dùng để xác đinh khóa chính
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKey : Attribute
    {

    }

    /// <summary>
    /// Atribute dùng để kiểm tra độ dài tối đa của dữ liệu
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxLength : Attribute
    {
        public int Value { get; set; }
        public string ErrorMsg { get; set; }
        public MaxLength(int length, string errorMsg = "")
        {
            this.Value = length;
            this.ErrorMsg = errorMsg;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ValidateEmail : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ValidatePhoneNumber : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ValidateNumber : Attribute
    {

    }
}
