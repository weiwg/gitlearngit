/*
 * @Description: 全局混入公用权限检查方法
 * @Author: weig
 * @Date: 2021-07-06 11:05:44
 * @LastEditors: weig
 * @LastEditTime: 2021-08-31 09:34:53
 */

import checkPermission from '@/utils/permission/permission'
import checkView from '@/utils/permission/viewPermission'

export default (app) =>{
  const myMixin = {
    install(app) {
      app.mixin({
        methods: {
          checkPermission,
          checkView,
        }
      })
    }
  }
  
  app.use(myMixin)
}

