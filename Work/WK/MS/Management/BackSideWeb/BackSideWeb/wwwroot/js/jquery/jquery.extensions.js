var _this = this;
var LayerIconType;
(function (LayerIconType) {
    LayerIconType[LayerIconType["Alert"] = 0] = "Alert";
    LayerIconType[LayerIconType["Ok"] = 1] = "Ok";
    LayerIconType[LayerIconType["Error"] = 2] = "Error";
    LayerIconType[LayerIconType["Tip"] = 3] = "Tip";
    LayerIconType[LayerIconType["Lock"] = 4] = "Lock";
    LayerIconType[LayerIconType["Disagree"] = 5] = "Disagree";
    LayerIconType[LayerIconType["Agree"] = 6] = "Agree";
})(LayerIconType || (LayerIconType = {}));
var Timer = (function () {
    function Timer() {
        this.timers = [];
    }
    Timer.prototype.set = function (seconds, callback, timeout) {
        var _this = this;
        callback(seconds);
        var countDownSec = seconds * 1000;
        var end = Date.now() + countDownSec;
        var index = setInterval(function () {
            var now = Date.now();
            var seconds = Math.ceil((end - now) / 1000);
            if (seconds > -1) {
                callback(seconds);
            }
            else {
                _this.clear(index);
                if (typeof timeout === "function") {
                    timeout();
                }
            }
        }, 1000);
        this.timers.push(index);
        return index;
    };
    Timer.prototype.clear = function (timer) {
        var index = this.timers.findIndex(function (t) { return t === timer; });
        if (index > -1) {
            clearInterval(timer);
            this.timers.splice(index, 1);
        }
    };
    Timer.prototype.allClear = function () {
        this.timers.forEach(function (index) { return clearInterval(index); });
        this.timers = [];
    };
    return Timer;
}());
var AjaxUpdateMode;
(function (AjaxUpdateMode) {
    AjaxUpdateMode[AjaxUpdateMode["Replace"] = 1] = "Replace";
    AjaxUpdateMode[AjaxUpdateMode["InsertBefore"] = 2] = "InsertBefore";
    AjaxUpdateMode[AjaxUpdateMode["InsertAfter"] = 3] = "InsertAfter";
})(AjaxUpdateMode || (AjaxUpdateMode = {}));
(function ($) {
    $.fn.reverse = [].reverse;
    var defaultSettings = {
        image: '~/images/validator/loading.gif',
        alternateText: "Loading",
        autoShow: true
    };
    $.fn.loading = function (options) {
        var methods = {
            init: function (options) {
                var settings = jQuery.extend({}, defaultSettings, options);
                if (settings.autoShow) {
                    methods.show.apply(this);
                }
            },
            show: function () {
                var $loading = $("#loading");
                if ($loading.length == 1) {
                    $loading.show();
                    return;
                }
                var positionStyle = "fixed";
                var $img = $("#progressImgage");
                var $mask = $("#maskOfProgressImage");
                var $container;
                if (options && options.aimDiv) {
                    $container = $(options.aimDiv).css("position", "relative");
                    positionStyle = "absolute";
                }
                else {
                    $container = $("body");
                }
                if ($mask.length == 0) {
                    $mask = $("<div id=\"maskOfProgressImage\"></div>").addClass("mask").addClass("hide");
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
            hide: function () {
                var $loading = $("#loading");
                if ($loading.length == 1) {
                    $loading.hide();
                    return;
                }
                $("#progressImgage").hide();
                $("#maskOfProgressImage").hide();
            },
            destroy: function () {
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
    };
    $.fn.onlyNumber = function (maxLength) {
        if (maxLength === void 0) { maxLength = null; }
        var $this = $(this);
        $this.on('input', function () {
            this.value = this.value.replace(/[^0-9]/g, "");
            if (maxLength && this.value.length > maxLength) {
                this.value = this.value.slice(0, maxLength);
            }
        });
        return this;
    };
    $.fn.setEyeToPasswordInput = function () {
        var $this = $(this);
        var $passwords = $this.find("input[type='password']");
        $passwords.each(function (index) {
            var $password = $($passwords[index]);
            var $parent = $password.parent();
            var $eye = $('<a href="javascript:void(0)" class="icon_input_eye_close"></a>');
            $eye.click(function () {
                var isPassword = $password.attr("type") === "password";
                $password.attr("type", isPassword ? "text" : "password");
                $eye.removeClass().addClass(isPassword ? "icon_input_eye_open" : "icon_input_eye_close");
            });
            $parent.append($eye);
        });
        return this;
    };
    $.fn.setDatePicker = function (setting) {
        if (setting === void 0) { setting = {}; }
        var now = new Date();
        var startDate = new Date(1990, now.getMonth() + 1, now.getDate());
        var mergeSettings = $.extend({
            months: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"],
            days: ["日", "一", "二", "三", "四", "五", "六"],
            first_day_of_week: 0,
            startDate: startDate,
            show_select_today: false,
            show_clear_date: false,
            show_icon: false,
            default_position: "below",
            header_captions: { days: "Y年F", months: "Y", years: "Y1 - Y2" }
        }, setting);
        $(this).Zebra_DatePicker(mergeSettings);
    };
    $.fn.updateDatePicker = function (setting) {
        if (setting === void 0) { setting = {}; }
        var mergeSettings = $.extend({
            months: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"],
            days: ["日", "一", "二", "三", "四", "五", "六"],
            first_day_of_week: 0,
            show_select_today: false,
            show_clear_date: false,
            show_icon: false,
            default_position: "below",
            header_captions: { days: "Y年F", months: "Y", years: "Y1 - Y2" }
        }, setting);
        var datepicker = $(this).data("Zebra_DatePicker");
        datepicker.update(mergeSettings);
    };
    $.fn.tmplBinding = function (model, target, clear) {
        if (clear === void 0) { clear = true; }
        if (clear) {
            target.html("");
        }
        $(this).tmpl(model).appendTo(target);
    };
    $.fn.hoverTips = function (setting) {
        var defaultSetting = {
            animation: 'fade',
            trigger: 'hover',
            maxWidth: 250,
            position: 'right'
        };
        var $this = $(this);
        if ($this.hasClass('tooltipstered')) {
            $this.tooltipster('destroy');
        }
        var mergedSetting = $.extend(defaultSetting, setting);
        if (mergedSetting.content instanceof jQuery) {
            mergedSetting.content = mergedSetting.content.clone();
        }
        $this.tooltipster(mergedSetting);
    };
    $.fn.hoverData = function () {
        var $this = $(this);
        $this.each(function () {
            var $each = $(this);
            $each.addClass('article_mode text-truncate');
        });
        $this.each(function () {
            var $each = $(this);
            if (!$each.attr('title')) {
                $each.attr('title', $each.html());
            }
            var maxWidth = parseInt($each.css('width')) || 400;
            $each.hoverTips({ position: 'top', maxWidth: maxWidth });
        });
    };
    $.fn.toggleWithSlide = function (isShow, duration, complete) {
        return this.each(function () {
            var $this = $(this);
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
    return _this;
})(jQuery);
$.extend({
    logOff: function () {
        location.href = globalVariables.GetUrl("Authority/Logout");
    },
    localStorage: {
        get: function (key) {
            var value = localStorage.getItem(key);
            return value ? JSON.parse(value) : undefined;
        },
        set: function (key, value) {
            if (value) {
                localStorage.setItem(key, JSON.stringify(value));
            }
        }
    },
    timer: new Timer(),
    dropdownMenuSelectedEvent: function (dropDownMenu, text, value, callback) {
        var $menu = $(dropDownMenu).parent(".dropdown_menu");
        var $dropdownButton = $menu.prev(".dropdown_toggle");
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
    alert: function (message, yesBtn, yesBtnFnc, noBtn, noBtnFunc) {
        if (yesBtn === void 0) { yesBtn = "确定"; }
        var btn = [yesBtn];
        if (noBtn) {
            btn.push(noBtn);
        }
        layer.alert(message, {
            icon: LayerIconType.Alert,
            title: "温馨提示",
            btn: btn,
            yes: function (index) {
                if (typeof yesBtnFnc === "function") {
                    yesBtnFnc();
                }
                layer.close(index);
            },
            btn1: noBtnFunc || (function () { }),
            closeBtn: false,
        });
    },
    tip: function (message, time) {
        if (time === void 0) { time = 3000; }
        layer.msg(message, { time: time });
    },
    toast: function (message, title, toastrOptions) {
        var maxToastCount = 3;
        var currentToastCount = $("#toast-container .toast-message").length;
        if (currentToastCount >= maxToastCount) {
            return;
        }
        var options = {
            timeOut: 3000,
            closeButton: true,
            positionClass: "toast-top-center"
        };
        if (toastrOptions) {
            $.extend(options, toastrOptions);
        }
        toastr.success(message, title, options);
    },
    copyToClipboard: function (text, callback) {
        var textArea = document.createElement("textarea");
        textArea.value = text;
        document.body.appendChild(textArea);
        textArea.select();
        document.execCommand("copy");
        document.body.removeChild(textArea);
        if (typeof callback === "function") {
            callback();
        }
    },
    dateStringFormat: function (date) {
        return date.toLocaleDateString("zh-CN").replace(/\//g, "-");
    },
    isValidDateStringFormat: function (dateString) {
        var dateFormat = /^\d{4}-(0?[1-9]|1[012])-(0?[1-9]|[12][0-9]|3[01])$/;
        return dateFormat.test(dateString);
    },
    setStartAndEndDatePicker: function ($startDateSelector, $endDateSelector, isReadonly, minStartDate) {
        if (!minStartDate) {
            var smallestDate = new Date(0);
            minStartDate = smallestDate;
        }
        if (isReadonly === null) {
            isReadonly = true;
        }
        var today = new Date();
        $startDateSelector.setDatePicker({
            onOpen: function () {
                $.datePickerOnOpen($startDateSelector, minStartDate, $endDateSelector.val() ? new Date($endDateSelector.val().toString()) : today);
            },
            readonly_element: isReadonly
        });
        $endDateSelector.setDatePicker({
            onOpen: function () {
                $.datePickerOnOpen($endDateSelector, $startDateSelector.val() ? new Date($startDateSelector.val().toString()) : minStartDate, today);
            },
            readonly_element: isReadonly
        });
    },
    datePickerOnOpen: function ($datePickerSelector, minDate, maxDate) {
        var minDateFormatString = $.dateStringFormat(minDate);
        var maxDateFormatString = $.dateStringFormat(maxDate);
        var originalValue = $datePickerSelector.val();
        if (!originalValue) {
            $datePickerSelector.val(maxDateFormatString);
        }
        $datePickerSelector.updateDatePicker({
            direction: [minDateFormatString, maxDateFormatString],
            offset: [0, $datePickerSelector.height()]
        });
        $datePickerSelector.val(originalValue);
    }
});
$.ajax2 = $.ajax2 || (function (options, selector) {
    var complete = options.complete;
    var fnSuccess = options.success;
    var failedFunc = options.failedFunc;
    var $loading = $(selector || "#loading").show();
    options.success = function (data, status, jqXhr) {
        if (fnSuccess) {
            fnSuccess(data, status, jqXhr);
            return;
        }
        if (data.message && !data.success) {
            failedFunc ? failedFunc(data) : alert(data.message);
            return;
        }
    };
    options.complete = function (httpRequest, status) {
        if (options.isAutoHideLoading !== false) {
            $loading.hide();
        }
        if (complete) {
            complete(httpRequest, status);
        }
    };
    var errorHandler = options.error;
    options.error = function (response) {
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
        400: function (data) {
            if (data.responseJSON) {
                var message = data.responseJSON.Message || data.responseJSON.message;
                if (message !== undefined && message !== '' && message != null) {
                    alert(message);
                }
            }
        },
        401: function (data) {
            $.logOff();
        },
        403: function (data) {
            if (data.responseJSON) {
                var message = data.responseJSON.Message || data.responseJSON.message;
                if (message !== undefined && message !== '' && message != null) {
                    alert(message);
                }
            }
            else {
                $.logOff();
            }
        },
        500: function (data) {
            if (data.responseJSON) {
                var message = data.responseJSON.message;
                if (message !== undefined && message !== '' && message != null) {
                    alert(message);
                }
            }
        }
    },
    error: function (xhr, textStatus, error) {
        if (xhr.status !== 0) {
        }
    }
});
window.alert = function (msg, callback) {
    setTimeout(function () {
        layer.alert(msg, {
            icon: 0,
            title: "温馨提示",
            closeBtn: false,
            shadeClose: true
        }, function (index) {
            if (typeof callback === "function")
                callback();
            layer.close(index);
        });
    }, 10);
};
window.confirm = function (msg, callback, cancel) {
    setTimeout(function () {
        layer.confirm(msg, { icon: 3, title: "确认信息", shadeClose: true, closeBtn: false }, function (index) {
            if (typeof callback === "function")
                callback();
            layer.close(index);
        }, function (index) {
            if (typeof cancel === "function")
                cancel();
            layer.close(index);
        });
    }, 10);
    return true;
};
window.generateImagePath = function (imgPath) {
    return "/wwwroot/images/".concat(imgPath.replace("/", ""));
};
