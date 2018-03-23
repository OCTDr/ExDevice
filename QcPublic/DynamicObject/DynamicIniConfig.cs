////////////////////////////////////
//   使用动态类封装的ini数据管理类
//  abao++
///////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
namespace QcPublic
{
    /// <summary>
    ///  ini的一个区域
    /// </summary>
    public class IniField : DynamicObject
    {
        Dictionary<string, string> Data=new Dictionary<string,string>();
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (Data.ContainsKey(binder.Name))
            {
                result = Data[binder.Name];
                return true;
            }
            result = "";
            return true;
        }
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (Data.ContainsKey(binder.Name))
            {
                Data[binder.Name] = value.ToString();
                return true;
            }
            Data.Add(binder.Name, value.ToString());
            return true;
        }
        public bool ContainsKey(string key)
        {
            return Data.ContainsKey(key);
        }
        /// <summary>
        ///  添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, string value)
        {
            Data.Add(key, value);
        }
        /// <summary>
        ///  保存到我文件
        /// </summary>
        /// <param name="sb"></param>
        public void SaveTo(StringBuilder sb)
        {            
            foreach (var v in Data)
            {
                if (v.Key[0] != '#')
                {
                    sb.Append(v.Key);
                    sb.Append('=');
                }
                sb.Append(v.Value);
                sb.Append("\r\n");
            }
        }
        /// <summary>
        ///  键值索引访问
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                if (Data.ContainsKey(key))
                {
                    return Data[key];
                }
                return null;
            }
            set
            {
                if (Data.ContainsKey(key))
                {
                    Data[key] = value;
                }
                else Data.Add(key, value);
            }
        }
    }
    /// <summary>
    ///  动态的ini访问类，可以使用区段名称和键值名称作为变量名访问
    /// </summary>
    public class DynamicIniConfig :DynamicObject        
    {
        Dictionary<string, IniField> Data = null;
        private string filename;
        Encoding _encoding = null;
        /// <summary>
        ///  从文件创建ini，如果文件不存在会自动创建,读取时目前只识别 # 和;行的备注，识别空行并可以保存时保留
        /// </summary>
        /// <param name="inifilename"></param>
        /// <param name="encoding"></param>
        public DynamicIniConfig(string inifilename,Encoding encoding=null)
        {
            if (System.IO.File.Exists(inifilename) == false)
            {
                System.IO.File.AppendAllText(inifilename, "");
            }
            if (encoding == null) encoding = System.Text.Encoding.Default;
            _encoding = encoding;
            var filedata = System.IO.File.ReadAllLines(inifilename,_encoding);
            string field = "";
            filename = inifilename;
            Data = new Dictionary<string,IniField>();
            int line = 0;
            foreach (var v in filedata)
            {
                if (v != "")
                {
                    if (v[0] == '[')
                    {
                        ///区块名称
                        field = v.Substring(1, v.Length - 2);
                        Data.Add(field, new IniField());
                    }
                    else if (v[0] == '#' || v[0] == ';')
                    {
                        //备注
                        if (field == "")
                        {
                            field = "#";
                            Data.Add("#", new IniField());
                        }
                        Data[field].Add("#" + line, v);
                    }
                    else
                    {
                        if (field == "")
                        {
                            field = "#";
                            Data.Add("#", new IniField());
                        }
                        if (field == "#")
                        {
                            Data[field].Add("#" + line, v);//区块名称没有申明的时候就写了键值对，就当是空白吧
                        }
                        else
                        {
                            //键值对处理
                            int pos = v.IndexOf('=');
                            if (pos > 0)
                            {
                                string key = v.Substring(0, pos);
                                string value = v.Substring(pos + 1, v.Length - pos - 1);

                                if (Data[field].ContainsKey(key) == false)
                                {
                                    Data[field].Add(key, value);
                                }
                            }
                        }

                    }
                }
                else
                {
                    if (field == "")
                    {
                        field = "#";
                        Data.Add("#", new IniField());
                    }
                    Data[field].Add("#" + line, v);//空行也保存起来
                }
                line++;
            }
        }
        public void add(string key,IniField NewIniField)
        {
            if(this .Data ==null)
            {
                this .Data = new Dictionary<string,IniField>();
            }
            this.Data.Add(key, NewIniField);
         }
        public IniField this  [string key]
        {
            get
            {
                if (Data.ContainsKey(key))
                {
                    return Data[key];
                }
                return null;
            }
            set
            {
                if (Data.ContainsKey(key))
                {
                    Data[key] = value;
                }
                Data.Add(key, value);
            }
        }
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {            
            if (Data.ContainsKey(binder.Name))
            {
                result = Data[binder.Name];
                    return true;
            }
            Data.Add(binder.Name, new IniField());
            result=Data[binder.Name];
            return true;
        }
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (Data.ContainsKey(binder.Name))
            {
                 Data[binder.Name]=value as IniField;
                return true;
            }
            Data.Add(binder.Name, value as IniField);
            return true;
        }
        public string GetValue(string field,string key)
        {
            if (Data == null) return "";
            if (Data.ContainsKey(field))
            {
                if (Data[field].ContainsKey(key))
                {
                    return Data[field][key];
                }
            }
            return "";
        }
        public void SetValue(string field, string key, string value)
        {
            if (Data.ContainsKey(field) == false)
            {
                Data.Add(field, new IniField());
            }
            if (Data[field].ContainsKey(key) == false)
            {
                Data[field].Add(key, value);
            }
            else
                Data[field][key] = value;
        }
        public void SaveToFile(string filename=null)
        {
            if (filename == null) filename = this.filename;
            StringBuilder sb = new StringBuilder();
            foreach (var v in Data)
            {
                if (v.Key != "#")
                {
                    sb.Append("[");
                    sb.Append(v.Key);
                    sb.Append("]\r\n");
                }
                v.Value.SaveTo(sb);
            }
            System.IO.File.WriteAllText(filename, sb.ToString(),_encoding);
        }
        public static string GetPrivateProfileString(string lpSectionName, string lpKeyName, string lpDefault, string lpFileName)
        {
            DynamicIniConfig ini = new DynamicIniConfig(lpFileName);
            string ret = ini.GetValue(lpSectionName, lpKeyName);
            if (ret == "") return lpDefault;
            return ret;
        }
        public static void SetPrivateProfileString(string lpSectionName, string lpKeyName, string lpValue, string lpFileName)
        {
            DynamicIniConfig ini = new DynamicIniConfig(lpFileName);
            ini.SetValue(lpSectionName, lpKeyName, lpValue);
            ini.SaveToFile(lpFileName);
        }
    }
}
