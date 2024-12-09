using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Controls.Selection;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Media;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using voicio.Models;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;
using AvaloniaEdit;
using Avalonia;

namespace voicio.ViewModels
{
    public class VoiceOperationViewModel : ViewModelBase
    {
        private void CreateCompiledExtension(object sender, RoutedEventArgs e)
        {
            Button codeButton = (Button)sender;
            VoiceOperation obj = (VoiceOperation)codeButton.DataContext;
            string code = "";
            var options = new CSharpCompilationOptions((OutputKind)LanguageVersion.Latest);
            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var compilation = CSharpCompilation.Create("DynamicAssembly")
            .AddSyntaxTrees(syntaxTree)
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);
                obj.ActionTreeExpression = ms.ToArray();  
            }
        }
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
        private void CompileActionViewer(object sender, RoutedEventArgs e)
        {
            var newWindow = new Window() { DataContext = new MainWindowViewModel() };
            Button codeButton = (Button)sender;
            newWindow.WindowState = WindowState.Maximized;
            VoiceOperation obj = (VoiceOperation)codeButton.DataContext;
            newWindow.Title = "Code for \""+ obj.Command + "\" command...";
            AvaloniaEdit.TextEditor logTextBox = new AvaloniaEdit.TextEditor();
            AvaloniaEdit.Document.TextDocument logText = new AvaloniaEdit.Document.TextDocument("");
            logTextBox.DataContext = newWindow.DataContext;
            logTextBox.ShowLineNumbers = true;
            logTextBox.Height = 750;
            
            logTextBox.BorderBrush = new SolidColorBrush(Color.Parse("#008b8b"));
            logTextBox.BorderThickness = Thickness.Parse("2");
            logTextBox.VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Visible;
            logTextBox.HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Visible;
            logTextBox.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;

            Button compileButton = new Button();
            compileButton.DataContext = newWindow.DataContext;
            compileButton.Click += CreateCompiledExtension;
            compileButton.Width = 80;
            compileButton.Content = "Compile";
            compileButton.Margin = Thickness.Parse("5,5,5,5");
            var panel = new StackPanel();
            panel.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch;
            panel.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
            panel.Children.Add(logTextBox);
            panel.Children.Add(compileButton);
            newWindow.Content = panel;
            newWindow.Show();
        }
        private Button ToggleCompileActionButtonInit(VoiceOperation obj)
        {
            var LogViewButton = new Button();
            LogViewButton.Click += CompileActionViewer;
            LogViewButton.Content = "Code";
            return LogViewButton;
        }
        private CheckBox IsActiveOperationCheckboxInit(VoiceOperation op)
        {
            var b = new CheckBox();
            b.IsChecked = op.IsActive;
            return b;
        }
        private void RemoveVoiceOperation(object sender, RoutedEventArgs e)
        {
            Button removeButton = (Button)sender;
            VoiceOperation removedOps = (VoiceOperation)removeButton.DataContext;
            VoiceOperationRows.Remove(removedOps);
            if (removedOps.IsSaved)
            {
                using (var DataSource = new HelpContext())
                {
                    DataSource.VoiceOperationTable.Attach(removedOps);
                    DataSource.VoiceOperationTable.Remove(removedOps);
                    DataSource.SaveChanges();
                }
            }
        }
        private void UpdateVoiceOperation(object sender, RoutedEventArgs e)
        {
            Button updateButton = (Button)sender;
            VoiceOperation updateHint = (VoiceOperation)updateButton.DataContext;
            List<Tag> assosiatedTags = new List<Tag>();
            if (updateHint.IsSaved)
            {
                using (var DataSource = new HelpContext())
                {
                    DataSource.VoiceOperationTable.Attach(updateHint);
                    DataSource.VoiceOperationTable.Update(updateHint);
                    DataSource.SaveChanges();
                }
            }
            else
            {
                using (var DataSource = new HelpContext())
                {
                    DataSource.VoiceOperationTable.Attach(updateHint);
                    DataSource.VoiceOperationTable.Add(updateHint);
                    DataSource.SaveChanges();
                }
                updateHint.IsSaved = true;
            }
        }
        private Button UpdateButtonInit()
        {
            var b = new Button();
            b.Background = new SolidColorBrush() { Color = new Color(255, 34, 139, 34) };
            b.Content = "Add";
            b.Click += UpdateVoiceOperation;
            return b;
        }
        private Button RemoveButtonInit()
        {
            var b = new Button();
            b.Background = new SolidColorBrush() { Color = new Color(255, 80, 00, 20) };
            b.Content = "Remove";
            b.Click += RemoveVoiceOperation;
            return b;
        }
        private DockPanel ButtonsPanelInit()
        {
            var panel = new DockPanel();
            panel.Children.Add(UpdateButtonInit());
            panel.Children.Add(RemoveButtonInit());
            panel.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
            return panel;
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
            TemplateColumn<VoiceOperation> IsActiveOperationColumn = new TemplateColumn<VoiceOperation>("Enabled", new FuncDataTemplate<VoiceOperation>((a, e) => IsActiveOperationCheckboxInit(a), supportsRecycling: true), width: TemplateColumnLength);
            TemplateColumn<VoiceOperation> ButtonColumn = new TemplateColumn<VoiceOperation>("", new FuncDataTemplate<VoiceOperation>((a, e) => ButtonsPanelInit(), supportsRecycling: true), width: TemplateColumnLength);
            TemplateColumn<VoiceOperation> CompileColumn = new TemplateColumn<VoiceOperation>("", new FuncDataTemplate<VoiceOperation>((a, e) => ToggleCompileActionButtonInit(a), supportsRecycling: true), width: TemplateColumnLength);
            TextColumn<VoiceOperation, string> DescriptionTextColumn = new TextColumn<VoiceOperation, string>("Description", x => x.Description, (r, v) => r.Description = v, options: EditOptions, width: TextColumnLength);
            TextColumn<VoiceOperation, string> CommandTextColumn = new TextColumn<VoiceOperation, string>("Voice Command", x => x.Command, (r, v) => r.Command = v, options: EditOptions, width: TextColumnLength);
            VoiceOperationGridData = new FlatTreeDataGridSource<VoiceOperation>(VoiceOperationRows)
            {
                Columns =
                    {
                        IsActiveOperationColumn,
                        DescriptionTextColumn,
                        CommandTextColumn,
                        CompileColumn,
                        ButtonColumn
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
