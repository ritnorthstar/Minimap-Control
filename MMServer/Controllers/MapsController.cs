using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataTypes;
using MMServer.Models;

namespace MMServer.Controllers
{
    public class MapsController : ApiController
    {
        static readonly IMapRepository repository = new LocalMapRepository();

        public IEnumerable<Map> GetAllMaps()
        {
            return repository.GetAll();
        }

        public Map GetMap(int id)
        {
            Map map = repository.Get(id);
            if (map == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return map;
        }

        public HttpResponseMessage PostMap(Map map)
        {
            map = repository.Add(map);
            var response = Request.CreateResponse<Map>(HttpStatusCode.Created, map);

            // Include resource location in response header
            string uri = Url.Link("DefaultApi", new { id = map.id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        public void PutMap(int id, Map map)
        {
            map.id = id;
            if (!repository.Update(map))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        public void DeleteMap(int id)
        {
            Map map = repository.Get(id);
            if (map == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            repository.Remove(id);
        }
    }
}
