import { PostType } from "@/enums";

export interface BaseInfoModel {
  /// 帖子 Id
  postId: string;

  postType: PostType;

  /// 封面照片
  coverUrl: string;
}
