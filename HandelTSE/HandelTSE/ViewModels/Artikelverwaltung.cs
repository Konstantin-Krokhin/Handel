﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandelTSE.ViewModels;

namespace HandelTSE.ViewModels
{
    class Artikelverwaltung
    {
        public List<items> Data { get; set; }
        List<items> it = new List<items>();
        public Artikelverwaltung()
        {
            for (int i = 0; i < 30; i++) it.Add(new items { titel = "", geld = "" });
            Data = it;
        }
        public class items
        {
            public string titel { get; set; }
            public string geld { get; set; }
        }
    }
    
}
