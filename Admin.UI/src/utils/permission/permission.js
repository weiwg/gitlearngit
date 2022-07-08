/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-07-06 10:38:59
 * @LastEditors: weig
 * @LastEditTime: 2021-12-23 13:46:40
 */

/**
 * 使用说明
  由于使用了全局混入检查权限方法，可以直接使用，无需导入
  v-if="checkPermission('权限点')" 或 v-if="checkPermission(['权限点'])"
  如未使用全局混入检查权限方法，需导入使用
  import checkPermission from '@/utils/permission'
  methods: {
      checkPermission
  }
 */
  import store from '@/store'
  import {IsNullOrEmpty} from "@/common/js/comm"
  import EnumConfig from "@/enum/EnumConfig"
  
  /**
   * 检查权限
   * @param {Array} value
   */
  export default function checkPermission(value) {
    const permissions = store.getters && store.getters.permissions;
    var userInfo = sessionStorage.getItem("userInfo");
    var objUserInfo = JSON.parse(userInfo);
    if (!IsNullOrEmpty(objUserInfo.userName)){
      //超级管理员
      if (objUserInfo.userName === EnumConfig.EnumConfig.Account.SuperAdmin || objUserInfo.userName == "administrator"){
        return true;
      }
    }
    if (!(permissions instanceof Array)) {
      return false;
    }
    let hasPermission = false;
    if (value instanceof Array && value.length > 0) {
      var lowerCase = [];
      value.forEach(function(item, i){
        lowerCase.push(item.toLowerCase());
      });
      hasPermission = permissions.some(permission => {
        return lowerCase.includes(permission.toLowerCase());
      });
    } else {
      hasPermission = permissions.includes(value);
    }
  
    return hasPermission
  }
