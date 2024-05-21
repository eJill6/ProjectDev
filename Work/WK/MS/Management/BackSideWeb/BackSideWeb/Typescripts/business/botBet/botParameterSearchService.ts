class botParameterSearchService extends baseCRUDService {
    protected $tabSelectorItems: JQuery<HTMLElement> = $('.pageTab ul a');
    private isFirstClick: boolean = true;
    private lotteryPatchTypeSelect = document.getElementById("lotteryPatchType") as HTMLDivElement;
    private botGroupSelect = document.getElementById("botGroupSelect") as HTMLSelectElement;
    private timeTypeSelect = document.getElementById("timeTypeSelect") as HTMLSelectElement;
    protected getInsertViewArea(): layerArea {
        return {
            width: 600,
            height: 700,
        } as layerArea;
    }
    protected getUpdateViewArea(): layerArea {
        return {
            width: 600,
            height: 700,
        } as layerArea;
    }

    constructor(pageApiUrlSetting: IPageApiUrlSetting) {
        super(pageApiUrlSetting);
        $(document).ready(() => {
            this.registerClickEvent();
            this.$tabSelectorItems.first().click();
            this.botGroupSelect.onchange = () => {
                this.updateTimeTypeOptions();
            };
        });
    }
    protected registerClickEvent(): void {
        this.$tabSelectorItems.click((event) => {
            event.preventDefault();

            this.$tabSelectorItems.removeClass("active");
            $(event.currentTarget).addClass("active");

            const tabId = $(event.currentTarget).attr("id");
            $("#lotteryPatchType").text(tabId);
            this.updateTimeTypeOptions();
            //First enter no one more trigger click()
            if (!this.isFirstClick) {
                const $jqSearchBtn = $(".jqSearchBtn");
                $jqSearchBtn.click();
            } else {
                this.isFirstClick = false;
            }
        });
    }

    protected getSubmitData(): ISearchGridParam {
        const data = new botParameterSearchServiceParam();
        data.lotteryPatchType = $('#lotteryPatchType').text();
        data.botGroup = this.botGroupSelect.value;
        data.timeType = this.timeTypeSelect.value;
        data.settingGroup = $('#settingGroupSelect').data().value;
        return data;
    }

    protected updateTimeTypeOptions(): void {
        var lotteryPatchTypeSelectValue = this.lotteryPatchTypeSelect.innerText;
        var botGroupSelectValue = this.botGroupSelect.value;
        // 清空舊的選項
        this.timeTypeSelect.innerHTML = '';
        if (botGroupSelectValue === '' || botGroupSelectValue === null || botGroupSelectValue === "全部")
        {
            this.addOption(this.timeTypeSelect, "全部", '');
            this.addOption(this.timeTypeSelect, 'T1', '0');
            this.addOption(this.timeTypeSelect, 'T2', '1');
            this.addOption(this.timeTypeSelect, 'T3', '2');
            this.addOption(this.timeTypeSelect, 'T4', '3');
            return;
        }
        // 基於不同的條件塞入新的選項
        if (lotteryPatchTypeSelectValue === '1') {
            if (botGroupSelectValue === '2') {
                this.addOption(this.timeTypeSelect, "全部", '');
                this.addOption(this.timeTypeSelect, 'T1', '0');
                this.addOption(this.timeTypeSelect, 'T2', '1');
            }
            else {
                this.addOption(this.timeTypeSelect, "全部", '');
                this.addOption(this.timeTypeSelect, 'T1', '0');
                this.addOption(this.timeTypeSelect, 'T2', '1');
                this.addOption(this.timeTypeSelect, 'T3', '2');
                this.addOption(this.timeTypeSelect, 'T4', '3');
            }
        } else if (lotteryPatchTypeSelectValue === '0') {
            if (botGroupSelectValue === '0') {
                this.addOption(this.timeTypeSelect, "全部", '');
                this.addOption(this.timeTypeSelect, 'T1', '0');
                this.addOption(this.timeTypeSelect, 'T2', '1');
                this.addOption(this.timeTypeSelect, 'T3', '2');
                this.addOption(this.timeTypeSelect, 'T4', '3');
            } else if (botGroupSelectValue === '1') {
                this.addOption(this.timeTypeSelect, "全部", '');
                this.addOption(this.timeTypeSelect, 'T1', '0');
                this.addOption(this.timeTypeSelect, 'T2', '1');
                this.addOption(this.timeTypeSelect, 'T3', '2');
            } else if (botGroupSelectValue === '2') {
                this.addOption(this.timeTypeSelect, "全部", '');
                this.addOption(this.timeTypeSelect, 'T1', '0');
                this.addOption(this.timeTypeSelect, 'T2', '1');
            }
        }
    }
    private addOption(selectElement: HTMLSelectElement, text: string, value: string) {
        var option = document.createElement('option');
        option.text = text;
        option.value = value;
        selectElement.add(option);
    }
}

class botParameterSearchServiceParam extends PagingRequestParam implements ISearchGridParam {
    lotteryPatchType: any;
    botGroup: any;
    timeType: any;
    settingGroup: any;
}