using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace TestPanuonUI
{
    public abstract class BindableObject : INotifyPropertyChanged
    {
        private Dictionary<string, object> dict = new Dictionary<string, object>();

        private static string GetProperyName(string methodName)
        {
            if (methodName.StartsWith("get_") || methodName.StartsWith("set_") ||
                methodName.StartsWith("put_"))
            {
                return methodName.Substring("get_".Length);
            }
            throw new Exception(methodName + " not a method of Property");
        }

        protected void SetValue(object value)
        {
            string propertyName = GetProperyName(new StackTrace(true).GetFrame(1).GetMethod().Name);
            dict[propertyName] = value;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected object GetValue()
        {
            string propertyName = GetProperyName(new StackTrace(true).GetFrame(1).GetMethod().Name);
            if (dict.ContainsKey(propertyName))
            {
                return dict[propertyName];
            }
            else
            {
                return null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
