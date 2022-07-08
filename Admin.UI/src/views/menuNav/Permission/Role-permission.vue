<!--
 * @Description: 
 * @Author: weig
 * @Date: 2021-08-20 08:46:50
 * @LastEditors: weig
 * @LastEditTime: 2021-12-09 16:11:21
-->
<template>
  <section style="padding:10px;">
      <el-row :gutter="10">
          <el-col :xs="24" :sm="6" class="toolar roles">
              <el-card>
                  <template #header>
                      <div class="clearfix">
                          <span>角色</span>
                          <el-button
                          :loading="loadingRoles"
                          type="text"
                          style="float:right;padding:3px 0;"
                          @click="refreshRoles"
                          >刷新</el-button>
                      </div>
                  </template>  
                  <div class="role-box">
                      <div
                        v-for="o in roles"
                        :key="o.id"
                        :class="o.id == roleId ? 'active' : ''"
                        class="item role-item"
                        @click="roleSelect(o.id)"
                      >
                        <span>{{ o.name}}</span>
                        <span style="float:right;">{{ o.description}}</span>
                      </div>
                  </div>
                  <!-- 分页 -->
                  <EupPagination
                    :current-page="pageIndex"
                    :pagesize="pageSize"
                    :total="pageTotal"
                    layout="simpleJumper"
                    @getPageData="getRoles"
                    @resPageData="resPageData">
                  </EupPagination>
              </el-card>
          </el-col>
          <el-col :xs="24" :sm="18" class="toolbar perms">
              <el-card>
                  <template #header>
                      <div class="clearfix">
                          <span>权限</span>
                          <el-button
                            :loading="loadingSave"
                            type="text"
                            style="float: right;"
                            @click="save"
                          >保存</el-button>
                          <el-button
                            :loading="loadingPermissions"
                            type="text"
                            style="float: right; padding: 3px 10px 3px 0px"
                            @click="getPermissionTree"
                          >刷新</el-button>
                      </div>
                  </template>
                  <el-table
                    ref="multipleTable"
                    :data="permissionTree"
                    :default-expand-all="true"
                    :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
                    row-key="id"
                    highlight-current-row
                    style="width: 100%;"
                    @select-all="onSelectAll"
                    @select="onSelect"
                    >
                        <el-table-column type="selection" width="50" />
                        <el-table-column prop="label" label="导航菜单" width="200" />
                        <el-table-column label="菜单接口" width>
                        <template #default="{ row }">
                            <el-checkbox-group v-if="row.apis && row.apis.length > 0" v-model="chekedApis">
                            <el-checkbox v-for="api in row.apis" :key="api.id" :label="api.id" @change="(value)=>onChange(value, row.id)">{{ api.label }}</el-checkbox>
                            </el-checkbox-group>
                        </template>
                        </el-table-column>               
                  </el-table>
              </el-card>
          </el-col>
      </el-row>
  </section>
</template>

<script>
import { reactive, toRefs, ref,onBeforeMount, onMounted,defineComponent } from 'vue'
import EupPagination from "@/components/EupPagination.vue"
import { formatTime, treeToList, listToTree, getTreeParentsWithSelf } from '@/utils/tool'
import { ElMessage,ElMessageBox, ElLoading } from 'element-plus'
import {elConfirmDialog,IsNullOrEmpty} from "@/common/js/comm"
import { getRoleListPage } from '@/serviceApi/admin/role'
import { getPermissions, getPermissionIds, addRolePermission } from '@/serviceApi/permission/permission'
import {useStore} from 'vuex'
export default defineComponent({
  name: 'RolePermission',
  components:{
    EupPagination: EupPagination,
  },
  setup(props, context) {
    const multipleTable = ref(null);
    const data = reactive({
        pageIndex: 1,
        pageSize: 10,
        pageTotal: 0,
        roleId: "",
        loadingRoles: false,
        loadingPermissions: false,
        loadingSave: false,
        store: {}, //vuex全局状态管理对象
        roles: [],
        permissionTree: [],
        apis: [],
        checkedPermissions: [],
        chekedApis: []
    });
    onBeforeMount(() => {
    });
    onMounted(() => {
      data.store = useStore();
      getRoles();
      getPermissionTree();
    });

    /** 刷新角色
     * @description
     * @author weig
     */
    function refreshRoles(){
        getRoles();
        getPermissionTree();
    }

    /**
     * @description 获取角色列表
     * @author weig
     */
    async function getRoles(){
        var params = {
            pageSize: data.pageSize,
            currentPage: data.pageIndex
        };
        /* const param = {
            ...pager
        }; */
        data.loadingRoles = true;
        const res = await getRoleListPage(params);
        if (res && 1 === res.code){
            data.pageTotal = res.data.total;
            data.roles = res.data?.list;
        } else {
            ElMessage.error(res.msg)
        }
        data.loadingRoles = false;
    }

    /**
     * @description 获取权限树
     * @author weig
     */
    async function getPermissionTree(){
        data.loadingPermissions = true;
        onSelectAll([]);

        const param = {};
        const res = await getPermissions(param);
        if (res && res.code === 1){
            const tree = listToTree(JSON.parse(JSON.stringify(res.data)));
            data.permissionTree = tree;
            getRolePermission();
        } else {
            ElMessage.error(res.msg);
        }
        data.loadingPermissions = false;
    }

    /**
     * @description 选择全部
     * @author weig
     * @param {Object} selection 被选中的数据
     */
    const onSelectAll = (selection)=>{
        const selections = treeToList(selection);
        const rows = treeToList(data.permissionTree);
        const checked = selections.length === rows.length;
        rows.forEach(row =>{
            multipleTable.value.toggleRowSelection(row, checked);   
            selectApis(checked, row); 
        });
        data.checkedPermissions = multipleTable.value.store.states.selection.value.map(s =>{
            return s.id;
        });
    }

    /**
     * @description 选中
     * @author weig
     * @param {Array} selection 选中的所有行
     * @param {Object} row 当前点击的行数据
     */
    const onSelect = (selection, row)=>{
        const checked = selection.some(s => s.id === row.id);
        if (row.children && row.children.length > 0){
            const rows = treeToList(row.children);
            rows.forEach(function(row){
                multipleTable.value.toggleRowSelection(row, checked);   
                selectApis(checked, row); 
            });
        } else {
            selectApis(checked, row); 
        }

        const parents = getTreeParentsWithSelf(data.permissionTree, row.id);
        parents.forEach(function(parent){
            const checked = data.checkedPermissions.includes(parent.id);
            if (!checked){
                multipleTable.value.toggleRowSelection(parent, true); 
            }
        });

        data.checkedPermissions = multipleTable.value.store.states.selection.value.map(s =>{
            return s.id;
        });
    }

    /**
     * @description 选中接口
     * @author weig
     * @param {Boolean} checked 
     * @param {Object} row
     */
    const selectApis = (checked, row)=>{
        if (row.apis){
            row.apis.forEach(a =>{
                const index = data.chekedApis.indexOf(a.id);
                if (checked){
                    if (index === -1){
                        data.chekedApis.push(a.id);
                    }
                } else {
                    if (index > -1){
                        data.chekedApis.splice(index, 1);
                    }
                }
            });
        }
    }

    /**
     * @description 选中角色
     * @author weig
     * @param {String} id
     */
    const roleSelect =(id)=>{
        data.roleId = id;
        onSelectAll([]);
        getRolePermission();
    }

    /**
     * @description 获取角色权限
     * @author weig
     */
    async function getRolePermission(){
        if(IsNullOrEmpty(data.roleId)){
            return;
        }

        data.loadingPermissions = true;
        const param = data.roleId;
        const res = await getPermissionIds(param);
        if (res && 1 === res.code){
            const permissionIds = res.data;
            const rows = treeToList(data.permissionTree);
            rows.forEach(row => {
                const checked = permissionIds.includes(row.id);
                multipleTable.value.toggleRowSelection(row, checked); 
            });
            data.checkedPermissions = multipleTable.value.store.states.selection.value.map(s =>{
                return s.id;
            });

            const apiIds = [];
            permissionIds.forEach(permissionId => {
                if (data.checkedPermissions.includes(permissionId)){
                    apiIds.push(permissionId);
                }
            });
            data.chekedApis = apiIds;
        } else {
            ElMessage.error(res.msg);
        }
        data.loadingPermissions = false;
    }

    /**
     * @description 保存权限
     * @author weig
     */
    async function save (){
        if(!saveValidate()){
            return;
        }
        const permissionIds = [...data.checkedPermissions];
        if (data.chekedApis.length > 0){
            permissionIds.push(...data.chekedApis);
        }
        const param = {permissionIds, roleId: data.roleId};

        data.loadingSave = true;
        const res = await addRolePermission(param);
        if (res && 1 === res.code){
            ElMessage.success("保存成功");
        } else {
            ElMessage.error(res.msg ? res.msg : "保存失败");
        }
        data.loadingSave = false;
    }

    /**
     * @description 验证保存
     * @author weig
     */
    const saveValidate =()=>{
        let isValid = true;
        if (IsNullOrEmpty(data.roleId)){
            ElMessage.warning("请选择角色！");
            isValid = false;
            return isValid;
        }
        return isValid;
    }

    /**
     * @description 单个接口改变
     * @author weig
     * @param {Boolean} value 是否选中
     * @param {string} id
     */
    const onChange = (value, id)=>{
        if(value){
            const parents = getTreeParentsWithSelf(data.permissionTree, id);
            parents.forEach(parent => {
                const checked = data.checkedPermissions.includes(parent.id);
                if (!checked){
                    multipleTable.value.toggleRowSelection(parent, true); 
                }
            });   
            data.checkedPermissions = multipleTable.value.store.states.selection.value.map(s =>{
                return s.id;
            });   
        }
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
    return {
      ...toRefs(data),
      refreshRoles,
      resPageData,
      multipleTable,
      roleSelect,
      getPermissionTree,
      save,
      onChange,
      onSelectAll,
      onSelect,
      getRoles,
    }
  },
})

</script>
<style scoped>
</style>