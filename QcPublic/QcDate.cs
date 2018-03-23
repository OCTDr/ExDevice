// 日期扩展类
// abao++ 
//这是一个日期的扩展，以便于日期格式化为统一的字符串，而不受到用户的时间设置变化
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QcPublic
{
   public  static class QcDate
    {
        public static string DateString()
        {
            return DateTime.Now.ToQcDateString();
        }
        /// <summary>
        /// 获取年-月-日 时:分:秒.毫秒 的格式
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public static string ToQcDateString(this DateTime date)
        {
            StringBuilder sb = new StringBuilder(40);
            sb.Append(date.Year)
                .Append('-')
                .Append(date.Month)
                .Append('-')
                .Append(date.Day)
                .Append(' ')
                .Append(date.Hour)
                .Append(':')
                .Append(date.Minute)
                .Append(':')
                .Append(date.Second)
               // .Append('.')
               // .Append(date.Millisecond)
                ;
            return sb.ToString();
        }
    }
}
