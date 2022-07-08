<!--
 * @Description: 
 * @Author: weig
 * @Date: 2021-07-27 14:35:04
 * @LastEditors: weig
 * @LastEditTime: 2021-12-17 09:47:10
-->
<template>
  <div>
        <!-- 面包屑 begin -->
        <eup-crumbs icon="el-icon-setting" firstCrumbs="角色管理" secondCrumbs="用户角色列表" />
        <!-- 面包屑 end -->
        <!-- 内容区域 begin -->
        <div class="container">
            <!-- 查询 -->
            <div class="handle-box">
                <el-form :inline="true" v-if="checkPermission([`api${state.VIEW_VERSION}:auth:userrole:getuserrolelist`,`api${state.VIEW_VERSION}:auth:userrole:adduserroleinfo`,`api${state.VIEW_VERSION}:auth:userrole:batchsoftdelete`])">
                    <el-form-item v-if="checkPermission([`api${state.VIEW_VERSION}:auth:userrole:getuserrolelist`])">
                        <eup-search :fields="state.fields" @click="onSearch" />
                    </el-form-item>
                    <el-form-item>
                    </el-form-item>
                    <el-form-item >
                    <el-button type="primary" icon="el-icon-plus" @click="handleClickAddData" v-if="checkPermission([`api${state.VIEW_VERSION}:auth:userrole:adduserroleinfo`])">新增</el-button>
                        </el-form-item>
                        <el-form-item >
                        <el-button
                            type="danger"
                            icon="el-icon-delete"
                            class="handle-del mr10"
                            @click="handleClickBatchDelete"
                            :disabled="state.sels.length === 0"
                            v-if="checkPermission([`api${state.VIEW_VERSION}:auth:userrole:batchsoftdelete`])"
                            >批量删除
                    </el-button>
                    </el-form-item>
                </el-form>
            </div>

            <!-- 列表 -->
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
                <el-table-column prop="userName" label="用户名" min-width="100" align="center"></el-table-column>
                <el-table-column prop="nickName" label="昵称" min-width="150" align="center"></el-table-column>
                <el-table-column prop="roleName" label="角色" min-width="100" align="center"></el-table-column>
                <el-table-column prop="phone" label="手机号" min-width="110" align="center"></el-table-column>
                <el-table-column prop="email" label="邮箱" min-width="160" align="center"></el-table-column>
                <el-table-column prop="createDate" label="创建时间" min-width="160" align="center"></el-table-column>
                <el-table-column label="操作" min-width="240" align="center" fixed="right" v-if="checkPermission([`api${state.VIEW_VERSION}:auth:userrole:softdelete`])">
                    <template #default="{$index, row }">
                        <template v-if="row.roleType != state.EnumConfig.EnumConfig.RoleType.SuperAdmin">
                            <el-button
                                type="primary"
                                icon="el-icon-plus"
                                @click="handleAddRoleToUser($index,row)"
                            >新增角色</el-button>
                            <el-button
                                type="danger"
                                icon="el-icon-delete"
                                @click="handleClickDelete(row)"
                                v-if="checkPermission([`api${state.VIEW_VERSION}:auth:userrole:softdelete`])"
                                class="ml5"
                            >删除</el-button>
                        </template>
                    </template>
                </el-table-column>
            </el-table>

            <!-- 分页 begin-->
            <EupPagination
                :current-page="state.pageIndex"
                :pagesizes="[10,20,50,100]"
                :pagesize="state.pageSize"
                :total="state.pageTotal"
                @getPageData="getData"
                @resPageData="resPageData">
            </EupPagination>
            <!-- 分页 end-->
        </div>
        <!-- 内容区域 end -->
        
        <!-- 添加/编辑窗口 begin -->
        <el-dialog 
          :title="state.dialogTitle"
          v-model="state.addDialogFormVisible"
          width="60%"
          @close="closeEditForm()">
          <el-form
            ref="addForm"
            :model="state.form"
            :rules="state.addFormRules"
            label-width="100px"
            :inline="false"
          >
            <el-row>
              <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
              <el-form-item label="手机号/邮箱" prop="userName">
                <el-select 
                  v-model="state.form.userId" 
                  filterable 
                  placeholder="请输入手机号/邮箱"
                  :remote-method="remoteHandle"
                  remote
                  :clearable="true"
                  @clear="onClearDialog"
                  :disabled="state.userRoleCondition"
                  >
                  <el-option
                    v-for="item in state.options"
                    :key="item.userId"
                    :label="item.nickName"
                    :value="item.userId">
                  </el-option>
                </el-select>
              </el-form-item>
              </el-col>  
              <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                  <el-form-item label="角色名称" prop="roleName">
                      <el-select v-model="state.form.roleId" placeholder="请选择">
                          <el-option
                          v-for="item in state.roleInfo"
                          :key="item.roleId"
                          :label="item.roleName"
                          :value="item.roleId"
                          :disabled='item.roleType ==999 ? true : false'
                          >
                          </el-option>
                      </el-select>
                  </el-form-item>
              </el-col>     
            </el-row>
          </el-form>
          <template #footer>
            <span class="dialog-footer">
              <el-button @click="closeEditForm()">取 消</el-button>
              <el-button type="primary" @click="addDialogFormSave(this)">确 定</el-button>
            </span>
          </template>
        </el-dialog>
        <!-- 添加/编辑窗口 end -->
  </div>

</template>

<script>
import { reactive, toRefs, onBeforeMount, onMounted,ref } from 'vue'
import { getUserRoleListPage, addUserRole, editUserRole, removeUserRole, batchRemoveUserRole, getRoleInfo, getUserInfo} from '@/serviceApi/permission/userRole'
import EupSearch from '@/components/eup-search/eup-search'
import EupPagination from "@/components/EupPagination.vue"
import {elConfirmDialog, IsNullOrEmpty} from "@/common/js/comm"
import { ElMessage,ElMessageBox } from 'element-plus'
import {useStore} from 'vuex'
import EupCrumbs from "../../../components/eup-crumbs/index.vue"
import Enum from "@/enum/EnumConfig"

var CURR_VIEW_VERSION = Enum.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var VIEW_VERSION = CURR_VIEW_VERSION == 'V0' ? '' : `:S:${CURR_VIEW_VERSION}`;
export default {
  name: 'UserRole',
  components:{
      EupSearch: EupSearch,
      EupPagination: EupPagination,
      EupCrumbs: EupCrumbs
  },
  setup(props, context) {
    const addForm = ref(null);
    const state = reactive({
        fields:[
            { value: 'userName', label: '用户名', default: true },
            { value: 'nickName', label: '昵称', type: 'string' },
            { value: 'phone', label: '手机号', type: 'string' },
            { value: 'email', label: '邮箱', type: 'string' },
        ],
        VIEW_VERSION: VIEW_VERSION,
        query: {
            userName: "",
            nickName:"",
            phone:"",
            email: ""
        },
        pageIndex: 1,
        pageSize: 10,
        tableData: [],
        multipleSelection: [],
        delList: [],
        pageTotal: 0,
        dynamicFilter:{},
        form: {
            userId: "",
            roleId: "",
            
            userRoleId: "",
            createUserId: "",
            updateUserId: ""
        },
        idx: -1,
        loading: false,
        refForm: null,
        dialogTitle: "",
        addDialogFormVisible: false,
        roleInfo: [],//角色信息
        options: [],//用户信息
        dialogType: 0, //0：编辑   1:新增
        store: {},
        addFormRules:{
            userName: [
                { required: true, message: '用户名称不饿能为空', trigger: 'blur' }
            ],
            roleName: [
                { required: true, message: '角色名称不饿能为空', trigger: 'blur' }
            ],
        },
        sels: [],
        userRoleCondition: false,
        EnumConfig: Enum,

    });
    onBeforeMount(() => {});
    onMounted(() => {
        getData();
        getRoleInfoList();
        state.store = useStore();
    });

    /**
     * @description 查询
     * @author weig
     * @param {Object} queryValue 动态查询字段
     */
    function onSearch(queryValue){
        state.pageIndex = 1;
        if (queryValue){
            if (queryValue.field){
                if (queryValue.field == "userName"){
                    state.query.nickName = "";
                    state.query.phone = "";
                    state.query.email = "";
                    state.query.userName = queryValue.value;
                } else if(queryValue.field == "nickName") {
                    state.query.nickName = queryValue.value;
                    state.query.userName = "";
                    state.query.phone = "";
                    state.query.email = "";
                } else if(queryValue.field == "phone") {
                    state.query.phone = queryValue.value;
                    state.query.userName = "";
                    state.query.nickName = "";
                    state.query.email = "";
                } else {
                    state.query.email = queryValue.value;
                    state.query.userName = "";
                    state.query.nickName = "";
                    state.query.phone = "";
                }
                getData();
            }
        } else {
            state.query.userName= "";
            state.query.nickName= "";
            state.query.phone = "";
            state.query.email = "";
            getData();
        }
    }

    /**
     * @description 获取表单信息
     * @author weig
     * @param
     */
    const getData =()=>{
        var params = {
            "currentPage": state.pageIndex,
            "pageSize": state.pageSize,
            "filter.userName": state.query.userName,
            "filter.nickName": state.query.nickName,
            "filter.phone": state.query.phone,
            "filter.email": state.query.email,
            "dynamicFilter": state.dynamicFilter
        }
        state.loading = true;
        getUserRoleListPage(params).then(function(res){
            if(res.code == 1){
                state.pageTotal = res.data.total;//初始化列表数据总数
                state.tableData = res.data.list;
                //添加num序号字段
                state.tableData.forEach((data, i) => {
                    data.num = i + 1;
                });
                // state.deleteImgData.isDelete = false;
                // state.deleteImgData.rowId = 0;
            } else {
                ElMessage.error(res.msg);   
            }
            state.loading = false;
            state.sels = [];
        });
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

    /**
     * @description 多选操作
     * @author weig
     * @param
     */
    const handleSelectionChange =(val)=> {
        state.multipleSelection = val;
        state.sels = state.multipleSelection;
    }

    /**
     * @description 新增
     * @author weig
     * @param
     */
    function handleClickAddData (){
        state.addDialogFormVisible = true;
        state.dialogTitle = "新增用户角色";
        state.dialogType = 1;
        state.userRoleCondition = false;
    }

    /**
     * @description 编辑
     * @author weig
     * @param {Number} index 行号
     * @param {Object} row 行数据
     */
    const handleEdit = (index, row)=>{
        let Json = JSON.parse(JSON.stringify(row));//深拷贝
        state.addDialogFormVisible = true;
        state.dialogTitle = "编辑用户角色";
        state.dialogType = 0;
        state.form.userId = Json.userId;
        state.options = [];
        state.options.push(Json);
        state.form.roleId = Json.roleId;
        state.form.userRoleId = Json.userRoleId;
        state.form.updateUserId = state.store.getters.userInfo.userId;
    }

    /**
     * @description 批量删除
     * @author weig
     * @param
     */
    const handleClickBatchDelete = ()=>{
        if (state.multipleSelection.length == 0){//未选中
            ElMessage.error("请选择要删除的数据！");
        } else {
            var Ids = state.multipleSelection.map(s =>{
              return s.userRoleId;
            });
            ElMessageBox.confirm('此操作将删除选中的记录, 是否继续?', '提示',{
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(()=>{
                batchRemoveUserRole(Ids).then(res=>{
                    if (res.code == 1){
                        ElMessage.success("操作成功！");
                        getData();
                    } else {
                        ElMessage.error("操作失败！");
                    }
                }).catch(err=>{
                    ElMessage.error(err.msg);
                });
            }).catch((err)=>{
                ElMessage.info("取消批量删除！");
            });
        }
    }

    /**
     * @description 删除
     * @author weig
     * @param {Object} row 行数据
     */
    const handleClickDelete = (row)=>{
        let id = row.userRoleId;
        if (id.trim() == "" || id == undefined || id == null){
            ElMessage.error("用户角色Id不能为空，删除失败！");
            return;
        }
        elConfirmDialog(ElMessageBox,'此操作将永久删除该数据, 是否继续?','提示', '', ()=>{
            removeUserRole(id).then(res =>{
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

    /**
     * @description 获取角色列表数据
     * @author weig
     */
    const getRoleInfoList = ()=>{
        getRoleInfo().then(res =>{
            if (res.code == 1){
                state.roleInfo = res.data.role;
            } else {
                ElMessage.error(res.msg);
            }
        });
    }

    /**
     * @description 关闭对话框
     * @author weig
     */
    const closeEditForm = () =>{
        state.addDialogFormVisible = false;
        addForm.value.resetFields();//清空表单数据
        state.dialogType = 0;
        state.form = {
            userId: "",
            roleId: "",     
            userRoleId: "",
            createUserId: "",
            updateUserId: ""
        };
        state.options = [];
        state.userRoleCondition = false;
    }

    /**
     * @description  保存新增/编辑对话框
     * @author weig
     */
    const addDialogFormSave = () =>{
        if (IsNullOrEmpty(state.form.userId)){
            ElMessage.error("用户名称不能为空！");
            return;
        }
        if (IsNullOrEmpty(state.form.roleId )){
            ElMessage.error("角色名称不能为空！");
            return;
        }
        let params = {};
        switch (state.dialogType){
            case 0://编辑
                params = {
                    UserId: state.form.userId,
                    RoleId: state.form.roleId,
                    UserRoleId: state.form.userRoleId,
                    UpdateUserId: state.store.getters.userInfo.userId
                }
                editUserRole(params).then(res =>{
                    if(res.code == 1){
                        ElMessage.success("编辑成功");
                        state.addDialogFormVisible=false;
                        getData();
                    } else {
                        ElMessage.error(res.msg);
                    }
                });
                break;
            case 1://新增
                params = {
                    UserId: state.form.userId,
                    RoleId: state.form.roleId,
                    CreateUserId: state.store.getters.userInfo.userId
                }
                addUserRole(params).then(res =>{
                    if(res.code == 1){
                        ElMessage.success("添加成功");
                        state.addDialogFormVisible=false;
                        getData();
                    } else {
                        ElMessage.error(res.msg);
                    }
                });
                break;
        }
    }

    /**
     * @description 查询用户信息
     * @author weig
     */
    const remoteHandle = ((value)=>{
        if (IsNullOrEmpty(value)){
            return;
        }
        let params = {
            name: value
        }
        if (params.name.trim() == ""){
            state.options = [];
            return;
        }          
        if (state.timeoutId){
            clearTimeout(state.timeoutId);
            state.timeoutId = null;
        }
        state.timeoutId = setTimeout(()=>{
            getUserInfo(params.name).then(function(res){
                if(res.code == 1){
                    state.options = [];
                    if (res.data.user.length > 0){
                        res.data.user.forEach((item)=>{
                        state.options.push({
                            userId: item.userId,
                            nickName: item.nickName,
                            phone: item.phone,
                            email: item.email,
                            userName: item.userName
                        });
                        });
                    }
                } else {
                    ElMessage.error(res.msg);   
                }
            });
        }, 500);
    });

    /**
     * @description 清空编辑或者添加对话框的查询条件
     * @author weig
     */
    const onClearDialog = () =>{
        state.options.length = 0;
    }

    /**
     * @description 给当前用户新增角色
     * @author weig
     * @param {Number} index 当前行序号
     * @param {Object} row 当前行数据
     */
    const handleAddRoleToUser = (index, row)=>{
        state.addDialogFormVisible = true;
        state.form.userId = row.userId;
        state.options.length = 0;
        state.dialogType = 1;
        state.options.push({
            userId: row.userId,
            nickName: row.nickName,
            phone: row.phone,
            email: row.email,
            userName: row.userName
        });
        state.userRoleCondition = true;
    }
    return {
      state,
      handleClickAddData,
      handleClickBatchDelete,
      handleSelectionChange,
      onSearch,
      getData,
      resPageData,
      handleClickDelete,
      closeEditForm,
      addDialogFormSave,
      handleEdit,
      addForm,
      remoteHandle,
      onClearDialog,
      handleAddRoleToUser
    }
  },
}

</script>
<style scoped>
</style>