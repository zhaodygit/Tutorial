using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Heavy.Web.ViewModels
{
    public class UserAddViewModel
    {
        //[BindNever]禁止绑定值
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "邮箱")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$")]
        public string Emial { get; set; }

        [Required]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "身份证")]
        public string IdCardNo { get; set; }

        [Required]
        [Display(Name = "生日")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public RoleAddViewModel Role { get; set; }

        public AnthorUserViewModel AnthorUser { get; set; }
    }

    public class AnthorUserViewModel
    {
        public string UserName { get; set; }
        public string  Email { get; set; }
    }
}
