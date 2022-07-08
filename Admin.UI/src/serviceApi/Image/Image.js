/*
 * @Description: 
 * @Author: 优
 * @Date: 2021-06-11 15:54:16
 * @LastEditors: weig
 * @LastEditTime: 2021-12-17 11:20:09
 */
import request from '../../utils/request'
import EnumConfig from '../../enum/EnumConfig'

var CURR_API_VERSION = EnumConfig.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var API_VERSION = CURR_API_VERSION == 'V0' ? '' : `/S/${CURR_API_VERSION}`;

//图片查询分页
export const GetListGetPageList=(params)=>{
    return request.apiGet(`/api${API_VERSION}/Resource/Image/GetListGetPageList`,params)
}
//删除图片
export const ImageSoftDelete=(params)=>{
  return  request.apiDelete(`/api${API_VERSION}/Resource/Image/SoftDelete?imgId=`+params)
}
//批量删除图片
export const ImageBatchSoftDelete=(params)=>{
    return request.apiPut(`/api${API_VERSION}/Resource/Image/BatchSoftDelete`,params)
}
//上传图片
export const postimg=(params)=>{
  return request.apiPost(`/Api${API_VERSION}/Resource/Image/UploadImage`,params)
}
