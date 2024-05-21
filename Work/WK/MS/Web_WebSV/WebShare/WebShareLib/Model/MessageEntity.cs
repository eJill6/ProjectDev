using System.Runtime.Serialization;

namespace SLPolyGame.Web.Model
{
    [DataContract(Name = "MessageEntity{0}")]
    public class MessageEntity<T> where T : class
    {
        [DataMember]
        public int Code { get; set; }

        [DataMember]
        public string Msg { get; set; }

        [DataMember]
        public T Data { get; set; }
    }
}