using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RedisVsDapperDemo.Helper
{
    public class GlobalConfigModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Keys { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<int> LimitedServiceIds { get; set; }
        public string Key { get; set; }
    }
    public class ConfigHelper
    {
        public const string GLOBAL_REQUEST_HEADER_KEY_NAME = "key";
        public const string DEFAULT_CONFIG_FILE_NAME = "appsettings.json";
        public const string GLOBAL_CONFIG_KEY_NAME = "GlobalConfig";
        public const string GLOBAL_CONFIG_URL_KEY_NAME = "GlobalConfigUrl";
        public const string SERVICE_ID_KEY_NAME = "ServiceId";
        public const string CONNECTION_STRINGS_KEY_NAME = "ConnectionStrings";
        public const string MAIN_CONNECTION_KEY_NAME = "MainConnection";
        public const string PROXY_KEY_NAME = "Proxy";
        public static Dictionary<string, string> ConnectionStrings { get; private set; }
        private static DateTime DefaultDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        public static GlobalConfigModel Global { get; private set; }
        private static IConfigurationRoot Configuration { get => LazyConfig.Value; }
        private static Lazy<IConfigurationRoot> LazyConfig = new Lazy<IConfigurationRoot>(() =>
        {
            return BuildConfig();
        });
        public static void Init()
        {
            var c = Configuration;
        }
        public static IConfigurationRoot BuildConfig(string ConfigFilePath = DEFAULT_CONFIG_FILE_NAME)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(ConfigFilePath, true, true);
            var result = builder.Build();
            ConnectionStrings = Get<Dictionary<string, string>>(CONNECTION_STRINGS_KEY_NAME, result);
            var globalConfigURL = result[GLOBAL_CONFIG_URL_KEY_NAME];
            if (string.IsNullOrWhiteSpace(globalConfigURL)) Global = Get<GlobalConfigModel>(GLOBAL_CONFIG_KEY_NAME, result);
            else
            {
                //var ServiceId = result[SERVICE_ID_KEY_NAME];
                //var pass = result[StringCipher.DEFAULT_PASS_KEY_NAME];
                //if (string.IsNullOrWhiteSpace(ServiceId) ||
                //    string.IsNullOrWhiteSpace(pass) ||
                //    !int.TryParse(ServiceId, out var C)) return result;
                //Retry:
                //try
                //{
                //    string Key = StringCipher.Encrypt($"[{ServiceId},{(long)(DateTime.UtcNow.AddSeconds(5) - DefaultDate).TotalSeconds}]", pass);
                //    var httpClient = new HttpClient();
                //    httpClient.DefaultRequestHeaders.Add(GLOBAL_REQUEST_HEADER_KEY_NAME, Key);
                //    var response = httpClient.GetAsync(globalConfigURL).Result;
                //    if (response.StatusCode != HttpStatusCode.OK)
                //    {
                //        Task.Delay(5000).Wait();
                //        goto Retry;
                //    }
                //    var globalStr = response.Content.ReadAsStringAsync().Result;
                //    globalStr = JsonConvert.DeserializeObject<ResponseData<string>>(globalStr).Data;
                //    if (!string.IsNullOrWhiteSpace(globalStr))
                //    {
                //        globalStr = StringCipher.Decrypt(globalStr, pass);
                //        if (!string.IsNullOrWhiteSpace(globalStr))
                //            Global = JsonConvert.DeserializeObject<GlobalConfigModel>(globalStr);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    Task.Delay(5000).Wait();
                //    goto Retry;
                //}
            }
            return result;
        }
        public static bool ValidTime(long Time)
        {
            return (DateTime.UtcNow - DefaultDate).TotalSeconds <= Time;
        }
        //public static ResponseData<string> GetEncryptedGlobalConfig(int ServiceId)
        //{
        //    if (Global.LimitedServiceIds?.Count > 0 &&
        //        !Global.LimitedServiceIds.Contains(ServiceId)) return new ResponseData<string>(ResponseStatusCode.NotFound);
        //    var cloned = Get<GlobalConfigModel>(GLOBAL_CONFIG_KEY_NAME);
        //    cloned.LimitedServiceIds = null;
        //    var resultString = JsonConvert.SerializeObject(cloned);
        //    return new ResponseData<string>(StringCipher.Encrypt(resultString));
        //}
        public static string Get(string Key)
        {
            return Configuration[Key];
        }
        public static IConfigurationSection GetSection(string Key)
        {
            return Configuration.GetSection(Key);
        }
        public static T Get<T>(string Key)
        {
            return Get<T>(Key, Configuration);
        }
        public static T Get<T>(string Key, IConfiguration Configuration)
        {
            return Configuration.GetSection(Key).Get<T>();
        }
    }
}
