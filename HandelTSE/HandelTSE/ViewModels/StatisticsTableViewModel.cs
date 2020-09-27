using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.DataVisualization.Charting;

namespace HandelTSE.ViewModels
{
    class StatisticsTableViewModel
    {
        private void LoadScatterChartData()
        {
            ((ScatterSeries)mcChart.Series[0]).ItemsSource = new KeyValuePair<DateTime, int>[]{
        new KeyValuePair<DateTime,int>(DateTime.Now, 100),
        new KeyValuePair<DateTime,int>(DateTime.Now.AddMonths(1), 130),
        new KeyValuePair<DateTime,int>(DateTime.Now.AddMonths(2), 150),
        new KeyValuePair<DateTime,int>(DateTime.Now.AddMonths(3), 125)
        };
        }
    }
}
