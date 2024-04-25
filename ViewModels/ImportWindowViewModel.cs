using ReactiveUI;
using System.Collections.Generic;
using System.Reactive;
using voicio.Models;

namespace voicio.ViewModels
{
    public class ImportWindowViewModel : ViewModelBase
    {
        private bool _IsPinnedWindow = false;
        public bool IsPinnedWindow
        {
            get => _IsPinnedWindow;
            set => this.RaiseAndSetIfChanged(ref _IsPinnedWindow, value);
        }
        
        public ImportWindowViewModel()
        {
            
        }
    }
}