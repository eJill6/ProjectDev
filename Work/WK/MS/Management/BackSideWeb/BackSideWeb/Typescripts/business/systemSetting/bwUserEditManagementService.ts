class bwUserEditManagementService extends editSingleRowService {
    constructor(param: editSingleRowParam) {
        super(param);
    }

    protected override handleEditResponse(response, isAutoHideLoading) {
        if (response.dataModel && response.dataModel.url) {
            let bwUserManagementService = parent["bwUserManagementService"];
            bwUserManagementService.search();
            let parentLayer = parent["layer"];
            let layerIndex = parentLayer.index;
            let area = {
                width: 420,
                height: 360
            } as layerArea;

            let left: number = ($(window.parent).width() / 2) - (area.width / 2);
            let top: number = ($(window.parent).height() / 2) - (area.height / 2);

            let position = {
                left: left,
                top: top
            };

            parentLayer.style(layerIndex, $.extend(area, position));

            location.href = response.dataModel.url;
        }
        else {
            super.handleEditResponse(response, isAutoHideLoading);
        }
    }
}