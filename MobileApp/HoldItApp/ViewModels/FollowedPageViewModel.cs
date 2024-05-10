using CommunityToolkit.Maui.Views;
using HoldItApp.Models;
using HoldItApp.Services;
using HoldItApp.Views;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HoldItApp.ViewModels
{
    public class FollowedPageViewModel: BindableObject
    {
        public ObservableCollection<UserModel> followedUsers { get; set; }
        public ObservableCollection<UserModel> allUsers { get; set; }
        public UserModel followedUser { get; set; }
        public ICommand userSelectionCommand { get; set; }

        public FollowedPageViewModel()
        {
            followedUsers = new ObservableCollection<UserModel>();
            allUsers = new ObservableCollection<UserModel>();
            followedUser = new UserModel();
            getFollowed();

            userSelectionCommand = new Command(() =>
            {
                if (followedUser == null) return;
                Shell.Current.ShowPopup(new PopUpUserPage(followedUser));

                followedUser = null;
                OnPropertyChanged(nameof(followedUser));
            });
        }

        private async void getFollowed()
        {                       
            await getUsers();
            int userId = Int32.Parse(await SecureStorage.GetAsync("userId"));
            UserModel user = await DataService.getProfileById(userId);
            string followed = user.followed;
            string[] followedArray = followed.Split(" +");
            followedArray = followedArray.Take(followedArray.Length - 1).ToArray();
            foreach (var potentiallyFollowed in allUsers)
            {
                foreach (var id in followedArray)
                {
                    if (potentiallyFollowed.id == Int32.Parse(id) && !id.IsNullOrEmpty())
                    {
                        followedUsers.Add(potentiallyFollowed);
                    }
                }
               
            }

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
