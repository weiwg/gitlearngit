<!--
 * @Description: 
 * @Author: weig
 * @Date: 2021-09-01 20:15:04
 * @LastEditors: weig
 * @LastEditTime: 2021-12-17 09:48:22
-->
<template>
    <div>
        <!-- 面包屑 begin -->
        <eup-crumbs icon="el-icon-s-management" firstCrumbs="缓存管理" secondCrumbs="缓存列表" />
        <!-- 面包屑 end -->

        <!-- 内容区域 begin -->
        <div class="container">
            <!-- 列表 -->
            <el-table
                :data="state.tableData"
                border
                class="table"
                ref="multipleTable"
                v-loading="state.loading"
            >
                <el-table-column prop="num" label="序号" width="60" align="center"></el-table-column>
                <el-table-column prop="description" min-width="300" label="缓存名" align="center"></el-table-column>
                <el-table-column prop="name" label="键名" min-width="300" align="center"></el-table-column>
                <el-table-column prop="value" label="键值" min-width="300" align="center"></el-table-column>
                <el-table-column label="操作" min-width="180" align="center" fixed="right" v-if="checkPermission([`api${state.VIEW_VERSION}:system:cache:clear`])">
                    <template #default="{row}">
                        <el-button
                            type="danger"
                            icon="el-icon-delete"
                            @click="handleClickDelete(row)"
                            class="ml5"
                        >删除
                        </el-button>
                    </template>
                </el-table-column>
            </el-table>
        </div>  
        <!-- 内容区域 end --> 
    </div>
</template>
<script>
import { reactive, onBeforeMount, onMounted,ref } from 'vue'
import { getCachePageList, ClearCache} from '@/serviceApi/system/cache'
import { ElMessage,ElMessageBox } from 'element-plus'
import {useStore} from 'vuex'
import {elConfirmDialog} from "@/common/js/comm"
import EupCrumbs from "../../../components/eup-crumbs/index.vue"
import EnumConfig from "@/enum/EnumConfig"

var CURR_VIEW_VERSION = EnumConfig.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var VIEW_VERSION = CURR_VIEW_VERSION == 'V0' ? '' : `:S:${CURR_VIEW_VERSION}`;

export default {
  name: 'Cache',
  components:{
      EupCrumbs: EupCrumbs
  },
  setup(props, context) {
    const refAddForm = ref(null);
    const state = reactive({
        VIEW_VERSION: VIEW_VERSION,
        pageIndex: 1,
        pageSize: 10,
        tableData: [],
        multipleSelection: [],
        pageTotal: 0,
        dynamicFilter:{},
        idx: -1,
        loading: false,
        refForm: null,
        store: {},

    });
    onBeforeMount(() => {
    });
    onMounted(() => {
        state.store=useStore();
        getData();
    });
    /**
     * @description 获取缓存列表信息
     * @author weig
     * @param
     */
    function getData (){
        state.loading = true;
        getCachePageList().then(function(res){
            if(res.code == 1){
                state.pageTotal = res.data.length;//初始化列表数据总数
                state.tableData = res.data;
                //添加num序号字段
                state.tableData.forEach((data, i) => {
                    data.num = i + 1;
                });
            } else {
                ElMessage.error(res.msg);   
            }
            state.loading = false;
        });
    }
    
    /**
     * @description 多选操作
     * @author weig
     * @param
     */
    const handleSelectionChange =(val)=> {
        state.multipleSelection = val;
    }

    /**
     * @description 清除缓存
     * @author weig
     * @param {Object} row 当前行数据
     */
    function handleClickDelete(row){
        var param = { cacheKey: row.value };
        elConfirmDialog(ElMessageBox,'此操作将永久删除该数据, 是否继续?','提示', '', ()=>{
            ClearCache(param.cacheKey).then(res =>{
                if (res.code == 1){
                    ElMessage.success("删除成功");
                    getData();
                } else {
                    ElMessage.error("删除失败！");
                }
            });
        }, ()=>{
            ElMessage.info("取消删除！");
        });
    }
    return {
      state,
      handleSelectionChange,
      getData,
      refAddForm,
      handleClickDelete
    }
  },
}
</script>
<style scoped>
</style>