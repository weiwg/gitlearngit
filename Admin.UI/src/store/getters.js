/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-07-06 10:39:51
 * @LastEditors: weig
 * @LastEditTime: 2021-12-08 16:04:16
 */

const getters = {
    tagsList: state => state.tags.tagsList,
    collapse: state => state.tags.collapse,
    menus: state => state.user.menus,
    permissions: state => state.user.permissions,
    userName: state => state.user.name,
    avatar: state => state.user.avatar,
    userInfo: state => JSON.parse(sessionStorage.getItem("userInfo")),
    showCard: state => state.user.showCard
  }
  export default getters
  
