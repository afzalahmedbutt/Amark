using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;

namespace WindowsServiceEndPoint
{
    public class SOAPInspector : IClientMessageInspector
    {
        public string LastRequestXml { get; private set; }
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            LastRequestXml = request.ToString();
            var newRequest = LastRequestXml.Replace(" encoding=\"utf-16\"", "")
                .Replace("xmlns:s", "xmlns:soap")
                .Replace("<s:", "<soap:")
                .Replace("</s:", "</soap:")
                .Replace("<Action s:", "<Action soap:");
            
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
           
            writer.Write(newRequest);
            writer.Flush();
            ms.Position = 0;
            var reader = XmlReader.Create(ms);
            request = Message.CreateMessage(reader, int.MaxValue, request.Version);
            var payload = request.ToString();
            return request;
        }

        /// Manipulate the SOAP message
        private string ChangeMessage()
        {
            // LastRequestXML is a string here. You can change it to your heart's content
            // Sample example here

            // NOTE - I couldn't load the string into an XDocument or XMLDocument as it was throwing all sort of errors
            // about undefined namespaces. String manipulation to the rescue!

            // strip out the envelope and header
            var startIndexOfStringToKeep = LastRequestXml.IndexOf("RequestHeader", StringComparison.CurrentCulture);

            // strip out the footer
            var sanitizedRequestXml = LastRequestXml.Substring(startIndexOfStringToKeep - 1);
            //var firstPhase = sanitizedRequestXml.Replace(@"</s:Envelope>", string.Empty);
            var firstPhase = sanitizedRequestXml.Replace(@"<soap:Body>", @"<soap:Body>");
            var secondPhase = firstPhase.Replace(@"</soap:Body>", @"</s:Body>");
            //var secondPhase = test.Replace(@"</s:Body>", string.Empty);
            //var thirdPhase = test2.Replace(@"</SomeServiceRequest>", string.Empty);

            //// wrap the body with the right element declarations sans any namespaces
            ////var bodyRequestXml = "<SomeServiceRequest>" + thirdPhase + "</SomeServiceRequest>";


            //var document = new XmlDocument();
            //var root = document.CreateElement("s", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
            //root.SetAttribute("xmlns:s", "http://schemas.xmlsoap.org/soap/envelope/");
            //root.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            //root.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
            //root.SetAttribute("xmlns", "http://www.randomnamespaceroot.co.uk/SomeServiceIdentification");
            //document.AppendChild(root);

            //var bodyHeader = document.CreateElement("s", "Body", "http://schemas.xmlsoap.org/soap/envelope/");
            //root.AppendChild(bodyHeader);

            //var body = document.CreateElement("IdentifyCustomerRequest", "http://www.randomnamespaceroot.co.uk/SomeServiceIdentification");
            //bodyHeader.AppendChild(body);

            //var replacedString = GetPayloadString(document);
            //var removedString = replacedString.Replace(@"<IdentifyCustomerRequest />", bodyRequestXml);

            return secondPhase;
        }


        /// Helper method to get the XMLDocument format right
        private string GetPayloadString(XmlDocument document)
        {
            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter, settings))
            {
                document.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                return stringWriter.GetStringBuilder().ToString();
            }
        }
    }
}
