using HoldItApp.Models;
using HoldItApp.Services;
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
            getAllPosts();
        }

        private async void getAllPosts()
        {            
            posts.Clear();
            IEnumerable<PostModel> list = await DataService.getPosts();
            list.ToList().ForEach(async fn => {
                int userId = Int32.Parse(await SecureStorage.GetAsync("userId"));
                fn.gridColumn = 0;
                if (fn.ownerId == userId)
                {
                    fn.gridColumn = 1;
                }
                
                posts.Add(fn);
                });         
        }
    }
}
