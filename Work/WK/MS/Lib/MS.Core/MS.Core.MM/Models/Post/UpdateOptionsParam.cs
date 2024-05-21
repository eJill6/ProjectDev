using MS.Core.MM.Models.Entities.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Post
{
    public class UpdateOptionsParam : MMOptions
    {
        [JsonIgnore] 
        public new DateTime CreateDate { get; set; }
        [JsonIgnore]
        public string CreateUser { get; set; } = String.Empty;
        //[JsonIgnore]
        public byte OptionType { get; set; }
        //[JsonIgnore]
        public byte PostType { get; set; }
        [JsonIgnore]
        public DateTime? ModifyDate { get; set; }
    }
}
