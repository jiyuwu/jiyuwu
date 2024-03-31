using System;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace tool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Timers.Timer timer;
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            cb_top.Click += Cb_top_Click;
            txt_input.SelectionChanged += Txt_input_SelectionChanged; ;
            txt_key.TextChanged += Txt_key_TextChanged;
            txt_input.Focus();

            #region 大小写转换控制
            btn_low.Click += Btn_low_Click;
            btn_up.Click += Btn_up_Click;
            btn_up_low.Click += Btn_up_low_Click;
            btn_change.Click += Btn_change_Click;
            timer = new System.Timers.Timer();
            timer.Interval = 1000; // 设置定时器间隔为1秒
            timer.Elapsed += Timer_Elapsed; // 绑定定时器触发事件
            cb_monitor.Click += Cb_monitor_Click;
            #endregion
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                string str = Clipboard.GetText();
                if (str != txt_low.Text && str != txt_up.Text && str != txt_up_low.Text)
                {
                    txt_key.Text = str;
                }
            });
        }
        private void Cb_monitor_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.IsChecked == true)
            {
                // 复选框被选中时的逻辑
                timer.Start();
            }
            else
            {
                // 复选框未被选中时的逻辑
                timer.Stop();
            }
        }
        private void Txt_input_SelectionChanged(object sender, RoutedEventArgs e)
        {
            GetSelectedText();
        }
        private void GetSelectedText()
        {
            // 获取输入框中选中文本的起始位置和长度
            int selectionStart = txt_input.SelectionStart;
            int selectionLength = txt_input.SelectionLength;

            // 如果有选中的文本
            if (selectionLength > 0)
            {
                // 使用 Substring 方法从输入框的 Text 属性中提取选中的文本
                txt_s.Text = txt_input.Text.Substring(selectionStart, selectionLength);
            }
            else
            {
                Console.WriteLine("未选中文本。");
            }
        }
        private void Cb_top_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)cb_top.IsChecked)
            {
                this.Topmost = true;
            }
            else
            {
                this.Topmost = false;
            }
        }

        private void Btn_change_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string txt = txt_input.Text;
                string s = txt_s.Text;
                string n = txt_n.Text;
                string get = txt.Replace(s, n);
                txt_output.Text = get;
            }
            catch (Exception) { }
        }

        private void Btn_up_low_Click(object sender, RoutedEventArgs e)
        {
            string textToCopy = txt_up_low.Text;
            Clipboard.SetText(textToCopy);
        }

        private void Btn_up_Click(object sender, RoutedEventArgs e)
        {
            string textToCopy = txt_up.Text;
            Clipboard.SetText(textToCopy);
        }

        private void Btn_low_Click(object sender, RoutedEventArgs e)
        {
            string textToCopy = txt_low.Text;
            Clipboard.SetText(textToCopy);
        }

        private void Txt_key_TextChanged(object sender, TextChangedEventArgs e)
        {
            touplow();
            tolow();
            toup();
        }
        private void tolow()
        {
            try
            {
                string txt = txt_key.Text;
                string get = txt.ToLower();
                txt_low.Text = get;
            }
            catch (Exception) { }
        }
        private void toup()
        {
            try
            {
                string txt = txt_key.Text;
                string get = txt.ToUpper();
                txt_up.Text = get;
            }
            catch (Exception) { }
        }
        private void touplow()
        {
            try
            {
                string txt = txt_key.Text;
                string[] parts = txt.Split(' ');

                for (int i = 0; i < parts.Length; i++)
                {
                    parts[i] = CapitalizeFirstLetter(parts[i]);
                }

                string result = string.Join(" ", parts);
                txt_up_low.Text = result;
            }
            catch (Exception) { }
        }
        static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            char[] chars = input.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            return new string(chars);
        }
    }
}
