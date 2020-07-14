using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace LocalizationTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public string Path { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            fileList.Visibility = Visibility.Collapsed;
            cancel.Visibility = Visibility.Collapsed;
        }
        //选择
        private void Select_Click(object sender, RoutedEventArgs e)
        {
            if (file.IsChecked == true)
            {
                var openFile = new OpenFileDialog
                {
                    Multiselect = false,
                    Title = "请选择：",
                    Filter = "Minecraft语言文件(*.lang,*.json)|*.lang;*.json"
                };
                openFile.ShowDialog();
                paths.Content = openFile.FileNames[0];
                Path = openFile.FileName;
                fileList.Visibility = Visibility.Collapsed;
                @select.Visibility = Visibility.Collapsed;
                cancel.Visibility = Visibility.Visible;
            }
            else if (folder.IsChecked == true)
            {
                var openFolder = new FolderBrowserDialog
                {
                    ShowNewFolderButton = false,
                    Description = "请选择需要批量处理的文件夹："
                };
                openFolder.ShowDialog();
                paths.Content = openFolder.SelectedPath;
                Path = openFolder.SelectedPath;
                fileList.Visibility = Visibility.Visible;
                var root = new DirectoryInfo(Path);
                foreach (var file in root.GetFiles("*.json"))
                {
                    fileList.Items.Add(file.Name);
                }
                foreach (var file in root.GetFiles("*.lang"))
                {
                    fileList.Items.Add(file.Name);
                }
                @select.Visibility = Visibility.Collapsed;
                cancel.Visibility = Visibility.Visible;
                Doto.Width = 615;
            }
            else
            {
                paths.Content = "请勾选！";
            }
        }

        //取消
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            @select.Visibility = Visibility.Visible;
            paths.Content = "";
            Path = "";
            cancel.Visibility = Visibility.Collapsed;
            fileList.Visibility = Visibility.Collapsed;
            Doto.Width = 764;
        }

        private void Doto_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Path))
            {
                if (toJson.IsChecked == true)
                {
                    if (Path.Contains(".lang"))
                    {
                        var keyReg = new Regex(".+(?==)");
                        var nameReg = new Regex("(?<==).+");
                        var jobj = new JObject();
                        foreach (string str in File.ReadAllLines(Path, Encoding.UTF8))
                        {
                            var key = keyReg.Match(str).ToString();
                            var name = nameReg.Match(str).ToString();
                            jobj.Add(key,name);
                        }
                        var newPath = Path.Replace(".lang", ".json");
                    }


                }
                else if (toLang.IsChecked == true)
                {
                    
                }
            }
            else
            {
                if (toJson.IsChecked == true)
                {

                }
                else if (toLang.IsChecked == true)
                {

                }
            }
        }


    }
}
