interface AjaxHtmlUpdateSetting {
    url: string,
    type: string,
    data?: any,
    mode?: number
}

interface IAjaxSetting extends JQuery.AjaxSettings {
    url: string;
    type: string;
    data?: any;
    dataType?: string,
    contentType?: any,
    async?: boolean,
    success?: (response: any, status?: string, jqXHR?: any) => void,
    error?: (response) => void,
    failedFunc?: (response) => void,
    complete?: (request, status) => void,
    processData?: boolean,
    isAutoHideLoading?: boolean, // 正常流程undefined或true的話，會在ajaxComplete時自動關閉Loading。若傳false則需自己處理關閉Loading
}

interface JQueryStatic {
    ajax2(setting: IAjaxSetting);
    logOff();
    localStorage: {
        get: (key: string) => any;
        set: (key: string, value: any) => void;
    },
    timer: {
        set(seconds: number, callback: (seconds: number) => void, timeout?: () => void): number,
        clear(timer: number),
        allClear(),
    },
    dropdownMenuSelectedEvent: (key: string, value: string, callback: (value: string) => void) => void,
    alert: (message: string, yesBtn: string, yesBtnFnc?: () => void, noBtn?: string, noBtnFunc?: () => void) => void,
    tip: (message: string, time?: number) => void,
    toast(message: string),
    copyToClipboard: (text: string, callback?: () => void) => void,
    dateStringFormat: (date: Date) => string,
    isValidDateStringFormat: (dateString: string) => boolean,
    setStartAndEndDatePicker: ($startDateSelector: JQuery<HTMLElement>, $endDateSelector: JQuery<HTMLElement>, isReadonly?: boolean, minStartDate?: Date) => void,
    datePickerOnOpen: ($datePickerSelector: JQuery<HTMLElement>, minDate: Date, maxDate: Date) => void,
}

interface JQuery<TElement = HTMLElement> {
    tmpl(input: any): any,
    reverse: any,
    loading: (options?) => JQuery<TElement>,
    onlyNumber: () => JQuery<TElement>,
    setEyeToPasswordInput: () => JQuery<TElement>,
    tmplBinding: (model: any, target: JQuery<HTMLElement>, clear?: boolean) => void;

    // DatePicker
    setDatePicker: (setting?: DatePickerSetting) => void,
    updateDatePicker: (setting?: DatePickerSetting) => void,
    Zebra_DatePicker: (setting: any) => any,
    update: (setting?: DatePickerSetting) => void,

    tooltipster: (setting?: any) => void,
    hoverTips: (setting?: TipsSetting) => void,
    hoverData: () => void,
    toggleWithSlide: (isShow?: boolean, duration?: JQuery.Duration, complete?: (this: HTMLElement) => void) => JQuery<HTMLElement>,
}

interface DatePickerSetting {
    months?: string[],
    days?: string[],
    start_date?: string,
    enabled_dates?: string[],
    disabled_dates?: string[],
    first_day_of_week?: number,
    direction?: any[] | string | number | boolean,
    offset?: number[],
    onOpen?: () => void,
    onChange?: (view: string, elements) => void,
    onSelect?: (date: string) => void,
    onClose?: () => void,
    readonly_element?: boolean,
}

interface TipsSetting {
    maxWidth?: number,
    position?: string,
    content?: string | JQuery<HTMLElement>,
}