import { initFullHeightGridPage } from '@serenity-is/corelib/q';
import { FrontsideMenuGrid } from './FrontsideMenuGrid';

$(function() {
    initFullHeightGridPage(new FrontsideMenuGrid($('#GridDiv')).element);
});