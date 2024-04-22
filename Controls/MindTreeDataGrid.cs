using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Controls.Primitives;
using voicio.ViewModels;

namespace voicio.Controls
{
    public class MindTreeDataGridRow : TreeDataGridRow
    {
        private bool? _RowType;
        public bool? RowType
        {
            get => _RowType;
            set => SetAndRaise(RowTypeProperty, ref _RowType, value);
        }
        public static readonly DirectProperty<MindTreeDataGridRow, bool?> RowTypeProperty =
            AvaloniaProperty.RegisterDirect<MindTreeDataGridRow, bool?>(
                nameof(RowType),
                o => o.RowType);
    }
    public class MindTreeDataGrid : TreeDataGrid
    {

        //public static readonly DirectProperty<MindTreeDataGrid, bool?> RowTypeProperty =
        //    AvaloniaProperty.RegisterDirect<MindTreeDataGrid, bool?>(
        //        nameof(RowType),
        //        o => o.RowType);
        public MindTreeDataGrid()
        {
        }
    }
}