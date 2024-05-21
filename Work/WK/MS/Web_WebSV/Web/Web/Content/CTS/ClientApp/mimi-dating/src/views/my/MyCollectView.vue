<template>
  <div class="main_container_flex">
    <!-- Header start -->
    <header class="header_height1 solid_color">
      <div class="header_back" @click="navigateToPrevious">
        <div>
          <img src="@/assets/images/header/ic_header_arrow.svg" alt="" />
        </div>
      </div>
      <div class="header_title">我的收藏</div>
    </header>
    <!-- Header end -->
    <!-- filter tab -->
    <div class="filter_tab">
      <ul>
        <li class="active">
          <div class="text">广场区</div>
          <div class="tab_bottom_line"></div>
        </li>
        <li @click="showComingSoon">
          <div class="text">担保区</div>
          <div class="tab_bottom_line"></div>
        </li>
        <li @click="showComingSoon">
          <div class="text">官方区</div>
          <div class="tab_bottom_line"></div>
        </li>
        <li @click="showComingSoon">
          <div class="text">体验区</div>
          <div class="tab_bottom_line"></div>
        </li>
      </ul>
    </div>
    <!-- filter tab end -->

    <!-- 共同滑動區塊 -->
    <EmptyView :message="`还没有收藏帖子哟～`"  v-if="isEmpty"></EmptyView>   
    <div class="flex_height" v-else>
      <div class="overflow no_scrollbar">
        <div class="text_notice normal">
          与觅友评论分享你的真实体验吧！每次都可参与空降抽奖
        </div>
        <div class="padding_basic">
          <div class="me_product_view">
            <ul class="full_cards">
              <li class="full_card no_premium_tag">
                <div class="location_tag">
                  <div>
                    <img
                      src="@/assets/images/card/ic_card_location.svg"
                      alt=""
                    />新疆
                  </div>
                </div>
                <div class="card_text">
                  <h1>99年健身教练99年健身教练99年健身教练</h1>
                  <div class="info_co">
                    <div class="info">
                      <div class="icon">
                        <img
                          src="@/assets/images/card/ic_card_body_info.svg"
                          alt=""
                        />
                      </div>
                      <div>162cm</div>
                      <div class="dot"></div>
                      <div>23岁</div>
                      <div class="dot"></div>
                      <div>C杯</div>
                    </div>
                  </div>
                  <div class="info_co">
                    <div class="info hobby">
                      <div class="icon">
                        <img
                          src="@/assets/images/card/ic_card_hobby.svg"
                          alt=""
                        />
                      </div>
                      <div>购物</div>
                      <div class="dot"></div>
                      <div>SPA</div>
                      <div class="dot"></div>
                      <div>旅游</div>
                    </div>
                  </div>
                  <p>职业：自由职业自由职业自由职业自由职业</p>
                  <p>期望收入：¥10000</p>
                  <div class="user_view">
                    <div class="num_total">
                      <ul>
                        <li>
                          <img
                            src="@/assets/images/card/ic_card_num_like.svg"
                            alt=""
                          />16
                        </li>
                        <li>
                          <img
                            src="@/assets/images/card/ic_card_num_chat.svg"
                            alt=""
                          />16
                        </li>
                        <li>
                          <img
                            src="@/assets/images/card/ic_card_num_watch.svg"
                            alt=""
                          />16
                        </li>
                      </ul>
                    </div>
                    <div class="num_time">1小时前更新</div>
                  </div>
                </div>
                <div class="photo">
                  <img src="@/assets/images/card/pic4.jpeg" alt="" />
                </div>
              </li>
              <li class="full_card no_premium_tag">
                <div class="location_tag">
                  <div>
                    <img
                      src="@/assets/images/card/ic_card_location.svg"
                      alt=""
                    />新疆
                  </div>
                </div>
                <div class="card_text">
                  <h1>99年健身教练99年健身教练99年健身教练</h1>
                  <div class="info_co">
                    <div class="info">
                      <div class="icon">
                        <img
                          src="@/assets/images/card/ic_card_body_info.svg"
                          alt=""
                        />
                      </div>
                      <div>162cm</div>
                      <div class="dot"></div>
                      <div>23岁</div>
                      <div class="dot"></div>
                      <div>C杯</div>
                    </div>
                  </div>
                  <div class="info_co">
                    <div class="info hobby">
                      <div class="icon">
                        <img
                          src="@/assets/images/card/ic_card_hobby.svg"
                          alt=""
                        />
                      </div>
                      <div>购物</div>
                      <div class="dot"></div>
                      <div>SPA</div>
                      <div class="dot"></div>
                      <div>旅游</div>
                    </div>
                  </div>
                  <p>职业：自由职业自由职业自由职业自由职业</p>
                  <p>期望收入：¥10000</p>
                  <div class="user_view">
                    <div class="num_total">
                      <ul>
                        <li>
                          <img
                            src="@/assets/images/card/ic_card_num_like.svg"
                            alt=""
                          />16
                        </li>
                        <li>
                          <img
                            src="@/assets/images/card/ic_card_num_chat.svg"
                            alt=""
                          />16
                        </li>
                        <li>
                          <img
                            src="@/assets/images/card/ic_card_num_watch.svg"
                            alt=""
                          />16
                        </li>
                      </ul>
                    </div>
                    <div class="num_time">1小时前更新</div>
                  </div>
                </div>
                <div class="photo">
                  <img src="@/assets/images/card/pic5.jpeg" alt="" />
                </div>
              </li>
              <li class="full_card no_premium_tag">
                <div class="location_tag">
                  <div>
                    <img
                      src="@/assets/images/card/ic_card_location.svg"
                      alt=""
                    />新疆
                  </div>
                </div>
                <div class="card_text">
                  <h1>99年健身教练99年健身教练99年健身教练</h1>
                  <div class="info_co">
                    <div class="info">
                      <div class="icon">
                        <img
                          src="@/assets/images/card/ic_card_body_info.svg"
                          alt=""
                        />
                      </div>
                      <div>162cm</div>
                      <div class="dot"></div>
                      <div>23岁</div>
                      <div class="dot"></div>
                      <div>C杯</div>
                    </div>
                  </div>
                  <div class="info_co">
                    <div class="info hobby">
                      <div class="icon">
                        <img
                          src="@/assets/images/card/ic_card_hobby.svg"
                          alt=""
                        />
                      </div>
                      <div>购物</div>
                      <div class="dot"></div>
                      <div>SPA</div>
                      <div class="dot"></div>
                      <div>旅游</div>
                    </div>
                  </div>
                  <p>职业：自由职业自由职业自由职业自由职业</p>
                  <p>期望收入：¥10000</p>
                  <div class="user_view">
                    <div class="num_total">
                      <ul>
                        <li>
                          <img
                            src="@/assets/images/card/ic_card_num_like.svg"
                            alt=""
                          />16
                        </li>
                        <li>
                          <img
                            src="@/assets/images/card/ic_card_num_chat.svg"
                            alt=""
                          />16
                        </li>
                        <li>
                          <img
                            src="@/assets/images/card/ic_card_num_watch.svg"
                            alt=""
                          />16
                        </li>
                      </ul>
                    </div>
                    <div class="num_time">1小时前更新</div>
                  </div>
                </div>
                <div class="photo">
                  <img src="@/assets/images/card/pic.png" alt="" />
                </div>
              </li>
              <li class="full_card no_premium_tag">
                <div class="location_tag">
                  <div>
                    <img
                      src="@/assets/images/card/ic_card_location.svg"
                      alt=""
                    />新疆
                  </div>
                </div>
                <div class="card_text">
                  <h1>99年健身教练99年健身教练99年健身教练</h1>
                  <div class="info_co">
                    <div class="info">
                      <div class="icon">
                        <img
                          src="@/assets/images/card/ic_card_body_info.svg"
                          alt=""
                        />
                      </div>
                      <div>162cm</div>
                      <div class="dot"></div>
                      <div>23岁</div>
                      <div class="dot"></div>
                      <div>C杯</div>
                    </div>
                  </div>
                  <div class="info_co">
                    <div class="info hobby">
                      <div class="icon">
                        <img
                          src="@/assets/images/card/ic_card_hobby.svg"
                          alt=""
                        />
                      </div>
                      <div>购物</div>
                      <div class="dot"></div>
                      <div>SPA</div>
                      <div class="dot"></div>
                      <div>旅游</div>
                    </div>
                  </div>
                  <p>职业：自由职业自由职业自由职业自由职业</p>
                  <p>期望收入：¥10000</p>
                  <div class="user_view">
                    <div class="num_total">
                      <ul>
                        <li>
                          <img
                            src="@/assets/images/card/ic_card_num_like.svg"
                            alt=""
                          />16
                        </li>
                        <li>
                          <img
                            src="@/assets/images/card/ic_card_num_chat.svg"
                            alt=""
                          />16
                        </li>
                        <li>
                          <img
                            src="@/assets/images/card/ic_card_num_watch.svg"
                            alt=""
                          />16
                        </li>
                      </ul>
                    </div>
                    <div class="num_time">1小时前更新</div>
                  </div>
                </div>
                <div class="photo">
                  <img src="@/assets/images/card/pic2.png" alt="" />
                </div>
              </li>
              <li class="full_card no_premium_tag">
                <div class="location_tag">
                  <div>
                    <img
                      src="@/assets/images/card/ic_card_location.svg"
                      alt=""
                    />新疆
                  </div>
                </div>
                <div class="card_text">
                  <h1>99年健身教练99年健身教练99年健身教练</h1>
                  <div class="info_co">
                    <div class="info">
                      <div class="icon">
                        <img
                          src="@/assets/images/card/ic_card_body_info.svg"
                          alt=""
                        />
                      </div>
                      <div>162cm</div>
                      <div class="dot"></div>
                      <div>23岁</div>
                      <div class="dot"></div>
                      <div>C杯</div>
                    </div>
                  </div>
                  <div class="info_co">
                    <div class="info hobby">
                      <div class="icon">
                        <img
                          src="@/assets/images/card/ic_card_hobby.svg"
                          alt=""
                        />
                      </div>
                      <div>购物</div>
                      <div class="dot"></div>
                      <div>SPA</div>
                      <div class="dot"></div>
                      <div>旅游</div>
                    </div>
                  </div>
                  <p>职业：自由职业自由职业自由职业自由职业</p>
                  <p>期望收入：¥10000</p>
                  <div class="user_view">
                    <div class="num_total">
                      <ul>
                        <li>
                          <img
                            src="@/assets/images/card/ic_card_num_like.svg"
                            alt=""
                          />16
                        </li>
                        <li>
                          <img
                            src="@/assets/images/card/ic_card_num_chat.svg"
                            alt=""
                          />16
                        </li>
                        <li>
                          <img
                            src="@/assets/images/card/ic_card_num_watch.svg"
                            alt=""
                          />16
                        </li>
                      </ul>
                    </div>
                    <div class="num_time">1小时前更新</div>
                  </div>
                </div>
                <div class="photo">
                  <img src="@/assets/images/card/pic3.jpeg" alt="" />
                </div>
              </li>
              <li class="full_card no_premium_tag">
                <div class="location_tag">
                  <div>
                    <img
                      src="@/assets/images/card/ic_card_location.svg"
                      alt=""
                    />新疆
                  </div>
                </div>
                <div class="card_text">
                  <h1>99年健身教练99年健身教练99年健身教练</h1>
                  <div class="info_co">
                    <div class="info">
                      <div class="icon">
                        <img
                          src="@/assets/images/card/ic_card_body_info.svg"
                          alt=""
                        />
                      </div>
                      <div>162cm</div>
                      <div class="dot"></div>
                      <div>23岁</div>
                      <div class="dot"></div>
                      <div>C杯</div>
                    </div>
                  </div>
                  <div class="info_co">
                    <div class="info hobby">
                      <div class="icon">
                        <img
                          src="@/assets/images/card/ic_card_hobby.svg"
                          alt=""
                        />
                      </div>
                      <div>购物</div>
                      <div class="dot"></div>
                      <div>SPA</div>
                      <div class="dot"></div>
                      <div>旅游</div>
                    </div>
                  </div>
                  <p>职业：自由职业自由职业自由职业自由职业</p>
                  <p>期望收入：¥10000</p>
                  <div class="user_view">
                    <div class="num_total">
                      <ul>
                        <li>
                          <img
                            src="@/assets/images/card/ic_card_num_like.svg"
                            alt=""
                          />16
                        </li>
                        <li>
                          <img
                            src="@/assets/images/card/ic_card_num_chat.svg"
                            alt=""
                          />16
                        </li>
                        <li>
                          <img
                            src="@/assets/images/card/ic_card_num_watch.svg"
                            alt=""
                          />16
                        </li>
                      </ul>
                    </div>
                    <div class="num_time">1小时前更新</div>
                  </div>
                </div>
                <div class="photo">
                  <img src="@/assets/images/card/pic6.png" alt="" />
                </div>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { NavigateRule,DialogControl } from "@/mixins";
import { EmptyView } from "@/components";
export default defineComponent({
  mixins: [NavigateRule, DialogControl],
  components:{EmptyView},
  data() {
    return {
      isEmpty: false,
    };
  },
});
</script>
