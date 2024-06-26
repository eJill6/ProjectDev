using System;
using System.Text.RegularExpressions;
using System.Web;

namespace Web.Helpers
{

    /// <summary>
    /// Lottery helper class.
    /// </summary>
    public static class CommonHelper
    {
        private static string[] array = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "S", "Y", "Z" };

        /// <summary>
        /// Bet success flag.
        /// </summary>
        public const string SuccessFlag = "ok";

        /// <summary>
        /// Get lottery result arrary.
        /// </summary>
        /// <param name="lotteryNum">Lottery result num.</param>
        /// <returns>Lottery result arrary.</returns>
        public static string[] GetLotteryResult(string lotteryNum)
        {
            if (!string.IsNullOrEmpty(lotteryNum))
            {
                return lotteryNum.Split(',');
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get countdown start value.
        /// </summary>
        /// <param name="futureTime">Future time</param>
        /// <param name="format">Format(dd:hh:mm:ss)</param>
        /// <returns>Countdown start value.</returns>
        public static string GetCountdownStartValue(DateTime futureTime, string format = "")
        {
            string startValue = string.Empty;
            var now = DateTime.Now;
            var differ = TimeSpan.MinValue;
            if (futureTime >= now)
            {
                differ = futureTime - now;
            }
            else
            {
                differ = now - futureTime;
            }
            if (string.IsNullOrEmpty(format))
            {
                startValue = string.Empty;
                if (differ.Days > 0)
                {
                    startValue += string.Format("{0:00}:", differ.Days);
                }
                if (!string.IsNullOrEmpty(startValue) || differ.Hours > 0)
                {
                    startValue += string.Format("{0:00}:", differ.Hours);
                }
                startValue += string.Format("{0:00}:", differ.Minutes);
                startValue += string.Format("{0:00}", differ.Seconds);
            }
            else
            {
                if (format.Contains("dd") && differ.Days > 0)
                {
                    startValue += string.Format("{0:00}:", differ.Days);
                }
                if (format.Contains("hh"))
                {
                    if (!string.IsNullOrEmpty(startValue) || differ.Hours > 0)
                    {
                        startValue += string.Format("{0:00}:", differ.Hours);
                    }
                }
                if (format.Contains("mm"))
                {
                    startValue += string.Format("{0:00}:", differ.Minutes);
                }
                if (format.Contains("ss"))
                {
                    startValue += string.Format("{0:00}:", differ.Seconds);
                }
                startValue = startValue.TrimEnd(':');
            }
            return startValue;
        }

        /// <summary>
        /// Get mask card user.
        /// </summary>
        /// <param name="cardUser">Card user</param>
        /// <param name="mask">Mask</param>
        /// <returns>Mask card user.</returns>
        public static string GetMaskCardUser(string cardUser, char mask = '*')
        {
            var maskCardUser = "**";
            if (!string.IsNullOrEmpty(cardUser))
            {
                if(cardUser.Length > 1)
                {
                    maskCardUser = $"{maskCardUser}{cardUser.Substring(cardUser.Length - 1,1)}";
                }
            }
            return maskCardUser;
        }

        /// <summary>
        /// Calculate page count.
        /// </summary>
        /// <param name="pageSize">Page size.</param>
        /// <param name="totalCount">Total count.</param>
        /// <returns>Page count.</returns>
        public static int CalculatePageCount(int pageSize, Int64 totalCount)
        {
            var pageCount = Math.Ceiling(totalCount / (double)pageSize);
            return (int)pageCount;
        }

        /// <summary>
        /// 十进制转36进制
        /// </summary>
        /// <param name="number"></param>
        /// <param name="result"></param>
        public static void Radix36(int number, ref string result)
        {
            int remainder = number % 36;
            result = array[remainder] + result;
            int a = number / 36;
            if (a >= 36)
            {
                Radix36(a, ref result);
            }
            else if (a > 0)
            {
                result = array[a] + result;
            }
        }

        /// <summary>
        /// http://detectmobilebrowsers.com/
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public static bool IsMobile(out string userAgent)
        {
            userAgent = "";

            bool flag = false;

            if (HttpContext.Current.Request.UserAgent != null)
            {
                string agent = HttpContext.Current.Request.UserAgent.ToLower();

                Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                if ((b.IsMatch(agent) || v.IsMatch(agent.Substring(0, 4))))
                {
                    flag = true;

                    string[] keywords = { "android", "iphone", "ipod", "ipad", "windows phone", "mqqbrowser", "phone", "mobile", "wap", "netfront", "java", "opera mobi", "opera mini", "ucweb", "windows ce", "symbian", "series", "webos", "sony", "blackberry", "dopod", "nokia", "samsung", "palmsource", "xda", "pieplus", "meizu", "midp", "cldc", "motorola", "foma", "docomo", "up.browser", "up.link", "blazer", "helio", "hosin", "huawei", "novarra", "coolpad", "webos", "techfaith", "palmsource", "alcatel", "amoi", "ktouch", "nexian", "ericsson", "philips", "sagem", "wellcom", "bunjalloo", "maui", "smartphone", "iemobile", "spice", "bird", "zte-", "longcos", "pantech", "gionee", "portalmmm", "jig browser", "hiptop", "benq", "haier", "^lct", "320x320", "240x320", "176x220", "w3c ", "acs-", "alav", "alca", "amoi", "audi", "avan", "benq", "bird", "blac", "blaz", "brew", "cell", "cldc", "cmd-", "dang", "doco", "eric", "hipt", "inno", "ipaq", "java", "jigs", "kddi", "keji", "leno", "lg-c", "lg-d", "lg-g", "lge-", "maui", "maxo", "midp", "mits", "mmef", "mobi", "mot-", "moto", "mwbp", "nec-", "newt", "noki", "oper", "palm", "pana", "pant", "phil", "play", "port", "prox", "qwap", "sage", "sams", "sany", "sch-", "sec-", "send", "seri", "sgh-", "shar", "sie-", "siem", "smal", "smar", "sony", "sph-", "symb", "t-mo", "teli", "tim-", "tosh", "tsm-", "upg1", "upsi", "vk-v", "voda", "wap-", "wapa", "wapi", "wapp", "wapr", "webc", "winw", "winw", "xda", "xda-", "Googlebot-Mobile" };

                    foreach (string item in keywords)
                    {
                        if (agent.Contains(item))
                        {
                            userAgent = item;
                            flag = true;
                            break;
                        }
                    }
                }
            }

            return flag;
        }
    }
}
