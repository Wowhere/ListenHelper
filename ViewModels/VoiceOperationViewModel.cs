using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Controls.Selection;
using Avalonia.Controls.Templates;
using Avalonia.Media;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private FlatTreeDataGridSource<VoiceOperation>? _VoiceOperationGridData;
        public FlatTreeDataGridSource<VoiceOperation>? VoiceOperationGridData
        {
            get => _VoiceOperationGridData;
            set => this.RaiseAndSetIfChanged(ref _VoiceOperationGridData, value);
        }
        private ObservableCollection<VoiceOperation>? _VoiceOperationRows;
        public ObservableCollection<VoiceOperation>? VoiceOperationRows
        {
            get => _VoiceOperationRows;
            set => this.RaiseAndSetIfChanged(ref _VoiceOperationRows, value);
        }
        public void AddTempOperation()
        {
            VoiceOperation newOp = new VoiceOperation(false);
            VoiceOperationRows.Add(newOp);
        }
        private CheckBox IsActiveOperationCheckboxInit(VoiceOperation op)
        {
            var b = new CheckBox();
            b.IsChecked = op.IsActive;
            return b;
        }
        public void TreeDataGridInit()
        {
            var TextColumnLength = new GridLength(1, GridUnitType.Star);
            var TemplateColumnLength = new GridLength(125, GridUnitType.Pixel);

            var EditOptions = new TextColumnOptions<VoiceOperation>
            {
                BeginEditGestures = BeginEditGestures.Tap,
                MinWidth = new GridLength(80, GridUnitType.Pixel)
            };
            TemplateColumn<VoiceOperation> IsActiveOperationColumn = new TemplateColumn<VoiceOperation>("", new FuncDataTemplate<VoiceOperation>((a, e) => IsActiveOperationCheckboxInit(a), supportsRecycling: true), width: TemplateColumnLength);
            TextColumn<VoiceOperation, string> DescriptionTextColumn = new TextColumn<VoiceOperation, string>("Description", x => x.Description, (r, v) => r.Description = v, options: EditOptions, width: TextColumnLength);
            TextColumn<VoiceOperation, string> CommandTextColumn = new TextColumn<VoiceOperation, string>("Voice Command", x => x.Command, (r, v) => r.Command = v, options: EditOptions, width: TextColumnLength);
            VoiceOperationGridData = new FlatTreeDataGridSource<VoiceOperation>(VoiceOperationRows)
            {
                Columns =
                    {
                        IsActiveOperationColumn,
                        DescriptionTextColumn,
                        CommandTextColumn
                    },
            };
            VoiceOperationGridData.Selection = new TreeDataGridCellSelectionModel<VoiceOperation>(VoiceOperationGridData);
        }
        public void ShowAllOperations()
        {
            using (var DataSource = new HelpContext())
            {
                List<VoiceOperation> voiceOperations = DataSource.VoiceOperationTable.ToList();
                VoiceOperationRows = new ObservableCollection<VoiceOperation>(voiceOperations);
            }
            TreeDataGridInit();
        }
        public VoiceOperationViewModel()
        {
            VoiceOperationRows = new ObservableCollection<VoiceOperation>();
            TreeDataGridInit();
            ShowAllOperations();
        }
    }
}
