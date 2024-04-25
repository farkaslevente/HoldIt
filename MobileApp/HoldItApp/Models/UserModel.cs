using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoldItApp.Models
{
    public class UserModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }        
        public string pPic { get; set; }
        public string followed { get; set; }
        public string role { get; set; }
    }
}
