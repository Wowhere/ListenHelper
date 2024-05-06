using System.IO;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;
using voicio.Models;
using System.Data.SqlClient;

namespace voicio.ViewModels
{
    public class ImportWindowViewModel : ViewModelBase
    {
        private string _importText = "";
        public string ImportText
        {
            get => _importText;
            set => this.RaiseAndSetIfChanged(ref _importText, value);
        }
        public void InputTextImport() {

            using (StringReader reader = new StringReader(ImportText))
            {
                string line = "";
                do { 
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        var t= line.Split(',');
                    }
                } while (line != null);
            }

        }
        public async Task FileImport()
        {
            //var storageProvider = StorageService.GetStorageProvider();
            //if (storageProvider is null)
            //{
            //    return null;
            //}
            //var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            //{
            //    Title = "Open file for import",
            //    AllowMultiple = false
            //});
        }
        public Interaction<Unit, string?> ShowOpenFileDialog { get; }
        public ReactiveCommand<Unit, Unit> InputTextImportCommand { get; }
        //public ReactiveCommand<Unit, Unit> FileImportCommand { get; }
        public ImportWindowViewModel()
        {
            InputTextImportCommand = ReactiveCommand.Create(InputTextImport);
            //FileImportCommand = ReactiveCommand.CreateFromTask(FileImport);
            ShowOpenFileDialog = new Interaction<Unit, string?>();
        }
    }
}