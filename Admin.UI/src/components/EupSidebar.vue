<!--
 * @Description: 
 * @Author: weig
 * @Date: 2021-05-24 10:24:20
 * @LastEditors: weig
 * @LastEditTime: 2021-12-09 12:01:42
-->
<template>
    <div class="sidebar">
        <el-menu
            class="sidebar-el-menu"
            :default-active="onRoutes"
            :collapse="collapse"
            background-color="#324157"
            text-color="#bfcbd9"
            active-text-color="#20a0ff"
            unique-opened
            router
        >
        <template v-if="showCard_func">
            <template v-for="(item, index) in items" :key="index">
                <template v-if="item.children && item.children.length > 0">
                    <!-- 一级菜单 -->
                    <template v-if="item.children && checkView(item.children.map((s) => {return s.index}))">
                        <el-submenu
                            v-if="item.children"
                            :index="item.index.toLowerCase()"
                            :key="item.index"
                        >
                            <template #title>
                                <i :class="item.icon"></i>
                                {{ item.title }}
                            </template>
                            <!-- 二级菜单 -->                                  
                            <template v-for="(subItem, i) in item.children" :key="i">
                                <template v-if="checkView([subItem.index])">    
                                    <el-menu-item 
                                        :key="i"                                            
                                        :index="subItem.index.toLowerCase()">
                                        {{ subItem.title }}
                                    </el-menu-item>
                                </template>
                            </template>
                        </el-submenu>
                    </template>
                    <template v-else>
                        <el-menu-item
                            v-if="!item.children"
                            :index="item.index.toLowerCase()"
                            :key="item.index"
                        >
                            <i :class="item.icon"></i>
                            {{ item.title }}
                        </el-menu-item>
                    </template>
                </template>
                <template v-else>
                    <el-menu-item :index="item.index.toLowerCase()" :key="item.index">
                        <i :class="item.icon"></i>
                        <template #title>{{ item.title }}</template>
                    </el-menu-item>
                </template>
            </template>
        </template>
        </el-menu>
    </div>
</template>

<script>
// import { GetCurrUserInfo} from "@/serviceApi/auth";
import {menu} from "@/config/menuConfig"
export default {
    data() {
        return {
            items: menu,
            // showCard: false,
        };
    },
    computed: {
        onRoutes() {
            return this.$route.path.toLowerCase()//this.$route.path.replace("/", "");//this.$route.path
        },
        collapse(){
            return this.$store.getters.collapse;
        },
        showCard_func(){
            return  this.$store.getters.showCard;//true;//this.$store.getters.showCard;  因为是同步请求模式。所以可以直接加载渲染页面了
        }
    },
    methods:{
    },
    mounted(){
    }
};
</script>
<style scoped>
</style>
