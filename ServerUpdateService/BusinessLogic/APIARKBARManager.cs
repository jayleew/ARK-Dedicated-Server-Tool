using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using ServerUpdateService.JSONDataContracts;
using Newtonsoft.Json;

namespace ServerUpdateService.BusinessLogic
{
    public class APIARKBARManager
    {
        private WebRequest CreateRequest(string uri)
        {
            var request = WebRequest.Create(uri);

            request.Method = "GET";        // Post method
            request.ContentType = "text/xml";     // content type            

            return request;
        }

        public T GetServerInfo<T>(string uri)
        {
            using (var reader = new StreamReader(CreateRequest(uri).GetResponse().GetResponseStream()))
            {                
                //deserialize and return the response                 

                JsonSerializer dserializer = new JsonSerializer();            
                return (T) dserializer.Deserialize(reader, typeof(T) );                  
            }
        }
    }
}
