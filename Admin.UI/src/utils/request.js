/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-06-11 15:54:16
 * @LastEditors: weig
 * @LastEditTime: 2021-12-16 09:16:11
 */
import axios from './axios'
import router from '../router/index'
import { RefreshToken } from '@/serviceApi/auth'
import { IsNullOrEmpty } from '@/common/js/comm'
import { ElMessage } from 'element-plus'

// 是否正在刷新
let isRefreshing = false;
// 重试队列 每一项都是一个待执行待函数
let requests = [];

const request = {
    /**
     * @description axios ajax 请求
     * @author weig
     * @param {json} params 请求参数
     * @returns {Promise} Promise对象
     */
    axiosApi: (params)=>{
        axios.token = params.token ? params.token : "";
        return axios.AxiosRequest(params.type, params.url, params.data).then(res =>{
            return handleData(res, params.errorCallBack, params.otherCallBack);
        }).catch(err =>{
            return handleData(err, params.errorCallBack, params.otherCallBack);
        });
    }
}

const requestAxios = {
    /**
     * @description post 不带token请求
     * @author weig
     * @param {string} url 请求地址
     * @param {json} data 请求参数
     * @returns {Promise} Promise对象
     */
    apiPostNoToken: (url, data)=>{
        if (IsNullOrEmpty(url)){
            ElMessage.error("请求地址不能为空！");
            return;
        }
        var param ={
            url: url,
            type: "post",
            data: data ? data : "",
            errorCallBack: errorCallBack,
            otherCallBack: refreshToken,
        };
        return request.axiosApi(param);
    },
    /**
     * @description post 带token请求
     * @author weig
     * @param {string} url 请求地址
     * @param {json} data 请求参数
     * @returns {Promise} Promise对象
     */
    apiPost: (url, data)=>{
        if (IsNullOrEmpty(url)){
            ElMessage.error("请求地址不能为空！");
            return;
        }
        var param ={
            url: url,
            type: "post",
            data: data ? data : "",
            token: sessionStorage.getItem("token"),
            errorCallBack: errorCallBack,
            otherCallBack: refreshToken,
        };
       return request.axiosApi(param);
    },
    /**
     * @description get 不带token请求
     * @author weig
     * @param {string} url 请求地址
     * @param {json} data 请求参数
     * @returns {Promise} Promise对象
     */
    apiGetNoToken: (url, data)=>{
        if (IsNullOrEmpty(url)){
            ElMessage.error("请求地址不能为空！");
            return;
        }
        var param ={
            url: url,
            type: "get",
            data: data ?  {params: data} : "",
            errorCallBack: errorCallBack,
            otherCallBack: refreshToken,
        };
       return request.axiosApi(param);
    },
    /**
     * @description get 带token请求
     * @author weig
     * @param {string} url 请求地址
     * @param {json} data 请求参数
     * @returns {Promise} Promise对象
     */
    apiGet: (url, data)=>{
        if (IsNullOrEmpty(url)){
            ElMessage.error("请求地址不能为空！");
            return;
        }
        var param ={
            url: url,
            type: "get",
            data: data ?  {params: data} : "",
            token: sessionStorage.getItem("token"),
            errorCallBack: errorCallBack,
            otherCallBack: refreshToken,
        };
        return request.axiosApi(param);
    },
    /**
     * @description put 不带token请求
     * @author weig
     * @param {string} url 请求地址
     * @param {json} data 请求参数
     * @returns {Promise} Promise对象
     */
    apiPutNoToken: (url, data)=>{
        if (IsNullOrEmpty(url)){
            ElMessage.error("请求地址不能为空！");
            return;
        }
        var param ={
            url: url,
            type: "put",
            data: data ? data : "",
            errorCallBack: errorCallBack,
            otherCallBack: refreshToken,
        };
        return request.axiosApi(param);
    },
    /**
     * @description put 带token请求
     * @author weig
     * @param {string} url 请求地址
     * @param {json} data 请求参数
     * @returns {Promise} Promise对象
     */
    apiPut: (url, data)=>{
        if (IsNullOrEmpty(url)){
            ElMessage.error("请求地址不能为空！");
            return;
        }
        var param ={
            url: url,
            type: "put",
            data: data ? data : "",
            token: sessionStorage.getItem("token"),
            errorCallBack: errorCallBack,
            otherCallBack: refreshToken,
        };
        return request.axiosApi(param);
    },
    /**
     * @description delete 不带token请求
     * @author weig
     * @param {string} url 请求地址
     * @param {json} data 请求参数
     * @returns {Promise} Promise对象
     */
    apiDeleteNoToken: (url, data)=>{
        if (IsNullOrEmpty(url)){
            ElMessage.error("请求地址不能为空！");
            return;
        }
        var param ={
            url: url,
            type: "delete",
            data: data ? data : "",
            errorCallBack: errorCallBack,
            otherCallBack: refreshToken,
        };
        return request.axiosApi(param);
    },
    /**
     * @description delete 带token请求
     * @author weig
     * @param {string} url 请求地址
     * @param {json} data 请求参数
     * @returns {Promise} Promise对象
     */
    apiDelete: (url, data)=>{
        if (IsNullOrEmpty(url)){
            ElMessage.error("请求地址不能为空！");
            return;
        }
        var param ={
            url: url,
            type: "delete",
            data: data ? data : "",
            token: sessionStorage.getItem("token"),
            errorCallBack: errorCallBack,
            otherCallBack: refreshToken,
        };
        return request.axiosApi(param);
    },
}

/**
 * @description 数据处理函数
 * @author weig
 * @param {object} res 请求成功信息对象
 * @param {function} errorCallBack 错误信息回调处理
 * @param {function} otherCallBack 错误信息中嵌套的回调
 * @returns {Promise} Promise对象
 */
function handleData(res, errorCallBack, otherCallBack){
    return new Promise((resolve, reject)=>{
        if (res.status === 200){//接口请求正常  
          if (res.msg == "login token is expiredlogin token is expired" && res.msgCode=="FAIL"){
            ElMessage.error(res.data.msg);
            router.push({ name: "Login" });
          } else if (res.msg == "login token not exist" && res.msgCode == "FATL"){
            ElMessage.error(res.msg);
            router.push({ name: "Login" });
          } else {
            resolve(res.data);
          }
        } else {//接口请求出错     
          if (errorCallBack && typeof errorCallBack === "function"){
              errorCallBack(res, otherCallBack).then(res =>{  
                  resolve(res.data);
              }).catch(err => {
                  reject(err);
              });
          }
        }
    });
}

/**
 * @description 错误响应处理回调函数
 * @author weig
 * @param {object} error 请求错误信息对象
 * @returns {Promise} Promise对象
 * 
 */
function errorCallBack(error, callBack){
    const { response } = error;
    if (response) {
        switch (response.status) {
          case 401://服务器拒绝执行，一般token或session过期 
            if (callBack && typeof callBack === "function") {
                return callBack(response);
            }
            // return Promise.resolve(error);
            break
          case 403://权限不足
            ElMessage.error("403 请求的资源不允许访问");
            // router.push({ name: "403" });
            break
          case 404://找不到地址
            // ElMessage.error("404 请求的内容不存在");
            router.push({ name: "404" });
            break
          case 405:
            ElMessage.error("405 不允许此方法");
            break
          case 406:
            ElMessage.error("406 请求的资源并不符合要求");
            break
          case 408:
            ElMessage.error("408 客户端请求超时");
            break
          case 413:
            ElMessage.error("413 请求体过大");
            break
          case 415:
            ElMessage.error("415 类型不正确");
            break
          case 416:
            ElMessage.error("416 请求的区间无效");
            break
  
          ////服务器请求异常 begin
          case 500:
            ElMessage.error("500 Internal Server Error 内部服务错误");
            break
          case 501:
            ElMessage.error("501 服务器不具备完成请求的功能");
            break
          case 502:
            ElMessage.error("502 Bad Gateway错误");
            break
          case 503:
            ElMessage.error("503 服务器目前无法使用（由于超载或停机维护）。通常，这只是暂时状态。（服务不可用）");
            break
          case 504:
            ElMessage.error("504 Bad Gateway timeout 网关超时");
            break
          case 505:
            ElMessage.error("505 服务器不支持请求中所用的 HTTP 协议版本。（HTTP 版本不受支持）");
            break
          ////服务器请求异常 end
  
          default:
            // ElMessage.error(response.statusText);
            router.push({ name: "Login" });
            break;
            // router.push({ name: "Login" });
        }
      }
      else {
        //服务器连结果都没有返回
        if (!window.navigator.onLine) {
          //客户端断网了，将页面跳转到一个断网的页面，可以结合路由使用
          ElMessage.error("网络中断，重新登录");
          router.push({ name: "Login" });
          return;
        }
      }
    return Promise.reject(error);
}

/**
 * @description 刷新token
 * @author weig
 * @param {object} error 错误信息对象
 * @returns {Promise} Promise对象
 */
function refreshToken(error){
    if (!isRefreshing) {
        isRefreshing = true;
        // 获取旧 token
        var oldToken = sessionStorage.getItem("token");
        // 刷新token
        return RefreshToken(oldToken).then(res => {     
          if (res.code == 1) {
            sessionStorage.setItem("token", res.data.token)
            // 重新请求接口 前过期的接口
            error.config.headers.Authorization = 'Bearer ' + res.data.token
            error.config.headers.withCredentials = true;
            requests.length > 0 && requests.map((cb) => {
              cb();
            });
            requests = [];  //注意要清空
            return axios.request(error.config);
          } else {
            ElMessage.error("刷新token失败,重新登录");
            sessionStorage.setItem("token", "");
            router.push({ name: "Login" });
          }
        })
        .finally(() => {
          isRefreshing = false;
        });
    } else {
        requests = [];  //注意要清空
        // 正在刷新token ,把后来的接口缓存起来
        return new Promise((resolve) => {
          requests.push(() => {
            error.config.headers.Authorization = 'Bearer ' + sessionStorage.getItem('token');
            error.config.headers.withCredentials = true;
            resolve(axios.request(error.config));
          });
        });
      }
}

export default requestAxios;

