using MvcSiteMapProvider.Security;
using System;
using MvcSiteMapProvider;

namespace MvcSiteMapAccessright
{
    public class SiteMapAclModule : IAclModule
    {
        #region IAclModule Members

        /// <summary>
        /// Determines whether node is accessible to user.
        /// </summary>
        /// <param name="siteMap">The site map.</param>
        /// <param name="node">The node.</param>
        /// <returns>
        /// 	<c>true</c> if accessible to user; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAccessibleToUser(ISiteMap siteMap, ISiteMapNode node)
        {
            try
            {
                bool userHasAccessToNode = true;

                if (node.Attributes.ContainsKey("accessRightExpr"))
                {
                    var compiler = new RuntimeCompiler<bool>();
                    userHasAccessToNode = compiler.ExecuteExpression(node.Attributes["accessRightExpr"].ToString().
                        Replace("{dept}", CoderBlogSession.Instance.Dept).
                        Replace("{level}", CoderBlogSession.Instance.Level));
                }

                return userHasAccessToNode;
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        #endregion
    }
}