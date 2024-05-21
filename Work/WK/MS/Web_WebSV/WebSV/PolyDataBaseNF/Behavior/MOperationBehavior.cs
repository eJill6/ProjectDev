using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Description;
using SLPolyGame.Web;

namespace SLPolyGame.Web.Behavior
{
    public class MOperationBehavior : Attribute, IOperationBehavior
    {
        public void AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.ClientOperation clientOperation)
        {
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.DispatchOperation dispatchOperation)
        {
            DispatchMessageInspector inspector = dispatchOperation.Parent.MessageInspectors
                .Where(x => x is DispatchMessageInspector)
                .FirstOrDefault() as DispatchMessageInspector;

            if (inspector != null)
            {
                inspector.AddOperation(operationDescription);
            }
            else
            {
                inspector = new DispatchMessageInspector(operationDescription);
                dispatchOperation.Parent.MessageInspectors.Add(inspector);
            }
        }

        public void Validate(OperationDescription operationDescription)
        {
        }
    }
}