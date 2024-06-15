using HoldItApp.Models;
using HoldItApp.Services;
using Microsoft.IdentityModel.Tokens;
using Svg;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HoldItApp.ViewModels
{
    public class PopUpDetailsViewModel: BindableObject, INotifyPropertyChanged
    {

        public int ownerId { get; set; }
        public string ownerName { get; set; }
        public string ownerPic { get; set; }
        public string imgUrl { get; set; }
        public string comment { get; set; }
        private bool _isVisible { get; set; }
        public bool isVisible { 
            get => _isVisible;
            set 
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    OnPropertyChanged(nameof(isVisible));
                }
            } 
        }        
        private bool _isVisibleInvers { get; set; }
        public bool isVisibleInvers
        {
            get => _isVisibleInvers;
            set
            {
                if (_isVisibleInvers != value)
                {
                    _isVisibleInvers = value;
                    OnPropertyChanged(nameof(isVisibleInvers));
                }
            }
        }
        public ObservableCollection<UserModel> followedUsers { get; set; }
        public ObservableCollection<UserModel> allUsers { get; set; }
        public UserModel targetUser { get; set; }
        public ICommand followCommand { get; set; }
        public ICommand unFollowCommand { get; set; }
        public PostModel post { get; set; }
        public PopUpDetailsViewModel(PostModel pm)
        {
            allUsers = new ObservableCollection<UserModel>();
            followedUsers = new ObservableCollection<UserModel>();          
            getFollowed();
            post = pm;
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
                    isVisibleInvers = true;
                    isVisible = !isVisibleInvers;
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
                    isVisibleInvers = false;
                    isVisible = !isVisibleInvers;
                }
               
            });

        }

        private async void getFollowed()
        {
            //await getUsers();
            int userId = Int32.Parse(await SecureStorage.GetAsync("userId"));
            UserModel user = await DataService.getProfileById(userId);            
            string[] followedArray = user.followed.Split(" +");
            followedArray = followedArray.Take(followedArray.Length - 1).ToArray();            
            isVisibleInvers = followedArray.Contains(post.ownerId.ToString());
            isVisible = !isVisibleInvers;
            
            //foreach (var potentiallyFollowed in allUsers)
            //{
            //    foreach (var id in followedArray)
            //    {
            //        if (potentiallyFollowed.id == Int32.Parse(id) && !id.IsNullOrEmpty())
            //        {
            //            followedUsers.Add(potentiallyFollowed);
            //            if(potentiallyFollowed.id == ownerId)
            //            {
            //                targetUser = potentiallyFollowed;
            //            }
            //        }
            //    }

            //}
            //    if (!followedUsers.Contains(targetUser)|| targetUser == null)
            //{
            //    //Follow
            //    isVisibleInvers = false;
            //    isVisible = true;
            //}
            //else
            //{
            //    //Unfollow
            //    isVisibleInvers = true;
            //    isVisible = false;
            //}

        }
        private async Task getUsers()
        {
            allUsers.Clear();
            IEnumerable<UserModel> list = await DataService.getProfiles();
            list.ToList().ForEach(fn => {
                allUsers.Add(fn);

            });
        }
    }
}
