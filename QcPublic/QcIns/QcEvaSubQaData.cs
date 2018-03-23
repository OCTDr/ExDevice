using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QcPublic
{
    public class QcEvaSubQaData:QcEvaDataEntry<QcEvaCheckData>
    {
        QcSubQuaelement Sqe = null;
        public QcEvaSubQaData(QcSubQuaelement sqe):base(sqe.Name)
        {
            this.Sqe = sqe;
            double.TryParse(sqe["权值"], out 权值);
            this.计分方式 = sqe["计分方式"];
            this.分组 = sqe["分组"];
        }

        public string 质量子元素名称 { get { return Sqe.Name; } }
        public void Calc()
        {
            lst.ForEach(t => t.Calc());
           this.得分= QcEvaCalcHelper.Calc(this, lst);
        }

        public void statistics(IEnumerable<QcEvaDataRow> errors = null)
        {
            lst.ForEach(t => t.Statistics(errors));
        }
    }
}
