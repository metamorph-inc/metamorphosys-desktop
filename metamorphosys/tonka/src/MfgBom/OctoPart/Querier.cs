/*
Copyright (C) 2013-2015 MetaMorph Software, Inc

Permission is hereby granted, free of charge, to any person obtaining a
copy of this data, including any software or models in source or binary
form, as well as any drawings, specifications, and documentation
(collectively "the Data"), to deal in the Data without restriction,
including without limitation the rights to use, copy, modify, merge,
publish, distribute, sublicense, and/or sell copies of the Data, and to
permit persons to whom the Data is furnished to do so, subject to the
following conditions:

The above copyright notice and this permission notice shall be included
in all copies or substantial portions of the Data.

THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;

namespace MfgBom.OctoPart
{

    [Serializable]
    public class OctopartQueryException : Exception
    {
        public OctopartQueryException()
            : base()
        { }

        public OctopartQueryException(string message)
            : base(message)
        { }

        protected OctopartQueryException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }

    [Serializable]
    public class OctopartQueryServerException : OctopartQueryException
    {
        public OctopartQueryServerException()
            : base()
        { }

        public OctopartQueryServerException(string message)
            : base(message)
        { }

        protected OctopartQueryServerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }

    [Serializable]
    public class OctopartQueryRateException : OctopartQueryException
    {
        public OctopartQueryRateException()
            : base()
        { }

        public OctopartQueryRateException(string message)
            : base(message)
        { }

        protected OctopartQueryRateException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }


    public class Querier
    {
        public String apiKey { get; private set; }
        public Querier(String ApiKey)
        {
            this.apiKey = ApiKey;
        }

        public String QueryMpn(String mpn)
        {
            var query = new List<dynamic>()
            {
                new Dictionary<string, string>()
                { { "mpn", mpn } }
            };

            string octopartUrlBase = "http://octopart.com/api/v3";
            string octopartUrlEndpoint = "parts/match";

            // Create the search request
            string queryString = (new JavaScriptSerializer()).Serialize(query);
            var client = new RestClient(octopartUrlBase);
            var req = new RestRequest(octopartUrlEndpoint, Method.GET)
                        .AddParameter("apikey", this.apiKey)
                        .AddParameter("queries", queryString)
                        .AddParameter("include[]", "specs")
                        .AddParameter("include[]", "descriptions");

            // Perform the search and obtain results
            var data = client.Execute(req).Content;
            var response = JsonConvert.DeserializeObject<dynamic>(data);

            var classResponse = response["__class__"];
            if (classResponse == "PartsMatchResponse")
            {
                return response["results"][0]["items"][0].ToString();
            }
            else
            {
                if (classResponse == "ClientErrorResponse")
                {
                    var message = response["message"].ToString();
                    if (message == "Access denied by rate limiter")
                    {
                        throw new OctopartQueryRateException(message);
                    }
                    else
                    {
                        throw new OctopartQueryException(message);
                    }
                }
                else if (classResponse == "ServerErrorResponse ")
                {
                    var message = response["message"].ToString();
                    throw new OctopartQueryServerException(message);
                }
                else
                {
                    throw new OctopartQueryException(response.ToString());
                }
            }
        }
    }
}
