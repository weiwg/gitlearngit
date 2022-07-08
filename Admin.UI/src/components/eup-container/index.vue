<!--
 * @Description: 
 * @Author: weig
 * @Date: 2021-07-14 14:31:04
 * @LastEditors: weig
 * @LastEditTime: 2021-12-09 13:50:35
-->

<template>
    <div>
        <section class="h-per-height">
            <el-container class="eup-container">
                <el-header v-if="showHeader" height="auto" :style="headerStyle">
                    <slot name="header"/>
                </el-header>
                <el-main :style="mainStyle">
                    <slot/>
                </el-main>
                <el-footer v-if="showFooter" :style="footerStyle">
                    <slot name="footer"/>
                </el-footer>
            </el-container>
        </section>
    </div>
</template>

<script>
/**
 * 容器组件
 * 使用说明
<my-container>
  <template #header>
    <el-form />
  </template>
  <el-table />
  <template #footer>
    <my-pagination />
  </template>
</my-container>
 */

export default {
    name: "EupContainer",
    props:{
        showHeader:{
            type: Boolean,
            default: true
        },
        showFooter:{
            type: Boolean,
            default: true
        },
        headerStyle: {
            type: [String, JSON],
            default: 'padding:10px 0px 0px 10px;'
        },
        mainStyle: {
            type: [String, JSON],
            default: 'padding:0px 10px;'
        },
        footerStyle: {
            type: [String, JSON],
            default: 'padding:5px 0px 10px 10px;'
        }

    }
}
</script>
<style scoped>
</style>
