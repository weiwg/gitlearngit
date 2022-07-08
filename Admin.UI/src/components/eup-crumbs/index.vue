<!--
 * @Description: 
 * @Author: weig
 * @Date: 2021-10-09 13:40:15
 * @LastEditors: weig
 * @LastEditTime: 2021-10-11 14:18:59
-->
<template>
    <div class="crumbs">
        <el-breadcrumb separator="/">
            <el-breadcrumb-item>
                <i :class="icon"></i> 
                {{firstCrumbs}}
            </el-breadcrumb-item>
            <el-breadcrumb-item>
                {{secondCrumbs}}
            </el-breadcrumb-item>
        </el-breadcrumb>
    </div>
</template>

<script>
import { reactive, toRefs, onBeforeMount, onMounted } from 'vue'
export default {
  name: 'EupCrumbs',
  props:{
    firstCrumbs:{
        type: String,
        default: ""
    },
    secondCrumbs:{
        type: String,
        default: ""
    },
    icon:{
        type: String,
        default:""
    }
  },
  setup(props, context) {
    const data = reactive({});
    onBeforeMount(() => {
    });
    onMounted(() => {
    });
    return {
      ...toRefs(data),
    }
  },
}

</script>
<style scoped lang='less'>
</style>