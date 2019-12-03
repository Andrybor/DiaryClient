using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Repositories.Models
{
    public class AccountWithToken
    {
        public Account Account { get; set; }
        public string Token { get; set; }
    }
}
