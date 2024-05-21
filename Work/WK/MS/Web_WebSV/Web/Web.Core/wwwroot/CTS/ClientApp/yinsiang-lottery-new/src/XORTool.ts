const w = window as any;

const XOR =
{
    Key:w.xorKey,
    encrypt: function(input: any, key: any):string{
        const inputBytes = this.stringToUtf8Bytes(input);
        const keyBytes = this.stringToUtf8Bytes(key);
        const encryptedBytes = new Uint8Array(inputBytes.length);
        
        for (let i = 0; i < inputBytes.length; i++) {
            encryptedBytes[i] = inputBytes[i] ^ keyBytes[i % keyBytes.length];
        }
        
        return this.bytesToBase64(encryptedBytes);
    },

    decrypt: function(input: any, key: any):string{
        key = typeof key === 'object' ? JSON.stringify(key) : key.toString();
        var decodedText = atob(input);
        var plainText = '';
        var length = decodedText.length;
    
        var textArray = [];
        for(var i=0; i < length; i++)
        {
            textArray.push(decodedText.charCodeAt(i) ^ key.charCodeAt(Math.floor(i % key.length)));      
        }    
    
        var decoder = new TextDecoder("utf-8");
        plainText= decoder.decode(new Uint8Array(textArray));
        return plainText;
    },

    stringToUtf8Bytes: function(str:string) {
        const encoder = new TextEncoder();
        return encoder.encode(str);
    },
    
    // 將 UTF-8 字節表示形式轉換為 Base64 字符串
    bytesToBase64: function(bytes:Uint8Array) {
        const binaryString = bytes.reduce((str, byte) => str + String.fromCharCode(byte), '');
        return btoa(binaryString);
    }
};


export default {
    XOR
}