using MS.Core.Attributes;
using MS.Core.MM.Models.Entities.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Post
{
    public class CreateOptionsParam: MMOptions
    {
        [JsonIgnore]
        public int OptionId { get; set; }
        [JsonIgnore]
        public DateTime? ModifyDate { get; set; }
    }
}
