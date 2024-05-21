using MS.Core.MMModel.Models.Post.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.My
{
    public class MyFavoritePost:MyFavorite
    {
        /// <summary>
        /// 帖子ID
        /// </summary>
        public string PostId { get; set; } 
        /// <summary>
        /// 帖子类型
        /// </summary>
        public int PostType { get; set; } 
        /// <summary>
        /// 封面图片
        /// </summary>
        public string CoverUrl { get; set; }
        /// <summary>
        /// 消息类型ID
        /// </summary>
        public int MessageId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 区域代码
        /// </summary>
        public string AreaCode { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public string Age { get; set; }
        /// <summary>
        /// 身高
        /// </summary>
        public string Height { get; set; }
        /// <summary>
        /// 罩杯
        /// </summary>
        public string Cup { get; set; }
        /// <summary>
        /// 期望价格
        /// </summary>
        public string LowPrice { get; set; }
        /// <summary>
        /// 解锁次数
        /// </summary>
        public int UnlockCount { get; set; }
        /// <summary>
        /// 观看数
        /// </summary>
        public int Views { get; set; }
        /// <summary>
        /// 解锁次数基础值
        /// </summary>
        public int UnlockBaseCount { get; set; }
        /// <summary>
        /// 观看次数基础值
        /// </summary>
	 	public int ViewBaseCount { get; set; }
        public string[] ServiceItem { get; set; }
        public string Job { get; set; }
    }
}
