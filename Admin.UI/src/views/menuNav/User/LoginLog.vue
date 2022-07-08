<template>
    <div>
        <!-- 面包屑 begin -->
        <eup-crumbs firstCrumbs="用户管理" secondCrumbs="登陆日志" icon="el-icon-s-custom" />
        <!-- 面包屑 end -->

        <!-- 内容区域 begin -->
        <div class="container">
            <div class="handle-box">
                <el-form ref="state.query" :model="state.query" :inline="true" v-if="checkPermission([`api${state.VIEW_VERSION}:Record:RecordLogin:GetPage`])">
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
                <el-table-column prop="userName" label="操作账号" min-width="130" align="center"></el-table-column>
                <el-table-column prop="ip" min-width="140" label="IP地址" align="center"></el-table-column>
                <el-table-column prop="browser" min-width="150" label="浏览器" align="center"></el-table-column>
                 <el-table-column prop="os" min-width="100" label="操作系统" align="center"></el-table-column>
                <el-table-column prop="elapsedMilliseconds" label="耗时（毫秒）"  min-width="120" align="center"></el-table-column>
                <el-table-column prop="status" label="登陆状态" min-width="90" align="center">
                <template #default="{row}">
                  <el-tag
                    :type="row.status ? 'success' : 'danger'"
                    disable-transitions
                  >{{ row.status ? '成功' : '失败' }}
                  </el-tag>
                </template>
                </el-table-column>
                <el-table-column prop="msg" label="登陆消息" min-width="160" align="center"></el-table-column>
                <el-table-column prop="createDate" label="登陆时间" min-width="160" align="center"></el-table-column>
                    <el-table-column
                    label="操作"
                    min-width="180"
                    align="center"
                    fixed="right"
                  >
          <template #default="scope">
            <el-button
              type="text"
              icon="el-icon-check"
              @click="handleClickDetails(scope.row)"
              >查看详情</el-button
            >
          </template>
        </el-table-column>
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
       
  <el-dialog
      title="详情"
      v-model="state.detailsVisible"
      width="40%"
    >
      <div class="home-container">
      <el-form ref="form" :model="state.form" label-width="100px">
         
     <el-form-item label="操作账号">
          <el-input v-model="state.form.userName" :readonly="true"/>
    </el-form-item>
        <el-form-item label="IP地址">
          <el-input
            v-model="state.form.ip"
             autocomplete="off"
             :readonly="true"
          />
        </el-form-item>
        <el-form-item label="浏览器">
          <el-input
            v-model="state.form.browser"
             autocomplete="off"
            :readonly="true"
          />
        </el-form-item>
        <el-form-item label="操作系统">
          <el-input
            v-model="state.form.os"
            autocomplete="off"
            :readonly="true"
          />
        </el-form-item>
         <el-form-item label="耗时(毫秒)">
          <el-input
            v-model="state.form.elapsedMilliseconds"
             autocomplete="off"
            :readonly="true"
          />
        </el-form-item>
        
         <el-form-item label="浏览器信息">
          <el-input
            v-model="state.form.browserInfo"
             autocomplete="off"
           :readonly="true"
            :autosize="{ minRows: 2, maxRows: 4 }"
            type="textarea"
          />
        </el-form-item>
      </el-form>
      </div>
    </el-dialog>


    </div>
</template>
<script>
import {reactive,onMounted} from "vue"
import { ElMessage } from 'element-plus'
import { getLoginLogPage} from "@/serviceApi/user/loginLog";
import EupPagination from "../../../components/EupPagination.vue"
import EupCrumbs from "../../../components/eup-crumbs/index.vue"
import EnumConfig from "@/enum/EnumConfig"

var CURR_VIEW_VERSION = EnumConfig.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var VIEW_VERSION = CURR_VIEW_VERSION == 'V0' ? '' : `:S:${CURR_VIEW_VERSION}`;

export default {
    name: "LoginLog",
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
            detailsVisible:false,
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
                "Filter.userName": state.query.nickName,
                "Filter.ip": state.query.ip,
            }
            state.loading = true;
            getLoginLogPage(params).then(function(res){
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
        //查看详情
    const handleClickDetails = (row) => {
      state.detailsVisible = true;
      state.form=row;
    };
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
            handleClickDetails,
            resPageData
        }
    }
};
</script>
<style scoped>
</style>
