/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-10-12 10:03:41
 * @LastEditors: weig
 * @LastEditTime: 2021-12-29 10:20:11
 */

var EnumConfig={
    //order begin
    orderStatus:{
        全部: 0,
        待支付: 1,
        待接单: 2,
        已接单: 3,
        送货中: 4,
        已送达: 5,
        已完成: 6,
        已取消: 7,
        原路退回: 88,
    },
    //order end

    //T_Auth_Api begin
    apiVersion_str:{
        // M_V1: "M_V1",
        S_V1: "S_V1",
        // Open_V1: "Open_V1",
    },
    //T_Auth_Api end

    //T_Auth_Role begin
    RoleType:{
        SuperAdmin: 999,
        Admin: 100,
        User: 200
    },
    //T_Auth_Role end
    API_VIEW_VERSION: {
        CURR_API_VIEW_VERSION: 'V1',   //当前系统接口版本号， 例如： V0,V1,V2,V3
    },
    Account: {
        SuperAdmin: "eonup" //超级管理员账号，暂时默认是这个，一般不会改
    },
    
    /**
     * 获取枚举名称 
     * 如：EnumConfig.getEnumName(item.Status, EnumConfig.MarketStatusEnum)
     * @param {String|Number} value 枚举值
     * @param {Object} enumobj 枚举对象
     * @returns 
     */
    getEnumName: (value, enumobj)=>{
        let enumName = "";
        for(let o in enumobj){
            if (value == enumobj[o]){
                enumName = o;
                break;
            }
        }
        return enumName;
    }
}

export default{
    EnumConfig
}