import { initFullHeightGridPage } from '@serenity-is/corelib/q';
import { LotteryInfoGrid } from './LotteryInfoGrid';

$(function() {
    initFullHeightGridPage(new LotteryInfoGrid($('#GridDiv')).element);
});