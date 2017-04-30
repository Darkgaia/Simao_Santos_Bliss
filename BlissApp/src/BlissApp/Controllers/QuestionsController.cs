using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BlissApp.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class QuestionsController : Controller
    {
        [HttpGet]
        public string[] Get(int? limit, int? offset, string filter, string question_filter)
        {
            if (limit.HasValue)
            {
                if (offset.HasValue)
                {
                    if (!string.IsNullOrEmpty(filter))
                    {
                        return new string[] { "limit: " + limit, "offset: " + offset, "filter: " + filter };
                    }
                    else
                    {
                        return new string[] { "limit: " + limit, "offset: " + offset };
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(filter))
                    {
                        return new string[] { "limit: " + limit, "filter: " + filter };
                    }
                    else
                    {
                        return new string[] { "limit: " + limit };
                    }
                }
            }
            else
            {
                if (offset.HasValue)
                {
                    if (!string.IsNullOrEmpty(filter))
                    {
                        return new string[] {"offset: " + offset, "filter: " + filter };
                    }
                    else
                    {
                        return new string[] {"offset: " + offset };
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(filter))
                    {
                        return new string[] {"filter: " + filter };
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(question_filter))
                        {
                            return new string[] { "question_filter: " + question_filter };
                        }
                        else
                        {
                            return new string[] { "defualt"};
                        }
                    }
                }
            }
        }
    }
}
