<!--
 * @Description: 
 * @Author: weig
 * @Date: 2021-06-11 15:54:16
 * @LastEditors: weig
 * @LastEditTime: 2021-12-20 18:00:04
-->
<template>
<div></div>
</template>
<script>
import { reactive, onMounted,onBeforeMount } from "vue";
export default {
  setup() {
    onBeforeMount(()=>{
      var href = `${process.env.VUE_APP_LOGIN_URL}?returnurl=` + window.location.protocol + "//" + window.location.hostname + ":" + window.location.port;
      window.location.href = href;
    })
    return {}
  }
};
</script>