var imageFileUploadService = (function () {
    function imageFileUploadService(rules) {
        this.$jqimageFileInput = $('#jqImageFile');
        this.$jqFullImageUrl = $('#jqFullImageUrl');
        this.initValidator();
        this.initJqHiddenGameImage();
        this.initImageFileInput(rules);
    }
    Object.defineProperty(imageFileUploadService.prototype, "$jqHiddenGameImage", {
        get: function () {
            return $('#jqHiddenGameImage');
        },
        enumerable: false,
        configurable: true
    });
    imageFileUploadService.prototype.initValidator = function () {
        var _this = this;
        $.validator.addMethod('imageDimensions', function (value, element, params) {
            return _this.validateChangedImage(function () {
                var image = _this.$jqHiddenGameImage;
                for (var _i = 0, params_1 = params; _i < params_1.length; _i++) {
                    var param = params_1[_i];
                    if (param.width / param.height === 1) {
                        if (image.width() <= param.width && image.height() <= param.height &&
                            ((image.width() / image.height()) === (param.width / param.height))) {
                            return true;
                        }
                    }
                    else {
                        if (image.width() <= param.width && image.height() <= param.height) {
                            return true;
                        }
                    }
                }
                return false;
            });
        });
        $.validator.addMethod('imageSize', function (value, element, params) {
            return _this.validateChangedImage(function () { return _this.fileSize <= params.size; });
        });
        $.validator.addMethod('imageExtensions', function (value, element, params) {
            return _this.validateChangedImage(function () {
                if (!value) {
                    return true;
                }
                var fileName = value;
                var allowedExtensions = params;
                var fileExtension = fileName.substr(fileName.lastIndexOf('.') + 1);
                return allowedExtensions.includes(fileExtension);
            });
        });
        $.validator.addMethod('imageRequired', function (value, element, params) {
            return _this.$jqFullImageUrl.val();
        });
    };
    imageFileUploadService.prototype.validateChangedImage = function (validate) {
        if (!this.isFileChanged) {
            return true;
        }
        return validate();
    };
    imageFileUploadService.prototype.initImageFileInput = function (rules) {
        var _this = this;
        this.$jqimageFileInput.change(function (event) {
            var reader = new FileReader();
            reader.onload = function (file) {
                _this.$jqHiddenGameImage.attr('src', file.target.result);
                _this.$jqHiddenGameImage.trigger('load');
            };
            var input = _this.$jqimageFileInput[0];
            if (input.files.length == 0) {
                _this.$jqFullImageUrl.val('');
                return;
            }
            var file = input.files[0];
            reader.readAsDataURL(file);
            _this.fileSize = file.size;
            _this.$jqFullImageUrl.val(file.name);
            _this.isFileChanged = true;
        });
        this.$jqimageFileInput.rules("add", rules);
    };
    imageFileUploadService.prototype.initJqHiddenGameImage = function () {
        var _this = this;
        this.$jqHiddenGameImage.on('load', function () {
            _this.hoverImage();
            _this.$jqimageFileInput.valid();
        });
        this.$jqHiddenGameImage.trigger('load');
    };
    imageFileUploadService.prototype.hoverImage = function () {
        if (this.$jqHiddenGameImage.attr('src')) {
            this.$jqFullImageUrl.hoverTips({
                position: 'top',
                content: $(this.$jqHiddenGameImage)
            });
        }
    };
    return imageFileUploadService;
}());
