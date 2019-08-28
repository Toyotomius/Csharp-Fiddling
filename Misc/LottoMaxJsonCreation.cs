using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Misc
{
    public class LottoMaxJsonCreation
    {
        private readonly List<LottoData> _lottoData = new List<LottoData>();

        public void CreateJson()
        {
            var rawFileLines = File.ReadAllLines(@"E:\Misc\LottoMaxRAW.txt");

            var numList = new List<int[]>();
            var dateList = new List<string>();
            var regPat = @"\s(?=\d{3,})";
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
                    var temp = Regex.Replace(rawFileLines[i], regPat, ", ");
                    dateList.Add(temp);
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
            var _lottoMax = new LottoMaxProp { LottoMax = _lottoData };
            var json = JsonConvert.SerializeObject(_lottoMax, Formatting.Indented);

            File.WriteAllText(@"E:\Misc\LottoMax.json", json);
        }

        public class LottoData
        {
            
            public string Date { get; set; }

            public int[] Numbers { get; set; }
            public int Bonus { get; set; }
        }

        public class LottoMaxProp
        {
            public List<LottoData> LottoMax { get; set; }
        }
    }
}
