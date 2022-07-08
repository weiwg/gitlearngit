<!--
 * @Description: 
 * @Author: weig
 * @Date: 2021-08-23 13:40:15
 * @LastEditors: weig
 * @LastEditTime: 2021-12-17 13:37:41
-->
<template>
    <div>
        <!-- 面包屑 begin -->
        <eup-crumbs icon="el-icon-s-management" firstCrumbs="系统管理" secondCrumbs="租户管理" />
        <!-- 面包屑 end -->

        <!-- 内容区域 begin -->
        <div class="container">
            <!-- 查询 -->
            <div class="handle-box">
                <el-form :model="state.query" :inline="true" v-if="checkPermission([`api${state.VIEW_VERSION}:System:SysRegion:GetPage`])">
                    <template v-if="checkPermission([`api${state.VIEW_VERSION}:System:Tenant:GetPage`])">
                        <el-form-item label="企业名称">
                            <el-input v-model="state.query.name" placeholder="企业名称" class="handle-input mr10"></el-input>
                        </el-form-item>
                        <el-form-item>
                            <el-button type="primary" icon="el-icon-search" @click="handleSearch">搜索</el-button>
                        </el-form-item>
                    </template>
                    <el-form-item v-if="checkPermission([`api${state.VIEW_VERSION}:System:Tenant:Add`])">
                        <el-button type="primary" icon="el-icon-plus" @click="handleClickAddData">新增</el-button>
                    </el-form-item>
                    <el-form-item v-if="checkPermission([`api${state.VIEW_VERSION}:System:Tenant:BatchSoftDelete`])">
                        <el-button
                            type="danger"
                            icon="el-icon-delete"
                            class="handle-del mr10"
                            @click="handleClickBatchDelete"
                            :disabled="state.sels.length === 0"
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
                highlight-current-row
            >
                <el-table-column type="selection" width="55" align="center"></el-table-column>
                <el-table-column prop="num" label="序号" width="55" align="center"></el-table-column>
                <el-table-column prop="name" label="企业名称" width="100" align="center"></el-table-column>
                <el-table-column prop="code" label="企业编码" align="center"></el-table-column>
                <el-table-column prop="dataIsolationTypeName" label="数据隔离" align="center"></el-table-column>
                <el-table-column prop="dbTypeName" label="数据库" width="120" align="center"/>
                <el-table-column prop="idleTime" label="空闲时间（分）" width="120" align="center"/>
                <el-table-column prop="createDate" label="创建时间" align="center"></el-table-column>
                <el-table-column prop="isActive" label="状态" align="center">
                    <template #default="{row}">
                        <el-tag :type="row.isActive == state.isActive.yes ? 'success' : 'danger'" disable-transitions>
                            {{ row.isActive == state.isActive.yes ? '正常' : '禁用' }}
                        </el-tag>
                    </template>
                </el-table-column>
                <el-table-column label="操作" width="240" align="center" fixed="right" v-if="checkPermission([`api${state.VIEW_VERSION}:System:Tenant:Update`])">
                    <template #default="{ $index, row }">
                        <el-dropdown  
                        split-button
                        type="primary" 
                        style="margin-left:10px;" 
                        @click="handleClickEditData($index, row)" 
                        @command="(command)=>onCommand(command,row)">
                            编辑
                            <template #dropdown v-if="checkPermission([`api${state.VIEW_VERSION}:auth:permission:savetenantpermissions`,`api${state.VIEW_VERSION}:System:Tenant:Delete`])">
                                <el-dropdown-menu :visible-arrow="false" style="margin-top: 2px;width:100px;text-align:center;">
                                    <el-dropdown-item command="setPermission" v-if="checkPermission([`api${state.VIEW_VERSION}:auth:permission:savetenantpermissions`])">设置权限</el-dropdown-item>
                                    <el-dropdown-item command="delete" v-if="checkPermission([`api${state.VIEW_VERSION}:System:Tenant:Delete`])">彻底删除</el-dropdown-item>
                                </el-dropdown-menu>
                            </template>
                        </el-dropdown>
                        <el-button
                            type="danger"
                            icon="el-icon-delete"
                            @click="handleClickDelete(row)"
                            class="ml10"
                            v-if="checkPermission([`api${state.VIEW_VERSION}:System:Tenant:SoftDelete`])"
                        >删除
                        </el-button>
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
            <eup-select-permission :tenantId="tenantId" :tenant="true" :title="title" :visible="state.selectPermissionVisible" @click="onSelectPermission" @showVisible="showVisibleFunc"/>
            <!-- <my-select-permission :tenant="true" :tenant-id="tenantId" :title="title" :visible="state.selectPermissionVisible" :set-permission-loading="state.setPermissionLoading" @click="onSelectPermission" /> -->
            <!-- 选择权限 end-->
        </div>
        <!-- 内容区域 end -->
        
        <!-- 添加/编辑窗口 begin -->
        <el-dialog 
        :title="state.dialogTitle"
        v-model="state.addDialogFormVisible"
        :close-on-click-modal="false"
        width="60%"
        @close="closeDialog">
            <el-form
                ref="refAddForm"
                :model="state.form"
                label-width="120px"
                :inline="false"
                :rules="state.addFormRules"           
            >
            <el-row>
                <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                    <el-form-item label="企业名称" prop="name">
                        <el-input v-model="state.form.name" auto-complete="off" />
                    </el-form-item>
                </el-col>
                <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                    <el-form-item label="企业编码" prop="code">
                        <el-input v-model="state.form.code" auto-complete="off" />
                    </el-form-item>
                </el-col>
                <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                    <el-form-item label="数据隔离类型" prop="dataIsolationType">
                        <el-select v-model="state.form.dataIsolationType" placeholder="数据隔离类型" style="width:100%;">
                            <el-option
                            v-for="item in state.dataIsolationTypeList"
                            :key="item.value"
                            :label="item.label"
                            :value="item.value"
                            />
                        </el-select>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row>
                <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                    <el-form-item label="姓名" prop="realName">
                        <el-input v-model="state.form.realName" auto-complete="off" />
                    </el-form-item>
                </el-col>
                <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                    <el-form-item label="手机号码" prop="phone">
                        <el-input v-model="state.form.phone" auto-complete="off" />
                    </el-form-item>
                </el-col>
                <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                    <el-form-item label="邮箱地址" prop="email">
                        <el-input v-model="state.form.email" auto-complete="off" />
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row>
                <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                    <el-form-item label="数据库" prop="dbType">
                    <el-select v-model="state.form.dbType" filterable placeholder="请选择数据库" style="width:100%;">
                        <el-option
                        v-for="item in state.dbTypeList"
                        :key="item.value"
                        :label="item.label"
                        :value="item.value"
                        />
                    </el-select>
                    </el-form-item>
                </el-col>
                <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                    <el-form-item label="空闲时间（分）" prop="idleTime">
                        <el-input-number v-model="state.form.idleTime" controls-position="right" :min="0" style="width:100%;"></el-input-number>
                    </el-form-item>
                </el-col>
                <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                    <el-form-item label="状态" prop="isActive">
                        <el-select v-model="state.form.isActive" placeholder="请选择租户状态" style="width:100%;">
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
                    <el-form-item label="连接字符串" prop="connectionString">
                        <el-input v-model="state.form.connectionString" type="textarea" :autosize="{ minRows: 2, maxRows: 4}" auto-complete="off" />
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
import { getTenantList, updateTenantInfo, addTenantInfo, SoftDeleteTenant, DeleteTenant,BatchSoftDeleteTenant  } from '@/serviceApi/system/tenant'
import { saveTenantPermissions } from '@/serviceApi/permission/permission'
import EupPagination from "@/components/EupPagination.vue"
import EupSelectPermission from '@/components/eup-select-window/eup-permission'
import { ElMessage,ElMessageBox } from 'element-plus'
import {useStore} from 'vuex'
import {elConfirmDialog} from "@/common/js/comm"
import EupCrumbs from "../../../components/eup-crumbs/index.vue"
import EnumConfig from "@/enum/EnumConfig"

var CURR_VIEW_VERSION = EnumConfig.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var VIEW_VERSION = CURR_VIEW_VERSION == 'V0' ? '' : `:S:${CURR_VIEW_VERSION}`;

export default {
  name: 'Tenant',
  components:{
      EupPagination: EupPagination,
      EupSelectPermission: EupSelectPermission,
      EupCrumbs, EupCrumbs
  },
  setup(props, context) {  
    const validatePhone = (rule, value, callback) => {
        var phone=value.replace(/\s/g, "");//去除空格
        //校验手机号，号段主要有(不包括上网卡)：130~139、150~153，155~159，180~189、170~171、176~178。14号段为上网卡专属号段
        let regs = /^(((13[0-9]{1})|(15[0-9]{1})|(16[0-9]{1})|(17[3-8]{1})|(18[0-9]{1})|(19[0-9]{1})|(14[5-7]{1}))+\d{8})$/;
        if(value.length == 0){
            callback();
        } else{
            if(!regs.test(phone)){
                callback([new Error('手机号输入不合法')]);
            }else{
                callback();
            }
        }
    }
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
            { name: '禁用', value: 0 }
        ],
        dialogTitle:"编辑角色",
        addDialogFormVisible: false, //编辑窗口是否显示
        addFormRules: {
            name: [{ required: true, message: '请输入企业名称', trigger: 'blur' }],
            code: [{ required: true, message: '请输入企业编码', trigger: 'blur' }],
            realName: [{ required: true, message: '请输入姓名', trigger: 'blur' }],
            phone: [
            { required: true, message: '请输入手机号码', trigger: 'blur' },
            { validator: validatePhone, trigger: ['blur', 'change'] }
            ],
            email: [
            { required: true, message: '请输入邮箱地址', trigger: 'blur' },
            { type: 'email', message: '请输入正确的邮箱地址', trigger: ['blur', 'change'] }
            ],
            isActive: [{ required: true, message: '请选择状态', trigger: 'change' }]
        },
        form: {
            "code": "",
            "name": "",
            "realName": "",
            "phone": "",
            "email": "",
            "dataIsolationType": 1,
            "dbType": 0,
            "connectionString": "",
            "idleTime": 0,
            "isActive": true,
            "description": "",
            
            id: ""
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

        statusList: [
            { name: '激活', value: true },
            { name: '禁用', value: false }
        ],
        dataIsolationTypeList: [
            { 'label': '独立数据库', 'value': 1 },
            { 'label': '共享数据库', 'value': 4 }
        ],
        dbTypeList: [
            { 'label': 'MySql', 'value': 0 },
            { 'label': 'SqlServer', 'value': 1 },
            { 'label': 'PostgreSQL', 'value': 2 },
            { 'label': 'Oracle', 'value': 3 },
            { 'label': 'Sqlite', 'value': 4 },
            { 'label': 'OdbcOracle', 'value': 5 },
            { 'label': 'OdbcSqlServer', 'value': 6 },
            { 'label': 'OdbcMySql', 'value': 7 },
            { 'label': 'OdbcPostgreSQL', 'value': 8 },
            { 'label': 'Odbc', 'value': 9 },
            { 'label': 'OdbcDameng', 'value': 10 },
            { 'label': 'MsAccess', 'value': 11 },
            { 'label': 'Dameng', 'value': 12 },
            { 'label': 'OdbcKingbaseES', 'value': 13 },
            { 'label': 'ShenTong', 'value': 14 },
            { 'label': 'KingbaseES', 'value': 15 },
            { 'label': 'Firebird', 'value': 16 }
        ],
        setPermissionLoading: false

    });
    onBeforeMount(() => {
    });
    onMounted(() => {
        state.store=useStore();
        getData();
    });
    const tenantId = computed (() =>{
        return state.currentRow?.tenantId;
    });
    const title = computed (() =>{
        return `设置${state.currentRow?.name}（${state.currentRow?.code}）权限`;
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
        getTenantList(params).then(function(res){
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
        state.dialogTitle = '新增租户';
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
        state.dialogTitle = '编辑租户';
        state.dialogType = 0;
        const rowData = JSON.parse(JSON.stringify(row));//深拷贝
        state.form.name = rowData.name;
        state.form.code = rowData.code;
        state.form.realName = rowData.realName;
        state.form.phone = rowData.phone;
        state.form.email = rowData.email;
        state.form.dataIsolationType = rowData.dataIsolationType;
        state.form.dbType = dbTypeListFormat(state.dbTypeList, rowData.dbTypeName) ? dbTypeListFormat(state.dbTypeList, rowData.dbTypeName)[0].value : 0;
        state.form.idleTime = rowData.idleTime;
        state.form.description = rowData.description;
        state.form.isActive = rowData.isActive == state.isActive.yes ?  true: false;
        state.form.id = rowData.id;
        state.form.connectionString = rowData.connectionString;
    }

    /**
     * @description 更多操作
     * @author weig
     * @param {String} command 指令名称
     * @param {Object} row 行对象数据
     */
    function onCommand(command, row) {
        state.currentRow = row
        if (command === 'setPermission') {
            state.selectPermissionVisible = true
        } else if (command == "delete"){
            handleDelete(state.currentRow?.tenantId)
        }
    }

    /**
     * @description 彻底删除租户信息
     * @author weig
     * @param {String} id
     */
    function handleDelete(id){
        if(id) {
            elConfirmDialog(ElMessageBox,'此操作将永久删除该数据, 是否继续?','提示', '', ()=>{
                DeleteTenant(id).then(res =>{
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
        } else {
            ElMessage.error("id信息为空或者错误！");
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
            "code": "",
            "name": "",
            "realName": "",
            "phone": "",
            "email": "",
            "dataIsolationType": 1,
            "dbType": 0,
            "connectionString": "",
            "idleTime": 0,
            "isActive": true,
            "description": "",
            id: ""
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
        if (state.dialogType == 1){//新增
            params = {
                name: state.form.name,
                code: state.form.code,
                realName: state.form.realName,
                phone: state.form.phone,
                email: state.form.email,
                dataIsolationType: state.form.dataIsolationType,
                dbType: state.form.dbType,
                connectionString: state.form.connectionString,
                isActive: state.form.isActive ? state.isActive.yes : state.isActive.no, 
                description: state.form.description,
                idleTime: state.form.idleTime,
                createUserId: state.store.getters.userInfo.userId
            };
        } else if (state.dialogType == 0){//编辑
            params = {
                name: state.form.name,
                code: state.form.code,
                realName: state.form.realName,
                phone: state.form.phone,
                email: state.form.email,
                dataIsolationType: state.form.dataIsolationType,
                dbType: state.form.dbType,
                connectionString: state.form.connectionString,
                isActive: state.form.isActive ? state.isActive.yes : state.isActive.no, 
                description: state.form.description,
                idleTime: state.form.idleTime,
                "id": state.form.id,
                updateUserId: state.store.getters.userInfo.userId
            }
        }
        switch(state.dialogType){
            case 0://编辑
                updateTenantInfo(params).then(res =>{
                    if(res.code == 1){
                        ElMessage.success("租户编辑成功");
                        state.addDialogFormVisible=false;
                        getData();
                    } else {
                        ElMessage.error(res.msg);
                    }
                });
                break;
            case 1://新增
                addTenantInfo(params).then(res =>{
                    if(res.code == 1){
                        ElMessage.success("租户新增成功");
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
        let id = row.tenantId;
        if (id.trim() == "" || id == undefined || id == null){
            ElMessage.error("Id不能为空，删除失败！");
            return;
        }
        elConfirmDialog(ElMessageBox,'此操作将永久删除该数据, 是否继续?','提示', '', ()=>{
            SoftDeleteTenant(id).then(res =>{
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
        if (state.multipleSelection.length == 0){//未选中
            ElMessage.error("请选择要删除的数据！");
        } else {
            var ids = state.multipleSelection.map(s =>{
              return s.id;
            });
            ElMessageBox.confirm('此操作将删除选中的记录, 是否继续?', '提示',{
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(()=>{
                BatchSoftDeleteTenant(ids).then(res=>{
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
            ElMessage.warning("企业名称不能为空！");
            return false;
        }
        if (state.form.code.trim() == ""){
            ElMessage.warning("企业编码不能为空！");
            return false;
        }
        if (state.form.realName.trim() == ""){
            ElMessage.warning("姓名不能为空！");
            return false;
        }
        if (state.form.phone.trim() == ""){
            ElMessage.warning("手机号不能为空！");
            return false;
        } else {
            var phone=state.form.phone.replace(/\s/g, "");//去除空格
            //校验手机号，号段主要有(不包括上网卡)：130~139、150~153，155~159，180~189、170~171、176~178。14号段为上网卡专属号段
            let regs = /^(((13[0-9]{1})|(15[0-9]{1})|(16[0-9]{1})|(17[3-8]{1})|(18[0-9]{1})|(19[0-9]{1})|(14[5-7]{1}))+\d{8})$/;
            if(phone.length == 0){
                ElMessage.warning("手机号不能为空！");
                return false;
            } else{
                if(!regs.test(phone)){
                    ElMessage.warning("手机号输入不合法");
                    return false;
                }
            }
        }
        if (state.form.email.trim() == ""){
            ElMessage.warning("邮箱地址不能为空！");
            return false;
        } else {
            var email=state.form.email.replace(/\s/g, "");//去除空格
            var regs=/^\w+@[a-z0-9]+\.[a-z]{2,4}$/;
            if (!regs.test(email)){
                ElMessage.warning("邮箱输入不合法！");
                return false;
            }
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
            if (item.value == val && val == 1){//激活
                bRet = true;
            } else if(item.value == val && val == 0) {//禁用
                bRet = false;
            }
        });
        return bRet;
    }

    /**
     * @description 数据库格式转换
     * @author weig
     * @param {Array} arrList
     * @param {string} name
     */
    const dbTypeListFormat = (arrList, name)=>{
        var temp = arrList.filter((data)=>{
            return data.label === name;
        });
        return temp;
    }

    /**
     * @description 选择保存权限
     * @author weig
     * @param {Array} permissionIds 权限点id
     */
    async function onSelectPermission (permissionIds){
        state.setPermissionLoading = true;
        const para = { permissionIds, tenantId: tenantId.value }
        const res = await saveTenantPermissions(para);
        if (res && res.code === 1) {
            state.selectPermissionVisible = false;
            ElMessage.success('保存成功');
        }
        state.setPermissionLoading = false;
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
      tenantId,
      title,
      onSelectPermission,
      showVisibleFunc
    }
  },
}
</script>
<style scoped>
</style>