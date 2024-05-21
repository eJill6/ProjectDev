using JxBackendService.Resource.Element;
using System;

namespace JxBackendService.Model.Exceptions
{
    /// <summary>
    /// 用於掛在WCF的ViewModel的一些顯示用的屬性，
    /// 因為WCF一定要有getter和setter才會在client端產出對應程式碼，
    /// 但唯讀屬性需避免改屬性被set
    /// </summary>
    public class SetterForWCFFormatOnlyException : Exception
    {
        public SetterForWCFFormatOnlyException() : base(MessageElement.SetterForWCFFormatOnly)
        {

        }
    }
}
