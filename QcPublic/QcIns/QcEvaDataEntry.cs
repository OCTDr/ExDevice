using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QcPublic
{
    

    public class QcEvaDataEntry
    {
        public double A类错误;
        public double B类错误;
        public double C类错误;
        public double D类错误;
        private double m_得分;
        public double 得分 { 
            get {
                if (m_得分 != QcEvaConst.配置错误分数 && m_得分 != QcEvaConst.未检查分数)
                {
                    if (QcEvaConst.设置负分为零分)
                    { 
                        return m_得分 < 0 ? 0 : Math.Round(m_得分, 1);
                    }
                    else
                    {
                        return Math.Round(m_得分, 1);
                    }
                }
            return Math.Round(m_得分, 1); 
            } 
            set { m_得分 = value; }
        }
        public double 权值;        
        public string 计分方式 { get; set; }
        public string 分组 { get; set; }
        public double 分组得分 { get; set; }
        virtual public QcEvaDataEntry.EvaNodeStats 是否合格 { get { return Stats; } }
        public enum EvaNodeStats
        {            
            合格,
            不合格
        }
        public EvaNodeStats Stats = EvaNodeStats.合格;
    }

    public class QcEvaDataEntry<T>:QcEvaDataEntryBase<T> where T :QcEvaDataEntry
    {
        public string 得分字符串
        {
            get
            {
                if (this.得分 == QcEvaConst.未检查分数) return "未检查";
                if (this.得分 == QcEvaConst.配置错误分数) return "配置错误";
                else
                    return this.得分.ToString();
            }
            
        }
     override  public QcEvaDataEntry.EvaNodeStats 是否合格
     {
        get
        {
                if (Math.Min(得分, 分组得分) < PublicConstValue.PublicConst.cWarningScore)
                {
                    return QcEvaCheckData.EvaNodeStats.不合格;
                }
                else
                {
                    if (Nodes.Any(t => t.是否合格 == QcEvaDataEntry.EvaNodeStats.不合格))
                        return QcEvaDataEntry.EvaNodeStats.不合格;
                    return QcEvaDataEntry.EvaNodeStats.合格;
                }  

        }
     }
    
         public QcEvaDataEntry(string name):base(name){}  
    }


    public class QcEvaDataEntryBase<T> : QcEvaDataEntry 
    {
        protected List<T> lst = new List<T>();
        public IEnumerable<T> Nodes
        {
            get
            {
                return lst;
            }
        }        

        protected string name = null;
        public string Name { get { return name; } }
        public QcEvaDataEntryBase(string name)
        {
            this.name = name;
            this.分组得分 = 1000000000;
            this.分组 = "不分组";
        }

        public void Add(T node)
        {
            lst.Add(node);
        }
    }
}
