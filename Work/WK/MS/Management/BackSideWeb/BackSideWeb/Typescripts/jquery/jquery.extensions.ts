enum LayerIconType {
    // !
    Alert = 0,

    // v
    Ok = 1,

    // x
    Error = 2,

    // ?
    Tip = 3,

    // 🔒︎
    Lock = 4,

    // :(
    Disagree = 5,

    // :)
    Agree = 6
}

class Timer {
    private timers: number[] = [];

    set(seconds: number, callback: (seconds: number) => void, timeout?: () => void): number {
        callback(seconds);
        const countDownSec = seconds * 1000;
        const end = Date.now() + countDownSec;
        const index = setInterval(() => {
            const now = Date.now();
            const seconds = Math.ceil((end - now) / 1000);
            if (seconds > -1) {
                callback(seconds);
            } else {
                this.clear(index);
                if (typeof timeout === "function") {
                    timeout();
                }
            }
        }, 1000);

        this.timers.push(index);

        return index;
    }

    clear(timer: number) {
        const index = this.timers.findIndex(t => t === timer);
        if (index > -1) {
            clearInterval(timer);
            this.timers.splice(index, 1);
        }
    }

    allClear() {
        this.timers.forEach(index => clearInterval(index));
        this.timers = [];
    }
}

enum AjaxUpdateMode {
    Replace = 1,
    InsertBefore = 2,
    InsertAfter = 3
}

($ => {
    $.fn.reverse = [].reverse;
    const defaultSettings = {
        image: '~/images/validator/loading.gif',
        alternateText: "Loading",
        autoShow: true
    };

    $.fn.loading = function (options) {
        /// <summary>Method or options.</summary>
        const methods = {
            init: function (options) {
                const settings = jQuery.extend({}, defaultSettings, options);

                if (settings.autoShow) {
                    methods.show.apply(this);
                }
            },
            show() {
                //amd2
                let $loading = $("#loading");
                if ($loading.length == 1) {
                    $loading.show();

                    return;
                }

                //amd1
                let positionStyle = "fixed";
                let $img = $("#progressImgage");
                let $mask = $("#maskOfProgressImage");
                let $container;

                if (options && options.aimDiv) {
                    $container = $(options.aimDiv).css("position", "relative");
                    positionStyle = "absolute";
                }
                else {
                    $container = $("body");
                }

                //if ($img.length == 0) {
                //    let loadingImagePath = `${globalVariables.WebRoot}Content/${globalVariables.Merchant}/2015/images/loading.gif`;
                //    $img = $(`<img id='progressImgage'  src='${loadingImagePath}' />`); //Loading小图标
                //    $container.append($img);
                //}

                if ($mask.length == 0) {
                    $mask = $("<div id=\"maskOfProgressImage\"></div>").addClass("mask").addClass("hide"); //Div遮罩
                    $container.append($mask);

                    $mask.css({
                        "position": positionStyle,
                        "top": "0",
                        "right": "0",
                        "bottom": "0",
                        "left": "0",
                        "z-index": "88888888",
                        "background-color": "#000000",
                        "display": "none"
                    });
                }

                $img.show().css({
                    "position": positionStyle,
                    "top": "40%",
                    "left": "50%",
                    "margin-top": function () { return -1 * $img.height() / 2; },
                    "margin-left": function () { return -1 * $img.width() / 2; }
                });

                $mask.show().css("opacity", "0.1");
            },
            hide() {
                let $loading = $("#loading");
                if ($loading.length == 1) {
                    $loading.hide();

                    return;
                }

                $("#progressImgage").hide();
                $("#maskOfProgressImage").hide();
            },
            destroy() {
                $("#progressImgage").remove();
                $("#maskOfProgressImage").remove();
            }
        };

        if (methods[options]) {
            methods[options].apply(this, Array.prototype.slice.call(arguments, 1));
        }
        else if (typeof options === "object" || !options) {
            methods.init.apply(this, arguments);
        }

        return this;
    }

    $.fn.onlyNumber = function (maxLength: number = null) {
        const $this = $(this);

        //$this.keydown(e => {
        //    const code = e.keyCode;
        //    code !== 229 && (code < 47 || code > 58) && (code < 96 || code > 105) && code !== 8 && e.preventDefault();
        //});

        //$this.keyup(function () {
        //    this.value = this.value.replace(/[^0-9]/g, "");
        //});

        $this.on('input', function () {
            this.value = this.value.replace(/[^0-9]/g, "");

            if (maxLength && this.value.length > maxLength) {
                this.value = this.value.slice(0, maxLength);
            }
        });

        return this;
    }

    $.fn.setEyeToPasswordInput = function () {
        const $this = $(this);
        const $passwords = $this.find("input[type='password']");
        $passwords.each(index => {
            const $password = $($passwords[index]);
            const $parent = $password.parent();
            const $eye = $('<a href="javascript:void(0)" class="icon_input_eye_close"></a>');
            $eye.click(() => {
                const isPassword = $password.attr("type") === "password";
                $password.attr("type", isPassword ? "text" : "password");
                $eye.removeClass().addClass(isPassword ? "icon_input_eye_open" : "icon_input_eye_close");
            });
            $parent.append($eye);
        });

        return this;
    }

    // 之後如果要替換日期套件可修改這裡, 而不影響其它地方異動
    // 如果有使用到日期事件上的操作, 建議也使用擴充的方式
    $.fn.setDatePicker = function (setting: DatePickerSetting = {}): void {
        const now = new Date();
        const startDate = new Date(1990, now.getMonth() + 1, now.getDate());
        const mergeSettings: DatePickerSetting = $.extend({
            months: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"],
            days: ["日", "一", "二", "三", "四", "五", "六"],
            first_day_of_week: 0,
            startDate,
            show_select_today: false,
            show_clear_date: false,
            show_icon: false,
            default_position: "below",
            header_captions: { days: "Y年F", months: "Y", years: "Y1 - Y2" }
        }, setting);
        $(this).Zebra_DatePicker(mergeSettings);
    }

    $.fn.updateDatePicker = function (setting: DatePickerSetting = {}): void {
        const mergeSettings: DatePickerSetting = $.extend({
            months: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"],
            days: ["日", "一", "二", "三", "四", "五", "六"],
            first_day_of_week: 0,
            show_select_today: false,
            show_clear_date: false,
            show_icon: false,
            default_position: "below",
            header_captions: { days: "Y年F", months: "Y", years: "Y1 - Y2" }
        }, setting);
        const datepicker = $(this).data("Zebra_DatePicker");
        datepicker.update(mergeSettings);
    }

    $.fn.tmplBinding = function (model: any, target: JQuery<HTMLElement>, clear: boolean = true) {
        if (clear) {
            target.html("");
        }

        $(this).tmpl(model).appendTo(target);
    }

    $.fn.hoverTips = function (setting?: TipsSetting) {
        const defaultSetting = {
            animation: 'fade',
            trigger: 'hover',
            maxWidth: 250,
            position: 'right'
        };

        const $this = $(this);

        if ($this.hasClass('tooltipstered')) {
            $this.tooltipster('destroy');
        }

        const mergedSetting = $.extend(defaultSetting, setting);

        if (mergedSetting.content instanceof jQuery) {
            mergedSetting.content = mergedSetting.content.clone();
            // tooltip之後套件會取走本來的HTML元素，所以這邊用clone的HTML元素，避免影響本來的HTML元素
        }

        $this.tooltipster(mergedSetting);
    };

    $.fn.hoverData = function () {
        const $this = $(this);
        $this.each(function () {
            const $each = $(this);
            $each.addClass('article_mode text-truncate');
        });

        //要等上面做完讓width定型再做下面的
        $this.each(function () {
            const $each = $(this);
            if (!$each.attr('title')) {
                $each.attr('title', $each.html());
            }

            const maxWidth: number = parseInt($each.css('width')) || 400;
            $each.hoverTips({ position: 'top', maxWidth: maxWidth });
        });
    };

    $.fn.toggleWithSlide = function (isShow?: boolean, duration?: JQuery.Duration, complete?: (this: HTMLElement) => void): JQuery<HTMLElement> {
        return this.each(function () {
            let $this = $(this);

            if (isShow === undefined) {
                isShow = !$this.is(':visible');
            }

            if (isShow && !$this.is(':visible')) {
                return $this.slideDown(duration, complete);
            }
            else if (!isShow && $this.is(':visible')) {
                return $this.slideUp(duration, complete);
            }
        });
    };

    return this;
})(jQuery);

$.extend({
    logOff() {
        location.href = globalVariables.GetUrl("Authority/Logout");
    },
    localStorage: {
        get: (key: string): any => {
            const value = localStorage.getItem(key);

            return value ? JSON.parse(value) : undefined;
        },
        set: (key: string, value: any): void => {
            if (value) {
                localStorage.setItem(key, JSON.stringify(value));
            }
        }
    },
    timer: new Timer(),
    dropdownMenuSelectedEvent: (dropDownMenu: HTMLElement, text: string, value: string, callback: (text?: string, value?: string) => void) => {
        const $menu = $(dropDownMenu).parent(".dropdown_menu");
        const $dropdownButton = $menu.prev(".dropdown_toggle");
        $dropdownButton.data("value", value).find("span").text(text);
        $dropdownButton.find("input").val(value);

        if ($dropdownButton.closest("form").length > 0) {
            $dropdownButton.valid();
        }

        $menu.hide();

        if (typeof callback === "function" && typeof value === "string") {
            callback(text, value);
        }
    },
    // 如果彈窗功能變多樣了, 可以再改名稱
    alert: (message: string, yesBtn: string = "确定", yesBtnFnc?: () => void, noBtn?: string, noBtnFunc?: () => void) => {
        const btn = [yesBtn];
        if (noBtn) {
            btn.push(noBtn);
        }

        layer.alert(message,
            {
                icon: LayerIconType.Alert,
                title: "温馨提示",
                btn,
                yes: index => {
                    if (typeof yesBtnFnc === "function") {
                        yesBtnFnc();
                    }

                    layer.close(index);
                },
                btn1: noBtnFunc || (() => { }),
                closeBtn: false, //取消X顯示
            });
    },
    tip: (message: string, time: number = 3000) => {
        layer.msg(message, { time });
    },
    toast: (message: string, title?: string, toastrOptions?: ToastrOptions) => {
        const maxToastCount = 3;
        var currentToastCount = $("#toast-container .toast-message").length;

        if (currentToastCount >= maxToastCount) {
            return;
        }

        var options = {
            timeOut: 3000,
            closeButton: true,
            positionClass: "toast-top-center"
        } as ToastrOptions;

        if (toastrOptions) {
            $.extend(options, toastrOptions);
        }

        toastr.success(message, title, options);
    },
    copyToClipboard(text: string, callback?: () => void) {
        const textArea = document.createElement("textarea");
        textArea.value = text;
        document.body.appendChild(textArea);
        textArea.select();
        document.execCommand("copy");
        document.body.removeChild(textArea);
        if (typeof callback === "function") {
            callback();
        }
    },
    dateStringFormat: (date: Date) => {
        return date.toLocaleDateString("zh-CN").replace(/\//g, "-");
    },
    isValidDateStringFormat: (dateString: string) => {
        let dateFormat: RegExp = /^\d{4}-(0?[1-9]|1[012])-(0?[1-9]|[12][0-9]|3[01])$/;

        return dateFormat.test(dateString);
    },
    setStartAndEndDatePicker: ($startDateSelector: JQuery<HTMLElement>, $endDateSelector: JQuery<HTMLElement>, isReadonly?: boolean, minStartDate?: Date) => {
        if (!minStartDate) {
            const smallestDate = new Date(0);
            minStartDate = smallestDate;
        }

        if (isReadonly === null) {
            isReadonly = true;
        }

        const today = new Date();

        $startDateSelector.setDatePicker({
            onOpen: () => {
                $.datePickerOnOpen($startDateSelector,
                    minStartDate,
                    $endDateSelector.val() ? new Date($endDateSelector.val().toString()) : today);
            },
            readonly_element: isReadonly
        });

        $endDateSelector.setDatePicker({
            onOpen: () => {
                $.datePickerOnOpen($endDateSelector,
                    $startDateSelector.val() ? new Date($startDateSelector.val().toString()) : minStartDate,
                    today);
            },
            readonly_element: isReadonly
        });
    },
    datePickerOnOpen: ($datePickerSelector: JQuery<HTMLElement>, minDate: Date, maxDate: Date): void => {
        const minDateFormatString = $.dateStringFormat(minDate);
        const maxDateFormatString = $.dateStringFormat(maxDate);

        const originalValue = $datePickerSelector.val();

        if (!originalValue) {
            $datePickerSelector.val(maxDateFormatString); //設定為最大值，讓datePicker打開時offset在最後
        }

        $datePickerSelector.updateDatePicker({
            direction: [minDateFormatString, maxDateFormatString],
            offset: [0, $datePickerSelector.height()]
        });

        $datePickerSelector.val(originalValue);
    }
});

// 現在有畫面開始共用, 之後要檢查是否可以取代掉舊的
$.ajax2 = $.ajax2 || ((options, selector?: string) => {
    const complete = options.complete;
    const fnSuccess = options.success;
    const failedFunc = options.failedFunc;

    const $loading = $(selector || "#loading").show();

    options.success = (data, status: string, jqXhr: any) => {
        if (fnSuccess) {
            fnSuccess(data, status, jqXhr);

            return;
        }

        if (data.message && !data.success) {
            failedFunc ? failedFunc(data) : alert(data.message);
            return;
        }
    };
    options.complete = (httpRequest, status) => {
        if (options.isAutoHideLoading !== false) {
            $loading.hide();
        }

        if (complete) {
            complete(httpRequest, status);
        }
    };

    const errorHandler = options.error;
    options.error = (response) => {
        $loading.hide();

        if (errorHandler) {
            errorHandler(response);
        }
    };

    return $.ajax(options);
});

$.ajaxSetup({
    cache: false,
    statusCode: {
        400(data) {
            if (data.responseJSON) {
                let message = data.responseJSON.Message || data.responseJSON.message;

                if (message !== undefined && message !== '' && message != null) {
                    alert(message);
                }
            }
        },
        401(data) {
            $.logOff();
        },
        403(data) {
            if (data.responseJSON) {
                let message = data.responseJSON.Message || data.responseJSON.message;

                if (message !== undefined && message !== '' && message != null) {
                    alert(message);
                }
            } else {
                $.logOff();
            }
        },
        500(data) {
            if (data.responseJSON) {
                let message = data.responseJSON.message;

                if (message !== undefined && message !== '' && message != null) {
                    alert(message);
                }
            }
        }
    },
    error: (xhr, textStatus, error) => {
        if (xhr.status !== 0) {
        }
    }
});

window.alert = (msg, callback?: () => void) => {
    setTimeout(() => {
        layer.alert(msg, {
            icon: 0,
            title: "温馨提示",
            closeBtn: false,
            shadeClose: true
        }, index => {
            if (typeof callback === "function") callback();
            layer.close(index);
        });
    }, 10);
};

window.confirm = (msg, callback?: () => void, cancel?: () => void) => {
    setTimeout(() => {
        layer.confirm(msg,
            { icon: 3, title: "确认信息", shadeClose: true, closeBtn: false },
            index => {
                if (typeof callback === "function") callback();
                layer.close(index);
            },
            index => {
                if (typeof cancel === "function") cancel();
                layer.close(index);
            });
    },
        10);
    return true;
};

window.generateImagePath = (imgPath: string) => {
    return `/wwwroot/images/${imgPath.replace("/", "")}`;
}