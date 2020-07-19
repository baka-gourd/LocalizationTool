using System.Windows.Input;

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
