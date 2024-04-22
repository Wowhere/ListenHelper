using ReactiveUI;
using voicio.Models;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using System.Collections.Generic;

namespace voicio.ViewModels
{
    public class TagWindowViewModel : ViewModelBase
    {
        private bool _IsPinnedWindow = false;
        public bool IsPinnedWindow
        {
            get => _IsPinnedWindow;
            set => this.RaiseAndSetIfChanged(ref _IsPinnedWindow, value);
        }
        private ObservableCollection<Tag>? _TagsRows;
        public ObservableCollection<Tag>? TagsRows
        {
            get => _TagsRows;
            set => this.RaiseAndSetIfChanged(ref _TagsRows, value);
        }
        private FlatTreeDataGridSource<Tag>? _TagsGridData;
        public FlatTreeDataGridSource<Tag>? TagsGridData
        {
            get => _TagsGridData;
            set => this.RaiseAndSetIfChanged(ref _TagsGridData, value);
        }
        public void TreeDataGridInit()
        {
            using (var DataSource = new HelpContext())
            {
                List<Tag> tags = new List<Tag>();

            }
        }
        public TagWindowViewModel()
        {
            TreeDataGridInit();
        }
    }
}