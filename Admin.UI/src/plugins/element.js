/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-06-11 15:54:16
 * @LastEditors: weig
 * @LastEditTime: 2021-12-23 17:45:03
 */
//全局引入,大的依赖组件最好按需引入，打包时文件小
import ElementPlus from 'element-plus'
import { createI18n } from 'vue-i18n'
import 'element-plus/lib/theme-chalk/index.css'
/* import 'vue-cropper/dist/index.css' */
import localeZH from 'element-plus/lib/locale/lang/zh-cn'
import localeEN from 'element-plus/lib/locale/lang/en'
import messages from '../utils/i18n'
import dire_permission from '../directive/index'
import checkPermissionMixin from  '@/mixin'
// console.log(msg)

// const messages = {
//   [localeEN.name]: {
//     el: localeEN.el,
//     i18n: msg.en.i18n,
//   },
//   [localeZH.name]: {
//     el: localeZH.el,
//     i18n: msg.zh.i18n,
//   },
// }

const i18n = createI18n({
  locale: localeZH.name,
  fallbackLocale: localeEN.name,
  messages,
});

export default (app) => {
  //自定义权限指令
  dire_permission(app);
   //混入查看权限方法
  checkPermissionMixin(app);

  app.use(ElementPlus, { locale:localeZH })
    .use(i18n);
}
