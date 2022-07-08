/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-06-30 14:41:16
 * @LastEditors: weig
 * @LastEditTime: 2021-11-01 14:32:00
 */
const Timestamp = new Date().getTime();
const VueLoaderPlugin = require('vue-loader/lib/plugin'); 
module.exports = {
    // publicPath: process.env.NODE_ENV == "production" ? "/" : "./",//发布
    // publicPath: process.env.NODE_ENV == "production" ? "./" : "/",//开发
    publicPath: "/",
    lintOnSave: false, // eslint 是否在保存时检查
    indexPath: 'index.html',
    // 当运行 vue-cli-service build 时生成的生产环境构建文件的目录。注意目标目录在构建之前会被清除 (构建时传入 --no-clean 可关闭该行为)。
    outputDir: 'dist',
    // 放置生成的静态资源 (js、css、img、fonts) 的 (相对于 outputDir 的) 目录
    assetsDir: 'static',
    pwa: {
      iconPaths: {
        favicon32: 'favicon.ico',
        favicon16: 'favicon.ico',
        appleTouchIcon: 'favicon.ico',
        maskIcon: 'favicon.ico',
        msTileImage: 'favicon.ico',
      }
    },
    devServer: {
      open: false,//编译完成是否打开网页
      host: 'localhost',//指定使用地址，默认localhost,0.0.0.0代表可以被外界访问
      port: 9000,//访问端口号  
      proxy: {//设置代理
        '/geoip': {
          target: 'https://ip.seeip.org',
          changeOrigin: true,
          secure: false,
          headers: {
            Referer: 'https://ip.seeip.org'
          }
        },
        '/reverse_geocoding': {
          target: 'https://api.map.baidu.com',
          changeOrigin: true,
          secure: false,
          headers: {
            Referer: 'https://api.map.baidu.com'
          }
        },
        '/v3': {
          target: 'https://restapi.amap.com',
          changeOrigin: true,
          secure: false,
          headers: {
            Referer: 'https://restapi.amap.com'
          }
        },
        '/location': {
          target: 'https://api.map.baidu.com',
          changeOrigin: true,
          secure: false,
          headers: {
            Referer: 'https://api.map.baidu.com'
          }
        },
        '/Api': {
          // target: 'http://locwl.api.eonup.com:18001', //本地
          // target: 'http://wuliu.api.eonup.com:8001', //测试
          // target: 'http://wuliu.api.eonup.com', //正式
          target: process.env.VUE_APP_BASE_API,
          changeOrigin: true,
          ws: true,
        },
        '/img': {
          // target: 'http://locwl.api.eonup.com:18001',//本地
          // target: 'http://wuliu.api.eonup.com:8001', //测试
          // target: 'http://wuliu.api.eonup.com', //正式
          target: process.env.VUE_APP_BASE_API,
          changeOrigin: true,
          ws: true,
        },
      }
    },
    configureWebpack: {
      externals: {
        'AMap': 'AMap' // 高德地图配置
      },
      // output: { // 输出重构  打包编译后的 文件名称  【模块名称.版本号.时间戳】
      //   filename: `TimeStamp/[name].${Timestamp}.js`,
      //   chunkFilename: `TimeStamp/[name].${Timestamp}.js`
      // }
      output: { // 输出重构  打包编译后的 文件名称  【模块名称.版本号.时间戳】
        filename: `TimeStamp/[name].${process.env.VUE_APP_VERSION}.${Timestamp}.js`,
        chunkFilename: `TimeStamp/[name].${process.env.VUE_APP_VERSION}.${Timestamp}.js`
      },
    },   
    chainWebpack: config => {//去掉v-i18n浏览器告警部分
      config.resolve.alias.set('vue-i18n', 'vue-i18n/dist/vue-i18n.cjs.js');
      config.module
      .rule('images')
      .use('url-loader')
      .loader('url-loader')
      .tap(options => Object.assign(options, { limit: 502400 }));
    }
  }