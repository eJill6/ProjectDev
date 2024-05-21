import axios from 'axios';
import message from '@/message';
const headers = {
    headers: { 'X-Requested-With': 'XMLHttpRequest' }
};

// For Platform
const platformInstance = axios.create(headers);
platformInstance.defaults.baseURL = message.toTokenPath('/');
platformInstance.interceptors.response.use(response => response.data);

const playformPostAsync = <TResult>(url: string, data?: any): Promise<TResult> => platformInstance.post(url, data);

// For LotterySpa
const lotterySpaInstance = axios.create(headers);

lotterySpaInstance.defaults.baseURL = message.toTokenPath('/LotterySpa');

lotterySpaInstance.interceptors.response.use(response => {
    const result = response.data;    
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

const lotterySpaGetAsync = <TResult>(url: string, params?: any): Promise<TResult> => lotterySpaInstance.request({
    method: 'GET',
    url,
    params: { ...params, _: new Date().getTime() }
});

const lotterySpaPostAsync = <TResult>(url: string, data?: any): Promise<TResult> => lotterySpaInstance.post(url, data);


export default { lotterySpaGetAsync, lotterySpaPostAsync, playformPostAsync };
