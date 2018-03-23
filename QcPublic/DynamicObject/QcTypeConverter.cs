using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace QcPublic
{
    public class QcTypeConverter:QcTypeConverter<string>
    {
        public QcTypeConverter(IEnumerable<string> lst)
            : base(lst, t => t)
        {
        }
        public QcTypeConverter(Func<IEnumerable<string>> listor)
            : base(null, t => t,listor)
        {
        }
    }
    public class QcTypeConverter<T> : QcTypeConverter<T, string> 
    {

        public QcTypeConverter(IEnumerable<T> lst, Func<T, string> seletor, Func<IEnumerable<T>> listor = null) :
            base(lst, seletor, listor) { }

    }
    public class QcTypeConverter<T,U> : TypeConverter
    {
        IEnumerable<T> lst = null;
        Func<T,U> seletor=null;
        Func<IEnumerable<T>> listor = null;
        public QcTypeConverter(IEnumerable<T> lst,Func<T,U> seletor,Func<IEnumerable<T>> listor=null)
         {
            this.lst=lst;
            this.seletor=seletor;
            this.listor = listor;
         }
    
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return value;
        }


        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            try
            {
                if (listor != null) lst = listor();
                var p = from t in lst select seletor(t);
                return new StandardValuesCollection(p.ToArray());
            }
            catch
            {
                return null;
            }
        }
    }
}
