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
var recycleBalanceService = (function (_super) {
    __extends(recycleBalanceService, _super);
    function recycleBalanceService(pageApiUrlSetting, htmlGridSearchContent) {
        if (htmlGridSearchContent === void 0) { htmlGridSearchContent = {
            $contentBody: $('#jqGridContent'),
            $gridContentRoot: $('#jqContentRoot'),
            $gridContentFooter: $('#jqContentFooter'),
            $pagination: $('.jqPagination'),
            $msUserContentBody: $('#jqMsUserGridContent'),
            $msUserGridContentRoot: $('#jqMsUserContentRoot'),
            userIdSelector: ".jqUserId"
        }; }
        var _this = _super.call(this, pageApiUrlSetting, htmlGridSearchContent) || this;
        _this.productRequestMap = {};
        _this.recycleBalanceSearchContent = htmlGridSearchContent;
        return _this;
    }
    recycleBalanceService.prototype.initRoutingKey = function (routingKey) {
        this.routingKey = routingKey;
    };
    recycleBalanceService.prototype.updateGridContent = function (response, htmlContents, self) {
        _super.prototype.updateGridContent.call(this, response, htmlContents, self);
        var $jqBody = $(response).find(".jqMsUserBody");
        var bodyHtml = $jqBody.html();
        var recycleBalanceSearchContent = htmlContents;
        if ($jqBody.find(recycleBalanceSearchContent.userIdSelector).text() === "") {
            var $table = recycleBalanceSearchContent.$msUserContentBody.closest("table");
            bodyHtml = _super.prototype.GetNoSearchResultHtml.call(this, $table);
        }
        recycleBalanceSearchContent.$msUserContentBody.html(bodyHtml);
    };
    recycleBalanceService.prototype.doAfterSearch = function (htmlContents, self) {
        _super.prototype.doAfterSearch.call(this, htmlContents, self);
        var recycleBalanceSearchContent = htmlContents;
        recycleBalanceSearchContent.$msUserGridContentRoot.show();
    };
    recycleBalanceService.prototype.recycleAllBalance = function (element) {
        var $element = $(element);
        var url = $element.data("url");
        var userId = $element.data("userid");
        this.doRecycleBalance(url, userId, "", "所有");
    };
    recycleBalanceService.prototype.recycleSingleProductBalance = function (element) {
        var self = this;
        var $element = $(element);
        var url = $element.data("url");
        var userId = $element.data("userid");
        var productCode = $element.data("productcode");
        var productName = $element.data("productname");
        this.doRecycleBalance(url, userId, productCode, productName);
    };
    recycleBalanceService.prototype.doRecycleBalance = function (url, userId, productCode, confirmProductName) {
        var _this = this;
        var self = this;
        window.confirm("\u786E\u5B9A\u534F\u52A9\u7528\u6237\u56DE\u6536".concat(confirmProductName, "\u5E10\u6237\u4F59\u989D\u5417\uFF1F"), function () {
            var isAutoHideLoading = true;
            var requestId = _this.generateGUID();
            $.each($(".jqProductRow"), function (index, row) {
                var rowProductCode = $(row).attr("productCode");
                if (productCode === "" || productCode === rowProductCode) {
                    self.productRequestMap[rowProductCode] = requestId;
                    self.setOperationResultHtml(rowProductCode, "");
                }
            });
            var param = {
                userId: userId,
                productCode: productCode,
                routingKey: self.routingKey,
                requestId: requestId
            };
            $.ajax2({
                url: url,
                type: "POST",
                data: param,
                success: function (response) {
                    new baseReturnModelService(response).responseHandler(function () {
                    }, isAutoHideLoading);
                },
            });
        });
    };
    recycleBalanceService.prototype.UpdateOperationContent = function (transferMessage) {
        var productRequestId = this.productRequestMap[transferMessage.ProductCode];
        if (transferMessage.RequestId !== productRequestId) {
            return;
        }
        this.appendOperationResultHtml(transferMessage.ProductCode, transferMessage.Summary);
    };
    recycleBalanceService.prototype.UpdateMiseLiveBalance = function () {
        var url = globalVariables.GetUrl("RecycleBalance/GetMiseLiveBalance");
        var userId = $(this.recycleBalanceSearchContent.userIdSelector).text();
        $.ajax({
            url: url,
            type: "GET",
            data: { userId: userId },
            success: function (response) {
                $(".jqMiseLiveGameBalance").text(response);
            },
        });
    };
    recycleBalanceService.prototype.UpdateTPGameBalance = function (productCode) {
        var url = globalVariables.GetUrl("RecycleBalance/GetTPGameBalance");
        var userId = $(this.recycleBalanceSearchContent.userIdSelector).text();
        $.ajax({
            url: url,
            type: "GET",
            data: { userId: userId, productCode: productCode },
            success: function (response) {
                $(".jqProductRow[productCode='".concat(productCode, "'] .jqTPGameBalance")).text(response);
            },
        });
    };
    recycleBalanceService.prototype.generateGUID = function () {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (Math.random() * 16) | 0;
            var v = c === 'x' ? r : (r & 0x3) | 0x8;
            return v.toString(16);
        });
    };
    recycleBalanceService.prototype.appendOperationResultHtml = function (productCode, content) {
        var $jqOperationResult = this.getJqOperationResult(productCode);
        var operationHtml = $jqOperationResult.html();
        if (operationHtml !== "") {
            operationHtml += "<br/>";
        }
        operationHtml += content;
        $jqOperationResult.html(operationHtml);
    };
    recycleBalanceService.prototype.setOperationResultHtml = function (productCode, content) {
        var $jqOperationResult = this.getJqOperationResult(productCode);
        $jqOperationResult.html(content);
    };
    recycleBalanceService.prototype.getJqOperationResult = function (productCode) {
        var $jqOperationResult = this.recycleBalanceSearchContent.$contentBody
            .find("tr[productcode='".concat(productCode, "']"))
            .find(".jqOperationResult");
        return $jqOperationResult;
    };
    return recycleBalanceService;
}(baseSearchGridService));
var TransferMessage = (function () {
    function TransferMessage() {
    }
    return TransferMessage;
}());
var recycleBalanceHtmlGridSearchContent = (function (_super) {
    __extends(recycleBalanceHtmlGridSearchContent, _super);
    function recycleBalanceHtmlGridSearchContent() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return recycleBalanceHtmlGridSearchContent;
}(htmlGridSearchContent));
