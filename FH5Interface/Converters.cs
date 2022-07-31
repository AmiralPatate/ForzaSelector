using FH5Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;

namespace FH5Interface
{
    public class BooleanToYesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) return "Yes";
            return "No";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return null; }
    }
    public class BooleanToNullNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) return "";
            return "No";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return null; }
    }
    public class BooleanToYesNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) return "Yes";
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return null; }
    }

    public class RowBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = value.GetType();
            CarStats model = (CarStats)type.GetProperty("Stats").GetValue(value, null);
            model.PI.ClassFromPi().Color();

            bool IsDriven = (bool)type.GetProperty("IsDriven").GetValue(value, null);

            if (IsDriven) return model.PI.ClassFromPi().Color().AdjustBrightness(.80);
            else return model.PI.ClassFromPi().Color().AdjustBrightness(.875);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return null; }
    }

    public static class Extensions
    {
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;

                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }

            return e.ToString();
        }

        public static Brush AdjustBrightness(this Brush color, double factor)
        {
            Color originalColour = ((SolidColorBrush)color).Color;
            Color adjustedColour = Color.FromArgb(originalColour.A,
                (byte)((255 - originalColour.R) * factor + originalColour.R),
                (byte)((255 - originalColour.G) * factor + originalColour.G),
                (byte)((255 - originalColour.B) * factor + originalColour.B));
            return new SolidColorBrush(adjustedColour);
        }
    }
}
