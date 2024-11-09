using CrawlerConsole.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerConsole.Interfaces
{
    public interface IGoogleSheet
    {
        public Task<IList<IList<object>>?> GetGoogleSheetDataAsync();
        public Task<bool> WriteGoogleSheetDataAsync(string sheetName, IList<IList<object>> values);
        public Task<bool> CreateNewGoogleSheet(string title);
    }
}
