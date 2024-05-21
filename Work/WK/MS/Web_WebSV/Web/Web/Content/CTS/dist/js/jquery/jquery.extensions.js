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
var Modal = (function () {
    function Modal() {
    }
    Modal.prototype.openHtml = function (html) {
        this.initModal();
        this.$modal.html(html);
    };
    Modal.prototype.open = function (templateId, model) {
        this.initModal();
        $(templateId).tmpl(model).appendTo(this.$modal);
    };
    Modal.prototype.close = function () {
        if (this.$modal) {
            this.$modal.html("");
        }
    };
    Modal.prototype.initModal = function () {
        if (!this.$modal) {
            this.$modal = $('<div id="modal"></div>');
            $("body").append(this.$modal);
        }
    };
    return Modal;
}());
var MobilePersonalCenter = (function () {
    function MobilePersonalCenter() {
        var _this = this;
        $(window).resize(function () {
            if (_this.$personalCenter.is(":visible")) {
                _this.$personalCenter.hide();
            }
        });
    }
    Object.defineProperty(MobilePersonalCenter.prototype, "$personalCenter", {
        get: function () {
            return $("#m-personal-center");
        },
        enumerable: false,
        configurable: true
    });
    MobilePersonalCenter.prototype.toggle = function (callback) {
        this.$personalCenter.is(":hidden") ? this.$personalCenter.fadeIn() : this.$personalCenter.fadeOut();
        if (typeof callback === "function") {
            callback();
        }
    };
    return MobilePersonalCenter;
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
        image: '/Content/MerchantShared/validator/images/loading.gif',
        alternateText: "Loading",
        autoShow: true
    };
    $.fn.hideLoading = function () {
        $.fn.loading('hide');
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
                if ($img.length == 0) {
                    var loadingImagePath = globalVariables.WebRoot + "Content/CTS/2015/images/loading.gif";
                    $img = $("<img id='progressImgage'  src='" + loadingImagePath + "' />");
                    $container.append($img);
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
    $.fn.onlyNumber = function () {
        var $this = $(this);
        $this.keydown(function (e) {
            var code = e.keyCode;
            code !== 229 && (code < 47 || code > 58) && (code < 96 || code > 105) && code !== 8 && e.preventDefault();
        });
        $this.keyup(function () {
            this.value = this.value.replace(/[^0-9]/g, "");
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
    $.fn.verifyCodes = function () {
        var $this = $(this);
        $this.keyup(function (event) {
            var $target = $(event.currentTarget);
            if (event.keyCode === 8) {
                if (!$target.val()) {
                    $this.each(function (index) {
                        var $code = $($this[index]);
                        if (!$code.val()) {
                            $($this[index - 1]).focus();
                            return false;
                        }
                    });
                }
            }
            else {
                $this.each(function (index) {
                    var $code = $($this[index]);
                    if (!$code.val()) {
                        $code.focus();
                        return false;
                    }
                });
            }
        });
        return this;
    };
    $.fn.combineValues = function () {
        var _this = this;
        var $this = $(this);
        var value = "";
        $this.each(function (index) { value += $($(_this)[index]).val(); });
        return value;
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
    $.fn.getCaptcha = function (actionTypeValue, identityToken) {
        var $this = $(this);
        $this.click(function () {
            $.ajax({
                url: globalVariables.GetUrl("Public/GenValidatorImg"),
                type: "POST",
                data: { actionTypeValue: actionTypeValue, identityToken: identityToken },
                success: function (response) {
                    if (response.isSuccess) {
                        $this.html("<img src=\"data:image/png;base64," + response.validatorImg + "\" />");
                    }
                }
            });
        });
        $this.click();
    };
    $.fn.tmplBinding = function (model, target, clear) {
        if (clear === void 0) { clear = true; }
        if (clear) {
            target.html("");
        }
        $(this).tmpl(model).appendTo(target);
    };
    $.fn.countdownBtn = function (seconds, btnText) {
        var $this = $(this);
        $this.html(btnText);
        $this.addClass("disable");
        $.timer.set(seconds, function (seconds) { return $this.html(seconds + "s"); }, function () { return $this.removeClass("disable").html(btnText); });
    };
    $.fn.sendAuthVerifyCode = function (webActionTypeValue, inputTypeValue, inputTypeData, moneyPassword) {
        var $this = $(this);
        $this.addClass("disable");
        $.ajax2({
            url: globalVariables.GetUrl("Authentication/SendAuthVerifyCode"),
            type: "POST",
            data: { webActionTypeValue: webActionTypeValue, inputTypeValue: inputTypeValue, inputTypeData: inputTypeData, moneyPassword: moneyPassword },
            success: function (response) {
                if (response.IsSuccess) {
                    $this.countdownBtn(90, "获取验证码");
                }
                else {
                    alert(response.Message);
                    $this.removeClass("disable");
                }
            }
        });
    };
    $.fn.jqTooltip = function () {
        var $this = $(this);
        $this.tooltip({ position: { my: "center bottom", at: "center top" } });
        $(".ui-helper-hidden-accessible").remove();
        return $this.tooltip("instance");
    };
    return _this;
})(jQuery);
$.extend({
    refreshUserInfo: function (enabledLoading) {
        var url = "/Home/GetRefreshBalanceInfo";
        var ajax = enabledLoading ? $.ajax2 : $.ajax;
        ajax({
            type: "POST",
            url: url,
            success: function (response) {
                $(".jqMainAvailableScores").text(response.AvailableScores);
                $(".jqMainFreezeScores").text(response.FreezeScores);
                $(".jqMainAllGain").text(response.AllGain);
                $.each(response.UserProductScores, function (_, value) {
                    $(".jqTPGameAvailableScores[productCode='" + value.ProductCode + "']")
                        .text(value.AvailableScoresText);
                    $(".jqTPGameFreezeScores[productCode='" + value.ProductCode + "']").text(value.FreezeScoresText);
                    $(".jqTPGameAllGain[productCode='" + value.ProductCode + "']").text(value.AllGain);
                });
            }
        });
    },
    toast: function (setting) {
        setting = $.extend({ timeout: 5000 }, setting);
        var $body = $("body");
        var $notification = $($(".notification")[0] || $('<div class="notification"></div>'));
        var $item = $('<div class="list"><div>');
        var $close = $('<div class="close"></div>')
            .append($("<img src=\"/Content/CTS/2015/images/notification/ic_notification_close.svg\" alt=\"\">"));
        var $icon = $("<div></div>").addClass("notify_icon")
            .append($("<img src=\"" + setting.icon + "\" alt=\"\">"))
            .append($("<div></div>").addClass(["name", setting.titleClass]).text(setting.title));
        var $message = $("<div></div>").addClass("notify_text").append($("<p class=\"break-all\">" + setting.message + "</p>"));
        $item.append($close).append($icon).append($message);
        $notification.prepend($item.hide().fadeIn(300));
        var removeItemAction = function (id) {
            $item.fadeOut(300, function () { return $item.remove(); });
            clearTimeout(id);
        };
        var timeId = setTimeout(function () { return removeItemAction(timeId); }, setting.timeout);
        $close.click(function () { return removeItemAction(timeId); });
        $body.append($notification);
    },
    transferToast: function (title, message) {
        $.toast({
            title: title,
            titleClass: "transfer",
            message: message,
            icon: "/Content/CTS/2015/images/notification/ic_notification_transfer.jpg"
        });
    },
    betToast: function (title, message) {
        $.toast({
            title: title,
            titleClass: "bet",
            message: message,
            icon: "/Content/CTS/2015/images/notification/ic_notification_bet.jpg"
        });
    },
    newsToast: function (title, message) {
        $.toast({
            title: title,
            titleClass: "news",
            message: message,
            icon: "/Content/CTS/2015/images/notification/ic_notification_news.jpg"
        });
    },
    logOff: function (isRedirect) {
        $.ajax({
            url: globalVariables.WebRoot + "Public/LogOff",
            type: "POST",
            success: function (response) {
                localStorage.clear();
                localStorage["LoginFlag"] = 0;
                if (!isRedirect) {
                    window.location.href = response;
                }
            }
        });
    },
    modal: new Modal(),
    ajaxHtmlUpdate: function ($target, setting) {
        var mergeSetting = $.extend({ mode: AjaxUpdateMode.Replace }, setting);
        $.ajax2({
            url: mergeSetting.url,
            type: mergeSetting.type,
            data: mergeSetting.data,
            success: function (response) {
                switch (mergeSetting.mode) {
                    case AjaxUpdateMode.InsertBefore:
                        $target.prepend(response);
                    case AjaxUpdateMode.InsertAfter:
                        $target.append(response);
                    default:
                        $target.html(response);
                }
                return;
            }
        });
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
    maskPhoneNumber: function (phone) {
        phone = phone || "";
        if (phone.length < 4) {
            return phone;
        }
        return phone.length > 6
            ? "" + phone.slice(0, 3) + "*".repeat(phone.length - 6) + phone.slice(phone.length - 3)
            : "" + phone[0] + "*".repeat(phone.length - 2) + phone[phone.length - 1];
    },
    permissionCheck: function (webActionTypeValue, success, failed) {
        $.ajax2({
            url: globalVariables.GetUrl("Account/GetUserAuthenticatorPermission"),
            type: "POST",
            data: { webActionTypeValue: webActionTypeValue },
            success: function (response) {
                success = success || (function (_) { });
                failed = failed || (function (_) { });
                response.IsAllowExecuted ? success(response) : failed(response);
            }
        });
    },
    dropdownMenuSelectedEvent: function (key, value, callback) {
        var $menu = $(event.currentTarget).parent(".dropdown_menu");
        var $dropdown = $menu.prev(".dropdown_toggle");
        $dropdown.data("value", value).text(key);
        $menu.toggle();
        if (typeof callback === "function" && typeof value === "string") {
            callback(key, value);
        }
    },
    isMobileSize: function () { return window.innerWidth <= 768; },
    mobilePersonalCenter: new MobilePersonalCenter(),
    gotoHomePage: function () { return location.href = globalVariables.GetUrl("Home/Default"); },
    contactCustomerService: function () {
        setTimeout(function () {
            var index = layer.open({
                title: "客服咨询",
                type: 2,
                area: ["1200px", "800px"],
                content: [globalVariables.CustomerService, "no"],
                shade: 0,
                shadeClose: false
            });
            if ($.isMobileSize()) {
                layer.full(index);
            }
        }, 10);
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
            btn2: noBtnFunc || (function () { }),
            closeBtn: false,
        });
    },
    tip: function (message, time) {
        if (time === void 0) { time = 3000; }
        layer.msg(message, { time: time });
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
    updateSpeedFinanceCount: function () {
        $.ajax2({
            url: globalVariables.GetUrl("Trade/GetUserProcessingSpeedFinanceCount"),
            type: "GET",
            success: function (response) {
                if (response.data) {
                    var rechargeCount = response.data.ProcessingSpeedRechargeCount;
                    var withdrawCount = response.data.ProcessingSpeedWithdrawalCount;
                    $(".dot>#speed-recharge, #fastrecharge>.dot>span").text(rechargeCount ? rechargeCount > 99 ? "99+" : rechargeCount : "");
                    $(".dot>#speed-withdraw, #fastwithdrawal>.dot>span").text(withdrawCount ? withdrawCount > 99 ? "99+" : withdrawCount : "");
                }
            }
        });
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
$.validator.passwordRule = function (element, params) {
    var $element = $(element);
    var value = $element.val();
    var hasSpaceStartOrEnd = value.trim().length !== value.length;
    if (hasSpaceStartOrEnd) {
        return "密码前后不可输入空格";
    }
    var passwordValidPattern = $element.attr("passwordValidPattern");
    var errorMessage = $element.attr("placeholder");
    if (!errorMessage) {
        errorMessage = $element.attr("errorMessage");
    }
    if (!errorMessage) {
        errorMessage = "format error";
    }
    var fnFormatErrorMsg = $element.attr("fnFormatErrorMsg");
    if (typeof window[fnFormatErrorMsg] == "function") {
        errorMessage = window[fnFormatErrorMsg](errorMessage);
    }
    var regex = new RegExp(passwordValidPattern);
    var result = regex.test(value);
    return result || errorMessage;
};
$.validator.displayValidationMsgs = function (selector, msg) {
    var $template = $("<script type=\"text/template\" type=\"text/x-jquery-tmpl\" id=\"jqErrorMsgTemplate\">\n                            <li>\n                                <span class=\"icon_input_error\"></span>\n                                <span class=\"jqErrorMsg\">${}</span>\n                            </li>\n                        </script>");
    $(selector).html($template.tmpl(msg)).toggle($template.tmpl(msg) !== "");
};
$.validator.getValidationMessageConfig = function () {
    return {
        timely: 3,
        rules: {},
        fields: {},
        messages: {},
        msgMaker: function (opt) {
            var $labelInput = $(opt.element).parents(".label_input");
            $labelInput.find(".input_error").remove();
            if (opt.msg) {
                var $error = $("<div class=\"input_error\" key=\"" + opt.key + "\">\n                                        <li>\n                                            <span class=\"icon_input_error\"></span>\n                                            <span>" + opt.msg + "</span>\n                                        </li>\n                                     </div>");
                $labelInput.append($error);
            }
            return false;
        },
        msgHide: function (element, _) {
            var key = $(element).attr("for");
            var $labelInput = $(element).parents(".label_input");
            $labelInput.find(".input_error[key='" + key + "']").remove();
        },
    };
};
$.validator.phoneRule = [/^1([3-9]{1})([0-9]{9})$/, "手机号码格式错误"];
$.validator.usernameStrongCheckRule = [/^(?=.*[a-zA-Z])(?=.*\d)[A-Za-z\d]{6,16}$/, "请输入6-16位字符, 需包含英文字母和数字"];
$.validator.passwordStrongCheckRule = [/^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*#?&]{6,16}$/, "请输入6-16位字符, 需包含英文字母和数字"];
$.validator.passwordSpaceCheckRule = $.validator.passwordRule;
$.validator.moneyPasswordStrongCheckRule = [/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)[A-Za-z\d@$!%*#?&]{10,16}$/, "请输入10-16位字符，需包含大小写英文和数字"];
$.validator.moneyPasswordSpaceCheckRule = $.validator.passwordRule;
$.ajax2 = $.ajax2 || (function (options, selector) {
    var complete = options.complete;
    var success = options.success;
    var error = options.error;
    var failedFunc = options.failedFunc;
    var $loading = $(selector || "#loading").show();
    options.success = function (data, status, jqXhr) {
        if (data.message && !data.success) {
            failedFunc ? failedFunc(data) : alert(data.message);
            return;
        }
        if (success) {
            success(data, status, jqXhr);
        }
    };
    options.complete = function (httpRequest, status) {
        $loading.hide();
        if (complete) {
            complete(httpRequest, status);
        }
    };
    return $.ajax(options);
});
$.ajaxSetup({
    cache: false,
    statusCode: {
        400: function (data) {
            if (data.responseJSON) {
                alert(data.responseJSON.Message || data.responseJSON.message);
            }
        },
        403: function (data) {
            if (data.responseJSON && data.responseJSON.isWebView) {
                alert(data.responseJSON.Message || data.responseJSON.message);
            }
            else {
                $.logOff();
            }
        }
    },
    success: function (data) {
        if (data && !data.isAlert) {
            alert("操作成功");
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
    return "/Content/CTS/2015/images/" + imgPath.replace("/", "");
};