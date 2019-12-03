using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Diary.Common.Helpers;
using Diary.Framework.Exceptions;
using Diary.Repositories.Models;
using ModernMessageBoxLib;

namespace Diary.Repositories
{
    public enum Controller
    {
        Accounts,
        Specialization,
        Course,
        Teacher,
        Subject,
        Auditorium,
        Group,
        Schedule,
        News,
        AccountType,
        Material,
        Student,
        ContactInfo,
        Homework,
        LessonInfo,
        Licenses
    }

    public enum Method
    {
        GetAll,
        Get,
        Post, // for auth
        Create,
        Delete,
        Edit,
        Authenticate,
        GetForTeacher,
        GetLessonInfoByGroup,
        VerifyLicense,
        AssignLicense
    }

    public static class SimpleService
    {
        private const string baseAddress = "http://andybor-001-site1.btempurl.com/api/";

        private const string baseAddressLocalHost = "https://localhost:44364/api/";

        public static HttpClient _client;

        public static User LoggedUser { get; set; }

        public static string Token { get; set; }

        static SimpleService()
        {
            _client = new HttpClient();

            _client.BaseAddress = new Uri(baseAddress);
        }

        public static async Task<T> Get<T>(object controller, object method,int? id = null)
        {
            try
            {
                if (id == null)
                {
                    var objects = await _client.GetAsync(controller + $"/{method.ToString()}");

                    return await GetObject<T>(objects);
                }
                else
                {
                    var objects = await _client.GetAsync(controller + $"/{method.ToString()}/{id}");

                    return await GetObject<T>(objects);
                }
            }
            catch (Exception ex)
            {
                throw new InternalServerError(ex);
            }
        }

        public static async Task<T> Post<T>(object controller, object method, T objectToPost)
        {
            try
            {
                var content = new StringContent(JsonHelper.Serialize(objectToPost), Encoding.UTF8,
                    "application/json");

                var responseMessage =
                    await _client.PostAsync(controller + $"/{method}", content);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var json = await responseMessage.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(json))
                        return JsonHelper.Deserialize<T>(json);
                }
                else
                {
                    var json = await responseMessage.Content.ReadAsStringAsync();
                    BadRequestMessage badRequest = JsonHelper.Deserialize<BadRequestMessage>(json);
                    QModernMessageBox.Show(badRequest.Message, "Internal Server Error",
                        QModernMessageBox.QModernMessageBoxButtons.Ok);
                }

                return default(T);
            }
            catch (Exception ex)
            {
                throw new InternalServerError(ex);
            }
        }


        public static async Task<object> PostObject<T>(object controller, object method, object objectToPost)
        {
            try
            {
                var content = new StringContent(JsonHelper.Serialize(objectToPost), Encoding.UTF8,
                    "application/json");

                var responseMessage =
                    await _client.PostAsync(controller + $"/{method.ToString()}", content);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var json = await responseMessage.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(json))
                        return JsonHelper.DeserializeObjectWithType(json,typeof(T));
                }
                else
                {
                    var json = await responseMessage.Content.ReadAsStringAsync();
                    BadRequestMessage badRequest = JsonHelper.Deserialize<BadRequestMessage>(json);
                    QModernMessageBox.Show(badRequest.Message, "Internal Server Error",
                        QModernMessageBox.QModernMessageBoxButtons.Ok);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new InternalServerError(ex);
            }
        }

        private static async Task<T> GetObject<T>(HttpResponseMessage objects)
        {
            if (objects.IsSuccessStatusCode)
            {
                var json = await objects.Content.ReadAsStringAsync();

                return JsonHelper.Deserialize<T>(json);
            }

            return default(T);
        }
    }
}