/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-08-10 16:06:36
 * @LastEditors: weig
 * @LastEditTime: 2021-12-17 11:24:40
 */
// import axios from '../../utils/axios'
import request from '../../utils/request'
import EnumConfig from '../../enum/EnumConfig'

var CURR_API_VERSION = EnumConfig.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var API_VERSION = CURR_API_VERSION == 'V0' ? '' : `/S/${CURR_API_VERSION}`;

//查询分页Api列表
export const getApiListPage = (params) => {
    // return axios.post('/api/Auth/Api/GetPage', params);
    return request.apiGet(`/api${API_VERSION}/Auth/Api/GetPage`, params);
}
//查询单条Api
export const getApiInfo = (params) => {
    // return axios.get(`/api/Auth/Api/Get?id=${params}`);
    return request.apiGet(`/api${API_VERSION}/Auth/Api/Get?id=${params}`);
}
//查询全部Api
export const getAllApiInfo = (params) => {
    // return axios.get(`/api/Auth/Api/GetList?key=${params.key.trim()}&apiVersion=${params.apiVersion}`);
    return request.apiGet(`/api${API_VERSION}/Auth/Api/GetList?key=${params.key.trim()}&apiVersion=${params.apiVersion}`);
}
//新增Api
export const addApi = (params) => {
    // return axios.post('/api/Auth/Api/Add', params);
    return request.apiPost(`/api${API_VERSION}/Auth/Api/Add`, params);
}
//修改Api
export const editApi = (params) => {
    // return axios.put('/api/Auth/Api/Update', params);
    return request.apiPut(`/api${API_VERSION}/Auth/Api/Update`, params);
}
//删除Api
export const removeApi = (params) => {
    // return axios.delete(`/api/Auth/Api/SoftDelete?id=${params}`);
    return request.apiDelete(`/api${API_VERSION}/Auth/Api/SoftDelete?id=${params}`);
}
//批量删除Api
export const batchRemoveApi = (params) => {
    // return axios.put('/api/Auth/Api/BatchSoftDelete',params);
    return request.apiPut(`/api${API_VERSION}/Auth/Api/BatchSoftDelete`,params);
}
//同步接口 支持新增和修改接口 根据接口是否存在自动禁用和启用api
export const syncApi = (params) => {
    // return axios.post('/api/Auth/Api/Sync',params);
    return request.apiPost(`/api${API_VERSION}/Auth/Api/Sync`,params);
}

export const getV2SwaggerJson = (params) => {  //已废
    // return axios.get('/swagger/V2/swagger.json?params=', params);
    return request.apiGet('/swagger/V2/swagger.json?params=', params);
}