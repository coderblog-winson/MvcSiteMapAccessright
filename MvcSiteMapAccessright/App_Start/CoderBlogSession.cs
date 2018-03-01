using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcSiteMapAccessright
{
    public class CoderBlogSession
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static CoderBlogSession Instance { get { return SingletonInstance; } }
        private static readonly CoderBlogSession SingletonInstance = new CoderBlogSession();

        /// <summary>
        /// User Role
        /// </summary>
        public string Level
        {
            get
            {
                if (HttpContext.Current.Session["Level"] != null)
                {
                    return HttpContext.Current.Session["Level"].ToString();
                }
                return "";
            }

            set
            {
                HttpContext.Current.Session["Level"] = value;
            }
        }

        /// <summary>
        /// User Dept
        /// </summary>
        public string Dept
        {
            get
            {
                if (HttpContext.Current.Session["Dept"] != null)
                {
                    return HttpContext.Current.Session["Dept"].ToString();
                }
                return "";
            }

            set
            {
                HttpContext.Current.Session["Dept"] = value;
            }
        }

    }
}