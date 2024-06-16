using System.IO;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;
using voicio.Models;
using System.Collections.Generic;

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
            List<Hint> import_hints = new List<Hint>();
            using (StringReader reader = new StringReader(ImportText))
            {
                string line = "";
                do { 
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        string[] t = line.Split(',');
                        import_hints.Add(new Hint(t[0], t[1]));
                    }
                } while (line != null);
            }
            using (var DataSource = new HelpContext())
            {
                DataSource.HintTable.AddRange(import_hints);
                DataSource.SaveChangesAsync();
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