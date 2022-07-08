/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-06-30 14:41:16
 * @LastEditors: weig
 * @LastEditTime: 2021-12-17 11:11:08
 */
import request from '../utils/request'
import EnumConfig from '../enum/EnumConfig'

var CURR_API_VERSION = EnumConfig.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var API_VERSION = CURR_API_VERSION == 'V0' ? '' : `/S/${CURR_API_VERSION}`;

//登出
export const logout = (params) => {
  return request.apiGet(`/api${API_VERSION}/User/Account/logout`);
}
//刷新token
export const RefreshToken = (params) => {
  return request.apiGetNoToken(`/api${API_VERSION}/User/Account/Refresh?token=${params}`);
}
//获取当前用户信息
export const GetCurrUserInfo = () => {
  return request.apiGet(`/api${API_VERSION}/User/UserInfo/GetCurrUserInfo`);
}
//修改密码
export const UpdatePassword = (params) => {
  return request.apiPut(`/api${API_VERSION}/User/UserInfo/UpdatePassword`, params);
}
//登录
export const  Login=(params)=>{ 
  return request.apiPost(`/api${API_VERSION}/User/Account/Login`,params); 
}
//注册账号
export const Register=(params)=>{
  return request.apiPost(`/api${API_VERSION}/User/Account/Register`,params); 
}