using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MISA.ApplicationCore.Enums;

namespace MISA.ApplicationCore.Entities
{
    /// <summary>
    /// Thông tin nhân viên
    /// </summary>
    /// Author: HHDang (21/7/2021)
    public class Employee : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Khóa chính
        /// </summary>
        [PrimaryKey]
        public Guid EmployeeId { get; set; }
        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [CheckDuplicate]
        [Required]
        [DisplayName("mã nhân viên")]
        [MaxLength(20)]
        public string EmployeeCode { get; set; }
        /// <summary>
        /// Họ
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Tên 
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Họ và tên đầy đủ của nhân viên
        /// </summary>
        [Required]
        [DisplayName("tên đầy đủ")]
        [MaxLength(100)]
        public string FullName { get; set; }
        /// <summary>
        /// Mã giới tính 
        /// </summary>
        public Gender? Gender { get; set; }
        /// <summary>
        /// Giới tính
        /// </summary>
        public string GenderName 
        {
            get { 
                switch(Gender)
                {
                    case Enums.Gender.Female:
                        return "Nữ";
                    case Enums.Gender.Male:
                        return "Nam";
                    case Enums.Gender.Other:
                        return "Giới tính khác";
                    default:
                        return null;
                }
            }
            set { }
        }
        /// <summary>
        /// Ngày sinh 
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
        /// <summary>
        /// Số điện thoại 
        /// </summary>
        [Required]
        [CheckDuplicate]
        [ValidatePhoneNumber]
        [DisplayName("số điện thoại")]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Email 
        /// </summary>
        [Required]
        [CheckDuplicate]
        [ValidateEmail]
        [DisplayName("email")]
        public string Email { get; set; }
        /// <summary>
        /// Địa chỉ 
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Số CMTND/CCCD
        /// </summary>
        [Required]
        [CheckDuplicate]
        [ValidateNumber]
        [DisplayName("số CMTND/CCCD")]
        [MaxLength(25)]
        public string IdentityNumber { get; set; }
        /// <summary>
        /// Ngày cấp
        /// </summary>
        public DateTime? IdentityDate { get; set; }
        /// <summary>
        /// Nơi cấp
        /// </summary>
        public string IdentityPlace { get; set; }
        /// <summary>
        /// Thời gian gia nhập công ty
        /// </summary>
        public DateTime? JoinDate { get; set; }
        /// <summary>
        /// Tình trạng quan hệ
        /// </summary>
        public int? MartialStatus { get; set; }
        /// <summary>
        /// Học vấn
        /// </summary>
        public int? EducationalBackground { get; set; }
        /// <summary>
        /// Lương
        /// </summary>
        public double? Salary { get; set; }
        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public Guid? QualificationId { get; set; }
        /// <summary>
        /// Mã phòng ban
        /// </summary>
        public Guid? DepartmentId { get; set; }
        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// Mã vị trí
        /// </summary>
        public Guid? PositionId { get; set; }
        /// <summary>
        /// Tên vị trí
        /// </summary>
        public string PositionName { get; set; }
        /// <summary>
        /// Tình trạng công việc
        /// </summary>
        public WorkStatus? WorkStatus { get; set; }
        /// <summary>
        /// Tên tình trạng công việc
        /// </summary>
        public string WorkStatusName
        {
            get
            {
                switch (WorkStatus)
                {
                    case Enums.WorkStatus.Retirement:
                        return "Đã nghỉ hưu";
                    case Enums.WorkStatus.Intern:
                        return "Thực tập sinh";
                    case Enums.WorkStatus.Working:
                        return "Đang làm việc";
                    case Enums.WorkStatus.MaternityLeave:
                        return "Nghỉ thai sản";
                    default: return null;
                }
            }
            set { }
        }
        /// <summary>
        /// Mã số thuế cá nhân
        /// </summary>
        [ValidateNumber]
        [CheckDuplicate]
        [DisplayName("mã số thuế")]
        [MaxLength(25)]
        public string PersonalTaxCode { get; set; }
        #endregion
    }
}
