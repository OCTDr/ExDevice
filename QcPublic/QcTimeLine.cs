using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QcPublic
{
  static  public class QcProjectTimeLine
    {
      static public bool WriteApointment(string prjid, string log, string taskid = "", string remark = "")
       {
           return DbHelper.Execute(makesql(prjid, log, taskid, remark));
       }
       static string makesql(string prjid, string log, string taskid = "", string remark = "")
       {
          string columers="";
           if (DbHelper.DBType == DatabaseType.Mdb)
           {
               columers = " ([时间],[项目编号],[事件],[任务编号],[备注]) ";
           }
           else
           {
               columers=" (时间,项目编号,事件,任务编号,备注) ";
           }
           StringBuilder updatesql = new StringBuilder(200);
            updatesql.Append("Insert Into ");
            updatesql.Append("qc_pro_timelineproject");
            updatesql.Append(columers);
            string values = "";
            if (taskid == "")
            {
                taskid = "null";
            }
            else
            {
                taskid = "\'" + taskid + "\'";
            }
            if (remark == "")
            {
                remark = "null";
            }
            else
            {
                remark = "\'" + remark + "\'";
            }
            string datetime = "";
           if (DbHelper.DBType == DatabaseType.Oracle)
           {
               datetime = "to_date('" + QcDate.DateString()+ "','yyyy-mm-dd hh24:mi:ss')";
           }
           else if (DbHelper.DBType == DatabaseType.Mdb || DbHelper.DBType == DatabaseType.Accdb)
           {
               datetime = "#" + DateTime.Now.ToLongTimeString() + "#";
           }
           else
           {
               datetime = "\'" + DateTime.Now.ToLongTimeString() + "\'";
           }
           values = string.Format("({0},'{1}','{2}',{3},{4})",datetime ,prjid ,log ,taskid ,remark);
           updatesql.Append(" Values ");
           updatesql.Append(values);
           return updatesql.ToString();
       }
    }

  static public class QcTaskTimeLine
   {
      static public bool WriteApointment(string taskid, string log, string jobid = "", string remark = "")
       {
           return DbHelper.Execute(makesql(taskid, log, jobid, remark));
       }
       static string makesql(string taskid, string log, string jobid = "", string remark = "")
       {
           string columers = "";
           if (DbHelper.DBType == DatabaseType.Mdb)
           {
               columers = " ([时间],[任务编号],[事件],[作业编号],[备注]) ";
           }
           else
           {
               columers = " (时间,任务编号,事件,作业编号,备注) ";
           }
           StringBuilder updatesql = new StringBuilder(200);
           updatesql.Append("Insert Into ");
           updatesql.Append("qc_pro_timelinetask");
           updatesql.Append(columers);
           string values = "";
           if (jobid == "")
           {
               jobid = "null";
           }
           else
           {
               jobid = "\'" + jobid + "\'";
           }
           if (remark == "")
           {
               remark = "null";
           }
           else
           {
               remark = "\'" + remark + "\'";
           }
           string datetime = "";
           if (DbHelper.DBType == DatabaseType.Oracle)
           {
               datetime = "to_date('" + QcDate.DateString() + "','yyyy-mm-dd hh24:mi:ss')";
           }
           else if (DbHelper.DBType == DatabaseType.Mdb || DbHelper.DBType == DatabaseType.Accdb)
           {
               datetime = "#" + DateTime.Now.ToLongTimeString() + "#";
           }
           else
           {
               datetime = "\'" + DateTime.Now.ToLongTimeString() + "\'";
           }
           values = string.Format("({0},'{1}','{2}',{3},{4})", datetime, taskid, log, jobid, remark);
           updatesql.Append(" Values ");
           updatesql.Append(values);
           return updatesql.ToString();
       }
   }

  static public class QcJobTimeLine
  {
      static public bool WriteApointment(string jobid, string log, string remark = "")
      {
          return DbHelper.Execute(makesql(jobid, log,  remark));
      }
      static string makesql(string jobid, string log, string remark = "")
      {
          string columers = "";
          if (DbHelper.DBType == DatabaseType.Mdb)
          {
              columers = " ([时间],[作业编号],[事件],[备注]) ";
          }
          else
          {
              columers = " (时间,作业编号,事件,备注) ";
          }
          StringBuilder updatesql = new StringBuilder(200);
          updatesql.Append("Insert Into ");
          updatesql.Append("qc_pro_timelinejob");
          updatesql.Append(columers);
          string values = "";
             if (remark == "")
          {
              remark = "null";
          }
          else
          {
              remark = "\'" + remark + "\'";
          }
          string datetime = "";
          if (DbHelper.DBType == DatabaseType.Oracle)
          {
              datetime = "to_date('" + QcDate.DateString() + "','yyyy-mm-dd hh24:mi:ss')";
          }
          else if (DbHelper.DBType == DatabaseType.Mdb || DbHelper.DBType == DatabaseType.Accdb)
          {
              datetime = "#" + DateTime.Now.ToLongTimeString() + "#";
          }
          else
          {
              datetime = "\'" + DateTime.Now.ToLongTimeString() + "\'";
          }
          values = string.Format("({0},'{1}','{2}',{3})", datetime, jobid, log, remark);
          updatesql.Append(" Values ");
          updatesql.Append(values);
          return updatesql.ToString();
      }
  }
}
