/////////////////////
// 用于管理基于DataRow动态类的每个字段值的管理和修改
// abao++
//////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace QcPublic
{
    /// <summary>
    /// 数据检查结果类，为字段和字段检验结果的映射
    /// </summary>
    public class QcCheckResult : Dictionary<string, string>
    {
        /// <summary>
        /// 被检查的对象
        /// </summary>
        private object m_CheckObject = null;
        /// <summary>
        ///  被检查的对象
        /// </summary>
        public Object CheckObject { get { return m_CheckObject; } }
        /// <summary>
        /// 构建针对对象的检查结果
        /// </summary>
        /// <param name="obj"></param>
        public QcCheckResult(DynamicDataRowObject  obj)
            : base()
        {
            m_CheckObject = obj;
        }
        /// <summary>
        /// 添加一条字段的错误信息
        /// </summary>
        /// <param name="field"></param>
        /// <param name="errormessage"></param>
        public new void Add(string field, string errormessage)
        {
            if (string.IsNullOrEmpty(errormessage)) return;
            if (base.ContainsKey(field))
            {
                base[field] += errormessage;
            }
            else
                base.Add(field, errormessage);
        }
        public static bool RemoveError(ListView lstview, object obj, QcCheckResult result = null)
        {
            for (int i = lstview.Items.Count - 1; i >= 0; i--)
            {
                ListViewItem item = lstview.Items[i];
                if (item.Tag == obj)
                {
                    lstview.Items.Remove(item);
                }
            }
            return true;
        }
        public bool AddToList(ListView lstview)
        {

            QcCheckResult.RemoveError(lstview, this.CheckObject, this);
            foreach (var v in this)
            {
                string name = (this.CheckObject as IQcNode).Name;
                if (string.IsNullOrEmpty(name))
                    name = "新建项";
                ListViewItem item = lstview.Items.Insert(0, name);
                item.SubItems.Add(v.Key);
                item.SubItems.Add(v.Value);
                item.Tag = this.CheckObject;
                item.ToolTipText = v.Value;
            }
            return true;
        }
        public void AddCheckNull(string checkfield, IEnumerable<string> fields)
        {
            foreach (string field in fields)
            {
                if (checkfield == null || checkfield == field)
                    this.Add(field, (this.m_CheckObject as DynamicDataRowObject).CheckNull(field));
            }

        }
        public void AddCheckUsed(string checkfield, IEnumerable<IQcNode> nodes, IEnumerable<string> fields, int count = 1)
        {
            foreach (var field in fields)
            {
                if (checkfield == null || checkfield == field)
                {
                    //不等于自身编码的要素才叫重复
                    if (nodes.Count(t => t[field] == (this.m_CheckObject as DynamicDataRowObject)[field] && t.Code !=(this.m_CheckObject as QcNode).Code ) > count)
                    {
                        this.Add(field, "重复");
                    }
                }
            }
        }
        public void AddCheckEnable(string checkfield, IEnumerable<string> fields)
        {
            foreach (var field in fields)
            {
                if (checkfield == null || checkfield == field)
                {
                    //不等于自身编码的要素才叫重复
                    if (field == "产品级别编码")
                    {
                        try
                        {
                            int num = Convert.ToInt16((this.m_CheckObject as DynamicDataRowObject)[field]);
                            if (num < 100 || num > 999)
                            {
                                this.Add(field, "无效产品级别编码，请输入[100-999]三位数字");
                            }

                        }
                        catch
                        {
                            this.Add(field, "无效产品级别编码，请输入[100-999]三位数字");
                        }
                    }
                }
            }
        }
        public void AddCheckEnum(string checkfield, string field, IEnumerable<string> enumvalues)
        {
            if (checkfield == null || checkfield == field)
            {
                if (enumvalues.Contains((this.CheckObject as DynamicDataRowObject)[field]) == false)
                {
                    this.Add(field, "不是有效的值");
                }
            }
        }
        public static string[] Enum计分方式 = { "最低分", "加权平均值" };
        public static string[] Enum结果值枚举 = { "Y/N", "R:R0", "M;M0", "A?B?C?D?" };
          
    }
}
