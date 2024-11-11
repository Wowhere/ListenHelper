using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using voicio.ViewModels;
using voicio.Views;
using voicio.Models;
using System.Threading.Tasks;
using voicio.Services;

namespace voicio
{
    public partial class App : Application
    {
        public App()
        {
            MainGlobalView AppModelView = new MainGlobalView();
            DataContext = AppModelView;
        }
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
        public async override void OnFrameworkInitializationCompleted()
        {
            var tempdb = new HelpContext();
            tempdb.Database.EnsureCreated(); //create DB if no DB is found
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                   DataContext = new MainWindowViewModel(),
                };
                desktop.ShutdownMode = Avalonia.Controls.ShutdownMode.OnExplicitShutdown;
            }
            await Task.Run(() =>
            {
                new VoiceService().StartService();
            });
            base.OnFrameworkInitializationCompleted();
        }
    }
}