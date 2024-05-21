namespace BackSideWeb.Helpers
{
    public class AppHelper
    {
        private static IConfiguration _config;

        public AppHelper(IConfiguration configuration) {
            _config = configuration;
        }

        public static string ReadAppSettings(params string[] session)
        {
            try
            {

                if (session.Any())
                {
                    return _config[string.Join(":", session)];
                }
            }
            catch {
                return "";
            }

            return "";
        }

        /// <summary>
        /// 读取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <returns></returns>
        public static List<T> ReadAppSetting<T>(params string[] session)
        {
            List<T> list = new List<T>();
            _config.Bind(string.Join(':', session), list);
            return list;
        }
    }
}
