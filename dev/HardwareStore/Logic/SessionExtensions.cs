using HardwareStore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HardwareStore.Logic
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public static void AddObject<T>(this ISession session, string key, T value)
        {
            var arr = session.GetObject<List<T>>(key) ?? new List<T>();
            arr.Add(value);
            session.SetObject(key, arr);
        }

        public static void RemoveObject<T>(this ISession session, string key, ref T value)
        {
            var arr = session.GetObject<List<T>>(key);
            if(arr != null)
            {
                arr.Remove(value);
                session.SetObject(key, arr);
            }
        }
    }
}
