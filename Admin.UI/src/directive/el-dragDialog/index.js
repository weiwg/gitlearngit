/*
 * @Description: 
 * @Author: weig
 * @Date: 2022-08-01 13:59:57
 * @LastEditors: weig
 * @LastEditTime: 2021-08-01 15:40:01
 */
import drag from "./drag";

const install = {
    install: function(app) {
        app.directive('elDragDialog', drag);
    }
}

// if (window.Vue) {
//     window['elDragDialog'] = drag
//     Vue.use(install);
// }
export default install