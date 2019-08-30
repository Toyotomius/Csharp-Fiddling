using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Misc
{
    public class USPowerballJsonCreation
    {
        private readonly List<LottoData> _lottoData = new List<LottoData>();

        public void CreateJson()
        {
            var rawFileLines = File.ReadAllLines(@"E:\Misc\USPowerballRAW");

            var numList = new List<int[]>();
            var dateList = new List<string>();
            
            var bonus = new List<int>();

            for (var i = 0; i < rawFileLines.Length; i++)
            {
                if (i % 2 != 0 && i != 0)
                {
                    numList.Add(rawFileLines[i].Split(' ').Select(n => Convert.ToInt32(n)).SkipLast(1).ToArray());
                    bonus.Add(rawFileLines[i].Split(' ').Select(n => Convert.ToInt32(n)).Last<int>());
                }
                else
                {
                    
                    dateList.Add(rawFileLines[i]);
                }
            }

            for (var i = 0; i < numList.Count; i++)
            {
                _lottoData.Add(new LottoData
                {
                    Date = dateList[i],
                    Numbers = numList[i],
                    Bonus = bonus[i]
                });
            }
            var _lottoMax = new USPowerballProp { USPowerball = _lottoData };
            var json = JsonConvert.SerializeObject(_lottoMax, Formatting.Indented);

            File.WriteAllText(@"E:\Misc\USPowerball.json", json);
        }

        public class LottoData
        {

            public string Date { get; set; }

            public int[] Numbers { get; set; }
            public int Bonus { get; set; }
        }

        public class USPowerballProp
        {
            public List<LottoData> USPowerball { get; set; }
        }
    }
}
