using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace QcPublic
{
    public class QcEvaDataRow:DynamicDataRowObject
    {
       static Random r = new Random(DateTime.Now.Millisecond);
        public QcEvaDataRow(DataRow row, QcCheckEntry checkentry)
            : base(row)
        {
            this.checkentry=checkentry;
            if (this.checkentry != null)
                检查项编码 = checkentry.Code;
        }
        public QcEvaDataRow(DataRow row, string checkentry)
            : base(row)
        {
            检查项编码 = checkentry;
 
        }
        public string 数据ID { get { return this["数据id"]; } }
        public string 规则ID { get { return this["规则id"]; } }
        public string 数据名称 { get { return this["数据名称"]; } }
        public string 检查项编码 { get; set; }
        public string 错误id { get { return this["错误id"]; } }

        public string 错漏数量 { get {
            double v = 0;
            if (double.TryParse(this["错漏数量"], out v))
            {
                return v.ToString();
            }
            return this["错漏数量"];
        } }
        public string 参数 { get { return this["参数"]; } }
        public string 检查对象 { get { return this["检查对象"]; } }
        public string 错误描述 { get { return this["错误描述"]; } }
        public string 错漏类别 { get { return this["错漏类别"]; } }
        public string 描述类型 { get { return this["描述类型"]; } }
        public string 是否确认 { get { return this["是否确认"]; } }
        public string 质量元素编码 { get { return checkentry.Parent.Parent.Code; } }
        public string 质量子元素编码 { get { return checkentry.Parent.Code; } }
        public QcCheckEntry checkentry = null;
        public static IEnumerable<QcEvaDataRow> GetErrors(string jobcode)
        {
            string errorsql = "select cd.数据id,cr.检查项编码,ec.错误id,ec.错漏数量,ec.是否确认,ec.检查对象,ec.错误描述,ec.错漏类别,ec.描述类型,ec.参数,ec.规则id " +
                    " from qc_ins_erorecode ec, Qc_Pro_Checkdata cd, Qc_Ins_Checkrule cr,qc_pro_job j " +
            " where j.方案id=cr.方案id  and cd.作业编号=j.作业编号 and ec.数据id=cd.数据id and ec.规则id=cr.规则id and  j.作业编号='" + jobcode + "' order by ec.错误id  ";
            var errors = DbHelper.Query(errorsql).Select(t => new QcEvaDataRow(t, t["检查项编码"].ToString()));
            return errors;
        }

        public static IEnumerable<QcEvaData> GetEvaData(QcJob job)
        {
           // QcProductLevel level = QcProductLevel.QcProductLevels.FirstOrDefault(t => t.Code == job["产品级别编码"]);
            QcCheckProject prj = QcCheckProject.GetCheckProjectByid(job["方案ID"]);

            
            if (prj != null)
            {
                QcProductType type = QcProductType.GetProductTypeById(prj["产品类别编码"]);

                Dictionary<string, QcCheckEntry> lst = new Dictionary<string, QcCheckEntry>();
                type.Nodes.ForEach(t => t.Nodes.ForEach(tt => tt.Nodes.ForEach(ttt=> lst.Add(ttt.Code,ttt))));
               /* var rows = DbHelper.Query("select distinct d.数据名称,j.作业编号,d.数据id,r.检查项编码,e.错误id,e.错漏数量,e.检查对象,e.错误描述,e.错漏类别 from qc_pro_checkdata d,qc_pro_job j,qc_ins_checkproject p,qc_ins_checkrule r,qc_ins_erorecode e " +
                 "where j.作业编号=d.作业编号  and e.规则id=r.规则id  and e.数据id=d.数据id and e.错漏类别!='Undefined' and j.作业编号='" + job.Code + "'");
                * */
                /*var rows = DbHelper.Query("select  dd.数据名称,dd.作业编号,dd.数据id,dd.检查项编码,e.错误id,e.错漏数量,e.检查对象,e.错误描述,e.错漏类别  " +
                " from  (select distinct d.数据名称,j.作业编号,d.数据id,r.检查项编码 " +
                " from qc_pro_checkdata d,qc_pro_job j,qc_ins_checkrule r " +
                "where j.作业编号=d.作业编号 and  r.方案id=j.方案id    and " +
                "j.作业编号 = '" + job.Code + "') dd  left join qc_ins_erorecode e on e.数据id=dd.数据id");
                 * */
                string sql = "select distinct d.数据名称,d.作业编号,d.数据id,ce.检查项编码,ce.结果值枚举 " +
 " from qc_pro_checkdata d,qc_pro_job j,qc_pro_producttype pt,qc_eva_quaelement qe,qc_eva_subquaelement sqe,qc_eva_checkentry ce " +
  " where   j.作业编号=d.作业编号 and  j.产品类别编码=pt.产品类别编码 and pt.产品类别编码=qe.产品类别编码 and qe.质量元素编码=sqe.质量元素编码 and sqe.质量子元素编码=ce.质量子元素编码 and " +
    " j.作业编号 like '"+job.Code+ "' order by d.数据名称  ";
#if GenErrorCode
                string errorsql = "select cd.数据id,cr.检查项编码,ec.错误id,ec.错漏数量,ec.检查对象,ec.错误描述,ec.错漏类别,ec.描述类型,ec.参数 " +
                    " from qc_ins_erorecode ec, Qc_Pro_Checkdata cd, Qc_Ins_Checkrule cr,qc_pro_job j " +
            " where j.方案id=cr.方案id  and cd.作业编号=j.作业编号 and ec.数据id=cd.数据id and ec.规则id=cr.规则id and j.作业编号='" + job.Code +"'";
                var errors = DbHelper.Query(errorsql);
                var errorgroups = errors.GroupBy(t => t["数据id"].ToString());
                string crsql = "select * from  Qc_Pro_Job j,Qc_Ins_Checkproject cp,qc_ins_checkrule cr where  j.方案id=cp.方案id and cp.方案id=cr.方案id  and  j.作业编号='"+job.Code+ "'";
                var crs = DbHelper.Query(crsql).Select(t => new { ID = t["规则ID"], Code = t["检查项编码"] });
#endif
                var rows = DbHelper.Query(sql);
                if (rows == null) return null;
                

                var datas=rows.Select(t => new QcEvaDataRow(t,lst[t.Field<string>("检查项编码")]));
                List<QcEvaData> data = new List<QcEvaData>();


                foreach (var ds in datas.GroupBy(dst=>dst.数据ID))
                {
                    #if GenErrorCode
                    var errorgroup = errorgroups.FirstOrDefault(t => t.Key == ds.First().数据ID);
#endif
                    string recodesql = "select * from qc_eva_checkrecode t where 数据ID='" + ds.First().数据ID + "'";
                    var recoderows = DbHelper.Query(recodesql);
                    IEnumerable<QcEvaCheckReCode> recodes = null;
                    if(recoderows!=null)
                        recodes=recoderows.Select(t => new QcEvaCheckReCode(t));
                    QcEvaData evadata = new QcEvaData(ds.First());
                    data.Add(evadata);
                    foreach (var qds in ds.GroupBy(qdst => qdst.质量元素编码))
                    {
                        QcEvaQaData evaqadata = new QcEvaQaData(qds.First().checkentry.Parent.Parent);
                        evadata.Add(evaqadata);
                        foreach (var sqds in qds.GroupBy(sqdst => sqdst.质量子元素编码))
                        {
                            QcEvaSubQaData subqadata = new QcEvaSubQaData(sqds.First().checkentry.Parent);
                            evaqadata.Add(subqadata);
                            foreach (var ceds  in sqds.GroupBy(cedst => cedst.检查项编码))
                            {
                                QcEvaCheckReCode recode = null;
                                if (recodes != null)
                                    recode = recodes.FirstOrDefault(t => t["检查项编码"] == ceds.First().检查项编码);
                                QcEvaCheckData checkdata = new QcEvaCheckData(ceds.First(),recode);
                                subqadata.Add(checkdata);
#if GenErrorCode
                                if (errorgroup != null)
                                {
                                    //foreach (var dr in errorgroup.Where(
                                     //   t => (t["数据id"].ToString() == ceds.First().数据ID) && (t["检查项编码"].ToString() == ceds.First().检查项编码)))
                                   // {
                                   //     checkdata.Add(new QcEvaDataRow(dr, ceds.First().checkentry));
                                  // }
                                  
                                }
                                else
                                {
                                    
                                    var dr=ceds.First();
                                    
                                    int count = r.Next() % 4 + 65;
                                    char change = (char)count;
                                    int n = 1;
                                    var checkentry = dr.checkentry;
                                    //"Y/N", "R:R0", "M;M0", "A?B?C?D?"
                                    switch (checkentry["结果值枚举"])
                                    {
                                        case "M;M0":
                                            n = 1;
                                            break;
                                        default:
                                            n = r.Next() % 6 + 5;
                                            break;
                                    }
                                    for (int i = 0; i < n; i++)
                                    {                                        
                                        QcErrorCode er = new QcErrorCode();
                                        switch (checkentry["结果值枚举"])
                                        {
                                            case "M;M0":
                                                er["参数"] = "M=5";
                                                break;
                                            case "R:R0":
                                                er["参数"] ="N="+(2000+ r.Next()%13000).ToString();
                                                break;
                                            default:
                                                n = r.Next() % 6 + 5;
                                                break;
                                        }
                                        er["检查对象"] = "11111";
                                        er["错误描述"] = "AAA";
                                        er["数据ID"] = dr.数据ID;
                                        er["作业编号"] = dr["作业编号"];
                                        er["描述类型"] = "1";
                                        er["是否确认"] = "1";
                                        er["错漏数量"] = "1";
                                        er["错漏类别"] =change.ToString ();
                                        er["规则ID"] = crs.Where(t=>t.Code.ToString()==dr.检查项编码).First().ID.ToString();
                                        er.Update();
                                    }
                                }
#endif
                            }
                        }
                    }
                    evadata.Calc();
                }
                return data;
            }
            return null;
        }
    }
}
