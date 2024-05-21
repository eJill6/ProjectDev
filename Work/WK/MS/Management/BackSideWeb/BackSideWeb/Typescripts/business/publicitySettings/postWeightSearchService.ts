class postWeightSearchService extends baseCRUDService {
    protected $tabSelectorItems: JQuery<HTMLElement> = $('.pageTab ul a');
    protected override defaultPageSize = 1000;

    protected getInsertViewArea(): layerArea {
        return {
            width: 600,
            height: 400,
        } as layerArea;
    }
    protected getUpdateViewArea(): layerArea {
        return {
            width: 600,
            height: 400,
        } as layerArea;
    }

    constructor(pageApiUrlSetting: IPageApiUrlSetting) {
        super(pageApiUrlSetting);
        $(document).ready(() => {
            this.registerClickEvent();
            this.$tabSelectorItems.first().click();
        });
    }
    protected registerClickEvent(): void {
        this.$tabSelectorItems.click((event) => {
            event.preventDefault();

            this.$tabSelectorItems.removeClass("active");
            $(event.currentTarget).addClass("active");
            $("#jqMemberFilter").show();
            $("#jqContentRoot").show();
            $(".btn_insert ").show();
            $("#jqGoldStoreFilter").hide();
            $("#jqGoldStoreContentRoot").hide();
            const tabId = $(event.currentTarget).attr("id");

            $("#postTypeTemp").text(tabId);
            if (tabId !== "3" && tabId !== "4") {
                const $jqSearchBtn = $(".jqSearchBtn");
                $jqSearchBtn.click();
            }

            if (tabId === "3" || tabId === "4") {
                $(".btn_insert ").hide();
                $("#jqMemberFilter").hide();
                $("#jqContentRoot").hide();
                $("#jqGoldStoreFilter").show();
                $("#jqGoldStoreContentRoot").show();

                switch (tabId) {
                    case "3":
                        $(".EditGoldStore ").hide();
                        $(".EditGoldStoreRecommend ").show();
                        $(".GoldStore ").text("推荐 3");
                        this.GetGoldStoreData(1);
                        break;
                    case "4":
                        $(".EditGoldStore ").show();
                        $(".EditGoldStoreRecommend ").hide();
                        $(".GoldStore ").text("TOP 3");
                        this.GetGoldStoreData(2);
                        break;
                    default:
                }
            }
        });
    }

    protected getSubmitData(): ISearchGridParam {
        const data = new postWeightSearchSearchParam();
        data.postType = $('#postTypeTemp').text();
        return data;
    }
    batchReview(type) {
        let selectedValues = this.getSelectedValues();

        if (selectedValues.length === 0) {
            alert("未选择帖子，请再确认");

            return;
        }

        if (selectedValues.length > 50) {
            alert("批量审核最大同时50笔,请再确认");

            return;
        }

        const isAutoHideLoading: boolean = false;

        $.ajax2({
            url: "/PostWeight/BatchReview",
            type: "POST",
            data: {
                postIds: selectedValues,
                type: type
            },
            isAutoHideLoading,
            success: function (response) {
                const isShowSuccessMessage: boolean = false;

                new baseReturnModelService(response).responseHandler(
                    () => {
                        $.fn.loading("hide");
                        $.toast("完成批量移除");
                        $("#selectAllPostId").prop("checked", false);
                        new pagerService(window.document).reloadCurrentPage();
                    },
                    isAutoHideLoading,
                    isShowSuccessMessage);
            }
        })
    }

    GetGoldStoreData(type) {
        $.ajax2({
            url: "/PostWeight/GetGoldStoreData",
            type: "GET",
            data: {
                type: type
            },
            success: function (response) {
                if (response.isSuccess === true) {
                    // 选择目标元素
                    var $goldStoreGridContent = $("#jqGoldStoreGridContent");
                    $goldStoreGridContent.empty();
                    // 遍历JSON数据并创建表格行
                    $.each(response.datas, function (index, item) {
                        var $tableRow = $("<tr>");
                        $tableRow.append($("<td>").text(item.top));
                        $tableRow.append($("<td>").text(item.userId));

                        // 将行添加到表格
                        $goldStoreGridContent.append($tableRow);
                    });

                    // 显示包含数据的元素
                    $("#jqGoldStoreContentRoot").show();
                }
            }
        })
    }
    openGoldStoreEdit(link: HTMLElement) {
        const url: string = $(link).data('url');
        const area = {
            width: 400,
            height: 360,
        } as layerArea;

        this.openView({
            url: url,
            area: area,
            title: '编辑'
        });
    }
    openGoldStoreRecommendEdit(link: HTMLElement) {
        const url: string = $(link).data('url');
        const area = {
            width: 400,
            height: 360,
        } as layerArea;

        this.openView({
            url: url,
            area: area,
            title: '编辑'
        });
    }
    private getSelectedValues(): string[] {
        // 获取所有选中的复选框的值
        let selectedValues: string[] = [];
        let checkboxes = document.querySelectorAll('input[name="selectPostId"]:checked');

        checkboxes.forEach(function (checkbox: HTMLInputElement) {
            selectedValues.push(checkbox.value);
        });

        return selectedValues;
    }
}

class postWeightSearchSearchParam extends PagingRequestParam implements ISearchGridParam {
    postType: any;
}