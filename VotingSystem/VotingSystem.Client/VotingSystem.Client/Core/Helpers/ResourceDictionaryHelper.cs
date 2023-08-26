using Xamarin.Forms;

namespace VotingSystem.Client.Core.Helpers
{
    public static class ResourceDictionaryHelper
    {
        public static Color GetColor(string key)
        {
            Application.Current.Resources.TryGetValue(key, out var colorPrimary);
            return colorPrimary as Color? ?? Color.Black;
        }
    }
}