/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-10-12 10:03:41
 * @LastEditors: weig
 * @LastEditTime: 2022-07-11 11:20:11
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
        SuperAdmin: "administrator" //超级管理员账号，暂时默认是这个，一般不会改
    },
    //项目名称
    ProName:{
        ArrProName: [
            {value: "HEB", label: "HEB",},
            {value: "HEC", label: "HEC"},
            {value: "HED", label: "HED"},
            {value: "HEF", label: "HEF"},
        ]
    },
    ProNameJson:{
        HEB: "HEB",
        HEC: "HEC",
        HED: "HED",
        HEF: "HEF"
    },
    //产线异常状态
    AbnomalStatus:{
        arrAbnomalStatus:[
            {value: 0, label: "全部"},
            {value: 1, label: "已处理"},
            {value: 2, label: "未处理"},
        ]     
    },
    AbnormalStatusJson:{
        全部: 0,
        已处理: 1,
        未处理: 2
    },
    //线体
    Lines:{
        line: [
            {value: "Line-1", label:"Line-1",proName: "HEB"},
            {value: "Line-1", label:"Line-1",proName: "HEC"},
            {value: "Line-1", label:"Line-1",proName: "HED"},
            {value: "Line-2", label:"Line-2",proName: "HED"},
            {value: "Line-1", label:"Line-1",proName: "HEF"},
            {value: "Line-2", label:"Line-2",proName: "HEF"},
        ]
    },
    //责任部门
    ResponDepart:{
        ArrResponDepart:[
            {value:0, label: "全部"},
            {value:1, label: "ME部"},
            {value:2, label: "生产部"},
            {value:3, label: "QE部"},
            {value:4, label: "厂务部"},
            {value:5, label: "仓库部"},
            {value:6, label: "IQC部"},
            {value:7, label: "REL部"},
            {value:8, label: "PD部"},
            {value:9, label: "IE部"}
        ]
    },
    ResponDepartJson:{
        全部: 0,
        ME部: 1,
        生产部: 2,
        QE部: 3,
        厂务部: 4,
        仓库部: 5,
        IQC部: 6,
        REL部: 7,
        PD部: 8,
        IE部: 9
    },
    //班别
    ClassAB:{
        arrClassAB:[
            {value:"全部", label: "全部"},
            {value:"白班", label: "白班"},
            {value:"晚班", label: "晚班"}
        ]
    },
    //HEX 工序站点
    FProcess:{
        arrFProcess:[
            {value:"全部", label: "全部"},
            //胶壳工序
            {value:"M6", label: "M6"},
            //主线工序
            {value:"INPUT", label: "INPUT"},
            {value:"QC4", label: "QC4"},
            {value:"M18.4", label: "M18.4"},
            {value:"QC5", label: "QC5"},
            {value:"M22", label: "M22"},
            {value:"M33", label: "M33"},
            {value:"QC6", label: "QC6"},
            {value:"M34", label: "M34"},
            {value:"M16.1", label: "M16.1"},
            {value:"M37", label: "M37"},
            {value:"M40", label: "M40"},
            {value:"M45", label: "M45"},
            {value:"M48", label: "M48"},
            {value:"M44", label: "M44"},
            {value:"M50", label: "M50"},
            {value:"MQC", label: "MQC"},
            {value:"FQC", label: "FQC"},
            //测试工站
            {value:"Btn_Depth", label: "Btn_Depth"},
            {value:"Jade_Test", label: "Jade_Test"},
            {value:"CF_Fit_Check", label: "CF_Fit_Check"},
            {value:"Magnet_Peak_Force", label: "Magnet_Peak_Force"},
            {value:"SOTA_Scan", label: "SOTA_Scan"}
        ]
    },
    FProcessJson:{
        全部:"全部",
        M6: "M6",
        INPUT: "INPUT",
        QC4: "QC4",
        "M18.4": "M18.4",
        QC5: "QC5",
        M22: "M22",
        M33: "M33",
        QC6: "QC6",
        M34: "M34",
        "M16.1": "M16.1",
        M37: "M37",
        M40: "M40",
        M45: "M45",
        M48: "M48",
        M44: "M44",
        M50: "M50",
        MQC: "MQC",
        FQC: "FQC",
        Btn_Depth: "Btn_Depth",
        Jade_Test: "Jade_Test",
        CF_Fit_Check: "CF_Fit_Check",
        Magnet_Peak_Force: "Magnet_Peak_Force",
        SOTA_Scan: "SOTA_Scan"
    },
    //异常大类(异常/停线)
    AbnormalType:{
        arrAbnormalType:[
            {value:0, label: "全部"},
            {value:1, label: "异常"},
            {value:2, label: "停线"},
        ]
    },
    AbnormalTypeJson:{
        全部: 0,
        异常: 1,
        停线: 2
    },
    //小类(机器故障/物料异常/停电等)
    AbnormalItemType:{
        arrAbnormalItemType:[
            {value:0, label: "全部"},
            {value:10, label: "机械故障"},
            {value:11, label: "物料异常"},
            {value:12, label: "停电"},
        ]
    },
    AbnormalItemTypeJson:{
        全部: 0,
        机械故障: 10,
        物料异常: 11,
        停电: 12
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