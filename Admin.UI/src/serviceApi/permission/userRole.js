/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-07-28 11:06:57
 * @LastEditors: weig
 * @LastEditTime: 2021-12-17 11:29:04
 */
// import axios from '../../utils/axios'
import request from '../../utils/request'
import EnumConfig from '../../enum/EnumConfig'

var CURR_API_VERSION = EnumConfig.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var API_VERSION = CURR_API_VERSION == 'V0' ? '' : `/S/${CURR_API_VERSION}`;

//查询分页用户角色
export const getUserRoleListPage = (params) => {
    // return axios.post('/api/Auth/UserRole/GetUserRoleList', params);
    return request.apiGet(`/api${API_VERSION}/Auth/UserRole/GetUserRoleList`, params);
}
//新增用户角色
export const addUserRole = (params) => {
    // return axios.post('/api/Auth/UserRole/AddUserRoleInfo', params);
    return request.apiPost(`/api${API_VERSION}/Auth/UserRole/AddUserRoleInfo`, params);
}
//修改用户角色
export const editUserRole = (params) => {
    // return axios.put('/api/Auth/UserRole/UpdateUserRoleInfo', params);
    return request.apiPut(`/api${API_VERSION}/Auth/UserRole/UpdateUserRoleInfo`, params);
}
//删除用户角色
export const removeUserRole = (params) => {
    // return axios.delete(`/api/Auth/UserRole/SoftDelete?id=${params}`);
    return request.apiDelete(`/api${API_VERSION}/Auth/UserRole/SoftDelete?id=${params}`);
}
//批量删除用户角色
export const batchRemoveUserRole = (params) => {
    // return axios.put('/api/Auth/UserRole/BatchSoftDelete',params);
    return request.apiPut(`/api${API_VERSION}/Auth/UserRole/BatchSoftDelete`,params);
}
//查询角色数据
export const getRoleInfo = () => {
    // return axios.get('/api/Auth/UserRole/GetRoleInfo');
    return request.apiGet(`/api${API_VERSION}/Auth/UserRole/GetRoleInfo`);
}
//查询用户
export const getUserInfo = (params) => {
    // return axios.get('/api/Auth/UserRole/GetUserInfo?name=' + params);
    return request.apiGet(`/api${API_VERSION}/Auth/UserRole/GetUserInfo?name=` + params);
}