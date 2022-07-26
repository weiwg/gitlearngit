/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-10-12 10:03:41
 * @LastEditors: weig
 * @LastEditTime: 2022-07-26 16:04:11
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
            {value:"开料", label: "开料"},
            {value:"QC工位", label: "QC工位"},
            {value:"M1", label: "M1"},
            {value:"M2", label: "M2"},
            {value:"M3.1", label: "M3.1"},
            {value:"M3", label: "M3"},
            {value:"M4", label: "M4"},
            {value:"M5", label: "M5"},
            {value:"M6", label: "M6"},
            {value:"M19", label: "M19"},
            {value:"M20", label: "M20"},
            {value:"M21", label: "M21"},
            {value:"M22", label: "M22"},
            {value:"M23", label: "M23"},
            {value:"M24", label: "M24"},
            {value:"M25", label: "M25"},
            {value:"M26", label: "M26"},
            {value:"M27", label: "M27"},
            {value:"M28", label: "M28"},
            {value:"M29", label: "M29"},
            {value:"M31", label: "M31"},
            {value:"M33", label: "M33"},
            {value:"M30", label: "M30"},
            {value:"M34", label: "M34"},
            {value:"M36", label: "M36"},
            {value:"M9.5", label: "M9.5"},
            {value:"M9.7", label: "M9.7"},
            {value:"M14", label: "M14"},
            {value:"M15", label: "M15"},
            {value:"M16", label: "M16"},
            {value:"M37", label: "M37"},
            {value:"M38", label: "M38"},
            {value:"M35", label: "M35"},
            {value:"M39", label: "M39"},
            {value:"M40", label: "M40"},
            {value:"M45", label: "M45"},
            {value:"M46", label: "M46"},
            {value:"M47", label: "M47"},
            {value:"M48", label: "M48"},
            {value:"M44", label: "M44"},
            {value:"M49", label: "M49"},
            {value:"M50.1", label: "M50.1"},
            {value:"M51/52", label: "M51/52"},
            {value:"T1", label: "T1"},
            {value:"T2", label: "T2"},
            {value:"T3 auto", label: "T3 auto"},
            {value:"T4", label: "T4"},
            {value:"T5", label: "T5"},
            {value:"T6", label: "T6"},
            {value:"T7", label: "T7"},
            {value:"T8", label: "T8"},
            {value:"T9", label: "T9"},
            {value:"T10", label: "T10"},
            {value:"M53", label: "M53"},
            {value:"M54", label: "M54"},
            {value:"其他", label: "其他"}
        ]
    },
    FProcessJson:{
        全部: "全部",
        开料: "开料",
        QC工位: "QC工位",
        M1: "M1",
        M2: "M2",
        "M3.1": "M3.1",
        M3: "M3",
        M4: "M4",
        M5: "M5",
        M6: "M6",
        M19: "M19",
        M20: "M20",
        M21: "M21",
        M22: "M22",
        M23: "M23",
        M24: "M24",
        M25: "M25",
        M26: "M26",
        M27: "M27",
        M28: "M28",
        M29: "M29",
        M31: "M31",
        M33: "M33",
        M30: "M30",
        M34: "M34",
        M36: "M36",
        "M9.5": "M9.5",
        "M9.7": "M9.7",
        M14: "M14",
        M15: "M15",
        M16: "M16",
        M37: "M37",
        M38: "M38",
        M35: "M35",
        M39: "M39",
        M40: "M40",
        M45: "M45",
        M46: "M46",
        M47: "M47",
        M48: "M48",
        M44: "M44",
        M49: "M49",
        "M50.1": "M50.1",
        "M51/52": "M51/52",
        T1: "T1",
        T2: "T2",
        "T3 auto": "T3 auto",
        T4: "T4",
        T5: "T5",
        T6: "T6",
        T7: "T7",
        T8: "T8",
        T9: "T9",
        T10: "T10",
        M53: "M53",
        M54: "M54",
        其他: "其他"
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