<template>

<div class="main_container">
            <div class="main_container_flex">
                <!-- Header start -->
                <header class="header_height1">
                    <div class="header_back" @click="navigateToPrevious">
                        <div> <CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt="" /></div>
                    </div>
                    <div class="header_title">觅钱包</div>
                    <div class="header_btn align_left">
                        <!-- <div>
                            <div class="style_c disable">取消</div>
                        </div> -->
                    </div>
                    <!-- <div class="header_btn align_right">
                        <div>
                            <div class="style_a">发帖规则</div>
                        </div>
                    </div>
                    <div class="header_btn align_right">
                        <div>
                            <div class="style_b disable">完成</div>
                        </div>
                    </div> -->
                </header>
                
                <!-- Header end -->

                <!-- 共同滑動區塊 -->
                <div class=" flex_height bg_second">
                    <div class="overflow no_scrollbar">
                        <div class=" padding_basic">
                            <!-- 你的切版放這裡 -->
                            <div class="wallet_card">
                                <div class="wallet_box">
                                    <div class="wallet_content">
                                        <div class="wallet_title">钻石钱包余额</div>
                                        <div class="wallet_price">{{ walletInfo.point }}</div>
                                    </div>
                                    <div class="wallet_content">
                                        <div class="btn_small" @click="goExchangeUrl">兑换</div>
                                    </div>
                                </div>
                            </div>
                            <div class="wallet_card main_wallet">
                                <div class="wallet_box">
                                    <div class="wallet_content">
                                        <div class="wallet_title">主账户余额（RMB）</div>
                                        <div class="wallet_price">{{ walletInfo.amount }}</div>
                                    </div>
                                    <div class="wallet_content">
                                      <div class="btn_small" @click="goDepositUrl">充值</div>
                                      <div class="btn_small" @click="goWithdrawUrl">提现</div>
                                    </div>
                                </div>
                                <div class="wallet_box">
                                    <div class="wallet_content wallet_content_flex">
                                        <div class="wallet_title">暂锁收益</div>
                                        <div class="wallet_price wallet_text_small">  {{ walletInfo.freezeIncome }}</div>
                                    </div>
                                    <div class="wallet_content wallet_content_flex">
                                        <div class="wallet_title">本月收益</div>
                                        <div class="wallet_price wallet_text_small">  {{ walletInfo.income }}</div>
                                    </div>
                                </div>
                            </div>
                            <div class="wallet_list_section">
                            <div class="wallet_list_item" @click="navigateToProfitDetail">
                              <div class="wallet_list_box">
                                <div>
                                  <CdnImage src="@/assets/images/wallet/ic_wallet_shouyi.svg" />
                                </div>
                                <div class="wallet_list_title">收益明细</div>
                              </div>
                              <div class="wallet_arrow">
                                <CdnImage
                                  src="@/assets/images/wallet/ic_wallet_list_arrow.svg"
                                />
                              </div>
                            </div>
                            <div class="wallet_list_item" @click="navigateToPaymentHistory">
                              <div class="wallet_list_box">
                                <div>
                                  <CdnImage src="@/assets/images/wallet/ic_wallet_xiaofei.svg" />
                                </div>
                                <div class="wallet_list_title">消费记录</div>
                              </div>
                              <div class="wallet_arrow">
                                <CdnImage
                                  src="@/assets/images/wallet/ic_wallet_list_arrow.svg"
                                />
                              </div>
                            </div>
                            <div class="wallet_list_item" @click="goExchangerecordUrl">
                              <div class="wallet_list_box">
                                <div>
                                  <CdnImage src="@/assets/images/wallet/ic_wallet_duihuan.svg" />
                                </div>
                                <div class="wallet_list_title">兑换记录</div>
                              </div>
                              <div class="wallet_arrow">
                                <CdnImage
                                  src="@/assets/images/wallet/ic_wallet_list_arrow.svg"
                                />
                              </div>
                            </div>
                            <div class="wallet_list_item" @click="goDwReportUrl">
                              <div class="wallet_list_box">
                                <div>
                                  <CdnImage src="@/assets/images/wallet/ic_wallet_chongti.svg" />
                                </div>
                                <div class="wallet_list_title">充提记录</div>
                              </div>
                              <div class="wallet_arrow">
                                <CdnImage
                                  src="@/assets/images/wallet/ic_wallet_list_arrow.svg"
                                />
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
import { CdnImage } from "@/components";
import { defineComponent } from "vue";
import { NavigateRule, PlayGame } from "@/mixins";
import api from "@/api";
import { WalletModel } from "@/models";

export default defineComponent({
  components: { CdnImage },
  mixins: [NavigateRule, PlayGame],
  data() {
    return {
      walletInfo: {} as WalletModel,
    };
  },
  methods: {
    async getWallet() {
      this.walletInfo = await api.getWalletInfo();
    },
  },
  async created() {
    await this.getWallet();
  },
});
</script>
