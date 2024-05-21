<template>
                <div class="officialstore_container">
                    <div class="bg_circlemask_top"></div>
                    <div class="officialstore_wrapper">
                        <div class="officialstore_outer">
                            <div class="officialstore_edit">
                                <div class="officialstore_editicon"><CdnImage src="@/assets/images/card/ic_boss_edit.svg"/></div>
                                <div class="officialstore_edittext"  v-if="shopDetail.isEditAudit">审核中</div>
                                <div class="officialstore_edittext"  v-else  @click="editShopInfo(shopDetail.applyId)">编辑资料</div>
                            </div>
                            <div class="officialstore_top">
                                <div class="officialstore_avatar">
                                    <AssetImage :item="setImageItem(shopDetail)" />
                                </div>
                                <div class="officialstore_inner">
                                    <div class="officialstore_inner_top">
                                        <div class="officialstore_sheet type">
                                            <div class="official_title_name" :data-text="shopDetail.shopName">{{shopDetail.shopName}}</div>
                                        </div>
                                    </div>
                                    <div class="officialstore_inner_bottom">
                                        <div class="officialstore_flex">
                                            <div class="officialstore_label blue">店龄{{shopDetail.shopYears}}年</div>
                                            <div class="officialstore_label purple">妹子{{shopDetail.girls}}位</div>
                                        </div>
                                        <div class="officialstore_flex">
                                            <div class="officialstore_label copper">{{shopDetail.dealOrder}}已约过</div>
                                            <div class="officialstore_label green">评分{{shopDetail.selfPopularity  }}</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="officialstore_bottom">
                                <div class="officialstore_londspeaker"><CdnImage src="@/assets/images/index/ic_loudspeaker.png"/><span>介绍：</span><div class="londspeaker_text">{{shopDetail.introduction}}</div></div>
                             
                            </div>
                        </div>
                    </div>
                </div>
                <!-- 店舖資訊 end -->

                <!-- 共同滑動區塊 -->
                <div class="flex_height">
                    <div class="bg_circlemask">
                        <div class="officialstore_tabs">
                            <div class="officialstore_tab" :class="{ active: isPostPage }" @click="resetPage">商品</div>
                            <div class="officialstore_tab" :class="{ active: !isPostPage }" @click="resetPage">商家信息</div>
                        </div>

                        <div class="officialstore_height overflow no_scrollbar" v-show="!isPostPage">
                            <div class="activate">
                                <div class="icon"><CdnImage src="@/assets/images/post/ic_time.svg"/></div>
                                <div class="text">
                                    <div class="business_title">营业时间</div>
                                    <div class="business_outer"   >
                                        <div class="business_icon"><CdnImage src="@/assets/images/post/ic_business_week.svg"/></div>
                                        <div class="business_text">营业时段：{{ shopDetail.businessDate }}</div>
                                        <div class="business_icon" @click="editDoBusinessTimePeriod(shopDetail.bossId)"><CdnImage src="@/assets/images/post/ic_business_edit.svg"/></div>
                                    </div>
                                    <div class="business_outer"   >
                                        <div class="business_icon"><CdnImage src="@/assets/images/post/ic_business_time.svg"/></div>
                                        <div class="business_text">营业时间：{{ shopDetail.businessHour }}</div>
                                        <div class="business_icon" @click="editDoBusinessTime(shopDetail.bossId)"><CdnImage src="@/assets/images/post/ic_business_edit.svg"/></div>
                                    </div>
                                </div>
                            </div>
                            <div class="list_title">
                                <div class="line"></div>
                                <div class="text">商家照片</div>
                            </div>
                            <div class="store_photo_wrapper">
                                <div class="store_photo" v-for="photoSource in businessPhotoSources">
                                  
                                    <CdnImage :src="photoSource.fullMediaUrl" alt="" v-if="isShowDefault(photoSource.fullMediaUrl)" />
                                    <AssetImage :item="setShopImage(photoSource)" v-else />
                                </div>
    
                            </div>
                            <div class="list_title">
                                <div class="line"></div>
                                <div class="text">商家承诺</div>
                            </div>
                            <div class="service_info">
                                <div class="list">
                                    <div class="icon"><CdnImage src="@/assets/images/post/ic_info_assure.svg" alt=""/></div>
                                    <div class="text">
                                        <p>从业保障：</p>
                                        <p>多年口碑经营 可验证信息</p>
                                    </div>
                                </div>
                                <div class="list">
                                    <div class="icon"><CdnImage src="@/assets/images/post/ic_info_safty.svg" alt=""/></div>
                                    <div class="text">
                                        <p>安全保障：</p>
                                        <p>商家入驻已缴纳保证金</p>
                                    </div>
                                </div>
                                <div class="list">
                                    <div class="icon"><CdnImage src="@/assets/images/post/ic_info_serviceitem.svg" alt=""/></div>
                                    <div class="text">
                                        <p>服务项目：</p>
                                        <p>售前咨询 售后服务</p>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="officialstore_infos_wrapper">
                                <div class="officialstore_infos_outer">
                                    <div class="overflow no_scrollbar">
                                        <div class="officialstore_cities">
                                            <div class="city_outer" :class="{ active: areaCode == '' }">
                                                <div class="city_inner" @click="resetAreaCode('')">
                                                    <div class="city_inner_name">全部</div>
                                                </div>
                                            </div>

                                            <div class="city_outer" :class="{ active: areaCode == province.province }" v-for="province in showProvinceList">
                                                <div class="city_inner" @click="resetAreaCode(province.province)">
                                                    <div class="city_inner_name">{{province.name}}</div>
                                                </div>
                                            </div>

                                           
                                        </div>
                                    </div>
                                </div>

                                <EmptyView :message="`暂无数据～`" v-if="isEmpty"></EmptyView>

                                <div class="officialstore_cards_outer" >
                                    <div class="overflow no_scrollbar w_100" @scroll="onScroll" ref="scrollContainer" >

                                        <div class="officialstore_cards" v-show="isShowAddPost()">
                                            <div class="card_outer add" >
                                                <div class="card_inner" @click="posting()">
                                                    <div class="box">
                                                        <form>
                                                            <label for="upload_btn">
                                                                <div class="icon">
                                                                    <CdnImage src="@/assets/images/element/ic_upload.svg" alt=""/>
                                                                </div>
                                                            </label>
                                                            <!-- <input id="upload_btn" type="file"> -->
                                                        </form>
                                                    </div>
                                                    <div class="card_add">添加帖子</div>
                                                </div>
                                            </div>

                                            <div class="card_outer" v-show="firstDataModel.postId">
                                                <div class="card_inner">
                                                    <div class="card_top">
                                                        <div class="card_top_img">   <AssetImage :item="setOfficialPostModelImageItem(firstDataModel)" /></div>
                                                        <div class="card_top_play"><CdnImage src="@/assets/images/card/ic_card_play.svg"/></div>
                                                        <div class="card_btns">
                                                            <div class="card_btn_outer red" @click="soldOutPost(firstDataModel.postId)"><div class="card_btn_inner">下架</div></div>
                                                            <!-- <div class="card_btn_outer midnight"><div class="card_btn_inner">编辑</div></div> -->
                                                        </div>
                                                    </div>
                                                    <div class="card_bottom">
                                                        <div class="official_title"><div class="official_name">{{firstDataModel.title}}</div></div>
                                                        <div class="info_co">
                                                            <div class="info">
                                                                <div class="icon"><CdnImage src="@/assets/images/card/ic_card_body_info.svg" alt=""/>
                                                                </div>
                                                                <div>{{ firstDataModel.height }}cm</div>
                                                                <div class="dot"></div>
                                                                <div>{{ firstDataModel.age }}岁</div>
                                                                <div class="dot"></div>
                                                                <div>{{firstDataModel.cup}}杯</div>
                                                            </div>
                                                        </div>
                                                        <p>期望收入：￥{{ firstDataModel.lowPrice}}</p>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="officialstore_cards" v-for="postList in sortedPostList">
                                            <div class="card_outer" v-for="post in postList">
                                                <div class="card_inner">
                                                    <div class="card_top">
                                                        <div class="card_top_img"> <AssetImage :item="setOfficialPostModelImageItem(post)" /></div>
                                                        <div class="card_top_play"><CdnImage src="@/assets/images/card/ic_card_play.svg"/></div>
                                                        <div class="card_btns">
                                                            <div class="card_btn_outer red" @click="soldOutPost(post.postId)"><div class="card_btn_inner">下架</div></div>
                                                            <!-- <div class="card_btn_outer midnight"><div class="card_btn_inner">编辑</div></div> -->
                                                        </div>
                                                    </div>
                                                    <div class="card_bottom">
                                                        <div class="official_title"><div class="official_name">{{showShopTitle(post.title)}}</div></div>
                                                        <div class="info_co">
                                                            <div class="info">
                                                                <div class="icon"><CdnImage src="@/assets/images/card/ic_card_body_info.svg" alt=""/>
                                                                </div>
                                                                <div>{{ post.height }}cm</div>
                                                                <div class="dot"></div>
                                                                <div>{{ post.age }}岁</div>
                                                                <div class="dot"></div>
                                                                <div>{{post.cup}}杯</div>
                                                            </div>
                                                        </div>
                                                        <p>期望收入：￥{{ post.lowPrice}}</p>
                                                    </div>
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
import { defineComponent } from "vue";
import { MutationType } from "@/store";
import { NavigateRule, DialogControl, PlayGame,Tools,ImageCacheManager,VirtualScroll, ScrollManager } from "@/mixins";
import { IntroductionType} from "@/enums";
import { EmptyView, AssetImage, CdnImage } from "@/components";
import api from "@/api";
import toast from "@/toast";
import {

  OfficialShopModel,
  OfficialShopDetailModel,
  ChinaCityInfo,
  ImageItemModel,
  OfficialPostModel,
  PageParamModel,
  MyOfficialPostQueryParamModel,
  MediaResultModel,
  PopupModel,
  OptionItemModel
} from "@/models";


export default defineComponent({
    mixins: [NavigateRule, DialogControl, PlayGame,Tools,ImageCacheManager,VirtualScroll, ScrollManager],
    components: {
        EmptyView,
        AssetImage,
        CdnImage,
    },
    data() {
        return{
            shopDetail: {} as OfficialShopDetailModel,
            areaCode: "",
            paramterAreaCode:[] as  string [],
            totalPage:1,      
            officialPostList: [] as OfficialPostModel[],
            officialPostListParam: {} as MyOfficialPostQueryParamModel,
            firstDataModel:{} as OfficialPostModel,
            isPostPage: true,
            provinceList: require("@/assets/json/province.json") as ChinaCityInfo[],
            cityList:require("@/assets/json/city.json") as ChinaCityInfo[],
            showProvinceList:[] as ChinaCityInfo[],
            isEmpty:false,
            selectDoBusinessTime:"",  
            isCheckFailure: false,
            selectDoBusinessTimePeriod:"",
        }
    },
    methods:{   
       
        checkFailure(item: number | string | any[]) {
            return (
                ((Array.isArray(item) && !item.length) || !item) && this.isCheckFailure
            );
        },
        showDoBusinessTime(bossId:string) {

            let currentBusinessTime:OptionItemModel[]=[
                {
                    key:0,
                    value:""
                }
            ]
            if(this.bossShopDetails.businessHour){
                currentBusinessTime[0].value=this.bossShopDetails.businessHour;
            }
            const infoModel: PopupModel = {
                title: "请选择营业时间",
                content: currentBusinessTime,
                isMultiple: false,
            };
            this.showShopSelectDoBusinessTime(infoModel, async (selectTime) => {
                this.selectDoBusinessTime=selectTime as string;
                try{
                    var result= await api.editShopDoBusinessTime(bossId,this.selectDoBusinessTime,2);
                }
                catch(e){
                    toast(e);
                }finally{
                    this.shopDetail.businessHour=this.selectDoBusinessTime;
                }
            });
        },
        showDoBusinessTimePeriod(bossId:string){
            let currentBusinessTimePeriod:OptionItemModel[]=[
                {
                    key:0,
                    value:""
                }
            ]
            if(this.bossShopDetails.businessDate){
                currentBusinessTimePeriod[0].value=this.bossShopDetails.businessDate;
            }
            const infoModel: PopupModel = {
                title: "请选择营业时段",
                content: currentBusinessTimePeriod,
                isMultiple: false,
            };
            this.showShopSelectDoBusinessTimePeriod(infoModel, async (selectTime) =>  { 
                this.selectDoBusinessTimePeriod=selectTime as string;
                try{
                    var result= await api.editShopDoBusinessTime(bossId,this.selectDoBusinessTimePeriod,1);
                }
                catch(e){
                    toast(e);
                }finally{
                    this.shopDetail.businessDate=this.selectDoBusinessTimePeriod;
                }
            });
        },  
        async editDoBusinessTimePeriod(bossId:string){

            if(bossId=='' || bossId==undefined)
            {
                toast("请刷新页面后重试!");
                return;
            }
            this.showDoBusinessTimePeriod(bossId);
        }, 
        async editDoBusinessTime(bossId:string) {  
            if(bossId=='' || bossId==undefined)
            {
                toast("请刷新页面后重试!");
                return;
            }
            this.showDoBusinessTime(bossId);
        },
        posting(){
            this.navigateToOfficialFrom();
        },
        editShopInfo(applyId:string){
            this.$store.commit(MutationType.SetBossIsEdit, true);
            this.$store.state.publishName=IntroductionType.Boss;
            this.navigateToApplyBossShopEdit(applyId);
            
        },
        resetPage() {
            this.isPostPage = !this.isPostPage;
        },
        async loadShopInfo(){
            try
            {

                this.$store.commit(MutationType.SetIsLoading, true);
                this.shopDetail = await api.getMyOfficialShopDetail();
                const medias = [{
                    applyId: this.shopDetail.applyId,
                    shopAvatarSource: this.shopDetail.shopAvatarSource
                }] as OfficialShopModel[];

                this.selectProvinceList(this.shopDetail.areaCodes);
                this.officialDownload(medias);
                this.officialShopImage(this.shopDetail.businessPhotoSourceViewModel);
                this.$store.commit(MutationType.SetMyBossShopDetail, this.shopDetail);
            } catch (e) {
                toast(e);
            }
            finally{
                this.$store.commit(MutationType.SetIsLoading, false);
            }
        },
        setImageItem(info: OfficialShopDetailModel) {
           
            const subId = this.getImageID(info.shopAvatarSource);
            let item: ImageItemModel = {
                id: info.applyId,
                subId: info.shopAvatarSource,
                class: "",
                src: info.shopAvatarSource,
                alt: "",
            };
            return item;
        },
        setShopImage(info:MediaResultModel)
        {
            let item: ImageItemModel = {
                id: info.id,
                subId: info.fullMediaUrl,
                class: "",
                src: info.fullMediaUrl,
                alt: "",
            };
            return item;
        },
        setOfficialPostModelImageItem(info: OfficialPostModel) {
            const subId = this.getImageID(info?.coverUrl);
            let item: ImageItemModel = {
                id: info?.postId,
                subId: subId,
                class: "",
                src: info?.coverUrl,
                alt: "",
            };
            return item;
        },
        resetAreaCode(areaCode: string) {

            this.areaCode=areaCode;
            let cityInfo= this.cityList.filter(c=>c.province===areaCode);

            if(cityInfo===undefined){
                toast("找不到所属城市!");
                return ;
            }
            this.paramterAreaCode = cityInfo.map(c=>c.code);
            this.reload();
        }, 
        async reload() {
            this.resetPageInfo();
            this.resetScroll();
            this.officialPostList = [];
            this.firstDataModel={} as OfficialPostModel;
            await this.loadPost();
        },
        async loadPost() {
            if (this.isLoading || this.scrollStatus.totalPage === this.pageInfo.pageNo || this.isPromising ) return;
            try {
                this.$store.commit(MutationType.SetIsLoading, true);
                this.officialPostListParam.areaCode = this.paramterAreaCode;
                this.officialPostListParam.pageNo = this.pageInfo.pageNo + 1;
                const result = await api.getMyOfficialPostList(this.officialPostListParam);
                if(result.data!=null && result.data.length>0){

                    this.myOfficialPostDownload(result.data);
                    this.scrollStatus.totalPage = result.totalPage;
                    this.officialPostList = this.officialPostList.concat(result.data);
                    this.isEmpty = !result.data.length;
                    this.pageInfo.pageNo = result.pageNo;
                    if(this.areaCode===''){
                       
                        this.firstDataModel=this.officialPostList[0];
                        if(this.firstDataModel!=null && this.firstDataModel!=undefined){
                             this.firstDataModel.title.length>=7?this.firstDataModel.title.substring(0,6)+"...":this.firstDataModel.title;
                        }
                        this.officialPostList.shift();
                    }
                }else{
                    await this.loadShopInfo();
                }

            } catch (error) {
                console.error(error);
            } finally {
                this.$store.commit(MutationType.SetIsLoading, false);
            }
        },
        async $_onScrollToBottom() {
            await this.loadPost();
        },
        selectProvinceList(selectProvince:string[]){
            const provinces=this.cityList.filter(c=>selectProvince.includes(c.code)).map(c=>c.province);
            const uniqueProvinces=provinces.filter((value,index,self)=>self.indexOf(value)===index);
            return this.showProvinceList=this.provinceList.filter(c=>uniqueProvinces.includes(c.province));
        },
        async soldOutPost(postId:string){
            this.$store.commit(MutationType.SetIsLoading, true);
            try {
                let postIds:string[]=[postId];
                await api.setShelfOfficialPost(postIds,1);
                toast("操作成功");
            } catch (e) {
                toast(e);
            }
            finally{
                this.$store.commit(MutationType.SetIsLoading, false);
                await this.reload();
            }

        },
        isShowAddPost(){
            return this.areaCode==='';
        },
        showShopTitle(postTitle:string)
        {
            return postTitle.length>=7?postTitle.substring(0,6)+"...":postTitle;
        }, 
        isShowDefault(src:string){
            return src.indexOf("avatar_store_default.png")>-1?true:false;
        }
    },
    computed:{
        applyId() {
           return this.shopDetail.applyId;
        },    
        sortedPostList() {
            let presentPostList = [];
            let length = this.officialPostList.length % 2 === 0 ? this.officialPostList.length / 2 : Math.floor(this.officialPostList.length / 2 + 1);
            for (let i = 0; i < length; i++) {
                let temp = this.officialPostList.slice(i * 2, i * 2 + 2);
                presentPostList.push(temp);
            }
            return presentPostList;
        },
        pageInfo() {
            return {
                pageNo: 0,
                pageSize: 10,
            } as PageParamModel;
        }, 
        $_virtualScrollItemElemHeight() {
            return 200;
        },
        businessPhotoSources() {

            let imageArray:MediaResultModel[]= this.shopDetail.businessPhotoSourceViewModel;
            if (imageArray) {

                if (imageArray.length > 3) {
       
                    return imageArray.slice(0, 3);
                }
                else {
                    while(imageArray.length<3)
                    {
           
                        let imagModel:MediaResultModel={
                            id:"@/assets/images/card/avatar_store_default.png",
                            fullMediaUrl:"@/assets/images/card/avatar_store_default.png"
                        }
                        imageArray.push(imagModel);
                    }
                }
            }
            return imageArray;
        },
        bossShopDetails(){
            return  this.$store.state.myBossShopDetail;
        },
    },
    async created(){
        await this.reload();
        await this.loadShopInfo();
    }
})
</script>
