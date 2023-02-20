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
        EnumRolePage GetActionToGoFromLoginPage(string username);
        List<ChallengeApprovalStatus> GetAllChallengeApprovalStatusCount();
        List<ChallengeApprovalStatus> GetChallengeDetailsById(int Id);
        List<Example> GetExampleByChallengeId(int Id);

    }  
}
