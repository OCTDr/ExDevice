/////////////////////////
// 编码生成类
// abao++
////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QcPublic
{
    class QcCode
    {
        /// <summary>
        ///  用于从指定的序列中，获取顺序号中的漏号用作编号，无漏号，则使用最后一个编号加1作为新的编号
        /// </summary>
        /// <param name="basenode">修改可以为空，用于支持用户管理的类编号</param>
        /// <param name="lst">同级别列表，用获取下一个可用的序号</param>
        /// <param name="partcodestart">编码开始位置</param>
        /// <returns></returns>
        ///   
       public static string GetNextPartNumber(IQcNode basenode,IEnumerable<IQcNode> lst, int partcodestart=0 )
        {
            return GetNextNumber(basenode, lst, partcodestart, 2, "00");
        }

        public static string GetNextNumber( IEnumerable<IQcNode> lst)
       {
           return GetNextNumber("", lst, 0, 100, "");
       }

        public static string GetNextNumber(IQcNode basenode,
            IEnumerable<IQcNode> lst,
            int partcodestart=0,
            int width=2,
            string format=null)
        {
            string codeprefix = basenode == null ? "" : basenode.Code;
            return GetNextNumber(codeprefix, lst, partcodestart, width, format);
        } 
        /// <summary>
        /// 获取下一个编号
        /// </summary>
        /// <param name="codeprefix"></param>
        /// <param name="lst"></param>
        /// <param name="partcodestart"></param>
        /// <param name="width"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetNextNumber(string codeprefix,
            IEnumerable<IQcNode> lst,
            int partcodestart=0,
            int width=2,
            string format=null)
        {
            
            if(format==null) format = "000000000000".Substring(0, width);
            if (lst == null) return codeprefix + (1.ToString(format));            
            ///获取子节点编码的前两位
            var lstnumber = from p in lst                         
                            where p.Code!="" 
                            select
                            int.Parse(p.Code.Substring(partcodestart, 
                            (width<p.Code.Length)?width:p.Code.Length ));
           lstnumber=lstnumber.OrderBy(t => t);//按照数字排序
            if (lstnumber.Count() == 0||lstnumber.First() > 1) return codeprefix + (1.ToString(format));            
            int before = -1;
            //遍历并求取差值，如果大于1，则用前一个号码加一作为新号码
            foreach (var n in lstnumber)
            {
                if (before != -1)                
                {
                    if (n - before > 1)
                        return codeprefix + (before + 1).ToString(format);
                }
		 before = n;
            }
            return codeprefix  + (lstnumber.Last() + 1).ToString(format);
        }
    }
}
