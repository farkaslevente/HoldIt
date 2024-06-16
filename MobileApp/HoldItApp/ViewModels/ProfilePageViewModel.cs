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
        private UserModel _user;
        public UserModel user
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> userImages { get; set; }
        public ObservableCollection<UserModel> resultUsers { get; set; }
        public ObservableCollection<PostModel> resultPosts { get; set; }
        public string selectedimgUrl { get; set; }
        public PostModel selectedPost { get; set; }
        private string _searchParam;
        public string searchParam
        {
            get => _searchParam;
            set
            {
                _searchParam = value;
                OnPropertyChanged();
            }
        }
        private string _comment;
        public string comment
        {
            get => _comment;
            set
            {
                _comment = value;              
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; set; }
        public ProfilePageViewModel()
        {
            resultUsers = new ObservableCollection<UserModel>();
            resultPosts = new ObservableCollection<PostModel>();
            users = new ObservableCollection<UserModel>();
            posts = new ObservableCollection<PostModel>();
            userImages = new ObservableCollection<string>();
            a();

            SearchCommand = new Command(async () =>
            {
                if (searchParam.IsNullOrEmpty())
                {
                    await Shell.Current.DisplayAlert("Please enter a search parameter", "Before you iniate our search engine please enter a search parameter", "Okay");
                }
                else
                {                
                    await getUsers();
                    await getAllPosts();
                    resultUsers.Clear();
                    resultPosts.Clear();
                    foreach (var user in users)
                    {
                        if (user.name.ToLower().Contains(searchParam.ToLower()))
                        {
                            resultUsers.Add(user);
                        }
                    }

                    foreach (var post in posts)
                    {
                        if (post.comment.ToLower().Contains(searchParam.ToLower()) && post.isPrivate == 0)
                        {
                            resultPosts.Add(post);
                        }
                    }
                    if (!resultUsers.IsNullOrEmpty())
                    {
                        if (!resultPosts.IsNullOrEmpty())
                        {
                            await Shell.Current.GoToAsync(nameof(SearchResultPage),
                                    new Dictionary<string, object> {
                                        { "resultsUsers",  resultUsers},
                                        { "resultPosts", resultPosts}
                                        });
                        }
                        else
                        {
                            await Shell.Current.GoToAsync(nameof(SearchResultPage),
                                    new Dictionary<string, object> {
                                        { "resultsUsers",  resultUsers},                        
                                        });
                        }
                    }
                    else if (!resultPosts.IsNullOrEmpty())
                    {
                        await Shell.Current.GoToAsync(nameof(SearchResultPage),
                                    new Dictionary<string, object> {
                                        { "resultPosts",  resultPosts},
                                        });
                    }
                    else
                    {
                        ObservableCollection<UserModel> emptyResult = new ObservableCollection<UserModel>();
                        await Shell.Current.GoToAsync(nameof(SearchResultPage),
                                    new Dictionary<string, object> {
                                        { "emptyResult",  emptyResult},
                                        });
                    }
                }
            });
            postDetailCommand = new Command( async() =>
            {
                await getAllPosts();
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
            user = await DataService.getProfileById(Convert.ToInt32(userId));
            IEnumerable<PostModel> list = await DataService.getPosts();
            foreach (var fn in list)
            {
                if (fn.ownerId == Int32.Parse(userId))
                {                    
                    fn.ownerPic = user.pPic;
                    fn.ownerName = user.name;
                    
                    posts.Add(fn);
                }               
            }
            
        }

        private async void a()
        {
             await getAllUploads();             
        }

        private async Task getUsers()
        {
            users.Clear();            
            IEnumerable<UserModel> list = await DataService.getProfiles();
            list.ToList().ForEach(fn => {
                users.Add(fn);

            });
        }

        
        public async Task getAllUploads()
        {
            userImages.Clear();
            int userId;
            string userIdString = await SecureStorage.GetAsync("userId");
            if (!userIdString.IsNullOrEmpty())
            {
                user = await DataService.getProfileById(Convert.ToInt32(userIdString));
            }            
            userId = string.IsNullOrEmpty(userIdString) ? 0 : Int32.Parse(userIdString);
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
