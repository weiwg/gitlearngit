/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-10-12 17:24:43
 * @LastEditors: weig
 * @LastEditTime: 2021-12-21 09:34:07
 */
import {createRouter, createWebHistory} from "vue-router";
import { ValidatePermissions} from '@/serviceApi/permission/permission'
import { GetQueryValue, ridUrlParam, getUrlParam, IsNullOrEmpty, delCookie,editUrlQuery } from '@/common/js/comm'
import { GetCurrUserInfo} from "@/serviceApi/auth";
import request from '../utils/request'
import { ElMessage } from 'element-plus'
import store from '../store/index'
import {routes} from "@/config/routeConfig"
import EnumConfig from '../enum/EnumConfig'

var CURR_API_VERSION = EnumConfig.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var API_VERSION = CURR_API_VERSION == 'V0' ? '' : `/S/${CURR_API_VERSION}`;

const router = createRouter({
    history: createWebHistory(),
    routes
});

const logoto = (next) => {
    request.apiPostNoToken(`/api${API_VERSION}/User/Account/Login`, { "userName": "null", "password": "null", "verifyCode": "null", "verifyCodeKey": "null", "loginToken": getUrlParam("logintoken") }).then(res => {
        if (res.code == 1) {
            sessionStorage.setItem("token", res.data.token);
            //缓存为空时设置用户信息缓存
            //置换token时,重新获取用户信息
            GetCurrUserInfo().then(res =>{
              store.state.user.userInfo = res.data;//全局缓存当前用户信息
              sessionStorage.setItem("userInfo", JSON.stringify(res.data));
              store.commit("user/getPermissionPointMenuInfo_sync");          
            });
        } else {
            sessionStorage.setItem("token", "");
            sessionStorage.setItem("userInfo", "");
            delCookie("ASP.NET_SessionId");
            delCookie("SessionId");
            let tryCount = getUrlParam(window.location.href);
            if (!IsNullOrEmpty(tryCount)) {
                tryCount = parseInt(tryCount +1);
            } else {
                tryCount = 1;
            }
            let url = editUrlQuery(window.location.href, "tryCount", tryCount);
            if (tryCount > 3) {
                return ElMessage.error("登陆失败");
            } else {
                window.location.href = `${process.env.VUE_APP_LOGIN_URL}?returnurl=${encodeURIComponent(url)}`;
                next();
            }
        }
    });
  }

  router.beforeEach((to, form, next) => {
    document.title = "Admin-" + to.meta.title;
    //处理第一次登陆或者退出登陆时路由中的"/"地址会重定向到/seyHome,没有登陆成功时无法跳转到首页的
    // if (to.fullPath.indexOf("/SysHome?logintoken=") != -1){
    //     var fullPath = to.fullPath.replace("/SysHome?logintoken=","/?logintoken=");
    //     to.fullPath = fullPath;
    // }
    //路由跳转前先从缓存中取出权限相关数据，更新到store中，避免每次刷新会清空store中的权限数据
    var authData = sessionStorage.getItem("permission");
    if (authData){
        var objAuthData = JSON.parse(authData);
        store.state.user.menus = objAuthData.menus;
        store.state.user.permissions = objAuthData.permissions;
        store.state.user.showCard = objAuthData.showCard;
    }
    next()
    return;
    if (to.path.toLowerCase() === '/Login'.toLowerCase()) {
       next();
    }
    const tokenStr = sessionStorage.getItem("token");
    if (tokenStr) {
      // if (window.location.href.indexOf("logintoken") != -1) {
      //   //置换新的token
      //   logoto(next);
      // } else {
      //   next();
      // }
      next();
    } else {
      // if (window.location.href.indexOf("logintoken") != -1) {
      //   //置换新的token
      //   logoto(next);
      // } else {
      //   next({ name: 'Login' });
      // }
      logoto(next);
    }
  })

export default router;