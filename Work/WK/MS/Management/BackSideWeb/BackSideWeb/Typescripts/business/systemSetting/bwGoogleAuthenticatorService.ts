class bwGoogleAuthenticatorService extends editSingleRowService {
    constructor(param: editSingleRowParam) {
        super(param);
    }

    protected override handleEditResponse(response) {
        const isAutoHideLoading: boolean = false;
        const isShowSuccessMessage: boolean = false;
        new baseReturnModelService(response).responseHandler(() => {
            $('#jqGoogleQRCodeImage').attr('src', response.dataModel.imageUrl);
            $('#jqUpdateDateText').text(response.dataModel.updateDateText);
        }, isAutoHideLoading, isShowSuccessMessage);
    }
}