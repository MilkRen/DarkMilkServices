namespace LauncherDM.Infrastructure.ReactiveUI
{
    public class LanguagesUpdate
    {
        public bool UpdateUI { get; set; }

        public LanguagesUpdate(bool updateUI)
        {
            UpdateUI = updateUI;
        }
    }
}
