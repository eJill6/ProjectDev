<template>
  <div class="bg_baccarat overflow no-scrollbar">
    <div class="confirm_up">
      <div class="confirm_infos">
        <div class="confirm_gameimg">
          <AssetImage :src="logoUrl" />
        </div>
      </div>
    </div>
    <div class="rule_section">
      <div class="rule_maintitle" data-text="游戏简介">游戏简介</div>
      <div class="rule_text">
        百家乐分为【庄家】和【闲家】，玩家可下注庄家或闲家，点数总和最接近9点者获胜。双方各收到至少2~3张牌，第3张牌为补牌。如果需要补牌，将依照【补牌规则】多发一张牌。任何一家拿到”例牌”(2张牌合计8或9点)时，牌局即结束，不再补牌。
      </div>
    </div>
    <!-- 玩法規則和賠率 -->
    <div class="confirm_middle">
      <div class="rule_maintitle" data-text="玩法规则和赔率">玩法规则和赔率</div>
      <div class="list_header_type">
        <div class="list_header_title">项目</div>
        <div class="list_header_title">赔率</div>
        <div class="list_header_title style2">说明</div>
      </div>
      <div class="type_list_inner fit_height">
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">庄</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">1.95</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">牌局结束后，点数大于闲家</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">闲</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">2</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">牌局结束后，点数大于庄家</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">和</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">9</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">
              牌局结束后，庄闲点数相同，开和局时退回庄闲的注金
            </div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">庄幸运6</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">13/21</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">
              当庄家以2张牌6点(不需补牌)胜出赔13倍，庄家以3张牌6点(需补牌)
              胜出赔21倍
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- 对子規則 -->
    <div class="confirm_middle pb">
      <div class="rule_maintitle" data-text="对子">对子</div>
      <div class="list_header_type">
        <div class="list_header_title">项目</div>
        <div class="list_header_title">赔率</div>
        <div class="list_header_title style2">说明</div>
      </div>
      <div class="type_list_inner fit_height">
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">庄对</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">12</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">
              庄的第一和第二张牌面值一样，不计花色
            </div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">闲对</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">12</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">
              闲的第一和第二张牌面值一样，不计花色
            </div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">任意对子</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">6</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">
              闲或庄的前两张牌的牌面(数字或英文字母不计花色)为依据，无论结果是庄对或闲对，即押注”任意对子”者为胜。当庄和闲都为任意对子时，仅派彩一次
            </div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">完美对子</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">26</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">
              闲或庄的前两张牌的牌面必须是同花同色同数字或同英文字为依据，游戏中，庄或闲家前两张牌符合要求，即押注”完美对子”者为胜。当庄和闲都为完美对子时，仅派彩一次
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="rule_section">
      <div class="rule_maintitle" data-text="点数计算方式">点数计算方式</div>
      <div class="rule_text">
        10、J、Q、K的牌面视作0点，其他按牌面点数计算。当所有牌的点数总和超过9时，仅算总数的个位数。例，最小点数为：0点(4+6=10)；最大点数为：9点(4+5=9)取个位数
      </div>
    </div>
    <div class="rule_section">
      <div class="rule_maintitle" data-text="例牌">例牌</div>
      <div class="rule_text">
        庄闲任何一方两牌合计为「8或9点」(称为「例牌」)，双方都不需补牌，即定胜负(双方同持8点或9点为和局)。
      </div>
    </div>
    <div class="rule_section">
      <div class="rule_maintitle" data-text="补牌规则">补牌规则</div>
      <div class="rule_text">
        若闲家不需补牌(即闲家首两张牌合计为6~9点)，庄家以”闲家补牌规则”补牌，即庄家首两张牌合计”0~5”点须补一张牌，6点以上不需补牌
      </div>
    </div>
    <!-- 閒兩牌合計點數 -->
    <div class="confirm_middle pb">
      <div class="list_header_type">
        <div class="list_header_title">闲两牌合计点数</div>
        <div class="list_header_title style3">(闲家)</div>
      </div>
      <div class="type_list_inner fit_height">
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">0</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">补一张牌</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">1</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">补一张牌</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">2</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">补一张牌</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">3</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">补一张牌</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">4</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">补一张牌</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">5</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">补一张牌</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">6</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">不得补牌</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">7</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">不得补牌</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">8</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">例牌，即定输赢</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">9</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">例牌，即定输赢</div>
          </div>
        </div>
      </div>
    </div>
    <!-- 莊兩牌合計點數 -->
    <div class="confirm_middle pb">
      <div class="list_header_type">
        <div class="list_header_title">庄两牌合计点数</div>
        <div class="list_header_title style3">(庄家)</div>
      </div>
      <div class="type_list_inner fit_height">
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">0</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">补一张牌</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">1</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">补一张牌</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">2</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">补一张牌</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">3</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">若闲家补得第三张牌是8，不得补牌</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">4</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">
              若闲家补得第三张牌是0,1,8,9，不得补牌
            </div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">5</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">
              若闲家补得第三张牌是0,1,2,3,8,9，不得补牌
            </div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">6</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">若闲家补得第三张牌是6,7，必须补牌</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">7</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">不得补牌</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">8</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">例牌，即定输赢</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">9</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">例牌，即定输赢</div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import AssetImage from "@/components/shared/AssetImage.vue";
import { defineComponent } from "vue";

export default defineComponent({
  components: { AssetImage },
  computed: {
    logoUrl() {
      const typeName = this.$store.state.lotteryInfo.gameTypeName as string;
      return !!typeName
        ? `@/assets/images/modal/logo_${typeName.toLocaleLowerCase()}_h.png`
        : "";
    },
  },
});
</script>
