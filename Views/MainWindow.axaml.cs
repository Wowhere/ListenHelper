using System;
using Avalonia.Controls;
using Avalonia.Input;

namespace voicio.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.Opened += StartFocusing;
            InitializeComponent();

        }
        private void StartFocusing(object sender, EventArgs arg) {
            var focused = this.FindControl<AutoCompleteBox>("searchbox");
            if (focused != null)
            {
                focused.Loaded += (s, e) => focused.Focus();
            }
        }
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