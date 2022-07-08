/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-07-06 14:59:53
 * @LastEditors: weig
 * @LastEditTime: 2021-12-23 13:49:43
 */

import { getPermissionsPointMenu} from '@/serviceApi/permission/permission'
import { ridUrlParam} from '@/common/js/comm'

const state = {
  name: '',
  avatar: require('@/assets/img/avatar.png'),
  menus: [],
  permissions: [],
  userInfo:{},
  showCard: false,
};

const mutations = {
  setName: (state, name) => {
    state.name = name
  },
  setAvatar: (state, avatar) => {
    if (avatar) {
      state.avatar = process.env.VUE_APP_AVATAR_URL + avatar
    } else {
      state.avatar = require('@/assets/img/avatar.png')
    }
  },
  setMenus: (state, menus) => {
    state.menus = menus;
  },
  setPermissions: (state, permissions) => {
    state.permissions = permissions;
  },
  setShowMenus:(state, value)=>{
    state.showCard = value;
    var data = {
      menus: state.menus,
      permissions: state.permissions,
      showCard: state.showCard
    };
    sessionStorage.setItem("permission", JSON.stringify(data));//缓存权限相关数据，解决vue刷新页面以后丢失store的数据
  },
  /**
   * @description 获取权限点和菜单（同步请求方式）
   * @author weig
   * @param {*} state 
   * @returns 
   */
  getPermissionPointMenuInfo_sync:(state) =>{
    getPermissionsPointMenu().then((res) =>{
      if(res.code == 1){
        state.permissions = res.data.permissions;
        state.menus = res.data.menus;
        state.showCard = true;
        var data = {
          menus: state.menus,
          permissions: state.permissions,
          showCard: state.showCard
        };
        sessionStorage.setItem("permission", JSON.stringify(data));//第一次登录的时候，缓存权限相关数据，解决vue刷新页面以后丢失store的数据
        //开始通过路由加载页面
        window.location.href = ridUrlParam(window.location.href, ["logintoken"]);//跳转到首页，进入路由守卫
      }
    });
  }
};

const actions = {
  /**
   * 获取权限点和菜单（异步请求方式）
   * @param {*} param0  
   */
  getPermissionPointMenuInfo:({commit}) =>{
    getPermissionsPointMenu().then((res) =>{
      if(res.code == 1){
        commit('setPermissions', res.data.permissions);
        commit('setMenus', res.data.menus);
        commit('setShowMenus', true);
      }
    });
  },
};

const modules= {};

const getters = {};

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  modules,
  getters
}
