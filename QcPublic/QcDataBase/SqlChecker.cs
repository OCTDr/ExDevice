/////////////////////////
//  sql关键字和非法字符过滤类
// abao++
////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QcPublic
{
    public class SqlChecker
    {
        private static string[] StrKeyword = "select |insert |delete |from |drop table|update |truncate |exec master|netlocalgroup |net user|or |and |=|!|'".Split('|');
  /// <summary>
  /// 检查指定的关键字是否包含sql语句的关键字，避免sql注入
  /// </summary>
  /// <param name="keyword"></param>
  /// <returns></returns>
        public static bool CheckKeyword(string keyword)
        {
            if(StrKeyword.Any(t=>keyword.Contains(t))) return false;
            return true;
            
        }
    }
}
