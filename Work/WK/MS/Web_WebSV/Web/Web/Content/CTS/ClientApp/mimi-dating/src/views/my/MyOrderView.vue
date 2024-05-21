<template>
    <div class="main_container_flex">
                <!-- Header start -->
                <header class="header_height1 solid_color">
                    <div class="header_back">
                        <div><img src="@/assets/images/header/ic_header_arrow.svg" alt=""></div>
                    </div>
                    <div class="header_title">我的订单</div>
                </header>
                <!-- Header end -->
                <!-- filter tab -->
                <div class="filter_tab">
                    <ul>
                        <li class="active">
                            <div class="text">全部</div>
                            <div class="tab_bottom_line"></div>
                        </li>

                        <li>
                            <div class="text">待确认</div>
                            <div class="tab_bottom_line"></div>
                        </li>
                        <li>
                            <div class="text">已确认</div>
                            <div class="tab_bottom_line"></div>
                        </li>
                    </ul>
                </div>
                <!-- filter tab end -->

                <!-- 共同滑動區塊 -->
                <div class=" flex_height" v-if="isEmpty">
                    <div class="overflow no_scrollbar">
                        <div class="me_order_list">
                            <div class="list">
                                <div class="photo"><img src="@/assets/images/card/pic.png"></div>
                                <div class="box">
                                    <div class="mix">
                                        <div class="text title">99年健身教练</div>
                                        <div class="basic_co">
                                            <!-- <div class="tag">已退款</div> -->
                                        </div>
                                    </div>
                                    <div class="mix align_center">
                                        <div class="text sub">支付金额：500钻石</div>
                                        <div class="basic_co">
                                            <div class="btn">评论</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="list">
                                <div class="photo"><img src="@/assets/images/card/pic5.jpeg"></div>
                                <div class="box">
                                    <div class="mix">
                                        <div class="text title">99年健身教练</div>
                                        <div class="basic_co">
                                            <div class="tag">已退款</div>
                                        </div>
                                    </div>
                                    <div class="mix align_center">
                                        <div class="text sub">支付金额：500钻石</div>
                                        <div class="basic_co">
                                            <!-- <div class="btn">评论</div> -->
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="list">
                                <div class="photo"><img src="@/assets/images/card/pic3.jpeg"></div>
                                <div class="box">
                                    <div class="mix">
                                        <div class="text title">99年健身教练</div>
                                        <div class="basic_co">
                                            <!-- <div class="tag">已退款</div> -->
                                        </div>
                                    </div>
                                    <div class="mix align_center">
                                        <div class="text sub">支付金额：500钻石</div>
                                        <div class="basic_co">
                                            <div class="btn">评论</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="list">
                                <div class="photo"><img src="@/assets/images/card/pic5.jpeg"></div>
                                <div class="box">
                                    <div class="mix">
                                        <div class="text title">99年健身教练</div>
                                        <div class="basic_co">
                                            <!-- <div class="tag">已退款</div> -->
                                        </div>
                                    </div>
                                    <div class="mix align_center">
                                        <div class="text sub">支付金额：500钻石</div>
                                        <div class="basic_co">
                                            <div class="btn">评论</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="list">
                                <div class="photo"><img src="@/assets/images/card/pic5.jpeg"></div>
                                <div class="box">
                                    <div class="mix">
                                        <div class="text title">99年健身教练</div>
                                        <div class="basic_co">
                                            <!-- <div class="tag">已退款</div> -->
                                        </div>
                                    </div>
                                    <div class="mix align_center">
                                        <div class="text sub">支付金额：500钻石</div>
                                        <div class="basic_co">
                                            <div class="btn">评论</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="list">
                                <div class="photo"><img src="@/assets/images/card/pic4.jpeg"></div>
                                <div class="box">
                                    <div class="mix">
                                        <div class="text title">99年健身教练</div>
                                        <div class="basic_co">
                                            <div class="tag">已退款</div>
                                        </div>
                                    </div>
                                    <div class="mix align_center">
                                        <div class="text sub">支付金额：500钻石</div>
                                        <div class="basic_co">
                                            <!-- <div class="btn">评论</div> -->
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="list">
                                <div class="photo"><img src="@/assets/images/card/pic2.png"></div>
                                <div class="box">
                                    <div class="mix">
                                        <div class="text title">99年健身教练</div>
                                        <div class="basic_co">
                                            <!-- <div class="tag">已退款</div> -->
                                        </div>
                                    </div>
                                    <div class="mix align_center">
                                        <div class="text sub">支付金额：500钻石</div>
                                        <div class="basic_co">
                                            <div class="btn">评论</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class=" flex_height" v-else>
                    <div class="overflow no_scrollbar">
                        <div class="empty_state_content">
                            <div class="empty_state">
                                <div class="icon"><img src="@/assets/images/public/pic_empty.png" alt=""></div>
                                <div class="nodata">还没有预约单哟～</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  data(){
    return {
        isEmpty:false
    }
  }
})
</script>
