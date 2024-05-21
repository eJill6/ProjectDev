import { PostType,ReviewStatusType } from "@/enums";
export interface ReportDetailModel {
    
    /// <summary>
    /// 创建时间
    /// </summary>
    createTime:string,
    createTimeText:string,
    /// <summary>
    /// 详情描述
    /// </summary>
    describe:string,

    /// <summary>
    /// 未通过的原因
    /// </summary>
    memo:string,

    /// <summary>
    /// 被举报的帖子ID
    /// </summary>
    postId:string,

    /// <summary>
    /// 举报编号
    /// </summary>
    reportId:string,

    /// <summary>
    /// 投诉原因
    /// </summary>
    reportTypeText:string,

    /// <summary>
    /// 截图证据
    /// </summary>
    photoIds:string[],

    /// <summary>
    /// 帖子类型
    /// </summary>
    postType:PostType,
    /// <summary>
    /// 帖子审核状态
    /// </summary
    postStatus:ReviewStatusType,
    /// <summary>
    /// 帖子是否上下架
    /// </summary
    postIsDelete:boolean,
  }