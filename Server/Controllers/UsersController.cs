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
    public class UsersController : ApiController
    {
        public HttpResponseMessage GetAllUsers()
        {
            HttpResponseMessage response;

            // remote read enabled
            if (Minimap.USERS_ALLOW_REMOTE_READ)
            {
                IEnumerable<UserObject> users = Minimap.UserManager().GetAll();
                response = Request.CreateResponse<IEnumerable<UserObject>>(HttpStatusCode.OK, users);
            }

            // remote read disabled
            else
            {
                response = Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            return response;
        }

        public HttpResponseMessage GetUser(string id)
        {
            HttpResponseMessage response;

            // remote read enabled
            if (Minimap.USERS_ALLOW_REMOTE_READ)
            {
                UserObject user = Minimap.UserManager().Get(id);

                // user not found
                if (user == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.NotFound);
                }

                // user found
                else
                {
                    response = Request.CreateResponse<UserObject>(HttpStatusCode.OK, user);
                }
            }

            // remote read disabled
            else
            {
                response = Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            return response;
        }

        public HttpResponseMessage PostUser(UserObject user)
        {
            HttpResponseMessage response;

            // remote write enabled
            if (Minimap.USERS_ALLOW_REMOTE_WRITE)
            {
                // add successful
                if (Minimap.UserManager().Add(user))
                {
                    response = Request.CreateResponse<UserObject>(HttpStatusCode.Created, user);

                    // include resource location in response header
                    string uri = Url.Link("DefaultApi", new { id = user.Id });
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

        public HttpResponseMessage PutUser(string id, UserObject user)
        {
            HttpResponseMessage response;

            user.Id = id;

            // remote write enabled
            if (Minimap.USERS_ALLOW_REMOTE_WRITE)
            {
                // user found
                if (user != null && Minimap.UserManager().Get(user.Id) != null)
                {
                    // add successful
                    if (Minimap.UserManager().Add(user))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK);
                    }

                    // add failed
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotAcceptable);
                    }
                }


                // user not found
                else
                {
                    response = PostUser(user);
                }
            }

            // remote write disabled
            else
            {
                response = Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            return response;
        }

        public HttpResponseMessage DeleteUser(string id)
        {
            HttpResponseMessage response;

            // remote write enabled
            if (Minimap.USERS_ALLOW_REMOTE_WRITE)
            {
                // remove successful
                if (Minimap.UserManager().Remove(id))
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
