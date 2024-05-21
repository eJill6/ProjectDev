<template>
  <div class=" flex_height bg_personal">
    <div class="overflow no_scrollbar">
      <div class="padding_order ">
          <div class="me_order me_none">

            <div class="user_info user_flex">
                <div class="avatar_second avatar_fit">
                  <CdnImage :src="memberInfo?.avatarUrl"  v-if="memberInfo?.avatarUrl?.indexOf('aes') < 0"/>
                  <AssetImage :item="setImageItem(memberInfo)" v-else />
                </div>
                <div class="info">
                    <div class="first_co">
                        <div class="basic">
                            <div class="name_third">{{ memberInfo.nickname }}</div>
                            <div class="member_tag"> <CdnImage class="vip_btn" v-if="memberVipIdentityIcon !== ''" :src="memberVipIdentityIcon" alt=""/></div>
                        </div>

                    </div>
                    <div class="add"> {{ convertDateTime(userInfo.registerTime) }} 加入觅觅</div>
                </div>
            </div>
            <div class="money_info">
                <div class="money_content">
                    <div class="amount_figure">{{ memberInfo.earnestMoney }}</div>
                    <div class="amount_item">保证金</div>
                </div>
                <div class="money_content">
                    <div class="amount_figure">{{ memberInfo.income }}</div>
                    <div class="amount_item">本月收益</div>
                </div>
                <div class="money_content">
                    <div class="amount_figure">{{ memberInfo.freezeIncome }}</div>
                    <div class="amount_item">暂锁收益</div>
                </div>
            </div>
          </div>
          <div class="me_order" v-if="showSquare(memberInfo.userIdentity)">
          <div class="favorite">
              <h1 class="text_bold">广场区</h1>
              <div class="card_group no_scrollbar">
                  <div class="money_info inset_distance">
                      <div class="money_content">
                          <div class="amount_figure">{{ publishedCount(PostType.Square) }}</div>
                          <div class="amount_item">已发帖</div>
                      </div>
                      <div class="money_content">
                          <div class="amount_figure">{{ unlockCount(PostType.Square) }}</div>
                          <div class="amount_item">解锁次数</div>
                      </div>
                      <div class="money_content money_color">
                          <div class="amount_figure">{{ publishCount() }}</div>
                          <div class="amount_item">可发帖次数</div>
                      </div>
                      <div class="money_content">
                          <div class="amount_figure">{{ totalIncome(PostType.Square) }}</div>
                          <div class="amount_item">累计收益</div>
                      </div>
                      </div>
                  </div>
              </div>
          </div>
          <div class="me_order" v-if="showXfg(memberInfo.userIdentity)">
          <div class="favorite">
              <h1 class="text_bold">寻芳阁</h1>
              <div class="card_group no_scrollbar">
                  <div class="money_info inset_distance">
                      <div class="money_content">
                          <div class="amount_figure">{{ publishedCount(PostType.Agency) }}</div>
                          <div class="amount_item">已发帖</div>
                      </div>

                      <div class="money_content" >
                          <div class="amount_figure">{{ unlockCount(PostType.Agency) }}</div>
                          <div class="amount_item">解锁次数</div>
                      </div>

                      <div class="money_content money_color" v-if="showXfgPostCount(memberInfo.userIdentity)">
                          <div class="amount_figure">{{ memberInfo.remainPublish }}</div>
                          <div class="amount_item">可发帖次数</div>
                      </div>

                      <div class="money_content">
                          <div class="amount_figure">{{ totalIncome(PostType.Agency) }}</div>
                          <div class="amount_item">累计收益</div>
                      </div>
                      </div>
                  </div>
              </div>
          </div>
          <div class="me_order" v-if="showOfficial(memberInfo.userIdentity)">
          <div class="favorite">
              <h1 class="text_bold">官方区</h1>
              <div class="card_group no_scrollbar">
                  <div class="money_info inset_distance">
                      <div class="money_content">
                          <div class="amount_figure">{{ publishedCount(PostType.Official) }}</div>
                          <div class="amount_item">已发帖</div>
                      </div>
                      <div class="money_content">
                          <div class="amount_figure">{{ unlockCount(PostType.Official) }}</div>
                          <div class="amount_item">预约次数</div>
                      </div>
                      <div class="money_content">
                          <div class="amount_figure">{{ totalIncome(PostType.Official) }}</div>
                          <div class="amount_item">累计收益</div>
                      </div>
                      </div>
                  </div>
              </div>
          </div>
      </div>   
    </div>      
</div>
</template>
<script lang="ts">
import api from "@/api";
import { CdnImage,AssetImage } from "@/components";
import { PostType, IdentityType, VipType } from "@/enums";
import { NavigateRule, PlayGame, OverviewMode, DialogControl,ImageCacheManager } from "@/mixins";
import { OverviewModel,ImageItemModel,MediaResultModel } from "@/models";
import { MutationType } from "@/store";
import { defineComponent } from "vue";

export default defineComponent({
components: { CdnImage,AssetImage },
mixins: [NavigateRule, PlayGame, OverviewMode, DialogControl,ImageCacheManager],
data() {
  return {
    PostType,
    VipType,
    IdentityType,
  };
},
methods: {
  async changeShopOpen() {
    this.$store.commit(MutationType.SetIsLoading, true);
    const result = await api.setShopOpen();
    this.info.isOpen = result.isOpen;
    this.$store.commit(MutationType.SetIsLoading, false);
  },
  publishLimit(num: number) {
    return num < 0 ? 0 : num;
  },
  publishedCount(type: PostType) {
    const result = this.memberInfo.statistic.find(
      (item) => item.type === type
    );
    if (!result) {
      return;
    }
    return result.publishedCount;
  },
  unlockCount(type: PostType) {
    const result = this.memberInfo.statistic.find(
      (item) => item.type === type
    );
    if (!result) {
      return;
    }
    return result.unlockCount;
  },
  totalIncome(type: PostType) {
    const result = this.memberInfo.statistic.find(
      (item) => item.type === type
    );
    if (!result) {
      return;
    }
    return result.totalIncome;
  },
  isBoss(){
    return this.userInfo.identity===IdentityType.Boss || this.userInfo.identity===IdentityType.SuperBoss;
  },
  showXfg(identity:IdentityType)
  {
    return identity===IdentityType.Boss || identity===IdentityType.SuperBoss || identity===IdentityType.Agent
  },
  showOfficial(identity:IdentityType)
  {
    return identity===IdentityType.Boss || identity===IdentityType.SuperBoss;
  },
  showSquare(identity:IdentityType)
  {
    return identity===IdentityType.Agent || identity===IdentityType.General
  },
  showXfgPostCount(identity:IdentityType)
  {
    return identity===IdentityType.Boss || identity===IdentityType.SuperBoss;
  },
  publishCount(){

    return this.userInfo.quantity?.showRemainingSend;
  },
  setImageItem(model: OverviewModel) {
      let item: ImageItemModel = {
        id: this.userInfo?.userId?.toString(),
        subId: model.avatarUrl,
        class: "",
        src: model.avatarUrl,
        alt: "",
      };
      return item;
    },
  
},
created() {
  if (this.checkUserEmpty) {
     this.setUserInfo();
  }
 
},
watch:{
  memberInfo(newValue,oldValue){
    if(this.memberInfo?.avatarUrl?.indexOf("aes")>-1){
        let list:MediaResultModel[]=[
          {
              id:this.userInfo.userId?.toString(),
              fullMediaUrl:this.memberInfo?.avatarUrl,
          }
        ]
        this.officialShopImage(list);
    }
  }
},
computed: {
  memberVipType() {
    if (this.memberInfo?.cardType?.length > 0) {
      return this.memberInfo.cardType[0];
    }
    return 0;
  },
  memberVipIdentityIcon() {
    switch (this.memberInfo.userIdentity) {
      case IdentityType.Agent:
        return "@/assets/images/level/level_badge_business.png";
      case IdentityType.Boss:
      case IdentityType.SuperBoss:
        return "@/assets/images/level/level_badge_boss.png";
      case IdentityType.Girl:
        return "@/assets/images/level/level_badge_girl.png";
      case IdentityType.Officeholder:
        return "";
    }
    return "";
  },
},
});
</script>