using Estudiar.Models;
using Estudiar.ViewModels;
using Estudiar.XInterface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Extensions;
using System.Collections.ObjectModel;

namespace Estudiar.RestClients
{
    public class NameEdit
    {
        public string name { get; set; }
    }
    public class RestClient<T>
    {
        public const string company = "http://8.9.3.229";
        private const string Foundation = "http://8.9.3.229:5000";
        //private const string WebServiceUrl = @"http://misloja.ml:5000/merch/pj_enterprises";
        //private const string WebServiceUrl = @"http://makeup-api.herokuapp.com/api/v1/products.json?brand=maybelline";
        public RestClient()
        {
            //GeneralFoundation = Foundation;
            string x = "";
        }
        
        public async Task<T> GetDashBoardAsync(string token)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);

                var response = await httpClient.GetAsync(Foundation + "/d");
                var json = await response.Content.ReadAsStringAsync();

                try
                {
                    if (JsonConvert.DeserializeObject<SignUpResponse>(json).msg.ToLower() == "jwt expired")
                    {
                        LoginResponse newLogin = await GetNewToken();
                        MainVM mainVM = new MainVM { LoginResponse = newLogin };
                        return await GetDashBoardAsync(newLogin.token);
                    }
                }
                catch (Exception)
                {
                }
                var taskModels = JsonConvert.DeserializeObject<T>(json);
                return taskModels;
            }
            catch (Exception)
            {
                DisplayToast();
                return default(T);
            }
            
        }
        public async Task<T> GetCourses(string token)
        {
            try
            {
                var httpClient = new HttpClient();
                


                var response = await httpClient.GetAsync("https://script.google.com/macros/s/AKfycbzKWrs1f1OwZ3xGzzqAIVCZgYsxY8TIYcl9QYdp_4NEqLOxLgfzIJHfOFYsAuN_M8y7/exec");
                var json = await response.Content.ReadAsStringAsync();
                
                var taskModels = JsonConvert.DeserializeObject<T>(json);
                return taskModels;
            }
            catch (Exception)
            {
                DisplayToast();
                return default(T);
            }
            
        }
        public async Task<Stream> GetMaterialStreamAsync(string token, string code, string name)
        {
            LoadingVM vM = new LoadingVM();
            var httpClient = new HttpClient();

            try
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                Shell.Current.Navigation.ShowPopup(new Views.Loading(vM) { IsLightDismissEnabled=false});
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);

                var buff = await httpClient.GetByteArrayAsync($"{Foundation}/course/{code}?name={name}");
                


                Stream stream = new MemoryStream(buff);
                vM.Dismiss = true;

                //var taskModels = JsonConvert.DeserializeObject<List<T>>(json);
                return stream;

            }
            catch (Exception)
            {
                DisplayToast();
                vM.Dismiss = true;
                try
                {

                    var response = await httpClient.GetAsync(($"{Foundation}/course/{code}?name={name}"));
                    var json = await response.Content.ReadAsStringAsync();
                    if (JsonConvert.DeserializeObject<SignUpResponse>(json).msg.ToLower() == "jwt expired")
                    {
                        LoginResponse newLogin = await GetNewToken();
                        MainVM mainVM = new MainVM { LoginResponse = newLogin };
                        return await GetMaterialStreamAsync(newLogin.token, code, name);
                    }
                }
                catch (Exception)
                {
                }
                return null;
            }
            
        }
        

        private async Task<LoginResponse> GetNewToken()
        {
            try
            {
                //DependencyService.Get<IToast>().LongToast("Session Expired, Re");

                string postUrl = "/auth/login";
                var httpClient = new HttpClient();
                LoginModel t = AppState<LoginModel>.DeserializeAndRestore("LoginModel");
                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Add(new StringContent(t.email.ToLower().Trim()), "email");
                content.Add(new StringContent(t.password), "password");
                var response = await httpClient.PostAsync(Foundation + postUrl, content);
                var result = await response.Content.ReadAsStringAsync();

                
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    String x = JsonConvert.DeserializeObject<SignUpResponse>(result).msg;
                    if (x == null)
                    {
                        return JsonConvert.DeserializeObject<LoginResponse>(result);
                    }
                    else
                    {
                        return new LoginResponse() { token = x };
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private  void DisplayToast()
        {
            DependencyService.Get<IToast>().LongToast("Please ensure a good internet connection");
        }
        public async Task<payUrl> PostSubscribeAsync( string Token)
        {
            LoadingVM vM = new LoadingVM();
            try
            {
                Shell.Current.Navigation.ShowPopup(new Views.Loading(vM) { IsLightDismissEnabled = false });
                string postUrl = "/pay/subscribe";
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);

                //HttpContent content = new HttpContent(,);
                //Created an empty content
                MultipartFormDataContent content = new MultipartFormDataContent();

                var responseMessage = await httpClient.PostAsync(Foundation + postUrl,null);
                var result = await responseMessage.Content.ReadAsStringAsync();

                try
                {
                    if (JsonConvert.DeserializeObject<SignUpResponse>(result).msg.ToLower() == "jwt expired")
                    {
                        LoginResponse newLogin = await GetNewToken();
                        MainVM mainVM = new MainVM { LoginResponse = newLogin };
                        return await PostSubscribeAsync(newLogin.token);
                    }
                }
                catch (Exception)
                {
                }

                vM.Dismiss = true;
                return JsonConvert.DeserializeObject<payUrl>(result);
            }
            catch (Exception)
            {
                DisplayToast();
                vM.Dismiss = true;
                return null;
            }

        }
        public async Task<bool> PostUnSubscribeAsync(string Token)
        {
            LoadingVM vM = new LoadingVM();
            try
            {
                Shell.Current.Navigation.ShowPopup(new Views.Loading(vM) { IsLightDismissEnabled = false });
                string postUrl = "/delete/subscription";
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);

                //Created an empty content
                MultipartFormDataContent content = new MultipartFormDataContent();

                var responseMessage = await httpClient.DeleteAsync(Foundation + postUrl);
                var result = await responseMessage.Content.ReadAsStringAsync();

                try
                {
                    if (JsonConvert.DeserializeObject<SignUpResponse>(result).msg.ToLower() == "jwt expired")
                    {
                        LoginResponse newLogin = await GetNewToken();
                        MainVM mainVM = new MainVM { LoginResponse = newLogin };
                        return await PostUnSubscribeAsync(newLogin.token);
                    }
                }
                catch (Exception)
                {
                }

                vM.Dismiss = true;
                if (responseMessage.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                DisplayToast();
                vM.Dismiss = true;
                return false;
            }

        }
        public async Task<paymentVerify> PostVerifySubscriptionAsync(string Token)
        {
            LoadingVM vM = new LoadingVM();
            try
            {
                Shell.Current.Navigation.ShowPopup(new Views.Loading(vM) { IsLightDismissEnabled = false });

                string postUrl = "/pay/verify";
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);


                var responseMessage = await httpClient.GetAsync(Foundation + postUrl);
                var result = await responseMessage.Content.ReadAsStringAsync();

                try
                {
                    if (JsonConvert.DeserializeObject<SignUpResponse>(result).msg.ToLower() == "jwt expired")
                    {
                        LoginResponse newLogin = await GetNewToken();
                        MainVM mainVM = new MainVM { LoginResponse = newLogin };
                        return await PostVerifySubscriptionAsync(newLogin.token);
                    }
                }
                catch (Exception)
                {
                }

                vM.Dismiss = true;
                return JsonConvert.DeserializeObject<paymentVerify>(result);
            }
            catch (Exception)
            {
                DisplayToast();
                vM.Dismiss = true;
                return null;
            }

        }
        public async Task<SignUpResponse> PostUploadAsync(UploadModel t,string Token)
        {
            LoadingVM vM = new LoadingVM();
            try
            {
                
                Shell.Current.Navigation.ShowPopup(new Views.Loading(vM) { IsLightDismissEnabled = false });
                string postUrl = "/upload";
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);

                if (t.faculty.ToLower() == "shared")
                {
                    t.dept = "shared";
                }

                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Add(new StringContent(t.year.ToString()), "year");
                content.Add(new StringContent(t.faculty.ToLower()), "faculty");
                content.Add(new StringContent(t.dept.ToLower()), "dept");
                content.Add(new StringContent(t.code.ToUpper()), "code");
                content.Add(new StringContent(t.title.Trim().ToLower()), "title");

                byte[] array;
                using (var memoryStream = new MemoryStream())
                {
                    await t.Material.Stream.CopyToAsync(memoryStream);
                    array = memoryStream.ToArray();
                }
                ByteArrayContent byteArrayContent = new ByteArrayContent(array);
                byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse($"application/pdf");
                content.Add(byteArrayContent, "material", t.Material.fileName.Trim().ToLower() + ".pdf");
                
                


                //content.Add(new StringContent(t.faculty), "description");
                var responseMessage = await httpClient.PostAsync(Foundation + postUrl, content);


                try
                {
                    var json = await responseMessage.Content.ReadAsStringAsync();
                    if (JsonConvert.DeserializeObject<SignUpResponse>(json).msg.ToLower() == "jwt expired")
                    {
                        LoginResponse newLogin = await GetNewToken();
                        MainVM mainVM = new MainVM { LoginResponse = newLogin };
                        return await PostUploadAsync(t,newLogin.token);
                    }
                }
                catch (Exception)
                {
                }


                vM.Dismiss = true;
                var result = await responseMessage.Content.ReadAsStringAsync();
                if (JsonConvert.DeserializeObject<SignUpResponse>(result).msg.ToLower() == "jwt expired")
                {
                    return new SignUpResponse { msg = "Error!!!\nPlease Log out and sign-in"};
                }
                return JsonConvert.DeserializeObject<SignUpResponse>(result);
            }
            catch (Exception)
            {
                DisplayToast();
                vM.Dismiss = true;


                return null;
            }
            
        }
        public async Task<SignUpResponse> EditProfile(string key, string value,string token)
        {
            try
            {
                string postUrl = "/d/edit?enabled=true";
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);

                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Add(new StringContent(value), key);
                var response = await httpClient.PutAsync(Foundation + postUrl, content);
                var result = await response.Content.ReadAsStringAsync();

                try
                {
                    if (JsonConvert.DeserializeObject<SignUpResponse>(result).msg.ToLower() == "jwt expired")
                    {
                        LoginResponse newLogin = await GetNewToken();
                        MainVM mainVM = new MainVM { LoginResponse = newLogin };
                        return await EditProfile(key, value,newLogin.token);
                    }
                }
                catch (Exception)
                {
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    SignUpResponse signUpResponse = JsonConvert.DeserializeObject<SignUpResponse>(await response.Content.ReadAsStringAsync());
                    DependencyService.Get<IToast>().LongToast($"Success: {key} changed");
                    return signUpResponse;//signUpResponse.msg;
                }
                

                //var json1 = await response.Content.ReadAsStringAsync();
                return new SignUpResponse { msg = "Error Updating Profile" };
            }
            catch (Exception)
            {
                DisplayToast();
                return null;
            }
            
        }
        public async Task<LoginResponse> PostLoginAsync(LoginModel t)
        {
            try
            {
                if (string.IsNullOrEmpty(t.email) || string.IsNullOrEmpty(t.password))
                {
                    return new LoginResponse() { token = "Please fill all the required fields" };
                }
                

                string postUrl = "/auth/login";
                var httpClient = new HttpClient();
                //httpClient.Timeout = TimeSpan.FromSeconds(4.0);

                //httpClient.Timeout = TimeSpan.FromSeconds(4.0);


                /*var KeyValues = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("email",(t as LoginModel).email),
                    new KeyValuePair<string, string>("password",(t as LoginModel).password),
                };

                var request = new HttpRequestMessage(HttpMethod.Post, Foundation + postUrl);

                request.Content = new FormUrlEncodedContent(KeyValues);

                var response = httpClient.SendAsync(request);*/
                //var json = await response.Content.ReadAsStringAsync();
                //return JsonConvert.DeserializeObject<LoginResponse>(result);
                //var result = await response.Content.ReadAsStringAsync();
                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Add(new StringContent(t.email.ToLower().Trim()), "email");
                content.Add(new StringContent(t.password), "password");
                var response = await httpClient.PostAsync(Foundation + postUrl, content);
                var result = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return new LoginResponse() { token = JsonConvert.DeserializeObject<SignUpResponse>(result).msg };
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
                {
                    return new LoginResponse() { token = "Timeout Error" };
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    return new LoginResponse() { token = JsonConvert.DeserializeObject<SignUpResponse>(result).msg };
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    String x = JsonConvert.DeserializeObject<SignUpResponse>(result).msg;
                    if (x == null)
                    {
                        return JsonConvert.DeserializeObject<LoginResponse>(result);
                    }
                    else
                    {
                        return new LoginResponse() { token = x };
                    }
                }
                return new LoginResponse() { token = "Server Error" }; //{ Merchant = new Merchant { Id = json } };
            }
            catch (Exception)
            {
                DisplayToast();
                return null;
            }   
            
        }
        

        public async Task<SignUpResponse> PostSignUpAsync(SignUpModel signUpModel)
        {
            try
            {

                if (string.IsNullOrEmpty(signUpModel.email) || string.IsNullOrEmpty(signUpModel.password))
                {
                    DependencyService.Get<IToast>().LongToast("Please fill all required fields");
                    return null;
                }
                string postUrl = "/auth/register";
                var httpClient = new HttpClient();

                if (string.IsNullOrEmpty(signUpModel.dept))
                {
                    signUpModel.dept = signUpModel.faculty;
                }

                var json = JsonConvert.SerializeObject(signUpModel);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Add(new StringContent(signUpModel.email.ToLower().Trim()), "email");
                content.Add(new StringContent(signUpModel.name.ToLower().Trim()), "name");
                content.Add(new StringContent(signUpModel.password), "password");
                content.Add(new StringContent(signUpModel.year.ToString()), "year");
                content.Add(new StringContent(signUpModel.dept.Trim().ToLower()), "dept");
                content.Add(new StringContent(signUpModel.faculty.Trim().ToLower()), "faculty");
                var response = await httpClient.PostAsync(Foundation + postUrl, content);
                //var result = await response.Content.ReadAsStringAsync();

                //return result.IsSuccessStatusCode;


                //var request = new HttpRequestMessage(HttpMethod.Post, Foundation + postUrl);
                ////request.
                //request.Content = new FormUrlEncodedContent(KeyValues);
                //var response = await httpClient.SendAsync(request);
                //var json = await response.Content.ReadAsStringAsync();

                
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    SignUpResponse signUpResponse = JsonConvert.DeserializeObject<SignUpResponse>(await response.Content.ReadAsStringAsync());
                    return signUpResponse;//signUpResponse.msg;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    SignUpResponse signUpResponse = JsonConvert.DeserializeObject<SignUpResponse>(await response.Content.ReadAsStringAsync());
                    return signUpResponse;
                }

                //var json1 = await response.Content.ReadAsStringAsync();
                return new SignUpResponse { msg = "Error Signing up" };
            }
            catch (Exception)
            {
                DisplayToast();
                return null;
            }
            
        }
        public async Task<bool> SendValidateTokenAsync(string token,string email)
        {
            string postUrl = "/auth/validate";
            var httpClient = new HttpClient();

            MultipartFormDataContent content = new MultipartFormDataContent();
            //content.Add(httpContent);
            content.Add(new StringContent(email), "email");
            content.Add(new StringContent(token), "token");
            var response = await httpClient.PostAsync(Foundation + postUrl, content);
            


            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                //string x = response.RequestMessage.ToString();
                ValidateModel validateModel = JsonConvert.DeserializeObject<ValidateModel>(result);
                return validateModel.valid;
            }
            else
            {
                return false;
            }
                //var json1 = await response.Content.ReadAsStringAsync();
        }
        public async Task<string> ResetPasswordAsync(string token, string email, string password)
        {
            try
            {
                string postUrl = "/auth/set-password";
                var httpClient = new HttpClient();

                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Add(new StringContent(email), "email");
                content.Add(new StringContent(token), "token");
                content.Add(new StringContent(password), "password");
                var response = await httpClient.PostAsync(Foundation + postUrl, content);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    SignUpResponse signUpResponse = JsonConvert.DeserializeObject<SignUpResponse>(result);
                    return signUpResponse.msg;//signUpResponse.msg;
                }
                else
                {
                    return "Error Changing password";
                }
            }
            catch (Exception)
            {
                DisplayToast();
                return null;
                  
            }
            
            //var json1 = await response.Content.ReadAsStringAsync();
        }
        public async Task<string> SendForgotPassAsync(string email)
        {
            try
            {
                string postUrl = "/auth/forgot-pass";
                var httpClient = new HttpClient();

                MultipartFormDataContent content = new MultipartFormDataContent();
                //content.Add(httpContent);
                content.Add(new StringContent(email), "email");
                var response = await httpClient.PostAsync(Foundation + postUrl, content);
                var result = await response.Content.ReadAsStringAsync();


                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //string x = response.RequestMessage.ToString();
                    try
                    {
                        SignUpResponse signUpResponse = JsonConvert.DeserializeObject<SignUpResponse>(result);
                        return signUpResponse.msg;
                    }
                    catch (Exception)
                    {
                        return "Success!!! check email for confirmation, else contact us";
                    }
                }
                else
                {
                    SignUpResponse signUpResponse = JsonConvert.DeserializeObject<SignUpResponse>(result);
                    return signUpResponse.msg;
                }
                //var json1 = await response.Content.ReadAsStringAsync();
                
            }
            catch (Exception)
            {
                return "An error occured. Check internet connection";
            }
            
        }
        /*public async Task<bool> PutAsync(int id, T t)
        {
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(t);
            HttpContent httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = await httpClient.PutAsync(WebServiceUrl + id, httpContent);
            return result.IsSuccessStatusCode;
        }*/
                //gets product and updates view
        
        
    }
}
