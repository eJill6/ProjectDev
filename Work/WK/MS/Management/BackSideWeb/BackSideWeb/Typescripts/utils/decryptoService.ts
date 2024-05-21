class decryptoService {
    private readonly decryptoKey: string = "94a4b778g01ca4ab";

    //單一下載檔案
    async fetchSingleDownload(imageUrl: string) {
        const result = (await this.getBase64Image(imageUrl)) as string;

        return result;
    }

    fetchAllAESImage() {
        const self = this;
        const aesImages = document.querySelectorAll('img[aes-src]');

        aesImages.forEach(async (element) => {
            // 取得 aes-src 屬性的值
            let url = element.getAttribute("aes-src");
            let imageBase64String = await self.fetchSingleDownload(url);
            element.setAttribute("src", imageBase64String);
        });
    }

    getBase64Image(imageUrl: string) {
        return new Promise(async (resolve, reject) => {
            const response = await fetch(imageUrl);
            const imageBlob = await response.blob();

            const hasDecrypto = this.doNeedDecrypto(imageUrl);
            const reader = new FileReader();

            reader.onload = async (event) => {
                try {
                    const target = event.target as FileReader;
                    const base64String = target.result as string;
                    const result = hasDecrypto
                        ? this.decryptoFile(base64String)
                        : base64String;
                    // Resolve the promise with the response value
                    resolve(result);
                } catch (err) {
                    reject(err);
                }
            };

            reader.onerror = (error) => {
                reject(error);
            };
            reader.readAsDataURL(imageBlob);
        });
    }

    ///解密成一般檔案
    decryptoFile(base64String: string) {
        // split the sha256 hash byte array into key and iv
        let keyPart = CryptoJS.enc.Utf8.parse(this.decryptoKey);

        const parts = base64String.split(";base64,");
        const arrayData = parts[1];

        const decrypted = CryptoJS.AES.decrypt(arrayData, keyPart, {
            mode: CryptoJS.mode.ECB,
            padding: CryptoJS.pad.NoPadding
        });

        const resultImage = `data:image/jpeg;base64,${decrypted.toString(
            CryptoJS.enc.Base64
        )}`;

        return resultImage;
    }

    doNeedDecrypto(urlString: string): boolean {
        const parts = urlString.split(".");

        if (parts.length < 2) {
            return false;
        }

        const index = parts.indexOf("aes");

        return index > 0;
    }
}