using MainCodeChallenge.Infrustructure;
using MainCodeChallenge.Models;
using MainCodeChallenge.Services;
using System;
using System.Collections.Generic;
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

        [HttpGet]
        public RedirectResult success()
        {
          return  Redirect(Url.Action("ProfilePage", "Account"));
           
        }


        [RestrictActionToRole(Roles = new string[] { "AdminPage" })]
        public ActionResult CreateNewChallenge()
        {
            return View();
        }






    }
}