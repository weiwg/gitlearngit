<!--
 * @Description: 
 * @Author: weig
 * @Date: 2021-05-24 10:24:20
 * @LastEditors: weig
 * @LastEditTime: 2021-12-29 10:22:06
-->
<template>
    <div class="tags" v-if="showTags">
        <ul>
            <li
                class="tags-li"
                v-for="(item,index) in tagsList"
                :class="{'active': isActive(item.path.toLowerCase())}"
                :key="index"
            >
                <router-link :to="item.path" class="tags-li-title">{{item.title}}</router-link>
                <span v-if="tagsList[index].name!='syshome'" class="tags-li-icon" @click="closeTags(index)">
                    <i class="el-icon-close"></i>
                </span>
            </li>
        </ul>
        <div class="tags-close-box">
            <el-dropdown @command="handleTags">
                <el-button size="mini" type="primary">
                    标签选项
                    <i class="el-icon-arrow-down el-icon--right"></i>
                </el-button>
                <template #dropdown>
                    <el-dropdown-menu size="small">
                        <el-dropdown-item command="other">关闭其他</el-dropdown-item>
                        <el-dropdown-item command="all">关闭所有</el-dropdown-item>
                    </el-dropdown-menu>
                </template>
            </el-dropdown>
        </div>
    </div>
</template>

<script>
export default {
    computed: {
        tagsList() {
            this.$store.state.tags.tagsList = sessionStorage.getItem("tagsList") ? JSON.parse(sessionStorage.getItem("tagsList")) : this.$store.getters.tagsList;
            return this.$store.getters.tagsList;
        },
        showTags() {
            return this.tagsList.length > 0;
        }
    },
    methods: {
        isActive(path) {
            return path === this.$route.fullPath.toLowerCase();
        },
        // 关闭单个标签
        closeTags(index) {
            const delItem = this.tagsList[index];
            this.$store.commit("tags/delTagsItem", { index });
            const item = this.tagsList[index]
                ? this.tagsList[index]
                : this.tagsList[index - 1];
            if (item) {
                delItem.path === this.$route.fullPath &&
                    this.$router.push(item.path);
            } else {
                this.$router.push("/");
            }
            sessionStorage.setItem("tagsList", JSON.stringify(this.tagsList));//更新浏览器缓存
        },
        // 关闭全部标签
        closeAll() {
            this.$store.commit("tags/clearTags");
            this.$router.push("/");
            var arr = [{
                name: 'syshome',
                title: '系统首页',
                path: `/syshome`
                }];
            sessionStorage.setItem("tagsList", JSON.stringify(arr));//更新浏览器缓存
        },
        // 关闭其他标签
        closeOther() {
            const curItem = this.tagsList.filter(item => {
                return item.path === this.$route.fullPath;
            });
            if (curItem.length!=0){
                if (curItem[0].title != '系统首页'){
                    //有其他页面tab时
                    curItem.unshift({
                        name: 'syshome',
                        title: '系统首页',
                        path: `/syshome`
                    });
                }
            } else {
                curItem.unshift({
                    name: 'syshome',
                    title: '系统首页',
                    path: `/syshome`
                });
            }

            this.$store.commit("tags/closeTagsOther", curItem);
            sessionStorage.setItem("tagsList", JSON.stringify(this.$store.getters.tagsList));//更新浏览器缓存
        },
        // 设置标签
        setTags(route) {
            const isExist = this.tagsList.some(item => {
                return item.name.toLowerCase() === route.name.toLowerCase();
            });
            if (!isExist) {
                if (this.tagsList.length >= 8) {
                    this.$store.commit("tags/delTagsItem", { index: 1 });
                }
                if (route.fullPath.toLowerCase() != "/Login".toLowerCase() && route.fullPath.toLowerCase() != "/403".toLowerCase()  && route.fullPath.toLowerCase()  != "/404".toLowerCase()){
                    this.$store.commit("tags/setTagsItem",
                    {
                        name: route.name.toLowerCase(),
                        title: route.meta.title,
                        path: route.fullPath.toLowerCase()
                    });        
                    sessionStorage.setItem("tagsList", JSON.stringify(this.$store.getters.tagsList));
                }
            }
        },
        handleTags(command) {
            command === "other" ? this.closeOther() : this.closeAll();
        }
    },
    watch: {
        $route(newValue, oldValue) {
            this.setTags(newValue);
        }
    },
    created() {
        this.setTags(this.$route);
        // 关闭当前页面的标签页
        // this.$store.commit("closeCurrentTag", {
        //     $router: this.$router,
        //     $route: this.$route
        // });
    }
};
</script>
<style>
</style>
