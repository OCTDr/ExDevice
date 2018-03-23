using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace QcPublic
{
   public  class QcDevice:QcNode
    {
        public override string Name
        {
            get
            {
                return this["设备名称"];
            }
            set
            {
                base.Name = value;
            }
        }
        public override string Code
        {
            get
            {
                return this[CodeField];
            }
            set
            {
                this[CodeField] = value;
            }
        }

        public override string CodeField
        {
            get { return "设备UID"; }
        }
        public QcDevice(DataRow row=null)
        :base(row, "QC_PRO_DEVICE")
        {
        }
        public override QcCheckResult Check(string field = null)
        {
            QcCheckResult result = new QcCheckResult(this);
            bool checkall = (field == null);
            result.AddCheckNull(field, new[] { "设备编号", "设备类型", "设备名称", "使用单位"});           
            result.AddCheckEnum(field, "设备类型", QcDeviceDescriptor.设备类型s);
            if (result.Count > 0) return result;
            return null;
        }
        public override bool Update(QcDbTransaction trans = null)
        {
           
            if (IsNew())
            {              
                this.Code = this.GetNextCode();
            }
            return base.Update(trans);
        }
        public static  QcDevice GetDeviceByUID(string uid)
        {
            try
            {
                return GetDevices(string.Format("设备UID='{0}'", uid)).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
        public static IEnumerable <QcDevice> GetDevices(string expr = "", string orderby = "")
        {
            if (expr != "") expr = " where " + expr;
            if (orderby != "") expr = expr + orderby;
            try
            {
                return DbHelper.Query("select * from QC_PRO_DEVICE " + expr).Select(t => new QcDevice(t));
            }
            catch
            {
                return GetDevices();
            }

        }
        public string GetNextCode()
        {
            string prestring = string.Format("{0}{1}{2}", DateTime.Now.Year, DateTime.Now.Month.ToString("00"), DateTime.Now.Day.ToString("00"));
            return QcCode.GetNextNumber(prestring,
               QcDevice.GetDevices(string.Format("设备UID like '%{0}%'", prestring)),
                8, 3, "000"
                );

        }
    }
}
