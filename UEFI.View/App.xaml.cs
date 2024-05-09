using UEFI.View.Services;
using UEFI.View.Views;

namespace UEFI.View
{
    partial class App
    {
        private void Application_Startup(object sender, System.Windows.StartupEventArgs e)
        {
            DI.Register();
            MainWindow = DI.GetRequiredService<MainWindow>();
            MainWindow.Show();
        }
    }
}