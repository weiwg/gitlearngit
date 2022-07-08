/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-09-01 20:26:53
 * @LastEditors: weig
 * @LastEditTime: 2021-12-17 11:35:11
 */
import request from '../../utils/request'
import EnumConfig from '../../enum/EnumConfig'

var CURR_API_VERSION = EnumConfig.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var API_VERSION = CURR_API_VERSION == 'V0' ? '' : `/S/${CURR_API_VERSION}`;

//获取缓存列表
export const getCachePageList = ()=>{
    return request.apiGet(`/api${API_VERSION}/System/Cache/List`);
}

//清除缓存
export const ClearCache = (params)=>{
    return request.apiDelete(`/api${API_VERSION}/System/Cache/Clear?cacheKey=`+ params);
}