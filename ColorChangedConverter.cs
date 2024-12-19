using System.Globalization;

namespace PCCE
{
    public class ColorChangedConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null) return Colors.Grey;
            if ((byte)value == 1) // camper
            {
                return Colors.Red;
            }
            else if ((byte)value == 2) // ground
            {
                return Colors.Orange;
            }
            else if ((byte)value == 3) // player
            {
                return Colors.Yellow;
            }
            else if ((byte)value == 4) // animal
            {
                return Colors.Green;
            }
            else if ((byte)value == 5) // cabin
            {
                return Colors.Blue;
            }
            else if ((byte)value == 6) 
            {
                return Colors.Purple;
            }
            else
            {
                return Colors.Gray;
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
