using MainCodeChallenge.Infrustructure;
using MainCodeChallenge.Models;
using MainCodeChallenge.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MainCodeChallenge.Controllers
{
    [Authorize]
    public class ChallengeController : Controller
    {
        // GET: Challenge

        public ICodeChallengeServices service;
        public ChallengeController()
        {
            service = new CodeChallengeServices();
        }

        [RestrictActionToRole(Roles = new string[] { "AdminPage" , "ProfilePage" })]
        public ActionResult DetailsChallenge(int id)
        {
            int Qid=0;
            try
            {
                Qid = id;
            }
            catch
            {
                //Mr Blukian Error Page 
                
            }
            int Uid = int.Parse(HttpContext.Session["UserID"].ToString());
            List<ChallengeApprovalStatus> challengeApprovalStatus = service.GetChallengeDetailsById(Qid);
            List<ApprovalStatus> approvalDone = new List<ApprovalStatus>();
            ApprovalStatus approvalStatus= new ApprovalStatus();
            approvalStatus = service.GetApprovalByUidQid(Uid, Qid).LastOrDefault();
            List<Example> example = service.GetExampleByChallengeId(Qid);
            approvalDone = service.GetApprovalIsDoneByUidQid(Uid,Qid);
            ViewData["approvalDone"] = approvalDone;
            ViewData["ExampleChallenge"] = example;
            ViewData["ApprovalStatus"] = service.GetAllChallengeApprovalStatusPerson(Qid,Uid);
            ViewData["IsItPossibleToPickUp"] =service.IsItPossibleToPickUp(Qid,Uid);
            ViewData["challengeApprovalStatus"] = challengeApprovalStatus;
            ViewData["Uid"] = Uid;
            ViewBag.Languages = service.GetLanguages();
            return View(approvalStatus);
        }

        public ActionResult PickUpChallenge(int Qid,int Uid)
        {
            
            if(service.PickUpChallenge(Qid, Uid))
            {
                Session["Point"] = service.GetPointById(Uid);
                return RedirectToAction("DetailsChallenge", new { id = Qid });
                
            }
            else
            {
                return View();
            }
            

        }

        [HttpPost]
        public  string SubmitChallenge (int Qid, int Uid,string Ans, int lan )
        {
           
            try
            {
                bool status = service.DoneChallenge(Qid, Uid, Ans, lan);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                
                return "error";
            
            }
            return "Ok";
             
        }




        [HttpPost]
        public string SaveNewChallenge (string CH,string CHName,EnumLevel L, int Rpoint,int factor , int Cid)
        {
            int UId = int.Parse(HttpContext.Session["UserID"].ToString());
            int Pid = service.GetPidByUserId(UId);
            try
            {
                bool status = service.SaveChallenge(CH,CHName,(int) L, Rpoint, factor,Pid, Cid);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return "error";

            }
            return "Ok";
        }



        [RestrictActionToRole(Roles = new string[] { "AdminPage" })]
        public ActionResult CreateNewChallenge()
        {
            var level = from EnumLevel s in Enum.GetValues(typeof(EnumLevel))
                           select new { ID = s, Name = s.ToString() };
            ViewBag.levelD = new SelectList(level, "ID", "Name");
            ViewBag.category = service.Getcategory();
            return View();
        }
       
        [RestrictActionToRole(Roles = new string[] {  "ProfilePage" })]
        public ActionResult detailsAnswer(int SId)
        {
            ApprovalStatus approvalStatus = service.GetApprovalStatusBySId(SId);
            return View(approvalStatus);
        }

        [RestrictActionToRole(Roles = new string[] { "AdminPage" })]
        public ActionResult DetailsChallengeAdmin(int Qid)
        {
            List<ApprovalStatus> approvalStatuses= service.GetApprovalIsDoneByQid(Qid);
            return View(approvalStatuses);
        }


        [RestrictActionToRole(Roles = new string[] { "AdminPage" })]
        public ActionResult detailsAnswerAdmin(int SId)
        {
            ApprovalStatus approvalStatus = service.GetApprovalStatusBySId(SId);
            return View(approvalStatus);
        }


        [RestrictActionToRole(Roles = new string[] { "AdminPage" })]
        public ActionResult FinalApprovalAdmin(int SId)
        {
            int UId = int.Parse(HttpContext.Session["UserID"].ToString());
            int Pid = service.GetPidByUserId(UId);
            bool result = service.FinalApproval(SId,Pid);
            int Qid = service.GetApprovalStatusBySId(SId).SQId;
            return RedirectToAction("DetailsChallengeAdmin", "Challenge", new { Qid = Qid });
        }

        public ActionResult ShowAnswer([Required] int? Qid)
        {
            if (ModelState.IsValid)
            {

                ViewBag.Qid = Qid;
                return View(service.GetAllApprovalAnswer(Qid.Value));
            }
            else
            {
                return View();
            }
        }
    }
}