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
                try
                {
                    Tbl_User obj = db.Tbl_User.First(x => x.UuserName == username);

                    if (obj.Uid == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch 
                {
                    return false;
                }


            }
          
        }

        

        public Tbl_User GetActionToGoFromLoginPage(string username)
        {
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
                             ch.QRpoint,
                             ch.QApoint,
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
                             LPointReceived = (int?)grp.Key.LPointReceived ?? 0,
                             LPointsRequired = (int?)grp.Key.LPointsRequired ??0,
                             QRpoint=(int?)grp.Key.QRpoint ?? 0,
                             QApoint=(int?)grp.Key.QApoint ?? 0,
                             CT_Name = grp.Key.CT_Name,
                             POwnerName = grp.Key.RP_FName + " " + grp.Key.RP_LName,
                             QpersonOwner = (int?)grp.Key.QpersonOwner ??0,
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
                             ch.QRpoint,
                             ch.QApoint,
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
                             LPointReceived=(int?)grp.Key.LPointReceived ??0,
                             LPointsRequired=(int?)grp.Key.LPointsRequired ??0,
                             CT_Name = grp.Key.CT_Name,
                             POwnerName=grp.Key.RP_FName + " " +grp.Key.RP_LName,
                             QpersonOwner=(int?)grp.Key.QpersonOwner ??0,
                             QRpoint = (int?)grp.Key.QRpoint ?? 0,
                             QApoint = (int?)grp.Key.QApoint ?? 0,
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

        public  UserInfo  GetUserInfoByUId(int UID)
        {
            CodeChallengeEntities db=new CodeChallengeEntities();
            List<UserInfo> userInfo = (from u in db.Tbl_RealPerson
                                 join p in db.Tbl_RealPesronPoint on u.RP_Userid equals p.PUserId into RealPersonPoint
                                 from p2 in RealPersonPoint.DefaultIfEmpty()
                                 where u.RP_Userid==UID
                                 select new UserInfo 
                                 {
                                        Uid=u.RP_id,
                                        PersonelId=u.RP_PersonelId,
                                        EmailAddress=u.RP_EmailAddress,
                                        PhoneNumber=u.RP_PhoneNumber,
                                        role=(EnumRoleS)u.RP_Role,
                                        rolePage=(EnumRolePage)u.RP_Role,
                                        //RealPersonFullname=u.getRealPersonFullname(),
                                        RealPersonFullname = u.RP_FName + " " + u.RP_LName,
                                        Ppoint =(int?)p2.PPoint ?? 0,
                                        RP_id=u.RP_id

                                 }).ToList();
            return userInfo.First();
            //Q Bluekian
        }


        public int GetPointById(int Uid)
        {
            CodeChallengeEntities db = new CodeChallengeEntities();
            var pointperson = (from a in db.Tbl_RealPesronPoint
                                  where a.PUserId  == Uid
                                  select new
                                  {
                                      point=a.PPoint
                                  }).ToList();
            return (int?)pointperson.First().point ?? 0;
        }






       public bool GetAllChallengeApprovalStatusPerson(int Qid, int Uid)
       {
            UserInfo userInfo = GetUserInfoByUId(Uid);
            CodeChallengeEntities db = new CodeChallengeEntities();
            var ApprovalStatus = (from a in db.Tbl_ApprovalStatus
                       where a.SQId == Qid && a.SQPid == userInfo.Uid
                                  select new
                       {
                          Qid=a.SQId
                       }).ToList();
            
            if(ApprovalStatus.Count!=0)
            {
                return true;
            }
            else
            {
                return false;
            }

       }

        public bool PickUpChallenge(int Qid, int Uid)
        {
            UserInfo userInfo = GetUserInfoByUId(Uid);
            CodeChallengeEntities db = new CodeChallengeEntities();
            var ApprovalStatus = (from a in db.Tbl_ApprovalStatus
                                  where a.SQId == Qid && a.SQPid == userInfo.Uid
                                  select new
                                  {
                                      Qid = a.SQId
                                  }).ToList();
            int Rpoint=0;
            ChallengeApprovalStatus ChallengeApprovalStatus = GetChallengeDetailsById(Qid).First();
            Rpoint = ChallengeApprovalStatus.QRpoint;

            if (ApprovalStatus.Count != 0)
            {
                return false;
            }
            else
            {
                UserInfo userinfo = GetUserInfoByUId(Uid);
                if(IsItPossibleToPickUp(Qid,Uid))
                {

                   var trans =  db.Database.BeginTransaction();

                    Tbl_ApprovalStatus Aps = new Tbl_ApprovalStatus();
                    Aps.SQId=Qid;
                    Aps.SQPid= userinfo.RP_id;
                    Aps.SQStatus = (int)EnumSQStatus.PickUp ;
                    Aps.SQDate = DateTime.Now;
                    db.Tbl_ApprovalStatus.Add(Aps);
                    db.SaveChanges();


                    
                    var result = db.Tbl_RealPesronPoint.SingleOrDefault(RP => RP.PUserId ==Uid );
                    if (result != null)
                    {
                        try
                        {
                            result.PPoint -= Rpoint;
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }




                    trans.Commit();
                    return true;
                }
                else
                {
                    return false;
                }
                
            }

        }

        public bool IsItPossibleToPickUp(int Qid, int Uid)
        {
            ChallengeApprovalStatus ChallengeApprovalStatus = GetChallengeDetailsById(Qid).First();
            if(ChallengeApprovalStatus.QRpoint <= GetPointById(Uid))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool DoneChallenge(int Qid,int Uid, string AnsText,int lan)
        {
            CodeChallengeEntities db=new CodeChallengeEntities();
            int PUid = GetPidByUserId(Uid);
            var result = db.Tbl_ApprovalStatus.SingleOrDefault(RP => RP.SQPid == PUid && RP.SQId == Qid);
            if (result != null)
            {
                try
                {
                    result.SQDate = DateTime.Now;
                    result.SAnswer = AnsText;
                    result.SAnswerLanguage = lan;
                    result.SQStatus = (int)EnumSQStatus.Done;
                    result.SQDate=DateTime.Now;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return false;
        }

        public List<ApprovalStatus> GetApprovalByUidQid(int Uid,int Qid)
        {
            
            CodeChallengeEntities db = new CodeChallengeEntities();
            UserInfo userinfo = GetUserInfoByUId(Uid);
           var lis =  (from e in db.Tbl_ApprovalStatus
                    where e.SQId == Qid && e.Tbl_RealPerson1.RP_Userid == Uid
                                     select new ApprovalStatus
                                     {
                                          SQId =e.SQId,
                                          SQPid =(int)e.SQPid,
                                          SQStatus =(int)e.SQStatus,
                                          SQDate =(DateTime)e.SQDate,
                                          ApStatus =(int)e.ApprovalStatus,
                                          ApprovalDate =(DateTime)e.ApprovalDate,
                                          ApprovalPID =(int)e.ApprovalPID,
                                          SAnswerLanguage =(int)e.SAnswerLanguage,
                                          SAnswer =e.SAnswer
                                     }).ToList();

            return lis;

        }




        public int GetPidByUserId(int Uid)
        {
            UserInfo userInfo = GetUserInfoByUId(Uid);
            return userInfo.RP_id;
        }


        public  List<Language> GetLanguages()
        {
            CodeChallengeEntities db = new CodeChallengeEntities();
            List<Language> languages = (from e in db.Tbl_Language
                                     select new Language
                                     {
                                         Lid =(int)e.Lid,
                                         Lname =(string)e.Lname
                                     }
                                     ).ToList();

            return languages;

        }


    }
}