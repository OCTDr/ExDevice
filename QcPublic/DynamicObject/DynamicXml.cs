////////////////////////////////////
//   使用动态类封装的xml管理类，未完成
//  abao++
///////////////////////////////////
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Dynamic;
namespace QcPublic
{
    /// <summary>
    /// 功能待根据需求完善
    /// </summary>

    public class DynamicXNode : DynamicObject
    {
        
        private XElement _XElement;
        public DynamicXNode(string name)
        {
            _XElement = new XElement(name);
        }
        public DynamicXNode(XElement node)
        {
            _XElement = node;
        }
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var node = _XElement.Element(binder.Name);
            if (node != null)
            {
                result = new DynamicXNode(node);
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var node = _XElement.Element(binder.Name);
            if (node != null)
            {
                node.SetValue(value);
            }
            else
            {
                if (value is DynamicXNode)
                {
                    _XElement.Add(new XElement(binder.Name));
                }
                else
                {
                    _XElement.Add(new XElement(binder.Name,value.ToString()));
                }
            }
            return true;
        }
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if(binder.Type.Equals(typeof(XElement)))
            {
                result= _XElement;
                return true;
            }
            return base.TryConvert(binder, out result);
        }
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var ttype = typeof(XElement);
            try
            {
                result = ttype.InvokeMember(binder.Name, System.Reflection.BindingFlags.InvokeMethod |
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance,
                    null,
                    _XElement,
                    args);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }
    }
    class DynamicXml:DynamicObject//,IEnumerable
    {
        //List<XElement> _elements;
        string FileName { get; set; }
        public DynamicXml(string strxml)
        {
           XDocument xdoc = XDocument.Parse(strxml);
        }
        /*
        public DynamicXml(string strFilename)
        {
            FileName = strFilename;
            XDocument xdoc = XDocument.Load(strFilename);
        }*/
    }
}
