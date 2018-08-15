using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PlantafelNAV.Helpers.Converter
{
    /// <summary>
    /// Converter to get color of current Status, bind with ellipse
    /// </summary>
    public class BrushToStatusConverter : IValueConverter
    {
        Brush red = new SolidColorBrush(Colors.Red);
        Brush green = new SolidColorBrush(Colors.Green);
        Brush yellow = new SolidColorBrush(Colors.Yellow);
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String temp = (String)value;
            switch (temp)
            {
                case "Activ":
                    return green;
                case "Delayed":
                    return red;
                case "Paused":
                    return yellow;
                default:
                    return green;

                
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
