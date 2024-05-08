using HoldItApp.Models;
using HoldItApp.Services;
using Svg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoldItApp.ViewModels
{
    public class PopUpDetailsViewModel: BindableObject
    {

        public int ownerId { get; set; }
        public string ownerName { get; set; }
        public string ownerPic { get; set; }
        public string imgUrl { get; set; }
        public string comment { get; set; }
        public PopUpDetailsViewModel(PostModel pm)
        {
            ownerId = pm.ownerId;
            ownerName = pm.ownerName;
            ownerPic = pm.ownerPic;
            imgUrl = pm.imgUrl;
            comment = pm.comment;
        }
    }
}
