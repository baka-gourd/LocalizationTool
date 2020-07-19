using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace LocalizationTool
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class LangtoJson : Window
    {
        public string RawPathTo { get; set; }
        public LangtoJson()
        {
            InitializeComponent();
            FileList.Visibility = Visibility.Collapsed;
            Cancel.Visibility = Visibility.Collapsed;
            ListName.Visibility = Visibility.Collapsed;
            BorderList.Visibility = Visibility.Collapsed;
        }
        private void Select_Click(object sender, RoutedEventArgs e)
        {
            if (File.IsChecked == true)
            {
                var openFile = new OpenFileDialog
                {
                    Multiselect = false,
                    Title = "请选择：",
                    Filter = "Minecraft语言文件(*.lang,*.json)|*.lang;*.json"
                };
                openFile.ShowDialog();
                try
                {
                    Paths.Content = "√";
                    RawPathTo = openFile.FileName;
                    FileList.Visibility = Visibility.Collapsed;
                    Select.Visibility = Visibility.Collapsed;
                    Cancel.Visibility = Visibility.Visible;
                }
                catch
                {
                    MessageBox.Show("请选择！");
                }
            }
            else if (Folder.IsChecked == true)
            {
                var openFolder = new FolderBrowserDialog
                {
                    ShowNewFolderButton = false,
                    Description = @"请选择需要批量处理的文件夹："
                };
                openFolder.ShowDialog();
                try
                {
                    Paths.Content = "√";
                    RawPathTo = openFolder.SelectedPath;
                    L2J.Height = 459;
                    FileList.Visibility = Visibility.Visible;
                    ListName.Visibility = Visibility.Visible;
                    BorderList.Visibility = Visibility.Visible;
                    var root = new DirectoryInfo(RawPathTo);
                    foreach (var file in root.GetFiles("*.json"))
                    {
                        FileList.Items.Add(file.Name);
                    }
                    foreach (var file in root.GetFiles("*.lang"))
                    {
                        FileList.Items.Add(file.Name);
                    }
                    Select.Visibility = Visibility.Collapsed;
                    Cancel.Visibility = Visibility.Visible;
                }
                catch
                {
                    MessageBox.Show("请选择！");
                }
            }
            else
            {
                Paths.Content = "请选择！";
            }
        }//选择
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Select.Visibility = Visibility.Visible;
            Paths.Content = "";
            RawPathTo = "";
            Cancel.Visibility = Visibility.Collapsed;
            FileList.Visibility = Visibility.Collapsed;
        }//取消

        private void Doto_Click(object sender, RoutedEventArgs e)
        {
            if (System.IO.File.Exists(RawPathTo))
            {

                if (RawPathTo.Contains(".lang"))
                {
                    var langJObject = Process.ParseLangFile(RawPathTo);
                    var newPath = RawPathTo.Replace(".lang", ".json");
                    System.IO.File.WriteAllText(newPath, langJObject.ToString());
                    MessageBox.Show("完成！输出到"+ newPath);
                } //单个转json,完成
                if (ToLang.IsChecked == true)
                {
                    var langList = Process.ParseJsonFile(RawPathTo);
                    var newPath = (RawPathTo.Replace(".json", ".lang"));
                    var sw = new StreamWriter(newPath, true, Encoding.UTF8);
                    foreach (var langStr in langList)
                    {
                        sw.WriteLine(langStr);
                    }
                    sw.Close();
                    MessageBox.Show("完成！输出到"+ newPath);
                }//单个转lang,完成
            }
            else
            {
                if (ToJson.IsChecked == true)
                {
                    var root = new DirectoryInfo(RawPathTo);
                    foreach (var f in root.GetFiles("*.lang"))
                    {
                        var langJObject = Process.ParseLangFile(RawPathTo + "/" + f);
                        var newPath = (RawPathTo + "/" + f).Replace(".lang", ".json");
                        System.IO.File.WriteAllText(newPath, langJObject.ToString());
                    }
                    MessageBox.Show("完成！");
                }//批量转json,完成
                if (ToLang.IsChecked == true)
                {
                    var root = new DirectoryInfo(RawPathTo);
                    foreach (var f in root.GetFiles("*.json"))
                    {
                        var langList = Process.ParseJsonFile(RawPathTo + "/" + f);
                        var newPath = (RawPathTo + "/" + f).Replace(".json", ".lang");
                        var sw = new StreamWriter(newPath, true, Encoding.UTF8);
                        foreach (var langStr in langList)
                        {
                            sw.WriteLine(langStr);
                        }
                        sw.Close();
                    }
                    MessageBox.Show("完成！");
                    //批量转lang,完成
                }
            }
        }
    }
}
