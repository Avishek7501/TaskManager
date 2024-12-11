using System;
using Xamarin.Forms;

namespace TaskManager.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var status = value as string;

           
            switch (status)
            {
                case "Pending":
                    return Color.Orange;
                case "In Progress":
                    return Color.Blue;
                case "Completed":
                    return Color.Green;
                default:
                    return Color.Gray;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
