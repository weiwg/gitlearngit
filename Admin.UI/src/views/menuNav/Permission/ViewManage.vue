<!--
 * @Description: 
 * @Author: weig
 * @Date: 2021-08-16 13:59:43
 * @LastEditors: weig
 * @LastEditTime: 2021-12-29 10:49:22
-->
<template>
  <div>
    <!-- 面包屑 begin -->
    <eup-crumbs icon="el-icon-setting" firstCrumbs="视图管理" secondCrumbs="视图列表" />
    <!-- 面包屑 end -->
    <!-- 内容区域 begin -->
    <!-- 表单内容 -->
    <div class="container">
      <!-- 查询 -->
      <div class="handle-box">
          <el-form :model="state.query" :inline="true" v-if="checkPermission([`api${state.VIEW_VERSION}:auth:view:getpage`,`api${state.VIEW_VERSION}auth:view:add`,`api${state.VIEW_VERSION}:auth:view:batchsoftdelete`])">
            <template v-if="checkPermission([`api${state.VIEW_VERSION}:auth:view:getpage`])">
              <el-form-item label="">
                <el-input v-model="state.query.key" placeholder="视图名或地址" class="handle-input mr10" @change="handleChangeQuery">
                  <template #prefix>
                    <i class="el-input__icon el-icon-search" />
                  </template>
                </el-input>
              </el-form-item>
              <el-form-item>
                <el-button type="primary" @click="handleSearch">查询</el-button>
              </el-form-item>
            </template>

              <!-- <el-form-item>
                <el-button 
                  type="primary"
                  icon="el-icon-refresh"
                  style="margin:0px;"
                  @click="onSync"
                  >同步视图
                </el-button>
              </el-form-item> -->
              <el-form-item v-if="checkPermission([`api${state.VIEW_VERSION}:auth:view:add`])">
                <el-button type="primary" icon="el-icon-plus" @click="handleClickAddData">新增</el-button>
              </el-form-item>
              <el-button
                  type="danger"
                  icon="el-icon-delete"
                  class="handle-del mr10"
                  @click="handleClickBatchDelete"
                  :disabled="state.sels.length === 0"
                  v-if="checkPermission([`api${state.VIEW_VERSION}:auth:view:batchsoftdelete`])"
                  >批量删除
              </el-button>
          </el-form>
      </div>

      <!-- 列表 -->
      <el-table
          border
          class="table"
          ref="multipleTable"
          header-cell-class-name="table-header"
          v-loading="state.loading"
          row-key="viewId"
          :data="state.viewTree"
          :default-expand-all="false"
          :expand-row-keys="state.expandRowKeys"
          :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
          highlight-current-row
          @select-all="onSelectAll"
          @select="onSelect"
      >
          <el-table-column type="selection" width="60"></el-table-column>
          <el-table-column prop="label" min-width="150"  label="视图名称"></el-table-column>
          <el-table-column prop="name" min-width="150" label="视图命名"></el-table-column>
          <el-table-column prop="path" min-width="220" label="视图地址"></el-table-column>
          <el-table-column prop="description" min-width="200" label="视图描述"></el-table-column>
          <el-table-column prop="isActive" min-width="120" align="center" label="状态">
              <template #default="{row}">
                  <el-tag :type="row.isActive == state.isActive.yes ? 'success' : 'danger'" disable-transitions>
                      {{ row.isActive == state.isActive.yes ? '正常' : '禁用' }}
                  </el-tag>
              </template>
          </el-table-column>
          <el-table-column label="操作" min-width="240" align="center" fixed="right" v-if="checkPermission([`api${state.VIEW_VERSION}:auth:api:getpage`])">
                  <template #default="{ $index, row }">
                    <el-button
                        type="primary"
                        icon="el-icon-edit"
                        @click="handleEdit($index,row)"
                        v-if="checkPermission([`api${state.VIEW_VERSION}:auth:view:update`])"
                    >编辑</el-button>
                    <el-button
                        type="danger"
                        icon="el-icon-delete"
                        @click="handleClickDelete(row)"
                        class="ml5"
                        v-if="checkPermission([`api${state.VIEW_VERSION}:auth:view:softdelete`])"
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
    <!-- 内容区域 end -->
    <!-- 添加/编辑窗口 begin -->
    <el-dialog 
      :title="state.dialogTitle"
      v-model="state.addDialogFormVisible"
      width="60%"
      @close="closeEditForm()"
      :fullscreen="state.dialogFull"
      >
      <el-form
        ref="addForm"
        :model="state.form"
        :rules="state.addFormRules"
        label-width="100px"
        :inline="false"
      >
        <el-form-item prop="parentIds" label="上级视图">
          <el-cascader
            :key="state.addFormKey"
            v-model="state.form.parentIds"
            placeholder="请选择，支持搜索功能"
            :options="state.modules"
            :props="{checkStrictly: true, value: 'viewId'}"
            filterable
            style="width:100%;"
          />
        </el-form-item>
        <el-form-item label="视图名称" prop="label">
          <el-input v-model="state.form.label" auto-complete="off" />
        </el-form-item>
        <el-form-item label="视图命名" prop="name">
          <el-input v-model="state.form.name" auto-complete="off" />
        </el-form-item>
        <el-form-item label="视图地址" prop="path">
          <el-input v-model="state.form.path" auto-complete="off" />
        </el-form-item>
        <el-form-item label="启用" prop="isActive">
          <el-switch v-model="state.form.isActive" />
        </el-form-item>
        <el-form-item label="说明" prop="description">
          <el-input v-model="state.form.description" type="textarea" rows="2" auto-complete="off" />
        </el-form-item>
      </el-form>
      <template #title>
        <div class="avue-crud__dialog__header">
            <span class="el-dialog__title">
            <span style="display:inline-block;width:3px;height:20px;margin-right:5px; float: left;margin-top:2px"></span>
              {{state.dialogTitle}}
            </span>
       <div class="avue-crud__dialog__menu" @click="state.dialogFull? state.dialogFull=false: state.dialogFull=true">
            <i class="el-icon-full-screen" title="全屏" v-if="!state.dialogFull"></i>
            <i class="el-icon-copy-document" title="缩小" v-else></i>
          </div> 
        </div>
      </template>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="closeEditForm()">取 消</el-button>
          <el-button type="primary" @click="addDialogFormSave()">确 定</el-button>
        </span>
      </template>
    </el-dialog>
    <!-- 添加/编辑窗口 end -->
  </div>
</template>

<script>
import { reactive, toRefs, onBeforeMount, onMounted, ref } from 'vue'
import { getViewListPage, getViewInfo, getAllViewInfo, addView, editView, removeView, batchRemoveView, syncView} from '@/serviceApi/permission/view'
import { ElMessage,ElMessageBox } from 'element-plus'
import {elConfirmDialog, IsNullOrEmpty} from "@/common/js/comm"
import { formatTime, treeToList, listToTree, getTreeParents } from '@/utils/tool'
import {useStore} from 'vuex'
import EupPagination from "@/components/EupPagination.vue"
import EupCrumbs from "../../../components/eup-crumbs/index.vue"
import EnumConfig from "@/enum/EnumConfig"

var CURR_VIEW_VERSION = EnumConfig.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var VIEW_VERSION = CURR_VIEW_VERSION == 'V0' ? '' : `:S:${CURR_VIEW_VERSION}`;
export default {
  name: 'ViewManage',
  components:{
    EupPagination: EupPagination,
    EupCrumbs: EupCrumbs
  },
  setup(props, context) {
    const multipleTable = ref(null);
    const addForm = ref(null);
    const state = reactive({
        query: {
            key: "", //视图名称/视图地址
            apiVersion: ""
        },
        VIEW_VERSION: VIEW_VERSION,
        pageIndex: 1,
        pageSize: 10,
        pageTotal: 0,
        tableData: [],
        delList: [],
        dynamicFilter:{},
        form: {
          viewId: 0,
          parentIds: [],
          path: '',
          label: '',
          httpMethods: '',
          isActive: false, 
          description: '',
          name: "",
          apiVersion: ""
        },
        isActive: {
          yes: 1,
          no: 2
        },
        addFormRules:{
          parentIds: [{ required: true, message: '请选择上级视图', trigger: 'change' }],
          path: [{ required: true, message: '请输入视图地址', trigger: 'blur' }],
          label: [{ required: true, message: '请输入视图名', trigger: 'blur' }],
        },
        idx: -1,
        loading: false,
        store: {}, //vuex全局状态管理对象
        addDialogFormVisible: false, //是否显示对话框
        dialogTitle: "", //对话框标题
        dialogType: 0, //对话框类型, 0:编辑框  1:新增框
        dialogWidth: 800,
        dialogFull: false, //是否为全屏 Dialog
        expandRowKeys: [],

        modules: [],
        sels: [], // 列表选中列
        viewTree: [],
        addFormKey: 1,

        FrontPageData: [],//所有的视图数据
    });
    onBeforeMount(() => {
    });
    onMounted(() => {
      getData();
      state.store = useStore();
    });

    /**
     * @description 获取表单数据
     * @author weig
     */
    const getData = () =>{
      state.loading = true;
      // var params = {
      //   "currentPage": state.pageIndex,
      //   "pageSize": state.pageSize,
      //   "filter": {
      //     "label": state.query.key
      //   }
      // }
      getAllViewInfo(state.query).then(function(res){
        if(res.code == 1){
            let list = JSON.parse(JSON.stringify(res.data));
            state.tableData = list;
            const keys = state.tableData.filter(l => l.parentId === 0 || l.parentId == "" || l.parentId == "0").map(l => l.viewId + '');
            state.expandRowKeys = keys;
            state.modules = listToTree(JSON.parse(JSON.stringify(state.tableData)),{
              viewId: 0,
              parentId: 0,
              label: '顶级'
            }, "viewId");
            list.forEach(l => {
              l._loading = false
            });
            const tree = listToTree(list, null, "viewId");
            state.sels = [];
            state.FrontPageData = tree;//页面全局缓存所有接口数据
            //前端加分页处理
            state.pageTotal = tree.length;//总数          
            pagination(state.pageIndex, state.pageSize, state.FrontPageData);
        } else {
            ElMessage.error(res.msg);   
        }
        state.loading = false;
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
     * @description 改变查询条件
     * @author weig
     * @param {String} val 值
     */
    const handleChangeQuery = (val)=>{
      //刷新列表
      getData();
    }

    /**
     * @description 多选操作
     * @author weig
     * @param {Array} selection 选中的数据
     */
    const onSelectAll =(selection)=> {
        const selections = treeToList(selection, null, 'children', 'viewId');
        const rows = treeToList(state.viewTree, null, 'children', 'viewId');
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
      const checked = selection.some(s => s.viewId === row.viewId);
      if (row.children && row.children.length > 0){
        const rows = treeToList(row.children, null, 'children', 'viewId');
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
        state.pageIndex = 1
        getData();
    }

    /**
     * @description 新增
     * @author weig
     */
    const handleClickAddData = () =>{
      state.addDialogFormVisible = true;
      state.dialogType = 1;
      state.dialogTitle = "新增";
    }

    /**
     * @description 批量删除
     * @author weig
     */
    const handleClickBatchDelete = () =>{
      const param = {ids:[]};
      if (state.sels.length == 0){
        ElMessage.error("请选择要删除的数据！");
        return;
      }
      param.ids = state.sels.map(s => {
        return s.viewId;      
      });
      ElMessageBox.confirm('此操作将删除选中的记录, 是否继续?', '提示',{
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
      }).then(()=>{
          batchRemoveView(param.ids).then(res=>{
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

    /**
     * @description 编辑
     * @author weig
     * @param {Number} index 行号
     * @param {Object} row 行数据
     */
    async function handleEdit (index, row){
      state.addDialogFormVisible = true;
      state.dialogType = 0;
      state.dialogTitle = "编辑";
      state.idx = index;
      const res = await getViewInfo(row.viewId)
      if (res && res.code == 1){
        const parents = getTreeParents(state.viewTree, row.viewId, "children", "viewId");
        const parentIds = parents.map(p => p.viewId);
        parentIds.unshift(0);
        const data = res.data;
        data.parentIds = parentIds;
        state.form.parentIds = data.parentIds;
        state.form.path = data.path;
        state.form.label = data.label;
        state.form.viewId = data.viewId;
        state.form.httpMethods = data.httpMethods;
        state.form.isActive = data.isActive == state.isActive.yes ? true : false;
        state.form.description = data.description;
        state.form.name = data.name;
        ++state.addFormKey;
      } else {
        ElMessage.error(res.msg)
      }
    }

    /**
     * @description 删除
     * @author weig
     * @param {Object} row 行数据
     */
    const handleClickDelete = (row) =>{
      var id = row.viewId;
      elConfirmDialog(ElMessageBox,'此操作将永久删除该数据, 是否继续?','提示', '', ()=>{
        removeView(id).then(res =>{
          if(res.code == 1){
              ElMessage.success(`删除第${state.idx + 1}行成功`);
              getData();
          } else {
              ElMessage.error(res.msg);
          }
        });
      }, ()=>{
          ElMessage.info("取消删除！");
      });
    }

    /**
     * @description 关闭对话框
     * @author weig
     */
    const closeEditForm = ()=>{
        state.addDialogFormVisible = false;
        addForm.value.resetFields();//清空表单数据
        state.dialogType = 0;
        ++state.addFormKey;
        state.form = {
          viewId: 0,
          parentIds: [],
          path: '',
          label: '',
          httpMethods: '',
          isActive: false, //编辑默认：false   新增默认: true
          description: '',
          name: ""
        };
    }

    /**
     * @description 确认保存
     * @author weig
     */
    const addDialogFormSave = ()=>{
      if(state.form.parentIds.length === 0){
        ElMessage.error("上级视图不能为空！");
        return;
      }
      if(IsNullOrEmpty(state.form.label)){
        ElMessage.error("视图名称不能为空！");
        return;
      }
      if(IsNullOrEmpty(state.form.path)){
        ElMessage.error("视图地址不能为空！");
        return;
      }
      const copyData = JSON.parse(JSON.stringify(state.form))
      const parentId = copyData.parentIds.pop();

      if (copyData.viewId === parentId && parentId !== 0){
        ElMessage.error("上级视图不能是自己！");
        state.addDialogFormVisible = false;
        return;
      }
      let params = {};
      switch (state.dialogType){
        case 0://编辑
            params = {
              "parentId": state.form.parentIds.pop(),
              "label": state.form.label,
              "path": state.form.path,
              "httpMethods": state.form.httpMethods,
              "description": state.form.description,
              "isActive": state.form.isActive ? state.isActive.yes : state.isActive.no,
              "viewId": state.form.viewId,
              updateUserId: state.store.getters.userInfo.userId,
              name: state.form.name,
            }
            editView(params).then(res =>{
                if(res.code == 1){
                    ElMessage.success(`修改${state.idx + 1}行成功`);
                    state.addDialogFormVisible = false;
                    getData();
                } else {
                    ElMessage.error(res.msg);
                }
            });
            break;
        case 1://新增
              params = {
                "parentId": state.form.parentIds.pop(),
                "label": state.form.label,
                "path": state.form.path,
                "httpMethods": state.form.httpMethods,
                "description": state.form.description,
                "isActive": state.form.isActive ? state.isActive.yes : state.isActive.no,
                createUserId: state.store.getters.userInfo.userId,
                name: state.form.name,
              }
              addView(params).then(res =>{
                  if(res.code == 1){
                      ElMessage.success("添加成功");
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
     * @description 同步视图
     * @author weig
     * @param
     */
    async function onSync (){
        const bUnFinish = true;
        if (bUnFinish){
            ElMessage.warning("开发中，敬请期待！")
        }
        const views = [];
        elConfirmDialog(ElMessageBox,'此操作将永久删除该数据, 是否继续?','提示', '', ()=>{
         const syncRes = syncView({views});
         if (syncRes && syncRes.code){
            ElMessage.success(`同步数据成功`);
            getData();
         } else {
            ElMessage.error(`同步数据失败！`);
         }
        }, ()=>{
            ElMessage.info("取消删除！");
        });
    }

    /** 
     * @description 前端分页
     * @author weig
     * @param {Number} pageNo
     * @param {Number} pageSize
     * @param {Array} array
     */
    const pagination = (pageNo, pageSize,array)=>{
        var offset = (pageNo -1) * pageSize;
        state.viewTree = (offset + pageSize >= array.length) ? array.slice(offset, array.length) : array.slice(offset, offset + pageSize);
    }
    return {
      state,
      onSelectAll,
      handleSearch,
      handleClickAddData,
      handleClickBatchDelete,
      handleEdit,
      handleClickDelete,
      getData,
      handleChangeQuery,
      multipleTable,
      addForm,
      onSelect,
      closeEditForm,
      addDialogFormSave,
      onSync,
      resPageData,
      pagination
    }
  },
}

</script>
<style scoped>
</style>