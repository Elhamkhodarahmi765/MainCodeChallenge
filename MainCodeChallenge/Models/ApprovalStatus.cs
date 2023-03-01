using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MainCodeChallenge.Models
{
    public class ApprovalStatus
    {
        public int SId { get; set; }
        public int SQId { get; set; }
        public int SQPid { get; set; }
        public int SQStatus { get; set; }
        public EnumSQStatus QStatus { get; set; }
        public DateTime? SQDate { get; set; }
        public int ApStatus { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public int ApprovalPID { get; set; }
        public int SAnswerLanguage { get; set; }
        [DataType(DataType.MultilineText)]
        [DisplayName("Body:")]
        public string SAnswer { get; set; }
        public string Lname { get; set; }
        public EnumApprovalStatus? approvalStatus { get; set; }
    }
}