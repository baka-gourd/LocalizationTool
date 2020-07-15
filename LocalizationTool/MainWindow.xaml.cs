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
                try
                {
                    paths.Content = openFile.FileNames[0];
                    Path = openFile.FileName;
                    fileList.Visibility = Visibility.Collapsed;
                    @select.Visibility = Visibility.Collapsed;
                    cancel.Visibility = Visibility.Visible;
                }
                catch
                {
                    MessageBox.Show("请选择！");
                }
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
                        var fuckReg1 = new Regex("\n*\r");
                        var fuckReg2 = new Regex("//(.*)");
                        var fuckReg3 = new Regex("#(.*)");
                        var jobj = new JObject();
                        foreach (string str in File.ReadAllLines(Path, Encoding.UTF8))
                        {
                            if (fuckReg1.IsMatch(str))
                                break;
                            if (fuckReg2.IsMatch(str))
                                break;
                            if (fuckReg3.IsMatch(str))
                                break;
                            var key = keyReg.Match(str).ToString();
                            var name = nameReg.Match(str).ToString();
                            if (!jobj.Children().Contains(key))
                            {
                                jobj.Add(key, name);
                            }
                        }
                        var newPath = Path.Replace(".lang", ".json");
                        File.WriteAllText(newPath, jobj.ToString());
                        MessageBox.Show("完成！");
                    }


                }//单个转json,完成
                if (toLang.IsChecked == true)
                {
                    if (Path.Contains(".json"))
                    {
                        var jFile = File.OpenText(Path);
                        var reader = new JsonTextReader(jFile);
                        var jObj = (JObject)JToken.ReadFrom(reader);
                        List<string> langList = new List<string>();
                        foreach (var jValue in jObj)
                        {
                            langList.Add(jValue.Key.ToString() + "=" + jValue.Value.ToString());
                        }
                        var newPath = (Path.Replace(".json", ".lang"));
                        var sw = new StreamWriter(newPath, true, Encoding.UTF8);
                        foreach (var langStr in langList)
                        {
                            sw.WriteLine(langStr);
                        }
                        sw.Close();
                        MessageBox.Show("完成！");
                    }
                }//单个转lang,完成
            }
            else
            {
                if (toJson.IsChecked == true)
                {
                    var root = new DirectoryInfo(Path);
                    foreach (var f in root.GetFiles("*.lang"))
                    {
                        var keyReg = new Regex(".+(?==)");
                        var nameReg = new Regex("(?<==).+");
                        var fuckReg1 = new Regex("\n*\r");
                        var fuckReg2 = new Regex("//(.*)");
                        var fuckReg3 = new Regex("#(.*)");
                        var jobj = new JObject();
                        foreach (string str in File.ReadAllLines(Path + "/" + f.ToString(), Encoding.UTF8))
                        {
                            if (fuckReg1.IsMatch(str))
                                break;
                            if (fuckReg2.IsMatch(str))
                                break;
                            if (fuckReg3.IsMatch(str))
                                break;
                            var key = keyReg.Match(str).ToString();
                            var name = nameReg.Match(str).ToString();
                            if (!jobj.Children().Contains(key))
                            {
                                jobj.Add(key, name);
                            }
                        }
                        var newPath = (Path + "/" + f.ToString()).Replace(".lang", ".json");
                        File.WriteAllText(newPath, jobj.ToString());
                    }
                    MessageBox.Show("完成！");
                }//批量转json,完成
                if (toLang.IsChecked == true)
                {
                    var root = new DirectoryInfo(Path);
                    foreach (var f in root.GetFiles("*.json"))
                    {
                        var jFile = File.OpenText(Path + "/" + f.ToString());
                        var reader = new JsonTextReader(jFile);
                        var jObj = (JObject)JToken.ReadFrom(reader);
                        List<string> langList = new List<string>();
                        foreach (var jValue in jObj)
                        {
                            langList.Add(jValue.Key.ToString() + "=" + jValue.Value.ToString());
                        }
                        var newPath = (Path + "/" + f.ToString()).Replace(".json", ".lang");
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
