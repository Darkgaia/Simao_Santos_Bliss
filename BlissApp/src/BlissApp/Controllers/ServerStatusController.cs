using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlissApp.Model;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BlissApp.Controllers
{
    

    [Route("api/[controller]")]
    public class ServerStatusController : Controller
    {
        private static ServerStatus stat = new ServerStatus
        {
            Status = "OK"
        };

        // GET: api/<controller>
        [HttpGet]
        public JsonResult Get()
        {
            
            return Json(stat);
        }

        // PUT api/<controller>
        [HttpPut]
        public void Put([FromBody]string value)
        {
            if (value == "OK" || value == "NOT OK")
            {
                stat.Status = value;
                Response.StatusCode = 204;
            }
            else
            {
                Response.StatusCode = 400;
                /*Add example of response*/
            }
        }
    }
}
