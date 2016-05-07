using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
/******************************
** 引用程序集
** System.Web.Extensions、System.Runtime.Serialization
** .Net Framework3.5版本(.net Framework4.0版本已经存在于System.Runtime.Serialization)还需要加入：
** System.ServiceModel.Web的程序引用集
******************************/

/******************************
** 命名空间:System.Text.RegularExpressions
******************************/

namespace Util
{
    public static class JsonHelper
    {
        /*****************************
       ** 作者： 
       ** 变更时间： 2011-6-6
       ******************************/
        #region###将对象序列化为字符串
        public static string Jso_ToJSON<T>(this T tem_obj)
        {
            JavaScriptSerializer tem_serializer = new JavaScriptSerializer();
            string jsonString = tem_serializer.Serialize(tem_obj);

            //替换Json的Date字符串
            string p = @"\\/Date\(-?\d{0,16}\)\\/";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);
            return jsonString;
        }
        #endregion

        /// <summary>
        /// 将Json序列化的时间由/Date(1294499956278+0800)转为字符串
        /// </summary>
        private static string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            Regex reg = new Regex(@"-?\d{1,16}");       //当时间小于1970-1-1时，时间数字为负数
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(reg.Match(m.Groups[0].Value).Groups[0].Value) + 28800000);   //加上8个小时的时区差
            result = dt.ToString("yyyy-MM-dd");
            return result;
        }



        /// <summary>
        /// 将时间字符串转为Json时间
        /// </summary>
        private static string ConvertDateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            return result;
        }

        /*****************************
        ** 作者： 
        ** 变更时间： 2011-6-6
        ******************************/
        #region###将对象序列化为字符串
        public static string Jso_ToJSON(this object tem_obj, int tem_recursionDepth)
        {
            JavaScriptSerializer tem_serializer = new JavaScriptSerializer();
            tem_serializer.RecursionLimit = tem_recursionDepth;
            return tem_serializer.Serialize(tem_obj);
        }
        #endregion

        /*****************************
        ** 作者： 
        ** 变更时间： 2011-6-6
        ******************************/
        #region###将字符串反序列化为对象
        public static object JsonToObject(string jsonString, object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            return serializer.ReadObject(mStream);
        }
        #endregion

        /*****************************
        ** 作者： 
        ** 变更时间： 2011-6-6
        ******************************/
        #region###将字符串反序列化为对象
        public static T Jso_DeJSON<T>(this string json)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(json);
        }
        #endregion

        /*****************************
        ** 作者： 
        ** 变更时间： 2012-4-12
        ******************************/
        #region###将DataTable表序列化为JSON字符串
        //public static string DataTable_ToJson(DataTable dt, int start, int limit)
        //{
        //    string json = "[";
        //    DataColumnCollection dcc = dt.Columns;      //DataTable列集合
        //    for (int r = start; r < start + limit && r < dt.Rows.Count; r++)
        //    {
        //        DataRow row = dt.Rows[r];
        //        json += "{";
        //        string rowJson = string.Empty;
        //        for (int i = 0; i < dcc.Count; i++)
        //        {
        //            rowJson += string.Format("\"{0}\":\"{1}\",", dcc[i].ColumnName, row[dcc[i].ColumnName]);
        //        }
        //        rowJson = rowJson.Remove(rowJson.LastIndexOf(','));
        //        json += rowJson + "},";
        //    }
        //    int n = json.LastIndexOf(',');
        //    if (n >= 0) json = json.Remove(n);
        //    json += "]";
        //    return json;
        //}
        #endregion


        #region###将JSON字符串反序列化为泛型T对象
        /// <summary>
        /// 将JSON字符串反序列化为泛型T对象
        /// </summary>
        /// <typeparam name="T">泛型T</typeparam>
        /// <param name="jsonString">JSON字符串</param>
        /// <returns>泛型T对象</returns>
        public static T JsonDeserialize<T>(this string jsonString)
        {
            T tObj = Activator.CreateInstance<T>();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(tObj.GetType());
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }
        #endregion

    }
}
