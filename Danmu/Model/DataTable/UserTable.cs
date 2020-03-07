using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string Name { get; set; }

        /// <summary>
        ///     密码
        /// </summary>
        [Required]
        public string PassWord { get; set; }

        /// <summary>
        ///     盐
        /// </summary>
        [Required]
        public string Salt { get; set; }

        /// <summary>
        ///     用户角色
        /// </summary>
        [Required]
        [DefaultValue(UserRole.GeneralUser)]
        public UserRole Role { get; set; } = UserRole.GeneralUser;

        /// <summary>
        ///     手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     邮箱
        /// </summary>
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
        public DateTime UpDateTime { get; set; } = DateTime.UtcNow;
    }

    public enum UserRole
    {
        SuperAdmin = 0,
        Admin = 1,
        Guests = 2,
        GeneralUser = 3
    }
}
