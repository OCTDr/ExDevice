////////////////////////////////////
//   节点接口
//  abao++
///////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QcPublic
{
    public interface IQcNode
    {
        /// <summary>
        ///  节点名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        ///  编码
        /// </summary>
        string Code { get; set; }
        /// <summary>
        /// 获取制定字段的值
        /// </summary>
        /// <param name="Field"></param>
        /// <returns></returns>
        string this[string field] { get; set; }
        /// <summary>
        ///  检查已有数据的合法性
        /// </summary>
        /// <returns></returns>
        QcCheckResult Check(string strField=null);
        

        
        
    }
}
