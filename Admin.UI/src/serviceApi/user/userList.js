/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-08-05 16:43:52
 * @LastEditors: weig
 * @LastEditTime: 2021-12-17 11:37:13
 */
import request from '../../utils/request'
import EnumConfig from '../../enum/EnumConfig'

var CURR_API_VERSION = EnumConfig.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var API_VERSION = CURR_API_VERSION == 'V0' ? '' : `/S/${CURR_API_VERSION}`;

//获取用户列表信息
export const getUserList = (params)=>{
    return request.apiGet(`/api${API_VERSION}/User/UserInfo/GetPage`, params);
}
//更新用户信息
export const updateUserInfo = (params)=>{
    return request.apiPut(`/api${API_VERSION}/User/UserInfo/Update`, params);
}
//重置系统密码
export const resetPassword = (params)=>{
    return request.apiPut(`/Api${API_VERSION}/User/UserInfo/ResetSysPassword`, params);
}
//重置手机
export const ResetPhone = (params)=>{
    return request.apiPut(`/api${API_VERSION}/User/UserInfo/ResetPhone`, params);
}
//重置邮箱
export const ResetEmail = (params)=>{
    return request.apiPut(`/api${API_VERSION}/User/UserInfo/ResetEmail`, params);
}
//获取用户下拉列表信息
export const GetSelectList = (params)=>{
    return request.apiGet(`/api${API_VERSION}/User/UserInfo/GetSelectList`, params);
}