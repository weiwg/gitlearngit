/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-08-16 13:53:37
 * @LastEditors: weig
 * @LastEditTime: 2021-12-17 11:29:27
 */
import request from '../../utils/request'
import EnumConfig from '../../enum/EnumConfig'

var CURR_API_VERSION = EnumConfig.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var API_VERSION = CURR_API_VERSION == 'V0' ? '' : `/S/${CURR_API_VERSION}`;

//查询分页分页视图
export const getViewListPage = (params) => {
    return request.apiPost(`/api${API_VERSION}/Auth/View/GetPage`, params);
}
//查询单条视图
export const getViewInfo = (params) => {
    return request.apiGet(`/api${API_VERSION}/Auth/View/Get?id=${params}`);
}
//查询全部视图
export const getAllViewInfo = (params) => {
    return request.apiGet(`/api${API_VERSION}/Auth/View/GetList`,params);
}
//新增视图
export const addView = (params) => {
    return request.apiPost(`/api${API_VERSION}/Auth/View/Add`, params);
}
//修改视图
export const editView = (params) => {
    return request.apiPut(`/api${API_VERSION}/Auth/View/Update`, params);
}
//删除视图
export const removeView = (params) => {
    return request.apiDelete(`/api${API_VERSION}/Auth/View/SoftDelete?id=${params}`);
}
//批量删除视图
export const batchRemoveView = (params) => {
    return request.apiPut(`/api${API_VERSION}/Auth/View/BatchSoftDelete`,params);
}

//同步视图 支持新增和修改视图 根据视图是否存在自动禁用和启用视图
export const syncView  = (params) => {
    return request.apiPost(`/api${API_VERSION}/Auth/View/Sync`,params);
}