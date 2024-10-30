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
        public ReactiveCommand<Unit, Unit> ShowVoiceOperationsCommand { get; }
        public ReactiveCommand<Unit,Unit> OpenMainWindow { get; }
        public ReactiveCommand<Unit, Unit> QuitAppCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowTagsWindowCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowImportWindowCommand { get; }
        public MainGlobalView() {
            OpenMainWindow = ReactiveCommand.Create(() => {
                var w1 = new MainWindow() { DataContext = new MainWindowViewModel() };
                w1.Show();
            });
            ShowVoiceOperationsCommand = ReactiveCommand.Create(() => {
                var w2 = new VoiceOperationWindow() { DataContext = new VoiceOperationViewModel() };
                w2.Show();
            });
            ShowImportWindowCommand = ReactiveCommand.Create(() => {
                var w3 = new ImportWindow() { DataContext = new ImportWindowViewModel() };
                w3.Show();
            });
            ShowTagsWindowCommand = ReactiveCommand.Create(() => {
                var w4 = new TagWindow() { DataContext = new TagWindowViewModel() };
                w4.Show();
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
