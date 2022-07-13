<!--
 *@Description: 
 *@Author: weig
 *@Date: 2022-07-09 09:56:53
 *@LastEditors: weig
 *@LastEditTime: 2022-07-09 14:46:53
-->
<template>
   <div>
        <!-- 面包屑 begin -->
        <eup-crumbs icon="el-icon-bangzhu" firstCrumbs="异常管理" secondCrumbs="异常列表" />
        <!-- 面包屑 end -->

        <!-- 内容区域 begin -->
        <div class="container">
            <!-- 查询 -->
            <div class="handle-box">
                <el-form :model="query" :inline="true" v-if="checkPermission([`api${VIEW_VERSION}:System:SysRegion:GetPage`])">
                    <template v-if="checkPermission([`api${VIEW_VERSION}:System:Tenant:GetPage`])">
                        <el-form-item label="异常单号">
                            <el-input v-model="query.abnormalNo" placeholder="异常单号" class="handle-input mr10"></el-input>
                        </el-form-item>
                        <el-form-item label="责任人">
                            <el-input v-model="query.responBy" placeholder="责任人" class="handle-input mr10"></el-input>
                        </el-form-item>
                        <el-form-item label="项目名称">
                            <el-select v-model="query.proName" placeholder="项目名称" class="handle-select mr10" @change="changeProNameHandle">
                                <el-option v-for="item in arrProName" :key="item.value" :label="item.label" :value="item.value"></el-option>
                            </el-select>
                        </el-form-item>
                        <el-form-item label="线体">
                            <el-select v-model="query.lineName" placeholder="线体" class="handle-select mr10">
                                <el-option v-for="item in arrLines" :key="item.value" :label="item.label" :value="item.value"></el-option>
                            </el-select>
                        </el-form-item>
                        <el-form-item label="责任部门">
                            <el-select v-model="query.responDepart" placeholder="责任部门" class="handle-select mr10">
                                <el-option v-for="item in arrResponDepart" :key="item.value" :label="item.label" :value="item.value"></el-option>
                            </el-select>
                        </el-form-item>
                        <el-form-item label="状态">
                            <el-select v-model="query.abnomalStatus" placeholder="状态" class="handle-select mr10">
                                <el-option v-for="item in arrAbnomalStatus" :key="item.value" :label="item.label" :value="item.value"></el-option>
                            </el-select>
                        </el-form-item>
                        <el-form-item label="开始时间">
                            <el-date-picker v-model="query.startDate" type="date" placeholder="选择日期" :disabledDate="disabledDate1">
                            </el-date-picker>
                        </el-form-item>
                        <el-form-item label="结束时间">
                            <el-date-picker v-model="query.endDate" type="date" placeholder="选择日期" :disabledDate="disabledDate2">
                            </el-date-picker>
                        </el-form-item>
                        <el-form-item>
                            <el-button type="primary" icon="el-icon-search" @click="handleSearch">搜索</el-button>
                        </el-form-item>
                    </template>
                    <el-form-item v-if="checkPermission([`api${VIEW_VERSION}:System:Tenant:Add`])">
                        <el-button type="primary" icon="el-icon-plus" @click="handleClickAddData">新增</el-button>
                    </el-form-item>
                    <el-form-item v-if="checkPermission([`api${VIEW_VERSION}:System:Tenant:BatchSoftDelete`])">
                        <el-button
                            type="danger"
                            icon="el-icon-delete"
                            class="handle-del mr10"
                            @click="handleClickBatchDelete"
                            :disabled="batchDelete.length === 0"
                            >批量删除
                        </el-button>
                    </el-form-item>
                    <!-- <el-form-item v-if="checkPermission([`api${VIEW_VERSION}:System:Tenant:Add`])">
                        <el-button type="primary" @click="handleClickExportData">导出</el-button>
                    </el-form-item> -->
                </el-form>
            </div>

            <!-- 列表 -->
            <el-table
                :data="tableData"
                border
                class="table"
                ref="multipleTable"
                header-cell-class-name="table-header"
                @selection-change="handleSelectionChange"
                v-loading="loading"
                highlight-current-row
            >
                <el-table-column type="selection" width="55" align="center"></el-table-column>
                <el-table-column prop="num" label="序号" width="55" align="center"></el-table-column>
                <el-table-column prop="abnormalNo" label="异常单号" min-width="160" align="center"></el-table-column>
                <el-table-column prop="projectNo" label="项目名称" width="80" align="center"></el-table-column>
                <el-table-column prop="lineName" label="线体" align="center"></el-table-column>
                <el-table-column prop="classAB" label="班别" align="center"></el-table-column>
                <el-table-column prop="fProcess" label="站点" width="120" align="center"/>
                <!-- <el-table-column prop="type" label="异常大类" width="120" align="center"/>
                <el-table-column prop="itemType" label="异常小类" width="120" align="center"/> -->
                <el-table-column prop="responDepart" label="责任部门" width="120" align="center"/>
                <el-table-column prop="responBy" label="责任人" align="center"></el-table-column>
                <el-table-column prop="beginTime" label="开始时间" align="center" min-width="160"></el-table-column>
                <el-table-column prop="endTime" label="结束时间" align="center" min-width="160"></el-table-column>
                <el-table-column prop="status" label="状态" align="center">
                    <template #default="{row}">
                        <el-tag :type="row.status == EnumConfig.EnumConfig.AbnomalStatus.已处理 ? 'success' : 'danger'" disable-transitions>
                            {{ EnumConfig.EnumConfig.getEnumName(row.status, EnumConfig.EnumConfig.AbnomalStatus)}}
                        </el-tag>
                    </template>
                </el-table-column>
                <el-table-column label="操作" width="240" align="center" fixed="right" v-if="checkPermission([`api${VIEW_VERSION}:System:Tenant:Update`])">
                    <template #default="{ $index, row }">
                        <el-button
                            type="primary"
                            icon="el-icon-edit"
                            @click="handleEdit($index,row)"
                            v-if="checkPermission([`api${VIEW_VERSION}:auth:view:update`])"
                        >编辑
                        </el-button>
                        <el-button
                            type="primary" 
                            icon="el-icon-check" 
                            @click="handleClickEditData($index,row)"
                            v-if="checkPermission([`api${VIEW_VERSION}:auth:view:update`])"
                        >处理
                        </el-button>
                        <el-button
                            type="danger"
                            icon="el-icon-delete"
                            @click="handleClickDelete(row)"
                            class="ml10"
                            v-if="checkPermission([`api${VIEW_VERSION}:System:Tenant:SoftDelete`])"
                        >删除
                        </el-button>
                    </template>
                </el-table-column>
            </el-table>
            <!-- 分页 begin-->
            <EupPagination
                :current-page="pageIndex"
                :pagesizes="[10,20,50,100]"
                :pagesize="pageSize"
                :total="pageTotal"
                @getPageData="getData"
                @resPageData="resPageData">
            </EupPagination>
            <!-- 分页 end-->
        </div>
        <!-- 内容区域 end -->
        <!-- 添加/编辑窗口 begin -->
        <el-dialog 
        :title="dialogTitle"
        v-model="addDialogFormVisible"
        :close-on-click-modal="false"
        width="60%"
        @close="closeDialog">
            <el-form
                ref="refAddForm"
                :model="form"
                label-width="120px"
                :inline="false"        
            >
                <el-row>
                    <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                        <el-form-item label="项目编号" prop="projectNo" required>
                            <el-select v-model="form.projectNo" placeholder="请选择项目编号" style="width:100%;" @change="changeProNameHandleForm">
                            <el-option
                                v-for="item in arrProName"
                                :key="item.value"
                                :label="item.label"
                                :value="item.value"
                            />
                            </el-select>
                        </el-form-item>
                    </el-col>                    
                    <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                        <el-form-item label="线体" prop="lineName" required>
                            <el-select v-model="form.lineName" placeholder="请选择线体" style="width:100%;">
                            <el-option
                                v-for="item in arrLinesForm"
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
                        <el-form-item label="班别" prop="classAB" required>
                            <el-select v-model="form.classAB" placeholder="请选择班别" style="width:100%;">
                            <el-option
                                v-for="item in arrClassAB"
                                :key="item.value"
                                :label="item.label"
                                :value="item.value"
                            />
                            </el-select>
                        </el-form-item>
                    </el-col>
                    <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                        <el-form-item label="工序站点" prop="fProcess" required>
                            <el-select v-model="form.fProcess" placeholder="请选择工序站点" style="width:100%;">
                            <el-option
                                v-for="item in arrFProcess"
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
                        <el-form-item label="异常大类" prop="fProcess" required>
                            <el-select v-model="form.type" placeholder="请选择异常大类" style="width:100%;">
                            <el-option
                                v-for="item in arrAbnomalType"
                                :key="item.value"
                                :label="item.label"
                                :value="item.value"
                            />
                            </el-select>
                        </el-form-item>
                    </el-col>
                    <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                        <el-form-item label="异常小类" prop="fProcess" required>
                            <el-select v-model="form.itemType" placeholder="请选择异常小类" style="width:100%;">
                            <el-option
                                v-for="item in arrAbnomalItemType"
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
                        <el-form-item label="责任部门" prop="responDepart" required>
                            <el-select v-model="form.responDepart" placeholder="请选择责任部门" style="width:100%;">
                            <el-option
                                v-for="item in arrResponDepart"
                                :key="item.value"
                                :label="item.label"
                                :value="item.value"
                            />
                            </el-select>
                        </el-form-item>
                    </el-col>
                    <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                        <el-form-item label="责任人" prop="responBy" required>
                            <el-select 
                            v-model="form.responBy" 
                            filterable 
                            placeholder="请输入责任人"
                            :remote-method="remoteHandle"
                            remote
                            :clearable="true"
                            @clear="onClearDialog"
                            >
                            <el-option
                                v-for="item in arrResponBy"
                                :key="item.personLiableId"
                                :label="item.name"
                                :value="item.personLiableId">
                            </el-option>
                            </el-select>
                        </el-form-item>
                    </el-col>  
                </el-row>
                <el-row>
                    <el-col :xs="24" :sm="24" :md="18" :lg="18" :xl="18">
                        <el-form-item label="开始时间" required>
                            <el-date-picker v-model="form.beginTime" type="date" placeholder="选择日期" :disabledDate="disabledDate1">
                            </el-date-picker>
                        </el-form-item>
                    </el-col>
                    <!-- <el-col :xs="24" :sm="24" :md="18" :lg="18" :xl="18">
                        <el-form-item label="结束时间">
                            <el-date-picker v-model="form.endTime" type="date" placeholder="选择日期" :disabledDate="disabledDate2">
                            </el-date-picker>
                        </el-form-item>
                    </el-col> -->
                </el-row>
                <el-row>
                    <el-col :xs="24" :sm="24" :md="18" :lg="18" :xl="18">
                    <el-form-item label="异常描述" prop="description">
                        <el-input v-model="form.description" type="textarea" :rows="2" auto-complete="off" />
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
import { reactive, toRefs, onBeforeMount, onMounted,onActivated,ref } from 'vue'
import { ElMessage,ElMessageBox } from 'element-plus'
import {useStore} from 'vuex'
import {elConfirmDialog, IsNullOrEmpty} from "@/common/js/comm"
import EupCrumbs from "../../../components/eup-crumbs/index.vue"
import EnumConfig from "../../../enum/EnumConfig"
import EupPagination from "@/components/EupPagination.vue"
import FileSaver from 'file-saver'
import XLSX from 'xlsx'

var CURR_VIEW_VERSION = EnumConfig.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var VIEW_VERSION = CURR_VIEW_VERSION == 'V0' ? '' : `:S:${CURR_VIEW_VERSION}`;
export default {
    name: 'AbnormalList',
    components:{
        EupPagination: EupPagination,
        EupCrumbs, EupCrumbs
    },
   setup(props, context) {
      const refAddForm = ref(null);
      const multipleTable = ref(null);
      const data = reactive({
        query: {                         //查询条件
            responBy:"",
            proName: "",
            lineName:"",
            abnomalStatus:"",
            responDepart:"",
            startDate:"",
            endDate:"",
            abnormalNo:""
        },
        VIEW_VERSION: VIEW_VERSION,      //版本号
        pageIndex: 1,
        pageSize: 10,
        tableData: [],                   //表格数据
        multipleSelection: [],
        delList: [],
        pageTotal: 0,
        dynamicFilter:{},
        form: {
            projectNo: "",
            lineName: "",
            classAB: "",
            fProcess: "",
            type: "",
            itemType: "",
            description: "",
            beginTime: "",
            endTime: "",
            id: "",
            createUserId:"",
            updateUserId:"",
            responDepart: "",
            responBy: "",
            status: "",
            abnormalNo: ""
        },
        idx: -1,
        loading: false,
        arrProName:[],                       //项目名称
        lines: [],
        arrLines:[],                         //线体
        arrAbnomalStatus: [],                //产线异常状态
        arrResponDepart:[],                  //责任部门
        arrClassAB:[],                       //班别
        arrFProcess:[],                      //工序站点
        arrAbnomalType:[],                   //异常大类
        arrAbnomalItemType:[],               //异常小类
        arrResponBy: [],                     //责任人
        useStore: null,
        batchDelete: [],                     //批量删除
        dialogTitle:"编辑异常",
        addDialogFormVisible: false,         //编辑窗口是否显示
        dialogType: 0,                       //0:编辑  1：添加
        arrLinesForm: [],                    //对话框线体
        timeoutId: null
      });
      onBeforeMount(() => {
      });
      onMounted(() => {
        //初始化项目名称
        data.arrProName.push({value : 0, label: '全部'});
        EnumConfig.EnumConfig.ProName.ArrProName.forEach(ele => {
            let item = {              
                value : ele.value, label: ele.label
            };
            data.arrProName.push(item);
        });
        data.lines = EnumConfig.EnumConfig.Lines.line;
        data.arrClassAB = EnumConfig.EnumConfig.ClassAB.arrClassAB;
        data.arrFProcess = EnumConfig.EnumConfig.FProcess.arrFProcess;
        data.arrAbnomalType = EnumConfig.EnumConfig.AbnormalType.arrAbnormalType;
        data.arrAbnomalItemType = EnumConfig.EnumConfig.AbnormalItemType.arrAbnormalItemType;
        data.arrAbnomalStatus = EnumConfig.EnumConfig.AbnomalStatus.arrAbnomalStatus;
        //获取当前登录用户的信息
        data.useStore = useStore();
        //初始化责任部门
        // data.arrResponDepart = EnumConfig.EnumConfig.ResponDepart.ArrResponDepart.unshift({value:0, label: "全部"});
        data.arrResponDepart = EnumConfig.EnumConfig.ResponDepart.ArrResponDepart;
      });
      onActivated(()=>{
      });
        /**
         * @description 获取表单信息
         * @author weig
         * @param
         */
        function getData (){
            var params = {
                "currentPage": data.pageIndex,
                "pageSize": data.pageSize,
                "filter.proName": data.query.proName,
                "filter.lineName": data.query.lineName,
                "filter.status": data.query.abnomalStatus,
                "filter.responDepart": data.query.responDepart,
                "filter.responBy": data.query.responBy,
                "filter.beginTime": data.query.startDate,
                "filter.endTime": data.query.endDate,
                "filter.abnormalNo": data.query.abnormalNo,
                "dynamicFilter": data.dynamicFilter
            }
            data.loading = true;
            // getTenantList(params).then(function(res){
            //     if(res.code == 1){
            //         data.pageTotal = res.data.total;//初始化列表数据总数
            //         data.tableData = res.data.list;
            //         //添加num序号字段
            //         data.tableData.forEach((data, i) => {
            //             data.num = i + 1;
            //         });
            //     } else {
            //         ElMessage.error(res.msg);   
            //     }
            //     data.loading = false;
            //     data.batchDelete = [];
            // });
        }
      /**
      * @description 对话框切换项目编号
      * @author weig     
      */
      const changeProNameHandleForm =()=>{
        //刷新线体
        data.arrLinesForm = [];
        data.arrLinesForm.push({value:0, label: "全部"});
        let arr = data.lines.filter((item, i)=>{
            if (item.proName == EnumConfig.EnumConfig.getEnumName(data.form.projectNo, EnumConfig.EnumConfig.ProNameJson)){
                return item;
            }
        });
        //数组合并
        data.arrLinesForm = data.arrLinesForm.concat(arr);  
      }
        //切换项目编号
        const changeProNameHandle = ()=>{
            //刷新线体
            data.arrLines = [];
            data.arrLines.push({value:0, label: "全部"});
            let arr = data.lines.filter((item, i)=>{
                if (item.proName == EnumConfig.EnumConfig.getEnumName(data.query.proName, EnumConfig.EnumConfig.ProNameJson)){
                    return item;
                }
            });
            //数组合并
            data.arrLines = data.arrLines.concat(arr);  
        }
        //日期选择范围
        const disabledDate1= (time) => {
            if (!IsNullOrEmpty(data.query.endDate)) {
                return time.getTime() > data.query.endDate;
            } 
            return time.getTime() > Date.now();
        }
        const disabledDate2= (time) => {
            if (!IsNullOrEmpty(data.query.startDate)){
                return time.getTime() < data.query.startDate || time.getTime() > Date.now();
            }
            return time.getTime() > Date.now();
        }
        
        /**
         * @description 搜索查询
         * @author weig
         * @param
        */
        const handleSearch=()=> {
            data.pageIndex = 1
            getData();
        }
        /**
         * @description 子组件返回分页数据
         * @author weig
         * @param {Object} obj
         */
        const resPageData = (obj) =>{
            data.pageIndex = obj.currPage;
            data.pageSize = obj.pageSize;
        }
        /**
         * @description 多选操作
         * @author weig
         * @param
         */
        const handleSelectionChange =(val)=> {
            data.multipleSelection = val;
            data.batchDelete = data.multipleSelection;
        }
        /**
         * @description 删除异常记录
         * @author weig
         * @param {Object} row 当前行数据
         */
        const handleClickDelete = (row)=>{
            let id = row.abnormalNo;
            if (IsNullOrEmpty(id)){
                ElMessage.error("Id不能为空，删除失败！");
                return;
            }
            elConfirmDialog(ElMessageBox,'此操作将永久删除该数据, 是否继续?','提示', '', ()=>{
                // SoftDeleteTenant(id).then(res =>{
                //     if (res.code == 1){
                //         ElMessage.success("删除成功");
                //         getData();
                //     } else {
                //         ElMessage.error("删除失败！");
                //     }
                // });
            }, ()=>{
                ElMessage.info("取消删除！");
            });
        }
        /**
         * @description 新增异常信息
         * @author weig
         */
        const handleClickAddData =()=>{
            data.addDialogFormVisible = true;
            data.dialogTitle = '新增异常';
            data.dialogType = 1;
        }
        /**
         * @description 批量删除
         * @author weig
         * @param
         */
        const handleClickBatchDelete = ()=>{
            if (data.multipleSelection.length == 0){//未选中
                ElMessage.error("请选择要删除的数据！");
            } else {
                var ids = data.multipleSelection.map(s =>{
                    return s.id;
                });
                ElMessageBox.confirm('此操作将删除选中的记录, 是否继续?', '提示',{
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(()=>{
                    // BatchSoftDeleteTenant(ids).then(res=>{
                    //     if (res.code == 1){
                    //         ElMessage.success("操作成功！");
                    //         getData();
                    //     } else {
                    //         ElMessage.error("操作失败！");
                    //     }
                    // }).catch(err=>{
                    //     ElMessage.error(err.msg);
                    // });
                }).catch((err)=>{
                    ElMessage.info("取消批量删除！");
                });
            }
        }
        /**
         * @description 编辑异常
         * @author weig
         * @param {Number} index 行号
         * @param {Object} row 行数据
         */
        const handleClickEditData = (index, row)=>{
            data.addDialogFormVisible = true;
            data.dialogTitle = '编辑异常';
            data.dialogType = 0;
            const rowData = JSON.parse(JSON.stringify(row));//深拷贝
            data.form ={
                projectNo: rowData.projectNo,
                lineName: rowData.lineName,
                classAB: rowData.classAB,
                fProcess: rowData.fProcess,
                type: rowData.type,
                itemType: rowData.itemType,
                description: rowData.description,
                beginTime: rowData.beginTime,
                endTime: rowData.endTime,
                responDepart: rowData.responDepart,
                responBy: rowData.responBy,
                status: rowData.status,
                id: rowData.id,
                abnormalNo: rowData.abnormalNo
            } ;
        }
        /**
         * @description 关闭编辑/新增对话框
         * @author weig
         * @param
         */
        const closeDialog = ()=>{
            data.addDialogFormVisible = false;
            refAddForm.value.resetFields();//清空表单
            //每次关闭对话框后清理表单历史数据
            data.form= {};
        }
        /**
         * @description 保存
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
            if (dialogType == 1){//新增
                params = {
                    projectNo: data.form.projectNo,
                    lineName: data.form.lineName,
                    classAB: data.form.classAB,
                    fProcess: data.form.fProcess,
                    type: data.form.type,
                    itemType: data.form.itemType,
                    description: data.form.description,
                    beginTime: data.form.beginTime,
                    // endTime: data.form.endTime,
                    responDepart: data.form.responDepart,
                    responBy: data.form.responBy,
                    status: EnumConfig.EnumConfig.AbnormalStatusJson.未处理,
                    createUserId: data.useStore.getters.userInfo.userId,
                    updateUserId: data.useStore.getters.userInfo.userId
                };
            } else if (data.dialogType == 0){//编辑
                params = {
                    projectNo: data.form.projectNo,
                    lineName: data.form.lineName,
                    classAB: data.form.classAB,
                    fProcess: data.form.fProcess,
                    type: data.form.type,
                    itemType: data.form.itemType,
                    description: data.form.description,
                    beginTime: data.form.beginTime,
                    // endTime: data.form.endTime,
                    abnormalNo: data.form.abnormalNo,
                    responDepart: data.form.responDepart,
                    responBy: data.form.responBy,
                    updateUserId: data.useStore.getters.userInfo.userId
                };
            }
            switch(data.dialogType){
                case 0://编辑
                    updateTenantInfo(params).then(res =>{
                        if(res.code == 1){
                            ElMessage.success("编辑成功");
                            data.addDialogFormVisible=false;
                            getData();
                        } else {
                            ElMessage.error(res.msg);
                        }
                    });
                    break;
                case 1://新增
                    addTenantInfo(params).then(res =>{
                        if(res.code == 1){
                            ElMessage.success("新增成功");
                            data.addDialogFormVisible=false;
                            getData();
                        } else {
                            ElMessage.error(res.msg);
                        }
                    });
                    break;
                default:
                    break;
            };
            data.form = {};//清空数据
        }
        /**
         * @description 校验新增/编辑异常数据
         * @author weig
         * @param 
         * @returns {Boolean} true
         */
        const validateFormData = ()=>{
            if (data.form.projectNo == 0){
                ElMessage.warning("项目名称不能为空！");
                return false;
            }
            if (data.form.lineName == 0 || IsNullOrEmpty(data.form.lineName)){
                ElMessage.warning("线体不能为空！");
                return false;
            }
            if (data.form.classAB == 0){
                ElMessage.warning("班别不能为空！");
                return false;
            }
            if (data.form.fProcess == 0){
                ElMessage.warning("工序站点不能为空！");
                return false;
            }
            if (data.form.type == 0){
                ElMessage.warning("异常大类不能为空！");
                return false;
            }
            if (data.form.itemType == 0){
                ElMessage.warning("异常小类不能为空！");
                return false;
            }
            if (data.form.responDepart == 0){
                ElMessage.warning("责任部门不能为空！");
                return false;
            }
            if (IsNullOrEmpty(data.form.responBy)){
                ElMessage.warning("责任人不能为空！");
                return false;
            }
            if (IsNullOrEmpty(data.form.beginTime)){
                ElMessage.warning("开始时间不能为空！");
                return false;
            }
            return true;
        }
        /**
         * @description 查询责任人
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
                data.arrResponBy = [];
                return;
            }          
            if (data.timeoutId){
                clearTimeout(data.timeoutId);
                data.timeoutId = null;
            }
            data.timeoutId = setTimeout(()=>{
                // getUserInfo(params.name).then(function(res){
                //     if(res.code == 1){
                //         data.options = [];
                //         if (res.data.user.length > 0){
                //             res.data.user.forEach((item)=>{
                //             data.options.push({
                //                 personLiableId: item.personLiableId,
                //                 name: item.name,
                //                 phone: item.phone,
                //                 department: item.department,
                //                 position: item.position
                //             });
                //             });
                //         }
                //     } else {
                //         ElMessage.error(res.msg);   
                //     }
                // });
            }, 500);
        });
        /**
         * @description 清空编辑或者添加对话框的查询条件
         * @author weig
         */
        const onClearDialog = () =>{
            data.arrResponBy.length = 0;
        }
        //导出
        // const handleClickExportData = ()=>{
        //     /**generate workbook object from table */
        //     var wb = XLSX.utils.table_to_book(document.querySelector("#out-table"));
        //     /**get binary string as output */
        //     var wbout = XLSX.write(wb, {bookType: 'xlsx', bookSST: true, type: 'array'});
        //     try{
        //         FileSaver.saveAs(new Blob([wbout], {type: 'application/octet-stream'}), '异常信息表.xlsx');
        //     }catch(e){
        //         if(typeof console !== 'undefined') console.log(e, wbout)
        //     }
        //     return wbout;
        // }
        return {
         ...toRefs(data),
         changeProNameHandle,
         disabledDate1,
         disabledDate2,
         handleSearch,
         resPageData,
         getData,
         handleSelectionChange,
         handleClickDelete,
         handleClickAddData,
         handleClickBatchDelete,
         handleClickEditData,
         closeDialog,
         refAddForm,
         multipleTable,
         changeProNameHandleForm,
         addDialogFormSave,
         remoteHandle,
         onClearDialog,
        //  handleClickExportData
      }
   },
}
</script>
<style scoped lang='scoped'>
</style>