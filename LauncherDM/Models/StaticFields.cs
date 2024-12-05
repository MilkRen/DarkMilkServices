using System.Reflection;

namespace LauncherDM.Models
{
    class StaticFields
    {
        public static string VersionPatch => Assembly.GetExecutingAssembly().GetName().Version?.ToString();
    }
}
