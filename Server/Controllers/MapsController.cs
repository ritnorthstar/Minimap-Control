using Core;
using Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Server.Controllers
{
    public class MapsController : ApiController
    {
        public HttpResponseMessage GetAllMaps()
        {
            HttpResponseMessage response;

            // remote read enabled
            if (Minimap.MAPS_ALLOW_REMOTE_READ)
            {
                IEnumerable<MapObject> maps = Minimap.MapManager().GetAll();
                response = Request.CreateResponse<IEnumerable<MapObject>>(HttpStatusCode.OK, maps);
            }

            // remote read disabled
            else
            {
                response = Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            return response;
        }

        public HttpResponseMessage GetMap(string id)
        {
            HttpResponseMessage response;

            // remote read enabled
            if (Minimap.MAPS_ALLOW_REMOTE_READ)
            {
                MapObject map = Minimap.MapManager().Get(id);

                // map not found
                if (map == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.NotFound);
                }

                // map found
                else
                {
                    response = Request.CreateResponse<MapObject>(HttpStatusCode.OK, map);
                }
            }
            
            // remote read disabled
            else
            {
                response = Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            return response;
        }

        public HttpResponseMessage PostMap(MapObject map)
        {
            HttpResponseMessage response;

            // remote write enabled
            if (Minimap.MAPS_ALLOW_REMOTE_WRITE)
            {
                // add successful
                if (Minimap.MapManager().Add(map))
                {
                    response = Request.CreateResponse<MapObject>(HttpStatusCode.Created, map);

                    // include resource location in response header
                    string uri = Url.Link("DefaultApi", new { id = map.Id });
                    response.Headers.Location = new Uri(uri);
                }

                // add failed
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.NotAcceptable);
                }
            }

            // remote write disabled
            else
            {
                response = Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            return response;
        }

        public HttpResponseMessage PutMap(MapObject map)
        {
            HttpResponseMessage response;

            // remote write enabled
            if (Minimap.MAPS_ALLOW_REMOTE_WRITE)
            {
                // map found
                if (map != null && Minimap.MapManager().Get(map.Id) != null)
                {
                    // add successful
                    if (Minimap.MapManager().Add(map))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK);
                    }

                    // add failed
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotAcceptable);
                    }
                }


                // map not found
                else
                {
                    response = PostMap(map);
                }
            }

            // remote write disabled
            else
            {
                response = Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            return response;
        }

        public HttpResponseMessage DeleteMap(string id)
        {
            HttpResponseMessage response;

            // remote write enabled
            if (Minimap.MAPS_ALLOW_REMOTE_WRITE)
            {
                // remove successful
                if (Minimap.MapManager().Remove(id))
                {
                    response = Request.CreateResponse(HttpStatusCode.OK);
                }

                // remove failed
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }

            // remote write disabled
            else
            {
                response = Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            return response;
        }
    }
}
