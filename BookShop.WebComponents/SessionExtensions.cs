using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BookShop.WebComponents
{
    public static class SessionExtensions
    {
        private static IFormatter _serializer = new BinaryFormatter();

        public static T GetOrAdd<T>(this ISession session, string key, Func<T> creator) where T : class
        {
            var data = session.Get<T>(key);

            if (data == null)
            {
                data = creator();

                Set(session, key, data);
            }

            return data;
        }

        public static T Get<T>(this ISession session, string key) where T : class
        {
            if (session.TryGetValue(key, out byte[] data) == true)
            {
                using (var stream = new MemoryStream(data))
                {
                    return _serializer.Deserialize(stream) as T;
                }
            }

            return null;
        }

        //public static T Get<T>(this ISession session, string key) where T : class
        //{
        //    if (session.TryGetValue(key, out byte[] data) == true)
        //    {
        //        var json = Encoding.Default.GetString(data);
        //        JsonConvert.DeserializeObject<T>(json);
        //    }

        //    return null;
        //}

        public static void Set(this ISession session, string key, object data)
        {
            using (var stream = new MemoryStream())
            {
                _serializer.Serialize(stream, data);

                session.Set(key, stream.ToArray());
            }
        }

        //public static void Set(this ISession session, string key, object data)
        //{
        //    var json = JsonConvert.SerializeObject(data);
        //    var bytes = Encoding.UTF8.GetBytes(json);
        //    session.Set(key, bytes);
        //}
    }
}
