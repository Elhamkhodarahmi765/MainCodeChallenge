using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainCodeChallenge.Repositories
{
    public class CodeChallengeRepository:ICodeChallengeRepository
    {
        private CodeChallengeEntities _context ;
        public CodeChallengeRepository(CodeChallengeEntities context)
        {
            this._context = context;
        }
        public IEnumerable<Tbl_Challenge> GetTbl_Challenge
        {
            get { return _context.Tbl_Challenge.ToList(); }
        }
        public IEnumerable<Tbl_ApprovalStatus> GetTbl_ApprovalStatus
        {
            get { return _context.Tbl_ApprovalStatus.ToList() ; }
        }

    }
}