/////////////////////////
//  动态类型包装类，包装一个类型描述器，可以供PropertyGrid显示和管理信息
// abao++
////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.ComponentModel;
using System.Data;
using DevComponents.DotNetBar;
namespace QcPublic
{
 
    /// <summary>
    ///  动态数据类描述器，可以包装工PropertyGrid显示
    /// </summary>
    public class DynamicTypeDescriptor:ICustomTypeDescriptor
    {

        private DataRow row { get { return RowObject.GetRow(); } }

        private Dictionary<string, bool> propertyBrowsable = new Dictionary<string, bool>();
        private List<string> unBrowsableCategorys = new List<string>();
        public List<string> UnBrowsableCategoryNames
        {
            get { return unBrowsableCategorys; }
        }

        public bool this[string PropertyName]
        {
            set
            {
                propertyBrowsable[PropertyName] = value;
            }
        }
        /// <summary>
        ///  数据行对象
        /// </summary>
        public readonly DynamicDataRowObject RowObject;
        /// <summary>
        ///  只读字段名称，特殊的提供 "" 空字符串将会让所有字段只读，!后面跟字段名称含义为不包含字符串为只读 = 则要求字段名称和其后完全相等才是只读
        /// </summary>
        protected  IEnumerable<string> _readonlyfields = null;
        /// <summary>
        ///  分类列表，特殊的 ""空字符串会匹配所有的字段，放在字典列表最后可以把任意字段归类
        /// </summary>
        protected IDictionary<string, string> _diccatetory = null;
        /// <summary>
        ///  转换器，主要用于为字段提供特殊的转换器，一般是为日期字段提供dbnull支持，为选择输入字段提供输入选项
        /// </summary>
        protected IDictionary<string, TypeConverter> _dicconverter=null;
        /// <summary>
        /// 默认只读属性
        /// </summary>
        private static string[] _defaultreadonly = { "编码", "创建","编号" ,"ID"};
        /// <summary>
        /// 默认的属性分类
        /// </summary>
        private static Dictionary<string, string> _defaultdiccatedory
            = new Dictionary<string, string>(){
            { "编码", "系统属性" }
            ,{ "编号", "系统属性" }
            , { "创建", "自动属性" } 
             , { "ID", "自动属性" } 
            ,{"备注","杂项"}
            ,{"","基本属性"}
            };
        /// <summary>
        ///  默认的计分方式选择
        /// </summary>
        private static Dictionary<string, TypeConverter> _defaultconverter
            = new Dictionary<string, TypeConverter>()
            {
                {"计分方式",new 计分方式Converter()}
                 ,{"结果值枚举",new 结果值枚举Converter()}
                 ,{"创建人",new QcNameConverter()}
                ,{"修改人",new QcNameConverter()}
                            };
        /// <summary>
        ///  构建一个动态数据类描述器
        /// </summary>
        /// <param name="DynamicObject">动态数据行对象</param>
        /// <param name="readonlyfields">只读字段集合</param>
        /// <param name="diccatetory">分类集合</param>
        /// <param name="dicconverter">转换器集合</param>
        public DynamicTypeDescriptor(DynamicDataRowObject DynamicObject,
            IEnumerable<string> readonlyfields=null
            ,IDictionary<string,string> diccatetory=null
            ,IDictionary<string,TypeConverter> dicconverter=null
            )
        {
            
            RowObject =DynamicObject;
        

            if (readonlyfields == null)
                this._readonlyfields = DynamicTypeDescriptor._defaultreadonly;
            else
                this._readonlyfields = readonlyfields;
            if (diccatetory == null)
                this._diccatetory = DynamicTypeDescriptor._defaultdiccatedory;
            else
                this._diccatetory = diccatetory;
            if (dicconverter == null)
                this._dicconverter = DynamicTypeDescriptor._defaultconverter;
            else
                this._dicconverter = dicconverter;

            //可见性初始化
            foreach (DataColumn c in row.Table.Columns)
            {
                if (!propertyBrowsable.Keys.Contains(c.ColumnName))
                {
                    propertyBrowsable.Add(c.ColumnName, true);
                }
            }
           
        }
        /// <summary>
        /// 设置为只读的
        /// </summary>
        /// <returns></returns>
        public DynamicTypeDescriptor SetReadOnly()
        {
             this._readonlyfields =new [] { "" };
             return this;
        }
        // Just use the default behavior from TypeDescriptor for most of these
        // This might need some tweaking to work correctly for ExpandoObjects though...6

        /// <summary>
        /// 获取组件名称
        /// </summary>
        /// <returns></returns>
        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return row;
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        // This is where the GetProperties() calls are
        // Ignore the Attribute for now, if it's needed support will have to be implemented
        // Should be enough for simple usage...

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(new Attribute[0]);
        }
        /// <summary>
        ///  主要的属性包装过程
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            // This just casts the ExpandoObject to an IDictionary<string, object> to get the keys
            /*
            return new PropertyDescriptorCollection(
                ((IDictionary<string, object>)_expando).Keys
                .Select(x => new ExpandoPropertyDescriptor(((IDictionary<string, object>)_expando), x))
                .ToArray());
             * */
            List<PropertyDescriptor> lstper = new List<PropertyDescriptor>();
            ///遍历所有的数据列
            foreach (DataColumn c in row.Table.Columns)
            {
                TypeConverter convert=null;
                _dicconverter.TryGetValue(c.ColumnName, out convert);
                bool readonlyflag = false;
                
                List<Attribute> attributes2 = new List<Attribute>();

                //只读数据行采用了特殊的几个语句
                foreach(var v in _readonlyfields)
                { 
                    if(v.Length>0)
                    {
                        if(v[0]=='!')
                        {
                            if(c.ColumnName.Contains(v.Substring(1))==false)
                            {
                                readonlyflag = true;
                                break;
                            }
                        }
                        else if(v[0]=='=')
                        {
                            if(v.Substring(1)==c.ColumnName)
                            { readonlyflag = true;
                            break;
                            }
                        }
                        else if (c.ColumnName.Contains(v))
                        {
                            readonlyflag = true;
                            break;
                        }
                    }
                        else
                        {
                            readonlyflag=true ;
                        }
                }                   
                //识别得出分类名称catetory
                string catetory = "";
                foreach (var v in this._diccatetory)
                {
                    if (c.ColumnName.Contains(v.Key))
                    {
                        catetory = v.Value;
                        break;
                    }
                }
             
               ///添加到属性集合
                if (propertyBrowsable[c.ColumnName] && !unBrowsableCategorys.Contains(catetory))
                {
                    lstper.Add(new DynamicPropertyDescriptor(RowObject,c.ColumnName,
                    readonlyflag, catetory, convert, propertyBrowsable[c.ColumnName], attributes2.ToArray()));
                }
            }


            PropertyDescriptorCollection pc = new PropertyDescriptorCollection(lstper.ToArray());            
            return pc;
        }

        // A nested PropertyDescriptor class that can get and set properties of the
        // ExpandoObject dynamically at run time
       
    }

    /// <summary>
    ///  空日期支持转换器
    ///  把当前日期，且小时为1的日期定义为空
    /// </summary>
    public class NullDateTimeTypeConverter : TypeConverter
    {

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            DateTime d;
            if (DateTime.TryParse(value.ToString(), out d))
                return d;
            else
                return DBNull.Value;
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            //把当前日期，且小时为1的日期定义为空
            if (value is DateTime)
            {
                DateTime d = (DateTime)value;
                DateTime now = DateTime.Now;
                if (d.Year == now.Year && d.Day == now.Day && d.Month == now.Month && d.Hour == 1)
                    return "";
                return d.ToShortDateString();
            }
            else
                return base.ConvertTo(context, culture,value, destinationType);
        }
      

    }
    /// <summary>
    /// 属性描述器
    /// </summary>
    public class DynamicPropertyDescriptor : PropertyDescriptor
    {
         static NullDateTimeTypeConverter DateTimeConverter = new NullDateTimeTypeConverter();
         private DataRow _row { get { return obj.GetRow(); } }
        DynamicDataRowObject obj=null;
        private string _name;
        private bool _readonly;
        private string _catetory;
        private TypeConverter _converter;
        private bool _isBrowsable = true;
        public bool Browsable
        {
            set { _isBrowsable = value; }
        }
   
    
     /// <summary>
        /// 构建一个属性描述其
     /// </summary>
     /// <param name="row"></param>
     /// <param name="name"></param>
     /// <param name="readonlyflag"></param>
     /// <param name="catetory"></param>
     /// <param name="converter"></param>
        public DynamicPropertyDescriptor(DynamicDataRowObject obj, string name
            ,bool readonlyflag=false
            ,string catetory=""
            ,TypeConverter converter=null
            ,bool isBrowsable = false 
            ,Attribute[] atts = null)
            : base(name, atts)
        {

            this.obj = obj;
            _name = name;
            this._readonly = readonlyflag;
            this._catetory = catetory;
            this._converter = converter;
            _isBrowsable = isBrowsable;
            if (this._converter == null) this._converter = base.Converter;
        }
        public override object GetEditor(Type editorBaseType)
        {
            if (_name.Contains("目录")) return new System.Windows.Forms.Design.FolderNameEditor();
            return base.GetEditor(editorBaseType);
        }
         /// <summary>
         ///  类型转换器，特殊的日期类型值为空时会使用日期转换器
         /// </summary>
        public override TypeConverter Converter
        {
            get
            {

                if (_row[_name] is DBNull)
                {
                    if (_row.Table.Columns[_name].DataType == typeof(DateTime))
                    {
                        return DateTimeConverter;
                    }
                }
                return _converter;
            }
        }
         
        /// <summary>
        ///  字段属性类型
        /// </summary>
        public override Type PropertyType
        {
            get
            {
                return _row.Table.Columns[_name].DataType;
                    
            }
        }
        /// <summary>
        ///  设置字段值
        /// </summary>
        /// <param name="component"></param>
        /// <param name="value"></param>
        public override void SetValue(object component, object value)
        {
            _row[_name] = value;
        }

         /// <summary>
         ///  取得字段值
         /// </summary>
         /// <param name="component"></param>
         /// <returns></returns>
        public override object GetValue(object component)
        {
            object value = _row[_name];
            if (value is DBNull)
            {
                if(_row.Table.Columns[_name].DataType  ==typeof(DateTime))
                {
                    //识别是否为空值，提供这个值供日期选择器使用，如果不选择日期，在最后会被转换为空值
                    return DateTime.Now.AddHours(-1*DateTime.Now.Hour+1);
                }
                return "";
            }
            else
                return value;
        }         
         /// <summary>
         ///  是否只读
         /// </summary>
        public override bool IsReadOnly
        {
            get
            {
                return _readonly;

            }
        }


        public override bool IsBrowsable
        {
            get
            {
                return _isBrowsable;
            }
        }

        public override Type ComponentType
        {
            get { return null; }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override string Category
        {
            get
            {
                return _catetory;
            }
        }

        public override string Description
        {
            get { return string.Empty; }
        }
    }
}
