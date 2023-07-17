using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsInput;
using WindowsInput.Native;

namespace Text_T9
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<string> list { get; set; } = new List<string>();
        public int ClickCount { get; set; }
        public int ClickIndex { get; set; }
        public int OldClickIndex { get; set; }
        public bool SameCLick { get; set; }
        public bool SmallFont { get; set; }
        public bool First { get; set; }
        public bool IsComplete { get; set; }
        public string start { get; set; }
        public string? word { get; set; }

        public bool IsT9 { get; set; }

        private Timer timer;

        public MainWindow()
        {
            InitializeComponent();
            FillList();
            ClickCount = 0;
            ClickIndex = 0;
            OldClickIndex = -1;
            SmallFont = true;
            First = true;
            timer = new Timer(a => Complete(), null, Timeout.Infinite, Timeout.Infinite);
            IsComplete = false;
            start = "";
            IsT9 = true;

        }

        private void T9()
        {
            Dispatcher.Invoke(() =>
            {
                if (txt.Text != "" && IsT9)
                {
                    try
                    {
                        var c = txt.Text.Split(" ");
                        start = c[c.Length - 1];
                        if (!string.IsNullOrEmpty(start))
                        {
                            word = list.FirstOrDefault(word => word.StartsWith(start))!;
                            var b = word?.Substring(start.Length)!;
                            word = b;
                        }


                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
            });
        }

        public void FillList()
        {
            string[] words = File.ReadAllLines(@"C:\Users\HP\source\repos\Text_T9\Text_T9\Vocabulary.txt");
            list.AddRange(words);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timer.Change(2000, Timeout.Infinite);
            var b = sender as Button;
            if (b == null) return;
            ClickCount++;
            if (ClickCount > 3) ClickCount -= 3;
            ClickIndex = int.Parse(b.Tag.ToString()!);
            Cast();
            Text_Write();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            timer.Change(2000, Timeout.Infinite);
            var b = sender as Button;
            if (b == null) return;
            ClickCount++;
            if (ClickCount > 4) ClickCount -= 4;
            ClickIndex = int.Parse(b.Tag.ToString()!);
            Cast();
            Text_Write();
        }

        private void Cast()
        {
            if (First)
            {
                OldClickIndex = ClickIndex;
                First = false;
            }
            else if (ClickIndex != OldClickIndex)
            {
                IsComplete = false;
                SameCLick = false;
                ClickCount = 0;
                ClickCount++;
            }
            else
            {
                if (OldClickIndex == ClickIndex)
                    SameCLick = true;
                else
                    SameCLick = false;
            }
            OldClickIndex = ClickIndex;
        }

        private void Text_Write()
        {
            if (!string.IsNullOrEmpty(word))
            {
                txt.Text = txt.Text.Remove(txt.Text.Length - word.Length);
                word = "";
            }
            var t = Task.Run(() =>
             {
                 Dispatcher.Invoke(() =>
                 {
                     char ch = (char)(OldClickIndex + ClickCount + 62);
                     string AddChar = ch.ToString();
                     if (SmallFont) AddChar = AddChar.ToLower();
                     if (SameCLick)
                     {
                         Dispatcher.Invoke(() =>
                         {
                             if (IsComplete)
                             {
                                 txt.Text += AddChar;
                                 txt.Select(txt.Text.Length-1, 1);
                                 txt.Focus();
                                 IsComplete = false;
                             }
                             txt.Text = txt.Text.Remove(txt.Text.Length-1, 1);
                         });
                     }
                     txt.Text+=AddChar;
                     txt.Select(txt.Text.Length - 1, 1);
                     txt.Focus();

                 });
             });
            t.ContinueWith((t) =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        T9();
                    }
                    else MessageBox.Show("T");
                });
            }).ContinueWith((tt) =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (tt.IsCompletedSuccessfully)
                    {
                        if (!string.IsNullOrEmpty(word))
                        {
                            txt.Text += word;
                            txt.Select(txt.Text.Length - word.Length, word.Length);
                            txt.Focus();
                        }
                    }
                    else MessageBox.Show("Tt");

                });
            });
        }


        public void Complete()
        {
            Dispatcher.Invoke(() =>
            {
                txt.Focusable = false;
                txt.Focusable = true;
                ClickCount = 0;
                IsComplete = true;
                if (string.IsNullOrEmpty(word))
                {
                    txt.SelectionLength = 0;
                    txt.SelectionStart = txt.Text.Length;
                }
            });
        }

        private void txt_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void Remove_Button(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(word))
            {
                txt.Text = txt.Text.Remove(txt.Text.Length - word.Length);
                word = "";
            }
            if (txt.Text != "") txt.Text = txt.Text.Remove(txt.Text.Length - 1, 1);
        }

        private void Button_Click_0(object sender, RoutedEventArgs e)
        {
            timer.Change(2000, Timeout.Infinite);
            var b = sender as Button;
            if (b == null) return;
            if (ClickCount > 1) ClickCount = 0;
            else if (ClickCount == 1) ClickCount = 11;
            ClickIndex = -31;
            ClickCount++;
            Cast();
            Text_Write();
        }

        private void Button_ClickLS(object sender, RoutedEventArgs e)
        {
            if (SmallFont) SmallFont = false;
            else SmallFont = true;
            MessageBox.Show($"Large Font is:{SmallFont}");
        }

        private void Button_Right(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(word))
            {
                Complete();
                word = "";
                txt.SelectionStart = txt.Text.Length;
            }
            if (txt.SelectionStart <= txt.Text.Length)
            {
                txt.SelectionStart++;
                txt.SelectionLength = 0;
                txt.Focus();
            }
        }

        private void Button_Add(object sender, RoutedEventArgs e)
        {
            var TextSplit = txt.Text.Split();
            var word = TextSplit[TextSplit.Length - 1];
            if (string.IsNullOrEmpty(word)) return;
            using (StreamWriter write = File.AppendText(@"C:\Users\HP\source\repos\Text_T9\Text_T9\Vocabulary.txt"))
            {
                write.WriteLine(word);
                MessageBox.Show($"{word} Added Vocabulary");
            }

        }

        private void Button_Left(object sender, RoutedEventArgs e)
        {
            if (txt.SelectionStart >= 1)
            {
                txt.SelectionStart--;
                txt.SelectionLength = 0;
                txt.Focus();
            }
        }

        private void Button_Up(object sender, RoutedEventArgs e)
        {


            int caretIndex = txt.CaretIndex;
            int lineIndex = txt.GetLineIndexFromCharacterIndex(caretIndex);

            if (lineIndex > 0)
            {
                int previousLineStart = txt.GetCharacterIndexFromLineIndex(lineIndex - 1);
                int offset = caretIndex - txt.GetCharacterIndexFromLineIndex(lineIndex);
                int newCaretIndex = previousLineStart + Math.Min(offset, txt.GetLineLength(lineIndex - 1));
                txt.CaretIndex = newCaretIndex;
            }
            txt.Focus();

        }

        private void Button_Down(object sender, RoutedEventArgs e)
        {

            int caretIndex = txt.CaretIndex;
            int lineIndex = txt.GetLineIndexFromCharacterIndex(caretIndex);

            if (lineIndex < txt.LineCount - 1)
            {
                int nextLineStart = txt.GetCharacterIndexFromLineIndex(lineIndex + 1);
                int offset = caretIndex - txt.GetCharacterIndexFromLineIndex(lineIndex);
                int newCaretIndex = nextLineStart + Math.Min(offset, txt.GetLineLength(lineIndex + 1));
                txt.CaretIndex = newCaretIndex;
            }
            txt.Focus();
        }

        private void Button_T9(object sender, RoutedEventArgs e)
        {
            if (IsT9) IsT9 = false;
            else IsT9 = true;
            MessageBox.Show($"T9 is {IsT9}");
        }
    }
}
