interface IHtmlSearchContent {
    $contentBody: any;
}

class htmlSearchContent implements IHtmlSearchContent {
    $contentBody: any;
}

class intervalSetting {
    id: number;
    seconds: number;
}

interface ISearchApiUrlSetting {
    searchApiUrl: string;
    exportApiUrl: string;
}

interface IBaseSearchService {
    search();
    refreshSearchResult(intervalSeconds: number);
}

abstract class baseSearchService implements IBaseSearchService {
    private lastSubmitData: object;
    private intervalSetting: intervalSetting;
    protected searchApiUrlSetting: ISearchApiUrlSetting;
    protected htmlSearchContent: IHtmlSearchContent;
    protected submitData: object;
    protected validatePairDateFieldSettings: [JQuery<HTMLElement>, JQuery<HTMLElement>, boolean?, Date?][] = [];

    constructor(searchApiUrlSetting: ISearchApiUrlSetting,
        htmlSearchContent: IHtmlSearchContent = { $contentBody: $('#jqContent') }) {
        this.searchApiUrlSetting = searchApiUrlSetting;
        this.htmlSearchContent = htmlSearchContent;
        this.intervalSetting = new intervalSetting();
    }

    search() {
        this.submitData = this.getSubmitData();
        this.doSearch(this.submitData);
    }

    refreshSearchResult(intervalSeconds: number) {
        this.clearIntervalId();

        if (typeof intervalSeconds !== "number" || intervalSeconds === 0) {
            return;
        }

        let self = this;

        this.intervalSetting.seconds = intervalSeconds;

        this.intervalSetting.id = setInterval(
            function () {
                self.doRefreshSearchResult();
            },
            this.intervalSetting.seconds * 1000);
    }

    protected doRefreshSearchResult() {
        this.search();
    }

    protected clearIntervalId() {
        if (this.intervalSetting.id === undefined) {
            return;
        }

        clearInterval(this.intervalSetting.id);
        this.intervalSetting.id = undefined;
    }

    protected abstract getSubmitData(): object;

    protected doSearch(submitData: object): void {
        if (!this.validateSubmitData()) {
            this.submitData = this.lastSubmitData; //revert submitData

            return;
        }

        const htmlContents = this.getHtmlContents();
        const updateGridContent = this.updateGridContent;
        const doAfterSearch = this.doAfterSearch;

        const self = this;

        $.ajax2({
            url: this.searchApiUrlSetting.searchApiUrl,
            type: "POST",
            data: submitData,
            success: response => {
                self.lastSubmitData = self.submitData;
                updateGridContent(response, htmlContents, self);
                doAfterSearch(htmlContents, self);
                self.refreshSearchResult(self.intervalSetting.seconds);
            }
        });
    }

    protected getHtmlContents(): IHtmlSearchContent {
        return this.htmlSearchContent;
    }

    protected updateGridContent(response, htmlContents: IHtmlSearchContent, self: any) {
        htmlContents.$contentBody.html(response);
    }

    protected abstract doAfterSearch(htmlContents: any, self: any): void;

    protected validateSubmitData(): boolean {
        for (const setting of this.validatePairDateFieldSettings) {
            if (!this.validatePairDateField(setting)) {
                return false;
            }
        }

        return true;
    }

    private validatePairDateField(validatePairDateFieldSetting: [JQuery<HTMLElement>, JQuery<HTMLElement>, boolean?, Date?]): boolean {
        const startDateString: string = validatePairDateFieldSetting[0].val().toString();
        const endDateString: string = validatePairDateFieldSetting[1].val().toString();
        const isAllowEmpty: boolean = validatePairDateFieldSetting[2];
        const minDate: Date = validatePairDateFieldSetting[3];

        if (!this.validateSingleDateField(startDateString, isAllowEmpty, minDate)
            || !this.validateSingleDateField(endDateString, isAllowEmpty, minDate)
        ) {
            return false;
        }

        const startDateValue: Date = new Date(startDateString);
        const endDateValue: Date = new Date(endDateString);

        if (startDateValue > endDateValue) {
            alert('日期格式不符');

            return false;
        }

        return true;
    }

    private validateSingleDateField(dateString: string, isAllowEmpty: boolean, minDate: Date): boolean {
        if ((dateString && !$.isValidDateStringFormat(dateString))
            || (!isAllowEmpty && !dateString)
        ) {
            alert('日期格式不符');

            return false;
        }

        const minDateValue: number = minDate ? minDate.setHours(0, 0, 0, 0) : new Date(0).setHours(0, 0, 0, 0);
        const maxDateValue: number = new Date().setHours(0, 0, 0, 0);
        const dateValue: number = new Date(dateString).setHours(0, 0, 0, 0);

        if (dateValue < minDateValue || dateValue > maxDateValue) {
            alert('输入的日期已超过可搜查的时间范围');

            return false;
        }

        return true;
    }

    protected initStartAndEndDatePicker($startDateSelector: JQuery<HTMLElement>, $endDateSelector: JQuery<HTMLElement>, isAllowEmpty?: boolean, minStartDate?: Date): void {
        const isReadonly = false;
        $.setStartAndEndDatePicker($startDateSelector, $endDateSelector, isReadonly, minStartDate);

        //把初始化的value當做預設值,讓清空的動作觸發時可以還原當時的value
        $startDateSelector.attr("defaultValue", $startDateSelector.val().toString());
        $endDateSelector.attr("defaultValue", $endDateSelector.val().toString());

        this.setValidatePairDateFieldSettings($startDateSelector, $endDateSelector, isAllowEmpty, minStartDate);
    }

    protected setValidatePairDateFieldSettings($startDateSelector: JQuery<HTMLElement>, $endDateSelector: JQuery<HTMLElement>, isAllowEmpty?: boolean, minStartDate?: Date) {
        this.validatePairDateFieldSettings.push([$startDateSelector, $endDateSelector, isAllowEmpty, minStartDate]);
    }

    protected initDefaultDatePicker(isAllowEmpty: boolean) {
        let self = this;

        $.each($(".jqStartDate"), function (index: number, value: HTMLElement) {
            let $startDate = $(value);
            let endDateId = $(value).attr("endDateId");
            let $endDate = $(`#${endDateId}`);
            self.initStartAndEndDatePicker($startDate, $endDate, isAllowEmpty);
        });
    }
}