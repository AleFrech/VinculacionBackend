using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Security;
using VinculacionBackend.Database;




namespace VinculacionBackend
{

    public class CustomAuthorizeAttribute : AuthorizeAttribute

    {

        private const string BasicAuthResponseHeader = "WWW-Authenticate";

        private const string BasicAuthResponseHeaderValue = "Basic";

        readonly VinculacionContext Context = new VinculacionContext();



        public string UsersConfigKey { get; set; }

        public string RolesConfigKey { get; set; }



        protected CustomPrincipal CurrentUser

        {

            get { return Thread.CurrentPrincipal as CustomPrincipal; }

            set { Thread.CurrentPrincipal = value as CustomPrincipal; }

        }



        public override void OnAuthorization(HttpActionContext actionContext)

        {

            try

            {

                AuthenticationHeaderValue authValue = actionContext.Request.Headers.Authorization;



                if (authValue != null && !String.IsNullOrWhiteSpace(authValue.Parameter) && authValue.Scheme == BasicAuthResponseHeaderValue)

                {

                    Credentials parsedCredentials = ParseAuthorizationHeader(authValue.Parameter);



                    if (parsedCredentials != null)

                    {

                        var user = Context.Users.FirstOrDefault(u => u.Email == parsedCredentials.Username && u.Password == parsedCredentials.Password);

                        if (user != null)

                        {

                            var roles =
                                Context.UserRoleRels.Where(u => u.User.Id == user.Id).Select(m => m.Role.Name).ToArray();
                          
                               

                            var authorizedUsers = ConfigurationManager.AppSettings[UsersConfigKey];

                            var authorizedRoles = ConfigurationManager.AppSettings[RolesConfigKey];



                            Users = String.IsNullOrEmpty(Users) ? authorizedUsers : Users;

                            Roles = String.IsNullOrEmpty(Roles) ? authorizedRoles : Roles;



                            CurrentUser = new CustomPrincipal(parsedCredentials.Username, roles);



                            if (!String.IsNullOrEmpty(Roles))

                            {

                                if (!CurrentUser.IsInRole(Roles))

                                {

                                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);

                                    actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);

                                    return;

                                }

                            }



                            if (!String.IsNullOrEmpty(Users))

                            {

                                if (!Users.Contains(CurrentUser.UserId.ToString()))

                                {

                                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);

                                    actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);

                                    return;

                                }

                            }



                        }

                    }

                }

            }

            catch (Exception)

            {

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);

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

    //Client credential

    public class Credentials

    {

        public string Username { get; set; }

        public string Password { get; set; }

    }

}