using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainCodeChallenge.Models
{
    public class Enums
    {

    }
    public enum EnumRolePage
    {
        AdminPage = 1,
        ProfilePage = 2
    }

    public enum EnumRoleS
    {
        Admin = 1,
        Profile = 2
    }

    public enum EnumLevel
    {
        Easy = 1,
        Normal = 2,
        Medium = 3, 
        Hard = 4,
        Expert = 5
    }

    public enum EnumSQStatus
    {
        PickUp=1,
        Done=2
    }

    public enum EnumApprovalStatus
    {
        AwaitingFinalApproval=1,
        FinalApproval=2
    }

    public  enum EnumPointParam
    {
        Decrease =1,
        Increase=2
    }
    public  enum EnumStatusRow
    {
        MainRow=1,
        SubRow=2
    }

}