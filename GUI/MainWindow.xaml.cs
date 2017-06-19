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
using Persistence;

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

            Analyzer ana = new Analyzer();

            Console.WriteLine("Cantidad total {0}", ana.CountAll());
            Console.WriteLine("Cantidad hoy {0}", ana.CountToday());
            Console.WriteLine("Cantidad esta hora {0}", ana.CountSince(DateTime.Now.AddHours(-1)));

            Console.WriteLine("mas cambios:");
            List<Tuple<string, int>> mostSwitched = ana.MostSwitchedProcess();            

            for (int i=0; i < mostSwitched.Count; i++)
            {
                Console.WriteLine(mostSwitched[i]);
            }

            Console.WriteLine();
            Console.WriteLine("mas uso en tiempo total:");
            List<Tuple<string, int>> mostUsed = ana.MostUsedProcess();

            for (int i = 0; i < mostUsed.Count; i++)
            {
                Console.WriteLine(mostUsed[i]);
            }


        }

        void Refresh_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Click");
            Monitor.ForcePersistData();
        }


    }
    


}
