using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Lottery_Regex
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var rawFile = "";
            using (var sr = new StreamReader(@"E:\Misc\649BonusHTML.txt"))
            {
                rawFile = sr.ReadToEnd();
            }

            var regPattern = @"\d+(?=</b></b)";
            var reg = new Regex(regPattern);
            MatchCollection matches = reg.Matches(rawFile);

            var matchList = new List<int>();

            foreach (Match m in matches)
            {
                var numValue = int.Parse(m.Value);
                matchList.Add(numValue);
            }

            string lottoFile = "";
            using (var sr = new StreamReader(@"E:\Misc\Lotto649.json"))
            {
                lottoFile = sr.ReadToEnd();
            }

            var lottoData = JsonConvert.DeserializeObject<LottoData>(lottoFile);

            for (var i = 0; i < matchList.Count; i++)
            {
                lottoData.Lotto649[i].Numbers.Add(matchList[i]);
            }

            File.WriteAllText(@"E:\Misc\Lotto649Bonus.json", JsonConvert.SerializeObject(lottoData));
        }

        public class Data
        {
            public string Date { get; set; }
            public List<int> Numbers { get; set; }
        }

        public class LottoData
        {
            public List<Data> Lotto649 { get; set; }
        }
    }
}
