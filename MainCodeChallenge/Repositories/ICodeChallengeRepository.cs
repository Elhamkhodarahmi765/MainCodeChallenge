using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainCodeChallenge.Repositories
{
    internal interface ICodeChallengeRepository
    {
        IEnumerable<Tbl_Challenge> GetTbl_Challenge { get; }
        IEnumerable<Tbl_ApprovalStatus> GetTbl_ApprovalStatus { get; }

    }
}
