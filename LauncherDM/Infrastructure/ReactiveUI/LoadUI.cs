namespace LauncherDM.Infrastructure.ReactiveUI
{
    public class LoadUI
    {
        public bool UpdateUI { get; set; }

        public LoadUI(bool updateUI)
        {
            UpdateUI = updateUI;
        }
    }
}
