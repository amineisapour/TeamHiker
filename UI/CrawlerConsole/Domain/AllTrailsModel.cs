using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerConsole.Domain
{
    public class AllTrailsLinkModel
    {
        public string Title { get; set; }
        public string Link { get; set; }
    }

    public class AllTraiReviewModel
    {
        public string PageTitle { get; set; }
        public string PageLink { get; set; }
        public string PageAddressMap { get; set; }
        public string Address { get; set; }
        public string ReviewAuthor { get; set; }
        public string ReviewDate { get; set; }
        public string ReviewRatingValue { get; set; }
        public string ReviewBody { get; set; }
    }
}
