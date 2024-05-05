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
    public class userPageViewModel : BindableObject
    {
        public ICommand userUpdateCommand { get; set; }
        public ObservableCollection<UserModel> users { get; set; }
        public UserModel user { get; set; }
        public string newUserName { get; set; }
        public string newUserEmail { get; set; }        
        public string updateError { get; set; }
        private bool _editVisibility;

        public bool editVisibility
        {
            get => _editVisibility;
            set
            {
                if (_editVisibility != value)
                {
                    _editVisibility = value;
                    OnPropertyChanged(nameof(editVisibility));
                }
            }
        }
        private bool _editVisibilityInvers;

        public bool editVisibilityInvers
        {
            get => _editVisibilityInvers;
            set
            {
                if (_editVisibilityInvers != value)
                {
                    _editVisibilityInvers = value;
                    OnPropertyChanged(nameof(editVisibilityInvers));
                }
            }
        }
        public bool _userChangeVisibility { get; set; }
        public bool userChangeVisibility
        {
            get => _userChangeVisibility;
            set
            {
                if (_userChangeVisibility != value)
                {
                    _userChangeVisibility = value;
                    OnPropertyChanged(nameof(userChangeVisibility));
                }
            }
        }
        public ICommand userEditCommand { get; set; }
        public userPageViewModel()
        {
            users = new ObservableCollection<UserModel>();            
            a();            
            editVisibility = false;
            editVisibilityInvers = !editVisibility;            
            userEditCommand = new Command(async () =>
            {
                editVisibility = true;
                editVisibilityInvers = !editVisibility;
            });

            userUpdateCommand = new Command(async () =>
            {
                int UserId = Int32.Parse(await SecureStorage.GetAsync("userId"));
                user = await DataService.getProfileById(UserId);
                
                if (string.IsNullOrEmpty(newUserEmail))
                {
                    newUserEmail = user.email;
                }
                if (!string.IsNullOrEmpty(newUserName))
                {
                    user.name = newUserName;
                }

                if (!string.IsNullOrEmpty(newUserEmail) && newUserEmail.Contains('@'))
                {
                    user.email = newUserEmail;

                }
                else if (!newUserEmail.Contains('@'))
                {
                    updateError = "Please provide a real email address";
                }               
                string UserPic = await SecureStorage.GetAsync("userImage");
                string userFollowed = await SecureStorage.GetAsync("userFollowed");
                int userRole = Int32.Parse(await SecureStorage.GetAsync("userRole"));
                await DataService.profileUpdate(user.id, user.name, user.email, user.pPic, userRole, userFollowed);
                await Shell.Current.GoToAsync(nameof(ProfilePage));
            });
        }       

        private async void a()
        {
            await GetuserData();
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
        public ICommand openUrlCommand =>
        new Command<string>(async (url) => await Launcher.OpenAsync(url));        

    }

}
