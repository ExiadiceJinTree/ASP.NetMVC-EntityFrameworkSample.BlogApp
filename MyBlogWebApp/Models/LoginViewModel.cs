using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBlogWebApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [DisplayName("ログインID")]
        public string LoginId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("パスワード")]
        public string Password { get; set; }
    }
}