using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Text_T9
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int ClickCount { get; set; }
        public int ClickIndex { get; set; }
        public int OldClickIndex { get; set; }

        public bool asd { get; set; }
        public bool LargeFont { get; set; }
        public bool First { get; set; }
        public bool IsComplete { get; set; }

        private Timer timer;
        public MainWindow()
        {
            InitializeComponent();
            ClickCount = 0;
            ClickIndex = 0;
            OldClickIndex = -1;
            First = true;
            timer = new Timer(a => Complete(), null, Timeout.Infinite, Timeout.Infinite);
            IsComplete = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timer.Change(2000, Timeout.Infinite);
            var b = sender as Button;
            if (b == null) return;
            ClickCount++;
            if (ClickCount > 3) ClickCount -= 3;
            ClickIndex = int.Parse(b.Tag.ToString());

            if (First)
            {
                OldClickIndex = ClickIndex;
                First = false;
            }
            else if (ClickIndex != OldClickIndex)
            {
                asd = false;
                ClickCount = 0;
                ClickCount++;
            }
            else
            {
                if (OldClickIndex == ClickIndex) 
                    asd = true;
                else 
                    asd = false;
            }
            OldClickIndex = ClickIndex;
            Text_Write();
        }


        private void Text_Write()
        {
            char ch = (char)(OldClickIndex + ClickCount + 62);
            if (LargeFont) ch.ToString().ToLower();
            if (asd)
            {
                Dispatcher.Invoke(() =>
                {
                    if (txt.Text != "")
                    {
                        if (IsComplete)
                        {
                            txt.Text += ch;
                            txt.Select(txt.Text.Length - 1, 1);
                            txt.Focus();
                            IsComplete = false;
                        }
                        txt.Text = txt.Text.Remove(txt.Text.Length - 1, 1);
                    }
                });
            }
            Dispatcher.Invoke(() =>
            {
                txt.Text += ch;
                txt.Select(txt.Text.Length - 1, 1);
                txt.Focus();
            });

        }

        public void Complete()
        {
            Dispatcher.Invoke(() =>
            {
                txt.Focusable = false;
                txt.CaretIndex = txt.Text.Length;
                txt.Focusable = true;
                ClickCount = 0;
                IsComplete = true;

            });
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            timer.Change(2000, Timeout.Infinite);
            var b = sender as Button;
            if (b == null) return;
            ClickCount++;
            if (ClickCount > 4) ClickCount -= 4;
            ClickIndex = int.Parse(b.Tag.ToString());

            if (First)
            {
                OldClickIndex = ClickIndex;
                First = false;
            }
            else if (ClickIndex != OldClickIndex)
            {
                asd = false;
                ClickCount = 0;
                ClickCount++;
            }
            else
            {
                if (OldClickIndex == ClickIndex)
                    asd = true;
                else
                    asd = false;
            }
            OldClickIndex = ClickIndex;
            Text_Write();
        }

        private void txt_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (((int)e.Key) >= 73 && (int)e.Key <= 83)
            {
                e.Handled = false;
            }
        }

        private void Remove_Button(object sender, RoutedEventArgs e)
        {
            if (txt.Text != "") txt.Text = txt.Text.Remove(txt.Text.Length - 1, 1);
        }
    }
}
