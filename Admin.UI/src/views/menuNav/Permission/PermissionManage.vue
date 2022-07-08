<!--
 * @Description: 
 * @Author: weig
 * @Date: 2021-08-17 09:20:09
 * @LastEditors: weig
 * @LastEditTime: 2021-12-17 09:46:19
-->
<template>
  <div>
    <!-- 面包屑 begin -->
    <eup-crumbs icon="el-icon-setting" firstCrumbs="权限管理" secondCrumbs="权限列表" />
    <!-- 面包屑 end -->
    <!-- 内容区域 begin -->
    <!-- 表单内容 -->
    <div class="container">
      <!-- 查询 -->
      <div class="handle-box">
          <el-form :model="state.filters" :inline="true" v-if="checkPermission([`api${state.VIEW_VERSION}:auth:permission:getlist`,`api${state.VIEW_VERSION}:auth:permission:addgroup`])">
            <template v-if="checkPermission([`api${state.VIEW_VERSION}:auth:permission:getlist`])">
              <el-form-item label="创建日期">
                <el-date-picker
                    v-model="state.filters.createTime"
                    type="daterange"
                    format="YYYY-MM-DD"
                    unlink-panels
                    range-separator="至"
                    start-placeholder="开始日期"
                    end-placeholder="结束日期"
                    
                    :disabledDate="disabledDate"
                    :shortcuts="state.shortcuts"
                    popper-class="datePicker"
                />
              </el-form-item>
              <el-form-item>
                  <el-input
                      v-model="state.filters.label"
                      placeholder="权限名称"
                      @keyup.enter="getPermissions"
                      @change="handleChangeQuery"
                  />
              </el-form-item>
              <el-form-item label="版本号">
                <el-select v-model="state.filters.apiVersion" @change="apiVerChangeHandle">
                  <el-option
                    v-for="item in state.apiVersion"
                    :key="item.value"
                    :label="item.label"
                    :value="item.value"
                  >
                  </el-option>
                </el-select>
              </el-form-item>
              <el-form-item>
                  <el-button type="primary" icon="el-icon-search" @click="handleSearch">查询</el-button>
              </el-form-item>
            </template>
            <el-form-item v-if="checkPermission([`api${state.VIEW_VERSION}:auth:permission:addgroup`])">
                <el-dropdown>
                    <el-button type="primary">新增<i class="el-icon-arrow-down el-icon--right"/></el-button>
                    <template #dropdown>
                        <el-dropdown-menu style="margin-top: 2px;">
                            <el-dropdown-item  icon="el-icon-folder" @click="onOpenAddGroup">新增分组</el-dropdown-item>
                            <el-dropdown-item  icon="el-icon-tickets" @click="onOpenAddMenu">新增菜单</el-dropdown-item>
                            <el-dropdown-item  icon="el-icon-magic-stick" @click="onOpenAddDot">新增权限点</el-dropdown-item>
                        </el-dropdown-menu>
                    </template>
                </el-dropdown>
            </el-form-item>
          </el-form>
          <!-- 列表 -->
          <el-table
                ref="multipleTable"
                v-loading="state.loading"
                highlight-current-row
                :data="state.permissionTree"
                row-key="permissionId"
                :default-expand-all="false"
                :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
                style="width: 100%;"
                @select-all="onSelectAll"
                @select="onSelect">

            <el-table-column label="权限" min-width="330">
                <template #default="{row}">
                <i :class="row.icon" />
                {{ row.label }}
                </template>
            </el-table-column>
            <el-table-column label="类型" min-width="100">
                <template #default="{ row }">
                {{ row.type === 1 ? '分组' : row.type === 2 ? '菜单' : row.type === 3 ? '权限点' : '' }}
                </template>
            </el-table-column>
            <el-table-column label="地址" min-width="330">
                <template #default="{ row }">
                {{ row.type === 2 ? row.path : row.type === 3 ? row.apiPaths : '' }}
                </template>
            </el-table-column>
            <el-table-column prop="isActive" label="状态" min-width="90" align="center">
                <template #default="{row}">
                <el-tag :type="row.isActive == state.isActive.yes ? 'success' : 'danger'" disable-transitions>
                    {{ row.isActive == state.isActive.yes ? '正常' : '禁用' }}
                </el-tag>
                </template>
            </el-table-column>
            <el-table-column
                label="操作"
                fixed="right"
                min-width="220"
                v-if="checkPermission([`api${state.VIEW_VERSION}:auth:permission:updategroup`,`api${state.VIEW_VERSION}:auth:permission:softdelete`])"
            >
                <template #default="{ $index, row }">
                    <el-button
                        type="primary"
                        icon="el-icon-edit"
                        @click="handleEdit($index, row)"
                        v-if="checkPermission([`api${state.VIEW_VERSION}:auth:permission:updategroup`])"
                    >编辑</el-button>
                    <el-button
                        type="danger"
                        icon="el-icon-delete"
                        @click="handleClickDelete($index, row)"
                        class="ml5"
                        v-if="checkPermission([`api${state.VIEW_VERSION}:auth:permission:softdelete`])"
                    >删除</el-button>
                </template>
            </el-table-column> 
          </el-table>
          <!-- 分页 begin-->
          <EupPagination
              :current-page="state.pageIndex"
              :pagesizes="[10,20,50,100]"
              :pagesize="state.pageSize"
              :total="state.pageTotal"
              @getPageData="pagination(state.pageIndex, state.pageSize, state.FrontPageData)"
              @resPageData="resPageData">
          </EupPagination>
          <!-- 分页 end-->
      </div>
      <!-- 内容区域 begin -->
    </div>
    <!-- 分组 begin -->
    <el-dialog 
      :title="(state.permissionGroup.dialogTitle == 0 ? '编辑' : '新增' ) + '分组'"
      v-model="state.permissionGroup.visible"
      width="60%"
      @close="onCloseGroup"
      :close-on-click-modal="false"
      :fullscreen="state.dialogFull"
      >
      <el-form
        ref="permissionGroupForm"
        :model="state.permissionGroup.form"
        :rules="state.formRules"
        label-width="100px"
        :inline="false"
      >
        <el-form-item prop="parentIds" label="上级分组">
          <el-cascader
            :key="state.permissionGroup.key"
            v-model="state.permissionGroup.form.parentIds"
            placeholder="请选择，支持搜索功能"
            :options="state.groupTree"
            :props="{checkStrictly: true, value: 'permissionId'}"
            filterable
            style="width:100%;"
          />
        </el-form-item>
        <el-form-item label="名称" prop="label">
          <el-input v-model="state.permissionGroup.form.label" auto-complete="off" />
        </el-form-item>
        <el-form-item label="图标类名" prop="icon">
          <el-input v-model="state.permissionGroup.form.icon" auto-complete="off" />
        </el-form-item>
        <el-form-item label="版本号" prop="apiVersion">
        <!--   <el-select v-model="state.permissionGroup.form.apiVersion" placeholder="版本号" :disabled="state.apiVersionDisable">
            <el-option
              v-for="item in state.apiVersion"
              :key="item.value"
              :label="item.label"
              :value="item.value"
              :disabled="item.disabled"
            >
            </el-option>
          </el-select> -->
            <el-input v-model="state.permissionGroup.form.apiVersion" auto-complete="off"  :disabled="true"/>
        </el-form-item>
        <el-form-item prop="opened" label="默认展开" width>
          <el-switch v-model="state.permissionGroup.form.opened" />
        </el-form-item>
        <el-form-item label="隐藏" prop="hidden">
          <el-switch v-model="state.permissionGroup.form.hidden" />
        </el-form-item>
      </el-form>
      <template #title>
        <div class="avue-crud__dialog__header">
            <span class="el-dialog__title">
            <span style="display:inline-block;width:3px;height:20px;margin-right:5px; float: left;margin-top:2px"></span>
              {{(state.permissionGroup.dialogTitle == 0 ? '编辑' : '新增' ) + '分组'}}
            </span>
          <div class="avue-crud__dialog__menu" @click="state.dialogFull? state.dialogFull=false: state.dialogFull=true">
            <i class="el-icon-full-screen" title="全屏" v-if="!state.dialogFull"></i>
            <i class="el-icon-copy-document" title="缩小" v-else></i>
          </div>
        </div>
      </template>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="onCloseGroup()">取 消</el-button>
          <el-button type="primary" @click="onSubmitGroup()">确 定</el-button>
        </span>
      </template>
    </el-dialog>
    <!-- 分组 end -->

    <!-- 菜单 begin -->
    <el-dialog 
      :title="(state.permissionMenu.dialogTitle == 0 ? '编辑' : '新增' ) + '菜单'"
      v-model="state.permissionMenu.visible"
      width="60%"
      @close="onCloseMenu"
      :close-on-click-modal="false"
      :fullscreen="state.dialogFull"
      >
      <el-form
        ref="menuForm"
        :model="state.permissionMenu.form"
        :rules="state.formRules"
        label-width="100px"
        :inline="false"
      >
        <el-form-item prop="parentIds" label="上级分组" width>
          <el-cascader
            :key="state.permissionMenu.key"
            v-model="state.permissionMenu.form.parentIds"
            placeholder="请选择，支持搜索功能"
            style="width: 100%;"
            :options="state.groupTree"
            :props="{ checkStrictly: true, value: 'permissionId'}"
            filterable
          />
        </el-form-item>
        <el-form-item prop="viewId" label="视图组件" width>
          <el-cascader
            :key="state.permissionMenu.key"
            v-model="state.permissionMenu.form.viewId"
            placeholder="请选择，支持搜索功能"
            style="width: 100%;"
            :options="state.viewTree"
            :props="{ value: 'viewId',label:'path', emitPath:false }"
            filterable
            clearable
            :show-all-levels="false"
            @change="onChangeView"
          >
            <template #default="{ data }">
              <span>{{ data.path }}</span>
              <span style="float:right;margin-left:15px;">{{ data.label }}</span>
            </template>
          </el-cascader>
        </el-form-item>
        <el-form-item label="访问地址" prop="path">
          <el-input v-model="state.permissionMenu.form.path" auto-complete="off" />
        </el-form-item>
        <el-form-item label="名称" prop="label">
          <el-input v-model="state.permissionMenu.form.label" auto-complete="off" />
        </el-form-item>
        <el-form-item label="版本号" prop="apiVersion">
        <!--   <el-select v-model="state.permissionMenu.form.apiVersion" placeholder="版本号" :disabled="state.apiVersionDisable">
            <el-option
              v-for="item in state.apiVersion"
              :key="item.value"
              :label="item.label"
              :value="item.value"
              :disabled="item.disabled"
            >
            </el-option>
          </el-select> -->
          <el-input v-model="state.permissionMenu.form.apiVersion" auto-complete="off"  :disabled="true"/>
        </el-form-item>
        <el-form-item label="描述" prop="description">
          <el-input v-model="state.permissionMenu.form.description" auto-complete="off" />
        </el-form-item>
        <el-form-item label="图标类名" prop="icon">
          <el-input v-model="state.permissionMenu.form.icon" auto-complete="off" />
        </el-form-item>
        <el-form-item label="标签可关闭" prop="closable" width>
          <el-switch v-model="state.permissionMenu.form.closable" />
        </el-form-item>
        <el-form-item label="隐藏" prop="hidden">
          <el-switch v-model="state.permissionMenu.form.hidden" />
        </el-form-item>
        <el-form-item label="打开新窗口" prop="newWindow">
          <el-switch v-model="state.permissionMenu.form.newWindow" />
        </el-form-item>
        <el-form-item label="链接外显" prop="external">
          <el-switch v-model="state.permissionMenu.form.external" />
        </el-form-item>
      </el-form>
      <template #title>
        <div class="avue-crud__dialog__header">
            <span class="el-dialog__title">
            <span style="display:inline-block;width:3px;height:20px;margin-right:5px; float: left;margin-top:2px"></span>
              {{(state.permissionMenu.dialogTitle == 0 ? '编辑' : '新增' ) + '菜单'}}
            </span>
          <div class="avue-crud__dialog__menu" @click="state.dialogFull? state.dialogFull=false: state.dialogFull=true">
            <i class="el-icon-full-screen" title="全屏" v-if="!state.dialogFull"></i>
            <i class="el-icon-copy-document" title="缩小" v-else></i>
          </div>
        </div>
      </template>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="onCloseMenu()">取 消</el-button>
          <el-button type="primary" @click="onSubmitMenu()">确 定</el-button>
        </span>
      </template>
    </el-dialog>
    <!-- 菜单 end -->

    <!-- 权限点 begin -->
    <el-dialog 
      :title="(state.permissionDot.dialogTitle == 0 ? '编辑' : '新增' ) + '权限点'"
      v-model="state.permissionDot.visible"
      width="60%"
      @close="onCloseDot"
      :close-on-click-modal="false"
      :fullscreen="state.dialogFull"
      >
      <el-form
        ref="dotForm"
        :model="state.permissionDot.form"
        :rules="state.formRules"
        label-width="100px"
        :inline="false"
      >
        <el-form-item prop="parentIds" label="上级菜单" width>
          <el-cascader
            :key="state.permissionDot.key"
            v-model="state.permissionDot.form.parentIds"
            placeholder="请选择，支持搜索功能"
            style="width: 100%;"
            :options="state.menuTree"
            :props="{ value: 'permissionId' }"
            filterable
          />
        </el-form-item>
        <el-form-item prop="apiIds" label="API接口" width>
          <el-cascader
            :key="state.permissionDot.key"
            v-model="state.permissionDot.form.apiIds"
            placeholder="请选择，支持搜索功能"
            style="width: 100%;"
            :options="state.apiTree"
            :props="{ value: 'apiId', label:'path', emitPath:false, multiple: true }"
            filterable
            :show-all-levels="false"
            @change="onChangeApis"
          >
            <template #default="{ data }">
              <span>{{ data.path }}</span>
              <span style="float:right;margin-left:15px;">{{ data.label }}</span>
            </template>
          </el-cascader>
        </el-form-item>
        <el-form-item label="名称" prop="label">
          <el-input v-model="state.permissionDot.form.label" auto-complete="off" />
        </el-form-item>
        <el-form-item label="编码" prop="code">
          <el-input v-model="state.permissionDot.form.code" auto-complete="off" />
        </el-form-item>
        <el-form-item label="版本号" prop="apiVersion">
       <!--    <el-select v-model="state.permissionDot.form.apiVersion" placeholder="版本号" :disabled="state.apiVersionDisable">
            <el-option
              v-for="item in state.apiVersion"
              :key="item.value"
              :label="item.label"
              :value="item.value"
              :disabled="item.disabled"
            >
            </el-option>
          </el-select> -->
          <el-input v-model="state.permissionDot.form.apiVersion" auto-complete="off"  :disabled="true"/>
        </el-form-item>
        <el-form-item label="描述" prop="description">
          <el-input v-model="state.permissionDot.form.description" auto-complete="off" />
        </el-form-item>
      </el-form>
      <template #title>
        <div class="avue-crud__dialog__header">
            <span class="el-dialog__title">
            <span style="display:inline-block;width:3px;height:20px;margin-right:5px; float: left;margin-top:2px"></span>
              {{(state.permissionDot.dialogTitle == 0 ? '编辑' : '新增' ) + '权限点'}}
            </span>
          <div class="avue-crud__dialog__menu" @click="state.dialogFull? state.dialogFull=false: state.dialogFull=true">
            <i class="el-icon-full-screen" title="全屏" v-if="!state.dialogFull"></i>
            <i class="el-icon-copy-document" title="缩小" v-else></i>
          </div>
        </div>
      </template>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="onCloseDot">取 消</el-button>
          <el-button type="primary" @click="onSubmitDot()">确 定</el-button>
        </span>
      </template>
    </el-dialog>
    <!-- 权限点 end -->
  </div>
</template>

<script>
import { reactive, toRefs, onBeforeMount, onMounted, ref } from 'vue'
import {getPermissionList,softDeletePermission,addGroup,addMenu,addDot,updateGroup,updateMenu,updateDot,getGroup,getMenu,getApi,getDot} from '@/serviceApi/permission/permission'
import { ElMessage,ElMessageBox, ElLoading } from 'element-plus'
import {elConfirmDialog,IsNullOrEmpty} from "@/common/js/comm"
import { formatTime, treeToList, listToTree, getTreeParents } from '@/utils/tool'
import { getAllViewInfo } from '@/serviceApi/permission/view'
import { getAllApiInfo } from '@/serviceApi/permission/api'
import {useStore} from 'vuex'
import EupPagination from "@/components/EupPagination.vue"
import EupCrumbs from "../../../components/eup-crumbs/index.vue"
import Enum from "@/enum/EnumConfig"

var CURR_VIEW_VERSION = Enum.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var VIEW_VERSION = CURR_VIEW_VERSION == 'V0' ? '' : `:S:${CURR_VIEW_VERSION}`;
export default {
  name: 'PermissionManage',
  components:{
    EupPagination: EupPagination,
    EupCrumbs: EupCrumbs
  },
  setup(props, context) {
    const multipleTable = ref(null);
    const permissionGroupForm = ref(null);
    const menuForm = ref(null);
    const apiForm = ref(null);
    const dotForm = ref(null);
    //今天日期
    const todayDate = [formatTime(new Date(), 'YYYY-MM-DD'), formatTime(new Date(), 'YYYY-MM-DD')];
    //昨天日期
    var end = new Date();
    var start = new Date();
    start.setTime(start.getTime() - 3600 * 1000 * 24);
    const yesterdayDate = [formatTime(start, 'YYYY-MM-DD'), formatTime(end, 'YYYY-MM-DD')];
    //近一周
    end = new Date();
    start = new Date();
    start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
    const thisWeekDate = [formatTime(start, 'YYYY-MM-DD'), formatTime(end, 'YYYY-MM-DD')];
    const state = reactive({
        filters: {
            createTime: "", 
            label: "",
            apiVersion: ""
        },
      VIEW_VERSION: VIEW_VERSION,
        currViewVersion: 'Open_V1',
        pageIndex: 1,
        pageSize: 10,
        pageTotal: 0,
        tableData: [],
        delList: [],
        dynamicFilter:{},
        formRules: {
            parentId: [{ required: true, message: '请选择上级', trigger: 'change' }],
            parentIds: [{ required: true, message: '请选择上级', trigger: 'change' }],
            apiId: [{ required: true, message: '请选择API接口', trigger: 'change' }],
            label: [{ required: true, message: '请输入名称', trigger: ['blur'] }],
            code: [{ required: true, message: '请输入编码', trigger: ['blur'] }],
            path: [{ required: true, message: '请输入菜单地址', trigger: ['blur'] }],
            apiVersion:[{required: true, message: '请选择接口版本号'}]
        },
        idx: -1,
        loading: false,
        store: {}, //vuex全局状态管理对象
        dialogType: 0, //对话框类型, 0:编辑框  1:新增框
        dialogFull: false, //是否为全屏 Dialog
        apiVersion: [],
        apiVersionDisable: false,

        groupTree: [],
        menuTree: [],
        apiTree: [],
        viewTree: [],

        permissionGroup: {
            addForm: {
                type: 1,
                parentId: 0,
                parentIds: [0],
                label: '',
                description: '',
                icon: '',
                hidden: false,
                opened: false,
                isActive: 1,
                createUserId: "",
                apiVersion: ""
            },
            form: {},
            visible: false,
            loading: false,
            key: 1,
            dialogTitle: 0 //0：编辑  1:新增
        },
        permissionMenu: {
            addForm: {
                type: 2,
                parentId: 0,
                parentIds: [0],
                viewId: null,
                viewIds: [],
                label: '',
                path: '',
                description: '',
                icon: '',
                hidden: false,
                closable: true,
                isActive: 1,
                newWindow: false,
                external: false,
                createUserId: "",
                apiVersion: ""
            },
            form: {},
            visible: false,
            loading: false,
            key: 1,
            dialogTitle: 0 //0：编辑  1:新增
        },
        permissionDot: {
            addForm: {
                permissionId: 0,
                type: 3,
                parentId: null,
                parentIds: [],
                label: '',
                code: '',
                description: '',
                // icon: '',
                createUserId: "",
                apiVersion: ""
            },
            form: {},
            visible: false,
            loading: false,
            key: 1,
            dialogTitle: 0 //0：编辑  1:新增
        },
        isActive:{
          yes: 1,
          no: 2
        },
        shortcuts: [
            {
                text: '今天',
                value: todayDate
                // value: () => {
                //     const end = new Date();
                //     const start = new Date();
                //     return [start, end];
                // }
            },
            {
                text: '昨天',
                value: yesterdayDate
                // value: () => {
                //     const end = new Date();
                //     const start = new Date();
                //     start.setTime(start.getTime() - 3600 * 1000 * 24);
                //     return [start, end];
                // }
            },
            {
                text: '近一周',
                value: thisWeekDate
                // value: () => {
                //     const end = new Date();
                //     const start = new Date();
                //     start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
                //     return [start, end];
                // }
            }
        ],
        
        expandRowKeys: [],
        permissionTree: [],
        sels: [], // 列表选中列

        FrontPageData: [],//所有的权限数据
    });
    onBeforeMount(() => {
    });
    onMounted(() => {
      state.apiVersion = [];
      let apiVersionEnum = Enum.EnumConfig.apiVersion_str;
      for(var key in apiVersionEnum){
        //初始化版本号下拉
        state.apiVersion.push({label: key, value: apiVersionEnum[key], disabled: false});//(apiVersionEnum[key].indexOf("S_") != -1 || RegExp(/^V/i).test(apiVersionEnum[key])) ? false : true});
      }
      if (state.apiVersion.length > 0){
        state.filters.apiVersion = state.apiVersion[0].label;
      }
      getPermissions();
      state.store = useStore();
    });

    /**
     * @description 获取权限列表
     * @author weig
     */
    async function getPermissions (){
      state.loading = true;
      const param = {
        key: state.filters.label,
        start: state.filters.createTime ? formatTime(state.filters.createTime[0], "YYYY-MM-DD") : '',
        end: state.filters.createTime ? formatTime(state.filters.createTime[1], "YYYY-MM-DD") : '',
        apiVersion: state.filters.apiVersion
      };
      const res =  await getPermissionList(param);
      if (res && res.code == 1){
          const list = res.data;

          //分组树
          const groups = list.filter(l => l.type === 1);
          state.groupTree = listToTree(JSON.parse(JSON.stringify(groups)), {
            permissionId: 0,
            parentId: 0,
            label: '顶级'
          }, "permissionId") ;
          ++state.permissionGroup.key;

          //菜单树
          const menus = list.filter(l => l.type === 1 || l.type === 2);
          state.menuTree = listToTree(JSON.parse(JSON.stringify(menus)), {
            permissionId: 0,
            parentId: 0,
            label: '顶级'
          }, "permissionId");
          ++state.permissionMenu.key;

          const keys = list.filter(l => l.opened).map(l => l.permissionId + '');
          state.expandRowKeys = keys;

          list.forEach(l => {
              l._loading = false;
          });
          const tree = listToTree(list,  null, "permissionId");
          state.FrontPageData = tree;//页面全局缓存所有接口数据
          //前端加分页处理
          state.pageTotal = tree.length;//总数          
          pagination(state.pageIndex, state.pageSize, state.FrontPageData);
          // state.permissionTree = tree;
          state.loading = false;
      }
    }

    /**
     * @description 改变查询条件
     * @author weig
     * @param {String} val 值
     */
    const handleChangeQuery = (val)=>{
      //刷新列表
      getPermissions();
    }

    /**
     * @description 禁用日期
     * @author weig
     * @param {datetime} time 时间
     */
    const disabledDate = (time) =>{
        return time.getTime() > Date.now()
    }

    //分组 begin 
    /**
     * @description 新增分组
     * @author weig
     */
    const onOpenAddGroup = () =>{
      state.permissionGroup.addForm.createUserId = state.store.getters.userInfo.userId;
      state.permissionGroup.form = JSON.parse(JSON.stringify(state.permissionGroup.addForm));
      state.permissionGroup.form.apiVersion=state.filters.apiVersion;
      state.permissionGroup.visible = true;
      state.permissionGroup.dialogTitle = 1;
      state.apiVersionDisable = false;
    }

    /**
     * @description 关闭分组对话框
     * @author weig
     */
    const onCloseGroup = ()=>{
        permissionGroupForm.value.resetFields();//清空表单数据
        ++state.permissionGroup.key;
        state.permissionGroup.form = {};
        state.permissionGroup.visible = false;
        state.permissionGroup.dialogTitle = 0;
        state.apiVersionDisable = false;
    }

    /**
     * @description 分组校验
     * @author weig
     * @returns {Boolean}
     */
    const validateGroup = ()=> {
        let isValid = false;
        if (state.permissionGroup.form.parentIds.length === 0){
            ElMessage.error("上级分组不能为空！");
            return isValid;
        }
        if (IsNullOrEmpty(state.permissionGroup.form.label)){
            ElMessage.error("名称不能为空！");
            return isValid;
        }
        return true;
    }

    /**
     * @description 保存分组
     * @author weig
     */
    async function onSubmitGroup (){
        if(!validateGroup()){
            return;
        }
        state.permissionGroup.loading = true;
        const param = JSON.parse(JSON.stringify(state.permissionGroup.form));
        param.parentId = param.parentIds.pop();
        let res;
        if (state.permissionGroup.dialogTitle === 0){
            res = await updateGroup(param);
        } else {
            res = await addGroup(param);
        }
        state.permissionGroup.loading = false;

        if (res && 1 == res.code) {
            ElMessage.success(0 === state.permissionGroup.dialogTitle ? "更新分组成功" : "新增分组成功");
        }else {
          ElMessage.error(res.msg);
          return;
        }
        state.permissionGroup.visible = false;
        state.permissionGroup.dialogTitle = 0;
        getPermissions();
    }
    //分组 end 

    //菜单 begin
    /**
     * @description 新增菜单
     * @author weig
     */
    async function onOpenAddMenu () {
      if (0 === state.viewTree.length){
          const loading = ElLoading.service({fullscreen: true});
          await getViewList();
          loading.close();
      }
      state.permissionMenu.addForm.createUserId = state.store.getters.userInfo.userId;
      state.permissionMenu.form = JSON.parse(JSON.stringify(state.permissionMenu.addForm));
      state.permissionMenu.form.apiVersion=state.filters.apiVersion;
      state.permissionMenu.visible = true;
      ++state.permissionMenu.key;
      state.permissionMenu.dialogTitle = 1;
      state.apiVersionDisable = false;
    }

    /**
     * @description 菜单校验
     * @author weig
     */
    const validateMenu = ()=> {
        let isValid = false;
        if (state.permissionMenu.form.parentIds.length === 0){
            ElMessage.error("上级分组不能为空！");
            return isValid;
        }
        if (IsNullOrEmpty(state.permissionMenu.form.path)){
            ElMessage.error("访问地址不能为空！");
            return isValid;
        }
        if (IsNullOrEmpty(state.permissionMenu.form.label)){
            ElMessage.error("名称不能为空！");
            return isValid;
        }
        return true;
    }

    /**
     * @description 关闭菜单对话框
     * @author weig
     */
    const onCloseMenu = ()=>{
        menuForm.value.resetFields();//清空表单数据
        ++state.permissionMenu.key;
        state.permissionMenu.form = {};
        state.permissionMenu.visible = false;
        state.permissionMenu.dialogTitle = 0;
        state.apiVersionDisable = false;
    }

    /**
     * @description 保存菜单
     * @author weig
     */
    async function onSubmitMenu (){
        if(!validateMenu()){
            return;
        }
        state.permissionMenu.loading = true;
        const param = JSON.parse(JSON.stringify(state.permissionMenu.form));
        param.parentId = param.parentIds.pop();
        let res;
        if (0 === state.permissionMenu.dialogTitle){
            res = await updateMenu(param);
        } else {
            res = await addMenu(param);
        }
        state.permissionMenu.loading = false;

        if (res && 1 == res.code) {
            ElMessage.success(0 === state.permissionMenu.dialogTitle ? "更新菜单成功" : "新增菜单成功");
        } else {
          ElMessage.error(res.msg);
          return;
        }
        state.permissionMenu.visible = false;
        state.permissionMenu.dialogTitle = 0;
        getPermissions();
    }
    //菜单 end

    //权限点 begin
    /**
     * @description 新增权限点
     * @author weig
     */
    async function onOpenAddDot() {
        if (0 === state.apiTree.length){
            const loading = ElLoading.service({fullscreen: true});
            await getApiList();
            loading.close();
        }
        state.permissionDot.addForm.createUserId = state.store.getters.userInfo.userId;
        state.permissionDot.form = JSON.parse(JSON.stringify(state.permissionDot.addForm));
        state.permissionDot.form.apiVersion=state.filters.apiVersion;
        state.permissionDot.visible = true;
        ++state.permissionDot.key;
        state.permissionDot.dialogTitle = 1;
        state.apiVersionDisable = false;
    }

    /**
     * @description 权限点校验
     * @author weig
     */
    const validateDot = ()=> {
        let isValid = false;
        if (state.permissionDot.form.parentIds.length === 0){
            ElMessage.error("上级菜单不能为空！");
            return isValid;
        }
        if (IsNullOrEmpty(state.permissionDot.form.code)){
            ElMessage.error("编码不能为空！");
            return isValid;
        }
        if (IsNullOrEmpty(state.permissionDot.form.label)){
            ElMessage.error("名称不能为空！");
            return isValid;
        }
        return true;
    }

    /**
     * @description 关闭权限点对话框
     * @author weig
     */
    const onCloseDot = ()=>{
        dotForm.value.resetFields();//清空表单数据
        ++state.permissionDot.key;
        state.permissionDot.form = {};
        state.permissionDot.visible = false;
        state.permissionDot.dialogTitle = 0;
        state.apiVersionDisable = false;
    }

    /**
     * @description 保存权限点
     * @author weig
     */
    async function onSubmitDot (){
        if(!validateDot()){
            return;
        }
        state.permissionDot.loading = true;
        const param = JSON.parse(JSON.stringify(state.permissionDot.form));
        param.parentId = param.parentIds[param.parentIds.length - 1];
        let res;
        if (0 === state.permissionDot.dialogTitle){
            res = await updateDot(param);
        } else {
            res = await addDot(param);
        }
        state.permissionDot.loading = false;

        if (res && 1 == res.code) {
          ElMessage.success(0 === state.permissionDot.dialogTitle ? "更新权限点成功" : "新增权限点成功");
        } else {
          ElMessage.error(res.msg);
          return;
        }
        state.permissionDot.visible = false;
        state.permissionDot.dialogTitle = 0;
        getPermissions();
    }

    /**
     * @description 切换api
     * @author weig
     * @param {String} value
     */
    function onChangeApis(value){
      if (value.length == 0){
          return;
      } 
      const apis = treeToList(state.apiTree, null, 'children', 'apiId');
      var api = []
      value.forEach((data, d) =>{
        api = api.concat(apis.filter(a => a.apiId === value[d]));
      });
      if (api.length > 0){
          state.permissionDot.form.label = api[0].label;
          //api[0]  /api/admin/permission/getdot 替换后  :api:admin:permission:getdot 
          //编码
          state.permissionDot.form.code = "";
          state.permissionDot.form.label = "";
          api.forEach((data2, d2) => {
            var tempApi = "";
            tempApi = api[d2].path.replace(/\//g, ':');
            if (tempApi){
              var first = tempApi.indexOf(":");//第一个:
              tempApi = tempApi.substring(first + 1, tempApi.length);
              state.permissionDot.form.code += tempApi.toLowerCase() + ','; 
            }
          });
          if (state.permissionDot.form.code){
            var tempIndex = state.permissionDot.form.code.lastIndexOf(',');//去掉最后一个,
            state.permissionDot.form.code = state.permissionDot.form.code.substring(0, tempIndex);
          }

          //名称
          var temp = "";
          state.apiTree.forEach((item, index)=>{
            if (item.apiId == api[0].parentId){
              temp = item.label;
            }
          });
          if (temp){
            state.permissionDot.form.label = `${temp} - ${api[0].label}`
          }
      } else {
        state.permissionDot.form.code = "";
        state.permissionDot.form.label = "";
      }
    }
    //权限点 end

    /**
     * @description 多选操作
     * @author weig
     * @param {Array} selection 选中的数据
     */
    const onSelectAll =(selection)=> {
        const selections = treeToList(selection, null, 'children', 'apiId');
        const rows = treeToList(state.apiTree, null, 'children', 'apiId');
        const checked = selections.length === rows.length;
        rows.forEach(row => {
          multipleTable.value.toggleRowSelection(row, checked);
        });
        state.sels = multipleTable.value.store.states.selection.value;
    }

    /**
     * @description 单行选中
     * @author weig
     * @param {Array} selection 当前选中行的上级父级节点
     * @param {Object} row 当前行数据
     */
    const onSelect = (selection, row) =>{
      const checked = false;
      checked = selection.some(s => s.permissionId === row.permissionId);   
      if (row.children && row.children.length > 0){
        const rows = treeToList(row.children, null, 'children', 'apiId');
        rows.forEach(row => {
          multipleTable.value.toggleRowSelection(row, checked);
        });
      }
      state.sels = multipleTable.value.store.states.selection.value;
    }

    /**
     * @description 查询
     * @author weig
     */
    const handleSearch =()=>{
        getPermissions();
    }

    /**
     * @description 编辑
     * @author weig
     * @param {Number} index 行号
     * @param {Object} row 行数据
     */
    async function handleEdit (index, row){
      state.apiVersionDisable = true;
      const parents = getTreeParents(state.permissionTree, row.permissionId, 'children', 'permissionId');
      const parentIds = parents.map(p => p.permissionId);
      parentIds.unshift(0);

      const type = row.type;
      let loading = ElLoading.service({ fullscreen: true });
      if (1 == type){//分组
            const res = await getGroup(row.permissionId);
            loading.close();
            if (res && 1 == res.code){
                const data = res.data;
                data.parentIds = parentIds;
                state.permissionGroup.form = data;
                state.permissionGroup.visible = true;
                ++state.permissionGroup.key;
                state.permissionGroup.dialogTitle = 0;
            } 
      } else if (2 == type){//菜单
            if (0 === state.viewTree.length) {
                await getViewList();
            }
            const res = await getMenu(row.permissionId);
            loading.close();
            if (res && 1 == res.code){
                const data = res.data;
                data.parentIds = parentIds;
                state.permissionMenu.form = data;
                state.permissionMenu.visible = true;
                ++state.permissionMenu.key;
                state.permissionMenu.dialogTitle = 0;
            }
      } else if (3 == type){//权限点
            if (state.apiTree.length === 0){
                await getApiList();
            }
            const res = await getDot(row.permissionId);
            loading.close();
            if (res && 1 === res.code){
                const data = res.data;
                data.parentIds = parentIds;
                state.permissionDot.form = data;
                state.permissionDot.visible = true;
                ++state.permissionDot.key;
                state.permissionDot.dialogTitle = 0;
            }
        }
    }

    /**
     * @description 获取视图列表
     * @author weig
     */
    async function getViewList(){
        const res = await getAllViewInfo({apiVersion: state.currViewVersion})
        if (res && 1 == res.code) {
            state.viewTree = listToTree(JSON.parse(JSON.stringify(res.data)),  null, "viewId");
        }
    }

    /**
     * @description 获取api列表
     * @author weig
     */
    async function getApiList(){
        var param = {
          key: "",
          apiVersion: state.filters.apiVersion
        };
        const res = await getAllApiInfo(param);
        if (res && 1 == res.code) {
            state.apiTree = listToTree(JSON.parse(JSON.stringify(res.data)),  null, "apiId");
        }
    }

    /**
     * @description 切换view
     * @author weig
     * @param {Object} value
     */
    function onChangeView(value){
        const views = treeToList(state.viewTree, null, 'children', 'viewId');
        const view = views.find(a => a.viewId === value);
        if (view && view.label){
            state.permissionMenu.form.label = view.label;
        } 
        if (view && view.path){
            state.permissionMenu.form.path = `${view.path}`;
        }
    }
    /**
     * @description 删除
     * @author weig
     * @param {Object} row 行数据
     */
    const handleClickDelete = (index, row) =>{
      var id = row.permissionId;
      elConfirmDialog(ElMessageBox,'此操作将永久删除该数据, 是否继续?','提示', '', ()=>{
        softDeletePermission(id).then(res =>{
          if(res.code == 1){
              ElMessage.success(`删除第${index + 1}行成功`);
              getPermissions();
          } else {
              ElMessage.error(res.msg);
          }
        });
      }, ()=>{
          ElMessage.info("取消删除！");
      });
    }

    /** 
     * @description 前端分页
     * @author weig
     * @param {Number} pageNo 页码
     * @param {Number} pageSize 页大小
     * @param {Array} array 分页数据源
     */
    const pagination = (pageNo, pageSize,array)=>{
        var offset = (pageNo -1) * pageSize;
        state.permissionTree = (offset + pageSize >= array.length) ? array.slice(offset, array.length) : array.slice(offset, offset + pageSize);
    }

    /**
     * @description 子组件返回分页数据
     * @author weig
     * @param {Object} obj 分页参数
     */
    const resPageData = (obj) =>{
        state.pageIndex = obj.currPage;
        state.pageSize = obj.pageSize;
    }

    /**
     * @description 切换版本号
     * @author weig
     */
    const apiVerChangeHandle = (val)=>{
      state.currViewVersion = val;
      state.viewTree.length = 0;//清空视图
      state.apiTree.length = 0;//清空接口
    }

    return {
      state,
      onSelectAll,
      handleSearch,
      handleEdit,
      handleClickDelete,
      handleChangeQuery,
      multipleTable,
      permissionGroupForm,
      menuForm,
      apiForm,
      dotForm,
      onSelect,
      validateGroup,
      onCloseGroup,
      onSubmitGroup,
      onChangeView,
      onChangeApis,
      validateMenu,
      onCloseMenu,
      onSubmitMenu,
      validateDot,
      onCloseDot,
      onSubmitDot,
      getPermissions,
      disabledDate,
      onOpenAddGroup,
      onOpenAddMenu,
      onOpenAddDot,
      pagination,
      resPageData,
      apiVerChangeHandle,
    }
  },
}
</script>
<style>
</style>