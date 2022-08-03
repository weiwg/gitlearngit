/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-06-11 15:54:16
 * @LastEditors: weig
 * @LastEditTime: 2021-12-23 17:00:29
 */
import {createApp} from 'vue'
import App from './App.vue'
import store from './store'
import router from './router'

import installElementPlusETC from './plugins/element'
import './assets/css/icon.css'


const app = createApp(App);
installElementPlusETC(app);
app
    .use(store)
    .use(router)
    .mount('#app')


