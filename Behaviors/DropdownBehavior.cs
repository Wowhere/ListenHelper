using Avalonia.Interactivity;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Xaml.Interactivity;
using System.Threading.Tasks;
using System;

namespace voicio.Behaviors
{
    public class DropdownBehavior: Behavior<AutoCompleteBox>
    {
        protected override void OnAttached()
        {
            if (AssociatedObject is not null)
            {
                AssociatedObject.KeyUp += OnKeyUp;
                AssociatedObject.DropDownOpening += DropDownOpening;
                Task.Delay(100).ContinueWith(_ => Avalonia.Threading.Dispatcher.UIThread.Invoke(() => { CreatePanel(); }));
            }

            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject is not null)
            {
                AssociatedObject.KeyUp -= OnKeyUp;
                AssociatedObject.DropDownOpening -= DropDownOpening;
            }

            base.OnDetaching();
        }

        private void OnKeyUp(object? sender, Avalonia.Input.KeyEventArgs e)
        {
            if (e.Key == Avalonia.Input.Key.Down)
            {
                ShowDropdown();
            }
        }

        private void DropDownOpening(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            //var prop = AssociatedObject.GetType().GetProperty("TextBox", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            //var tb = (TextBox?)prop?.GetValue(AssociatedObject);
        }

        private void ShowDropdown()
        {
            if (AssociatedObject is not null && !AssociatedObject.IsDropDownOpen)
            {
                typeof(AutoCompleteBox).GetMethod("PopulateDropDown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.Invoke(AssociatedObject, new object[] { AssociatedObject, EventArgs.Empty });
                typeof(AutoCompleteBox).GetMethod("OpeningDropDown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.Invoke(AssociatedObject, new object[] { false });

                //if (!AssociatedObject.IsDropDownOpen)
                {
                    //We *must* set the field and not the property as we need to avoid the changed event being raised (which prevents the dropdown opening).
                    var ipc = typeof(AutoCompleteBox).GetField("_ignorePropertyChange", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if ((bool)ipc?.GetValue(AssociatedObject) == false)
                        ipc?.SetValue(AssociatedObject, true);

                    AssociatedObject.SetCurrentValue<bool>(AutoCompleteBox.IsDropDownOpenProperty, true);
                }
            }
        }

        private Button CreateDropdownButton()
        {
            //var prop = AssociatedObject.GetType().GetProperty("TextBox", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            //var tb = (TextBox?)prop?.GetValue(AssociatedObject);

            var btn = new Button()
            {
               Content = "↓",
               Margin = new(1),
               ClickMode = ClickMode.Press
            };
            btn.Click += (s, e) => ShowDropdown();

            //tb.InnerRightContent = btn;
            return btn;
        }
        private void CleanTextBox()
        {
            if (AssociatedObject is not null)
            {
                {
                    AssociatedObject.Text = "";
                }
            }
        }
        private Button CreateRemoveTextButton()
        {
            var btn = new Button()
            {
                 Content = "Ⅹ",
                 //Margin = new(1),
                 ClickMode = ClickMode.Press,
                 Width = 28,
                 Height = 28
            };
            btn.Background = new SolidColorBrush() { Color = new Color(255, 80, 0, 20) };
            btn.Margin = new(0, 0, 4, 0);
            btn.CornerRadius = new Avalonia.CornerRadius(25);
            btn.Click += (s, e) => CleanTextBox();
            return btn;
            
        }
        private void CreatePanel()
        {
            if (AssociatedObject != null)
            {
                var prop = AssociatedObject.GetType().GetProperty("TextBox", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                var tb = (TextBox?)prop?.GetValue(AssociatedObject);

                var panel = new DockPanel()
                {
                    //Content = "Ⅹ",
                    Margin = new(1),
                    //ClickMode = ClickMode.Press
                };
                //btn.Click += (s, e) => CleanTextBox();
                tb.InnerRightContent = panel;
                panel.Children.Add(CreateRemoveTextButton());
                panel.Children.Add(CreateDropdownButton());
                panel.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
                //pab.InnerLeftContent = CreateRemoveTextButton();
                //tb.InnerRightContent = CreateDropdownButton();
                //CreateRemoveTextButton();
                //CreateDropdownButton();
            }
        }
        
    }
}

