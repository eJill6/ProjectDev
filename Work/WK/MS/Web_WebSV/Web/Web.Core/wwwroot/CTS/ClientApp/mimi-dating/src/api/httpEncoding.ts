import axios from "axios";
import global from "@/global";
import XORTool from "@/XORTool";

const XOR = XORTool.XOR;
const headers = {
  headers: 
  { 
      'X-Requested-With': 'XMLHttpRequest',
      'Content-Type': 'application/json;charset=UTF-8',
      'enc-bytes': 'true'
  }
};
const responseXORDecrypt = (responseData: any) => {
  return JSON.parse(XOR.decrypt(responseData, XOR.Key));
}

const responseDecode = (responseData: any) => {
  return JSON.parse(decodeURIComponent(escape(atob(responseData))));
}

const requestEncode = (requestData: any) => {

  if (requestData){
      requestData = btoa(encodeURIComponent(JSON.stringify(requestData)));
  }

  return requestData;
}

const requestXOREncrypt = (requestData: any) => {

  if (requestData){
    return XOR.encrypt(encodeURIComponent(JSON.stringify(requestData)), XOR.Key);
  }

  return requestData;
}

const apiInstance = axios.create(headers);
apiInstance.defaults.baseURL = global.toTokenPath("/MM");

apiInstance.interceptors.response.use(
  (response) => {
    const result = responseXORDecrypt(response.data);
    if (!result && response.status === 200) {
      return result.data;
    }

    if (!result.isSuccess || result.errorCode || result.errorMessage)
      throw new Error(`${result.errorMessage ? result.errorMessage : ""}`);

    return result.data;
  },
  (error) => {
    const errorCode = error.response.data.code;
    const errorMessage = error.response.data.errorMessage;    
    throw new Error(`${errorMessage ? errorMessage : ""}`);
  }
);

const paramsToQueryString = (params: any): string => {
  const queryString = Object.keys(params)
      .map(key => encodeURIComponent(key) + '=' + encodeURIComponent(params[key]))
      .join('&');
  return queryString;
};

const apiGetAsync = <TResult>(url: string, params?: any): Promise<TResult> => {
    const updatedParams = {
        ...params,
        _: new Date().getTime()
    };
    
    const path = `${apiInstance.defaults.baseURL}/${url}?${paramsToQueryString(updatedParams)}`;

    return apiInstance.request({
        baseURL:'/',
        method: 'GET',
        url:'/',
        headers: { 
            'enc-bytes': true,
            'enc-path':XOR.encrypt(path, XOR.Key) 
        }
    });
  };
const apiPostAsync = <TResult>(url: string, data?: any): Promise<TResult> => {
    data = requestXOREncrypt(data);
    const path = `${apiInstance.defaults.baseURL}/${url}`.replace('//','/');
    const encrypt = XOR.encrypt(path, XOR.Key);
    return apiInstance.post('/', data,{
        baseURL: '/',
        headers: { 
            'enc-bytes': true,
            'enc-path':encrypt
        }
    });
}

// For Chat
const chatInstance = axios.create(headers);

chatInstance.defaults.baseURL = global.toTokenPath('/Chat');

chatInstance.interceptors.response.use(
  (response) => {
    const result = responseXORDecrypt(response.data);
    if (!result && response.status === 200) {
      return result.dataModel;
    }
    
    if (!result.isSuccess)
      throw new Error(`${result.code}`);
    
    return result.dataModel;
  },
  (error) => {
    const errorCode = error.response.data.code;
    const errorMessage = error.response.data.message;    
    throw new Error(`${errorMessage ? errorMessage : ""}`);
  }
);

const chatApiGetAsync = <TResult>(url: string, params?: any): Promise<TResult> => {
  const updatedParams = {
      ...params,
      _: new Date().getTime()
  };

  const path = `${chatInstance.defaults.baseURL}/${url}?${paramsToQueryString(updatedParams)}`;

  return chatInstance.request({
      baseURL:'/',
      method: 'GET',
      url:'/',
      headers: { 
          'enc-bytes': true,
          'enc-path':XOR.encrypt(path, XOR.Key) 
      }
  });
}
const chatApiPostAsync = <TResult>(url: string, data?: any): Promise<TResult> => {

  data = requestXOREncrypt(data);
    const path = `${chatInstance.defaults.baseURL}/${url}`.replace('//','/');
    const encrypt = XOR.encrypt(path, XOR.Key);
    return chatInstance.post('/', data,{
        baseURL: '/',
        headers: { 
            'enc-bytes': true,
            'enc-path':encrypt
        }
    });
}

export default { apiGetAsync, apiPostAsync, chatApiGetAsync, chatApiPostAsync };
