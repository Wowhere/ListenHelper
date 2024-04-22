using Avalonia.Controls;
using Avalonia.Input;

namespace voicio.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var focused = this.FindControl<AutoCompleteBox>("searchInput");
            if (focused != null)
            {
                focused.AttachedToVisualTree += (s, e) => focused.Focus();
            }
        }
        //protected override void OnInitialized()
        //{
        //    base.OnInitialized();
        //    var focused = this.FindControl<AutoCompleteBox>("searchInput");
        //    if (focused != null)
        //    {
        //        focused.AttachedToVisualTree += (s, e) => focused.Focus();
        //    }
        //}
        public async void CopyHintToClipboard(object sender, TappedEventArgs e)
        {
            if (e.Source.GetType() == typeof(TextBlock))
            {
                TreeDataGrid c = (TreeDataGrid)sender;
                var clipboard = GetTopLevel(c).Clipboard;
                var dataObject = new DataObject();

                dataObject.Set(DataFormats.Text, ((TextBlock)e.Source).Text);
                await clipboard.SetDataObjectAsync(dataObject);
            }
        }
    }
}