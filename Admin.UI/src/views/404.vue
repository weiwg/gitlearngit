<!--
 * @Description: 
 * @Author: weig
 * @Date: 2021-10-11 14:30:12
 * @LastEditors: weig
 * @LastEditTime: 2021-12-09 12:25:20
-->
<template>
  <div class="error-page">
      <div class="error-code-404">4<span>0</span>4</div>
      <div class="error-desc">您所访问的页面无法找到，请尝试返回网站首页或者上一页</div>
      <div class="error-handle">
          <router-link to="/">
            <el-button type="primary" size="large">返回首页</el-button>
          </router-link>
          <el-button class="error-btn" type="primary" size="large" @click="goBack">返回上一页</el-button>
      </div>
  </div>
</template>

<script>
export default {
  methods: {
      goBack(){
          this.$router.go(-1);
      }
  }
}
</script>
<style scoped>
</style>
