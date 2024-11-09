using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerConsole.Domain
{
    public class SettingsModel
    {
        public required GoogleSheetModel GoogleSheet { get; set; }
        //public required CalculatorModel Calculator { get; set; }
        //public required string ViravinNameInSnappShop { get; set; }

    }

    #region Google Sheet
    public class GoogleSheetModel
    {
        public required string SpreadId { get; set; }
        public required string Name { get; set; }
        public required string ColumnRange { get; set; }
        public required int StartIndex { get; set; }

        public required GoogleSheetColumnsIndexModel ColumnIndex { get; set; }
    }
    public class GoogleSheetColumnsIndexModel
    {
        public required int SnappShopId { get; set; }
        public required int SnappShopLink { get; set; }
        public required int SnappShopColor { get; set; }
        public required int Mobile140Link { get; set; }
        public required int Mobile140Color { get; set; }
        public required int GooshiShopLink { get; set; }
        public required int GooshiShopColor { get; set; }
    }
    #endregion
}
