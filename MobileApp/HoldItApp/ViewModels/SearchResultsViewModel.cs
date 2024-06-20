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
    public class SearchResultsViewModel : BindableObject, IQueryAttributable
    {
        public ObservableCollection<UserModel> userResults { get; set; }
        public UserModel selectedUser { get; set; }
        public ObservableCollection<PostModel> postResults { get; set; }
        public PostModel selectedPost { get; set; }

        public ICommand userSelectionCommand { get; set; }
        public ICommand postSelectionCommand { get; set; }
        public SearchResultsViewModel()
        {
            postSelectionCommand = new Command( () =>
            {
                if (selectedPost == null) return;
                Shell.Current.ShowPopup(new PopUpDetailsPage(selectedPost));

                selectedPost = null;
                OnPropertyChanged(nameof(selectedPost));
            });

            userSelectionCommand = new Command(async () =>
            {
                if (selectedUser == null) return;
                await SecureStorage.SetAsync("Includeprivate", "no");
                Shell.Current.ShowPopup(new PopUpUserPage(selectedUser));

                selectedUser = null;
                OnPropertyChanged(nameof(selectedUser));
            });
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (!query.ContainsKey("emptyResult"))
            {
                if (query.ContainsKey("resultsUsers"))
                {
                    userResults = query["resultsUsers"] as ObservableCollection<UserModel>;
                }
                else
                {
                    userResults = new ObservableCollection<UserModel>();
                }


                if (query.ContainsKey("resultPosts"))
                {
                    postResults = query["resultPosts"] as ObservableCollection<PostModel>;
                }
                else
                {
                    postResults = new ObservableCollection<PostModel>();
                }
                overWritePost();
            }

            

        }


        public async void overWritePost()
        {           
                foreach (var post in postResults)
                {
                    string userId = await SecureStorage.GetAsync("userId");
                    if (userId.IsNullOrEmpty())
                    {
                        userId = "0";
                    }
                    UserModel owner = await DataService.getProfileById(post.ownerId);
                    post.ownerPic = owner.pPic;
                    post.ownerPicPos = post.ownerId == Int32.Parse(userId) ? 2 : 0;
                    post.ownerName = owner.name;
                    post.ownerNamePos = post.ownerId == Int32.Parse(userId) ? "End" : "Start";
                    string[] butcheredDate = post.time.Split(" ");
                    post.time = $"{butcheredDate[1]}. {butcheredDate[2]}. {butcheredDate[4].Split(':')[0]}:{butcheredDate[4].Split(':')[1]}";
                    post.gridColumn = 0;
                    post.messageColor = Colors.Black;
                    post.textColor = Colors.White;
                    if (post.ownerId == Int32.Parse(userId))
                    {
                        post.gridColumn = 1;
                        post.messageColor = Colors.Blue;
                        post.textColor = Colors.White;
                    }
                    if (post.imgUrl.IsNullOrEmpty())
                    {
                        post.imgUrl = "onebyone.png";
                    }
                }
                OnPropertyChanged(nameof(userResults));
                OnPropertyChanged(nameof(postResults));
            }                  
    }
}
