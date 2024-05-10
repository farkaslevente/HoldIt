using HoldItApp.Models;
using HoldItApp.Services;
using Microsoft.IdentityModel.Tokens;
using Svg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HoldItApp.ViewModels
{
    public class PopUpDetailsViewModel: BindableObject
    {

        public int ownerId { get; set; }
        public string ownerName { get; set; }
        public string ownerPic { get; set; }
        public string imgUrl { get; set; }
        public string comment { get; set; }
        public ICommand followCommand { get; set; }
        public ICommand unFollowCommand { get; set; }
        public PopUpDetailsViewModel(PostModel pm)
        {
            ownerId = pm.ownerId;
            ownerName = pm.ownerName;
            ownerPic = pm.ownerPic;
            imgUrl = pm.imgUrl;
            comment = pm.comment;

            followCommand = new Command(async () =>
            {
                string uName = await SecureStorage.GetAsync("userName");
                if (uName.IsNullOrEmpty())
                {
                    await Shell.Current.DisplayAlert("Unauthorized action detected", "To follow someone you must log into our application first!", "Go back");
                }
                else
                {
                    await DataService.adProfileFollow(pm.ownerId);
                }
                
            });

            unFollowCommand = new Command(async () =>
            {
                string uName = await SecureStorage.GetAsync("userName");
                if (uName.IsNullOrEmpty())
                {
                    await Shell.Current.DisplayAlert("Unauthorized action detected", "To unfollow someone you must log into our application first!", "Go back");
                }
                else
                {
                    await DataService.removeProfileFollow(pm.ownerId);
                }
            });

        }
    }
}
