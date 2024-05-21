var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (g && (g = 0, op[0] && (_ = 0)), _) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
var bannerSearchService = (function (_super) {
    __extends(bannerSearchService, _super);
    function bannerSearchService(pageApiUrlSetting) {
        var _this = _super.call(this, pageApiUrlSetting) || this;
        _this.$tabSelectorItems = $('.pageTab ul a');
        _this.defaultPageSize = 1000;
        $(document).ready(function () {
            _this.registerClickEvent();
            _this.$tabSelectorItems.first().click();
        });
        return _this;
    }
    bannerSearchService.prototype.getInsertViewArea = function () {
        return {
            width: 600,
            height: 750,
        };
    };
    bannerSearchService.prototype.getUpdateViewArea = function () {
        return {
            width: 600,
            height: 750,
        };
    };
    bannerSearchService.prototype.registerClickEvent = function () {
        var _this = this;
        this.$tabSelectorItems.click(function (event) {
            event.preventDefault();
            _this.$tabSelectorItems.removeClass("active");
            $(event.currentTarget).addClass("active");
            var tabId = $(event.currentTarget).attr("id");
            $("#locationTypeTemp").text(tabId);
            if (tabId == "1") {
                $(".locationTypeTips").text("※首页主选单：避免变型，请制作 68*60 固定 2倍或以上比例的图片。档案格式仅允许 png。档案大小1MB以下。");
            }
            if (tabId == "2") {
                $(".locationTypeTips").text("※首页轮播Banner：避免变型，请制作 355*112 或同比例的图片。档案格式允许 jpg 或 png。档案大小1MB以下。");
            }
            if (tabId == "3") {
                $(".locationTypeTips").text("※店铺轮播Banner：避免变型，请制作 355*150 或同比例的图片。档案格式允许 jpg 或 png。档案大小1MB以下。");
            }
            var $jqSearchBtn = $(".jqSearchBtn");
            $jqSearchBtn.click();
        });
    };
    bannerSearchService.prototype.getSubmitData = function () {
        var data = new bannerSearchParam();
        data.locationType = $('#locationTypeTemp').text();
        return data;
    };
    bannerSearchService.prototype.getBase64Image = function (keyContent) {
        return __awaiter(this, void 0, void 0, function () {
            var src, newWin;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        if (!(keyContent.indexOf('aes') > 0)) return [3, 2];
                        return [4, new decryptoService().fetchSingleDownload(keyContent)];
                    case 1:
                        src = _a.sent();
                        newWin = window.open('', '_blank');
                        newWin.document.body.style.backgroundColor = "black";
                        newWin.document.body.innerHTML = "<div style='height:100%;display:grid;place-items:center;'><img src='" + src + "'</div>";
                        return [3, 3];
                    case 2:
                        window.open(keyContent);
                        _a.label = 3;
                    case 3: return [2];
                }
            });
        });
    };
    return bannerSearchService;
}(baseCRUDService));
var bannerSearchParam = (function (_super) {
    __extends(bannerSearchParam, _super);
    function bannerSearchParam() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return bannerSearchParam;
}(PagingRequestParam));
