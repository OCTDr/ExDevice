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
    public class QcCheckOperator:QcNode
    {
        public static readonly string TableName = "QC_INS_CHECKOPERATOR";
        //这是无法更新的类
        public QcCheckOperator(DataRow row):base(row,"")
        {

        }
        public override string CodeField
        {
            get { return "算子编码"; }
        }
        public override string Name { get { return this["算子名称"]; } set { } }
        public override string Code { get { return this["算子编码"]; } set { } }
        public override QcCheckResult Check(string strField = null)
        {
            return null;
        }
    }
    public class QcProductOperator : DynamicDataRowObject
    {
        
       public static readonly string NameField="名称";
       QcCheckOperator m_CheckOperator=null;

       public QcCheckOperator CheckOperator{
            get{return m_CheckOperator;}
        }
        static List<QcCheckOperator> m_lstCheckOperator = null;
        public static List<QcCheckOperator> CheckOperators
        {
            get
            {
                if (m_lstCheckOperator==null)
                {
                    InitOperators();
                }
                return m_lstCheckOperator;
            }
        }
        private static void InitOperators()
        {
            string sql="Select * from "+QcCheckOperator.TableName ;
            m_lstCheckOperator=(from p in DbHelper.Query(sql) select new QcCheckOperator(p)).ToList();
        }
       public static readonly string TableName = "QC_INS_PRODUCTOPERATOR";
       public string Name
       {
           get { return m_CheckOperator.Name; }
       }
       public static QcCheckOperator GetCheckOperatorByCode(string code)
       {
           return CheckOperators.FirstOrDefault(t => t.Code == code);
       }
        private QcCheckOperator GetCheckOperatorByName(string name)
        {
            return CheckOperators.FirstOrDefault(t => t.Name == name);
        }
       public QcProductOperator(QcCheckEntry parent,DataRow row) : base(row, TableName) 
       { 
           Parent = parent;
           m_CheckOperator = GetCheckOperatorByCode(row["算子编码"].ToString());
       }      
        
         public QcProductOperator(QcCheckEntry parent,string code) : base(null, TableName)
        {
            Parent = parent;
            m_CheckOperator = GetCheckOperatorByCode(code);
        }

          public QcCheckEntry Parent = null;
          public string Code
          {
              get
              {
                  return this.CheckOperator.Code;
              }
          }
          public string GetNextPartCode()
          {
              //谁会运行到这里呢？
              return null;
          }
          public QcCheckResult Check(string field=null)
          {             
              return null;           
          }
          public QcProductOperator CloneTo(QcCheckEntry  Parent)
          {
              QcProductOperator qce = new QcProductOperator (Parent,this.CheckOperator.Name);
              return qce;
          }
          public static List<QcProductOperator> GetQcProductOperator(QcCheckEntry  checkentry)
          {
              var pls = DbHelper.Query("Select * from "+QcProductOperator.TableName+" where 检查项编码='"+checkentry.Code+"'");
              return (from p in pls select new QcProductOperator(checkentry,p)).ToList();
          }
         
          public override bool DeleteFromDb(QcDbTransaction trans)
          {
              bool ret=base.DeleteFromDb(trans);
              if(ret) Parent.Nodes.Remove(this);
              return ret;
          }
          public override bool Update(QcDbTransaction trans)
          {
              if (IsNew())
              {
                  this["检查项编码"] = Parent.Code;
                  this["标准检查项编码"] = Parent["标准检查项编码"];
                  this["算子编码"] = CheckOperator.Code ;
                  this["产品类别编码"] = Parent.Parent.Parent.Parent.Code;
                  this["产品级别编码"] = Parent.Parent.Parent.Parent.Parent.Code;
                  
              }
              //QcLog.LogString("Start QcProductOperator Update "+this.Name );
              bool ret=base.Update(trans);
              if (ret == false && IsNew())
              {
                  //QcLog.LogString("QcProductOperator Update Fail! "+this.Name );
                  this["检查项编码"] = "";
              }
              if (ret)
              {
                  if(Parent.Nodes.Contains(this))
                    Parent.Nodes.Add(this);
              }
              return ret;
          }
    }
}
