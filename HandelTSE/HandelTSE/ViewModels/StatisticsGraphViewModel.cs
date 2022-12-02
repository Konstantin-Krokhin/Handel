using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandelTSE.ViewModels
{
    class StatisticsGraphViewModel
    {
        public List<Client> Data { get; set; }

        public StatisticsGraphViewModel()
        {
            Data = new List<Client>()
            {
                new Client { data1 = "R1", data2 = 80 },
                new Client { data1 = "R2", data2 = 70 },
                new Client { data1 = "Z3", data2 = 60 },
                new Client { data1 = "Z4", data2 = 82 },
                new Client { data1 = "Z5", data2 = 80 },
                new Client { data1 = "Z6", data2 = 70 },
                new Client { data1 = "Z7", data2 = 60 },
            };
        }
    }
    public class Client
    {
        public string data1 { get; set; }

        public double data2 { get; set; }
    }
}
