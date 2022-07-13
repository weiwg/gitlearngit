/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-10-18 14:33:12
 * @LastEditors: weig
 * @LastEditTime: 2021-12-29 10:20:22
 */
import Home from "../views/Home.vue";
import SysHome from "../views/menuNav/SysHome.vue";
import Userlist from "../views/menuNav/User/UserList.vue";
import LoginLog from "../views/menuNav/User/LoginLog.vue";
import OperLog from "../views/menuNav/User/OperLog.vue";
import AreaManage from "../views/menuNav/System/AreaManage.vue";

export const routes = [
    {
        path: "/",
        name: "Home",
        component: Home,
        redirect: `/SysHome`, 
        caseSensitive: false, 
        children: [
            {
                path: `/Syshome`,
                name: "SysHome",
                meta: {
                    title: '系统首页',
                    permission: true
                },
                component: SysHome,
                caseSensitive: false
            },
            {
                path: `/User/UserList`,
                name: "UserList",
                meta: {
                    title: '用户列表',
                    permission: true
                },
                component: Userlist,
                caseSensitive: false
            },
            {
                path: `/User/LoginLog`,
                name: "LoginLog",
                meta: {
                    title: '登陆日志',
                    permission: true
                },
                component: LoginLog,
                caseSensitive: false
            },
            {
                path: `/User/OperLog`,
                name: "OperLog",
                meta: {
                    title: '操作日志',
                    permission: true
                },
                component: OperLog,
                caseSensitive: false
            },
            {
                path: `/System/AreaManage`,
                name: "AreaManage",
                meta: {
                    title: '地区管理',
                    permission: true
                },
                component: AreaManage,
                caseSensitive: false
            },
            {
                path: `/System/Tenant`,
                name: "Tenant",
                meta: {
                    title: '租户管理',
                    permission: true
                },
                component: () => import ("../views/menuNav/System/Tenant.vue"),
                caseSensitive: false
            },
            {
                path: `/System/Cache`,
                name: "Cache",
                meta: {
                    title: '缓存管理',
                    permission: true
                },
                component: () => import ("../views/menuNav/System/Cache.vue"),
                caseSensitive: false
            },
            {
                path: `/Permission/Role`,
                name: "Role",
                meta: {
                    title: '角色管理',
                    permission: true
                },
                component: () => import (
                "../views/menuNav/Permission/Role.vue"),
                caseSensitive: false
            }, 
            {
                path: `/Permission/UserRole`,
                name: "UserRole",
                meta: {
                    title: '用户角色',
                    permission: true
                },
                component: () => import ("../views/menuNav/Permission/UserRole.vue"),
                caseSensitive: false
            },
            {
                path: `/Permission/Interface`,
                name: "Interface",
                meta: {
                    title: '接口管理',
                    permission: true
                },
                component: () => import ("../views/menuNav/Permission/Api.vue"),
                caseSensitive: false
            },
            {
                path: `/Permission/ViewManage`,
                name: "ViewManage",
                meta: {
                    title: '视图管理',
                    permission: true
                },
                component: () => import ("../views/menuNav/Permission/ViewManage.vue"),
                caseSensitive: false
            },
            {
                path: `/Permission/PermissionManage`,
                name: "PermissionManage",
                meta: {
                    title: '权限管理',
                    permission: true
                },
                component: () => import ("../views/menuNav/Permission/PermissionManage.vue"),
                caseSensitive: false
            },
            // {
            //     path: `/Permission/Role-Permission`,
            //     name: "RolePermission",
            //     meta: {
            //         title: '角色权限',
            //         permission: true
            //     },
            //     component: () => import ("../views/menuNav/Permission/Role-Permission.vue")
            // },
            {
                path:`/Image/Image`,
                name:"Image",
                component: () => import ("../views/menuNav/Image/Image.vue"),
                meta:{
                    title:'图片中心',
                    permission: true
                },
                caseSensitive: false
             },
             {
                path:`/Abnormal/List`,
                name:"AbnormalList",
                component: () => import ("../views/menuNav/Abnormal/List.vue"),
                meta:{
                    title:'异常列表',
                    permission: true
                },
                caseSensitive: false
             }             
        ]
    },
    // {
    //     path: "/Login",
    //     name: "Login",
    //     meta: {
    //         title: '登录',
    //         permission: true
    //     },
        
    //     component: () => import (/* webpackChunkName: "login" */"../views/Account/Login.vue"),
    //     caseSensitive: false
    // },
    {
        path: "/Login",
        name: "Login",
        meta: {
            title: '登录',
            permission: true
        },
        
        component: () => import (/* webpackChunkName: "login" */"../views/Account/LYLogin.vue"),
        caseSensitive: false
    },
    {
        path: `/Demo`,
        name: "Demo",
        meta: {
            title: '测试Demo',
            permission: true
        },
        component: () => import ("../views/menuNav/Demo.vue"),
        caseSensitive: false
    },
    {
        path: "/i18n",
        name: "i18n",
        meta: {
            title: '国际化语言',
            permission: true
        },
        component: () => import (
        /* webpackChunkName: "i18n" */
        "../views/I18n.vue"),
        caseSensitive: false
    }, {
        path: "/upload",
        name: "upload",
        meta: {
            title: '上传插件',
            permission: true
        },
        component: () => import (
        /* webpackChunkName: "upload" */
        "../views/Upload.vue"),
        caseSensitive: false
    }, {
        path: "/icon",
        name: "icon",
        meta: {
            title: '自定义图标',
            permission: true
        },
        component: () => import (
        /* webpackChunkName: "icon" */
        "../views/Icon.vue"),
        caseSensitive: false
    }, {
        path: '/404',
        name: '404',
        meta: {
            title: '找不到页面',
            permission: true
        },
        component: () => import (/* webpackChunkName: "404" */
        '../views/404.vue'),
        caseSensitive: false
    }, {
        path: '/403',
        name: '403',
        meta: {
            title: '没有权限',
            permission: true
        },
        component: () => import (/* webpackChunkName: "403" */
        '../views/403.vue'),
        caseSensitive: false
    },
];