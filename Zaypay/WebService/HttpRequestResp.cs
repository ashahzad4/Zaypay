using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Xml;
using System.Collections;
using System.Xml.Linq;
using Zaypay;
using Zaypay.Utility;


namespace Zaypay.WebService
{
    public class HttpRequestResp
    {
        private string URI;
        private string Request;
        private string RequestMethod = "GET";


        public HttpRequestResp(string pURI, string reqMethod = "GET", string pRequest = "")
        {
            Request = pRequest;
            URI = pURI;
            RequestMethod = reqMethod;
        }

        public Hashtable GetResponse()
        {

            //HttpWebResponse resp = SendRequest();
            ////XmlTextReader reader = new XmlTextReader(resp.GetResponseStream());

            //StreamReader reader = new StreamReader(resp.GetResponseStream());

            //// Read the whole contents and return as a string  
            //string result = reader.ReadToEnd();
            //Console.WriteLine("TTTTTTTTT: " + result);
            //System.Diagnostics.Debug.WriteLine("TTTTTTT: " + result);

            //while (reader.Read())
            //{


            //    switch (reader.NodeType)
            //    {
            //        case XmlNodeType.Element: // The node is an Element.
            //            Console.Write("<" + reader.Name);

            //            while (reader.MoveToNextAttribute()) // Read attributes.
            //                Console.Write(" " + reader.Name + "='" + reader.Value + "'");
            //            Console.Write(">");
            //            break;
            //        case XmlNodeType.Text: //Display the text in each element.
            //            Console.WriteLine(reader.Value);
            //            break;
            //        case XmlNodeType.EndElement: //Display end of element.
            //            Console.Write("</" + reader.Name);
            //            Console.WriteLine(">");
            //            break;
            //    }
            //}

            HttpWebResponse resp2 = SendRequest();
            XmlTextReader reader2 = new XmlTextReader(resp2.GetResponseStream());

            XmlDocument doc = new XmlDocument();
            doc.Load(reader2);
            XmlNode main = doc.DocumentElement;

            Hashtable htMain = XMLParser.ParseNode(main);

            return htMain;
        }

        public HttpWebResponse SendRequest()
        {

            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(URI);

            webrequest.Method = RequestMethod;
            webrequest.ContentType = "application/x-www-form-urlencoded";
            webrequest.Accept = "application/xml";

            if (RequestMethod == "POST")
            {
                BuildReqStream(ref webrequest);
            }

            return (HttpWebResponse)webrequest.GetResponse();


        }

        private void BuildReqStream(ref HttpWebRequest webrequest)
        {
            
            byte[] bytes = Encoding.ASCII.GetBytes(Request);
            
            webrequest.ContentLength = bytes.Length;

            Stream oStreamOut = webrequest.GetRequestStream();
            oStreamOut.Write(bytes, 0, bytes.Length);
            oStreamOut.Close();
        }

    }
}
