import axios from "axios";
import global from "@/global";

const headers = {
  headers: { "X-Requested-With": "XMLHttpRequest" },
};

const apiInstance = axios.create(headers);
apiInstance.defaults.baseURL = global.toTokenPath("/MM");

apiInstance.interceptors.response.use(
  (response) => {
    const result = response.data;
    if (!result && response.status === 200) {
      return result.data;
    }

    if (!result.isSuccess || result.errorCode || result.errorMessage)
      throw new Error(`${result.code} ${result.errorMessage ? result.errorMessage : ""}`);

    return result.data;
  },
  (error) => {
    const errorCode = error.response.data.code;
    const errorMessage = error.response.data.errorMessage;    
    throw new Error(`${errorCode} ${errorMessage ? errorMessage : ""}`);
  }
);

const apiGetAsync = <TResult>(url: string, params?: any): Promise<TResult> =>
  apiInstance.request({
    method: "GET",
    url,
    params: { ...params, _: new Date().getTime() },
  });

const apiPostAsync = <TResult>(url: string, data?: any): Promise<TResult> =>
  apiInstance.post(url, data);

export default { apiGetAsync, apiPostAsync };
