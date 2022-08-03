<!--
 *@Description: 
 *@Author: weig
 *@Date: 2022-07-09 09:56:53
 *@LastEditors: weig
 *@LastEditTime: 2022-07-22 16:46:53
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
                <el-form :model="query" :inline="true" v-if="checkPermission([`api${VIEW_VERSION}:product:abnormal:getpage`])">
                <!-- <el-form :model="query" :inline="true" v-permission="[`api${VIEW_VERSION}:product:abnormal:getpage12121`]"> -->
                    <template v-if="checkPermission([`api${VIEW_VERSION}:product:abnormal:getpage`])">
                        <el-form-item label="异常单号">
                            <el-input v-model="query.abnormalNo" placeholder="异常单号" class="handle-input mr10"></el-input>
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
                            <el-select v-model="query.responDepart" placeholder="责任部门" class="handle-select mr10" @change="changeQueryDepartHandle">
                                <el-option v-for="item in arrResponDepart" :key="item.value" :label="item.label" :value="item.value"></el-option>
                            </el-select>
                        </el-form-item>
                        <el-form-item label="责任人">
                            <!-- <el-input v-model="query.responBy" placeholder="责任人" class="handle-input mr10"></el-input> -->
                            <el-select 
                            v-model="query.responBy" 
                            placeholder="请选择责任人"
                            >
                                <el-option
                                    v-for="item in arrResponBy"
                                    :key="item.personLiableId"
                                    :label="item.name"
                                    :value="item.personLiableId">
                                </el-option>
                            </el-select>
                        </el-form-item>
                        <el-form-item label="工站">
                            <el-select v-model="query.fProcess" placeholder="工站" class="handle-select mr10">
                                <el-option v-for="item in arrFProcess" :key="item.value" :label="item.label" :value="item.value"></el-option>
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
                    <!-- <el-form-item v-permission="[`api${VIEW_VERSION}:product:abnormal:add`]" v-if="checkPermission([`api${VIEW_VERSION}:product:abnormal:add`])"> -->
                    <el-form-item v-permission="[`api${VIEW_VERSION}:product:abnormal:add`]">
                        <el-button type="primary" icon="el-icon-plus" @click="handleClickAddData">新增</el-button>
                    </el-form-item>
                    <el-form-item v-if="checkPermission([`api${VIEW_VERSION}:product:abnormal:batchdelete`])">
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
                <el-table-column prop="responDepart" label="责任部门" width="120" align="center" :formatter="responDepartFormatter"/>
                <el-table-column prop="responName" label="责任人" align="center"></el-table-column>
                <el-table-column prop="beginTime" label="开始时间" align="center" min-width="160"></el-table-column>
                <el-table-column prop="endTime" label="结束时间" align="center" min-width="160"></el-table-column>
                <el-table-column prop="status" label="状态" align="center">
                    <template #default="{row}">
                        <el-tag :type="row.status == EnumConfig.EnumConfig.AbnormalStatusJson.已处理 ? 'success' : 'danger'" disable-transitions>
                            {{ EnumConfig.EnumConfig.getEnumName(row.status, EnumConfig.EnumConfig.AbnormalStatusJson)}}
                        </el-tag>
                    </template>
                </el-table-column>
                <el-table-column label="操作" width="240" align="center" fixed="right" v-if="checkPermission([`api${VIEW_VERSION}:product:abnormal:update`,`api${VIEW_VERSION}:product:abnormal:get`])">
                    <template #default="{ $index, row }">
                        <el-dropdown  
                        split-button
                        type="primary" 
                        style="margin-left:10px;" 
                        @click="handleClickEditData($index, row)" 
                        @command="(command)=>onCommand(command,row)"
                        >
                            编辑
                            <template #dropdown v-if="checkPermission([`api${VIEW_VERSION}:product:abnormal:update`])">
                                <el-dropdown-menu :visible-arrow="false" style="margin-top: 2px;width:100px;text-align:center;">
                                    <el-dropdown-item command="handle" :disabled="cmdDisabledFunc($index, row)" v-if="checkPermission([`api${VIEW_VERSION}:product:abnormal:updateabnormalhandle`])">处理异常</el-dropdown-item>
                                    <el-dropdown-item command="delete" :disabled="cmdDisabledFunc($index, row)" v-if="checkPermission([`api${VIEW_VERSION}:product:abnormal:delete`])">删除异常</el-dropdown-item>
                                    <el-dropdown-item command="handletime" v-if="checkPermission([`api${VIEW_VERSION}:product:abnormal:updateabnormalhandletime`])">结束时间</el-dropdown-item>
                                </el-dropdown-menu>
                            </template>
                        </el-dropdown>
                        <el-button
                            type="primary"
                            icon="el-icon-check"
                            @click="getAbnormalDetails($index, row)"
                            class="ml10"
                            v-if="checkPermission([`api${VIEW_VERSION}:product:abnormal:get`])"
                        >详情
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
        <div v-elDragDialog>
            <el-dialog       
            :title="dialogTitle"
            v-model="addDialogFormVisible"
            :close-on-click-modal="false"
            width="60%"
            @close="closeDialog"
            :fullscreen="dialogFull">
                <el-form
                    ref="refAddForm"
                    :model="form"
                    label-width="120px"
                    :inline="false"        
                >
                    <el-row>
                        <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                            <el-form-item label="项目编号" prop="projectNo" required>
                                <el-select v-model="form.projectNo" placeholder="请选择项目编号" style="width:100%;" @change="changeProNameHandleForm" :disabled="dialogType == 1 ? false : true">
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
                                <el-select v-model="form.lineName" placeholder="请选择线体" style="width:100%;" :disabled="dialogType == 1 ? false : true">
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
                                <!-- <el-select v-model="form.fProcess" placeholder="请选择工序站点" style="width:100%;">
                                <el-option
                                    v-for="item in arrFProcess"
                                    :key="item.value"
                                    :label="item.label"
                                    :value="item.value"
                                />
                                </el-select> -->
                                <el-select 
                                v-model="form.fProcess" 
                                filterable 
                                placeholder="请选择工序站点"
                                >
                                    <el-option
                                    v-for="item in arrFProcess"
                                    :key="item.value"
                                    :label="item.label"
                                    :value="item.value"
                                    >
                                    </el-option>
                                </el-select>
                            </el-form-item>
                        </el-col>
                    </el-row>
                    <el-row>
                        <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                            <el-form-item label="异常大类" prop="type" required>
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
                            <el-form-item label="异常小类" prop="itemType" required>
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
                                <el-select v-model="form.responDepart" placeholder="请选择责任部门" style="width:100%;" :disabled="dialogType == 1 ? false : true" @change="changeEditDepartHandle">
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
                                placeholder="请选择责任人"
                                :disabled="dialogType == 1 ? false : true"
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
                                <el-date-picker v-model="form.beginTime" type="datetime" placeholder="选择日期" :disabledDate="disabledDate3">
                                </el-date-picker>
                            </el-form-item>
                        </el-col>
                    </el-row>
                    <el-row>
                        <el-col :xs="24" :sm="24" :md="18" :lg="18" :xl="18">
                        <el-form-item label="异常描述" prop="description" required>
                            <el-input v-model="form.description" type="textarea" :rows="2" auto-complete="off" placeholder="请输入异常描述！"/>
                        </el-form-item>
                        </el-col>
                    </el-row>
                </el-form>
                <template #title>
                    <div class="avue-crud__dialog__header">
                        <span class="el-dialog__title">
                        <span style="display:inline-block;width:3px;height:20px;margin-right:5px; float: left;margin-top:2px"></span>
                        {{dialogTitle}}
                        </span>
                        <div class="avue-crud__dialog__menu" @click="dialogFull? dialogFull=false: dialogFull=true">
                            <i class="el-icon-full-screen" title="全屏" v-if="!dialogFull"></i>
                            <i class="el-icon-copy-document" title="缩小" v-else></i>
                        </div>
                    </div>
                </template>
                <template #footer>
                    <span class="dialog-footer">
                    <el-button @click="closeDialog()">取 消</el-button>
                    <el-button type="primary" @click="addDialogFormSave()">确 定</el-button>
                    </span>
                </template>
            </el-dialog>
        </div>
        <!-- 添加/编辑窗口 end -->
        <!-- 详情窗口 begin -->
        <div v-elDragDialog>
            <el-dialog 
                title="异常详情" 
                :close-on-click-modal="false"
                :destroy-on-close="true"
                v-model="detailsVisible"
                width="70%"
                @close="closeDetailsDialog"
                >
                <div class="home-container">
                    <el-form
                        ref="refDetailsForm"
                        :model="formDetails"
                        label-width="80px"
                        :inline="false"        
                    >
                        <!-- <el-divider content-position="left"><h3>详情信息</h3> -->
                            <el-row>
                                <el-col :xs="24" :sm="24" :md="12" :lg="8" :xl="6">
                                    <el-form-item label="项目编号" prop="projectNo"  label-width="100px">
                                    <input v-model="formDetails.projectNo" class="inputin" :disabled="true" />
                                    </el-form-item>
                                </el-col>
                                <el-col :xs="24" :sm="24" :md="12" :lg="8" :xl="6">
                                    <el-form-item label="线体" prop="lineName"  label-width="100px">
                                    <input v-model="formDetails.lineName" class="inputin" :disabled="true" />
                                    </el-form-item>
                                </el-col>
                                <el-col :xs="24" :sm="24" :md="12" :lg="8" :xl="6">
                                    <el-form-item label="班别" prop="classAB"  label-width="100px">
                                    <input v-model="formDetails.classAB" class="inputin" :disabled="true" />
                                    </el-form-item>
                                </el-col>
                                <el-col :xs="24" :sm="24" :md="12" :lg="8" :xl="6">
                                    <el-form-item label="工序站点" prop="fProcess"  label-width="100px">
                                    <input v-model="formDetails.fProcess" class="inputin" :disabled="true" />
                                    </el-form-item>
                                </el-col>
                                <el-col :xs="24" :sm="24" :md="12" :lg="8" :xl="6">
                                    <el-form-item label="异常大类" prop="type"  label-width="100px">
                                    <input v-model="formDetails.type" class="inputin" :disabled="true" />
                                    </el-form-item>
                                </el-col>
                                <el-col :xs="24" :sm="24" :md="12" :lg="8" :xl="6">
                                    <el-form-item label="异常小类" prop="itemType"  label-width="100px">
                                    <input v-model="formDetails.itemType" class="inputin" :disabled="true" />
                                    </el-form-item>
                                </el-col>
                                <el-col :xs="24" :sm="24" :md="12" :lg="8" :xl="6">
                                    <el-form-item label="责任部门" prop="responDepart"  label-width="100px">
                                    <input v-model="formDetails.responDepart" class="inputin" :disabled="true" />
                                    </el-form-item>
                                </el-col>
                                <el-col :xs="24" :sm="24" :md="12" :lg="8" :xl="6">
                                    <el-form-item label="责任人" prop="responBy"  label-width="100px">
                                    <input v-model="formDetails.responName" class="inputin" :disabled="true" />
                                    </el-form-item>
                                </el-col>
                                <el-col :xs="24" :sm="24" :md="12" :lg="8" :xl="6">
                                    <el-form-item label="开始时间" prop="beginTime"  label-width="100px">
                                    <input v-model="formDetails.beginTime" class="inputin" :disabled="true" />
                                    </el-form-item>
                                </el-col>
                                <el-col :xs="24" :sm="24" :md="12" :lg="8" :xl="6">
                                    <el-form-item label="结束时间" prop="endTime"  label-width="100px">
                                    <input v-model="formDetails.endTime" class="inputin" :disabled="true" />
                                    </el-form-item>
                                </el-col>
                            </el-row>
                            <el-row>
                                <el-col :xs="24" :sm="24" :md="12" :lg="8" :xl="6">
                                    <el-form-item label="异常描述" prop="description"  label-width="100px">
                                    <input v-model="formDetails.description" class="inputin" :disabled="true" />
                                    </el-form-item>
                                </el-col>
                            </el-row>
                            <el-row v-if="formDetails.status">
                                <el-col :xs="24" :sm="24" :md="12" :lg="8" :xl="6">
                                    <el-form-item label="原因分析" prop="reason"  label-width="100px">
                                    <input v-model="formDetails.reason" class="inputin" :disabled="true" />
                                    </el-form-item>
                                </el-col>
                            </el-row>
                            <el-row v-if="formDetails.status">
                                <el-col :xs="24" :sm="24" :md="12" :lg="8" :xl="6">
                                    <el-form-item label="临时对策" prop="tempMeasures"  label-width="100px">
                                    <input v-model="formDetails.tempMeasures" class="inputin" :disabled="true" />
                                    </el-form-item>
                                </el-col>
                            </el-row>
                            <el-row v-if="formDetails.status">
                                <el-col :xs="24" :sm="24" :md="12" :lg="8" :xl="6">
                                    <el-form-item label="长期对策" prop="fundaMeasures"  label-width="100px">
                                    <input v-model="formDetails.fundaMeasures" class="inputin" :disabled="true" />
                                    </el-form-item>
                                </el-col>
                            </el-row>
                        <!-- </el-divider> -->
                    </el-form>
                </div>
            </el-dialog>
        </div>
        <!-- 详情窗口 end -->
        <!-- 处理异常窗口 begin -->
        <div v-elDragDialog>
            <el-dialog 
                title="处理异常" 
                :close-on-click-modal="false"
                :destroy-on-close="true"
                v-model="handleAbnormalVisible"
                width="70%"
                @close="closeHandleDialog"
                :fullscreen="dialogFull"
                >
                <el-form
                    ref="refHandleForm"
                    :model="handleForm"
                    label-width="120px"
                    :inline="false"        
                >
                    <el-row>
                        <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                            <el-form-item label="项目编号" prop="proName" required>
                                <el-select v-model="handleForm.proName" placeholder="请选择项目编号" style="width:100%;" @change="changeProNameHandleForm" :disabled="true">
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
                                <el-select v-model="handleForm.lineName" placeholder="请选择线体" style="width:100%;" :disabled="true">
                                <el-option
                                    v-for="item in arrLinesForm"
                                    :key="item.value"
                                    :label="item.label"
                                    :value="item.value"
                                />
                                </el-select>
                            </el-form-item>
                        </el-col>
                        <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                            <el-form-item label="班别" prop="classAB" required>
                                <el-select v-model="handleForm.classAB" placeholder="请选择班别" style="width:100%;" :disabled="true">
                                <el-option
                                    v-for="item in arrClassAB"
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
                            <el-form-item label="工序站点" prop="fProcess" required>
                                <el-select v-model="handleForm.fProcess" placeholder="请选择工序站点" style="width:100%;" :disabled="true">
                                <el-option
                                    v-for="item in arrFProcess"
                                    :key="item.value"
                                    :label="item.label"
                                    :value="item.value"
                                />
                                </el-select>
                            </el-form-item>
                        </el-col>
                        <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                            <el-form-item label="责任部门" prop="responDepart" required>
                                <el-select v-model="handleForm.responDepart" placeholder="请选择责任部门" style="width:100%;" :disabled="true">
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
                                v-model="handleForm.responBy" 
                                filterable 
                                placeholder="请输入责任人/手机号码"
                                :remote-method="remoteHandle"
                                remote
                                :clearable="true"
                                @clear="onClearDialog"
                                :disabled="true"
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
                        <el-form-item label="原因分析" prop="reason" required>
                            <el-input v-model="handleForm.reason" type="textarea" :rows="2" auto-complete="off" placeholder="请填写原因分析！" />
                        </el-form-item>
                        </el-col>
                    </el-row>
                    <el-row>
                        <el-col :xs="24" :sm="24" :md="18" :lg="18" :xl="18">
                        <el-form-item label="临时对策" prop="tempMeasures" required>
                            <el-input v-model="handleForm.tempMeasures" type="textarea" :rows="2" auto-complete="off" placeholder="请填写临时对策！"/>
                        </el-form-item>
                        </el-col>
                    </el-row>
                    <el-row>
                        <el-col :xs="24" :sm="24" :md="18" :lg="18" :xl="18">
                        <el-form-item label="长期对策" prop="fundaMeasures" required>
                            <el-input v-model="handleForm.fundaMeasures" type="textarea" :rows="2" auto-complete="off" placeholder="请填写长期对策！"/>
                        </el-form-item>
                        </el-col>
                    </el-row>
                    <!-- <el-row>
                        <el-col :xs="24" :sm="24" :md="18" :lg="18" :xl="18">
                            <el-form-item label="结束时间" required>
                                <el-date-picker v-model="handleForm.endTime" type="datetime" placeholder="选择日期">
                                </el-date-picker>
                            </el-form-item>
                        </el-col>
                    </el-row> -->
                </el-form>
                <template #title>
                    <div class="avue-crud__dialog__header">
                        <span class="el-dialog__title">
                        <span style="display:inline-block;width:3px;height:20px;margin-right:5px; float: left;margin-top:2px"></span>
                        {{dialogTitle}}
                        </span>
                        <div class="avue-crud__dialog__menu" @click="dialogFull? dialogFull=false: dialogFull=true">
                            <i class="el-icon-full-screen" title="全屏" v-if="!dialogFull"></i>
                            <i class="el-icon-copy-document" title="缩小" v-else></i>
                        </div>
                    </div>
                </template>
                <template #footer>
                    <span class="dialog-footer">
                    <el-button @click="closeHandleDialog()">取 消</el-button>
                    <el-button type="primary" @click="handleDialogFormSave()">确 定</el-button>
                    </span>
                </template>
            </el-dialog>
        </div>
        <!-- 详情窗口 end -->
        <!-- 处理异常窗口 begin -->
        <div v-elDragDialog>
            <el-dialog 
                title="编辑异常结束时间" 
                :close-on-click-modal="false"
                :destroy-on-close="true"
                v-model="handleTimeAbnormalVisible"
                width="50%"
                @close="closeHandleTimeDialog"
                :fullscreen="dialogFull"
                >
                <el-form
                    ref="refHandleTimeForm"
                    :model="handleTimeForm"
                    label-width="120px"
                    :inline="false"        
                >
                    <el-row>
                        <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                            <el-form-item label="项目编号" prop="proName" required>
                                <el-select v-model="handleTimeForm.proName" placeholder="请选择项目编号" style="width:100%;" @change="changeProNameHandleForm" :disabled="true">
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
                                <el-select v-model="handleTimeForm.lineName" placeholder="请选择线体" style="width:100%;" :disabled="true">
                                <el-option
                                    v-for="item in arrLinesForm"
                                    :key="item.value"
                                    :label="item.label"
                                    :value="item.value"
                                />
                                </el-select>
                            </el-form-item>
                        </el-col>
                        <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                            <el-form-item label="班别" prop="classAB" required>
                                <el-select v-model="handleTimeForm.classAB" placeholder="请选择班别" style="width:100%;" :disabled="true">
                                <el-option
                                    v-for="item in arrClassAB"
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
                            <el-form-item label="工序站点" prop="fProcess" required>
                                <el-select v-model="handleTimeForm.fProcess" placeholder="请选择工序站点" style="width:100%;" :disabled="true">
                                <el-option
                                    v-for="item in arrFProcess"
                                    :key="item.value"
                                    :label="item.label"
                                    :value="item.value"
                                />
                                </el-select>
                            </el-form-item>
                        </el-col>
                        <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                            <el-form-item label="责任部门" prop="responDepart" required>
                                <el-select v-model="handleTimeForm.responDepart" placeholder="请选择责任部门" style="width:100%;" :disabled="true">
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
                                v-model="handleTimeForm.responBy" 
                                filterable 
                                placeholder="请输入责任人/手机号码"
                                :remote-method="remoteHandle"
                                remote
                                :clearable="true"
                                @clear="onClearDialog"
                                :disabled="true"
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
                            <el-form-item label="结束时间" required>
                                <el-date-picker v-model="handleTimeForm.endTime" type="datetime" placeholder="选择日期" format="YYYY-MM-DD HH:mm:ss">
                                </el-date-picker>
                            </el-form-item>
                        </el-col>
                    </el-row>
                </el-form>
                <template #title>
                    <div class="avue-crud__dialog__header">
                        <span class="el-dialog__title">
                        <span style="display:inline-block;width:3px;height:20px;margin-right:5px; float: left;margin-top:2px"></span>
                        {{dialogTitle}}
                        </span>
                        <div class="avue-crud__dialog__menu" @click="dialogFull? dialogFull=false: dialogFull=true">
                            <i class="el-icon-full-screen" title="全屏" v-if="!dialogFull"></i>
                            <i class="el-icon-copy-document" title="缩小" v-else></i>
                        </div>
                    </div>
                </template>
                <template #footer>
                    <span class="dialog-footer">
                    <el-button @click="closeHandleTimeDialog()">取 消</el-button>
                    <el-button type="primary" @click="handleTimeDialogFormSave()">确 定</el-button>
                    </span>
                </template>
            </el-dialog>
        </div>
        <!-- 详情窗口 end -->
   </div>
</template>
<script>
import { reactive, toRefs, onBeforeMount, onMounted, onActivated, ref } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { useStore } from 'vuex'
import { elConfirmDialog, IsNullOrEmpty, formatDateTime, dateTimeCompare } from "@/common/js/comm"
import EupCrumbs from "../../../components/eup-crumbs/index.vue"
import EnumConfig from "../../../enum/EnumConfig"
import EupPagination from "@/components/EupPagination.vue"
// import elDragDialog from "@/directive/el-dragDialog";
import FileSaver from 'file-saver'
import XLSX from 'xlsx'
import {abnormalGetPage, abnormalGet, abnormalAdd,abnormalBatchDelete,abnormalDelete,abnormalUpdate,
getAbnormalPersonInfo,updateAbnormalHandle,UpdateAbnormalHandleTime,GetResponByListByDepart} from '../../../serviceApi/Abnormal/Abnormal'
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
      const refDetailsForm = ref(null);
      const refHandleForm = ref(null);
      const refHandleTimeForm = ref(null);
      const data = reactive({
        query: {                         //查询条件
            responBy:"",
            proName: "",
            lineName:"",
            abnomalStatus:0,
            responDepart:0,
            startDate:"",
            endDate:"",
            abnormalNo:"",
            fProcess:""
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
            type: 0,
            itemType: 0,
            description: "",
            beginTime: "",
            endTime: "",
            id: "",
            createUserId:"",
            updateUserId:"",
            responDepart: 0,
            responBy: "",
            responName: "",
            status: 2,
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
        timeoutId: null,
        detailsVisible: false,
        handleAbnormalVisible: false,
        handleTimeAbnormalVisible: false,
        handleForm: {
            proName: "",
            lineName: "",
            classAB: "",
            fProcess: "",
            responDepart: 0,
            responBy: "",
            endTime:"",
            reason: "",
            tempMeasures: "",
            fundaMeasures: "",
            abnormalNo:""
        },
        formDetails: {},
        handleTimeForm:{},
        dialogFull: false, //是否为全屏 Dialog
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
        data.arrResponDepart = EnumConfig.EnumConfig.ResponDepart.ArrResponDepart;
        getData();
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
                "filter.abnomalStatus": data.query.abnomalStatus,
                "filter.responDepart": data.query.responDepart,
                "filter.responBy": data.query.responBy,
                "filter.startDate": IsNullOrEmpty(data.query.startDate) ? data.query.startDate: formatDateTime(data.query.startDate, "yyyy-MM-dd"),
                "filter.endDate": IsNullOrEmpty(data.query.endDate) ? data.query.endDate: formatDateTime(data.query.endDate, "yyyy-MM-dd"),
                "filter.abnormalNo": data.query.abnormalNo,
                "filter.fProcess": data.query.fProcess,
                "dynamicFilter": data.dynamicFilter
            }
            data.loading = true;
            abnormalGetPage(params).then(function(res){
                if(res.code == 1){
                    data.pageTotal = res.data.total;//初始化列表数据总数
                    data.tableData = res.data.list;
                    //添加num序号字段
                    data.tableData.forEach((data, i) => {
                        data.num = i + 1;
                    });
                } else {
                    ElMessage.error(res.msg);   
                }
                data.loading = false;
                data.batchDelete = [];
            });
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
        const disabledDate= (time) => {
            return time.getTime() > Date.now();
        }
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
         * 只能选择今天
         * @param {*} time 
         */
        const disabledDate3 =(time) =>{
            // return time.getTime() > Date.now() || time.getTime() < Date.now()
            //自然白班
            if (Date.now > new Date(formatDateTime(new Date(), "yyyy-MM-dd " +"08:00")).getTime() && Date.now < new Date(formatDateTime(new Date(), "yyyy-MM-dd " +"20:00")).getTime()){
                return time.getTime() > new Date(formatDateTime(new Date(), "yyyy-MM-dd " +"08:00" )).getTime() || time.getTime() < new Date(formatDateTime(new Date(), "yyyy-MM-dd " +"20:00")).getTime();
            } else{
                return time.getTime() > new Date(formatDateTime(new Date(), "yyyy-MM-dd " +"20:00" )).getTime() || time.getTime() < new Date(formatDateTime(new Date(Date.now() + 86400000), "yyyy-MM-dd " + "08:00")).getTime();
            }
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
                abnormalDelete(id).then(res =>{
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
         * @description 新增异常信息
         * @author weig
         */
        const handleClickAddData =()=>{
            data.addDialogFormVisible = true;
            data.dialogTitle = '新增异常';
            data.dialogType = 1;
            data.form.beginTime = formatDateTime(new Date(), "yyyy-MM-dd HH:mm:ss");
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
                    abnormalBatchDelete(ids).then(res=>{
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
         * @description 编辑异常
         * @author weig
         * @param {Number} index 行号
         * @param {Object} row 行数据
         */
        const handleClickEditData = (index, row)=>{
            //限制只能创建人或者责任人才能做编辑操作
            if (data.useStore.getters.userInfo.userId != row.createUserId){
                ElMessage.warning("啊哦~ 你没有权限访问该页面哦")
                return;
            }
            //已经处理状态下不能编辑了
            if (EnumConfig.EnumConfig.AbnormalStatusJson.已处理 == row.status){
                ElMessage.warning("靓仔~ 该异常信息已经处理了哦~~")
                return;
            }
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
                responBy: rowData.responName ? rowData.responName : rowData.responBy,
                responName: rowData.responName,
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
            data.dialogFull = false;
        }
        /**
         * @description 关闭详情对话框
         * @author weig
         * @param
         */
        const closeDetailsDialog = ()=>{
            data.detailsVisible = false;
            refDetailsForm.value.resetFields();//清空表单
            //每次关闭对话框后清理表单历史数据
            data.form= {};
            data.dialogFull = false;
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
            if (data.dialogType == 1){//新增
                params = {
                    projectNo: data.form.projectNo,
                    lineName: data.form.lineName,
                    classAB: data.form.classAB,
                    fProcess: data.form.fProcess,
                    type: data.form.type,
                    itemType: data.form.itemType,
                    description: data.form.description,
                    beginTime: data.form.beginTime,
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
                    abnormalNo: data.form.abnormalNo,
                    responDepart: data.form.responDepart,
                    responBy: data.form.responBy,
                    updateUserId: data.useStore.getters.userInfo.userId,
                    createUserId: data.useStore.getters.userInfo.userId,
                };
            }
            switch(data.dialogType){
                case 0://编辑
                    abnormalUpdate(params).then(res =>{
                        if(res.code == 1){
                            ElMessage.success("编辑成功");
                            data.addDialogFormVisible=false;
                            getData();
                            data.form = {};//清空数据
                        } else {
                            ElMessage.error(res.msg);
                        }
                    });
                    break;
                case 1://新增
                    abnormalAdd(params).then(res =>{
                        if(res.code == 1){
                            ElMessage.success("新增成功");
                            data.addDialogFormVisible=false;
                            getData();
                            data.form = {};//清空数据
                        } else {
                            ElMessage.error(res.msg);
                        }
                    });
                    break;
                default:
                    break;
            };
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
            if (data.form.classAB == "全部" || data.form.classAB == 0){
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
            if (IsNullOrEmpty(data.form.description)){
                ElMessage.warning("异常描述不能为空！");
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
            if ((data.form.responDepart == EnumConfig.EnumConfig.ResponDepartJson.全部 && data.addDialogFormVisible==true) || data.query.responDepart == EnumConfig.EnumConfig.ResponDepartJson.全部){
                ElMessage.warning("请先选择部门！");   
                return;
            }
            let params = {
                name: value,
                responDepart: data.addDialogFormVisible ? data.form.responDepart : data.query.responDepart
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
                getAbnormalPersonInfo(params).then(function(res){
                    if(res.code == 1){
                        data.options = [];
                        if (res.data.length > 0){
                            res.data.forEach((item)=>{
                                data.arrResponBy.push({
                                    personLiableId: item.personLiableId,
                                    name: item.name,
                                    phone: item.phone,
                                    department: item.department,
                                    position: item.position
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
        /**
         * @description 更多操作
         * @author weig
         * @param {String} command 指令名称
         * @param {Object} row 行对象数据
         */
        function onCommand(command, row) {
            if (command === 'handle') {
                handleAbnormalInfo(row);
            } else if (command == "delete"){
                handleClickDelete(row)
            } else if (command == "handletime"){
                handleClickTime(row);
            }
        }
        /**
         * @description 处理异常信息
         * @author weig
         * @param {Object} row 行对象数据
         */
        const handleAbnormalInfo = (row)=>{
            data.handleAbnormalVisible = true;
            data.handleForm ={
                proName: row.projectNo,
                lineName: row.lineName,
                classAB: row.classAB,
                fProcess: row.fProcess,
                responDepart: row.responDepart,
                responBy: row.responName,
                // endTime:"",
                reason: "",
                tempMeasures: "",
                fundaMeasures: "",
                abnormalNo: row.abnormalNo
            };
        }
        /**
         * @description 编辑异常结束时间，只有创建者才能编辑
         * @author weig
         * @param {Object} row 行对象数据
         */
        const handleClickTime = (row)=>{
            if (row.createUserId != data.useStore.getters.userInfo.userId){
                ElMessage.warning("啊哦~ 你没有权限访问该页面哦")
                return;
            }
            data.handleTimeAbnormalVisible = true;
            data.handleTimeForm ={
                proName: row.projectNo,
                lineName: row.lineName,
                classAB: row.classAB,
                fProcess: row.fProcess,
                responDepart: row.responDepart,
                responBy: row.responName,
                endTime: row.endTime,
                beginTime: row.beginTime,
                abnormalNo: row.abnormalNo
            };
        }
        /**
         * @description 获取异常详细信息
         * @author weig
         * @param {number} index 
         * @param {Object} row 行对象数据
         */
        const getAbnormalDetails = (index, row)=>{
            const rowData = JSON.parse(JSON.stringify(row));//深拷贝
            data.formDetails ={//初始化详情数据
                projectNo: rowData.projectNo,
                lineName: rowData.lineName,
                classAB: rowData.classAB,
                fProcess: rowData.fProcess,
                type: EnumConfig.EnumConfig.getEnumName(rowData.type, EnumConfig.EnumConfig.AbnormalTypeJson),
                itemType: EnumConfig.EnumConfig.getEnumName(rowData.itemType, EnumConfig.EnumConfig.AbnormalItemTypeJson),
                description: rowData.description,
                beginTime: rowData.beginTime,
                endTime: rowData.endTime,
                responDepart: EnumConfig.EnumConfig.getEnumName(rowData.responDepart, EnumConfig.EnumConfig.ResponDepartJson),
                responBy: rowData.responName ? rowData.responName : rowData.responBy,
                responName: rowData.responName,
                status: rowData.status,
                id: rowData.id,
                abnormalNo: rowData.abnormalNo,
                reason: rowData.reason,
                tempMeasures: rowData.tempMeasures,
                fundaMeasures: rowData.fundaMeasures
            } ;
            data.detailsVisible = true;
        }
        /**
         * @description 责任部门格式化
         * @author weig
         * @param {Object} row 行对象数据
         * @returns {string}
         */
        const responDepartFormatter = (row) =>{
            return EnumConfig.EnumConfig.getEnumName(row.responDepart, EnumConfig.EnumConfig.ResponDepartJson);
        }
        /**
         * @description 关闭处理异常窗口
         * @author weig
         */
        const closeHandleDialog =()=>{
            data.handleAbnormalVisible = false;
            refHandleForm.value.resetFields();//清空表单
            //每次关闭对话框后清理表单历史数据
            data.handleForm= {};
            data.dialogFull = false;
        }
        /**
         * @description 关闭处理异常窗口
         * @author weig
         */
        const closeHandleTimeDialog =()=>{
            data.handleTimeAbnormalVisible = false;
            refHandleTimeForm.value.resetFields();//清空表单
            //每次关闭对话框后清理表单历史数据
            data.handleTimeForm= {};
            data.dialogFull = false;
        }
        /**
         * @description 保存处理信息
         * @author weig
         * @param
         */
        const handleDialogFormSave = ()=>{
            if (IsNullOrEmpty(data.handleForm.reason)){
                ElMessage.warning("原因分析不能为空！")
                return;
            }
            if (IsNullOrEmpty(data.handleForm.tempMeasures)){
                ElMessage.warning("临时对策不能为空！")
                return;
            }
            if (IsNullOrEmpty(data.handleForm.fundaMeasures)){
                ElMessage.warning("长期对策不能为空！")
                return;
            }
            // if (IsNullOrEmpty(data.handleForm.endTime)){
            //     ElMessage.warning("结束时间不能为空！")
            //     return;
            // }
            let params ={
                abnormalNo: data.handleForm.abnormalNo,
                reason: data.handleForm.reason,
                tempMeasures: data.handleForm.tempMeasures,
                fundaMeasures: data.handleForm.fundaMeasures,
                // endTime: data.handleForm.endTime
            }
            updateAbnormalHandle(params).then((res)=>{
                if (res.code ==1){
                    ElMessage.success(res.msg);
                    data.handleAbnormalVisible = false; 
                    refHandleForm.value.resetFields();
                    data.handleForm = {};
                    getData();
                } else {
                    ElMessage.error(res.msg);
                }
            }).catch((e)=>{
                ElMessage.error("异常处理失败！");
            });       
        }

        /**
         * @description 更新异常结束时间
         * @author weig
         * @param
         */
        const handleTimeDialogFormSave = ()=>{
            if (IsNullOrEmpty(data.handleTimeForm.endTime)){
                ElMessage.warning("结束时间不能为空！")
                return;
            }
            if (dateTimeCompare(data.handleTimeForm.beginTime, data.handleTimeForm.endTime)){
                ElMessage.warning("结束时间不能小于开始时间！")
                return;
            }
            let params ={
                abnormalNo: data.handleTimeForm.abnormalNo,
                endTime: formatDateTime(data.handleTimeForm.endTime, "yyyy-MM-dd HH:mm:ss")
            }
            UpdateAbnormalHandleTime(params).then((res)=>{
                if (res.code ==1){
                    ElMessage.success(res.msg);
                    data.handleTimeAbnormalVisible = false; 
                    refHandleTimeForm.value.resetFields();
                    data.handleTimeForm = {};
                    getData();
                } else {
                    ElMessage.error(res.msg);
                }
            }).catch((e)=>{
                ElMessage.error("更新异常结束时间失败！");
            });       
        }
        /**
         * @description 下拉指令禁用
         * @author weig
         * @param {number} index 行号
         * @param {Json} row 行数据
         * @returns {boolean} disabled
        */
        const cmdDisabledFunc = (index, row)=>{
            if(row.status == EnumConfig.EnumConfig.AbnormalStatusJson.已处理 || data.useStore.getters.userInfo.userId != row.createUserId){
                return true;
            }
            return false;
        }
        /**
         * @description 查询部门切换
         * @author weig
         * @param {number} value 部门
        */
        const changeQueryDepartHandle = (value)=>{
            if (value == 0){
                data.arrResponBy = [];
            }
            var params = {
                responDepart: value
            }
            GetResponByListByDepart(params).then((res)=>{
                if (res.code ==1){
                    data.arrResponBy = res.data;
                } else {
                    ElMessage.error(res.msg);
                }
            }).catch((e)=>{
                ElMessage.error("获取责任人失败！");
            });  
        }
        /**
         * @description 编辑部门切换
         * @author weig
         * @param {number} value 部门
        */
        const changeEditDepartHandle = (value)=>{
            if (value == 0){
                data.arrResponBy = [];
            }
            var params = {
                responDepart: value
            }
            GetResponByListByDepart(params).then((res)=>{
                if (res.code ==1){
                    data.arrResponBy = res.data;
                } else {
                    ElMessage.error(res.msg);
                }
            }).catch((e)=>{
                ElMessage.error("获取责任人失败！");
            });  
        }
        
        return {
            ...toRefs(data),
            changeProNameHandle,
            disabledDate,
            disabledDate1,
            disabledDate2,
            disabledDate3,
            handleSearch,
            resPageData,
            getData,
            handleSelectionChange,
            handleClickAddData,
            handleClickBatchDelete,
            handleClickEditData,
            closeDialog,
            closeDetailsDialog,
            closeHandleTimeDialog,
            refAddForm,
            refDetailsForm,
            refHandleForm,
            refHandleTimeForm,
            multipleTable,
            changeProNameHandleForm,
            addDialogFormSave,
            remoteHandle,
            onClearDialog,
            //  handleClickExportData
            EnumConfig,
            onCommand,
            getAbnormalDetails,
            responDepartFormatter,
            closeHandleDialog,
            handleDialogFormSave,
            handleTimeDialogFormSave,
            cmdDisabledFunc,
            changeQueryDepartHandle,
            changeEditDepartHandle
      }
   },
}
</script>
<style scoped lang='scoped'>
</style>