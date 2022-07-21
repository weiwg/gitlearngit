/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-10-27 14:17:27
 * @LastEditors: weig
 * @LastEditTime: 2021-12-17 11:38:53
 */
import axios from 'axios'
import EnumConfig from '../enum/EnumConfig'

axios.defaults.headers['Content-Type'] = 'application/json;charset=utf-8';
axios.defaults.headers['Cache-Control'] = 'no-cache';
axios.defaults.headers['Pragma'] = 'no-cache';
axios.defaults.headers['Pragma'] = 'no-cache';
// axios.defaults.headers["Access-Control-Allow-Origin"]="*";

const requestAxios = axios.create({
  // baseURL: 'http://localhost:9000', //开发
  baseURL: '',//process.env.VUE_APP_BASE_API, //发布测试
  timeout: 2000000 
});

// 拦截请求
requestAxios.interceptors.request.use(
  config => {
    const token = requestAxios.token ? requestAxios.token : "";
    if (token) {
      config.headers.Authorization = 'Bearer ' + token;
      config.url = config.url.replace(/^\/api/i, `/Api`); 
    } else {
      config.url = config.url.replace(/^\/api/i, `/Api`); 
    }
    return config
  },
  err => {
    return Promise.reject(err);
  }
);

//请求响应
requestAxios.interceptors.response.use(res => {
  return Promise.resolve(res);
},
 async error => {
    return Promise.reject(error);
});

  /**
   * @description 公共请求方法
   * @author weig
   * @param {string} requestWay 请求方式
   * @param {string} url 请求地址
   * @param {json} param  请求参数
   * @returns {Promise} Promise对象
   */
  requestAxios.AxiosRequest = (requestWay = "post", url, param)=>{
    if (param){
      return new Promise((resolve, reject) =>{
        //   resolve(requestAxios[`${requestWay}`](url, param));
          requestAxios[`${requestWay}`](url, param).then(res =>{
            resolve(res);
          }).catch(err =>{
            reject(err);
          });
      });
    } else {
      return new Promise((resolve, reject) =>{
        // resolve(requestAxios[`${requestWay}`](url));
        requestAxios[`${requestWay}`](url).then(res =>{
            resolve(res);
          }).catch(err =>{
            reject(err);
          });
      });
    }
  }
export default requestAxios