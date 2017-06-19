using Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace GUI
{       
    /// <summary>
    /// Interaction logic for Stats.xaml
    /// </summary>
    public partial class Stats : UserControl, INotifyPropertyChanged
    {

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

        Analyzer ana;


        public Stats()
        {
            InitializeComponent();
            ana = new Analyzer();
            DataContext = this;
        }

        public void ShowData()
        {
            TotalCount = ana.CountAll();
            TodayCount = ana.CountToday();
            HourCount = ana.CountSince(DateTime.Now.AddHours(-1));
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
