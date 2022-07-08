<!--
 * @Description: 
 * @Author: weig
 * @Date: 2021-07-05 17:44:41
 * @LastEditors: weig
 * @LastEditTime: 2021-12-17 11:55:03
-->
<template>
    <div>
        <!-- 面包屑 begin -->
        <eup-crumbs icon="el-icon-setting" firstCrumbs="角色管理" secondCrumbs="角色列表" />
        <!-- 面包屑 end -->
        <!-- 内容区域 begin -->
        <div class="container">
            <!-- 查询 -->
            <div class="handle-box">
                <el-form :model="state.query" :inline="true" v-if="checkPermission([`api${state.VIEW_VERSION}:auth:role:getpage`,`api${state.VIEW_VERSION}:auth:role:add`,`api${state.VIEW_VERSION}:auth:role:batchsoftdelete`])">
                <!-- <el-form :model="state.query" :inline="true"> -->
                    <template v-if="checkPermission([`api${state.VIEW_VERSION}:auth:role:getpage`])">
                    <!-- <template> -->
                        <el-form-item label="角色名">
                            <el-input v-model="state.query.name" placeholder="角色名" class="handle-input mr10"></el-input>
                        </el-form-item>
                        <el-button type="primary" icon="el-icon-search" @click="handleSearch">搜索</el-button>
                    </template>
                    <el-button type="primary" icon="el-icon-plus" @click="handleClickAddData" v-if="checkPermission([`api${state.VIEW_VERSION}:auth:role:add`])">新增</el-button>
                    <!-- <el-button type="primary" icon="el-icon-plus" @click="handleClickAddData">新增</el-button> -->
                    <el-button
                        type="danger"
                        icon="el-icon-delete"
                        class="handle-del mr10"
                        @click="handleClickBatchDelete"
                        :disabled="0 === state.sels.length"
                        v-if="checkPermission([`api${state.VIEW_VERSION}:auth:role:batchsoftdelete`])"
                        >批量删除
                    </el-button>
                    <!-- v-if="checkPermission(['api:auth:role:batchsoftdelete'])" -->
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
                <el-table-column prop="name" label="角色名"  min-width="150" align="center"></el-table-column>
                <el-table-column prop="roleType" label="编码" min-width="140" align="center"></el-table-column>
                <el-table-column prop="description" label="说明" min-width="200" align="center"></el-table-column>
                <el-table-column prop="createDate" label="创建时间" min-width="160" align="center"></el-table-column>
                <el-table-column prop="isActive" label="状态" min-width="120" align="center">
                    <template #default="{row}">
                        <el-tag :type="row.isActive == state.isActive.yes ? 'success' : 'danger'" disable-transitions>
                            {{ row.isActive == state.isActive.yes ? '正常' : '禁用' }}
                        </el-tag>
                    </template>
                </el-table-column>
                <el-table-column label="操作" min-width="240" align="center" fixed="right" v-if="checkPermission([`api${state.VIEW_VERSION}:auth:role:update`])">
                <!-- <el-table-column label="操作" width="240" align="center" fixed="right"> -->
                    <template #default="{ $index, row }">
                        <template v-if="row.roleType != state.EnumConfig.RoleType.SuperAdmin">
                            <el-dropdown  
                            split-button 
                            type="primary" 
                            style="margin-left:10px;" 
                            @click="handleClickEditData($index, row)" 
                            @command="(command)=>onCommand(command,row)"
                            >
                                编辑
                                <template #dropdown>
                                <el-dropdown-menu :visible-arrow="false" style="margin-top: 2px;width:100px;text-align:center;" v-if="checkPermission([`api${state.VIEW_VERSION}:auth:permission:assign`])">
                                <!-- <el-dropdown-menu :visible-arrow="false" style="margin-top: 2px;width:100px;text-align:center;"> -->
                                    <el-dropdown-item command="setPermission">设置权限</el-dropdown-item>
                                </el-dropdown-menu>
                                </template>
                            </el-dropdown>
                            <el-button
                                type="danger"
                                icon="el-icon-delete"
                                @click="handleClickDelete(row)"
                                class="ml10"
                                :disabled="row.roleType == 'SuperAdministrator' || row.name=='超级管理员'"
                                v-if="checkPermission([`api${state.VIEW_VERSION}:auth:role:softdelete`])"
                            >删除
                            </el-button>
                            <!-- v-if="checkPermission(['api:auth:role:softdelete'])" -->
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
            
            <!-- 选择权限 begin-->
            <eup-select-permission :roleId="roleId" :title="title" :visible="state.selectPermissionVisible" @click="onSelectPermission" @showVisible="showVisibleFunc"/>
            <!-- 选择权限 end-->
        </div>
        <!-- 内容区域 end -->
        
        <!-- 添加/编辑窗口 begin -->
        <el-dialog 
        :title="state.dialogTitle"
        v-model="state.addDialogFormVisible"
        width="60%"
        @close="closeDialog">
            <el-form
                ref="refAddForm"
                :model="state.form"
                label-width="80px"
                :inline="false"
                :rules="state.addFormRules"           
            >
            <el-row>
                <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                    <el-form-item label="角色名" prop="name">
                        <el-input v-model="state.form.name" auto-complete="off" />
                    </el-form-item>
                </el-col>
                <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                    <el-form-item label="编码" prop="roleType">
                        <el-select v-model="state.form.roleType" placeholder="请选择角色类型" style="width:100%;">
                            <el-option
                                v-for="item in state.codeList"
                                :key="item.value"
                                :label="item.label"
                                :value="item.value"
                            />
                        </el-select>
                    </el-form-item>
                </el-col>
                <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                    <el-form-item label="状态" prop="isActive">
                        <el-select v-model="state.form.isActive" placeholder="请选择角色状态" style="width:100%;">
                        <el-option
                            v-for="item in state.statusList"
                            :key="item.value"
                            :label="item.name"
                            :value="item.value"
                        />
                        </el-select>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row>
                <el-col :xs="24" :sm="24" :md="18" :lg="18" :xl="18">
                <el-form-item label="说明" prop="description">
                    <el-input v-model="state.form.description" type="textarea" :rows="2" auto-complete="off" />
                </el-form-item>
                </el-col>
            </el-row>
            </el-form>
        <template #footer>
            <span class="dialog-footer">
            <el-button @click="closeDialog()">取 消</el-button>
            <el-button type="primary" @click="addDialogFormSave()">确 定</el-button>
            </span>
        </template>
        </el-dialog>
        <!-- 添加/编辑窗口 end -->
    </div>
</template>
<script>
import { reactive, toRefs, onBeforeMount, onMounted,onActivated,ref,computed } from 'vue'
import { getRoleListPage, removeRole, editRole, addRole, batchRemoveRole } from '@/serviceApi/permission/role'
import { addRolePermission  } from '@/serviceApi/permission/permission'
import EupPagination from "@/components/EupPagination.vue"
import EupSelectPermission from '@/components/eup-select-window/eup-permission'
import { ElMessage,ElMessageBox } from 'element-plus'
import {useStore} from 'vuex'
import {elConfirmDialog} from "@/common/js/comm"
import EupCrumbs from "../../../components/eup-crumbs/index.vue"
import Enum from "@/enum/EnumConfig"

var CURR_VIEW_VERSION = Enum.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var VIEW_VERSION = CURR_VIEW_VERSION == 'V0' ? '' : `:S:${CURR_VIEW_VERSION}`;
export default {
  name: 'Role',
  components:{
      EupPagination: EupPagination,
      EupSelectPermission: EupSelectPermission,
      EupCrumbs: EupCrumbs
  },
  setup(props, context) {
    const refAddForm = ref(null);
    const state = reactive({
        query: {
            name: ""
        },
        VIEW_VERSION: VIEW_VERSION,
        pageIndex: 1,
        pageSize: 10,
        tableData: [],
        multipleSelection: [],
        delList: [],
        pageTotal: 0,
        dynamicFilter:{},
        form: {},
        idx: -1,
        loading: false,
        refForm: null,
        store: {},
        statusList: [
            { name: '激活', value: 1 },
            { name: '禁用', value: 2 }
        ],
        dialogTitle:"编辑角色",
        addDialogFormVisible: false, //编辑窗口是否显示
        addFormRules: {
            name: [{ required: true, message: '请输入角色名', trigger: 'blur' }],
            roleType: [{ required: true, message: '请输入编码', trigger: 'blur' }],
            isActive: [{ required: true, message: '请输入状态', trigger: 'change' }]
        },
        form: {
            name: '',
            roleType: 200,
            isActive: 1,
            description: "",
            "roleId": "",
            "id": ""
        },
        isActive: {
            yes: 1,
            no: 2
        },
        deleteLoading: false,
        dialogType: 0, //0:编辑  1：添加
        currentRow: {},
        selectPermissionVisible: false,
        sels: [],
        codeList:[
            {label:"系统管理员", value: 100},
            {label:"平台用户", value: 200}
        ],
        EnumConfig: {},
    });
    onBeforeMount(() => {
    });
    onMounted(() => {
        state.store=useStore();
        state.EnumConfig = Enum.EnumConfig;
        getData();
    });
    const roleId = computed (() =>{
        return state.currentRow?.roleId;
    });
    const title = computed (() =>{
        return `设置${state.currentRow?.name}（${state.currentRow?.roleType}）权限`;
    });
    /**
     * @description 获取表单信息
     * @author weig
     * @param
     */
    function getData (){
        var params = {
            "currentPage": state.pageIndex,
            "pageSize": state.pageSize,
            "filter.name": state.query.name,
            "dynamicFilter": state.dynamicFilter
        }
        state.loading = true;
        getRoleListPage(params).then(function(res){
            if(1 == res.code){
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
            state.sels = [];
        });
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
     * @description 触发搜索按钮
     * @author weig
     * @param
     */
    const handleSearch=()=> {
        state.pageIndex = 1
        getData();
    }
    
    /**
     * @description 新增角色
     * @author weig
     * @param
     */
    const handleClickAddData = () =>{
        state.addDialogFormVisible = true;
        state.dialogTitle = '新增角色';
        state.dialogType = 1;
    }

    /**
     * @description 编辑角色
     * @author weig
     * @param {Number} index 行号
     * @param {Object} row 行数据
     */
    const handleClickEditData = (index, row)=>{
        state.addDialogFormVisible = true;
        state.dialogTitle = '编辑角色';
        state.dialogType = 0;
        const rowData = JSON.parse(JSON.stringify(row));//深拷贝
        state.form.name = rowData.name;
        state.form.roleType = rowData.roleType;
        state.form.description = rowData.description;
        state.form.isActive = rowData.isActive;
        state.form.roleId = rowData.roleId;
    }

    /**
     * @description 更多操作
     * @author weig
     * @param {String} command 指令名称
     * @param {Object} row 行对象数据
     */
    function onCommand(command, row) {
        if (command === 'setPermission') {
            state.currentRow = row
            state.selectPermissionVisible = true
        }
    }

    /**
     * @description 关闭编辑/新增对话框
     * @author weig
     * @param
     */
    const closeDialog = ()=>{
        state.addDialogFormVisible = false;
        refAddForm.value.resetFields();//清空表单
        //每次关闭对话框后清理表单历史数据
        state.form= {
            name: '',
            roleType: 200,
            isActive: 1, 
            description: "",
            "roleId": "",
            "id": ""
        };
    }

    /**
     * @description 保存新增/编辑对话框
     * @author weig
     * @param
     */
    const addDialogFormSave = ()=>{
        //先校验数据
        let bValue = validateFormData();
        if (!bValue){
            return;
        }
        let params = {};
        if (1 == state.dialogType){//新增
            params = {
                name: state.form.name,
                roleType: state.form.roleType,
                isActive: state.form.isActive, 
                description: state.form.description,
                createUserId: state.store.getters.userInfo.userId
            };
        } else if (0 == state.dialogType){//编辑
            params = {
                name: state.form.name,
                roleType: state.form.roleType,
                isActive: state.form.isActive, 
                description: state.form.description,
                "roleId": state.form.roleId,
                "id": state.form.id,
                updateUserId: state.store.getters.userInfo.userId
            }
        }

        switch(state.dialogType){
            case 0://编辑
                editRole(params).then(res =>{
                    if(1 == res.code){
                        ElMessage.success("角色编辑成功");
                        state.addDialogFormVisible=false;
                        getData();
                    } else {
                        ElMessage.error(res.msg);
                    }
                });
                break;
            case 1://新增
                addRole(params).then(res =>{
                    if(1 == res.code){
                        ElMessage.success("角色新增成功");
                        state.addDialogFormVisible=false;
                        getData();
                    } else {
                        ElMessage.error(res.msg);
                    }
                });
                break;
            default:
                break;
        }
    }

    /**
     * @description 删除角色
     * @author weig
     * @param {Object} row 当前行数据
     */
    function handleClickDelete(row){
        if (!deleteValidate()){
            return;
        }
        let roleId = row.roleId;
        if (roleId.trim() == "" || roleId == undefined || roleId == null){
            ElMessage.error("角色Id不能为空，删除失败！");
            return;
        }
        elConfirmDialog(ElMessageBox,'此操作将永久删除该数据, 是否继续?','提示', '', ()=>{
            removeRole(roleId).then(res =>{
                if (1 == res.code){
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
     * @description 删除校验
     * @author weig
     * @param {Object} row 当前行对象
     * @returns {Boolean} true
     */
    const deleteValidate = (row)=>{
        let isValid = true;
        if (row && row.name === 'admin') {
            ElMessage.warning(`${row.description},禁止删除！`);
            isValid = false;
        }
        return isValid;
    }

    /**
     * @description 批量删除
     * @author weig
     * @param
     */
    function handleClickBatchDelete (){
        if (0 == state.multipleSelection.length){//未选中
            ElMessage.error("请选择要删除的数据！");
        } else {
            var roleIds = state.multipleSelection.map(s =>{
              return s.roleId;
            });
            ElMessageBox.confirm('此操作将删除选中的记录, 是否继续?', '提示',{
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(()=>{
                batchRemoveRole(roleIds).then(res=>{
                    if (1 == res.code){
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
     * @description 子组件返回分页数据
     * @author weig
     * @param {Object} obj
     */
    const resPageData = (obj) =>{
        state.pageIndex = obj.currPage;
        state.pageSize = obj.pageSize;
    }

    /**
     * @description 校验新增/编辑角色数据
     * @author weig
     * @param 
     * @returns {Boolean} true
     */
    const validateFormData =()=>{
        if (state.form.name.trim() == ""){
            ElMessage.warning("角色名不能为空！");
            return false;
        }
        return true; 
    }

    /**
     * @description 枚举转换
     * @author weig
     * @param {Array} arrList 数组对象
     * @param {Number} val 值
     * @returns {Boolean} bool
     */
    const enumFormat =(arrList, val)=>{
        if(val == "" || val == undefined || val == null){
            return false;
        }
        let bRet = false;
        arrList.forEach(item =>{
            if (item.value == val && 1 == val){//激活
                bRet = true;
            } else if(item.value == val && 0 == val) {//禁用
                bRet = false;
            }
        });
        return bRet;
    }

    /**
     * @description 选择保存权限
     * @author weig
     * @param {} permissionIds
     */
    async function onSelectPermission (permissionIds){
        const para = { permissionIds, roleId: roleId.value }
        const res = await addRolePermission(para);
        if (res && 1 === res.code) {
            state.selectPermissionVisible = false;
            ElMessage.success('保存成功');
        }
    }

    /**
     * @description 修改权限对话框显示与隐藏属性值
     * @author weig
     * @param {Boolean} value 
     */
    function showVisibleFunc(value){
        state.selectPermissionVisible = value;
    }
    return {
      state,
      handleSelectionChange,
      handleSearch,
      handleClickAddData,
      getData,
      resPageData,
      onCommand,
      closeDialog,
      addDialogFormSave,
      refAddForm,
      handleClickDelete,
      handleClickBatchDelete,
      handleClickEditData,
      roleId,
      title,
      onSelectPermission,
      showVisibleFunc
    }
  },
}

</script>
<style scoped>
</style>
