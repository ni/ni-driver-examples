using System;
using System.Globalization;
using System.Windows.Data;
using NationalInstruments.SystemConfiguration;

namespace NationalInstruments.Examples.GetAndSetImage
{
    class NetworkInterfaceSettingsToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((NetworkInterfaceSettings)value)
            {
                case NetworkInterfaceSettings.ResetPrimaryResetOthers:
                    return "Reset Primary, Reset Others";
                case NetworkInterfaceSettings.PreservePrimaryResetOthers:
                    return "Preserve Primary, Reset Others";
                case NetworkInterfaceSettings.PreservePrimaryPreserveOthers:
                    return "Preserve Primary, Preserve Others";
                case NetworkInterfaceSettings.PreservePrimaryApplyOthers:
                    return "Preserve Primary, Apply Others";
                case NetworkInterfaceSettings.ApplyPrimaryResetOthers:
                    return "Apply Primary, Reset Others";
                case NetworkInterfaceSettings.ApplyPrimaryPreserveOthers:
                    return "Apply Primary, Preserve Others";
                case NetworkInterfaceSettings.ApplyPrimaryApplyOthers:
                    return "Apply Primary, Apply Others";
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
