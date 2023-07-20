using Newtonsoft.Json;

namespace CraveWheels.Extensions
{
    // Naming convention
    // 'Name of Object to Extend' + 'Extensions'
    // it must be a static class
    public static class SessionExtensions
    {
        // 'this' indicates which type of object to extend
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
