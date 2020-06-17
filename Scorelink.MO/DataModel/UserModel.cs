using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorelink.MO.DataModel
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Telephone { get; set; }
        public string Status { get; set; }
        public string Admin { get; set; }
        public string RegisterDate { get; set; }
        public string ExpireDate { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDate { get; set; }
    }
}
