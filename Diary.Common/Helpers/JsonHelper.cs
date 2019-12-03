using System;
using Newtonsoft.Json;

namespace Diary.Common.Helpers
{
    public static class JsonHelper
    {
        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string Serialize(object json)
        {
            return JsonConvert.SerializeObject(json);
        }

        public static object DeserializeObjectWithType(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
        }

        public static object DeserializeObject(string json)
        {
            return JsonConvert.DeserializeObject(json);
        }
    }
}