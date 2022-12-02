using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandelTSE.ViewModels;
using HandelTSE.Views;

namespace HandelTSE
{
    public partial class TransactionParkingViewModel
    {
        public List<items> Data { get; set; }
        List<items> it = new List<items>();
    public TransactionParkingViewModel()
    {

        for (int i=0; i< 30; i++) it.Add(new items { geparkt = "", gesamt = "", artikel = "", bediener = "" });
            Data = it;
            
        }

    }
    public class items
    {
        public string geparkt { get; set; }
        public string gesamt { get; set; }
        public string artikel { get; set; }
        public string bediener { get; set; }
    }
}
