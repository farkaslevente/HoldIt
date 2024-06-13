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
    public class PrivateTimelinePageViewModel: BindableObject, IQueryAttributable
    {
        public ObservableCollection<PostModel> posts { get; set; }
        public ObservableCollection<UserModel> users { get; set; }
        public PostModel selectedPost { get; set; }
        public UserModel target { get; set; }
        public ICommand infoCommand { get; set; }        
        private System.Timers.Timer _refreshTimer;
        public PrivateTimelinePageViewModel()
        {           
            posts = new ObservableCollection<PostModel>();
            users = new ObservableCollection<UserModel>();
            getAllPosts();
            infoCommand = new Command(() =>
            {
                if (selectedPost == null) return;
                Shell.Current.ShowPopup(new PopUpDetailsPage(selectedPost));

                selectedPost = null;
                OnPropertyChanged(nameof(selectedPost));
            });


        }
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("followedUser"))
            {
                target = query["followedUser"] as UserModel;
                await SecureStorage.SetAsync("targetId", target.id.ToString());                    
            }



        }
        private async Task getUsers()
        {
            users.Clear();
            IEnumerable<UserModel> list = await DataService.getProfiles();
            list.ToList().ForEach(fn => {
                users.Add(fn);
            });
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
                if (fn.isPrivate == 1 && ((fn.ownerId == Convert.ToInt32(userId) && fn.targetId == target.id)||(fn.ownerId == target.id && fn.targetId == Convert.ToInt32(userId))))
                {
                    if (!posts.Any(item => item.id == fn.id))
                    {                        
                        UserModel owner = users.Where(x => x.id == fn.ownerId).First();
                        fn.ownerPic = owner.pPic;

                        string[] butcheredDate = fn.time.Split(" ");
                        fn.time = $"{butcheredDate[1]}. {butcheredDate[2]}. {butcheredDate[4].Split(':')[0]}:{butcheredDate[4].Split(':')[1]}";

                        fn.gridColumn = fn.ownerId == Int32.Parse(userId) ? 1 : 0;
                        fn.messageColor = fn.ownerId == Int32.Parse(userId) ? Colors.Blue : Colors.Black;
                        fn.ownerPicPos = fn.ownerId == Int32.Parse(userId) ? 2 : 0;
                        fn.ownerName = owner.name;
                        fn.ownerNamePos = fn.ownerId == Int32.Parse(userId) ? "End" : "Start";
                        fn.textColor = Colors.White;

                        if (fn.imgUrl.IsNullOrEmpty())
                        {
                            fn.imgUrl = "onebyone.png";
                        }



                        postsToAdd.Add(fn);
                    }
                }
            }

            foreach (var post in postsToAdd)
            {
                posts.Add(post);
            }
        }

        private async Task getAllPosts()
        {
            await getUsers();
            string userId = await SecureStorage.GetAsync("userId");    
            posts.Clear();
            IEnumerable<PostModel> list = await DataService.getPosts();
            list.ToList().ForEach(async fn =>
            {
                if (fn.isPrivate == 1 && ((fn.ownerId == Convert.ToInt32(userId) && fn.targetId == target.id) || (fn.ownerId == target.id && fn.targetId == Convert.ToInt32(userId))))            
                {
                    UserModel owner = users.Where(x => x.id == fn.ownerId).First();
                    fn.ownerPic = owner.pPic;
                    fn.ownerPicPos = fn.ownerId == Int32.Parse(userId) ? 2 : 0;
                    fn.ownerName = owner.name;
                    fn.ownerNamePos = fn.ownerId == Int32.Parse(userId) ? "End" : "Start";
                    string[] butcheredDate = fn.time.Split(" ");
                    fn.time = $"{butcheredDate[1]}. {butcheredDate[2]}. {butcheredDate[4].Split(':')[0]}:{butcheredDate[4].Split(':')[1]}";


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
                }
            });
            InitializeTimer();
        }
    }
}
