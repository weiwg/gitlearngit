/*
 * @Description: tab页签状态管理
 * @Author: weig
 * @Date: 2021-07-06 15:42:46
 * @LastEditors: weig
 * @LastEditTime: 2021-12-29 11:24:26
 */
const state = {
    tagsList: [
        {
            name: 'syshome',
            title: '系统首页',
            path: `/syshome`
        }
    ],//默认固定显示首页
    collapse: false
};

const mutations = {
    delTagsItem(state, data) {
        state
            .tagsList
            .splice(data.index, 1);
    },
    setTagsItem(state, data) {
        state
            .tagsList
            .push(data)
    },
    clearTags(state) {
        //清理tab时保留系统首页
        state.tagsList = [            
            {
                name: 'syshome',
                title: '系统首页',
                path: `/syshome`
            }]
    },
    closeTagsOther(state, data) {
        state.tagsList = data;
    },
    closeCurrentTag(state, data) {
        for (let i = 0, len = state.tagsList.length; i < len; i++) {
            const item = state.tagsList[i];
            if (item.path === data.$route.fullPath) {
                if (i < len - 1) {
                    data
                        .$router
                        .push(state.tagsList[i + 1].path);
                } else if (i > 0) {
                    data
                        .$router
                        .push(state.tagsList[i - 1].path);
                } else {
                    data
                        .$router
                        .push("/");
                }
                state
                    .tagsList
                    .splice(i, 1);
                break;
            }
        }
    },
    // 侧边栏折叠
    hadndleCollapse(state, data) {
        state.collapse = data;
    }
};

const actions = {};

const modules= {};

const getters = {};

export default{
    namespaced: true,
    state,
    mutations,
    actions,
    modules,
    getters
}

