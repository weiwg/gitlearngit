/*
 * @Description: 
 * @Author: weig
 * @Date: 2022-08-01 11:59:57
 * @LastEditors: weig
 * @LastEditTime: 2021-08-01 13:40:01
 */
// 引入拖拽js
import { startDrag } from './drag2.js'
/**
 * 为el-dialog弹框增加拖拽功能
 * @param {*} el 指定dom
 * @param {*} binding 绑定对象
 * desc   只要用到了el-dialog的组件，都可以通过增加v-draggable属性变为可拖拽的弹框
 */
const draggable = (el, binding) => {
    // 绑定拖拽事件 [绑定拖拽触发元素为弹框头部、拖拽移动元素为整个弹框]
    startDrag(el.querySelector('.el-dialog__header'), el.querySelector('.el-dialog'), binding.value);
};
 
const directives = {
    draggable,
};
// 这种写法可以批量注册指令
export default {
  install(Vue) {
      Object.keys(directives).forEach((key) => {
      Vue.directive(key, directives[key]);
    });
  },
};