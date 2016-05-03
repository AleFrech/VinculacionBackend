using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VinculacionBackend.CustomDataNotations
{
    public class HandleApiError:HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
          
            if (filterContext.ExceptionHandled)
            {
                return;
            }
           
              
                var actionName = filterContext.RouteData.Values["action"].ToString();
                var controllerType = filterContext.Controller.GetType();
                var method = controllerType.GetMethod(actionName);
                var returnType = method.ReturnType;

              
                if (returnType == typeof(JsonResult))
                {
                    filterContext.Result = new JsonResult()
                    {
                        Data = "Not Found"
                    };
                }

              
            filterContext.ExceptionHandled = true;
        }
    }
}