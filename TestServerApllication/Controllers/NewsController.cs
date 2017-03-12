using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TestServerApllication.Controllers
{
    public class NewsController : ApiController
    {
        public List<News.NewsStruct> selectAll()
        {
            return News.selectAll();
        }
    }
}
