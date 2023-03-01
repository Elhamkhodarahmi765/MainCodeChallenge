﻿using MainCodeChallenge.Infrustructure;
using MainCodeChallenge.Models;
using MainCodeChallenge.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MainCodeChallenge.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public ICodeChallengeServices service;
        public AccountController() 
        {
            service = new CodeChallengeServices();
        }


        [AllowAnonymous]
        public ActionResult login()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(Account model )
        {
            if (!ModelState.IsValid)
                return View(model);
            //I have to add ActiveDirectoryCode (MR BBlukian)
            bool result = service.Authenticate(model.user, model.password);
            if (result == true)
            {
                //Q Mr Blukian
                FormsAuthentication.SetAuthCookie(model.user, false);
                var user = service.GetActionToGoFromLoginPage(model.user);
                Session["UserRole"] = user.getRole().ToString();
                Session["UserID"] = user.Uid;
                return RedirectToAction(user.getRole().ToString());
            }

            ViewBag.ErrorMessage = "The username or password is incorrect.";
            return View(model);

        }

        [RestrictActionToRole(Roles = new string[] { "AdminPage" })]
        public ActionResult AdminPage()
        {
            int UId = int.Parse(HttpContext.Session["UserID"].ToString());
            int Pid=service.GetPidByUserId(UId);
            List<ChallengeApprovalStatus> challengeApprovalStatus= service.GetAllChallengeApprovalStatusCount().Where(ch=>ch.QpersonOwner==Pid).ToList();
            return View(challengeApprovalStatus);
        }


        [RestrictActionToRole(Roles = new string[] { "ProfilePage" })]
        public ActionResult ProfilePage()
        {
            var user  = HttpContext.Session["UserID"].ToString();
            int UId = int.Parse(HttpContext.Session["UserID"].ToString());
            string fullName = service.GetUserInfoByUId(UId).RealPersonFullname;
            List<ChallengeApprovalStatusP> challengeApprovalStatusP = service.GetAllChallengeApprovalStatusCountByUid(UId);
            return View(challengeApprovalStatusP);
        }

        [AllowAnonymous]
        public ActionResult ErrorPage()
        {
            return View();
        }
        
    }

}