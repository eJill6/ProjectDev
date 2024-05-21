class imageFileUploadService {
    private isFileChanged: boolean;
    private fileSize: number;
    private $jqimageFileInput: JQuery<HTMLElement> = $('#jqImageFile');
    private $jqFullImageUrl: JQuery<HTMLElement> = $('#jqFullImageUrl');

    private get $jqHiddenGameImage(): JQuery<HTMLElement> {
        return $('#jqHiddenGameImage');
    }

    constructor(rules: any) {
        this.initValidator();
        this.initJqHiddenGameImage();
        this.initImageFileInput(rules);
    }

    private initValidator() {
        $.validator.addMethod('imageDimensions', (value, element, params) => {
            return this.validateChangedImage(() => {
                const image: JQuery<HTMLElement> = this.$jqHiddenGameImage;

                for (const param of params) {
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

        $.validator.addMethod('imageSize', (value, element, params) => {
            return this.validateChangedImage(() => this.fileSize <= params.size);
        });

        $.validator.addMethod('imageExtensions', (value, element, params) => {
            return this.validateChangedImage(() => {
                if (!value) {
                    return true;
                }

                let fileName: string = value;
                const allowedExtensions: string[] = params;
                const fileExtension: string = fileName.substr(fileName.lastIndexOf('.') + 1);

                return allowedExtensions.includes(fileExtension);
            })
        });

        $.validator.addMethod('imageRequired', (value, element, params) => {
            return this.$jqFullImageUrl.val();
        });
    }

    private validateChangedImage(validate: () => boolean): boolean {
        if (!this.isFileChanged) {
            return true; //没改過图不检查
        }

        return validate();
    }

    private initImageFileInput(rules: any) {
        this.$jqimageFileInput.change((event) => {
            const reader = new FileReader();

            reader.onload = (file) => {
                this.$jqHiddenGameImage.attr('src', file.target.result as string);
                this.$jqHiddenGameImage.trigger('load');
            };

            const input: HTMLInputElement = this.$jqimageFileInput[0] as HTMLInputElement;

            if (input.files.length == 0) {
                this.$jqFullImageUrl.val('');

                return;
            }

            const file = input.files[0];
            reader.readAsDataURL(file);

            this.fileSize = file.size;
            this.$jqFullImageUrl.val(file.name);
            this.isFileChanged = true;
        });

        this.$jqimageFileInput.rules("add", rules);
    }

    private initJqHiddenGameImage() {
        this.$jqHiddenGameImage.on('load', () => {
            this.hoverImage();
            this.$jqimageFileInput.valid();
        });

        this.$jqHiddenGameImage.trigger('load');
    }

    private hoverImage() {
        if (this.$jqHiddenGameImage.attr('src')) {
            this.$jqFullImageUrl.hoverTips({
                position: 'top',
                content: $(this.$jqHiddenGameImage)
            });
        }
    }
}