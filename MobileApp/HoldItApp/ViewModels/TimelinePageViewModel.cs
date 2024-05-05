using HoldItApp.Models;
using HoldItApp.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoldItApp.ViewModels
{
    public class TimelinePageViewModel: BindableObject
    {
        public ObservableCollection<PostModel> posts { get; set; }

        public TimelinePageViewModel()
        {
            posts = new ObservableCollection<PostModel>();
            getAllPosts();
        }

        private async void getAllPosts()
        {            
            posts.Clear();
            IEnumerable<PostModel> list = await DataService.getPosts();
            list.ToList().ForEach(async fn => {
                string[] butcheredDate = fn.time.Split(" ");
                fn.time = $"{butcheredDate[3]}. {butcheredDate[1]}. {butcheredDate[2]}.";               
                string userId = await SecureStorage.GetAsync("userId");
                if (userId.IsNullOrEmpty())
                {
                    userId = "0";
                }
                fn.gridColumn = 0;
                fn.messageColor = Colors.Black;
                fn.textColor = Colors.White;
                if (fn.ownerId == Int32.Parse(userId))
                {
                    fn.gridColumn = 1;
                    fn.messageColor = Colors.Blue;
                    fn.textColor = Colors.White;
                }
                if (fn.imgUrl.IsNullOrEmpty())
                {
                    fn.imgUrl = "onebyone.png";
                }
                
                posts.Add(fn);
                });         
        }
    }
}
