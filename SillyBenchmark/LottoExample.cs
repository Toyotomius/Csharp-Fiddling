using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SillyBenchmark
{
    public class LottoExample
    {
        public class LottoData
        {
            public List<Data> Lotto649 { get; set; }
        }

        public class Data
        {
            public string Date { get; set; }

            public List<int> Numbers { get; set; }
        }
        public static List<int> DummyLock = new List<int>();
        public void LEAsync()
        {
            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();

            string lottoFile = "Lotto649.json";
            string outputFile = "ExampleSingles.txt";
            string json = File.ReadAllText(lottoFile);

            var lottoData = JsonConvert.DeserializeObject<LottoData>(json);

            var singlesList = (from n in lottoData.Lotto649.SelectMany(x => x.Numbers)
                group n by n
                into g
                orderby g.Count() descending
                select new LottoSinglesCount {Number = g.Key, Count = g.Count()}).ToList();

            
            watch.Stop();
            Console.WriteLine($"Example Execution Time: {watch.ElapsedMilliseconds} ms");
            WriteExample(singlesList);
        }

        public void WriteExample(List<LottoSinglesCount> singlesList)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < singlesList.Count; i++)
            {
             
                sb.AppendLine($"Number: {singlesList[i].Number}, Frequency: {singlesList[i].Count}");
            }

            string outputFile = "ExampleSingles.txt";
            lock (DummyLock)
            {
                using (StreamWriter sw = new StreamWriter(outputFile, append: true))
                {
                    sw.Write(sb);
                }
            }
        }
    }
}