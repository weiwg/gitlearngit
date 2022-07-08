<!--
 * @Description: 
 * @Author: weig
 * @Date: 2021-06-11 15:54:16
 * @LastEditors: weig
 * @LastEditTime: 2021-12-09 11:23:48
-->
<template>
    <div class="about">
        <eup-header />
        <eup-sidebar />
        <div class="content-box" :class="{ 'content-collapse': collapse }">
            <eup-tags></eup-tags>
            <div class="content">
                <router-view v-slot="{ Component }">
                    <transition name="move" mode="out-in">
                        <keep-alive>
                            <component :is="Component" />
                        </keep-alive>
                    </transition>
                </router-view>
            </div>
        </div>
    </div>
</template>
<script>
import EupHeader from "../components/EupHeader";
import EupSidebar from "../components/EupSidebar";
import EupTags from "../components/EupTags.vue";
export default {
    components: {
        EupHeader,
        EupSidebar,
        EupTags
    },
    computed: {
        tagsList() {
            return this.$store.getters.tagsList.map(item => item.name);
        },
        collapse() {
            return this.$store.getters.collapse;
        }
    },
};
</script>
<style>
</style>
