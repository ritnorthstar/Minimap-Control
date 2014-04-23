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
    public class TeamsController : ApiController
    {
        public HttpResponseMessage GetAllTeams()
        {
            HttpResponseMessage response;

            // remote read enabled
            if (Minimap.TEAMS_ALLOW_REMOTE_READ)
            {
                IEnumerable<TeamObject> teams = Minimap.TeamManager().GetAll();
                response = Request.CreateResponse<IEnumerable<TeamObject>>(HttpStatusCode.OK, teams);
            }

            // remote read disabled
            else
            {
                response = Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            return response;
        }

        public HttpResponseMessage GetTeam(string id)
        {
            HttpResponseMessage response;

            // remote read enabled
            if (Minimap.TEAMS_ALLOW_REMOTE_READ)
            {
                TeamObject team = Minimap.TeamManager().Get(id);

                // team not found
                if (team == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.NotFound);
                }

                // team found
                else
                {
                    response = Request.CreateResponse<TeamObject>(HttpStatusCode.OK, team);
                }
            }

            // remote read disabled
            else
            {
                response = Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            return response;
        }

        [Route("api/teams/{id}/users")]
        [HttpGet]
        public HttpResponseMessage GetUsersByTeam(string id)
        {
            HttpResponseMessage response;

            // remote read enabled
            if (Minimap.USERS_ALLOW_REMOTE_READ)
            {
                TeamObject team = Minimap.TeamManager().Get(id);

                // team not found
                if (team == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.NotFound);
                }

                // team found
                else
                {
                    IEnumerable<UserObject> users = Minimap.UserManager().GetAll().Where(u => u.TeamId == id);
                    response = Request.CreateResponse<IEnumerable<UserObject>>(HttpStatusCode.OK, users);
                }
            }

            // remote read disabled
            else
            {
                response = Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            return response;
        }

        public HttpResponseMessage PostTeam(TeamObject team)
        {
            HttpResponseMessage response;

            // remote write enabled
            if (Minimap.TEAMS_ALLOW_REMOTE_WRITE)
            {
                // add successful
                if (Minimap.TeamManager().Add(team))
                {
                    response = Request.CreateResponse<TeamObject>(HttpStatusCode.Created, team);

                    // include resource location in response header
                    string uri = Url.Link("DefaultApi", new { id = team.Id });
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

        public HttpResponseMessage PutTeam(TeamObject team)
        {
            HttpResponseMessage response;

            // remote write enabled
            if (Minimap.TEAMS_ALLOW_REMOTE_WRITE)
            {
                // team found
                if (team != null && Minimap.TeamManager().Get(team.Id) != null)
                {
                    // add successful
                    if (Minimap.TeamManager().Add(team))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK);
                    }

                    // add failed
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotAcceptable);
                    }
                }


                // team not found
                else
                {
                    response = PostTeam(team);
                }
            }

            // remote write disabled
            else
            {
                response = Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            return response;
        }

        public HttpResponseMessage DeleteTeam(string id)
        {
            HttpResponseMessage response;

            // remote write enabled
            if (Minimap.TEAMS_ALLOW_REMOTE_WRITE)
            {
                // remove successful
                if (Minimap.TeamManager().Remove(id))
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
