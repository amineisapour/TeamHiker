using CrawlerConsole.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerConsole.Mapper
{
    public static class AllTraiReviewMapper
    {
        public static List<AllTraiReviewModel> MapFromRangeData(IList<IList<object>> values)
        {
            var items = new List<AllTraiReviewModel>();
            if (values != null)
            {
                foreach (var value in values)
                {
                    if (value != null)
                    {
                        AllTraiReviewModel item = new()
                        {
                            PageTitle = (value[0] != null) ? value[0].ToString() : "",
                            PageLink = (value[1] != null) ? value[1].ToString() : "",
                            PageAddressMap = (value[2] != null) ? value[2].ToString() : "",
                            Address = (value[3] != null) ? value[3].ToString() : "",
                            ReviewAuthor = (value[4] != null) ? value[4].ToString() : "",
                            ReviewDate = (value[5] != null) ? value[5].ToString() : "",
                            ReviewRatingValue = (value[6] != null) ? value[6].ToString() : "",
                            ReviewBody = (value[7] != null) ? value[7].ToString() : ""
                        };
                        items.Add(item);
                    }
                }
            }
            return items;
        }
        public static IList<IList<object>> MapToRangeData(List<AllTraiReviewModel> items)
        {
            var rangeData = new List<IList<object>>();
            foreach (AllTraiReviewModel item in items)
            {
                var objectList = new List<object>()
                {
                    item.PageTitle,
                    item.PageLink,
                    item.PageAddressMap,
                    item.Address,
                    item.ReviewAuthor,
                    item.ReviewDate,
                    item.ReviewRatingValue,
                    item.ReviewBody
                };
                rangeData.Add(objectList);
            }
            
            //var rangeData = new List<IList<object>> { objectList };

            return rangeData;
        }
    }
}