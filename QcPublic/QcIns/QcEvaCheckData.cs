using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QcPublic
{
    public class QcEvaCheckData : QcEvaDataEntryBase<QcEvaDataRow>
    {


        public QcEvaCheckData(QcEvaDataRow row, QcEvaCheckReCode recode)
            : base(row.checkentry.Name)
        {
            this.checkentry = row.checkentry;
            this.Data = row;
            this.ReCode = recode;
            Get错误数量();
            this.计分方式 = checkentry["计分方式"];
            this.分组 = checkentry["分组"];
            double.TryParse(checkentry["权值"], out 权值);
        }

        private void Get错误数量()
        {
         
            if (this.ReCode != null)
            {
                
                if(!double.TryParse(ReCode["A类错误"], out this.A类错误)) this.A类错误 =0;
               if(! double.TryParse(ReCode["B类错误"], out this.B类错误))  this.B类错误=0;
               if (!double.TryParse(ReCode["C类错误"], out this.C类错误)) this.C类错误=0;
               if (!double.TryParse(ReCode["D类错误"], out this.D类错误)) this.D类错误=0;
            }
        }
        QcCheckEntry checkentry = null;
        public string 检查结果;
        public string 检查项名称 { get { return name; } }
        public string 结果值枚举 { get { return checkentry["结果值枚举"]; } }
        public string 限差值 { get { return checkentry["限差值"]; } }
        QcEvaPara Para = null;
        QcEvaDataRow Data = null;
        QcEvaCheckReCode ReCode = null;
        public string 描述类型
        {
            get
            {
                return ReCode["描述类型"];
            }
        }
        public string 参数
        {
            get
            {
                return ReCode["参数"];
            }
        }
        public void LoadErrors()
        {
            lst.Clear();
            string errorsql = "select cd.数据id,cr.检查项编码,ec.错误id,ec.错漏数量,ec.检查对象,ec.错误描述,ec.错漏类别,ec.描述类型,ec.参数,ec.是否确认,ec.规则id" +
                    " from qc_ins_erorecode ec, Qc_Pro_Checkdata cd, Qc_Ins_Checkrule cr " +
            " where ec.数据id=cd.数据id and ec.规则id=cr.规则id and ec.数据id='"            
            + Data.数据ID + "' and cr.检查项编码='" + checkentry.Code + "'"
            + " and ec.是否确认=-1  order by ec.错误id"
            ;
            var erros = DbHelper.Query(errorsql);
            if (erros != null)
            {
                foreach (var e in erros)
                {
                    Add(new QcEvaDataRow(e, checkentry));
                }
            }
        }
        /// <summary>
        ///  计算中误差类别的分数
        /// </summary>
        /// <param name="m">检测中误差</param>
        /// <param name="m0">技术设计要求中误差</param>
        /// <returns></returns>


        public void Statistics(IEnumerable<QcEvaDataRow> errors = null)
        {
            if (ReCode == null)
                ReCode = new QcEvaCheckReCode();
            if (errors != null)
            {
                lst.Clear();
                foreach (var e in errors.Where(t => t.检查项编码 == this.checkentry.Code))
                {
                    e.checkentry = this.checkentry;
                    lst.Add(e);
                }
            }
            else
                LoadErrors();
            if (lst.Count > 0)
            {
                double suma = lst.Where(t => t.错漏类别 == "A" && t.描述类型 == "1").Sum(t => double.Parse(t.错漏数量));
                double sumb = lst.Where(t => t.错漏类别 == "B" && t.描述类型 == "1").Sum(t => double.Parse(t.错漏数量));
                double sumc = lst.Where(t => t.错漏类别 == "C" && t.描述类型 == "1").Sum(t => double.Parse(t.错漏数量));
                double sumd = lst.Where(t => t.错漏类别 == "D" && t.描述类型 == "1").Sum(t => double.Parse(t.错漏数量));
                string 描述类型 = lst.Any(t => t.描述类型 == "1" && t.是否确认 != "0") ? "1" : "2";
               // string 参数 = lst.First()["参数"];
                ReCode["数据ID"] = Data.数据ID;
                ReCode["检查项编码"] = Data.检查项编码;
                ReCode["A类错误"] = suma.ToString();
                ReCode["B类错误"] = sumb.ToString();
                ReCode["C类错误"] = sumc.ToString();
                ReCode["D类错误"] = sumd.ToString();
                Get错误数量();
                ReCode["描述类型"] = 描述类型;
            // ReCode["参数"] = 参数;
                ReCode.Update();
            }          
            else
            {
                ReCode["数据ID"] = Data.数据ID;
                ReCode["检查项编码"] = Data.检查项编码;
                ReCode["A类错误"] = "";
                ReCode["B类错误"] = "";
                ReCode["C类错误"] ="";
                ReCode["D类错误"] = "";
                Get错误数量();
                ReCode["描述类型"] = "";
                ReCode["备注"] = "未检查";
                ReCode.Update();
            }
            

        }

        public void Calc()
        {
            if (ReCode == null)
            {
                this.Statistics();
            }
            Para = new QcEvaPara(this.限差值);
            switch (结果值枚举)
            {
                case "M;M0":
                    CalScoreMM0();
                    break;
                case "R:R0":
                    CalScoreRR0();
                    break;
                case "Y/N":
                    if (描述类型 == "1")
                    {
                        if (Para.Count > 1)
                        {
                            double df1=0;
                            if (double.TryParse(Para["N"], out df1) == false)
                            {
                                this.得分 = QcEvaConst.配置错误分数;
                            }
                            else
                            {
                                this.得分 = df1;
                            }
                        }
                        else this.得分 = QcEvaConst.不符合分数;
                    }
                    else if (描述类型 == "")
                    {
                        this.得分 = QcEvaConst.未检查分数;
                    }
                    else
                    {
                        if (Para.Count > 0)
                        {
                            double df2=0;
                            if(double.TryParse(Para["Y"],out df2)==false)
                            {
                                this.得分 = QcEvaConst.配置错误分数;
                            }
                            else
                            {
                                this.得分 = df2;
                            }
                        }
                        else
                        {
                            this.得分 = QcEvaConst.符合分数;
                        }
                    }
                    break;
                case "A?B?C?D?":
                    if (描述类型 == "")
                    {
                        this.得分 = QcEvaConst.未检查分数;
                    }
                    else
                    {
                        if (this.限差值 == "")
                        {
                            this.得分 = QcEvaConst.总分数  - QcEvaConst.A类错误扣分 * this.A类错误 - QcEvaConst.B类错误扣分 * this.B类错误
                                - QcEvaConst.C类错误扣分 * this.C类错误 - QcEvaConst.D类错误扣分 * this.D类错误;
                        }
                        else
                        {
                            double a=0,b=0,c=0,d=0;
                            if(double.TryParse(Para["A"],out a)==false
                                ||double.TryParse(Para["B"],out b)==false
                                ||double.TryParse(Para["C"],out c)==false
                                ||double.TryParse(Para["D"],out d)==false)
                            {
                                this.得分=QcEvaConst.配置错误分数;
                                return ;
                            }

                            this.得分 = QcEvaConst.总分数 - a * this.A类错误 - b * this.B类错误 - c * this.C类错误 - d * this.D类错误;
                        }
                    }
                    break;

            }
        }


        private void CalScoreRR0()
        {
            if (ReCode["备注"]=="未检查")
            {
                this.得分 = QcEvaConst.未检查分数;
            }
            else
            {
                this.得分 = QcEvaCalcHelper.CalScoreRR0(参数, A类错误, B类错误, C类错误, D类错误, Para);
            }
        }

        private void CalScoreMM0()
        {
            this.得分 = QcEvaCalcHelper.CalScoreMM0(参数, Para);
        }

    }

}
