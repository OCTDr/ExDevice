using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QcPublic
{
    public class QcNameConverter : QcTypeConverter
    {
        public QcNameConverter(IEnumerable<string> lst):base(lst)
        {
            
        }
        public QcNameConverter(Func<IEnumerable<string>> listor)
            : base(listor)
        {
        }
        public QcNameConverter():base(()=>null)
        {
        }
        public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            var name = QcUser.GetName(value.ToString());
            return name;
            //return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            //return base.ConvertFrom(context, culture, value);
            return QcUser.GetUserID(value.ToString());
        }
    }
}
