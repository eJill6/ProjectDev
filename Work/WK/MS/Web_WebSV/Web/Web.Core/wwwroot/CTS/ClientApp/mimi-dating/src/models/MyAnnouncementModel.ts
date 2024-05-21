import { AnnouncementType } from "@/enums";

export interface MyAnnouncementModel {
    /// <summary>
    /// ID
    /// </summary>
    id: number;
    /// <summary>
    /// 内容
    /// </summary>
    homeContent: string;
    /// <summary>
    /// 
    /// </summary>
    redirectUrl:string;
    /// <summary>
    /// 创建时间
    /// </summary>
    createDate:string;
    
    createDateText:string;
    /// <summary>
    /// 修改时间
    /// </summary>
    modifyDate:string;
    /// <summary>
    /// 标题
    /// </summary>
    title:string;
  }