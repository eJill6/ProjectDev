class changePasswordEditSingleRowService extends editSingleRowService {
    protected handleEditResponse(response: IBaseReturnModel, isAutoHideLoading: boolean) {
        new baseReturnModelService(response).responseHandler(() => {
            const logoutDelayTime: number = 1000;
            setTimeout(() => location.href = globalVariables.GetUrl("Authority/Logout"), logoutDelayTime);
        }, isAutoHideLoading);
    }
}