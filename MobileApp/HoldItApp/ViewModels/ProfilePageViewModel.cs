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
    public class ProfilePageViewModel : BindableObject
    {
        public ICommand postDetailCommand { get; set; }
        public ObservableCollection<UserModel> users { get; set; }
        public ObservableCollection<PostModel> posts { get; set; }
        public UserModel user { get; set; }
        public ObservableCollection<string> userImages { get; set; }
        public string selectedimgUrl { get; set; }
        public PostModel selectedPost { get; set; }
        
        public ProfilePageViewModel()
        {
            
            users = new ObservableCollection<UserModel>();
            posts = new ObservableCollection<PostModel>();
            userImages = new ObservableCollection<string>();
            a();
            
            postDetailCommand = new Command(async () =>
            {
                
                foreach (var post in posts)
                {
                    if (post.imgUrl == selectedimgUrl)
                    {
                        selectedPost = post;
                        break;
                    }
                }
                if (selectedPost == null) return;                
                Shell.Current.ShowPopup(new PopUpDetailsPage(selectedPost));

                selectedPost = null;
                OnPropertyChanged(nameof(selectedPost));
            });
        }

        private async Task getAllPosts()
        {
            string userId = await SecureStorage.GetAsync("userId");                       
            IEnumerable<PostModel> list = await DataService.getPosts();
            foreach (var fn in list)
            {
                if (fn.ownerId == Int32.Parse(userId))
                {
                    UserModel owner = await DataService.getProfileById(fn.ownerId);
                    fn.ownerPic = owner.pPic;
                    fn.ownerName = owner.name;
                    //string[] butcheredDate = fn.time.Split(" ");
                    //fn.time = $"{butcheredDate[1]}. {butcheredDate[2]}. {butcheredDate[4].Split(':')[0]}:{butcheredDate[4].Split(':')[1]}";                        
                    posts.Add(fn);
                }               
            }
            
        }

        private async void a()
        {
            await getAllPosts();
             getAllUploads();
             GetuserData();
        }

        private async Task GetuserData()
        {
            string userName = await SecureStorage.GetAsync("userName");
            string userEmail = await SecureStorage.GetAsync("userEmail");
            string userImage = await SecureStorage.GetAsync("userImage");
            string tempUserId = await SecureStorage.GetAsync("userId");

            int userId;
            bool Success = int.TryParse(tempUserId, out userId);
            if (Success)
            {
                user = new UserModel
                {
                    name = userName,
                    email = userEmail,
                    pPic = userImage,
                    id = userId,                    
                };
                OnPropertyChanged(nameof(user));
            }      
            
        }
        public async Task getAllUploads()
        {
            userImages.Clear();
            int userId = Int32.Parse(await SecureStorage.GetAsync("userId"));
            IEnumerable<string> list = await DataService.getUploads();
            list.ToList().ForEach(fn => {
                if (fn.Split('_')[0] == userId.ToString())
                {
                    userImages.Add($"{DataService.url}/uploads/{fn}");
                }
                
            });
        }
    }

}
