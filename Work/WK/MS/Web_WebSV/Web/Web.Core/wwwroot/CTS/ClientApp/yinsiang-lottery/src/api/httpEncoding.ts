import axios from 'axios';
import message from '@/message';
import XORTool from '@/XORTool';
const headers = {
    headers: 
    { 
        'X-Requested-With': 'XMLHttpRequest',
        'Content-Type': 'application/json;charset=UTF-8',
        'enc-bytes': 'true'
    }
};
const XOR = XORTool.XOR;
const responseXORDecrypt = (responseData: any) => {
    return JSON.parse(XOR.decrypt(responseData, XOR.Key));
}
const responseDecode = (responseData: any) => {
    return JSON.parse(atob(responseData));
}
const requestXOREncrypt = (requestData: any) => {

    if (requestData){
        return XOR.encrypt(encodeURIComponent(JSON.stringify(requestData)), XOR.Key);
    }

    return requestData;
}
const requestEncode = (requestData: any) => {

    if (requestData){
        requestData = btoa(encodeURIComponent(JSON.stringify(requestData)));
    }

    return requestData;
}

// For Platform
const platformInstance = axios.create(headers);
platformInstance.defaults.baseURL = message.toTokenPath('/');
platformInstance.interceptors.response.use(response => responseXORDecrypt(response.data));

const playformPostAsync = <TResult>(url: string, data?: any): Promise<TResult> => {

    data = requestXOREncrypt(data);
    const path = `${platformInstance.defaults.baseURL}${url}`.replace('//','/');
    return platformInstance.post('', data,{
        baseURL: '/',
        headers: { 
            'enc-bytes': true,
            'enc-path':XOR.encrypt(path, XOR.Key) 
        }
    });
};

// For LotterySpa
const lotterySpaInstance = axios.create(headers);

lotterySpaInstance.defaults.baseURL = message.toTokenPath('/LotterySpa');


lotterySpaInstance.interceptors.response.use(response => {
    var result:any;
    if(response.headers['enc-bytes']){
        result = responseXORDecrypt(response.data);
    }
    else{
        result = responseDecode(response.data);   
    }

    if(!result && response.status === 200) {
        return result.data;
    }    
    if (!result.isSuccess || result.errorCode || result.errorMessage)
        throw new Error(result.errorMessage);

    return result.data;
}, error => {
    const errorMessage = error.response.data.errorMessage;

    throw new Error(errorMessage);
});

const paramsToQueryString = (params: any): string => {
    const queryString = Object.keys(params)
        .map(key => encodeURIComponent(key) + '=' + encodeURIComponent(params[key]))
        .join('&');
    return queryString;
};

const lotterySpaGetAsync = <TResult>(url: string, params?: any): Promise<TResult> => {
    const updatedParams = {
        ...params,
        _: new Date().getTime()
    };
    
    const path = `${lotterySpaInstance.defaults.baseURL}/${url}?${paramsToQueryString(updatedParams)}`;

    return lotterySpaInstance.request({
        method: 'GET',
        url:'',
        baseURL: '/',
        headers: { 
            'enc-bytes': true,
            'enc-path':XOR.encrypt(path, XOR.Key) 
        }
    });
}
const lotterySpaPostAsync = <TResult>(url: string, data?: any): Promise<TResult> => {

    data = requestXOREncrypt(data);
    const path = `${lotterySpaInstance.defaults.baseURL}/${url}`;
    
    return lotterySpaInstance.post('', data,{
        baseURL: '/',
        headers: { 
            'enc-bytes': true,
            'enc-path':XOR.encrypt(path, XOR.Key) 
        }
    });
};

export default { lotterySpaGetAsync, lotterySpaPostAsync, playformPostAsync };
