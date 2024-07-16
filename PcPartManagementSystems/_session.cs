namespace PcPartManagementSystems
{
    public class _session
    {
        public void UserLogin(Microsoft.AspNetCore.Http.HttpContext httpContext, bl.model.Users user)
        {

            // Store user information in session
            httpContext.Session.SetString("_UserName", user.Username);
            httpContext.Session.SetString("_FullName", user.Firstname + " " + user.Lastname);

            httpContext.Session.SetString("_Role", user.Role);
        }

        public void ClearSession(HttpContext httpContext)
        {
            //Clear user information in session
            httpContext.Session.Remove("_UserName");
            httpContext.Session.Remove("_FullName");
            httpContext.Session.Remove("_Role");
        }

        public Boolean IsAddmin(Microsoft.AspNetCore.Http.HttpContext httpContext, bl.model.Users user)
        {
            httpContext.Session.SetString("_Role", user.Role);
            if (user.Role != "admin")
            {
                return false;
            }
            return true;
        }

        public Boolean IsUserLoggedIn(HttpContext httpContext)
        {
            return httpContext.Session.GetString("_FullName") != null;
        }

        public string GetSessionValue(HttpContext httpContext, string key)
        {
            return httpContext.Session.GetString(key);
        }

    }

}
