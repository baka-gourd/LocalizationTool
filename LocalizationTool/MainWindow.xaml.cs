using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using Newtonsoft.Json;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace LocalizationTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowLangtoJson_Click(object sender, RoutedEventArgs e)
        {
            var l2J = new LangtoJson();
            l2J.Show();
        }

        private void ShowUpgrade_Click(object sender, RoutedEventArgs e)
        {
            var upgrade = new Upgrade();
            upgrade.Show();
        }
    }
}
