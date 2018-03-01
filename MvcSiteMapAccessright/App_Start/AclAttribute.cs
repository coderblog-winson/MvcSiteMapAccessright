
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Web.Routing;

namespace MvcSiteMapAccessright
{
    public class AclAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //for testing, set the current user rights in session (should be set this after login)
            CoderBlogSession.Instance.Dept = "B";
            CoderBlogSession.Instance.Level = "Supervisor";
            

            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            var currentController = filterContext.RouteData.Values["Controller"].ToString();
            var currentAction = filterContext.RouteData.Values["Action"].ToString();


            var isAuthenticated = true;

            //get the menu item from sitemap xml for access right checking
            XElement doc = RemoveAllNamespaces(XDocument.Load(HttpContext.Current.Server.MapPath("~/Mvc.sitemap")).Root);
            var currentNode = (from el in doc.Descendants("mvcSiteMapNode")
                               where (string)el.Attribute("controller") == currentController &&
                               (string)el.Attribute("action") == currentAction && el.Attribute("clickable") == null
                               select el).FirstOrDefault();

            //check AccessRightExpr
            if (currentNode != null)
            {
                isAuthenticated = CheckMenuItemRight(currentNode);
            }

            if (!isAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "AccessDenied" }));
            }

        }

        /// <summary>
        /// Check the menu item access right with recursion
        /// </summary>
        /// <param name="node">current node</param>
        /// <param name="User"></param>
        /// <returns></returns>
        private bool CheckMenuItemRight(XElement node)
        {
            var isAuthenticated = true;
            if (node.Attribute("accessRightExpr") != null)
            {
                var compiler = new RuntimeCompiler<bool>();
                isAuthenticated = compiler.ExecuteExpression(node.Attribute("accessRightExpr").Value.
                        Replace("{dept}", CoderBlogSession.Instance.Dept). //replace the tag to variable
                        Replace("{level}", CoderBlogSession.Instance.Level));//replace the tag to variable
            }

            if (node.Attribute("roles") != null)
            {
                //TODO::if you have implement the ASP.NET identity Authorization, e.g
                //isAuthenticated = node.Attribute("roles").Value.Split(',').Any(User.IsInRole);
            }


            if (isAuthenticated && node.Parent != null)
            {
                isAuthenticated = CheckMenuItemRight(node.Parent);
            }

            return isAuthenticated;
        }

        protected XElement RemoveAllNamespaces(XElement doc)
        {
            //XElement doc = XDocument.Load(fileURI).Root;
            foreach (XElement xe in doc.DescendantsAndSelf())
            {
                xe.Name = xe.Name.LocalName;
                xe.ReplaceAttributes((from xattrib in xe.Attributes().Where(xa => !xa.IsNamespaceDeclaration) select new XAttribute(xattrib.Name.LocalName, xattrib.Value)));
            }
            return doc;
        }

    }
}