/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-08-23 14:16:56
 * @LastEditors: weig
 * @LastEditTime: 2021-12-17 11:35:47
 */
import request from '../../utils/request'
import EnumConfig from '../../enum/EnumConfig'

var CURR_API_VERSION = EnumConfig.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var API_VERSION = CURR_API_VERSION == 'V0' ? '' : `/S/${CURR_API_VERSION}`;

//查询分页租户
export const getTenantList = (params)=>{
    return request.apiGet(`/api${API_VERSION}/System/Tenant/GetPage`, params);
}
//修改租户
export const updateTenantInfo = (params)=>{
    return request.apiPut(`/api${API_VERSION}/System/Tenant/Update`, params);
}
//新增租户
export const addTenantInfo = (params) => { 
    return request.apiPost(`/api${API_VERSION}/System/Tenant/Add`, params)
}
//删除租户信息
export const SoftDeleteTenant = (params) => { 
    return request.apiDelete(`/api${API_VERSION}/System/Tenant/SoftDelete?id=` + params)
}
//彻底删除租户信息
export const DeleteTenant = (params) => { 
    return request.apiDelete(`/api${API_VERSION}/System/Tenant/Delete?id=` + params)
}
//批量删除租户
export const BatchSoftDeleteTenant = (params) => { 
    return request.apiPut(`/api${API_VERSION}/System/Tenant/BatchSoftDelete`, params)
}