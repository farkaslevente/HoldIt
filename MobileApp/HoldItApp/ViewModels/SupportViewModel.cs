using HoldItApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HoldItApp.ViewModels
{
    public class SupportViewModel: BindableObject
    {
        public string questionTitle { get; set; }
        public string questionContent { get; set; }
        public ICommand subscribeCommand { get; set; }
        public ICommand supportCommand { get; set; }

        public SupportViewModel()
        {
            subscribeCommand = new Command(async () =>
            {
                await DataService.postSub();
                await Shell.Current.DisplayAlert("", "Thank you for your interest in our project", "Go back");                
                OnPropertyChanged(questionTitle);
            });

            supportCommand = new Command(async () =>
            {
                await DataService.postSupport(questionTitle, questionContent);
                await Shell.Current.DisplayAlert("Thank you for your response!", "As soon as we process your response we will reach out to you!", "Go back");
                questionTitle = "";
                questionContent = "";
                OnPropertyChanged(questionTitle);
            });

        }
    }
}
