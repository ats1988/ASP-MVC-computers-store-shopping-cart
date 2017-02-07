using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace EntityFrameworkDatabaseFirst.Models
{
    public static class helpers
    {
        public static MvcHtmlString SortDirection(this HtmlHelper helper, ref WebGrid grid, string columnName)
        {
            string html = "";
            if (grid.SortColumn == columnName && grid.SortDirection == System.Web.Helpers.SortDirection.Ascending)
            {
                html = "▲";
            }
            else if (grid.SortColumn == columnName && grid.SortDirection == System.Web.Helpers.SortDirection.Descending)
            {
                html = "▼"; //http://stackoverflow.com/questions/2701192/what-characters-can-be-used-for-up-down-triangle-arrow-without-stem-for-displa
            }
            else
            {
                html = "";
            }
            return MvcHtmlString.Create(html);
        }

    }
}