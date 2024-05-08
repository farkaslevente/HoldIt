using HoldItApp.Models;
using HoldItApp.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoldItApp.ViewModels
{
    public class TimelinePageViewModel: BindableObject
    {
        public ObservableCollection<PostModel> posts { get; set; }
        public bool first { get; set; }
        private System.Timers.Timer _refreshTimer;
        public TimelinePageViewModel()
        {
            first = true;
            posts = new ObservableCollection<PostModel>();
            getAllPosts();
            
            
            
        }
        private void InitializeTimer()
        {            
            _refreshTimer = new System.Timers.Timer();
            _refreshTimer.Interval = TimeSpan.FromSeconds(5).TotalMilliseconds;
            _refreshTimer.Elapsed += async (sender, e) => await RefreshDataAsync();
            _refreshTimer.Start();
        }

        private async Task RefreshDataAsync()
        {           
            string userId = await SecureStorage.GetAsync("userId");
            IEnumerable<PostModel> list = await DataService.getPosts();

            List<PostModel> postsToAdd = new List<PostModel>();

            foreach (var fn in list)
            {               
                if (!posts.Any(item => item.id == fn.id))
                {
                    UserModel owner = await DataService.getProfileById(fn.ownerId);
                    fn.ownerPic = owner.pPic;

                    string[] butcheredDate = fn.time.Split(" ");
                    fn.time = $"{butcheredDate[3]}. {butcheredDate[1]}. {butcheredDate[2]}.";

                    if (userId.IsNullOrEmpty())
                    {
                        userId = "0";
                    }
                    fn.gridColumn = fn.ownerId == Int32.Parse(userId) ? 1 : 0;
                    fn.messageColor = fn.ownerId == Int32.Parse(userId) ? Colors.Blue : Colors.Black;
                    fn.ownerPicPos = fn.ownerId == Int32.Parse(userId) ? 2 : 0;                    
                                     
                    fn.textColor = Colors.White;

                    if (fn.imgUrl.IsNullOrEmpty())
                    {
                        fn.imgUrl = "onebyone.png";
                    }



                    postsToAdd.Add(fn);
                }               
            }        

            foreach (var post in postsToAdd)
            {
                posts.Add(post);
            }          
        }

        private async void getAllPosts()
        {
            string userId = await SecureStorage.GetAsync("userId");
            posts.Clear();
            IEnumerable<PostModel> list = await DataService.getPosts();
            list.ToList().ForEach(async fn =>
            {
                UserModel owner = await DataService.getProfileById(fn.ownerId);
                fn.ownerPic = owner.pPic;
                fn.ownerPicPos = fn.ownerId == Int32.Parse(userId) ? 2 : 0;
                string[] butcheredDate = fn.time.Split(" ");
                fn.time = $"{butcheredDate[3]}. {butcheredDate[1]}. {butcheredDate[2]}.";
                
                if (userId.IsNullOrEmpty())
                {
                    userId = "0";
                }
                fn.gridColumn = 0;
                fn.messageColor = Colors.Black;
                fn.textColor = Colors.White;
                fn.textAlingment = "Start";
                if (fn.ownerId == Int32.Parse(userId))
                {
                    fn.gridColumn = 1;
                    fn.messageColor = Colors.Blue;
                    fn.textColor = Colors.White;
                    fn.textAlingment = "End";
                }
                if (fn.imgUrl.IsNullOrEmpty())
                {
                    fn.imgUrl = "onebyone.png";
                }

                posts.Add(fn);
            });
            InitializeTimer();
        }
    }
}
