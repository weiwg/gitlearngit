/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-06-11 15:54:16
 * @LastEditors: weig
 * @LastEditTime: 2021-09-23 10:36:51
 */
import axios from 'axios'
import { Toast } from 'vant'
import { IsNullOrEmpty } from '@/common/js/comm'

axios.defaults.headers['Content-Type'] = 'application/json;charset=utf-8'
axios.defaults.headers['Cache-Control'] = 'no-cache'
axios.defaults.headers['Pragma'] = 'no-cache'

const requestAxios = axios.create({
    baseURL: 'http://localhost:9000',
    timeout: 2000000
})
// 拦截请求
requestAxios.interceptors.request.use(
    config => {
        return config
    },
    err => {
        return Promise.reject(err)
    }
)

//请求响应
requestAxios.interceptors.response.use(res => {
    if (res.status != 200) {
        return Promise.reject(res)
    }
    return res.data
},
    error => {
        const { response } = error
        if (response) {
            switch (response.status) {
                case 401://服务器拒绝执行，一般token或session过期 
                    Toast.fail('401')
                    break
                case 403://权限不足
                    Toast.fail('403')
                    break
                case 404://找不到地址
                    Toast.fail('404')
                    break
                default:
                    Toast.fail('未知异常')
                    router.push({ name: "Login" });
            }
        }
        else {
            //服务器连结果都没有返回
            if (!window.navigator.onLine) {
                //客户端断网了，将页面跳转到一个断网的页面，可以结合路由使用
                Toast.fail('网络中断，重新登录')
                router.push({ name: "Login" });
                return
            }
        }
        return Promise.reject(error)
    })
export default requestAxios