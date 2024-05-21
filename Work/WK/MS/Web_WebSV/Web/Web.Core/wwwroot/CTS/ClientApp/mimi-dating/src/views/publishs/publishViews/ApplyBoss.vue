<template>
    <div class=" flex_height">
        <ImageZoom :image="imageZoomItem" :isEdit="true" v-if="imageZoomSwitch" @deleteImage="deleteImageItem"></ImageZoom>
        <div class="overflow no_scrollbar post bg_post_b ">
            <div class="head_prompt_notice alert" v-if="errorMessage">
                {{ errorMessage }}
            </div>
            <!-- <div class="head_prompt_notice alert">请输入店铺名称</div> -->
            <!-- 滑動內容 -->
            <div class=" padding_basic">
                <div class="introduce_title">
                    <CdnImage src="@/assets/images/post/pic_title_b_1.png" alt="" />
                </div>
                <div class="introduce_text" v-html="messages.what">

                </div>
                <div class="introduce_text mt_16">
                    <div class="padding_basic_2 pt_0 pb_0 px_0">
                        <div class="sheet_main_title">填写资料</div>
                        <div id="shopName" class="sheet_main grayline"
                            :class="{ alert: checkFailure(appointmentInfo.shopName) }">
                            <div class="title title_center">店铺名称</div>
                            <div class="content content_right">
                                <form class="form_full">
                                    <input class="input_style" placeholder="请输入最多7个字符" maxlength="7" 
                                        v-model="appointmentInfo.shopName"  >
                                </form>
                            </div>
                        </div>
                        <div id="introduction" class="sheet_main grayline"
                            :class="{ alert: checkFailure(appointmentInfo.introduction) }">
                            <div class="title title_center">店铺介绍</div>
                            <div class="content content_right">
                                <form class="form_full">
                                    <input class="input_style" placeholder="一句话概述您的店铺,最多十七个字" maxlength="17"
                                        v-model="appointmentInfo.introduction" >
                                </form>
                            </div>
                        </div>
                        <div id="girls" class="sheet_main grayline" :class="{ alert: checkFailure(appointmentInfo.girls) }">
                            <div class="title title_center">妹子数量</div>
                            <div class="content content_right">
                                <form class="form_full">
                                    <input class="input_style" placeholder="请输入数字1-99999" maxlength="5" type="tel"
                                      v-model="appointmentInfo.girls" @input="handInput($event,5)" @blur="Number.isNaN(parseInt(($event.target as HTMLInputElement).value,10))? appointmentInfo.girls=1:appointmentInfo.girls=parseInt(($event.target as HTMLInputElement).value,10)" >
                                </form>
                            </div>
                        </div>
                        <!-- <div id="contactApp" class="sheet_main grayline"
                            :class="{ alert: checkFailure(appointmentInfo.contactApp) }">
                            <div class="title title_center">联系软件</div>
                            <div class="content content_right">
                                <form class="form_full">
                                    <input class="input_style" placeholder="例如微信、QQ、电话、putato" maxlength="15"
                                        v-model="appointmentInfo.contactApp" >
                                </form>
                            </div>
                        </div> -->
                        <div id="contactInfo" class="sheet_main grayline"
                            :class="{ alert: checkFailure(appointmentInfo.contactInfo) }">
                            <div class="title title_center">QQ号码</div>
                            <div class="content content_right">
                                <form class="form_full">
                                    <input class="input_style" placeholder="请填写正确的QQ号码" maxlength="15"
                                        v-model="appointmentInfo.contactInfo">
                                </form>
                            </div>
                        </div>
                        <div id="dealOrder" class="sheet_main grayline"
                            :class="{ alert: checkFailure(appointmentInfo.dealOrder) }">
                            <div class="title title_center">成交订单</div>
                            <div class="content content_right">
                                <form class="form_full">
                                    <input class="input_style"  placeholder="请输入数字1—99999" maxlength="5"
                                        v-model="appointmentInfo.dealOrder" type="tel" @input="handInput($event,5)"  @blur="Number.isNaN(parseInt(($event.target as HTMLInputElement).value,10))? appointmentInfo.dealOrder=1:appointmentInfo.dealOrder=parseInt(($event.target as HTMLInputElement).value,10)" >
                                </form>
                            </div>
                        </div>
                        <div id="selfPopularity" class="sheet_main grayline"
                            :class="{ alert: checkFailure(appointmentInfo.selfPopularity) }">
                            <div class="title title_center">自评人气</div>
                            <div class="content content_right">
                                <form class="form_full">
                                    <input class="input_style"  placeholder="请输入数字1—99999" maxlength="5"
                                        v-model="appointmentInfo.selfPopularity" type="tel"  @input="handInput($event,5)" @blur="Number.isNaN(parseInt(($event.target as HTMLInputElement).value,10))? appointmentInfo.selfPopularity=1:appointmentInfo.selfPopularity=parseInt(($event.target as HTMLInputElement).value,10)">
                                </form>
                            </div>
                        </div>
                        <div id="shopYears" class="sheet_main grayline"
                            :class="{ alert: checkFailure(appointmentInfo.shopYears) }">
                            <div class="title title_center">店龄</div>
                            <div class="content content_right">
                                <form class="form_full">
                                    <input class="input_style"  placeholder="请输入数字1—99"  maxlength="2"
                                        v-model="appointmentInfo.shopYears" type="tel" @input="handInput($event,2)"  @blur="Number.isNaN(parseInt(($event.target as HTMLInputElement).value,10))? appointmentInfo.shopYears=1:appointmentInfo.shopYears=parseInt(($event.target as HTMLInputElement).value,10)" >
                                </form>
                            </div>
                        </div>
                        <!--弹框-->
                        <!-- <div class="sheet_main alert grayline">
                               <div class="title title_center">店龄</div>
                               <div class="content content_right">
                                   <form class="form_full">
                                       <input class="input_style" placeholder="请输入数字1—99"
                                           maxlength="2"
                                           v-model="appointmentInfo.shopYears"
                                       >
                                   </form>
                               </div>
                           </div> -->
                        <ApplyBossMediaSelection :title="selectTitle" :coverPhotoTitle="coverPhotoTitle"
                            :coverPhotoMax="coverPhotoMax" :coverPhotoSource="coverPhotoSource"
                            :shopPhotoSource="shopPhotoSource" :media="media" :max="maxCount" :plusBoxClass="appendBoxClass"
                            @show="showImageZoom"></ApplyBossMediaSelection>
                    </div>
                </div>
                <div class="introduce_title">
                    <CdnImage src="@/assets/images/post/pic_title_b_2.png" alt="" />
                </div>
                <div class="introduce_text">
                    <div class="list" v-html="messages.how">

                    </div>
                </div>
                <div class="introduce_btn" @click="verifyIdentity">
                    <div class="btn_default no_shadow">{{ subBtnText }}</div>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { NavigateRule, DialogControl, PlayGame, MediaCenter } from "@/mixins";
import api from "@/api";
import {
    AdvertisingContentType,
    PostType,
    SourceType,
    MediaType,
    IdentityType,
    IdentityApplyStatusType,
} from "@/enums";
import {
    WhatIsDataModel,
    OfficialShopDetailModel,
    MediaModel,
    MediaResultModel,
    MessageDialogModel,
    event
} from "@/models";
import toast from "@/toast";
import { ApplyBossMediaSelection, ImageZoom, CdnImage } from "@/components";
import { MutationType } from "@/store";

export default defineComponent({
    components: { ApplyBossMediaSelection, ImageZoom, CdnImage },
    mixins: [NavigateRule, DialogControl, PlayGame, MediaCenter],
    data() {

        
        return {
            messages: {} as WhatIsDataModel,
            selectTitle: "商家照片（选填）",
            coverPhotoTitle: "照片（将作为店铺头像展示）",
            coverPhotoMax: 1,
            maxCount: 3,
            coverPhotoSource: SourceType.BossApply,
            shopPhotoSource: SourceType.BusinessPhoto,
            media: MediaType.Image,
            appendBoxClass: "gray",
            appointmentInfo: { contactApp: "QQ" } as OfficialShopDetailModel,
            errorMessage: "",
            shopDetail: {} as OfficialShopDetailModel,
            isBossShopEdit: false,
            isCheckFailure: false,
            grisMaxLeng:5
        };

    },
    methods: {
       
        shopPhotoMax() {
            return this.appointmentInfo.businessPhotoSource.length > this.maxCount ? this.maxCount : this.appointmentInfo.businessPhotoSource.length;
        },
        loadBossShopDetails() {

            if (!this.isMyBossShopEdit) {
                return;
            }

            this.appointmentInfo = this.bossShopDetails;

            for (const item of this.appointmentInfo?.businessPhotoSourceViewModel) {

                if (item.fullMediaUrl.includes("avatar_store_default.png")) {
                    continue;
                }

                let result = this.getImageCache(item) as string;
                let subId = this.getImageID(item.fullMediaUrl);
                let fileName = `${subId}.${this.getBase64ImageEXT(result)}`;
                const image: MediaModel = {
                    bytes: result,
                    fileName: fileName,
                    sourceType: SourceType.BusinessPhoto,
                    mediaType: MediaType.Image,
                    id: item.id,
                    subId: item.fullMediaUrl,
                };
                this.mediaList.push(image);
            }
            const sl: MediaResultModel = {
                id: this.appointmentInfo.applyId,
                fullMediaUrl: this.appointmentInfo.shopAvatarSource
            };

            const shopAvatarSourceByte = this.getImageCache(sl) as string;
            let fileName = `${this.appointmentInfo.applyId}_shopAvatarSource.${this.getBase64ImageEXT(shopAvatarSourceByte)}`;
            const coverImage: MediaModel = {
                bytes: shopAvatarSourceByte,
                fileName: fileName,
                sourceType: SourceType.BossApply,
                mediaType: MediaType.Image,
                id: this.appointmentInfo.applyId,
                subId: this.appointmentInfo.shopAvatarSource,
            }
            this.mediaList.push(coverImage)

            this.$store.commit(MutationType.SetMediaSelect, this.mediaList);

        },
        showImageZoom(imageModel: MediaModel) {
            this.$store.commit(MutationType.SetIntroductionImageMode, true);
            this.imageZoomItem = imageModel;
        },
        deleteImageItem() {
            this.deleteImage(this.imageZoomItem);
            this.$store.commit(MutationType.SetIntroductionImageMode, false);
        },
        getImageCache(item: MediaResultModel) {
            const container = this.$store.state.imageCache.get(item.id);
            if (container) {
                return container.get(item.fullMediaUrl) || "";
            }
        },
        async applyUploadImages(sourceType: SourceType) {
            const images = await this.uploadImagesBySourType(sourceType);
            if (!images.length) {
                this.$store.commit(MutationType.SetIsLoading, false);
                this.errorMessage = sourceType === SourceType.BossApply ? "店铺封面图片上传失败!" : "店铺图片上传失败";
                return;
            }
            const result = images.map((image) => image.id);
            return result;
        },
        checkFailure(item: number | string | any[]) {
            return (
                    ((Array.isArray(item) && !item.length) || !item ) && this.isCheckFailure
                );
        },
        isNumber(value: string) {
            return /^[0-9]+$/.test(value);
        },
        async verifyIdentity() {
            if (!this.userInfo.hasPhone) {
                const messageModel: MessageDialogModel = {
                    message: "需要绑定手机号码才能申请觅经纪/觅老板",
                    cancelTitle: "前往绑定",
                    buttonTitle: "暂不绑定",
                    cancelButtonEnable: true,
                };

                this.showMessageDialog(messageModel, async () => {
                    this.goBindPhoneUrl();
                });
                return;
            }

            const tagString = this.checkProperties(this.appointmentInfo);
            this.isCheckFailure = !!tagString;
            if (this.isCheckFailure) {
                if (tagString == "invalidValue") {
                    if (this.appointmentInfo.shopName.length > 7 ) {
                        toast("请输入最多7个字符");
                    } else if (this.appointmentInfo.introduction.length > 17) {
                        toast("请输入最多17个字符");
                    } else if (this.appointmentInfo.girls > 99999 || this.appointmentInfo.dealOrder > 99999 || this.appointmentInfo.selfPopularity > 99999) {
                        toast("请输入数字1-99999");
                    } else if (this.appointmentInfo.shopYears > 99) {
                        toast("请输入数字1-99");
                    }else if(this.appointmentInfo.girls <=0 || this.appointmentInfo.dealOrder <=0 || this.appointmentInfo.selfPopularity <=0 ||this.appointmentInfo.shopYears <=0){
                        toast("请输入大于0的数字");
                    }
                }else if(tagString=="trim")
                {
                    toast("请勿输入全空格");
                }else {
                    const top = document.getElementById(tagString)?.scrollIntoView();
                    window.scrollTo(0, top || 0);
                }
                return;
            } else if (!this.imageSelect.filter(c => c.sourceType === SourceType.BossApply).length) {
                toast("请上传店铺封面照片");
            }
            else if(this.userInfo.identity != IdentityType.General && !this.isMyBossShopEdit){
                toast("您已经有身份了，不允许重复申请");

            }
            else
            {


                if(this.userInfo.isApplyBoss && !this.isMyBossShopEdit){
                    toast("您已申请过觅老板，请连系客服人员调整身份");
                    return;
                }


                let identityApplyStatus = await api.getCertificationInfo();
                if (identityApplyStatus.applyStatus === IdentityApplyStatusType.Applying) {
                    toast("您的申请已经提交,请耐心等候!");
                    return;
                }


                if (identityApplyStatus.applyStatus === IdentityApplyStatusType.Pass && !this.isMyBossShopEdit) {
                    toast("您的申请已经通过,不允许重复提交申请");
                    return;
                }
                let isApplyBoss=await api.getUserIdIsApplyBoss();

                if(isApplyBoss && !this.isMyBossShopEdit){
                    toast("您已申请过觅老板，请连系客服人员调整身份");
                    return;
                }

                try {

                    this.$store.commit(MutationType.SetIsLoading, true);
                    let shopAvatarSource: string[] = await this.applyUploadImages(SourceType.BossApply) as string[];
                    this.appointmentInfo.shopAvatarSource = shopAvatarSource.toString();

                    if (this.imageSelect.filter(c => c.sourceType === SourceType.BusinessPhoto).length > 0) {
                        let shopPhoto: string[] = await this.applyUploadImages(SourceType.BusinessPhoto) as string[];
                        this.appointmentInfo.businessPhotoSource = shopPhoto;
                    }else
                    {
                        this.appointmentInfo.businessPhotoSource=[];
                    }
                    
                    await api.bossIdentityApplyOrUpdate(this.appointmentInfo);
                    this.navigateToApply("觅老板申请");
                } catch (error) {
                    const message = error instanceof Error ? (error as Error).message : (error as string);
                    toast(message);
                }
                finally {
                    this.$store.commit(MutationType.SetIsLoading, false);
                }
            }
        },
       
        checkProperties(obj: OfficialShopDetailModel): string {
          
            if (!obj.shopName) {
                return `shopName`;
            }
            if (!obj.introduction) {
                return `introduction`;
            }
            if (!obj.girls) {
                return `girls`;
            }
            // if (!obj.contactApp) {
            //     return `contactApp`;
            // }
            if (!obj.contactInfo) {
                return `contactInfo`;
            }
            if (!obj.dealOrder) {
                return `dealOrder`;
            }
            if (!obj.selfPopularity) {
                return `selfPopularity`;
            }
            if (!obj.shopYears) {
                return `shopYears`;
            }
            if(!obj.shopName.trim() || !obj.introduction.trim()  || !obj.contactInfo.trim() )
            {
                return `trim`;
            }
            if (
                obj.shopName.length > 7 ||
                obj.introduction.length > 17 ||
                obj.girls > 99999 ||
                obj.dealOrder > 99999 ||
                obj.selfPopularity > 99999 ||
                obj.shopYears > 99 ||
                obj.girls <=0 ||
                obj.dealOrder <=0 ||
                obj.selfPopularity <=0 ||
                obj.shopYears <=0
            ) {
                return `invalidValue`;
            }
            return "";
        },
        showAlert() {
            setTimeout(() => {
                this.errorMessage = "";
            }, 1000);
        },
        async getWhatIs() {
            try {
                this.messages = await api.getWhatIs(PostType.Square, AdvertisingContentType.SeekBoss);
            } catch (error) {
                toast(error);
            }
        },
        getBase64ImageEXT(base64: string) {
            let re = new RegExp('data:image/(?<ext>.*?);base64,.*')
            let res = re.exec(base64)
            if (res) {
                return res.groups?.ext
            }
        },
        handblur:()=>{
                
        },
        handInput:(event:Event,maxLength:number):void =>{
           
            //·~！@￥
            const input = event.target as HTMLInputElement;
            //input.value=input.value.replace('-','').replace('.','').replace('+','').replace("#",'').replace("*",'').replace('·','').replace('~','').replace("!","").replace("@","").replace("$","").replace("￥","");
            input.value=input.value.replace(/\D/g, '');
            if (input.value.length > maxLength) {
                input.value = input.value.replace('-','').slice(0, maxLength); // 截取前5个字符    
            }
            
        }
    },
    watch: {
        errorMessage(content: String) {
            if (content) {
                this.showAlert();
            }
        }
    },
    async created() {
        await this.getWhatIs();
        this.loadBossShopDetails();
    },
   
    computed: {

        certificationStatus() {
            return this.$store.state.certificationStatus;
        },
        imageZoomSwitch() {
            return this.$store.state.isImageZoomMode || false;
        },
        bossShopDetails() {
            return this.$store.state.myBossShopDetail;
        },
        subBtnText() {
            return this.isMyBossShopEdit ? "申请更新资料" : "申请觅老板";
        },
        mediaList() {
            return this.$store.state.mediaSelectList as MediaModel[];
        },
        isMyBossShopEdit() {
            return this.$store.state.isBossShopEdit;
        } ,
    },
})
</script>