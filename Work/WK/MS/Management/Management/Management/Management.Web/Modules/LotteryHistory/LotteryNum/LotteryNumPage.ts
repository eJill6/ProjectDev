import { initFullHeightGridPage } from '@serenity-is/corelib/q';
import { LotteryNumGrid } from './LotteryNumGrid';

$(function() {
    initFullHeightGridPage(new LotteryNumGrid($('#GridDiv')).element);
});