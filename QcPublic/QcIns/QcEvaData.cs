using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace QcPublic
{
    public static  class EvaPara
    {        
        public static double R0 = 5;
        public static  int N1 = 2200;
        public static int N2 = 2200;
        public static int N3 = 2200;
        public static int N4 = 2200;
        public static int N5 = 2200;
    }
    
    public class QcEvaData:QcEvaDataEntry<QcEvaQaData>
    {
        public string 质量等级
        {
            get
            {
                if (得分 > 90)
                    return "优";
                else if (得分 > 75) return "良";
                else if (得分 > 60) return "合格";
                else
                    return "不合格";
            }
        }
        public QcEvaData(QcEvaDataRow Row):base(Row.数据名称)
        {            
            this.Data = Row;        
        }
        private QcEvaDataRow Data = null;
        public string 数据名称 { get { return name; } }
        public string 数据ID { get { return this.Data.数据ID; } }
        public void Calc()
        {
            lst.ForEach(t => t.Calc());
            this.得分 = QcEvaCalcHelper.Calc(this, lst);

        }
        public void UpdateToChkData()
        {
            string str = "";
            if (this.得分 == QcEvaConst.未检查分数)
            {
                str = "未检查";
            }
            else if (this.得分 == QcEvaConst.配置错误分数)
            {
                str = "配置错误";
            }
            else
            {
                str = this.得分.ToString();
            }
            string update = string.Format("update qc_pro_checkdata d set d.评价得分='{0}' where d.数据id='{1}'", str, this.数据ID);
            DbHelper.Execute(update);
        }


        public void statistics()
        {
            //string sql = "update " + QcEvaCheckReCode.TableName + " t set t.A类错误='',t.B类错误='',t.C类错误='',t.D类错误='',t.备注='', t.描述类型='' where 数据ID='" + Data.数据ID + "' ";
            //DbHelper.Execute(sql);
            //string errorsql = "select cd.数据id,cr.检查项编码,ec.错误id,ec.错漏数量,ec.检查对象,ec.错误描述,ec.错漏类别,ec.描述类型,ec.备注 " +
            //        " from qc_ins_erorecode ec, Qc_Pro_Checkdata cd, Qc_Ins_Checkrule cr,qc_pro_job j " +
            //" where j.方案id=cr.方案id  and cd.作业编号=j.作业编号 and ec.数据id=cd.数据id and ec.规则id=cr.规则id and ec.数据id='" + Data.数据ID + "'";
            string errorsql = "select cd.数据id,cr.检查项编码,ec.错误id,ec.错漏数量,ec.检查对象,ec.错误描述,ec.错漏类别,ec.描述类型,ec.参数,ec.是否确认 " +
                    " from qc_ins_erorecode ec, Qc_Pro_Checkdata cd, Qc_Ins_Checkrule cr " +
            " where ec.数据id=cd.数据id and ec.规则id=cr.规则id and ec.数据id='" + Data.数据ID + "' and ec.是否确认=-1";
            var errors = DbHelper.Query(errorsql).Select(t=>new QcEvaDataRow(t,t["检查项编码"].ToString()));
            lst.ForEach(t => t.statistics(errors));
            //Parallel.ForEach(lst, t => t.statistics());
        }
    }
}
