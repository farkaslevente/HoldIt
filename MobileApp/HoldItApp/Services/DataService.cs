using HoldItApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MobilApp_Szakdolgozat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using HoldItApp.ViewModels;
using System.Net.Http.Headers;
using FFImageLoading.Args;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using Camera.MAUI;
using Microsoft.Maui.Controls;



namespace HoldItApp.Services
{
    public class DataService
    {
        public static ObservableCollection<PostModel> posts { get; set; } = new ObservableCollection<PostModel>();             
        
        public static string url = "";            //Your IP address and PORT number must be here like the example one row above

        // _    _                                     _               
        //| |  | |                                   (_)              
        //| |  | |___ ___ _ __   ___ ___ _ ____   ___ ___ ___ ___ 
        //| |  | / __|/ _ \ '__| / __|/ _ \ '__\ \ / / |/ __/ _ \/ __|
        //| |__| \__ \  __/ |    \__ \  __/ |   \ V /| | (_|  __/\__ \
        // \____/|___/\___|_|    |___/\___|_|    \_/ |_|\___\___||___/
        // 


        public static async Task<RegisterModel> register(RegisterModel user)
        {
            RegisterModel errorRegister = new RegisterModel();

            string jsonData = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(url + "/register", content);

            // Debug.WriteLine(response.StatusCode);
            string result = await response.Content.ReadAsStringAsync();

            if ((int)response.StatusCode == 400)
            {
                //hiba
                dynamic errorObj = JsonConvert.DeserializeObject<dynamic>(result);
                foreach (JProperty variable in errorObj)
                {
                    if (variable.Name == "name")
                    {
                        errorRegister.name = variable.Value[0].ToString();
                    }
                    if (variable.Name == "email")
                    {
                        errorRegister.email = variable.Value[0].ToString();
                    }
                    if (variable.Name == "password")
                    {
                        errorRegister.pwd = variable.Value[0].ToString();
                    }
                }
            }
            return errorRegister;
        }

        public static async Task<string> login(string email, string password)
        {
            string jsonData = JsonConvert.SerializeObject(new { email = email, pwd = password });
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(url + "/login", content);
            string result = await response.Content.ReadAsStringAsync();

            if ((int)response.StatusCode == 401)
            {
                return "Invalid login!";
            }
            else
            {

                string[] fullJWT = result.Split(':');
                var fullResult = fullJWT[1].Trim('"');
                string[] trimmedJWT = fullResult.Split('"');
                var trimmedResult = trimmedJWT[0];
                string[] payload = trimmedResult.Split('.');
                var finalPayload = payload[1];

                var Result = JWTTokenService.DecodeJwt(JWTTokenService.ConvertJwtStringToJwtSecurityToken(trimmedResult));
                string[] ResultwithoutMustacheOne = Result[0].ToString().Split("{");
                string[] ResultwithoutMustachetwo = ResultwithoutMustacheOne[1].Split("}");
                string[] finalResult = ResultwithoutMustachetwo[0].Split(",");

                string userImage = null;

                foreach (string item in finalResult)
                {
                    string[] keyValue = item.Split(':');
                    if (keyValue.Length >= 2)
                    {
                        string key = keyValue[0].Trim('"');
                        string value = string.Join(":", keyValue.Skip(1)).Trim('"');

                        if (key == "pPic")
                        {
                            userImage = value;
                            break;
                        }
                    }
                }
                if (userImage != null)
                {
                    await SecureStorage.SetAsync("userImage", userImage);
                }
                await SecureStorage.SetAsync("userId", finalResult[0].Split(':')[1].Trim('"'));
                await SecureStorage.SetAsync("userName", finalResult[1].Split(':')[1].Trim('"'));
                await SecureStorage.SetAsync("userEmail", finalResult[2].Split(':')[1].Trim('"'));
                await SecureStorage.SetAsync("userFollowed", finalResult[4].Split(':')[1].Trim('"'));
                await SecureStorage.SetAsync("userRole", finalResult[5].Split(':')[1].Trim('"'));
                await SecureStorage.SetAsync("userToken", trimmedResult);

                return null;

            }
        }
        public static async Task<IEnumerable<UserModel>> getProfiles()
        {
            using (var client = new HttpClient())
            {
                string token = await SecureStorage.GetAsync("userToken");
                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(url);
                var uri = $"/users";
                var result = await client.GetStringAsync(uri);

                return JsonConvert.DeserializeObject<List<UserModel>>(result);
            }
        }
        public static async Task<UserModel> getProfileById(int userId)
        {
            using (var client = new HttpClient())
            {
                string token = await SecureStorage.GetAsync("userToken");
                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(url);
                var uri = $"/users/{userId}";
                var result = await client.GetStringAsync(uri);

                return JsonConvert.DeserializeObject<UserModel>(result);
            }
        }

        public static async Task<string> profilePictureUpdate(int id, string picUrl)
        {
            string jsonData = JsonConvert.SerializeObject(new
            {
                id = id,
                pPic = picUrl
            });
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            string token = await SecureStorage.GetAsync("userToken");
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.PostAsync(url + "/users/changepicture", content);
            string result = await response.Content.ReadAsStringAsync();

            if ((int)response.StatusCode == 401)
            {
                return "error";
            }
            else
            {
                string success = "success";
                await SecureStorage.SetAsync("success", success);
                return null;
            }
        }

        public static async Task<string> profileUpdate(int id, string userName, string userEmail, string userPic, int userRole, string userFollowed)
        {
            string jsonData = JsonConvert.SerializeObject(new
            {
                id = id,
                name = userName,
                email = userEmail,
                pPic = userPic,
                followed = userFollowed,
                role = userRole,
            });
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            string token = await SecureStorage.GetAsync("userToken");
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.PutAsync(url + "/users/put", content);
            string result = await response.Content.ReadAsStringAsync();

            if ((int)response.StatusCode == 401)
            {
                return "error";
            }
            else
            {
                string success = "success";
                await SecureStorage.SetAsync("success", success);
                await SecureStorage.SetAsync("userId", id.ToString());
                await SecureStorage.SetAsync("userName", userName);
                await SecureStorage.SetAsync("userEmail", userEmail);
                await SecureStorage.SetAsync("userRole", userRole.ToString());
                await SecureStorage.SetAsync("userFollowed", userFollowed);
                return null;
            }
        }

        public static async Task<string> adProfileFollow(int userId)
        {
            string jsonData = JsonConvert.SerializeObject(new
            {
                userId = userId,
            });
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            string token = await SecureStorage.GetAsync("userToken");
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.PostAsync(url + "/users/addfollow", content);
            string result = await response.Content.ReadAsStringAsync();

            if ((int)response.StatusCode == 401)
            {
                return "error";
            }
            else
            {
                return null;
            }
        }

        public static async Task<string> removeProfileFollow(int userId)
        {
            string jsonData = JsonConvert.SerializeObject(new
            {
                userId = userId,
            });
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            string token = await SecureStorage.GetAsync("userToken");
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.PostAsync(url + "/users/removefollow", content);
            string result = await response.Content.ReadAsStringAsync();

            if ((int)response.StatusCode == 401)
            {
                return "error";
            }
            else
            {
                return null;
            }
        }

        public static async Task<string[]> getFollowed()
        {
            int uId = Int32.Parse(await SecureStorage.GetAsync("userId"));
            UserModel localU = await getProfileById(uId);
            string storedFavoritesList = localU.followed;
            string[] storedFavorites = storedFavoritesList.Split(" + ");
            for (int i = 0; i < storedFavorites.Length; i++)
            {
                if (storedFavorites[i].Contains(" +"))
                {
                    storedFavorites[i] = storedFavorites[i].Replace(" +", "");
                }
            }
            return storedFavorites;
        }

        public static async Task resetPwd(string email)
        {
            await SecureStorage.SetAsync("resetMail", email);
            string jsonData = JsonConvert.SerializeObject(new
            {
                email = email
            });
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(url + "/users/resetpassword", content);
            string result = await response.Content.ReadAsStringAsync();
        }

        public static async Task resetPwdCode(string code)
        {
            string resetMail = await SecureStorage.GetAsync("resetMail");
            string jsonData = JsonConvert.SerializeObject(new
            {
                token = code.ToUpper(),
                email = resetMail
            });
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(url + "/users/authorizereset", content);
            string result = await response.Content.ReadAsStringAsync();
            await SecureStorage.SetAsync("resetToken", result);
        }
        public static async Task resetPwdFinal(string newPassword)
        {
            string resetToken = await SecureStorage.GetAsync("resetToken");
            string[] fullJWT = resetToken.Split(':');
            var fullResult = fullJWT[1].Trim('"');
            string[] trimmedJWT = fullResult.Split('"');
            var trimmedResult = trimmedJWT[0];
            string jsonData = JsonConvert.SerializeObject(new
            {
                password = newPassword
            });
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", trimmedResult);
            HttpResponseMessage response = await client.PostAsync(url + "/users/newpassword", content);
            string result = await response.Content.ReadAsStringAsync();
        }
        public static async Task newPassword(string newPwd)
        {
            string jsonData = JsonConvert.SerializeObject(new
            {
                newpassword = newPwd
            });
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            string token = await SecureStorage.GetAsync("userToken");
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.PostAsync(url + "/users/newpassword", content);
            string result = await response.Content.ReadAsStringAsync();

        }

        public static async Task postSupport(string title, string question)
        {
            string jsonData = JsonConvert.SerializeObject(new
            {
                title = title,
                question = question
            });
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                string token = await SecureStorage.GetAsync("userToken");
                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(url);
                var uri = "/users/support";
                HttpResponseMessage response = await client.PostAsync(uri, content);
                string result = await response.Content.ReadAsStringAsync();
                return;
            }
        }
        public static async Task postSub()
        {
            using (var client = new HttpClient())
            {
                string token = await SecureStorage.GetAsync("userToken");
                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(url);
                var uri = "/users/subscribe";
                HttpResponseMessage response = await client.GetAsync(uri);
                string result = await response.Content.ReadAsStringAsync();
                return;
            }
        }

        public static async Task postNewPwd(string newPwd)
        {
            string jsonData = JsonConvert.SerializeObject(new
            {
                password = newPwd,
            });
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                string token = await SecureStorage.GetAsync("userToken");
                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(url);
                var uri = "/users/newpassword";
                HttpResponseMessage response = await client.PostAsync(uri, content);
                string result = await response.Content.ReadAsStringAsync();
                return;
            }
        }

 // _____          _                         _               
 //|  __ \        | |                       (_)              
 //| |__) |__  ___| |_   ___  ___ _ ____ ___    ___ ___  ___ 
 //|  ___/ _ \/ __| __| / __|/ _ \ '__\ \ / / |/ __/ _ \/ __|
 //| |  | (_) \__ \ |_  \__ \  __/ |   \ V /| | (_|  __/\__ \
 //|_|   \___/|___/\__| |___/\___|_|    \_/ |_|\___\___||___/
        public static async Task<IEnumerable<PictureCatalogModel>> getAllPictures()
        {
            using (var client = new HttpClient())
            {
                string token = await SecureStorage.GetAsync("userToken");
                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(url);
                var uri = "/pictures";
                var result = await client.GetStringAsync(uri);

                return JsonConvert.DeserializeObject<List<PictureCatalogModel>>(result);
            }
        }

        public static async Task<IEnumerable<PostModel>> getPosts()
        {
            using (var client = new HttpClient())
            {
                string token = await SecureStorage.GetAsync("userToken");
                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(url);
                var uri = "/uploads";
                var result = await client.GetStringAsync(uri);

                return JsonConvert.DeserializeObject<List<PostModel>>(result);
            }
        }

        public static async Task deletePost(int postId)
        {
            using (var client = new HttpClient())
            {
                string token = await SecureStorage.GetAsync("userToken");
                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(url);
                var uri = $"/uploads/{postId}";
                HttpResponseMessage response = await client.DeleteAsync(uri);

                return;
            }
        }        

        public static async Task<IEnumerable<string>> getUploads()
        {
            using (var client = new HttpClient())
            {
                string token = await SecureStorage.GetAsync("userToken");
                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(url);
                var uri = "/uploads/upload";
                var result = await client.GetStringAsync(uri);

                return JsonConvert.DeserializeObject<List<string>>(result);
            }
        }
           
        public static async Task<string> newPostUpload(string imgUrl, string comment, int ownerId, int isPrivate, int targetId)
        {
            string jsonData = JsonConvert.SerializeObject(new {
                imgUrl = imgUrl,
                comment = comment,
                ownerId = ownerId,
                isPrivate = isPrivate,
                targetId = targetId
            });           
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            string token = await SecureStorage.GetAsync("userToken");
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.PostAsync(url + "/uploads/post", content);
            string result = await response.Content.ReadAsStringAsync();

            if ((int)response.StatusCode == 401)
            {
                return "error";
            }
            else
            {
                await SecureStorage.SetAsync("uploaded", true.ToString());
                return null;
            }
        }

        //dolog van vele
        public static async Task<string> UpdatePostUpload(string comment, string imgUrl)
        {
            string jsonData = JsonConvert.SerializeObject(new
            {
                comment = comment,
                imgUrl = imgUrl
            });
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            string token = await SecureStorage.GetAsync("userToken");
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.PostAsync(url + "/ads/post", content);
            string result = await response.Content.ReadAsStringAsync();

            if ((int)response.StatusCode == 401)
            {
                return "error";
            }
            else
            {
                await SecureStorage.SetAsync("uploaded", true.ToString());
                return null;
            }
        }
        private static async Task getAllPosts()
        {
            posts.Clear();
            IEnumerable<PostModel> list = await getPosts();
            list.ToList().ForEach(p => posts.Add(p));
        }
        
        public static async Task<string> imageUpload(int userId, int imgId) 
        {
            
            string dummyJsonData = JsonConvert.SerializeObject(new
            {
                imgUrl = "",
                comment = "",
                ownerId = userId
            });
            StringContent dummyContent = new StringContent(dummyJsonData, Encoding.UTF8, "application/json");
            HttpClient dummyClient = new HttpClient();
            string dummyToken = await SecureStorage.GetAsync("userToken");
            dummyClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", dummyToken);
            HttpResponseMessage dummyResponse = await dummyClient.PostAsync(url + "/posts", dummyContent);
            string dummyResult = await dummyResponse.Content.ReadAsStringAsync();
            await getAllPosts();
            int postId = 0;
            for (int i = 0; i < posts.Count(); i++)
            {
                if (postId < posts[i].id)
                {
                    postId = posts[i].id;
                }
            }
            await deletePost(postId);
            postId++;            

            var uploadFile = await MediaPicker.PickPhotoAsync();

            if (uploadFile == null) return "error";
           
                var httpContent = new MultipartFormDataContent();         
            string[] fileType = uploadFile.FileName.Split('.');
            uploadFile.FileName = $"{userId}_{postId}_{imgId}.{fileType[1]}";
            httpContent.Add(new StreamContent(await uploadFile.OpenReadAsync()), "file", uploadFile.FileName);
            
            var httpClient = new HttpClient();
            string token = await SecureStorage.GetAsync("userToken");
            httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.PostAsync($"{url}/uploads/upload", httpContent);
            var response = await result.Content.ReadAsStringAsync();
            //await Shell.Current.DisplayAlert("Response from server", response, "K");
            await SecureStorage.SetAsync("imgId", imgId.ToString());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (Stream stream = await uploadFile.OpenReadAsync())
                {
                    await stream.CopyToAsync(memoryStream);
                }
                byte[] imageData = memoryStream.ToArray();
                PopUpViewModel.imageData = imageData;
            }
            return uploadFile.FileName;
        }

        public static async Task<string> CaptureAndUploadImageAsync(int userId, int imgId, Stream cameraView)
        {            
            var photoResult = cameraView;
            if (photoResult == null) return "error";         
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await photoResult.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                string dummyJsonData = JsonConvert.SerializeObject(new
                {
                    imgUrl = "",
                    comment = "",
                    ownerId = userId
                });
                StringContent dummyContent = new StringContent(dummyJsonData, Encoding.UTF8, "application/json");
                HttpClient dummyClient = new HttpClient();
                string dummyToken = await SecureStorage.GetAsync("userToken");
                dummyClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", dummyToken);
                HttpResponseMessage dummyResponse = await dummyClient.PostAsync(url + "/uploads/post", dummyContent);
                string dummyResult = await dummyResponse.Content.ReadAsStringAsync();
                await getAllPosts();
                int postId = 0;
                for (int i = 0; i < posts.Count(); i++)
                {
                    if (postId < posts[i].id)
                    {
                        postId = posts[i].id;
                    }
                }
                await deletePost(postId);
                postId++;
                var httpContent = new MultipartFormDataContent();
                string fileName = $"{userId}_{postId}_{imgId}.jpg";
                httpContent.Add(new StreamContent(memoryStream), "file", fileName);                
                var httpClient = new HttpClient();
                string token = await SecureStorage.GetAsync("userToken");
                httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
                var result = await httpClient.PostAsync($"{url}/uploads/upload", httpContent);
                var response = await result.Content.ReadAsStringAsync();                
                await SecureStorage.SetAsync("imgId", imgId.ToString());
                return fileName;
            }
        }
        public class TokenResponse
        {
            public string Token { get; set; }
        }

        

    }
}
