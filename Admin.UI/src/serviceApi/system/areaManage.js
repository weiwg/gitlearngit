/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-08-04 17:30:49
 * @LastEditors: weig
 * @LastEditTime: 2021-12-17 11:34:04
 */
import request from '../../utils/request'
import EnumConfig from '../../enum/EnumConfig'

var CURR_API_VERSION = EnumConfig.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var API_VERSION = CURR_API_VERSION == 'V0' ? '' : `/S/${CURR_API_VERSION}`;

//分页查询
export const getAreaList = (params)=>{
    return request.apiGet(`/api${API_VERSION}/System/SysRegion/GetPage`, params);
}
//单条查询
export const getAreaInfo = (params)=>{
    return request.apiGet(`/api${API_VERSION}/System/SysRegion/Get?id=`+params);
}
//更新地区数据
export const updateAreaInfo = (params)=>{
    return request.apiPut(`/api${API_VERSION}/System/SysRegion/Update`, params);
}
//新增地区数据
export const addAreaInfo = (params) => { 
    return request.apiPost(`/api${API_VERSION}/System/SysRegion/Add`, params)
}
//获取下拉菜单数据
export const getSelectList = (params) => { 
    return request.apiGet(`/api${API_VERSION}/System/SysRegion/GetSelectList?parentId=`+params)
}
//删除地区信息
export const SoftDelete = (params) => { 
    return request.apiDelete(`/api${API_VERSION}/System/SysRegion/SoftDelete?id=` + params)
}
//批量删除地区信息
export const BatchSoftDelete = (params) => { 
    return request.apiPut(`/api${API_VERSION}/System/SysRegion/BatchSoftDelete`, params)
}
