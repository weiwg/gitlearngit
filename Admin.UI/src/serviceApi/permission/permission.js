/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-07-06 13:54:44
 * @LastEditors: weig
 * @LastEditTime: 2021-12-20 18:06:56
 */
import request from '../../utils/request'
import EnumConfig from '../../enum/EnumConfig'

var CURR_API_VERSION = EnumConfig.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var API_VERSION = CURR_API_VERSION == 'V0' ? '' : `/S/${CURR_API_VERSION}`;

// const apiPrefix = `${process.env.VUE_APP_BASE_API}/admin/permission/`

// 权限管理

//查询权限列表
export const getPermissionList = (params) => {
  return request.apiGet(`/api${API_VERSION}/Auth/Permission/GetList?key=${params.key}&start=${params.start}&end=${params.end}&apiVersion=${params.apiVersion}`);
}
//查询单条分组
export const getGroup = (params) => {
  return request.apiGet(`/api${API_VERSION}/Auth/Permission/GetGroup?id=${params}`);
}
//查询单条菜单
export const getMenu = (params) => {
  return request.apiGet(`/api${API_VERSION}/Auth/Permission/GetMenu?id=${params}`);
}
//查询单条接口
export const getApi = (params) => {
  return request.apiGet(`/api${API_VERSION}/Auth/Permission/GetApi?id=${params}`);
}
//查询单条权限点
export const getDot = (params) => {
  return request.apiGet(`/api${API_VERSION}/Auth/Permission/GetDot?id=${params}`);
}
//查询角色权限-权限列表
export const getRolePermissionAndPermissionList = () => {
  return request.apiGet(`/api${API_VERSION}/Auth/Permission/GetPermissionList`);
}
//软删除权限
export const softDeletePermission = (params) => {
  return request.apiDelete(`/api${API_VERSION}/Auth/Permission/SoftDelete?id=${params}`);
}
//彻底删除权限
export const deletePermission = (params) => {
  return request.apiDelete(`/api${API_VERSION}/Auth/Permission/Delete?id=${params}`);
}
//查询角色权限-权限列表
export const getPermissions = () => {
  return request.apiGet(`/api${API_VERSION}/Auth/Permission/GetPermissionList`);
}
//查询角色权限
export const getPermissionIds = (params) => {
  return request.apiGet(`/api${API_VERSION}/Auth/Permission/GetRolePermissionList?roleId=${params}`);
}
//查询租户权限
export const GetTenantPermissionIds = (params) => {
  return request.apiGet(`/api${API_VERSION}/Auth/Permission/GetTenantPermissionList?tenantId=${params}`);
}
//保存角色权限
export const addRolePermission = (params) => {
  return request.apiPost(`/api${API_VERSION}/Auth/Permission/Assign`, params);
}
//保存租户权限
export const saveTenantPermissions = (params) => {
  return request.apiPost(`/api${API_VERSION}/Auth/Permission/SaveTenantPermissions`, params);
}
//新增分组
export const addGroup = (params) => {
  return request.apiPost(`/api${API_VERSION}/Auth/Permission/AddGroup`, params);
}
//新增菜单
export const addMenu = (params) => {
  return request.apiPost(`/api${API_VERSION}/Auth/Permission/AddMenu`, params);
}
//新增接口
export const addApi = (params) => {
  return request.apiPost(`/api${API_VERSION}/Auth/Permission/AddApi`,params);
}
//新增权限点
export const addDot = (params) => {
  return request.apiPost(`/api${API_VERSION}/Auth/Permission/AddDot`, params);
}
//修改分组
export const updateGroup = (params) => {
  return request.apiPut(`/api${API_VERSION}/Auth/Permission/UpdateGroup`, params);
}
//修改菜单
export const updateMenu = (params) => {
  return request.apiPut(`/api${API_VERSION}/Auth/Permission/UpdateMenu`, params);
}
//修改接口
export const updateApi = (params) => {
  return request.apiPut(`/api${API_VERSION}/Auth/Permission/UpdateApi`, params);
}
//修改权限点
export const updateDot = (params) => {
  return request.apiPut(`/api${API_VERSION}/Auth/Permission/UpdateDot`, params);
}
//获取权限点和菜单
export const getPermissionsPointMenu = ()=>{
  return request.apiGet(`/api${API_VERSION}/Auth/Permission/GetPermissionsPointMenu`);
}
//验证权限
export const ValidatePermissions = (params) =>{
  return request.apiGet(`/api${API_VERSION}/Auth/Permission/ValidatePermissions`, params);
}

