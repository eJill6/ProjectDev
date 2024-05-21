import { initFullHeightGridPage } from '@serenity-is/corelib/q';
import { PalyInfoGrid } from './PalyInfoGrid';

$(function() {
    initFullHeightGridPage(new PalyInfoGrid($('#GridDiv')).element);
});