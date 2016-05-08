using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using VinculacionBackend.Data.Database;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;


namespace VinculacionBackend.Security.BasicAuthentication
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private const string BasicAuthResponseHeader = "WWW-Authenticate";
        private const string BasicAuthResponseHeaderValue = "Basic";
        public string UsersConfigKey { get; set; }
        public string RolesConfigKey { get; set; }
        protected CustomPrincipal CurrentUser
        {
            get { return Thread.CurrentPrincipal as CustomPrincipal; }
            set { Thread.CurrentPrincipal = value as CustomPrincipal; }
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            VinculacionContext context = new VinculacionContext();
            try
            {
                AuthenticationHeaderValue authValue = actionContext.Request.Headers.Authorization;
                if (authValue == null)
                {
                    CurrentUser = new CustomPrincipal("", new string[] { "Anonymous" });
                    if (!string.IsNullOrEmpty(Roles))
                    {
                        if (!CurrentUser.IsInRole(Roles))
                        {
                            actionContext.Response =
                                actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                            actionContext.Response.Headers.Add(BasicAuthResponseHeader,
                                BasicAuthResponseHeaderValue);
                            return;
                        }
                    }
                }
                if (string.IsNullOrWhiteSpace(authValue?.Parameter) || authValue.Scheme != BasicAuthResponseHeaderValue)
                    return;
                var parsedCredentials = ParseAuthorizationHeader(authValue.Parameter);
                if (parsedCredentials == null) return;
                var user =
                    context.Users.FirstOrDefault(
                        u => u.Email == parsedCredentials.Username && u.Password == parsedCredentials.Password);
                if (user == null) return;
                {
                    var roles =
                        Enumerable.ToArray<string>(context.UserRoleRels.Where(u => u.User.Id == user.Id).Select(m => m.Role.Name));
                    var authorizedUsers = ConfigurationManager.AppSettings[UsersConfigKey];
                    var authorizedRoles = ConfigurationManager.AppSettings[RolesConfigKey];
                    Users = string.IsNullOrEmpty(Users) ? authorizedUsers : Users;
                    Roles = string.IsNullOrEmpty(Roles) ? authorizedRoles : Roles;
                    CurrentUser = new CustomPrincipal(parsedCredentials.Username, roles);
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.User = CurrentUser;
                    }
                    if (!string.IsNullOrEmpty(Roles))
                    {
                        if (!CurrentUser.IsInRole(Roles))
                        {
                            actionContext.Response =
                                actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                            actionContext.Response.Headers.Add(BasicAuthResponseHeader,
                                BasicAuthResponseHeaderValue);
                            return;
                        }
                    }
                    if (string.IsNullOrEmpty(Users)) return;
                    if (Users.Contains(CurrentUser.UserId.ToString())) return;
                    actionContext.Response =
                        actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                    actionContext.Response.Headers.Add(BasicAuthResponseHeader,
                        BasicAuthResponseHeaderValue);
                    return;
                }
            }
            catch (Exception e)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, e.Message);
                actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                return;
            }
        }
        private Credentials ParseAuthorizationHeader(string authHeader)
        {
            string[] credentials = Encoding.ASCII.GetString(Convert.FromBase64String(authHeader)).Split(new[] { ':' });
            if (credentials.Length != 2 || string.IsNullOrEmpty(credentials[0]) || string.IsNullOrEmpty(credentials[1]))
                return null;
            return new Credentials() { Username = credentials[0], Password = credentials[1], };
        }
    }
}