using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using Monitor;

namespace GUI
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        WindowMonitor Monitor;

        public MainWindow()
        {
            InitializeComponent();
            Monitor = new WindowMonitor("config.json");
        }

        void Refresh_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Click");
        }


    }
    


}
