using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Newtonsoft.Json;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace LocalizationTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// 800宽
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowLangtoJson(object sender, MouseButtonEventArgs e)
        {
            var l2J = new LangtoJson();
            l2J.Show();
        }

        private void ShowUpgrade(object sender, MouseButtonEventArgs e)
        {
            var upgrade = new Upgrade();
            upgrade.Show();
        }

        private void ExtractJar(object sender, MouseButtonEventArgs e)
        {
            var ej = new ExtractJar();
            ej.Show();
        }
    }
}
