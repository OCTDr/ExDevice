////////////////////////////////////
//   检查项类
//  abao++
///////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace QcPublic
{
    public class QcCheckEntry : QcNode
    {
       public static readonly string NameField="名称";

       public static readonly string TableName = "QC_EVA_CHECKENTRY";
       public override  string Name
       {
           get { return row[QcCheckEntry.NameField ].ToString(); }
           set { Row[QcCheckEntry.NameField ] = value; }

       }
       public QcCheckEntry(QcSubQuaelement parent) : base(null, TableName) { Parent = parent; }
          private QcCheckEntry(DataRow row)
              : base(row)
          {
              if (row == null) return;
          }
          public QcSubQuaelement Parent = null;
          public override string CodeField
          {
              get { return "检查项编码"; }
          }
          public override  string Code
          {
              get
              {
                  return this["检查项编码"];
              }
              set
              {
                  if (string.IsNullOrEmpty(value))
                  {
                      this["检查项编码"] = "";
                      this["质量子元素编码"] = "";
                  }
                  else
                  {
                      this["检查项编码"] = value;
                      this["质量子元素编码"] = value.Substring(0, 9);
                  }
              }
          }
          public string GetNextPartCode()
          {
              //谁会运行到这里呢？
              return null;
          }
          public override QcCheckResult Check(string field = null)
          {
              QcCheckResult result = new QcCheckResult(this);
              bool checkall = (field == null);
              result.AddCheckNull(field, new[] { "名称", "计分方式", "分组","结果值枚举" });
              result.AddCheckUsed(field, Parent.Nodes, new[] { "名称" });
              result.AddCheckEnum(field, "计分方式", 计分方式Converter.计分方式s);
              result.AddCheckEnum(field, "结果值枚举", 结果值枚举Converter.结果值枚举s);              
              result.AddCheckEnum(field, "备注", 检查项备注Converter.检查项备注s);
              if (result.Count > 0) return result;
              return null;           
          }


          //public class 结果值枚举Converter : QcTypeConverter
          //{
          //    public static string[] 结果值枚举 = new string[] { "Y/N", "R:R0", "M;M0", "A?B?C?D?" };
          //    public 结果值枚举Converter() : base(结果值枚举) { }

          //}
                
          public QcCheckEntry  CloneTo(QcSubQuaelement Parent)
          {
              QcCheckEntry qce = new QcCheckEntry(Parent);
              qce["标准检查项编码"] = this["标准检查项编码"];
              qce.CloneFields(this);
              return qce;
          }
          public static List<QcCheckEntry> GetCheckEntry(QcSubQuaelement SubQuaelement)
          {
              var pls = DbHelper.Query("Select * from QC_EVA_CHECKENTRY where 质量子元素编码='" + SubQuaelement.Code+ "'");
              return (from p in pls select new QcCheckEntry(p) { Parent=SubQuaelement}).ToList();
          }
          List<QcProductOperator> m_Nodes = null;
          public List<QcProductOperator> Nodes
          {
              get
              {
                  if (m_Nodes == null)
                      m_Nodes = QcProductOperator.GetQcProductOperator(this);
                  if (m_Nodes == null)
                      m_Nodes = new List<QcProductOperator>();
                  return m_Nodes;
              }
          }
        /// <summary>
          /// where 检查项编码 like=expr
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
          public static List<QcCheckEntry> GetCheckEntry(string expr)
          {
              var pls = DbHelper.Query("Select * from QC_EVA_CHECKENTRY where 检查项编码 like '" + expr + "%'");
              if (pls == null) return new List<QcCheckEntry>();
              return (from p in pls select new QcCheckEntry(p) { Parent = null }).ToList();
          }
          /// <summary>
          /// where expr is not null
          /// </summary>
          /// <param name="expr"></param>
          /// <returns></returns>
          public static List<QcCheckEntry> GetCheckEntrySql(string expr)
          {
              string sql = expr == "" ? "Select * from QC_EVA_CHECKENTRY" : "Select * from QC_EVA_CHECKENTRY where " + expr;
              var pls = DbHelper.Query(sql);
              if (pls == null) return new List<QcCheckEntry>();
              return (from p in pls select new QcCheckEntry(p) { Parent = null }).ToList();
          }
          public override bool DeleteFromDb(QcDbTransaction trans)
          {
              if (Nodes.ToArray().Any(t => t.DeleteFromDb(trans) == false)) return false;
              bool ret=base.DeleteFromDb(trans);
            
              if(ret) Parent.Nodes.Remove(this);
              return ret;
          }
          public override bool Update(QcDbTransaction trans)
          {

              if (IsNew()) this.Code = Parent.GetNextPartCode();
              //QcLog.LogString("Start QcCheckEntry Update");
              bool ret = base.Update(trans);
              if (ret == false && IsNew()) { this.Code = ""; return false; }
              if (Nodes.ToArray().Any(t => t.Update(trans) == false))
              {
                  if (IsNew()) this.Code = "";
                  return false;
              }
              if (ret)
              {
                  if(Parent.Nodes.Contains(this)==false)
                    Parent.Nodes.Add(this);
              }
              return ret;
          }
          public object Tag { get; set; }
    }
}
