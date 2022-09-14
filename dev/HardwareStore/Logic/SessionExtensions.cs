using HardwareStore.Models;
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
            var cartThings = session.GetObject<List<T>>(key) ?? new List<T>();
            cartThings.Add(value);
            session.SetObject(key, cartThings);
        }
    }
}
