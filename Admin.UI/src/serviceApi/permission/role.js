/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-07-07 13:36:50
 * @LastEditors: weig
 * @LastEditTime: 2021-12-17 11:27:38
 */

import request from '../../utils/request'
import EnumConfig from '../../enum/EnumConfig'

var CURR_API_VERSION = EnumConfig.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var API_VERSION = CURR_API_VERSION == 'V0' ? '' : `/S/${CURR_API_VERSION}`;

//查询分页角色
export const getRoleListPage = (params) => {
    return request.apiGet(`/api${API_VERSION}/Auth/Role/GetPage`, params);
}
//新增角色
export const addRole = (params) => {
    return request.apiPost(`/api${API_VERSION}/Auth/Role/Add`, params);
}
//修改角色
export const editRole = (params) => {
    return request.apiPut(`/api${API_VERSION}/Auth/Role/Update`, params);
}
//删除角色
export const removeRole = (params) => {
    return request.apiDelete(`/api${API_VERSION}/Auth/Role/SoftDelete?id=${params}`);
}
//批量删除角色
export const batchRemoveRole = (params) => {
    return request.apiPut(`/api${API_VERSION}/Auth/Role/BatchSoftDelete`,params);
}
