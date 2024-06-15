using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoldItApp.Models
{
    public class PostModel
    {
        public int id { get; set; }
        public string imgUrl { get; set; }
        public string time { get; set; }
        public string comment { get; set; }
        public int ownerId { get; set; }
        public int isPrivate { get; set; }
        public int targetId { get; set; }
        public string ownerName { get; set; }
        public string ownerNamePos { get; set; }
        public string ownerPic { get; set; }
        public int ownerPicPos { get; set; }
        public bool ownerFollowed { get; set; }
        public int gridColumn { get; set; }
        public string textAlingment { get; set; }
        public Color messageColor { get; set; }
        public Color textColor { get; set; }
    }
}
