using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QcPublic
{
    public class QcEvaQaData : QcEvaDataEntry<QcEvaSubQaData>
    {
        QcQuaelement Qa = null;
        public QcEvaQaData(QcQuaelement qa):base(qa.Name)
        {
            this.Qa = qa;
            double.TryParse(qa["权值"], out 权值);
            this.计分方式 = Qa["计分方式"];
            this.分组 = qa["分组"];
        }
        public string 质量元素名称 { get { return Qa.Name; } }
        public void Calc()
        {
            lst.ForEach(t => t.Calc());
            this.得分 = QcEvaCalcHelper.Calc(this, lst);
        }
        public void statistics(IEnumerable<QcEvaDataRow> errors = null)
        {
            lst.ForEach(t => t.statistics(errors));
        }
    }
}
