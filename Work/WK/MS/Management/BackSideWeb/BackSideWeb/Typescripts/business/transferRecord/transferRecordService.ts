class transferRecordService extends baseSearchGridService {
    constructor(searchApiUrlSetting: ISearchApiUrlSetting) {
        super(searchApiUrlSetting);

        const minStartDate = new Date();
        minStartDate.setMonth(minStartDate.getMonth() - 3);

        const isAllowEmpty: boolean = true;
        this.initStartAndEndDatePicker($('#StartDate'), $('#EndDate'), isAllowEmpty, minStartDate);
    }

    protected override validateSubmitData() {
        let data = this.submitData as transferRecordSubmitData;

        if (data.UserID.trim() == '' && data.ProductCode.trim() == '') {
            alert('请输入用户ID或指定产品查询');

            return false;
        }

        return super.validateSubmitData();
    }
}

class transferRecordSubmitData extends PagingRequestParam implements ISearchGridParam {
    UserID: string;
    ProductCode: string;
    StartDate: string;
    EndDate: string;
    OrderStatus: string;
    TransferType: string;
}