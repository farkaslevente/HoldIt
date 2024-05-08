using HoldItApp.Models;
using HoldItApp.Services;
using HoldItApp.Views;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;

namespace HoldItApp.ViewModels
{
    public class PopUpViewModel: BindableObject
    {
        public int imageId { get; set; }
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

        public static event EventHandler<byte[]> ImageDataUpdated;

        private static byte[] _imageData;
        public static byte[] imageData
        {
            get => _imageData;
            set
            {
                _imageData = value;
                ImageDataUpdated?.Invoke(null, _imageData); // Notify subscribers that image data has been updated
            }
        }

        private string _UploadedImgUrl;
        public string UploadedImgUrl
        {
            get => _UploadedImgUrl;
            set
            {
                _UploadedImgUrl = value;
                OnPropertyChanged();
            }
        }

        public ICommand uploadCommand { get; set; }
        public ICommand imageUploadCommand { get; set; }
        public PopUpViewModel()
        {
            imageUploadCommand = new Command(async () =>
            {
                int userId = int.Parse(await SecureStorage.GetAsync("userId"));

                List<string> imageList = new List<string>();
                string response = await DataService.imageUpload(userId, imageId);                
                if (response == "error")
                {
                    await Shell.Current.DisplayAlert("Error", "Please choose a picture", "Go back");                    
                }
                else
                {
                    imageId = int.Parse(await SecureStorage.GetAsync("imgId")) + 1;
                    UploadedImgUrl = $"{DataService.url}/uploads/{response}";                    
                }
            });
            uploadCommand = new Command(async () =>
            {
                int userId = Int32.Parse(await SecureStorage.GetAsync("userId"));
                if (UploadedImgUrl.IsNullOrEmpty())
                {
                    UploadedImgUrl = "onebyone.png";
                }
                await DataService.newPostUpload(
                                                   UploadedImgUrl,
                                                   comment,
                                                   userId
                                                   );                
                if (await SecureStorage.GetAsync("uploaded") == true.ToString())
                {
                    await Shell.Current.GoToAsync(nameof(TimelinePage));
                }
            });           
        }
        

        
    }
}
