using System;
using System.Globalization;
using VotingSystem.Client.Components.NotificationPopup.Models;
using Xamarin.Forms;

namespace VotingSystem.Client.Components.NotificationPopup.Converters
{
    public class NotificationTypeToAnimationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (NotificationType) value;
            switch (type)
            {
                case NotificationType.Success:
                    return "Assets.Animations.SuccessAnimation.json";
                case NotificationType.Error:
                    return "Assets.Animations.ErrorAnimation.json";
                case NotificationType.Warning:
                    return "Assets.Animations.WarningAnimation.json";
                default:
                    return "Assets.Animations.SuccessAnimation.json";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}