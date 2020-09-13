using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace WindowsServiceEndPoint
{
    public class SOAPEndpointBehaviour : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            //throw new NotImplementedException();
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(new SOAPInspector());
            //clientRuntime.MessageInspectors.Add(new SOAPInspector());
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            //throw new NotImplementedException();
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            //throw new NotImplementedException();
        }
    }
}
