using ReactiveUI;
using voicio.Models;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using System.Collections.Generic;
using System.Reactive;
using System.Linq;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Controls.Templates;
using Avalonia.Controls.Selection;
using Avalonia.Media;
using Avalonia.Interactivity;
using Avalonia.Controls.Primitives;
using DynamicData;

namespace voicio.ViewModels
{
    public class TagWindowViewModel : ViewModelBase
    {
        private string _query = "";
        public string Query
        {
            get => _query;
            set => this.RaiseAndSetIfChanged(ref _query, value);
        }
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
        public ReactiveCommand<Unit, Unit> ShowTagsCommand { get; }
        public void ShowAllTags()
        {
            using (var DataSource = new HelpContext())
            {
                List<Tag> tags = DataSource.TagTable.ToList();
                TagsRows = new ObservableCollection<Tag>(tags);
            }
            TreeDataGridInit();
        }
        private void RemoveTag(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Tag RemovedTag = (Tag)b.DataContext;
            TagsRows.Remove(RemovedTag);
            if (RemovedTag.IsSaved)
            {
                using (var DataSource = new HelpContext())
                {
                    DataSource.TagTable.Attach(RemovedTag);
                    DataSource.TagTable.Remove(RemovedTag);
                    DataSource.SaveChanges();
                }
            }
        }
        private void UpdateTag(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Tag UpdateTag = (Tag)b.DataContext;
            if (UpdateTag.IsSaved)
            {
                using (var DataSource = new HelpContext())
                {
                    DataSource.TagTable.Attach(UpdateTag);
                    DataSource.TagTable.Update(UpdateTag);
                    DataSource.SaveChanges();
                }
            }
            else
            {
                using (var DataSource = new HelpContext())
                {
                    DataSource.TagTable.Attach(UpdateTag);
                    DataSource.TagTable.Add(UpdateTag);
                    DataSource.SaveChanges();
                }
                UpdateTag.IsSaved = true;
            }
        }
        private Button UpdateButtonInit()
        {
            var b = new Button();
            b.Background = new SolidColorBrush() { Color = new Color(255, 34, 139, 34) };
            b.Content = "Add";
            b.Click += UpdateTag;
            return b;
        }
        private Button RemoveButtonInit()
        {
            var b = new Button();
            b.Background = new SolidColorBrush() { Color = new Color(255, 80, 00, 20) };
            b.Content = "Remove";
            b.Click += RemoveTag;
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
            var GridOptions = new TextColumnOptions<Tag>
            {
                BeginEditGestures = BeginEditGestures.Tap,
                MinWidth = new GridLength(240, GridUnitType.Pixel)
            };
            TextColumn<Tag, string> TagTextColumn = new TextColumn<Tag, string>("Tag", x => x.TagText, (r, v) => r.TagText = v, options: GridOptions);
            //TextColumn<Tag, string> TagUses = new TextColumn<Tag, string>("Tag", x => x.TagText);
            TemplateColumn<Tag> TagButtonsColumn = new TemplateColumn<Tag>("", new FuncDataTemplate<Tag>((a, e) => ButtonsPanelInit(), supportsRecycling: true));
            TagsGridData = new FlatTreeDataGridSource<Tag>(TagsRows)
            {
                Columns =
                {
                   TagTextColumn,
                   TagButtonsColumn,
                },
            };
            TagsGridData.Selection = new TreeDataGridCellSelectionModel<Tag>(TagsGridData);
        }
        public void AddTag()
        {
            Tag NewTag = new Tag(false);
            TagsRows.Add(NewTag);
        }
        public ReactiveCommand<Unit, Unit> StartSearchCommand { get; }
        public void StartSearch()
        {
            using (var DataSource = new HelpContext())
            {
                List<Tag> tags = new List<Tag>();
                tags.Add(DataSource.TagTable.Where(b => b.TagText.Contains(Query)).ToList());
                TagsRows = new ObservableCollection<Tag>(tags);
            }
            TreeDataGridInit();
        }
        public TagWindowViewModel()
        {
            Query = "";
            StartSearchCommand = ReactiveCommand.Create(StartSearch);
            ShowTagsCommand = ReactiveCommand.Create(ShowAllTags);
            TagsRows = new ObservableCollection<Tag>();
            TreeDataGridInit();
        }
    }
}