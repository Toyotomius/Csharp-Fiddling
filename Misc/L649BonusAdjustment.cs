using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Misc
{
    internal class L649BonusAdjustment
    {
        /// <summary>
        /// I screwed up when planning the bonus numbers for the 649 lottery.
        /// Decided having the number as the last part of each number array was a bad idea
        /// and would throw off calculations. This script is designed to go back through the json
        /// and grab the bonus numbers, then output them to their own json object.
        /// Probably inefficient and terrible but I was annoyed and this worked.
        /// </summary>
        /// <param name="args"></param>
        private static void BonusAdjustment()
        {
            string json;

            using (var sr = new StreamReader(@"E:\Misc\Lotto649Bonus.json"))
            {
                json = sr.ReadToEnd();
            }

            LottoData lottoData = JsonConvert.DeserializeObject<LottoData>(json);

            var bonus = new List<int>();
            for (var i = 0; i < lottoData.Lotto649.Count; i++)
            {
                var bonusNum = lottoData.Lotto649[i].Numbers[6];
                bonus.Add(bonusNum);
            }

            var regexPattern = @"]";
            var reg = new Regex(regexPattern);

            var lottoFile = File.ReadAllLines(@"E:\Misc\Lotto649Bonus.json");
            var sb = new StringBuilder();

            var y = 0;

            for (var x = 0; x < lottoFile.Length; x++)
            {                                                       // Just a one off script. This magic number represents the last ] in the json file
                                                                    // which doesn't need to be matched and will be out of index range for bonus list.
                if (Regex.IsMatch(lottoFile[x], regexPattern) && x <= 44401)
                {
                    y = (x - 1) / 12;
                    // Using notepad++ I determined that there would be a consistent 12 index gap between matches.
                    // The equation for this is y = 12x+1. Solve for x is (y-1)/12. Subbed normal index variables (i, y, etc) for math logic.
                    var str = $"],\n      \"Bonus\" : {bonus[y] }";
                    sb.Append(reg.Replace(lottoFile[x], str));
                }
                else
                {
                    sb.Append(lottoFile[x]);
                }
            }

            using (var sw = new StreamWriter(@"E:\Misc\Test.json"))
            {
                sw.WriteLine(sb.ToString());
            }
        }

        // Note: All other required changes will be done with a simple find/replace in notepad++.
        // (I had to math >.<;;)
        // (Full honesty, took me too long to figure out the basic equation.
        // Did so many as a kid shoulda had it in a few seconds. Gods I need to remember how to math.)

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
