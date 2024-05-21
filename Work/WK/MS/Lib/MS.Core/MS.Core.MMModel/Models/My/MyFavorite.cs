using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.My
{
    public class MyFavorite
    {
       public string FavoriteId { get; set; }
       public string PostId { get; set; }
       public int UserId { get; set; }
       public DateTime CreateTime { get; set; }
       public int Type { get; set; }
    }
}
