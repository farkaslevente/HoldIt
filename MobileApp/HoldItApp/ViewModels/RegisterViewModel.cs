using HoldItApp.Models;
using HoldItApp.Services;
using HoldItApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HoldItApp.ViewModels
{
    public class RegisterViewModel : BindableObject
    {      
        public RegisterModel regModel { get; set; }
        public string regName { get; set; }
        public string regEmail { get; set; }
        public string resetEmail { get; set; }
        public string resetCode { get; set; }
        public string regPassword { get; set; }
        public string regConfirmPwd { get; set; }                
        public bool resetVis { get; set; }
        public string resetPwd { get; set; }
        public string error { get; set; }

        private RegisterModel _errorMessage;

        public RegisterModel errorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; OnPropertyChanged(); }
        }


        public ICommand registerCommand { get; set; }
        public ICommand resetPwdCommand { get; set; }
        public ICommand resetPwdCodeCommand { get; set; }
        public ICommand resetPwdFinalCommand { get; set; }

        public RegisterViewModel()
        {            
            resetVis = false;            
            resetPwdCommand = new Command(async () =>
            {
                await DataService.resetPwd(resetEmail);
                await Shell.Current.DisplayAlert("Ellenőrizze postaládáját", "A jelszó visszaállításhoz szükséges információkat elküldtük a megadott emailcímre", "Rendben");
                await Shell.Current.GoToAsync(nameof(ResetPwdCodePage));
            });
            resetPwdCodeCommand = new Command(async () =>
            {
                await DataService.resetPwdCode(resetCode);
                await Shell.Current.GoToAsync(nameof(ResetPwdFinalStagePage));

            });
            resetPwdFinalCommand = new Command(async () =>
            {
                await DataService.resetPwdFinal(resetPwd);
                await Shell.Current.GoToAsync(nameof(LoginPage));
            });

            registerCommand = new Command(async () => {
                if (regName != null)
                {
                    if (regEmail != null && regEmail.Contains('@'))
                    {
                        if (regPassword == regConfirmPwd)
                        {
                            error = "";

                            regModel = new RegisterModel
                            {
                                name = regName,
                                email = regEmail,
                                pwd = regPassword
                            };
                            errorMessage = await DataService.register(regModel);
                            if (errorMessage.email == null)
                            {
                                await Shell.Current.GoToAsync(nameof(MainPage));
                            }
                        }
                        else
                        {
                            error = "Your provided passwords are not matching!";
                        }                                     
                    }
                    else
                    {
                        error = "Please provide a valid email address!";
                    }
                }
                else
                {
                    error = "Please provide a public username!";
                }
            });
        }       
    }
}
