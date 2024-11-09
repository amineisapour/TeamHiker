using CrawlerConsole.Domain;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerConsole.Mapper
{
    public static class AllTrailsLinkMapper
    {
        public static List<AllTrailsLinkModel> MapFromRangeData(IList<IList<object>> values)
        {
            var items = new List<AllTrailsLinkModel>();
            if(values != null)
            {
                foreach (var value in values)
                {
                    if(value != null)
                    {
                        AllTrailsLinkModel item = new()
                        {
                            Title = (value.Count > 0 && value[0] != null) ? value[0].ToString() : "",
                            Link = (value.Count > 1 && value[1] != null) ? value[1].ToString() : ""
                        };
                        items.Add(item);
                    }
                }
            } 
            return items;
        }
        public static IList<IList<object>> MapToRangeData(AllTrailsLinkModel item)
        {
            var objectList = new List<object>() { item.Title, item.Link };
            var rangeData = new List<IList<object>> { objectList };
            return rangeData;
        }
    }
}
