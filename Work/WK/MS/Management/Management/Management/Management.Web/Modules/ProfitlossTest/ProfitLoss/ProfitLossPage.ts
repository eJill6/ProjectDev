import { initFullHeightGridPage } from '@serenity-is/corelib/q';
import { ProfitLossGrid } from './ProfitLossGrid';

$(function() {
    initFullHeightGridPage(new ProfitLossGrid($('#GridDiv')).element);
});