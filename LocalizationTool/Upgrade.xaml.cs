using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MessageBox = System.Windows.MessageBox;

namespace LocalizationTool
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Upgrade
    {
        public string RawPathUpgradeLow { get; set; }
        public string RawPathUpgradeHigh { get; set; }
        public string RawPathUpgradeAdv { get; set; }
        public Upgrade()
        {
            InitializeComponent();
        }


        private void UpgradeTo_Click(object sender, RoutedEventArgs e)
        {
            if (RawPathUpgradeAdv == "")
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
                    if (!langDict.TryGetValue(key, out _))
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
                    if (!langDictChs.TryGetValue(key, out _))
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
                    if (!langDictEng.TryGetValue(key, out _))
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

        private void LangZh_Click(object sender, RoutedEventArgs e)
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
                LabelLangZh.Content = "√";
            }
            catch
            {
                MessageBox.Show("请选择！");
            }
        }//选择低版本，完成

        private void JsonEn_Click(object sender, RoutedEventArgs e)
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
                LabelJsonEn.Content = "√";
            }
            catch
            {
                MessageBox.Show("请选择！");
            }
        }//选择高版本，完成

        private void LangEn_Click(object sender, RoutedEventArgs e)
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
                LabelLangEn.Content = "√";
            }
            catch
            {
                MessageBox.Show("请选择！");
            }
        }//选择高级，完成
    }
}
