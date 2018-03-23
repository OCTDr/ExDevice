using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QcPublic
{
    public class QcEvaConst
    {
        public const double 总分数 = 100;
        public const double 符合分数 = 100;
        public const double 不符合分数 = 0;
        public const double 配置错误分数 = -9998;
        public const double 未检查分数 = -9999;
        public const double A类错误扣分 = 42;
        public const double B类错误扣分 = 12;
        public const double C类错误扣分 = 4;
        public const double D类错误扣分 = 1;
        public const double 普遍问题数量 = 999999;
        public static bool 设置负分为零分 = false;
    }

   class QcEvaCalcHelper
    {
      
        public static double 获取最小值(IGrouping<string, QcEvaDataEntry> lst)//每组的最小分
        {
            if (lst.Key == "不分组"||lst .Key =="0")
            {
                return lst.Min(t => t.得分);
            }
            var tdata = lst.First();
            QcEvaCheckData check = null;

            if (tdata is QcEvaCheckData)//检查项
            {
                check = lst.First() as QcEvaCheckData;
            }
            else if (tdata is QcEvaSubQaData)//质量子元素
            {
                check = (lst.First() as QcEvaSubQaData).Nodes.First();
            }
            else if (tdata is QcEvaQaData )//质量元素 oct修改 2016.1-18
            {
                return lst.Min(t => t.得分);//一般质量元素级别采用的是各个质量元素的最低分直接娶最低分
            }
                            
            double 得分 = 0;
            if (check != null)//非质量元素级别，在质量元素级别不能采用把所有个数加起来，在质量
            {
                if (check.结果值枚举 == "R:R0")//需要把所有的错误个数加起来
                {
                    得分 = CalScoreRR0(check.参数, lst.Sum(t => t.A类错误), lst.Sum(t => t.B类错误), lst.Sum(t => t.C类错误), lst.Sum(t => t.D类错误), new QcEvaPara(check.限差值));
                }
                else
                {
                    得分 = QcEvaConst.总分数 - (QcEvaConst.总分数 * lst.Count() - lst.Sum(t => t.得分));
                }
            }                       
            foreach (var v in lst) v.分组得分 = 得分;
            return 得分;
        }
        public static double 合并计分(IEnumerable<QcEvaDataEntry> lst)//每组的最小分
        {
            var tdata = lst.First();
            QcEvaCheckData check = null;

            if (tdata is QcEvaCheckData)
            {
                check = lst.First() as QcEvaCheckData;
            }
            else if (tdata is QcEvaSubQaData)
            {
                check = (lst.First() as QcEvaSubQaData).Nodes.First();
            }
            else if (tdata is QcEvaQaData)//质量元素 oct修改 2016.1-18
            {
                return QcEvaConst.总分数 - (QcEvaConst.总分数 * lst.Count() - lst.Sum(t => t.得分));//采用各个质量元素的扣分总和
            }
            double 得分 = 0;
            if (check != null)
            {
                if (check.结果值枚举 == "R:R0")
                {
                    得分 = CalScoreRR0(check.参数, lst.Sum(t => t.A类错误), lst.Sum(t => t.B类错误), lst.Sum(t => t.C类错误), lst.Sum(t => t.D类错误), new QcEvaPara(check.限差值));
                }
            }
            得分 = QcEvaConst.总分数 - (QcEvaConst.总分数 * lst.Count() - lst.Sum(t => t.得分));
            return 得分;
        }
        public static double Calc(QcEvaDataEntry data, IEnumerable<QcEvaDataEntry> lst)
        {
            data.A类错误 = 0;
            data.B类错误 = 0;
            data.C类错误 = 0;
            data.D类错误 = 0;
            data.得分 = 0;
            foreach (var t in lst)
            {
                data.A类错误 += t.A类错误;
                data.B类错误 += t.B类错误;
                data.C类错误 += t.C类错误;
                data.D类错误 += t.D类错误;
            }
            var errornode = lst.FirstOrDefault(t => t.得分 == QcEvaConst.配置错误分数 || t.得分 == QcEvaConst.未检查分数);
            if (errornode != null) return errornode.得分;

            var datalst = lst.Where(t => t.计分方式 != "不计分");

            var 计分方式 = datalst.First().计分方式;

            if (计分方式 == "最低分")
            {
                var scores = datalst.GroupBy(t => t.分组).Select(t =>
                    获取最小值(t)
                    );
                return scores.Min();

            }
            if (计分方式 == "带权平均值")
            {           
                
                return datalst.Sum(t => t.得分 * t.权值);
            }
            if (计分方式 == "合并计分")
            {
            //    得分 = CalScoreRR0(check.参数, lst.Sum(t => t.A类错误), lst.Sum(t => t.B类错误), lst.Sum(t => t.C类错误), lst.Sum(t => t.D类错误), new QcEvaPara(check.限差值));
               // return 100 - (100 * datalst.Count() - datalst.Sum(t => t.得分));
                return 合并计分(datalst);
            }
            return 0;
        }

        #region "分数计算"
        private static double CalcScoreRR0(double r, double r0)
        {
            return 60 + 40 / r0 * (r0 - r);
        }
        //public static double CalScoreRR0(string 参数, double A类错误, double B类错误, double C类错误, double D类错误, QcEvaPara Para)
        //{
        //    var errorparas = new QcEvaPara(参数);
        //    double N = -1;
        //    double r = 1;
        //    if (double.TryParse(errorparas[0], out N))
        //    {

        //        double sumra = A类错误;
        //        double sumrb = B类错误;
        //        double sumrc = C类错误;
        //        if (Para.Count == 1)
        //        {
        //            double r0 = 0;
        //            if (double.TryParse(Para[0], out r0))
        //            {
        //                r = (sumra + sumrb + sumrc) / N * 100;
        //                if (r0 > 0)
        //                {
        //                    return CalcScoreRR0(r, r0);
        //                }
        //                else
        //                {
        //                    if (sumra + sumrb + sumrc > 0)
        //                    {
        //                        return QcEvaConst.不符合分数;

        //                    }
        //                    return 100;
        //                }
        //            }
        //            else
        //            {
        //                return QcEvaConst.配置错误分数;
        //            }
        //            //double sumrd=lst.Where(t=>t.错漏类别=="D").Sum(t=>double.Parse(t.错漏数量));  
        //        }
        //        else if (Para.Count == 2)
        //        {
        //            double r01 = 0;
        //            double r02 = 0;

        //            if (double.TryParse(Para[0], out r01) && double.TryParse(Para[1], out r02))
        //            {
        //                if (Para.GetKey(0) == "A+B")
        //                {
        //                    double r1 = (sumra + sumrb) / N * 100;
        //                    double r2 = (sumrc) / N * 100;
        //                    return Math.Min(CalcScoreRR0(r1, r01), CalcScoreRR0(r2, r02));
        //                }
        //                else if (Para.GetKey(0) == "A")
        //                {

        //                    double r1 = (sumra) / N * 100;
        //                    double r2 = (sumrb + sumrc) / N * 100;
        //                    if (r01 > 0)
        //                    {
        //                        return Math.Min(CalcScoreRR0(r1, r01), CalcScoreRR0(r2, r02));
        //                    }
        //                    else
        //                    {
        //                        if (sumra > 0)
        //                        {
        //                            return QcEvaConst.不符合分数;
        //                        }
        //                        else
        //                            return CalcScoreRR0(r2, r02);
        //                    }
        //                }


        //            }
        //            else
        //            {
        //                return QcEvaConst.配置错误分数;
        //            }
        //        }
        //        else if (Para.Count == 3)
        //        {

        //            double r01 = 0;
        //            double r02 = 0;
        //            double r03 = 0;
        //            if (double.TryParse(Para[0], out r01) && double.TryParse(Para[1], out r02) && double.TryParse(Para[2], out r03))
        //            {
        //                double r1 = (sumra) / N * 100;
        //                if (r1 > r01)
        //                {
        //                    return QcEvaConst.不符合分数;

        //                }
        //                double r2 = (sumrb) / N * 100;
        //                double r3 = (sumrc) / N * 100;
        //                return Math.Min(CalcScoreRR0(r2, r02), CalcScoreRR0(r3, r03));
        //            }
        //            else
        //            {
        //                return QcEvaConst.配置错误分数;
        //            }
        //        }
        //    }
        //    return QcEvaConst.未检查分数;
        //}
//        17:05:10
//abao++ 2014/9/23 17:05:10
        public static  double CalScoreRR0(string 参数, double A类错误, double B类错误, double C类错误,double D类错误, QcEvaPara Para)
        {
            var errorparas = new QcEvaPara(参数);
            double N = -1;
            if(Para.Count==0) return QcEvaConst.配置错误分数;
            if (double.TryParse(errorparas[0], out N))
            {

                double sumra = A类错误;
                double sumrb = B类错误;
                double sumrc = C类错误;
                double sumrd = D类错误;
                double Score = 999999;
                for(int i=0;i<Para.Count;i++)
                {
                    double r0 = 0;
                    if (double.TryParse(Para[i], out r0))
                    {
                        var exprs = Para.GetKey(i).Split('+');
                        double sum = 0 ;
                        foreach(var e in exprs)
                        {
                            switch(e)
                            {
                                case "A":
                                    sum += sumra;
                                    break;
                                case "B":
                                    sum += sumrb;
                                    break;
                                case "C":
                                    sum += sumrc;
                                    break;
                                case "D":
                                    sum += sumrd;
                                    break;
                            }
                        }
                        double r = sum / N * 100;
                        if (r0 > 0)
                            Score = Math.Min(Score, CalcScoreRR0(r, r0));
                        else
                        {
                            if (sum > 0) Score = QcEvaConst.不符合分数;
                            else Score = Math.Min(Score, QcEvaConst.总分数);
                        }
                    }
                    else
                    {
                        return QcEvaConst.配置错误分数;
                    }
                }
                return Score;
            }
            else
            {
                return QcEvaConst.配置错误分数;
            }
        }

        public static double CalcScoreMM0(double m, double m0)
        {
            if (m <= 0.3 * m0)
            {
                return 100;
            }
            return 60 + 40 / (0.7 * m0) * (m0 - m);
        }

        public static double CalScoreMM0(string 参数, QcEvaPara Para)
        {

            var paraary = new QcEvaPara(参数);
            double m = -1;
            double m0 = -1;
            if (paraary.Count > 0)
            {
                if (double.TryParse(paraary[0], out m))
                {

                    if (double.TryParse(Para[0], out m0))
                    {
                        if (Para.GetKey(0).ToUpper() == "GB24356")
                        {
                            if (m < m0 / 3 + 0.0000001) return 100;
                            else if (m < m0 + 0.0000001) return 120 - (60 * m / m0);
                            else return QcEvaConst.不符合分数;

                        }
                        else
                        {
                            return CalcScoreMM0(m, m0);

                        }
                    }
                    else
                    {
                        return QcEvaConst.配置错误分数;
                    }


                }
                else
                {
                    return QcEvaConst.配置错误分数;
                }
            }
            else
            {
                return QcEvaConst.未检查分数;
            }
        }

   
    }
        #endregion
}
