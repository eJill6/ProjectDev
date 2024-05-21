class imgConvertToAesCRUDService extends baseCRUDService implements baseSearchGridService {
    override doAfterSearch(htmlContents: IHtmlSearchContent, self: any): void {
        super.doAfterSearch(htmlContents, self);

        var aesService = new decryptoService();
        aesService.fetchAllAESImage();
    }
}