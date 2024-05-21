interface IBaseReturnModel {
    isSuccess: boolean;
    message: string;
}

class baseReturnModelService {
    private baseReturnModel: IBaseReturnModel;

    constructor(baseReturnModel: IBaseReturnModel) {
        this.baseReturnModel = baseReturnModel;
    }

    responseHandler(callback: () => void, isAutoHideLoading: boolean, isShowSuccessMessage: boolean = true) {
        const response: IBaseReturnModel = this.baseReturnModel;

        if (!response.isSuccess) {
            if (!isAutoHideLoading) {
                //如果沒有照流程自動關Loading, 這邊手動關
                $.fn.loading("hide");
            }

            alert(response.message);

            return;
        }

        const showSuccessMessage: Function = () => {
            if (!isAutoHideLoading) {
                //如果沒有照流程自動關Loading, 這邊手動關
                $.fn.loading("hide");
            }

            let successFunc = callback;

            if (isShowSuccessMessage) {
                successFunc = () => {
                    let executingWin: Window = window;

                    if (window.self !== window.top) {
                        executingWin = window.parent;
                    }

                    executingWin.$.toast("提交成功！");

                    if (callback) {
                        callback();
                    }
                };
            }

            successFunc();
        };

        const dbSyncTime: number = 800;
        setTimeout(showSuccessMessage, dbSyncTime);
    };
}