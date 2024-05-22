class platformAESService {
    private static readonly s_key: string = "94a4b778g01ca4ab";

    fetchAllAESImage() {
        const self = this;
        const aesImages = document.querySelectorAll('img[aes-src]');

        aesImages.forEach(async (element) => {
            // 取得 aes-src 屬性的值
            let url = element.getAttribute("aes-src");
            let imageBase64String = await self.fetchDecryptImageBase64String(url);
            element.setAttribute("src", imageBase64String);
        });
    }

    //單一下載檔案
    async fetchDecryptImageBase64String(imageUrl: string): Promise<string> {
        const result = (await this.getDecryptImageBase64String(imageUrl)) as string;

        return result;
    }

    private getDecryptImageBase64String(imageUrl: string) {
        return new Promise(async (resolve, reject) => {
            const response = await fetch(imageUrl);
            const imageBlob = await response.blob();
            const reader = new FileReader();

            reader.onload = async (event) => {
                try {
                    const target = event.target as FileReader;
                    const aesBase64String = target.result as string;
                    const base64String = this.decryptBase64String(aesBase64String);
                    const imageBase64String = `data:image/jpeg;base64,${base64String}`;

                    // Resolve the promise with the response value
                    resolve(imageBase64String);
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

    //解密成一般檔案
    private decryptBase64String(base64String: string): string {
        // split the sha256 hash byte array into key and iv
        let keyPart = CryptoJS.enc.Utf8.parse(platformAESService.s_key);

        const parts = base64String.split(";base64,");
        const arrayData = parts[1];

        const decrypted = CryptoJS.AES.decrypt(arrayData, keyPart, {
            mode: CryptoJS.mode.ECB,
            padding: CryptoJS.pad.NoPadding
        });

        return decrypted.toString(CryptoJS.enc.Base64);
    }

    isAESFileExtension(path: string): boolean {
        if (path === undefined || path === null) {
            return false;
        }

        path = path.toLowerCase();

        return path.endsWith(".aes");
    }
}