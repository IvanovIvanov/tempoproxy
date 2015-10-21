using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;

namespace TempoProxy.Controllers
{
    [RoutePrefix("api/Proxy")]
    public class ProxyController : ApiController
    {
        private static XmlDocument MakeRequest(string requestUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                return (xmlDoc);

            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                return null;
            }
        }

        private static List<string> GetEnabledDevs()
        {
            IFileWorker file = new FileWorker(null);
            String dataFromFile = file.ReadFile();

            List<string> Devs = JsonConvert.DeserializeObject<List<string>>(dataFromFile);
            return Devs;
        }


        //api/Media/GetReport                
        [Route("GetReport")]
        public HttpResponseMessage GetReport()
        {
            XmlDocument RawXml = MakeRequest("https://caddiesoft.atlassian.net/plugins/servlet/tempo-getWorklog/?dateFrom=2011-01-01&dateTo=2015-01-31&format=xml&diffOnly=false&tempoApiToken=818caed8-4359-4b62-a681-d9cf25098e16");
            
            MemoryStream xmlStream = new MemoryStream( );
            RawXml.Save(xmlStream);
            xmlStream.Flush();
            xmlStream.Position = 0;

            XElement root = XElement.Load(xmlStream);

            var EnabledDevs = GetEnabledDevs();

            XDocument output = new XDocument(new XElement(root.Name, root.Elements("worklog")
                .Where(d => EnabledDevs.Contains((string)d.Element("staff_id"))))); 
                                                    
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                output.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                string XMLString = stringWriter.GetStringBuilder().ToString();
                                
                return new HttpResponseMessage()
                {
                    Content = new StringContent(XMLString,
                        Encoding.UTF8, "application/xml")
                };            
            }                                                
        }     
    }
}
