using System;
using System.Collections.Generic;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using Monitor;
using Persistence;
using System.Diagnostics;

namespace GUI
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        WindowMonitor Monitor;
        Analyzer ana;            

        public MainWindow()
        {
            InitializeComponent();

            StartTime.SelectedDate = DateTime.Now.AddDays(-1);
            EndTime.SelectedDate = DateTime.Now.AddDays(1);

            Monitor = new WindowMonitor("config.json");
            ana = new Analyzer();
            RefreshData();
            RefreshButton.ToolTip = "Reload data analysis";
            DataContext = this;

            /////////////
            // Asserts //
            /////////////

            int total = 0;
            var hoy_mas_cambiados = ana.MostSwitchedProcess(DateTime.Now.AddHours(-1), DateTime.Now);
            foreach(Tuple<string, int> t in hoy_mas_cambiados)
            {
                Console.WriteLine(t);
                total += t.Item2;
            }
            Console.WriteLine("total: " + total);
            Debug.Assert(total == ana.CountSince(DateTime.Now.AddHours(-1)));

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
            Stats.ShowData();

            DateTime start = (DateTime)StartTime.SelectedDate;
            DateTime end = (DateTime)EndTime.SelectedDate;

            var mostSwitchedProcess = ana.MostSwitchedProcess(start, end);
            var mostUsedProcess = ana.MostUsedProcess(start,end);

            BuildPieChart(SwitchPieChart, mostSwitchedProcess);
            BuildPieChart(MostUsedChart, mostUsedProcess);
        }


        void Refresh_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Click");
            Monitor.ForcePersistData();
            RefreshData();
        }


    }
}
