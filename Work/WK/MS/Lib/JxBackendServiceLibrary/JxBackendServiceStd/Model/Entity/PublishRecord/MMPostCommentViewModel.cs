namespace JxBackendService.Model.Entity.PublishRecord
{
    public class MMPostCommentViewModel : MMPostCommentModel
    {
        /// <summary>
        /// 帖子类型描述 帖子類型。1：廣場、2：寻芳阁（原担保(原為中介)）、3：官方、4：體驗
        /// </summary>
        public string PostTypeDesc
        {
            get
            {
                return PostType == 1 ? "广场" : PostType == 2 ? "寻芳阁" : PostType == 3 ? "官方" : "体验";
            }
        }

        /// <summary>
        /// 区域描述
        /// </summary>
        public string AreaCodeDesc { get; set; }

        /// <summary>
        /// 状态描述
        /// </summary>
        public string StatusDesc
        {
            get
            {
                return Status == 1 ? "审核中" : Status == 2 ? "已通过" : "未通过";
            }
        }

        /// <summary>
        /// 消费时间描述
        /// </summary>
        public string SpentTimeDesc
        {
            get
            {
                return SpentTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public string ExamineTimeDesc
        {
            get
            {
                return ExamineTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }
}