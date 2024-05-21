<template>
  <div class="main_container bg_subpage">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1 solid_color">
        <div class="header_back" @click="navigateToPrevious">
          <div>
            <CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>
        <div class="header_title">防骗指南</div>
      </header>
      <!-- Header end -->

      <!-- 共同滑動區塊 -->
      <div class="flex_height">
        <div class="overflow no_scrollbar">
          <div class="padding_basic">
            <!-- 你的切版放這裡 -->
            <div class="pure_text_article">
              <p>
                为了让广大用户可以顺利地使用平台，避免上当受骗。大家做到以下几点，基本就能避免上当受骗，希望对大家有所帮助。
              </p>
              <p>
                一、如果妹子是自己租公寓或者小区里的房子，打听一下房子的租金就可以知道妹子做这生意的成本大概是多少，依据这个可以判断是不是坑。不熟悉的妹子千万不要选择玩上门，因为没有自己承租地方来做这个生意的，100％都是骗子。
              </p>
              <p>
                二、未见面，拒绝提前支付任何费用，如车费、定金、安全保证金、预付一半费用、付全款后才见面等各种理由的费用，切勿因为已开房或其他费用，因小失大，不停给对方打款，越陷越深。
              </p>
              <p>
                三、见面后，确认为所预约的人，付费给妹子，或当着妹子的面付给中间人，付费后禁止对方以任何理由离开房间，如有必要离开，请随行，或要求对方退款后，方可离开。
              </p>
              <p>
                四、见面后，人照不符的，不满意的，可以拒绝接受服务，或给车费让她走。
              </p>
              <p>
                五、到店服务的，和描述不一致，直接离开。进店后，问清楚服务项目，确定有服务，选到满意技师，再交费或服务后再交费。模糊介绍服务，未选人先交费，要求办卡才有服务的，立即离开，并联系平台反馈。
              </p>
              <p>六、拒绝支付任何以平台旗号的费用。</p>
              <p>
                七、在平台找的妹子，请以帖子标明的价格为准，超出范围，请自行决定，如有损失，自行负责。
              </p>
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
import { NavigateRule } from "@/mixins";

export default defineComponent({
  components: { CdnImage },
  mixins: [NavigateRule],
});
</script>
