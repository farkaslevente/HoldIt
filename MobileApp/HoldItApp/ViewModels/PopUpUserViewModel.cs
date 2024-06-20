using CommunityToolkit.Maui.Views;
using HoldItApp.Models;
using HoldItApp.Services;
using HoldItApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HoldItApp.ViewModels
{
    public class PopUpUserViewModel : BindableObject
    {

        public int uId { get; set; }
        public string uName { get; set; }
        public string uPic { get; set; }
        public ObservableCollection<string> userImages { get; set; }
        public ObservableCollection<string> privates { get; set; }
        public ObservableCollection<PostModel> posts { get; set; }
        public PostModel selectedPost { get; set; }
        public UserModel user { get; set; }
        public ICommand postDetailCommand { get; set; }
        public string selectedimgUrl { get; set; }  
        public PopUpUserViewModel(UserModel pm)
        {
            user = pm;
            posts = new ObservableCollection<PostModel>();
            userImages = new ObservableCollection<string>();
            privates = new ObservableCollection<string>();
            uId = pm.id;
            uName = pm.name;
            uPic = pm.pPic;
            getEssentials();

            postDetailCommand = new Command(() =>
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

        private async void getEssentials()
        {
            await getAllPosts();
            
        }

        private async Task getAllPosts()
        {
            posts.Clear();
            IEnumerable<PostModel> list = await DataService.getPosts();
            string IncludePrivate = await SecureStorage.GetAsync("Includeprivate");
            foreach (var fn in list)
            {
                if(IncludePrivate == "yes")
                {
                    if (fn.ownerId == user.id)
                    {
                        fn.ownerPic = user.pPic;
                        fn.ownerName = user.name;
                        posts.Add(fn);                        
                    }
                }
                else
                {
                    if (fn.ownerId == user.id )
                    {
                        if(fn.isPrivate == 0)
                        {
                            fn.ownerPic = user.pPic;
                            fn.ownerName = user.name;
                            posts.Add(fn);
                        }
                        else
                        {
                            privates.Add(fn.id.ToString());
                        }
                        
                    }                    
                }
             
            }
            await getAllUploads();
        }

        public async Task getAllUploads()
        {
            userImages.Clear();            
            IEnumerable<string> list = await DataService.getUploads();
            list.ToList().ForEach(fn => {                            
                if (!privates.Contains(fn.Split('_')[1]))
                {
                    userImages.Add($"{DataService.url}/uploads/{fn}");
                }

            });
            
        }
    }
}
