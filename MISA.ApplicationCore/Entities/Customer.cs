using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Entities
{
    public class Customer : BaseEntity
    {
        #region Properties
        /// <summary>
        /// Khóa chính
        /// </summary>
        [PrimaryKey]
        public Guid CustomerId { get; set; }
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        [CheckDuplicate]
        [Required]
        [DisplayName("mã khách hàng")]
        [MaxLength(20)]
        public string CustomerCode { get; set; }
        /// <summary>
        /// Họ
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Tên
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Họ và tên
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Giới tính
        /// </summary>
        public int? Gender { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [Required]
        [CheckDuplicate]
        [ValidateEmail]
        public string Email { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        [Required]
        [CheckDuplicate]
        [ValidatePhoneNumber]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Id nhóm khách hàng
        /// </summary>
        public Guid? CustomerGroupId { get; set; }
        /// <summary>
        /// Tổng tiền nợ
        /// </summary>
        public double? DebitAmount { get; set; }
        /// <summary>
        /// Mã thẻ thành viên
        /// </summary>
        [Required]
        [CheckDuplicate]
        public string MemberCardCode { get; set; }
        /// <summary>
        /// Tên công ty
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// Mã số thuế công ty
        /// </summary>
        [CheckDuplicate]
        [ValidateNumber]
        public string CompanyTaxCode { get; set; }
        /// <summary>
        /// Đã dừng follow?
        /// </summary>
        public bool? IsStopFollow { get; set; }
        #endregion
    }
}
