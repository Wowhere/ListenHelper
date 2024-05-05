using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using System.Reactive;
using voicio.Views;

namespace voicio.ViewModels
{
    public class MainGlobalView: ViewModelBase
    {
        private ImportWindow importWindow = null;
        private TagWindow tagWindow = null;
        public ReactiveCommand<Unit,Unit> OpenMainWindow { get; }
        public ReactiveCommand<Unit, Unit> QuitAppCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowTagsWindowCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowImportWindowCommand { get; }
        public MainGlobalView() {
            OpenMainWindow = ReactiveCommand.Create(() => {
                var w1 = new MainWindow() { DataContext = new MainWindowViewModel() };
                w1.Show();
            });
            ShowImportWindowCommand = ReactiveCommand.Create(() => {
                if (importWindow is not null) { importWindow.Focus(); }
                else
                {
                    importWindow = new ImportWindow() { DataContext = new ImportWindowViewModel() };
                    importWindow.Show();
                }
            });
            ShowTagsWindowCommand = ReactiveCommand.Create(() => {
                if (tagWindow is not null) { tagWindow.Focus(); tagWindow.Show(); }
                else
                {
                    tagWindow = new TagWindow() { DataContext = new TagWindowViewModel() };
                    tagWindow.Show();
                }
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
