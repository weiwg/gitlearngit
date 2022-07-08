/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-06-11 15:54:16
 * @LastEditors: weig
 * @LastEditTime: 2021-12-17 11:36:27
 */
import ipMap from '../../utils/ipMap'
import request from '../../utils/request'
import EnumConfig from '../../enum/EnumConfig'

var CURR_API_VERSION = EnumConfig.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var API_VERSION = CURR_API_VERSION == 'V0' ? '' : `/S/${CURR_API_VERSION}`;

//登录日志
export const getLoginLogPage = (params)=>{
    return request.apiGet(`/api${API_VERSION}/Record/RecordLogin/GetPage`, params);
}