var formUtilService = (function () {
    function formUtilService() {
    }
    formUtilService.prototype.serializeObject = function ($form) {
        var serializeString = $form.serialize();
        var formArray = serializeString.split("&");
        var formObject = {};
        for (var i = 0; i < formArray.length; i++) {
            if (formArray[i] == "") {
                continue;
            }
            var _a = formArray[i].split("="), key = _a[0], value = _a[1];
            var decodedKey = decodeURIComponent(key);
            var decodedValue = decodeURIComponent(value);
            formObject[decodedKey] = decodedValue;
        }
        return formObject;
    };
    formUtilService.prototype.objectToFormData = function (obj) {
        var formData = new FormData();
        for (var key in obj) {
            if (obj.hasOwnProperty(key)) {
                formData.append(key, obj[key]);
            }
        }
        return formData;
    };
    return formUtilService;
}());
