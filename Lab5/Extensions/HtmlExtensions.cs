using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab5.Extensions
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// This method returns output when evaluation criteria is meet
        /// </summary>
        /// <param name="value">Html Helper</param>
        /// <param name="evaluation">Evaluation criteria</param>
        /// <returns>Html output</returns>
        public static MvcHtmlString If(this MvcHtmlString value, bool evaluation)
        {
            return evaluation ? value : MvcHtmlString.Empty;
        }
    }
}