using HandelTSE.Views;
using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.DataVisualization.Charting;


namespace HandelTSE.ViewModels
{
    class StatTableModel
    {
        public List<Client> Data { get; set; }

        public StatTableModel()
        {
            Data = new List<Client>()
            {
                new Client { data1 = "R1", data2 = 80 },
                new Client { data1 = "R2", data2 = 70 },
                new Client { data1 = "Z3", data2 = 60 },
                new Client { data1 = "Z4", data2 = 82 },
            };
        }
    }
    
}
