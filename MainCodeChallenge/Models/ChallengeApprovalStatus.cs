using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainCodeChallenge.Models
{
    public class ChallengeApprovalStatus
    {
        public int Qid { get; set; }
        public int QpersonOwner { get; set; }
        public string QName { get; set; }
        public string QDescription { get; set; }
        public EnumLevel QLevel { get; set; }
        public int QCId { get; set; }
        public string CT_Name { get; set; }
        public string POwnerName { get; set; }
        public int CountOfA { get; set; }
        public int LPointsRequired { get; set; }
        public int LPointReceived { get; set; }
        public int QRpoint { get; set; }
        public int QApoint { get; set; }
        public int Qfactor { get; set; }
        

    }
}