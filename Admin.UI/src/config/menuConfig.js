/*
 * @Description: 
 * @Author: weig
 * @Date: 2021-10-19 09:39:03
 * @LastEditors: weig
 * @LastEditTime: 2021-12-29 10:21:02
 */
export const menu = [
    {
        icon: "el-icon-s-home",
        index: `/SysHome`,
        title: "系统首页",
    },
    {
        icon: "el-icon-s-custom",
        index: "2-1",
        title: "用户管理",
        children:[
            {
                index: `/User/UserList`,
                title: "用户列表",
            },
            {
                index: `/User/LoginLog`,
                title: "登陆日志",
            },
            {
                index: `/User/OperLog`,
                title: "操作日志",
            }
        ]
    },
    {
        icon: "el-icon-s-management",
        index: "2-2",
        title: "系统管理",
        children:[
            {
                index: `/System/AreaManage`,
                title: "地区管理",
            },
            {
                index: `/System/Tenant`,
                title: "租户管理",
            },
            {
                index: `/System/Cache`,
                title: "缓存管理",
            }
        ]
    },
    {   
        icon: "el-icon-setting",
        index: "2-3",
        title: "权限管理",
        children:[
            {
                index: `/Permission/UserRole`,
                title: "用户角色",
            },
            {
                index: `/Permission/Role`,
                title: "角色管理",
            },
            {
                index: `/Permission/Interface`,
                title: "接口管理",
            },
            {
                index: `/Permission/ViewManage`,
                title: "视图管理",
            },
            {
                index: `/Permission/PermissionManage`,
                title: "权限管理",
            },
            // {
            //     index: `/Permission/Role-Permission`,
            //     title: "角色权限",
            // },
        ]
    },
    {   
        icon: "el-icon-picture-outline",
        index: "2-4",
        title: "资源管理",
        children:[
            {
                index: `/Image/Image`,
                title: "图片中心",
            },
        ]
    },
    {   
        icon: "el-icon-bangzhu",
        index: "2-5",
        title: "异常管理",
        children:[
            {
                index: `/Abnormal/List`,
                title: "异常列表",
            },
        ]
    },

    // {   
    //   icon: "el-icon-star-off",
    //     index: `/Demo`,
    //     title: "测试Demo",
    // }
];
