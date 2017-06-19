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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GUI
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        WindowMonitor Monitor;
        Analyzer ana;            

        int _totalCount;
        int _todayCount;
        int _hourCount;

        public int TotalCount
        {
            get
            {
                return _totalCount;
            }
            set
            {
                _totalCount = value;
                NotifyPropertyChanged();
            }
        }

        public int TodayCount
        {
            get
            {
                return _todayCount;
            }
            set
            {
                _todayCount = value;
                NotifyPropertyChanged();
            }
        }

        public int HourCount
        {
            get
            {
                return _hourCount;
            }
            set
            {
                _hourCount = value;
                NotifyPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            Monitor = new WindowMonitor("config.json");
            ana = new Analyzer();
            RefreshData();
            DataContext = this;
        }

        void BuildPieChart(PieChart chart, List<Tuple<string, int>> data)
        {
            chart.PieCollection.Clear();

            foreach (Tuple<string, int> tuple in data)
            {
                chart.PieCollection.Add(new PieSeries
                {
                    Title = tuple.Item1,
                    Values = new ChartValues<double> { tuple.Item2 },
                    DataLabels = true,
                    LabelPoint = SwitchPieChart.PointLabel
                });
            }
        }

        void RefreshData()
        {
            TotalCount = ana.CountAll();
            TodayCount = ana.CountToday();
            HourCount = ana.CountSince(DateTime.Now.AddHours(-1));
            BuildPieChart(SwitchPieChart, ana.MostSwitchedProcess());
            BuildPieChart(MostUsedChart, ana.MostUsedProcess());
        }


        void Refresh_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Click");
            Monitor.ForcePersistData();
            RefreshData();
        }



        public event PropertyChangedEventHandler PropertyChanged;
        void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
