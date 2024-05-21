<template>
  <div class="gamerule_inner">
    <div class="gamerule_name">极速三公玩法说明</div>
    <div class="gamerule_text">
      三公游戏使用一副扑克牌中的52张牌
      (除大小王外)、发牌顺序为连续发放三张牌给闲、庄两家比牌，并根据牌型判断输赢，会员可任意进行投注。
    </div>
  </div>
  <div class="gamerule_inner">
    <div class="gamerule_name">玩法规则和赔率</div>
    <div class="gamerule_list_inner">
      <div class="gamerule_list_name">1.闲庄</div>
      <div class="list_header_type">
        <div class="list_header_title">项目</div>
        <div class="list_header_title">赔率</div>
        <div class="list_header_title style2">说明</div>
      </div>
      <div class="type_list_inner fit_height">
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">闲</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">1.96</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">
              闲家点数大于庄家，若闲家以6点的任意牌型组合胜出，赔率为1.47
            </div>
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
              闲庄两家点数与公牌数都相同时，即为和局，不比较花色；和局将退回投注闲、庄赢的玩家本金
            </div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">庄</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">1.96</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">
              闲家点数大于庄家，若庄家以6点的任意牌型组合胜出，赔率为1.47
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="gamerule_list_inner">
      <div class="gamerule_list_name">2.三公</div>
      <div class="list_header_type">
        <div class="list_header_title">项目</div>
        <div class="list_header_title">赔率</div>
        <div class="list_header_title style2">说明</div>
      </div>
      <div class="type_list_inner fit_height">
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">庄三公</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">70</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">闲开出三公牌型即算赢</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">闲三公</div>
          </div>
          <div class="type_item">
            <div class="type_text gray">70</div>
          </div>
          <div class="type_item style2">
            <div class="type_text gray">庄开出三公牌型即算赢</div>
          </div>
        </div>
      </div>
    </div>
    <div class="gamerule_list_inner">
      <div class="gamerule_list_name">3.牌面数值</div>
      <ul>
        <li>10、J、Q、K都为10，但10不为公，其他按牌面点数计算。</li>
        <li>A牌的牌值为1点</li>
      </ul>
    </div>
    <div class="gamerule_list_inner">
      <div class="gamerule_list_name">4.牌面大小</div>
      <ul>
        <li>
          三公 &gt; 双公9 &gt; 单公9 &gt; 9点 &gt; 双公8 &gt; 单公8 &gt; 8点
          &gt; 双公7 &gt; 单公7 &gt; 7点 &gt; 双公6 &gt; 单公6 &gt; 6点 &gt;
          双公5 &gt; 单公5 &gt; 5点 &gt; 双公4 &gt; 单公4 &gt; 4点 &gt; 双公3
          &gt; 单公3 &gt; 3点 &gt; 双公2 &gt; 单公2 &gt; 2点 &gt; 双公1 &gt;
          单公1 &gt; 1点 &gt; 双公0 &gt; 单公0 &gt; 0点
        </li>
        <li>特殊牌型：三公，无大小之分，例：K Q Q、Q J J</li>
        <li>
          其他牌型：比较三张牌总点数，总点数大为赢，点数一致则公牌多为赢，双公
          &gt; 单公 &gt; 点数牌，例：J Q 9 &gt; J 10 9 &gt; 10 10 9
        </li>
        <li>
          闲、庄公牌数相同时则比较三张牌内最大的单张数字大小，如双方都是三公，则为打和
        </li>
        <li>
          单张数字大小：K &gt; Q &gt; J &gt; 10 &gt; 9 &gt; 8 &gt; 7 &gt; 6 &gt;
          5 &gt; 4 &gt; 3 &gt; 2 &gt; A
        </li>
      </ul>
    </div>
    <div class="gamerule_list_inner">
      <div class="gamerule_list_name">5.牌面点数</div>
      <ul>
        <li>
          公牌及10点以「0」点计，其它牌则以牌面点数来计算，总和超过10或20则取其个位数字来看。
        </li>
        <li>
          牌型组成为「公牌+点数」，开奖点数仅显示「点数」以及赢的一方显示「闲赢
          / 庄赢」例如闲拿到1张公牌与点数和为8且赢，则显示「8点 闲赢」。
        </li>
        <li>
          最大牌型为「三公」，其次为「双公9」；牌型中若无公牌，则最大牌型为9点，扑克牌的花色无任何影响。
        </li>
        <li>
          双方点数与公牌数相同时则为和，出现和牌时，退回下注「闲」或「庄」的投注额。
        </li>
      </ul>
    </div>
    <div class="gamerule_name">基本的牌型大小</div>
    <div class="gamerule_list_inner">
      <div class="list_header_type">
        <div class="list_header_title">项目</div>
        <div class="list_header_title style3">说明</div>
      </div>
      <div class="type_list_inner fit_height">
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">三公</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">任意三张由J、Q、K组成的牌</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">双公9</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">其中2牌为公，另一张牌为9点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">单公9</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">其中1牌为公，合计点数为9点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">9点</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">合计点数为9点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">双公8</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">其中2牌为公，另一张牌为8点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">单公8</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">其中1牌为公，合计点数为8点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">8点</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">合计点数为8点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">双公7</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">其中2牌为公，另一张牌为7点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">单公7</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">其中1牌为公，合计点数为7点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">7点</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">合计点数为7点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">双公6</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">其中2牌为公，另一张牌为6点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">单公6</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">其中1牌为公，合计点数为6点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">6点</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">合计点数为6点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">双公5</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">其中2牌为公，另一张牌为5点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">单公5</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">其中1牌为公，合计点数为5点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">5点</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">合计点数为5点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">双公4</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">其中2牌为公，另一张牌为4点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">单公4</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">其中1牌为公，合计点数为4点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">4点</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">合计点数为4点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">双公3</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">其中2牌为公，另一张牌为3点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">单公3</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">其中1牌为公，合计点数为3点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">3点</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">合计点数为3点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">双公2</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">其中2牌为公，另一张牌为2点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">单公2</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">其中1牌为公，合计点数为2点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">2点</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">合计点数为2点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">双公1</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">其中2牌为公，另一张牌为1点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">单公1</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">其中1牌为公，合计点数为1点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">1点</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">合计点数为1点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">双公0</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">其中2牌为公，另一张牌为10点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">单公0</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">其中1牌为公，合计点数为10或20点</div>
          </div>
        </div>
        <div class="type_row fit_height">
          <div class="type_item">
            <div class="type_text gray">0点</div>
          </div>
          <div class="type_item style3">
            <div class="type_text gray">合计点数为10或20点</div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
