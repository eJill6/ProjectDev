using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections;

namespace SLPolyGame.Web.DBUtility
{
    public static class SafeSql
    {
        static List<string> strictsafestr = new List<string>() { "'", "=", ">", "<", "script", "object", "applet", "[", "]", "select", "execute", "exec", "join", "union", "where", "insert", "delete", "update", "like", "drop", "create", "rename", "count", "chr", "mid", "truncate", "nchar", "char", "alter", "cast", "exists" };
        static List<string> safestr = new List<string>() { "'", "=", ">", "<", "[", "]", ";", "--", "xp_", "%", "~" };
        public static bool CheckParams(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < safestr.Count; i++)
                {
                    if (str.ToLower().Contains(safestr[i].ToLower()))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool StrictCheckParams(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < strictsafestr.Count; i++)
                {
                    if (str.ToLower().Contains(strictsafestr[i].ToLower()))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}