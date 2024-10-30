using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using voicio.Models;

namespace voicio.ViewModels
{
    public class VoiceOperationViewModel : ViewModelBase
    {
        private bool _IsPinnedWindow = false;
        public bool IsPinnedWindow
        {
            get => _IsPinnedWindow;
            set => this.RaiseAndSetIfChanged(ref _IsPinnedWindow, value);
        }
        private FlatTreeDataGridSource<VoiceOperation>? _TagsGridData;
        public FlatTreeDataGridSource<VoiceOperation>? TagsGridData
        {
            get => _TagsGridData;
            set => this.RaiseAndSetIfChanged(ref _TagsGridData, value);
        }
    }
}
