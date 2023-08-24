using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainCodeChallenge.Models
{
    public class UserInfo
    {
        public int Uid { get; set; }
        public string PersonelId { get; set; }
        public int RP_id { get; set; }
        public string RealPersonFullname { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public EnumRoleS role { get; set; }
        public EnumRolePage rolePage { get; set; }
        public int Ppoint { get; set; }
        public string username { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }

    }
}