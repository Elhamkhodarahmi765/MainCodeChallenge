using MainCodeChallenge.Models;
using MainCodeChallenge.Repositories;
using MainCodeChallenge.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainCodeChallenge.Services
{
    public class CodeChallengeServices : ICodeChallengeServices
    {
       
        public bool Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            using (CodeChallengeEntities db = new CodeChallengeEntities())
            {
                Tbl_User obj = db.Tbl_User.First(x => x.UuserName == username);

                if (obj.Uid ==  null)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
          
        }

        

        public Tbl_User GetActionToGoFromLoginPage(string username)
        {
            //search role in database
            byte  role =0;
            CodeChallengeEntities db = new CodeChallengeEntities();
            var q = db.Tbl_User.Where(x => x.UuserName.Equals(username)).SingleOrDefault();
            return q;
        }

      
        public List<ChallengeApprovalStatus> GetAllChallengeApprovalStatusCount()
        {
            CodeChallengeEntities db = new CodeChallengeEntities();
            List<ChallengeApprovalStatus> challengeApprovalStatus = (from ch in db.Tbl_Challenge
                         join Aps in db.Tbl_ApprovalStatus on ch.Qid equals Aps.SQId into ChAps
                         from Qid in ChAps.DefaultIfEmpty()
                         group new { ch, Qid }
                         by new
                         {
                             ch.Qid,
                             ch.QStatus,
                             ch.QName,
                             ch.QLevel,
                             ch.Tbl_Level.LPointsRequired,
                             ch.Tbl_Level.LPointReceived,
                             ch.QDescription,
                             Qid.SQId,
                             ch.Tbl_Category.CT_Name,
                             ch.Tbl_RealPerson.RP_FName,
                             ch.Tbl_RealPerson.RP_LName,
                             ch.QpersonOwner
                         } into grp
                         orderby grp.Key.Qid descending
                         select new ChallengeApprovalStatus
                         {
                             Qid = grp.Key.Qid,
                             QDescription = grp.Key.QDescription,
                             QName = grp.Key.QName,
                             QLevel = (EnumLevel)grp.Key.QLevel,
                             LPointReceived = (int)grp.Key.LPointReceived,
                             LPointsRequired = (int)grp.Key.LPointsRequired,
                             CT_Name = grp.Key.CT_Name,
                             POwnerName = grp.Key.RP_FName + " " + grp.Key.RP_LName,
                             QpersonOwner = (int)grp.Key.QpersonOwner,
                             CountOfA = grp.Where(t => t.Qid.SQId != null).Count()
                         }).ToList();

            return challengeApprovalStatus;
        }



         public List<ChallengeApprovalStatus> GetChallengeDetailsById(int Id)
         {
            CodeChallengeEntities db = new CodeChallengeEntities();
            List<ChallengeApprovalStatus> challengeApprovalStatus = (from ch in db.Tbl_Challenge
                         join Aps in db.Tbl_ApprovalStatus on ch.Qid equals Aps.SQId into ChAps
                         from Qid in ChAps.DefaultIfEmpty()
                         group new { ch, Qid }
                         by new
                         {
                             ch.Qid,
                             ch.QStatus,
                             ch.QName,
                             ch.QLevel,
                             ch.Tbl_Level.LPointsRequired,
                             ch.Tbl_Level.LPointReceived,
                             ch.QDescription,
                             Qid.SQId,
                             ch.Tbl_Category.CT_Name,
                             ch.Tbl_RealPerson.RP_FName,
                             ch.Tbl_RealPerson.RP_LName,
                             ch.QpersonOwner
                         } into grp
                         orderby grp.Key.Qid descending
                         select new ChallengeApprovalStatus
                         {
                             Qid = grp.Key.Qid ,
                             QDescription = grp.Key.QDescription,
                             QName = grp.Key.QName,
                             QLevel = (EnumLevel)grp.Key.QLevel,
                             LPointReceived=(int)grp.Key.LPointReceived,
                             LPointsRequired=(int)grp.Key.LPointsRequired,
                             CT_Name = grp.Key.CT_Name,
                             POwnerName=grp.Key.RP_FName + " " +grp.Key.RP_LName,
                             QpersonOwner=(int)grp.Key.QpersonOwner, 
                             CountOfA = grp.Where(t => t.Qid.SQId != null).Count()
                         }).ToList();

            return challengeApprovalStatus.Where(c => c.Qid == Id).ToList();
            //Mr Blukian Q (where)
         }


        public List<Example> GetExampleByChallengeId(int Id)
        {
            CodeChallengeEntities db = new CodeChallengeEntities();
            List<Example> example = (from e in db.Tbl_Example
                                     select new Example
                                     {
                                         EXId= e.EXId,
                                         EXName= e.EXName,
                                         EXInputModel= e.EXInputModel,
                                         EXOutputModel= e.EXOutputModel,
                                         EXQId=(int) e.EXQId 
                                     }
                                     ).ToList();

            return example.Where(c => c.EXQId == Id).ToList();
        }
    }
}