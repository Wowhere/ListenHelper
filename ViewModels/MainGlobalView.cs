using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using System.Reactive;
using voicio.Views;

namespace voicio.ViewModels
{
    public class MainGlobalView: ViewModelBase
    {
        public ReactiveCommand<Unit,Unit> OpenMainWindow { get; set; }
        public ReactiveCommand<Unit, Unit> QuitAppCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowTagsWindowCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowImportWindowCommand { get; }
        public MainGlobalView() {
            OpenMainWindow = ReactiveCommand.Create(() => {
                var w1 = new MainWindow() { DataContext = new MainWindowViewModel() };
                w1.Show();
            });
            ShowImportWindowCommand = ReactiveCommand.Create(() => {
                var w2 = new ImportWindow() { DataContext = new ImportWindowViewModel() };
                w2.Show();
            });
            ShowTagsWindowCommand = ReactiveCommand.Create(() => {
                var w3 = new TagWindow() { DataContext = new TagWindowViewModel() };
                w3.Show();
            });
            QuitAppCommand = ReactiveCommand.Create(() => {
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    desktop.Shutdown();
                }
            });

        }
    }
}
