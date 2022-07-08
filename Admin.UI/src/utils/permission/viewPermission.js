/*
 * @Description: 视图权限
 * @Author: weig
 * @Date: 2021-08-30 10:06:16
 * @LastEditors: weig
 * @LastEditTime: 2021-12-23 13:46:26
 */
/**
 * 使用说明
  由于使用了全局混入检查权限方法，可以直接使用，无需导入
  v-if="checkView('视图路径/路由路径')" 或 v-if="checkView(['视图路径/路由路径'])"
  如未使用全局混入检查视图/路由权限方法，需导入使用
  import checkView from '@/utils/viewPermission'
  methods: {
      checkView
  }
 */
  import store from '@/store'
  import {IsNullOrEmpty} from "@/common/js/comm"
  import EnumConfig from "@/enum/EnumConfig"
  /**
   * 检查视图权限
   * @param {Array} value
   */
  export default function checkView(value) {
    const menus = store.getters && store.getters.menus;
    var userInfo = sessionStorage.getItem("userInfo");
    var objUserInfo = JSON.parse(userInfo);
    if (!IsNullOrEmpty(objUserInfo.userName)){
      //超级管理员
      if (objUserInfo.userName === EnumConfig.EnumConfig.Account.SuperAdmin || objUserInfo.userName == "administrator"){
        return true;
      }
    }
    var views = menus.map((s) =>{return s.viewPath});
    if (!(views instanceof Array)) {
      return false;
    }
    let hasPermission = false;
    if (value instanceof Array && value.length > 0) {
      var lowerCase = [];       
      value.forEach(function(item, i){
        lowerCase.push(item.toLowerCase());
      });
      hasPermission = views.some(view => {
        return lowerCase.includes(view.toLowerCase());
      });
    } else {
      hasPermission = views.includes(value);
    }
    
    return hasPermission;
  }