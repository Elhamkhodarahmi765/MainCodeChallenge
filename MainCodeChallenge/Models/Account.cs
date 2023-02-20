using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;



namespace MainCodeChallenge.Models
{
    public class Account
    {
        [Required(ErrorMessage = "Please enter your username")]
        [MinLength(8, ErrorMessage="Username must be more than 8 characters")]
        [Display(Name ="UserName")]
        public string user { get; set; }

        [MinLength(8, ErrorMessage = "Username must be more than 8 characters")]
        [Display(Name = "UserName")]
        [Required(ErrorMessage = "Please enter your password")]
        //[RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$", ErrorMessage = "The password must be at least 8 digits long and contain at least one letter, one number and one special character")]
        public string password { get; set; }
        public bool activeStatus { get; set; }
    }

   
}