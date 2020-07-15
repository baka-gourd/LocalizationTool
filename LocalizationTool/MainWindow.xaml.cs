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
        public string RawPathTo { get; set; }
        public string RawPathUpgradeLow { get; set; }
        public string RawPathUpgradeHigh { get; set; }
        public string RawPathUpgradeAdv { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            FileList.Visibility = Visibility.Collapsed;
            Cancel.Visibility = Visibility.Collapsed;
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
                    Paths.Content = openFile.FileNames[0];
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
                    Paths.Content = openFolder.SelectedPath;
                    RawPathTo = openFolder.SelectedPath;
                    FileList.Visibility = Visibility.Visible;
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
                    Doto.Width = 615;
                }
                catch
                {
                    MessageBox.Show("请选择！");
                }
            }
            else
            {
                Paths.Content = "请勾选！";
            }
        }//选择
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Select.Visibility = Visibility.Visible;
            Paths.Content = "";
            RawPathTo = "";
            Cancel.Visibility = Visibility.Collapsed;
            FileList.Visibility = Visibility.Collapsed;
            Doto.Width = 764;
        }//取消

        private void Doto_Click(object sender, RoutedEventArgs e)
        {
            if (System.IO.File.Exists(RawPathTo))
            {
                if (ToJson.IsChecked == true)
                {
                    if (RawPathTo.Contains(".lang"))
                    {
                        var keyReg = new Regex(".+(?==)");
                        var nameReg = new Regex("(?<==).+");
                        var fuckReg1 = new Regex("\n*\r");
                        var fuckReg2 = new Regex("//(.*)");
                        var fuckReg3 = new Regex("#(.*)");
                        var fuckReg4 = new Regex("^( \\*)");
                        var fuckReg5 = new Regex("^(/\\*)");
                        var jobj = new JObject();
                        foreach (string str in System.IO.File.ReadAllLines(RawPathTo, Encoding.UTF8))
                        {
                            if (fuckReg1.IsMatch(str))
                                continue;
                            if (fuckReg2.IsMatch(str))
                                continue;
                            if (fuckReg3.IsMatch(str))
                                continue;
                            if (fuckReg4.IsMatch(str))
                                continue;
                            if (fuckReg5.IsMatch(str))
                                continue;
                            var key = keyReg.Match(str).ToString();
                            var name = nameReg.Match(str).ToString();
                            if (key == "" && name == "")
                                continue;
                            if (!jobj.TryGetValue(key, out var rubbish1))
                            {
                                jobj.Add(key, name);
                            }
                        }
                        var newPath = RawPathTo.Replace(".lang", ".json");
                        System.IO.File.WriteAllText(newPath, jobj.ToString());
                        MessageBox.Show("完成！");
                    }


                }//单个转json,完成
                if (ToLang.IsChecked == true)
                {
                    if (RawPathTo.Contains(".json"))
                    {
                        var jFile = System.IO.File.OpenText(RawPathTo);
                        var reader = new JsonTextReader(jFile);
                        var jObj = (JObject)JToken.ReadFrom(reader);
                        List<string> langList = new List<string>();
                        foreach (var jValue in jObj)
                        {
                            langList.Add(jValue.Key + "=" + jValue.Value);
                        }
                        var newPath = (RawPathTo.Replace(".json", ".lang"));
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
                if (ToJson.IsChecked == true)
                {
                    var root = new DirectoryInfo(RawPathTo);
                    foreach (var f in root.GetFiles("*.lang"))
                    {
                        var keyReg = new Regex(".+(?==)");
                        var nameReg = new Regex("(?<==).+");
                        var fuckReg1 = new Regex("\n*\r");
                        var fuckReg2 = new Regex("//(.*)");
                        var fuckReg3 = new Regex("#(.*)");
                        var jobj = new JObject();
                        foreach (string str in System.IO.File.ReadAllLines(RawPathTo + "/" + f, Encoding.UTF8))
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
                        var newPath = (RawPathTo + "/" + f).Replace(".lang", ".json");
                        System.IO.File.WriteAllText(newPath, jobj.ToString());
                    }
                    MessageBox.Show("完成！");
                }//批量转json,完成
                if (ToLang.IsChecked == true)
                {
                    var root = new DirectoryInfo(RawPathTo);
                    foreach (var f in root.GetFiles("*.json"))
                    {
                        var jFile = System.IO.File.OpenText(RawPathTo + "/" + f);
                        var reader = new JsonTextReader(jFile);
                        var jObj = (JObject)JToken.ReadFrom(reader);
                        List<string> langList = new List<string>();
                        foreach (var jValue in jObj)
                        {
                            langList.Add(jValue.Key + "=" + jValue.Value);
                        }
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

        private void Upgradeto113_Click(object sender, RoutedEventArgs e)
        {
            if (IsAdv.IsChecked != true)
            {
                var keyReg = new Regex(".+(?==)");
                var nameReg = new Regex("(?<==).+");
                var fuckReg1 = new Regex("\n*\r");
                var fuckReg2 = new Regex("//(.*)");
                var fuckReg3 = new Regex("#(.*)");
                var fuckReg4 = new Regex("^( \\*)");
                var fuckReg5 = new Regex("^(/\\*)");
                var jFile = System.IO.File.OpenText(RawPathUpgradeHigh);
                var reader = new JsonTextReader(jFile);
                var jObj = (JObject)JToken.ReadFrom(reader);
                var langDict = new Dictionary<string, string>();
                foreach (string str in System.IO.File.ReadAllLines(RawPathUpgradeLow, Encoding.UTF8))
                {
                    if (fuckReg1.IsMatch(str))
                        continue;
                    if (fuckReg2.IsMatch(str))
                        continue;
                    if (fuckReg3.IsMatch(str))
                        continue;
                    if (fuckReg4.IsMatch(str))
                        continue;
                    if (fuckReg5.IsMatch(str))
                        continue;
                    var key = keyReg.Match(str).ToString();
                    var name = nameReg.Match(str).ToString();
                    if (key == "" && name == "")
                        continue;
                    if (!langDict.TryGetValue(key, out var rubbish2))
                        langDict.Add(key, name);
                }
                foreach (var jT in jObj)
                {
                    var hasKey = langDict.TryGetValue(jT.Key, out string resultName);
                    if (hasKey)
                        jObj[jT.Key] = resultName;
                }
                var newOutPath = RawPathUpgradeHigh.Replace(".json", "_out.json");
                System.IO.File.WriteAllText(newOutPath, jObj.ToString());
                MessageBox.Show("完成！");
            }
            else
            {
                var keyReg = new Regex(".+(?==)");
                var nameReg = new Regex("(?<==).+");
                var fuckReg1 = new Regex("\n*\r");
                var fuckReg2 = new Regex("//(.*)");
                var fuckReg3 = new Regex("#(.*)");
                var fuckReg4 = new Regex("^( \\*)");
                var fuckReg5 = new Regex("^(/\\*)");
                var jFile = System.IO.File.OpenText(RawPathUpgradeHigh);
                var reader = new JsonTextReader(jFile);
                var jObj = (JObject)JToken.ReadFrom(reader);
                var langDictChs = new Dictionary<string, string>();
                var langDictEng = new Dictionary<string, string>();
                foreach (string str in System.IO.File.ReadAllLines(RawPathUpgradeLow, Encoding.UTF8))
                {
                    if (fuckReg1.IsMatch(str))
                        continue;
                    if (fuckReg2.IsMatch(str))
                        continue;
                    if (fuckReg3.IsMatch(str))
                        continue;
                    if (fuckReg4.IsMatch(str))
                        continue;
                    if (fuckReg5.IsMatch(str))
                        continue;
                    var key = keyReg.Match(str).ToString();
                    var name = nameReg.Match(str).ToString();
                    if (key == "" && name == "")
                        continue;
                    if (!langDictChs.TryGetValue(key, out var rubbish2))
                        langDictChs.Add(key, name);
                }
                foreach (string str in System.IO.File.ReadAllLines(RawPathUpgradeAdv, Encoding.UTF8))
                {
                    if (fuckReg1.IsMatch(str))
                        continue;
                    if (fuckReg2.IsMatch(str))
                        continue;
                    if (fuckReg3.IsMatch(str))
                        continue;
                    if (fuckReg4.IsMatch(str))
                        continue;
                    if (fuckReg5.IsMatch(str))
                        continue;
                    var key = keyReg.Match(str).ToString();
                    var name = nameReg.Match(str).ToString();
                    if (key == "" && name == "")
                        continue;
                    if (!langDictEng.TryGetValue(key, out var rubbish3))
                        langDictEng.Add(key, name);
                }
                foreach (var jT in jObj)
                {
                    var keySelect = langDictEng.FirstOrDefault(i => i.Value == jT.Value.ToString()).Key;
                    if (keySelect == null)
                        continue;
                    var hasKey = langDictChs.TryGetValue(keySelect, out string resultName);
                    if (hasKey)
                        jObj[jT.Key] = resultName;
                }
                var newOutPath = RawPathUpgradeHigh.Replace(".json", "_out.json");
                System.IO.File.WriteAllText(newOutPath, jObj.ToString());
                MessageBox.Show("完成！");
            }
        }//低->高，完成

        private void Low_Click(object sender, RoutedEventArgs e)
        {
            var openFile = new OpenFileDialog
            {
                Multiselect = false,
                Title = "请选择：",
                Filter = "Minecraft语言文件(*.lang)|*.lang"
            };
            openFile.ShowDialog();
            try
            {
                RawPathUpgradeLow = openFile.FileName;
                LowLabel.Content = openFile.FileName;
            }
            catch
            {
                MessageBox.Show("请选择！");
            }
        }//选择低版本，完成

        private void High_Click(object sender, RoutedEventArgs e)
        {
            var openFile = new OpenFileDialog
            {
                Multiselect = false,
                Title = "请选择：",
                Filter = "Minecraft语言文件(*.json)|*.json"
            };
            openFile.ShowDialog();
            try
            {
                RawPathUpgradeHigh = openFile.FileName;
                HighLabel.Content = openFile.FileName;
            }
            catch
            {
                MessageBox.Show("请选择！");
            }
        }//选择高版本，完成

        private void Adv_Click(object sender, RoutedEventArgs e)
        {
            var openFile = new OpenFileDialog
            {
                Multiselect = false,
                Title = "请选择：",
                Filter = "Minecraft语言文件(*.lang)|*.lang"
            };
            openFile.ShowDialog();
            try
            {
                RawPathUpgradeAdv = openFile.FileName;
            }
            catch
            {
                MessageBox.Show("请选择！");
            }
        }//选择高级，完成

        private void IsAdv_Checked(object sender, RoutedEventArgs e)
        {
            if (RawPathUpgradeAdv == null)
            {
                MessageBox.Show("请选择高级文件！");
            }
        }
    }
}
