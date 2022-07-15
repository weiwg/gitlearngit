<template>
    <div>
        <!-- 面包屑 begin -->
        <eup-crumbs icon="el-icon-s-custom" firstCrumbs="用户管理" secondCrumbs="操作日志" />
        <!-- 面包屑 end -->

        <!-- 内容区域 begin -->
        <div class="container">
            <div class="handle-box">
                <el-form ref="state.query" :model="state.query" :inline="true" v-if="checkPermission([`api${state.VIEW_VERSION}:Record:RecordOperation:GetPage`])">
                    <el-form-item label="账号">
                        <el-input v-model="state.query.nickName" placeholder="账号" class="handle-input mr10"></el-input>
                    </el-form-item>
                    <el-form-item label="IP地址">
                        <el-input v-model="state.query.ip" placeholder="IP地址" class="handle-input mr10"></el-input>
                    </el-form-item>
                    <el-button type="primary" icon="el-icon-search" @click="handleSearch">搜索</el-button>
                </el-form>
            </div>
            <el-table
                :data="state.tableData"
                border
                class="table"
                ref="multipleTable"
                header-cell-class-name="table-header"
                @selection-change="handleSelectionChange"
                v-loading="state.loading"
            >
                <el-table-column type="selection" width="60" align="center"></el-table-column>
                <el-table-column prop="num" label="序号" width="60" align="center"></el-table-column>
                <el-table-column prop="userName" label="操作账号" align="center" min-width="130"></el-table-column>
                <el-table-column prop="ip" label="IP地址" align="center" width="140"></el-table-column>
                <el-table-column prop="apiLabel" label="接口名称" align="center" min-width="130"></el-table-column>
                <el-table-column prop="apiPath" label="接口地址" align="center" min-width="150"></el-table-column>
                <el-table-column prop="elapsedMilliseconds" label="耗时（毫秒）"  min-width="110" align="center"></el-table-column>
                <el-table-column prop="status" label="操作状态" min-width="80" align="center">
                <template #default="{row}">
                  <el-tagW
                    :type="row.status ? 'success' : 'danger'"
                    disable-transitions
                  >{{ row.status ? '成功' : '失败' }}
                  </el-tagW>
                </template>
                </el-table-column>
                <el-table-column prop="msg" label="操作消息" min-width="120" align="center"></el-table-column>
                <el-table-column prop="createDate" label="操作时间" align="center" min-width="160" fixed="right"></el-table-column>
            </el-table>
            <!-- 分页 begin -->
            <EupPagination
                :current-page="state.pageIndex"
                :pagesizes="[10,20,50,100]"
                :pagesize="state.pageSize"
                :total="state.pageTotal"
                @getPageData="getData"
                @resPageData="resPageData">
            </EupPagination>
            <!-- 分页 end -->
        </div>
        <!-- 内容区域 end -->
    </div>
</template>
<script>
import {reactive,onMounted} from "vue"
import { ElMessage } from 'element-plus'
import { getOprationLogPage} from "@/serviceApi/user/operLog";
import EupPagination from "../../../components/EupPagination.vue"
import EupCrumbs from "../../../components/eup-crumbs/index.vue"
import EnumConfig from "@/enum/EnumConfig"

var CURR_VIEW_VERSION = EnumConfig.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var VIEW_VERSION = CURR_VIEW_VERSION == 'V0' ? '' : `:S:${CURR_VIEW_VERSION}`;

export default {
    name: "OperLog",
    components: {
        EupPagination: EupPagination,
        EupCrumbs: EupCrumbs
    },
    setup(){
        const state = reactive({
            VIEW_VERSION: VIEW_VERSION,
            query: {
                nickName: "",
                ip: ""
            },
            pageIndex: 1,
            pageSize: 10,
            tableData: [],
            multipleSelection: [],
            delList: [],
            pageTotal: 0,
            form: {},
            dynamicFilter:{},
            loading: false
        });
        onMounted(()=>{
            getData();
        })
        //获取表单信息
        const getData=()=>{
            var params = {
                "currentPage": state.pageIndex,
                "pageSize": state.pageSize,
                "filter.userName": state.query.nickName, 
                "filter.ip": state.query.ip,
                "dynamicFilter": state.dynamicFilter
            }
            state.loading = true;
            getOprationLogPage(params).then(function(res){
                if(res.code == 1){
                state.pageTotal = res.data.total;//初始化列表数据总数
                state.tableData = res.data.list;
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
        // 多选操作
        const handleSelectionChange =(val)=> {
            state.multipleSelection = val;
        }
        // 触发搜索按钮
        const handleSearch=()=> {
            state.pageIndex = 1
            getData();
        }

        /**
         * @description 子组件返回分页数据
         * @author weig
         * @param {Object} obj
         */
        const resPageData = (obj) =>{
            state.pageIndex = obj.currPage;
            state.pageSize = obj.pageSize;
        }
        return {
            state,
            handleSelectionChange,
            handleSearch,
            getData,
            resPageData
        }
    }
};
</script>
<style scoped>
</style>
