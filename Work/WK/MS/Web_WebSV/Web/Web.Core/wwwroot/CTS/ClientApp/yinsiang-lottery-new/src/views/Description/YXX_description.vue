<template>
  <!-- 玩法说明 start -->
  <div class="bg_yusiasieh overflow no-scrollbar">
    <div class="confirm_up height">
      <div class="confirm_infos">
        <div class="confirm_gameimg yusiasieh">
          <AssetImage src="@/assets/images/modal/logo_yusiasieh_h.png" />
        </div>
      </div>
    </div>
    <div class="rule_section">
      <div class="rule_maintitle" data-text="游戏简介">游戏简介</div>
      <div class="rule_text">
        鱼虾蟹又称鱼虾蟹骰宝,其形式与赔率跟骰宝基本一样,不过采用的骰子由图案代替点数。其中,鱼(红色)=1、虾(绿色)=2、葫芦(蓝色)=3、铜钱(蓝色)=4、蟹(绿色)=5、鸡(红色)=6。
      </div>
    </div>
    <!-- 玩法規則和賠率 -->
    <div class="confirm_middle">
      <div class="rule_maintitle" data-text="玩法规则和赔率">
        玩法规则和赔率
      </div>
      <!-- 大小 -->
      <div class="rule_subtitle">1. 大小</div>
      <div class="list_header_type">
        <div class="list_header_title">项目</div>
        <div class="list_header_title">赔率</div>
        <div class="list_header_title style2">说明</div>
      </div>
      <div class="type_list_inner fit_height">
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">大</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">1.96</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">三颗骰子的点数总和为11点~17点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">小</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">1.96</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">三颗骰子的点数总和为4点~10点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">单</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">1.96</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">三颗骰子的点数总和为单数</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">双</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">1.96</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">三颗骰子的点数总和为双数</div>
          </div>
        </div>
      </div>
    </div>
    <div class="confirm_middle">
      <!-- 总合 -->
      <div class="rule_subtitle">2. 总合</div>
      <div class="list_header_type">
        <div class="list_header_title">项目</div>
        <div class="list_header_title">赔率</div>
        <div class="list_header_title style2">说明</div>
      </div>
      <div class="type_list_inner fit_height">
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">4、17</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">63</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">三颗骰子的点数总和为4或17</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">5、16</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">31.5</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">三颗骰子的点数总和为5或16</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">6、15</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">19</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">三颗骰子的点数总和为6或15</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">7、14</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">12.6</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">三颗骰子的点数总和为7或14</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">8、13</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">9</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">三颗骰子的点数总和为8或13</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">9、12</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">7.5</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">三颗骰子的点数总和为9或12</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">10、11</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">7</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">三颗骰子的点数总和为10或11</div>
          </div>
        </div>
      </div>
    </div>
    <div class="confirm_middle">
      <!-- 单骰 -->
      <div class="rule_subtitle">3. 单骰</div>
      <div class="list_header_type">
        <div class="list_header_title">项目</div>
        <div class="list_header_title">赔率</div>
        <div class="list_header_title style2">说明</div>
      </div>
      <div class="type_list_inner fit_height">
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">鱼、虾、葫芦、铜钱、蟹、鸡</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">1.96</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">
              投注每颗骰子1至6中指定的点数，点数出现一次，赔率1.96
            </div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">鱼、虾、葫芦、铜钱、蟹、鸡</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">2.94</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">
              投注每颗骰子1至6中指定的点数，点数出现二次，赔率2.94
            </div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">鱼、虾、葫芦、铜钱、蟹、鸡</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">3.92</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">
              投注每颗骰子1至6中指定的点数，点数出现三次，赔率3.92
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="confirm_middle">
      <!-- 指定三色 -->
      <div class="rule_subtitle">4. 指定三色</div>
      <div class="list_header_type">
        <div class="list_header_title">项目</div>
        <div class="list_header_title">赔率</div>
        <div class="list_header_title style2">说明</div>
      </div>
      <div class="type_list_inner fit_height">
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">红、绿、蓝</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">1.96</div>
          </div>
          <div class="type_item style2 col">
            <div class="type_text gray">
              开出的骰子中，只能1颗骰子颜色与投注颜色相同。例：
            </div>
            <div class="type_dice">
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_fish.png" />
              </div>
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_shrimp.png" />
              </div>
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_crab.png" />
              </div>
              <div class="text">下注【红色】即中奖</div>
            </div>
            <div class="type_dice">
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_fish.png" />
              </div>
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_chicken.png" />
              </div>
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_crab.png" />
              </div>
              <div class="text">下注【红色】即算输</div>
            </div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">红对、绿对、蓝对</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">3.92</div>
          </div>
          <div class="type_item style2 col">
            <div class="type_text gray">
              开出的骰子中，只能2颗骰子颜色与投注颜色相同。例：
            </div>
            <div class="type_dice">
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_fish.png" />
              </div>
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_crab.png" />
              </div>
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_chicken.png" />
              </div>
              <div class="text">下注【红色】即中奖</div>
            </div>
            <div class="type_dice">
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_fish.png" />
              </div>
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_shrimp.png" />
              </div>
              <div class="yusiasieh_dice_sm">                
                <AssetImage src="@/assets/images/record/ic_dice_sm_crab.png" />
              </div>
              <div class="text">下注【红色】即算输</div>
            </div>
            <div class="type_dice">
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_chicken.png" />
              </div>
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_chicken.png" />
              </div>
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_chicken.png" />
              </div>
              <div class="text">下注【红色】即算输</div>
            </div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">红豹、绿豹、蓝豹</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">20</div>
          </div>
          <div class="type_item style2 col">
            <div class="type_text gray">
              开出的骰子中，所有骰子颜色与投注颜色相同。例：
            </div>
            <div class="type_dice">
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_fish.png" />
              </div>
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_chicken.png" />
              </div>
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_fish.png" />
              </div>
              <div class="text">下注【红色】即中奖</div>
            </div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">红绿蓝任意豹子</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">7</div>
          </div>
          <div class="type_item style2 col">
            <div class="type_text gray">
              开出的骰子中，所有骰子颜色与投注颜色相同。例：
            </div>
            <div class="type_dice">
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_fish.png" />
              </div>
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_chicken.png" />
              </div>
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_fish.png" />
              </div>
              <div class="text">即中奖</div>
            </div>
            <div class="type_dice">
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_coppercoins.png" />
              </div>
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_coppercoins.png" />
              </div>
              <div class="yusiasieh_dice_sm">
                <AssetImage src="@/assets/images/record/ic_dice_sm_gourd.png" />
              </div>
              <div class="text">即中奖</div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="confirm_middle">
      <!-- 围骰 -->
      <div class="rule_subtitle">5. 围骰</div>
      <div class="list_header_type">
        <div class="list_header_title">项目</div>
        <div class="list_header_title">赔率</div>
        <div class="list_header_title style2">说明</div>
      </div>
      <div class="type_list_inner fit_height">
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">三个鱼</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">169</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">三颗骰子都是鱼</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">三个虾</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">169</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">三颗骰子都是虾</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">三个葫芦</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">169</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">三颗骰子都是葫芦</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">三个铜钱</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">169</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">三颗骰子都是铜钱</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">三个蟹</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">169</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">三颗骰子都是蟹</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">三个鸡</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">169</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">三颗骰子都是鸡</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">全围</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">28.16</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">
              三颗骰子数字相同即为全围，若开出豹子111和666，通杀大、小、单、双、对子、总和、单骰
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <!-- 玩法说明 end -->
</template>
<script lang="ts">
import AssetImage from "@/components/shared/AssetImage.vue";
import { defineComponent } from "vue";

export default defineComponent({
  components: { AssetImage },
});
</script>
