using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using SpydusWebAPI.Models;

namespace SpydusWebAPI.Controllers
{
    public class PatronController : ApiController
    {
        private Validator validator;

        [HttpGet]
        public HttpResponseMessage isValid(string id, string password) {
            string result = string.Empty;
            var patronMD = new PatronModel();

            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(password))
            {
                validator = new Validator();

                if (validator.isNidValid(id))
                {
                    result = patronMD.isValid(null, id, password);
                }
                else 
                {
                    result = patronMD.isValid(id, null, password);
                }
            }
            else
            {
                result = "Wrong Parameters";
            }

            var res = new { response = result };

            return Request.CreateResponse(HttpStatusCode.OK, res, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
        
        }

        [HttpGet]
        public HttpResponseMessage isBcodeValid(string bcode, string password)
        {
            string result = string.Empty;
            var patronM = new PatronModel();

            if (!string.IsNullOrEmpty(bcode) && !string.IsNullOrEmpty(password))
            {
                result = patronM.isValid(bcode, null, password);

            } else {
                result = "Wrong Parameters";
            }

            var res = new { response = result };

            return Request.CreateResponse(HttpStatusCode.OK, res, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            
        }


        [HttpGet]
        public string isNidValid(string natid, string password) {

            string result = string.Empty;
            var patronM = new PatronModel();

            if (!string.IsNullOrEmpty(natid) && !string.IsNullOrEmpty(password))
            {
                result = patronM.isValid(null, natid, password);

            }
            else
            {
                result = "Wrong Parameters";
            }


            return result;
        }


        public string GetPatron(string id) {

            return "You are requesting id: " + id;
        }
    }
}
