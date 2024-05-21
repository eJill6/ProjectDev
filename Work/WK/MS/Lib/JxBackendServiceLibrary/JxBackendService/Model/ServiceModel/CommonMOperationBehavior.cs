using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;

namespace JxBackendService.Model.ServiceModel
{
    public class CommonMOperationBehavior : Attribute, IOperationBehavior
    {
        //private static readonly CommonDispatchMessageInspector _inspector = new CommonDispatchMessageInspector();
        public static Func<OperationDescription, CommonDispatchMessageInspector> CreateApplicationInspector { get; set; }

        public void AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.ClientOperation clientOperation)
        {
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.DispatchOperation dispatchOperation)
        {
            CommonDispatchMessageInspector inspector = dispatchOperation.Parent.MessageInspectors
                .Where(x => x is CommonDispatchMessageInspector)
                .FirstOrDefault() as CommonDispatchMessageInspector;

            if (inspector != null)
            {
                inspector.AddOperation(operationDescription);
            }
            else
            {
                inspector = CreateApplicationInspector.Invoke(operationDescription);
                dispatchOperation.Parent.MessageInspectors.Add(inspector);
            }
        }

        public void Validate(OperationDescription operationDescription)
        {
        }
    }
}