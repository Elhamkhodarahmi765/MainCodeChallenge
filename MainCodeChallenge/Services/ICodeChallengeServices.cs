using MainCodeChallenge.Models;
using MainCodeChallenge.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainCodeChallenge.Services
{
    public interface ICodeChallengeServices
    {
        bool Authenticate(string username, string password);
        Tbl_User GetActionToGoFromLoginPage(string username);
        List<ChallengeApprovalStatus> GetAllChallengeApprovalStatusCount();
        List<ChallengeApprovalStatus> GetChallengeDetailsById(int Id);
        List<Example> GetExampleByChallengeId(int Id);
        UserInfo GetUserInfoByUId(int UID);
        bool GetAllChallengeApprovalStatusPerson(int Qid, int Uid);
        bool IsItPossibleToPickUp(int Qid, int Uid);
        bool PickUpChallenge(int Qid, int Uid);
        bool DoneChallenge(int Qid, int Uid, string AnsText, int lan);
        int GetPidByUserId(int Uid);
        List<ApprovalStatus> GetApprovalByUidQid(int Uid, int Qid);
        List<Language> GetLanguages();
        List<ApprovalStatus> GetApprovalIsDoneByUidQid(int Uid, int Qid);
        List<ChallengeApprovalStatusP> GetAllChallengeApprovalStatusCountByUid(int Uid);
        ApprovalStatus GetApprovalStatusBySId(int Sid);
        bool SaveChallenge(string CH, string CHName, int L, int Rpoint, int factor, int Pid);
        List<ApprovalStatus> GetApprovalIsDoneByQid(int Qid);
    }  
}
