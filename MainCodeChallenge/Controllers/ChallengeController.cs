using MainCodeChallenge.Infrustructure;
using MainCodeChallenge.Models;
using MainCodeChallenge.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
            ApprovalStatus approvalStatus= new ApprovalStatus();
            approvalStatus.SQPid = service.GetPidByUserId(Uid);
            approvalStatus.SQId = Qid;
            List<Example> example = service.GetExampleByChallengeId(Qid);
            ViewData["ExampleChallenge"] = example;
            ViewData["ApprovalStatus"] = service.GetAllChallengeApprovalStatusPerson(Qid,Uid);
            ViewData["IsItPossibleToPickUp"] =service.IsItPossibleToPickUp(Qid,Uid);
            ViewData["challengeApprovalStatus"] = challengeApprovalStatus;
            ViewData["Uid"] = Uid;
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

    }
}