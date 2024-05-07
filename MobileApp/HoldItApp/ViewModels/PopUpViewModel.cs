using HoldItApp.Models;
using HoldItApp.Services;
using HoldItApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HoldItApp.ViewModels
{
    public class PopUpViewModel: BindableObject
    {
        public string comment { get; set; }
        public ICommand uploadCommand { get; set; }
        public ICommand imageUploadCommand { get; set; }
        public PopUpViewModel()
        {
            imageUploadCommand = new Command(async () =>
            {

            });
            uploadCommand = new Command(async () =>
            {                

            });           
        }
        

        
    }
}
