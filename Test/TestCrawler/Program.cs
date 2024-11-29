using System.Runtime;

namespace TestCrawler
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            decimal delay = 0.5m;
            var s = (int)(delay * 1000);

            //Console.OutputEncoding = System.Text.Encoding.UTF8;

            List<string> urls = new List<string>()
            {
                "https://snappshop.ir/product/snp-5031396673434366434",
                "https://snappshop.ir/product/snp-92036395",
                "https://snappshop.ir/product/snp-1672486627",
                "https://snappshop.ir/product/snp-503139667",
                "https://snappshop.ir/product/snp-1972082104",
                "https://snappshop.ir/product/snp-1454460930",
                "https://snappshop.ir/product/snp-368520028",
                "https://snappshop.ir/product/snp-1755035933",
                "https://snappshop.ir/product/snp-451791052",
                "https://snappshop.ir/product/snp-1746011470",
                "https://snappshop.ir/product/snp-1173353111",
                "https://snappshop.ir/product/snp-2105465185",
                "https://snappshop.ir/product/snp-988520349",
                "https://snappshop.ir/product/snp-201976360",
                "https://snappshop.ir/product/snp-1138093365sds",
                "https://snappshop.ir/product/snp-1138093365",
                "https://snappshop.ir/product/snp-2139308648",
                "https://snappshop.ir/product/snp-767406157",
                "https://snappshop.ir/product/snp-1026294369",
                "https://snappshop.ir/product/snp-1932302373",
                "https://snappshop.ir/product/snp-64011675",
                "https://snappshop.ir/product/snp-335238048",
                "https://snappshop.ir/product/snp-1008594955",
                "https://snappshop.ir/product/snp-617401709",
                "https://snappshop.ir/product/snp-1676062921",
                "https://snappshop.ir/product/snp-1241089781",
                "https://snappshop.ir/product/snp-1629502156",
                "https://snappshop.ir/product/snp-1118678053",
                "https://snappshop.ir/product/snp-342933197",
                "https://snappshop.ir/product/snp-1431117297",
                "https://snappshop.ir/product/snp-1327041642",
                "https://snappshop.ir/product/snp-903462880",
                "https://snappshop.ir/product/snp-1944414142"
            };
            
            Crawler.Infrastructure.Core2 core = new Crawler.Infrastructure.Core2();
            DateTime start = DateTime.Now;
            int batchSize = 10;
            List<Dictionary<string, string>> lst = new List<Dictionary<string, string>>();
            for (int index = 0; index < urls.Count; index += batchSize)
            {
                var linkBatchList = urls.Skip(index).Take(batchSize).ToList();
                var tasks = new List<Task<Dictionary<string, string>>>();
                foreach (var link in linkBatchList)
                {
                    tasks.Add(Task.Run(() => core.Crawl3(link)));
                }
                var result = await Task.WhenAll(tasks);
                lst.AddRange(result);
            }
            DateTime end = DateTime.Now;
            Console.Write(end - start);
            Console.WriteLine();

            foreach (var items in lst)
            {
                foreach (KeyValuePair<string, string> data in items)
                {
                    Console.WriteLine($"{data.Key} : {data.Value}");
                }
            }
            

            //var res = lst;
            Console.WriteLine();
            Console.WriteLine("finish");
            Console.ReadKey();
        }
    }
}
