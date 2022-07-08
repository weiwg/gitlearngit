<!--
 * @Description: 
 * @Author: weig
 * @Date: 2021-07-14 14:58:08
 * @LastEditors: weig
 * @LastEditTime: 2021-12-17 15:55:39
-->
<template>
    <div>
        <!-- 对话框模式 begin -->
        <el-dialog
            :title="title"
            v-model="visible"
            width="80%"
            @close="onCancel"
            @opened="onSearch"
            :fullscreen="state.dialogFull"
        >
            <eup-container v-loading="state.loadingPermissions" :show-header="false" :show-footer="false">
                <el-table
                    ref="multipleTable"
                    :data="state.permissionTree"
                    :default-expand-all="true"
                    :tree-props="{children: 'children', hasChildren: 'hasChildren' }"
                    row-key="permissionId"
                    highlight-current-row
                    style="width: 100%;"
                    @select-all="onSelectAll"
                    @select="onSelect"
                >
                <el-table-column type="selection" width="50" />
                <el-table-column prop="label" label="导航菜单" width="200" />
                <el-table-column label="菜单接口" width>
                <template #default="{ row }">
                    <el-checkbox-group v-if="row.apis && row.apis.length > 0" v-model="state.chekedApis">
                        <el-checkbox v-for="api in row.apis" :key="api.permissionId" :label="api.permissionId" @change="(value)=>onChange(value, row.permissionId)">{{ api.label }}</el-checkbox>
                    </el-checkbox-group>
                </template>
                </el-table-column>
                </el-table>
            </eup-container>
            
            <template #title>
                <div class="avue-crud__dialog__header">
                    <span class="el-dialog__title">
                    <span style="display:inline-block;width:3px;height:20px;margin-right:5px; float: left;margin-top:2px"></span>
                    {{title}}
                    </span>
                <div class="avue-crud__dialog__menu" @click="state.dialogFull? state.dialogFull=false: state.dialogFull=true">
                    <i class="el-icon-full-screen" title="全屏" v-if="!state.dialogFull"></i>
                    <i class="el-icon-copy-document" title="缩小" v-else></i>
                </div>
                </div>
            </template>
            
            <div class="drawer-footer" style="margin: 10px 0 0; text-align: center;">
                <el-button @click="onCancel">取消</el-button>
                <el-button type="primary" @click="onSure">确 定</el-button>
            </div>
        </el-dialog>
        <!-- 对话框模式 end -->

        <!-- 抽屉模式 begin -->
        <!-- <eup-window
            v-loading="state.loadingPermissions"
            :title="title"
            :modal="modal"
            :wrapperClosable="true"
            :modalAppendToBody="modalAppendToBody"
            :visible="visible"
            :beforeClose="onCancel"
            embed
            drawer
            size="100%"
            @opened="onSearch"
        >
            <el-table
                ref="multipleTable"
                :data="state.permissionTree"
                :default-expand-all="true"
                :tree-props="{children: 'children', hasChildren: 'hasChildren'}"
                row-key="id"
                highlight-current-row
                style="width:100%"
                @select-all="onSelectAll"
                @select="onSelect"
            >
                <el-table-column type="selection" width="50" />
                <el-table-column prop="label" label="导航菜单" width="200" />
                <el-table-column label="菜单接口" width>
                <template #default="{ row }">
                    <el-checkbox-group v-if="row.apis && row.apis.length > 0" v-model="state.chekedApis">
                        <el-checkbox v-for="api in row.apis" :key="api.id" :label="api.id" @change="(value)=>onChange(value, row.id)">{{ api.label }}</el-checkbox>
                    </el-checkbox-group>
                </template>
                </el-table-column>
            </el-table>
        </eup-window> -->
        <!-- 抽屉模式 end -->

    </div>
</template>

<script>
import { treeToList, listToTree, getTreeParentsWithSelf } from '@/utils/tool.js'
import { reactive, toRefs, onBeforeMount, onMounted, ref } from 'vue'
import { getPermissions, getPermissionIds, GetTenantPermissionIds } from '@/serviceApi/permission/permission'
import EupContainer from '@/components/eup-container'
import EupWindow from '@/components/eup-window'
export default {
    name: 'EupSelectPermission',
    components:{
        EupContainer:EupContainer,
        EupWindow
    },
    props: {
        visible: {
            type: Boolean,
            default: false
        },
        modal: {
            type: Boolean,
            default: false
        },
        modalAppendToBody: {
            type: Boolean,
            default: false
        },
        roleId: {
            type: String,
            default: ''
        },
        tenantId: {
            type: String,
            default: ""
        },
        title: {
            type: String,
            default: '设置权限'
        },
        tenant:{
            type: Boolean,
            default: false
        },
        setPermissionLoading: {
            type: Boolean,
            default: false
        },
    },
    emits: ["click",'showVisible',"update","cancel"],
    setup(props, context) {
        const multipleTable = ref();
        const state = reactive({
            permissionTree: [],
            apis: [],
            loadingPermissions: false,
            checkedPermissions: [],
            chekedApis: [],
            dialogFull: false
        });
        onBeforeMount(() => {
        });
        onMounted(() => {
        });

        /**
         * @description 取消
         * @author weig
         * @param
         */
        const onCancel = ()=>{
            state.chekedApis = [];
            state.checkedPermissions = [];
            multipleTable.value.clearSelection();
            context.emit("showVisible", false);//触发父组件自定义事件修改对话框显示隐藏
            context.emit('update', false)
            context.emit('cancel')
        }

        /**
         * @description 确定
         * @author weig
         * @param
         */
        const onSure= ()=>{
            const permissionIds = [...state.checkedPermissions];
            if (state.chekedApis.length > 0) {
                permissionIds.push(...state.chekedApis);
            }
            context.emit('click', permissionIds);
        }

        /**
         * @description 查询权限
         * @author weig
         * @param
         */
        const onSearch = ()=>{
            getPermissions_func();
        }

        /**
         * @description 改变权限
         * @author weig
         * @param {Boolean} value 是否选中
         * @param {String} id 菜单权限ID
         */
        const onChange = (value, id)=>{
            if (value){ //选中
                const parents = getTreeParentsWithSelf(state.permissionTree, id, "children", "permissionId");
                parents.forEach(parent => {
                    const checked = state.checkedPermissions.includes(parent.permissionId);
                    if (!checked) {
                        multipleTable.value.toggleRowSelection(parent, true);
                    }
                });

                state.checkedPermissions = multipleTable.value.store.states.selection.value.map(s => {
                    return s.permissionId
                });
            } 
        }
        
        /**
         * @description 获取加载权限树
         * @author weig
         * @param
         */
        async function getPermissions_func (){
            state.loadingPermissions = true;
            onSelectAll([]);

            const param = {};
            const res = await getPermissions(param);
            state.loadingPermissions = false;
            const tree = listToTree(JSON.parse(JSON.stringify(res.data)), null, "permissionId");
            state.permissionTree = tree;
            getRolePermission();
        }

        /**
         * @description 获取角色权限
         * @author weig
         * @param
         */
        async function getRolePermission (){
            if (!(props.roleId !="" || props.tenantId != "")) {
                return;
            }
            state.loadingPermissions = true;
            const para = { roleId: props.roleId };
            const res = await (props.tenant ? GetTenantPermissionIds(props.tenantId) : getPermissionIds(para.roleId));
            state.loadingPermissions = false;
            const permissionIds = res.data;
            const rows = treeToList(state.permissionTree, null, "children", "permissionId");
            rows.forEach(row => {
                const checked = permissionIds.includes(row.permissionId);
                multipleTable.value.toggleRowSelection(row, checked);
            });
            state.checkedPermissions = multipleTable.value.store.states.selection.value.map(s => {
                return s.permissionId;
            });
            const apiIds = [];
            permissionIds.forEach(permissionId => {
                if (!state.checkedPermissions.includes(permissionId)) {
                    apiIds.push(permissionId);
                }
            })
            state.chekedApis = apiIds;
        }

        /**
         * @description 选择全部
         * @author weig
         * @param {Array} selection 选中所有行
         */
        const onSelectAll =(selection)=>{
            const selections = treeToList(selection, null, "children", "permissionId");
            const rows = treeToList(state.permissionTree, null, "children", "permissionId");
            const checked = selections.length === rows.length;
            rows.forEach(row => {
                multipleTable.value.toggleRowSelection(row, checked);
                selectApis(checked, row);
            });

            state.checkedPermissions = multipleTable.value.store.states.selection.value.map(s => {
                return s.permissionId;
            });
        }

        /**
         * @description 选中行
         * @author weig
         * @param {Array} selection
         * @param {Object} row 当前行
         */
        const onSelect = (selection, row)=>{
            const checked = selection.some(s => s.permissionId === row.permissionId);
            if (row.children && row.children.length > 0) {
                multipleTable.value.toggleRowSelection(row, checked);
                const rows = treeToList(row.children, null, "children", "permissionId");
                rows.forEach(r => {
                    if (!checked){
                        multipleTable.value.toggleRowSelection(r, false);
                        selectApis(false, r);
                    } else {
                        multipleTable.value.toggleRowSelection(r, true);
                        selectApis(checked, r);
                    }
                });
            } else {
                selectApis(checked, row);
            }

            const parents = getTreeParentsWithSelf(state.permissionTree, row.permissionId, "children", "permissionId");
            parents.forEach(parent => {
                const checked = state.checkedPermissions.includes(parent.permissionId);
                if (!checked) {
                    multipleTable.value.toggleRowSelection(row, true);
                } else {
                    multipleTable.value.toggleRowSelection(row, false);
                }
            });

            state.checkedPermissions = multipleTable.value.store.states.selection.value.map(s => {
                return s.permissionId;
            });
        }

        /**
         * @description 自动选择/取消菜单接口
         * @author weig
         * @param {Boolean} checked 是否选中
         * @param {Array} row 菜单接口数组
         */
        const selectApis = (checked, row)=>{
            if (row.apis) {
                row.apis.forEach(a => {
                    const index = state.chekedApis.indexOf(a.permissionId);
                    if (checked) {
                        if (index === -1) {
                            state.chekedApis.push(a.permissionId);
                        }
                    } else {
                        if (index > -1) {
                            state.chekedApis.splice(index, 1);
                        }
                    }
                });
            }
        }
        return{
            state,
            multipleTable,
            onCancel,
            onSure,
            onSearch,
            onSelect,
            onSelectAll,
            onChange
        }        
    },
}

</script>
<style scoped>
</style>
