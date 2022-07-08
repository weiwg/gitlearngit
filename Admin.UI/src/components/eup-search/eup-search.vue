<!--
 * @Description: 
 * @Author: weig
 * @Date: 2021-07-27 14:53:01
 * @LastEditors: weig
 * @LastEditTime: 2021-11-23 09:48:22
-->
<template>
  <div>
        <el-input
            v-model="data.filter.value"
            placeholder="请输入"
            class="my-search"
            clearable
            @change="onSearch"
            @clear="onClear"
        >
        <template #prepend>
            <el-select
                v-model="data.filter.field"
                placeholder="请选择"
                style="width:100px;"
                @change="onChange"
            >
            <el-option
                v-for="(o, index) in data.newFields"
                :key="index"
                :label="o.label"
                :value="o.value"
            />
        </el-select>
        </template>
        <template #append>
            <el-button icon="el-icon-search" @click="onSearch" />
        </template>
    </el-input>
  </div>
</template>

<script>
/**
 * 查询组件
 * 使用说明
  import MySearch from '@/components/my-search/my-search'
  components: { MySearch }
  fields: [
    { value: 'userName', label: '用户名', default: true },
    { value: 'nickName', label: '昵称', type: 'string' },
  ],
  <my-search :fields="fields" @click="onSearch" />
*/
import { reactive, toRefs, onBeforeMount, onMounted } from 'vue'
export default {
  name: 'EupSearch',
  props:{
      fields:{
          type: Array,
          default(){
              return [];
          }
      }
  },
  setup(props, context) {
    const data = reactive({
        newFields: [],
        filter:{
            field: "",
            operator: '',
            value: ''
        }
    })
    onBeforeMount(() => {
    });
    onMounted(() => {
        props.fields.forEach(element => {
            if (!element.type || element.type === "string"){
                data.newFields.push(element);
            }
        });
        let fieldValue = "";
        if (data.newFields.length > 0){
            const field = data.newFields.find(a => a.default === true);
            if (field){
                fieldValue = field.value;
            } else {
                fieldValue = newFields[0].value;
            }
        }
        data.filter.field = fieldValue;
        data.filter.operator = 'Equal';
        data.filter.value = '';
    });

    /**
     * @description 切换选项
     * @author weig
     * @param {string} value
     */
    const onChange = (value)=>{
        // data.filter.value = "";
        const dynamicFilter = data.filter.value !== '' ? { ...data.filter } : null;
        context.emit('click', dynamicFilter);
    }

    /**
     * @description 查询
     * @author weig
     * @param
     */
    const onSearch = () =>{
        const dynamicFilter = data.filter.value !== '' ? { ...data.filter } : null;
        context.emit('click', dynamicFilter);
    }

    /**
     * @description 清除查询条件,恢复默认状态
     * @author weig
     */
    const onClear = () =>{
        data.filter = {
            field: "userName",
            operator: 'Equal',
            value: ''
        };
    }
    return {
      data,
      onChange,
      onSearch,
      onClear
    }
  },
}

</script>
<style scoped lang='less'>
</style>