class recycleBalanceService extends baseSearchGridService {
    private routingKey: string;
    private productRequestMap: Record<string, string> = {} //productCode, requestId
    private recycleBalanceSearchContent: recycleBalanceHtmlGridSearchContent;

    constructor(pageApiUrlSetting: IPageApiUrlSetting,
        htmlGridSearchContent: recycleBalanceHtmlGridSearchContent = {
            $contentBody: $('#jqGridContent'),
            $gridContentRoot: $('#jqContentRoot'),
            $gridContentFooter: $('#jqContentFooter'),
            $pagination: $('.jqPagination'),
            $msUserContentBody: $('#jqMsUserGridContent'),
            $msUserGridContentRoot: $('#jqMsUserContentRoot'),
            userIdSelector: ".jqUserId"
        }) {
        super(pageApiUrlSetting, htmlGridSearchContent);

        this.recycleBalanceSearchContent = htmlGridSearchContent;
    }

    initRoutingKey(routingKey: string): void {
        this.routingKey = routingKey;
    }

    protected override updateGridContent(response, htmlContents: IHtmlSearchContent, self: any) {
        super.updateGridContent(response, htmlContents, self);

        let $jqBody: JQuery<HTMLElement> = $(response).find(".jqMsUserBody");
        let bodyHtml = $jqBody.html();
        let recycleBalanceSearchContent = htmlContents as recycleBalanceHtmlGridSearchContent;

        if ($jqBody.find(recycleBalanceSearchContent.userIdSelector).text() === "") {
            let $table = recycleBalanceSearchContent.$msUserContentBody.closest("table");
            bodyHtml = super.GetNoSearchResultHtml($table);
        }

        recycleBalanceSearchContent.$msUserContentBody.html(bodyHtml);
    }

    protected override doAfterSearch(htmlContents: IHtmlSearchContent, self: any): void {
        super.doAfterSearch(htmlContents, self);
        let recycleBalanceSearchContent = htmlContents as recycleBalanceHtmlGridSearchContent;
        recycleBalanceSearchContent.$msUserGridContentRoot.show();
    }

    recycleAllBalance(element: HTMLElement): void {
        const $element = $(element);
        const url: string = $element.data("url");
        const userId: string = $element.data("userid");

        this.doRecycleBalance(url, userId, "", "所有");
    }

    recycleSingleProductBalance(element: HTMLElement): void {
        let self = this;
        const $element = $(element);
        const url: string = $element.data("url");
        const userId: string = $element.data("userid");
        const productCode: string = $element.data("productcode");
        const productName: string = $element.data("productname");

        this.doRecycleBalance(url, userId, productCode, productName);
    }

    private doRecycleBalance(url: string, userId: string, productCode: string, confirmProductName: string): void {
        let self = this;

        window.confirm(
            `确定协助用户回收${confirmProductName}帐户余额吗？`,
            () => {
                const isAutoHideLoading = true;
                const requestId = this.generateGUID();

                //初始化requestId
                $.each($(".jqProductRow"), function (index, row) {
                    let rowProductCode: string = $(row).attr("productCode");

                    if (productCode === "" || productCode === rowProductCode) {
                        self.productRequestMap[rowProductCode] = requestId;
                        self.setOperationResultHtml(rowProductCode, "");
                    }
                });

                var param = {
                    userId,
                    productCode,
                    routingKey: self.routingKey,
                    requestId
                };

                $.ajax2({
                    url: url,
                    type: "POST",
                    data: param,
                    success: response => {
                        new baseReturnModelService(response).responseHandler(() => {
                        }, isAutoHideLoading);
                    },
                });
            }
        );
    }

    UpdateOperationContent(transferMessage: ITransferMessage): void {
        let productRequestId = this.productRequestMap[transferMessage.ProductCode];

        //若畫面上的Guid與MQ內容一致，則找到對應畫面欄位寫入資料
        if (transferMessage.RequestId !== productRequestId) {
            return;
        }

        this.appendOperationResultHtml(transferMessage.ProductCode, transferMessage.Summary);
    }

    UpdateMiseLiveBalance(): void {
        const url = globalVariables.GetUrl("RecycleBalance/GetMiseLiveBalance");
        const userId: string = $(this.recycleBalanceSearchContent.userIdSelector).text();

        $.ajax({
            url: url,
            type: "GET",
            data: { userId },
            success: response => {
                $(".jqMiseLiveGameBalance").text(response);
            },
        });
    }

    UpdateTPGameBalance(productCode: string): void {
        const url = globalVariables.GetUrl("RecycleBalance/GetTPGameBalance");
        const userId: string = $(this.recycleBalanceSearchContent.userIdSelector).text();

        $.ajax({
            url: url,
            type: "GET",
            data: { userId, productCode },
            success: response => {
                $(`.jqProductRow[productCode='${productCode}'] .jqTPGameBalance`).text(response);
            },
        });
    }

    private generateGUID(): string {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, (c) => {
            const r = (Math.random() * 16) | 0;
            const v = c === 'x' ? r : (r & 0x3) | 0x8;

            return v.toString(16);
        });
    }

    private appendOperationResultHtml(productCode: string, content: string): void {
        let $jqOperationResult = this.getJqOperationResult(productCode);
        let operationHtml = $jqOperationResult.html();

        if (operationHtml !== "") {
            operationHtml += "<br/>";
        }

        operationHtml += content;

        $jqOperationResult.html(operationHtml);
    }

    private setOperationResultHtml(productCode: string, content: string): void {
        let $jqOperationResult = this.getJqOperationResult(productCode);

        $jqOperationResult.html(content);
    }

    private getJqOperationResult(productCode: string): JQuery<HTMLElement> {
        let $jqOperationResult = this.recycleBalanceSearchContent.$contentBody
            .find(`tr[productcode='${productCode}']`)
            .find(".jqOperationResult");

        return $jqOperationResult;
    }
}

interface ITransferMessage {
    ProductCode: string;
    RequestId: string;
    Summary: string;
    IsReloadMiseLiveBalance: boolean;
}

class TransferMessage implements ITransferMessage {
    ProductCode: string;
    RequestId: string;
    Summary: string;
    IsReloadMiseLiveBalance: boolean;
}

class recycleBalanceHtmlGridSearchContent extends htmlGridSearchContent implements IHtmlGridSearchContent {
    $msUserContentBody: JQuery<HTMLElement>;
    $msUserGridContentRoot: JQuery<HTMLElement>;
    userIdSelector: string;
}