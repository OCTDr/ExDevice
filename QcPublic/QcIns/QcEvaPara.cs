using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QcPublic
{
    class QcEvaPara
    {
        Dictionary<string, string> Paras = new Dictionary<string, string>();
        public QcEvaPara(string Para)
        {
            if (Para == "") return;
            var ps = Para.Split(';');
            int count = 0;
            foreach (var p in ps)
            {
                string name = count.ToString();
                string value = p;
                if (p.IndexOf('=') > 0)
                {
                    var tp = p.Split('=');
                    name = tp[0];
                    value = tp[1];
                }
                count++;
                Paras.Add(name, value);                    
            }
        }
       public  string GetKey(int index)
        {
            if (index >= 0 && index < this.Paras.Count) return Paras.Keys.ElementAt(index);
            return "";
        }
       public int Count
       {
           get
           {
               return Paras.Count;
           }
       }
       public string this[int index]
        {
            get
            {
                if (index >= 0 && index < this.Paras.Count) return Paras.Values.ElementAt(index);
                return "";
            }

        }
       public string this[string index]
        {
            get
            {
                if (Paras.ContainsKey(index))
                    return Paras[index];
                return "";

            }
        }
    }
}
