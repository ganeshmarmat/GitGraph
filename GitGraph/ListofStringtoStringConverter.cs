using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GitGraph
{
    public class ListofStringtoStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if(value.GetType()==typeof(List<string>))
            {
                var lst = (List<string>)value;
                string returnstr = "";
                foreach (var item in lst)
                {
                    returnstr += " " + item;  
                }
                return returnstr;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
