class bwUserManagementService extends baseCRUDService {
    constructor(pageApiUrlSetting: IPageApiUrlSetting) {
        super(pageApiUrlSetting);
    }

    openGoogleQRCode(link: HTMLElement, keyContent: string) {
        const url: string = $(link).data('url');
        const area = {
            width: 420,
        } as layerArea;

        this.openView({
            url: url,
            keyContent: keyContent,
            area: area
        });
    }
}