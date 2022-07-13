<!--
 * @Description: 
 * @Author: weig
 * @Date: 2021-08-20 16:12:56
 * @LastEditors: weig
 * @LastEditTime: 2021-10-19 16:36:44
-->
<template>
  <div>  
    <div @click="clickText">谈{{text}}</div>  
    <div @click="clickName">{{name}}</div> 
  </div>
</template>

<script>
import { reactive, toRefs, onBeforeMount, onMounted,onActivated } from 'vue'
export default {
  name: '',
  setup() {
    console.log('1-开始创建组件-setup')
    const data = reactive({
        text: 1,
        name: 1
    })

    onBeforeMount(() => {
      console.log('2.组件挂载页面之前执行----onBeforeMount')
    })
    onMounted(() => {
      console.log('3.-组件挂载到页面之后执行-------onMounted')
      // data.text = 2
      // data.name = 3
      console.log(`${data.text },${data.name}`)
    }),
    onActivated(()=>{
      console.log(`${data.text },${data.name}`)
    })

    const clickText= ()=>{
      data.text++;
    }
    const clickName= ()=>{
      data.name++;
    }
    return {
      ...toRefs(data),
      clickText,
      clickName
    }
  },
}

</script>
<style scoped lang='less'>
</style>