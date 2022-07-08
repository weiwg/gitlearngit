<template>
    <div>
        <!-- 面包屑 begin -->
        <eup-crumbs icon="el-icon-s-management" firstCrumbs="系统管理" secondCrumbs="地区管理" />
        <!-- 面包屑 end -->

        <!-- 内容区域 begin -->
        <div class="container">
            <div class="handle-box">   
              <template v-if="checkPermission([`api${state.VIEW_VERSION}:System:SysRegion:GetPage`,`api${state.VIEW_VERSION}:System:SysRegion:Add`,`api${state.VIEW_VERSION}:System:SysRegion:BatchSoftDelete`])">
                <template v-if="checkPermission([`api${state.VIEW_VERSION}:System:SysRegion:GetPage`])">
                  <el-input v-model="state.query.regionName" placeholder="地区名称" class="handle-input mr10"></el-input>
                  <el-button type="primary" icon="el-icon-search" @click="handleSearch">搜索</el-button>
                </template>         
                <el-button type="primary" icon="el-icon-plus" @click="handleClickAddData" v-if="checkPermission([`api${state.VIEW_VERSION}:System:SysRegion:Add`])">新增</el-button>
                <el-button
                  type="danger"
                  icon="el-icon-delete"
                  class="handle-del mr10"
                  @click="handleClickBatchDelete(this)"
                  v-if="checkPermission([`api${state.VIEW_VERSION}:System:SysRegion:BatchSoftDelete`])"
                >批量删除</el-button>
              </template>
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
                <el-table-column prop="regionId" label="地区Id" min-width="80" align="center"></el-table-column>
                <el-table-column prop="parentId" label="上级Id" min-width="80" align="center"></el-table-column>
                <el-table-column prop="fullId" label="完整Id" min-width="80" align="center"></el-table-column>
                <el-table-column prop="regionName" label="地区名称" min-width="80" align="center"></el-table-column>
                <el-table-column prop="shortName" label="省级简称" min-width="80" align="center"></el-table-column>
                <el-table-column prop="pinYin" label="名称拼音" min-width="120" align="center"></el-table-column>
                <el-table-column prop="longitude" label="经度" min-width="90" align="center"></el-table-column>
                <el-table-column prop="latitude" label="纬度" min-width="90" align="center"></el-table-column>
                <el-table-column prop="depth" label="地区级别" min-width="80" align="center"></el-table-column>
                <el-table-column prop="sequence" label="地区排序" min-width="80" align="center"></el-table-column>
                <el-table-column label="操作" min-width="220" align="center" fixed="right" v-if="checkPermission([`api${state.VIEW_VERSION}:System:SysRegion:Update`,`api${state.VIEW_VERSION}:System:SysRegion:Get`,`api${state.VIEW_VERSION}:System:SysRegion:SoftDelete`])">
                  <template #default="scope">
                    <el-button
                    type="text"
                    icon="el-icon-edit"
                    @click="handleEdit(scope.$index, scope.row)"
                    v-if="checkPermission([`api${state.VIEW_VERSION}:System:SysRegion:Update`])"
                    >编辑</el-button>
                    <el-button
                        type="text"
                        icon="el-icon-check"
                        @click="handleClickDetails(scope.row)"
                        v-if="checkPermission([`api${state.VIEW_VERSION}:System:SysRegion:Get`])"
                    >查看详情</el-button>
                    <el-button
                        type="text"
                        icon="el-icon-delete"
                        class="red"
                        @click="handleClickDelete(scope.row, cb_Delete_Func, this)"
                        v-if="checkPermission([`api${state.VIEW_VERSION}:System:SysRegion:SoftDelete`])"
                    >删除</el-button>
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

        <!-- 详情窗口 begin -->
        <el-dialog 
          title="地区详情" 
          v-model="state.detailsVisible"
          :close-on-click-modal="false"
          width="60%">
          <el-form
            ref="detailsForm"
            :model="state.detailsForm"
            label-width="80px"
            :inline="false"
            >
            <el-row>
              <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                  <el-form-item label="地区级别" prop="depth">
                    <el-select v-model="state.detailsForm.depth" placeholder="请选择" :disabled="true">
                      <el-option
                        v-for="item in state.depthOption"
                        :key="item.value"
                        :label="item.label"
                        :value="item.value">
                      </el-option>
                    </el-select>
                  </el-form-item>
              </el-col>    
              <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6" v-if="state.detailsForm.depth==2 || state.detailsForm.depth==3">
                  <el-form-item label="省" prop="province">
                    <el-select v-model="state.detailsForm.province" placeholder="请选择" :disabled="true">
                      <el-option
                        v-for="item in state.provinceOption"
                        :key="item.value"
                        :label="item.label"
                        :value="item.value">
                      </el-option>
                    </el-select>
                  </el-form-item>
              </el-col>
              <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6" v-if="state.detailsForm.depth==3" >
                  <el-form-item label="市" prop="city">
                    <el-select v-model="state.detailsForm.city" placeholder="请选择" :disabled="true">
                        <el-option
                        v-for="item in state.cityOption"
                        :key="item.value"
                        :label="item.label"
                        :value="item.value">
                        </el-option>
                    </el-select>
                  </el-form-item>
              </el-col>
              <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6" v-if="state.addForm.depth == 1">
                  <el-form-item label="省级简称" prop="shortName">
                    <el-input v-model="state.detailsForm.shortName" autocomplete="off" :disabled="true" />
                  </el-form-item>
              </el-col>
              <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                  <el-form-item label="地区ID" prop="regionId">
                    <el-input v-model="state.detailsForm.regionId" autocomplete="off" :disabled="true" />
                  </el-form-item>
              </el-col>
              <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                  <el-form-item label="地区名称" prop="regionName">
                    <el-input v-model="state.detailsForm.regionName" autocomplete="off" :disabled="true" />
                  </el-form-item>
              </el-col>
              <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                  <el-form-item label="名称拼音" prop="pinYin">
                    <el-input v-model="state.detailsForm.pinYin" autocomplete="off" :disabled="true" />
                  </el-form-item>
              </el-col>
              <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                  <el-form-item label="经度" prop="longitude">
                    <el-input maxlength="8"  v-model="state.detailsForm.longitude" autocomplete="off" :disabled="true" />
                  </el-form-item>
              </el-col>
              <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                  <el-form-item label="纬度" prop="latitude">
                    <el-input maxlength="8"  v-model="state.detailsForm.latitude" autocomplete="off" :disabled="true" />
                  </el-form-item>
              </el-col>
              <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                  <el-form-item label="地区排序" prop="sequence">
                    <el-input v-model="state.detailsForm.sequence" autocomplete="off" disabled />
                  </el-form-item>
              </el-col>
            </el-row>
          </el-form>
        </el-dialog>
        <!-- 详情窗口 end -->
        <!-- 添加/编辑窗口 begin -->
        <el-dialog 
          :title="state.dialogTitle"
          v-model="state.addDialogFormVisible"
          :close-on-click-modal="false"
          width="60%"
          @close="closeEditForm()">
          <el-form
            ref="addForm"
            :model="state.addForm"
            :rules="state.addFormRules"
            label-width="100px"
            :inline="false"
          >
            <el-row>
              <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                  <el-form-item label="地区级别" prop="region">
                      <el-select v-model="state.addForm.depth" placeholder="请选择" :disabled=!state.isEditDialogType>
                          <el-option
                          v-for="item in state.depthOption"
                          :key="item.value"
                          :label="item.label"
                          :value="item.value">
                          </el-option>
                      </el-select>
                  </el-form-item>
              </el-col>    
              <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6" v-if="state.addForm.depth==2 || state.addForm.depth==3">
                  <el-form-item label="省" prop="province">
                      <el-select v-model="state.addForm.province" placeholder="请选择" @change="changeProvince" :disabled=!state.isEditDialogType >
                          <el-option
                          v-for="item in state.provinceOption"
                          :key="item.value"
                          :label="item.label"
                          :value="item.value">
                          </el-option>
                      </el-select>
                  </el-form-item>
              </el-col>
              <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6" v-if="state.addForm.depth==3">
                  <el-form-item label="市" prop="city" v-if="state.addForm.depth==3">
                      <el-select v-model="state.addForm.city" placeholder="请选择" :disabled=!state.isEditDialogType >
                          <el-option
                          v-for="item in state.cityOption"
                          :key="item.value"
                          :label="item.label"
                          :value="item.value">
                          </el-option>
                      </el-select>
                  </el-form-item>
              </el-col>
              <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6" v-if="state.addForm.depth == 1">
                  <el-form-item label="省级简称" prop="shortName">
                    <el-input v-model="state.addForm.shortName" autocomplete="off"  />
                  </el-form-item>
              </el-col>
              <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                  <el-form-item label="地区ID" prop="regionId">
                    <el-input v-model="state.addForm.regionId" autocomplete="off"  :disabled=!state.isEditDialogType />
                  </el-form-item>
              </el-col>
              <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                  <el-form-item label="地区名称" prop="regionName">
                    <el-input v-model="state.addForm.regionName" autocomplete="off"  />
                  </el-form-item>
              </el-col>
              <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                  <el-form-item label="名称拼音" prop="pinYin">
                    <el-input v-model="state.addForm.pinYin" autocomplete="off"  />
                  </el-form-item>
              </el-col>
              <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6" v-show="!state.isEditDialogType">
                  <el-form-item label="经度" prop="longitude">
                    <el-input maxlength="8" oninput = "value=value.replace(/[^\d.]/g,'')" v-model="state.addForm.longitude" autocomplete="off"  disabled/>
                  </el-form-item>
              </el-col>
              <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6" v-show="!state.isEditDialogType">
                  <el-form-item label="纬度" prop="latitude">
                    <el-input maxlength="8" oninput = "value=value.replace(/[^\d.]/g,'')" v-model="state.addForm.latitude" autocomplete="off"  disabled/>
                  </el-form-item>
              </el-col>
              <el-col :xs="24" :sm="12" :md="8" :lg="8" :xl="6">
                  <el-form-item label="地区排序" prop="sequence">
                    <el-input v-model="state.addForm.sequence" autocomplete="off"  />
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
import {reactive,onMounted, ref} from "vue"
import { ElMessage,ElMessageBox } from 'element-plus'
import { getAreaList,getAreaInfo,updateAreaInfo,addAreaInfo,getSelectList,SoftDelete,BatchSoftDelete} from "@/serviceApi/system/areaManage"
import EupPagination from "../../../components/EupPagination.vue"
import EupCrumbs from "../../../components/eup-crumbs/index.vue"
import EnumConfig from "@/enum/EnumConfig"

var CURR_VIEW_VERSION = EnumConfig.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var VIEW_VERSION = CURR_VIEW_VERSION == 'V0' ? '' : `:S:${CURR_VIEW_VERSION}`;

export default {
    name: "AreaManage",
    components: {
        EupPagination: EupPagination,
        EupCrumbs: EupCrumbs
    },
    setup(){
      const addForm = ref(null)
        const state = reactive({
          VIEW_VERSION: VIEW_VERSION,
            query: {
                regionName: ""
            },
            pageIndex: 1,
            pageSize: 10,
            tableData: [],
            multipleSelection: [],
            pageTotal: 0,
            dynamicFilter:{},
            loading: false,
            detailsVisible: false,
            addDialogFormVisible: false,
            dialogTitle: "",
            addFormRules:{
                province:[{ 
                  required: true, message: '请选择省份', trigger: 'blur' }
                ],
                city:[{ 
                  required: true, message: '请选择城市', trigger: 'blur' }
                ],
                shortName:[{ 
                  required: true, message: '省级简称不能为空', trigger: 'blur' }
                ],
                regionId:[{ 
                  required: true, message: '地区ID不能为空', trigger: 'blur' }
                ],
                regionName:[{ 
                  required: true, message: '地区名称', trigger: 'blur' }
                ],
                pinYin:[{ 
                  required: true, message: '名称拼音不能为空', trigger: 'blur' }
                ],
                longitude:[
                //     { 
                //   validator: validateLongitude, trigger: 'blur' },
                //   { 
                //   validator: validateLongitude, trigger: 'change' },
                  { 
                  required: true, message: '经度不能为空', trigger: 'blur' }
                ],
                latitude:[
                //     { 
                //   validator: validateLatitude, trigger: 'blur' },
                //   { 
                //   validator: validateLatitude, trigger: 'change' },
                  { 
                  required: true, message: '纬度不能为空', trigger: 'blur' }
                ],
                sequence:[{ 
                  required: true, message: '地区排序不能为空', trigger: 'blur' }
                ],
            },
            //新增/编辑数据
            addForm:{
                regionId: 0,
                parentId: 0,
                fullId: "",
                regionName: "",
                shortName: "",
                pinYin: "",
                longitude: "",
                latitude: "",
                depth: 1,
                sequence: 1,
                version: 0,

                province: "",
                city: ""
            },
            //详情数据
            detailsForm:{
              regionId: 0,
              regionName: "",
              shortName: "",
              pinYin: "",
              longitude: "",
              latitude: "",
              depth: 1,
              sequence: 1,
              province: "",
              city: ""
            },
            //地区级别
            depthOption:[
                {
                    value:1,label:"省"
                },
                {
                    value:2,label:"市"
                },
                {
                    value:3,label:"区"
                }
            ],
            provinceOption: [],//省份
            cityOption: [],//城市
            isEditDialogType: false,//默认是编辑对话框，true是新增对话框
        });
        onMounted(()=>{
            getData();
        })
        //获取表单信息
        const getData=()=>{
            var params = {
                "currentPage": state.pageIndex,
                "pageSize": state.pageSize,
                "filter.RegionName": state.query.regionName,
                "dynamicFilter": state.dynamicFilter
            }
            state.loading = true;
            getAreaList(params).then(function(res){
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
        //改变省份
        const changeProvince =(val)=>{
          state.addForm.city = "";
          state.cityOption = [];//清空缓存
          getSelectList_Area(val,(data)=>{
            data.forEach(function(data, index){
              var subJson = {};
              subJson.value = data.regionId;
              subJson.label = data.regionName;
              state.cityOption.push(subJson);//添加一级下拉菜单
            });
          });
        }
        //获取地区
        const getSelectList_Area = (regionId, callback)=>{
          getSelectList(regionId).then((res)=>{
            if (res.code == 1){
              if (callback){
                callback(res.data);
              }
            }else {
              ElMessage.error(res.msg);
            }
          }).catch((err)=>{
            ElMessage.error(err.msg)
          });
        }
        //新增
        const handleClickAddData = ()=>{
            state.addDialogFormVisible =true;
            state.isEditDialogType = true;
            state.dialogTitle = "新增地区";
            state.addForm = {//初始化
                regionId: 0,
                parentId: 0,
                fullId: "",
                regionName: "",
                shortName: "",
                pinYin: "",
                longitude: "",
                latitude: "",
                depth: 1,
                sequence: 1,
                version: 0,

                province: "",
                city: ""
            };
            if (state.provinceOption.length == 0){
                getSelectList_Area(0, (data)=>{
                    data.forEach(function(data, index){
                    var subJson = {};
                    subJson.value = data.regionId;
                    subJson.label = data.regionName;
                    state.provinceOption.push(subJson);//添加一级下拉菜单
                    });
                });
            }
        }
        //新增或者更新数据
        const addDialogFormSave = (t)=>{
            if (state.addForm.depth == 1){
                if (state.addForm.shortName == "" || state.addForm.shortName == undefined || state.addForm.shortName == null){
                    ElMessage.error("省级简称不能为空");
                    return;
                }
            }
            if (state.addForm.depth == 2 || state.addForm.depth == 3){
                if (state.addForm.province == "" || state.addForm.province == undefined || state.addForm.province == null){
                    ElMessage.error("省份不能为空");
                    return;
                }
                if (state.addForm.depth == 3){
                    if (state.addForm.city == "" || state.addForm.city == undefined || state.addForm.city == null){
                        ElMessage.error("城市不能为空");
                        return;
                    }
                }
            }
            if (state.addForm.regionName == "" || state.addForm.regionName == undefined || state.addForm.regionName == null){
                ElMessage.error("地区名称不能为空");
                return;
            }
            //编辑修改状态下需要校验经纬度，新增添加状态下不用校验经纬度（不需要填写经纬度）
            if (!state.isEditDialogType && (state.addForm.longitude == "" || state.addForm.longitude == undefined || state.addForm.longitude == null)){
                ElMessage.error("经度不能为空");
                return;
            }
            if (!state.isEditDialogType && (state.addForm.latitude == "" || state.addForm.latitude == undefined || state.addForm.latitude == null)){
                ElMessage.error("纬度不能为空");
                return;
            }
            if (state.addForm.sequence == "" || state.addForm.sequence == undefined || state.addForm.sequence == null){
                ElMessage.error("地区排序不能为空");
                return;
            }
            if (state.addForm.depth == 1){//省
                state.addForm.parentId = 0;
                state.addForm.regionId = parseInt(state.addForm.regionId);
                state.addForm.fullId = "0"
            } else if (state.addForm.depth == 2){//市
                state.addForm.parentId = parseInt(state.addForm.province);
                state.addForm.regionId = parseInt(state.addForm.regionId);
                state.addForm.fullId = state.addForm.province;
            } else if (state.addForm.depth == 3){//区
                state.addForm.parentId = parseInt(state.addForm.city);
                state.addForm.regionId = parseInt(state.addForm.regionId);
                state.addForm.fullId = state.addForm.province + ',' + state.addForm.city;
            }
            var params = {};
            var operType = 0;//操作类型
            if(state.dialogTitle == "新增地区"){
              //参数封装
              params = {
                "regionId": state.addForm.regionId,
                "parentId": state.addForm.parentId,
                "fullId": state.addForm.fullId,
                "regionName": state.addForm.regionName,
                "shortName": state.addForm.shortName,
                "pinYin": state.addForm.pinYin,
                "longitude": state.addForm.longitude,
                "latitude": state.addForm.latitude,
                "depth": state.addForm.depth,
                "sequence": state.addForm.sequence
              }
              operType = 1;
            } else {
              //参数封装
              params = {
                "regionId": state.addForm.regionId,
                "parentId": state.addForm.parentId,
                "fullId": state.addForm.fullId,
                "regionName": state.addForm.regionName,
                "shortName": state.addForm.shortName,
                "pinYin": state.addForm.pinYin,
                "longitude": state.addForm.longitude,
                "latitude": state.addForm.latitude,
                "depth": state.addForm.depth,
                "sequence": state.addForm.sequence,
                "version": 0
              }
              operType = 2;
            }
            
            if (1 == operType) {//新增
              addAreaInfo(params).then(res =>{
              if (res.code == 1){
                  ElMessage.success({
                    type: 'success',
                    message: '提交成功!'
                  });
                  getData();
                  state.addDialogFormVisible = false;
              } else {
                  ElMessage.warning({
                    type: 'warning',
                    message: res.msg
                  });
              }
              }).catch(err=>{
                  ElMessage.error({
                      type: 'error',
                      message: '提交失败！'
                  });
              }); 
            } else if (2 == operType){//编辑
              updateAreaInfo(params).then(res =>{
                if (res.code == 1){
                    ElMessage.success({
                      type: 'success',
                      message: '提交成功!'
                    });
                    getData();
                    state.addDialogFormVisible = false;
                } else {
                    ElMessage.warning({
                      type: 'warning',
                      message: res.msg
                    });
                }
              }).catch(err=>{
                  ElMessage.error({
                    type: 'error',
                    message: '提交失败！'
                  });
              }); 
            }
        }
        //编辑
        const handleEdit = (index, row)=>{
            state.dialogTitle = "编辑地区";
            state.addDialogFormVisible = true;
            state.isEditDialogType = false;

            state.addForm.regionId=row.regionId;
            state.addForm.depth=row.depth;
            state.addForm.fullId=row.fullId;
            state.addForm.latitude=row.latitude;
            state.addForm.longitude=row.longitude;
            state.addForm.parentId=row.parentId;
            state.addForm.pinYin=row.pinYin;
            state.addForm.regionName=row.regionName;
            state.addForm.sequence=row.sequence;
            state.addForm.shortName=row.shortName;
            if (row.depth == 1){//省
                state.addForm.city = "";
                state.addForm.province=row.regionId + "";
            } else if (row.depth == 2){//市
                if (state.provinceOption.length == 0){
                    state.provinceOption = [];
                    getSelectList_Area(0, (data)=>{
                        data.forEach(function(data, index){
                            var subJson = {};
                            subJson.value = data.regionId;
                            subJson.label = data.regionName;
                            state.provinceOption.push(subJson);//添加一级下拉菜单
                        });
                        state.addForm.province=row.parentId;
                    });
                }
            } else if (row.depth == 3){
              state.provinceOption = []
                if (state.provinceOption.length == 0){
                    state.provinceOption = [];
                    getSelectList_Area(0, (data)=>{                    
                      data.forEach(function(data, index){
                          var subJson = {};
                          subJson.value = data.regionId;
                          subJson.label = data.regionName;
                          state.provinceOption.push(subJson);//添加一级下拉菜单
                      });
                      var arr = row.fullId.split(",");
                      if (arr.length == 2){
                          state.addForm.province= parseInt(arr[0]);
                      }

                      state.cityOption = [];
                      getSelectList_Area(state.addForm.province,(data)=>{
                          data.forEach(function(data, index){
                              var subJson = {};
                              subJson.value = data.regionId;
                              subJson.label = data.regionName;
                              state.cityOption.push(subJson);//添加二级下拉菜单
                          });
                          state.addForm.city =parseInt(arr[1]);
                      });
                    });                 
                }
            }
        }
        //删除
        const handleClickDelete =(row, callback, t)=>{
          var id = row.regionId;
          if (callback){
            callback(id, t);
          }
        }
        //批量删除
        function handleClickBatchDelete(t){
          if(state.multipleSelection.length == 0){//未选中
            ElMessage.error("请选择要删除的数据！");
          } else {
            var arrIds = state.multipleSelection.map(s =>{
              return s.regionId;
            });
            ElMessageBox.confirm('此操作将删除选中的记录, 是否继续?', '提示', {
              confirmButtonText: '确定',
              cancelButtonText: '取消',
              type: 'warning'
            }).then(() => {
              BatchSoftDelete(arrIds).then(res=>{
                if (res.code == 1){
                  ElMessage.success("操作成功！");
                  getData();
                } else {
                  ElMessage.error("操作失败！");
                }
              }).catch(err=>{
                ElMessage.error(err.msg);
              });
            }).catch(err=>{
              ElMessage.info("取消批量删除！");
            });
          }
        }
        //关闭对话框
        const closeEditForm = ()=>{
            state.addDialogFormVisible =false;
            addForm.value.resetFields();
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

        //详情
        const handleClickDetails = (row) =>{
          state.detailsVisible  = true;
          if (state.provinceOption.length == 0){
            getSelectList_Area(0, (data)=>{
                data.forEach(function(data, index){
                var subJson = {};
                subJson.value = data.regionId;
                subJson.label = data.regionName;
                state.provinceOption.push(subJson);//添加一级下拉菜单
                });
            });
          }
          state.detailsForm.depth = row.depth;
          // state.detailsForm.province = row.parentId;
          // state.detailsForm.city = row.regionId;
          state.detailsForm.shortName = row.shortName;
          state.detailsForm.regionId = row.regionId;
          state.detailsForm.regionName = row.regionName;
          state.detailsForm.longitude = row.longitude;
          state.detailsForm.latitude = row.latitude;
          state.detailsForm.sequence = row.sequence;
          state.detailsForm.pinYin = row.pinYin;
          if (row.depth == 1){//省
                state.detailsForm.city = "";
                state.detailsForm.province=row.regionId + "";
            } else if (row.depth == 2){//市
                if (state.provinceOption.length == 0){
                    state.provinceOption = [];
                    getSelectList_Area(0, (data)=>{
                        data.forEach(function(data, index){
                            var subJson = {};
                            subJson.value = data.regionId;
                            subJson.label = data.regionName;
                            state.provinceOption.push(subJson);//添加一级下拉菜单
                        });
                        state.detailsForm.province=row.parentId;
                    });
                }
            } else if (row.depth == 3){
              state.provinceOption = []
                if (state.provinceOption.length == 0){
                    state.provinceOption = [];
                    getSelectList_Area(0, (data)=>{                    
                      data.forEach(function(data, index){
                          var subJson = {};
                          subJson.value = data.regionId;
                          subJson.label = data.regionName;
                          state.provinceOption.push(subJson);//添加一级下拉菜单
                      });
                      var arr = row.fullId.split(",");
                      if (arr.length == 2){
                          state.detailsForm.province= parseInt(arr[0]);
                      }

                      state.cityOption = [];
                      getSelectList_Area(state.detailsForm.province,(data)=>{
                          data.forEach(function(data, index){
                              var subJson = {};
                              subJson.value = data.regionId;
                              subJson.label = data.regionName;
                              state.cityOption.push(subJson);//添加二级下拉菜单
                          });
                          state.detailsForm.city =parseInt(arr[1]);
                      });
                    });                 
                }
            }
        };
        //经度校验
        const validateLongitude = (rule, value, callback)=>{
            //经度,整数部分为0-180小数部分为0到15位
            var longreg = /^(\-|\+)?(((\d|[1-9]\d|1[0-7]\d|0{1,3})\.\d{0,15})|(\d|[1-9]\d|1[0-7]\d|0{1,3})|180\.0{0,15}|180)$/
            if (!longreg.test(value)) {
                callback(new Error('经度整数部分为0-180,小数部分为0到15位!'))
            }
            callback()
        }
        //纬度校验
        const validateLatitude = (rule, value, callback) => {
            //纬度,整数部分为0-90小数部分为0到15位
            var latreg = /^(\-|\+)?([0-8]?\d{1}\.\d{0,15}|90\.0{0,15}|[0-8]?\d{1}|90)$/
            if (!latreg.test(value)) {
                callback(new Error('纬度整数部分为0-90,小数部分为0到15位!'))
            }
            callback()
        }
        //删除回调
        const cb_Delete_Func = function(params,t){
          ElMessageBox.confirm('此操作将永久删除该数据, 是否继续?', '提示', {
              confirmButtonText: '确定',
              cancelButtonText: '取消',
              type: 'warning'
            }).then(() => {
              SoftDelete(params).then(res =>{
                if (res.code == 1){
                  ElMessage.success({
                    type: 'success',
                    message: '删除成功!'
                  });
                  //刷新表单
                  getData();
                } else {
                  ElMessage.warning({
                    type: 'warning',
                    message: res.msg
                  });
                }
              }).catch(err=>{
                ElMessage.error({
                  type: 'error',
                  message: '删除失败！'
                });
              });    
            }).catch(() => {
              ElMessage.info({
                type: 'info',
                message: '已取消删除'
              });
          });
        }
        return {
            state,
            handleSelectionChange,
            handleSearch,
            handleClickDetails,
            handleClickAddData,
            closeEditForm,
            changeProvince,
            addDialogFormSave,
            validateLongitude,
            validateLatitude,
            handleEdit,
            cb_Delete_Func,
            handleClickDelete,
            handleClickBatchDelete,
            addForm,
            getData,
            resPageData
        }
    }
};
</script>
<style scoped>
</style>
