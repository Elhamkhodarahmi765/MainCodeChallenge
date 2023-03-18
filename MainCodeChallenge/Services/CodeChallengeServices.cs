using MainCodeChallenge.Models;
using MainCodeChallenge.Repositories;
using MainCodeChallenge.Utilities;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;

namespace MainCodeChallenge.Services
{
    public class CodeChallengeServices : ICodeChallengeServices
    {
       
        public bool AuthenticateInActiveDirectory(string username, string password)
        {
            PrincipalContext principalContext;//= new PrincipalContext(authenticationType);
            bool isAuthenticated = false;
            UserPrincipal userPrincipal = null;
            string txtDomain = "";
            //int AuturizeLevel = 1;
            if (txtDomain.Equals(""))
            {
                try
                {
                    principalContext = new PrincipalContext(ContextType.Domain);
                    isAuthenticated = principalContext.ValidateCredentials(username, password, ContextOptions.Negotiate);
                    userPrincipal = UserPrincipal.FindByIdentity(principalContext, username);
                    if (userPrincipal != null && isAuthenticated)
                    {
                        return true;

                    }else
                    {
                        return false;
                    }
                }
                catch (Exception esException)
                {
                    return false;
                }
            }
            return false;
        }

        public UserInfo FindInActiveDirectory2(string username)
        {
            PrincipalContext principalContext;//= new PrincipalContext(authenticationType);
            //bool isAuthenticated = false;
            UserPrincipal userPrincipal = null;
            string txtDomain = "";
            //int AuturizeLevel = 1;

            if (txtDomain.Equals(""))
            {
                try
                {
                    principalContext = new PrincipalContext(ContextType.Domain);

                    //isAuthenticated = principalContext.ValidateCredentials(username, password, ContextOptions.Negotiate);
                    userPrincipal = UserPrincipal.FindByIdentity(principalContext, username);
                    UserInfo userInfo = new UserInfo();
                    //txtResault.Text += "isAuthenticated:" + isAuthenticated + "\n";
                    if (userPrincipal != null)
                    {
                        userInfo.RealPersonFullname = userPrincipal.DisplayName;
                        userInfo.EmailAddress = userPrincipal.EmailAddress;
                        userInfo.username = username;
                        return userInfo;

                    }
                }
                catch (Exception esException)
                {

                }
            }
            return new UserInfo();
        }


        public bool CreateUser(string username)
        {
            try
            {
                UserInfo userInfo = FindInActiveDirectory2(username);
                CodeChallengeEntities db = new CodeChallengeEntities();
                Tbl_User user = new Tbl_User();
                user.UuserName = user.UuserName;
                user.Role = 1;
                user.UActiveStatus = true;
                db.Tbl_User.Add(user);
                db.SaveChanges();
                return CreateRealPerson(userInfo ,user.Uid);
            }
            catch
            {
                return false;
            }

        }

        public bool CreateRealPerson(UserInfo userInfo ,int Uid)
        {
            try
            {
                CodeChallengeEntities db = new CodeChallengeEntities();
                Tbl_RealPerson realperson = new Tbl_RealPerson();
                realperson.RP_Userid = Uid;
                realperson.RP_Role = 1;
                realperson.RP_FName = userInfo.RealPersonFullname;
                realperson.RP_EmailAddress = userInfo.EmailAddress;
                db.Tbl_RealPerson.Add(realperson);
                db.SaveChanges();

                int RP_id = realperson.RP_id;
                return CreateRealPersonpoint(Uid, RP_id, 200); 
            }
            catch
            {
                return false;
            }
        }

        public Boolean CreateRealPersonpoint(int Uid, int RP_id, int Point)
        {
            try
            {
                CodeChallengeEntities db = new CodeChallengeEntities();
                Tbl_RealPesronPoint realpersonP = new Tbl_RealPesronPoint();
                realpersonP.PUserId  = Uid;
                realpersonP.RP_id = RP_id;
                realpersonP.PPoint = Point;
                db.Tbl_RealPesronPoint.Add(realpersonP);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }



        public bool AuthenticateAD(string username)
        {
            if (string.IsNullOrEmpty(username) )
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
                        
                        return CreateUser(username);
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
            return false;
        }











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
                         where Qid.StatusRow==1 || Qid.StatusRow==null
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







        public List<ChallengeApprovalStatusP> GetAllChallengeApprovalStatusCountByUid(int Uid)
        {
            int Upid = GetPidByUserId(Uid);
            CodeChallengeEntities db = new CodeChallengeEntities();
            List<ChallengeApprovalStatusP> challengeApprovalStatusP = (from ch in db.Tbl_Challenge
                                                                     join Aps in db.Tbl_ApprovalStatus on ch.Qid equals Aps.SQId into ChAps
                                                                     from Qid in ChAps.DefaultIfEmpty()
                                                                     where Qid.StatusRow == 1 || Qid.StatusRow == null
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
                                                                     select new ChallengeApprovalStatusP
                                                                     {
                                                                         Qid = grp.Key.Qid,
                                                                         QDescription = grp.Key.QDescription,
                                                                         QName = grp.Key.QName,
                                                                         QLevel = (EnumLevel)grp.Key.QLevel,
                                                                         LPointReceived = (int?)grp.Key.LPointReceived ?? 0,
                                                                         LPointsRequired = (int?)grp.Key.LPointsRequired ?? 0,
                                                                         QRpoint = (int?)grp.Key.QRpoint ?? 0,
                                                                         QApoint = (int?)grp.Key.QApoint ?? 0,
                                                                         CT_Name = grp.Key.CT_Name,
                                                                         POwnerName = grp.Key.RP_FName + " " + grp.Key.RP_LName,
                                                                         QpersonOwner = (int?)grp.Key.QpersonOwner ?? 0,
                                                                         CountOfA = grp.Where(t => t.Qid.SQId != null).Count(),
                                                                         solved = grp.Any(t => t.Qid.SQPid == Upid && t.Qid.ApprovalStatus == 2)
                                                                     }).ToList();

            return challengeApprovalStatusP;
        }

        public List<ChallengeApprovalStatus> GetChallengeDetailsById(int Id)
         {
            CodeChallengeEntities db = new CodeChallengeEntities();
            List<ChallengeApprovalStatus> challengeApprovalStatus = (from ch in db.Tbl_Challenge
                         join Aps in db.Tbl_ApprovalStatus on ch.Qid equals Aps.SQId into ChAps
                         from Qid in ChAps.DefaultIfEmpty()
                         where Qid.StatusRow == 1 || Qid.StatusRow == null
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
                             ch.QpersonOwner,
                             ch.Qfactor,
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
                             CountOfA = grp.Where(t => t.Qid.SQId != null).Count(),
                             Qfactor= (int?)grp.Key.Qfactor ?? 0
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
            int Pid = 0;
            ChallengeApprovalStatus ChallengeApprovalStatus = GetChallengeDetailsById(Qid).FirstOrDefault();
            Rpoint = ChallengeApprovalStatus.QRpoint;
            Pid = ChallengeApprovalStatus.QpersonOwner;


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
                    Aps.ApprovalPID = Pid;
                    Aps.StatusRow =(int) EnumStatusRow.MainRow;
                    db.Tbl_ApprovalStatus.Add(Aps);
                    db.SaveChanges();

                    //point
                    ChangePoint(userinfo.RP_id, Rpoint,EnumPointParam.Decrease );

                    trans.Commit();
                    return true;
                }
                else
                {
                    return false;
                }
                
            }

        }

        public void ChangePoint(int Uid,int point, EnumPointParam enumPointParam )
        {
            CodeChallengeEntities db=new CodeChallengeEntities();
            var result = db.Tbl_RealPesronPoint.SingleOrDefault(RP => RP.RP_id == Uid);
            if (enumPointParam == EnumPointParam.Decrease)
            {
                if (result != null)
                {
                    try
                    {
                        result.PPoint -= point;
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            else
            {
                if (result != null)
                {
                    try
                    {
                        result.PPoint += point;
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
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
            //var result = db.Tbl_ApprovalStatus.Last(RP => RP.SQPid == PUid && RP.SQId == Qid);
            //MR Blukian
            var result = db.Tbl_ApprovalStatus.Where(RP => RP.SQPid == PUid && RP.SQId == Qid).OrderByDescending(RP => RP.SId).FirstOrDefault();

            if (result != null)
            {
                if(result.SQStatus==(int)EnumSQStatus.PickUp )
                {
                    try
                    {
                        result.SQDate = DateTime.Now;
                        result.SAnswer = AnsText;
                        result.SAnswerLanguage = lan;
                        result.SQStatus = (int)EnumSQStatus.Done;
                        result.SQDate = DateTime.Now;
                        result.ApprovalStatus = 1;
                        db.SaveChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }else if (result.SQStatus == (int)EnumSQStatus.Done)
                {

                    Tbl_ApprovalStatus Aps = new Tbl_ApprovalStatus();
                    Aps.SQId = Qid;
                    Aps.SQPid = PUid;
                    Aps.SQDate = DateTime.Now;
                    Aps.ApprovalPID = result.ApprovalPID;
                    Aps.ApprovalStatus = 1;
                    //editDone
                    Aps.SQDate = DateTime.Now;
                    Aps.SAnswer = AnsText;
                    Aps.SAnswerLanguage = lan;
                    Aps.SQStatus = (int)EnumSQStatus.Done;
                    Aps.SQDate = DateTime.Now;
                    Aps.StatusRow =(int) EnumStatusRow.SubRow;
                    db.Tbl_ApprovalStatus.Add(Aps);
                    db.SaveChanges();
                    return true;
                }
               
            }
            return false;
        }


        public bool SaveChallenge(string CH,string CHName, int L, int Rpoint, int factor,int Pid,int Cid)
        {
            try
            {
                CodeChallengeEntities db = new CodeChallengeEntities();
                Tbl_Challenge challenge = new Tbl_Challenge();
                challenge.QLevel = L;
                challenge.QName = CHName;
                challenge.Qfactor = factor;
                challenge.QRpoint = Rpoint;
                challenge.QpersonOwner = Pid;
                challenge.QDescription = CH;
                challenge.QCId = (byte)Cid;
                db.Tbl_Challenge.Add(challenge);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
  
           
        }




        public List<ApprovalStatus> GetApprovalByUidQid(int Uid,int Qid)
        {
            
            CodeChallengeEntities db = new CodeChallengeEntities();
            UserInfo userinfo = GetUserInfoByUId(Uid);
            var lis =  (from e in db.Tbl_ApprovalStatus
                    where e.SQId == Qid && e.SQPid==userinfo.RP_id
                                     select new ApprovalStatus
                                     {
                                         SId=(int)e.SId,
                                          SQId =(int?)e.SQId ??0,
                                          SQPid =(int?)e.SQPid ??0,
                                          SQStatus =(int?)e.SQStatus??0,
                                          SQDate =(DateTime)e.SQDate,
                                          ApStatus =(int?)e.ApprovalStatus??0,
                                          ApprovalDate =(DateTime)e.ApprovalDate,
                                          ApprovalPID =(int?)e.ApprovalPID??0,
                                          SAnswerLanguage =(int?)e.SAnswerLanguage??0,
                                          SAnswer =e.SAnswer,
                                          Lname = e.Tbl_Language.Lname,
                                         QStatus = (EnumSQStatus?)e.SQStatus ?? 0,
                                         approvalStatus =(EnumApprovalStatus)e.ApprovalStatus
                                     }).ToList();

             return lis;

        }



        public List<ApprovalStatus> GetApprovalIsDoneByUidQid(int Uid, int Qid)
        {

            CodeChallengeEntities db = new CodeChallengeEntities();
            UserInfo userinfo = GetUserInfoByUId(Uid);
            var lis = (from e in db.Tbl_ApprovalStatus
                       where e.SQId == Qid && e.SQPid == userinfo.RP_id && e.SQStatus==2
                       select new ApprovalStatus
                       {
                           SId=(int)e.SId,
                           SQId = (int?)e.SQId ?? 0,
                           SQPid = (int?)e.SQPid ?? 0,
                           SQStatus = (int?)e.SQStatus ?? 0,
                           SQDate = (DateTime)e.SQDate,
                           ApStatus = (int?)e.ApprovalStatus ?? 0,
                           ApprovalDate = (DateTime)e.ApprovalDate,
                           ApprovalPID = (int?)e.ApprovalPID ?? 0,
                           SAnswerLanguage = (int?)e.SAnswerLanguage ?? 0,
                           SAnswer = e.SAnswer,
                           Lname = e.Tbl_Language.Lname,
                           approvalStatus = (EnumApprovalStatus)e.ApprovalStatus
                       }).ToList();

            return lis;

        }


        public List<ApprovalStatus> GetApprovalIsDoneByQid( int Qid)
        {
            CodeChallengeEntities db = new CodeChallengeEntities();
            var lis = (from e in db.Tbl_ApprovalStatus
                       where e.SQId == Qid  && e.SQStatus == 2
                       select new ApprovalStatus
                       {
                           SId = (int)e.SId,
                           SQId = (int?)e.SQId ?? 0,
                           SQPid = (int?)e.SQPid ?? 0,
                           SQStatus = (int?)e.SQStatus ?? 0,
                           SQDate = (DateTime)e.SQDate,
                           ApStatus = (int?)e.ApprovalStatus ?? 0,
                           ApprovalDate = (DateTime)e.ApprovalDate,
                           ApprovalPID = (int?)e.ApprovalPID ?? 0,
                           SAnswerLanguage = (int?)e.SAnswerLanguage ?? 0,
                           SAnswer = e.SAnswer,
                           Lname = e.Tbl_Language.Lname,
                           approvalStatus = (EnumApprovalStatus)e.ApprovalStatus,
                           PersonelName=e.Tbl_RealPerson.RP_FName + " " + e.Tbl_RealPerson.RP_LName,
                           APpersonelName=e.Tbl_RealPerson1.RP_FName  + " " + e.Tbl_RealPerson1.RP_LName 

                       }).ToList();

            return lis;

        }



        public ApprovalStatus GetApprovalStatusBySId(int Sid)
        {
            CodeChallengeEntities db = new CodeChallengeEntities();
            var lis = (from e in db.Tbl_ApprovalStatus
                       where e.SId==Sid
                       select new ApprovalStatus
                       {
                           SId = (int)e.SId,
                           SQId = (int?)e.SQId ?? 0,
                           SQPid = (int?)e.SQPid ?? 0,
                           SQStatus = (int?)e.SQStatus ?? 0,
                           QStatus = (EnumSQStatus?)e.SQStatus ??0,
                           SQDate = (DateTime)e.SQDate,
                           ApStatus = (int?)e.ApprovalStatus ?? 0,
                           ApprovalDate = (DateTime)e.ApprovalDate,
                           ApprovalPID = (int?)e.ApprovalPID ?? 0,
                           SAnswerLanguage = (int?)e.SAnswerLanguage ?? 0,
                           SAnswer = e.SAnswer,
                           Lname = e.Tbl_Language.Lname,
                           approvalStatus = (EnumApprovalStatus)e.ApprovalStatus
                       }).ToList();

            return lis.FirstOrDefault();

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




        public List<Category> Getcategory()
        {
            CodeChallengeEntities db = new CodeChallengeEntities();
            List<Category> Category = (from e in db.Tbl_Category
                                        select new Category
                                        {
                                            CId = (int)e.CT_Id,
                                            CName = (string)e.CT_Name
                                        }
                                     ).ToList();

            return Category;

        }


        public List<ApprovalStatus> GetAllApprovalAnswer(int Qid)
        {
            CodeChallengeEntities db = new CodeChallengeEntities();
            var languages = GetLanguages();
            var model = (from e in db.Tbl_ApprovalStatus
                         where e.SQId == Qid && e.ApprovalStatus == (int)EnumApprovalStatus.FinalApproval

                         select new ApprovalStatus()
                         {
                             SId = (int)e.SId,
                             SQId = (int?)e.SQId ?? 0,
                             SQPid = (int?)e.SQPid ?? 0,
                             SQStatus = (int?)e.SQStatus ?? 0,
                             QStatus = (EnumSQStatus?)e.SQStatus ?? 0,
                             SQDate = (DateTime)e.SQDate,
                             ApStatus = (int?)e.ApprovalStatus ?? 0,
                             ApprovalDate = (DateTime)e.ApprovalDate,
                             ApprovalPID = (int?)e.ApprovalPID ?? 0,
                             SAnswerLanguage = (int?)e.SAnswerLanguage ?? 0,
                             Language = e.Tbl_Language.Lname,
                             SAnswer = e.SAnswer,
                             Lname = e.Tbl_Language.Lname,
                             approvalStatus = (EnumApprovalStatus)e.ApprovalStatus
                         }).ToList();
            return model;

        }






        public bool FinalApproval (int Sid, int Pid)
        {
            CodeChallengeEntities db = new CodeChallengeEntities();
            var result = db.Tbl_ApprovalStatus.Where(RP => RP.SId == Sid ).FirstOrDefault();

            if (result != null)
            {
                if (result.ApprovalStatus == (int)EnumApprovalStatus.AwaitingFinalApproval)
                {
                    try
                    {
                        result.ApprovalDate = DateTime.Now;
                        result.ApprovalStatus = 2;
                        result.ApprovalPID = Pid;
                        db.SaveChanges();
                        int Qid = (int)result.SQId;
                        ChallengeApprovalStatus challengeApprovalStatus = GetChallengeDetailsById(Qid).FirstOrDefault();
                        int point = challengeApprovalStatus.QRpoint * challengeApprovalStatus.Qfactor ;
                        ChangePoint((int)result.SQPid, point, EnumPointParam.Increase);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }


            }else
            {
                return false;
            }
        }

    }
}