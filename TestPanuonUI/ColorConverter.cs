using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Data;
using System.Windows.Media;

namespace TestPanuonUI
{
        // 定义转换器
    [ValueConversion(typeof(string), typeof(SolidColorBrush))]
    public class ColorConverter:IValueConverter
    {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if (value == null || value.ToString() == "")
                    return "";

                if (value.ToString() == "不通过")//这里根据你里面的值自己写判断条件
                {
                    try
                    {
                        return new SolidColorBrush(Colors.Red);
                    }
                    catch
                    { throw; }
                }
            if (value.ToString() == "通过")
            {
                try
                {
                    return new SolidColorBrush(Colors.Green);
                }
                catch
                { throw; }
            }
            if (value.ToString() == "正在测试...")
            {
                try
                {
                    return new SolidColorBrush(Colors.Gray);
                }
                catch
                { throw; }
            }

            if (value.ToString() == "待测试")
            {
                try
                {
                    return new SolidColorBrush(Colors.Gray);
                }
                catch
                { throw; }
            }

            return new SolidColorBrush(Colors.Black);
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }
 

    }
}
