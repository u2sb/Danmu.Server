using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Danmu.Utils.Common;

namespace Danmu.Model.DataTable
{
    [Table("User")]
    public class UserTable
    {
        /// <summary>
        ///     用户ID
        /// </summary>
        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        ///     用户名
        /// </summary>
        [Required]
        [MaxLength(16)]
        [MinLength(4)]
        public string Name { get; set; }

        /// <summary>
        ///     密码
        /// </summary>
        [Required]
        [MinLength(6)]
        public string PassWord { get; set; }

        /// <summary>
        ///     盐
        /// </summary>
        [Required]
        public string Salt { get; set; } = new RandomStringBuilder().Create(6);

        /// <summary>
        ///     用户角色
        /// </summary>
        [Required]
        [DefaultValue(UserRole.GeneralUser)]
        public UserRole Role { get; set; } = UserRole.Guests;

        /// <summary>
        ///     手机号
        /// </summary>
        [Phone]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     邮箱
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        ///     生成时间 UTC
        /// </summary>
        [Column(TypeName = "timestamp(3)")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        ///     修改时间 UTC
        /// </summary>
        [Column(TypeName = "timestamp(3)")]
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;

        public UserTable ToSecurity()
        {
            return new UserTable
            {
                Id = Id,
                Name = Name,
                Role = Role,
                PhoneNumber = PhoneNumber,
                Email = Email,
                CreateTime = CreateTime,
                UpdateTime = UpdateTime
            };
        }
    }

    public enum UserRole
    {
        Guests = 0,
        SuperAdmin = 1,
        Admin = 2,
        GeneralUser = 3
    }
}
