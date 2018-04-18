using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using VKR.Models;

namespace VKR.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            string login = HttpContext.Request.Params["value"];
            using (var db = new Contexts())
            {
                if (db.Users.Where(c => c.Login == login).FirstOrDefault() == null)
                {
                    return "false";
                }
                else
                {
                    return "true";
                }
            }
        }

    }
}
