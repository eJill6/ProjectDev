class fileDownloadService {
    download(data: Blob, filename: string): void {
        if (typeof (window.navigator as any).msSaveBlob !== 'undefined') {
            (window.navigator as any).msSaveBlob(data, filename);
        }
        else {
            var blobURL = (window.URL && window.URL.createObjectURL) ? window.URL.createObjectURL(data) : window.webkitURL.createObjectURL(data);
            var tempLink = document.createElement('a');
            tempLink.style.display = 'none';
            tempLink.href = blobURL;
            tempLink.setAttribute('download', filename);

            if (typeof tempLink.download === 'undefined') {
                tempLink.setAttribute('target', '_blank');
            }

            document.body.appendChild(tempLink);
            tempLink.click();

            setTimeout(function () {
                document.body.removeChild(tempLink);
                window.URL.revokeObjectURL(blobURL);
            }, 200)
        }
    }
}