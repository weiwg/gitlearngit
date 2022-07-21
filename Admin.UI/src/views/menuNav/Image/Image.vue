<!--
 * @Description: 
 * @Author: 优
 * @Date: 2021-08-05 15:47:11
 * @LastEditors: weig
 * @LastEditTime: 2021-12-17 09:44:52
-->
<template>
  <div>
        <!-- 面包屑 begin -->
        <eup-crumbs icon="el-icon-picture-outline" firstCrumbs="资源管理" secondCrumbs="图片中心" />
        <!-- 面包屑 end -->
        <!-- 内容区域 begin -->
         <div class="container">
            <div class="handle-box">
                <el-form ref="state.query" :model="state.query" :inline="true" v-if="checkPermission([`api${state.VIEW_VERSION}:resource:image:getlistgetpagelist`,`api${state.VIEW_VERSION}:resource:image:uploadimage`,`api${state.VIEW_VERSION}:resource:image:batchsoftdelete`])">
                  <template v-if="checkPermission([`api${state.VIEW_VERSION}:resource:image:getlistgetpagelist`])">
                    <el-form-item label="图片类型">
                      <el-select v-model="state.query.FileCategory" placeholder="图片类型" class="handle-select mr10" @change="driverTypeHandle">
                        <el-option v-for="item in state.optionstypede" :key="item.FileCategory" :label="item.label" :value="item.FileCategory"></el-option>
                      </el-select>
                    </el-form-item>
                    <el-button type="primary" icon="el-icon-search" @click="handleSearch">搜索</el-button>
                  </template>
                  <el-button type="primary" icon="el-icon-plus" @click="handleClickAddData" v-if="checkPermission([`api${state.VIEW_VERSION}:resource:image:uploadimage`])">新增</el-button>
                  <template v-if="checkPermission([`api${state.VIEW_VERSION}:resource:image:batchsoftdelete`])">
                    <el-button
                      type="danger"
                      icon="el-icon-delete"
                      class="handle-del mr10"
                      @click="handleClickBatchDelete()"
                      >批量删除</el-button>
                    <el-checkbox :indeterminate="state.isIndeterminate" v-model="state.stcheckAll" @change="handleCheckAllChange">全选</el-checkbox>
                  </template>
                </el-form>
              <div >
    <el-row  >
  <el-col :xs="12" :sm="8" :md="6" :lg="4" :xl="3" v-for="(item,index) in state.cities" :key="index">
    <dl class="res-pic"> 
        <dt class="res-box">  
          <el-image :src="item.filePath" class="res-img"  :preview-src-list="[item.filePath]" >
        </el-image>
          <div  class="icon"><i class="el-icon-delete-solid " @click="handleChec(item.imgId)"></i></div>
        </dt>
      
        <dd>  <el-checkbox-group v-model="state.checkedCities" @change="handleCheckedCitiesChange">
    <el-checkbox  :label="item">{{item.fileName}} </el-checkbox>
  </el-checkbox-group></dd>
              </dl>
  </el-col>
         </el-row>
              
  </div>
            </div>
            <!-- 分页 begin -->
            <EupPagination
                :current-page="state.pageIndex"
                :pagesizes="[24,48,96,192]"
                :pagesize="state.pageSize"
                :total="state.pageTotal"
                @getPageData="getData"
                @resPageData="resPageData">
            </EupPagination>
            <!-- 分页 end -->
        </div>
        <!-- 内容区域 end -->
                <!-- 添加/编辑窗口 begin -->
        <el-dialog 
          title="添加图片"
          v-model="state.addDialogFormVisible"
          :close-on-click-modal="false"
          width="30%"
          @close="closeDialog">
            <el-form
                ref="refAddForm"
                :model="state.addForm"
                label-width="80px"
                :inline="false"         
              >
              <el-row>
                    <el-form-item label="图片类型" prop="bannerType">
                      <el-select v-model="state.addForm.FileCategory" placeholder="请选择">
                            <el-option
                              v-for="item in state.optionstypedes"
                              :key="item.FileCategory"
                              :label="item.label"
                              :value="item.FileCategory"
                            ></el-option
                          ></el-select>
                    </el-form-item>
                   <el-form-item label="图片" prop="Portrait" >
                        <el-upload action="#"  list-type="picture-card"  :on-change="handleChange" :on-remove="handleRemove"  :on-success="haderOnsuucess" >
                         <img v-if="state.form.portraitUrl" :src="state.form.portraitUrl" style="width:100%;height:100%">
                            <i v-else class="el-icon-plus avatar-uploader-icon"></i>
                        </el-upload>
                    </el-form-item> 
                <!-- </el-col> -->
              </el-row>
            </el-form>
          <template #footer>
            <span class="dialog-footer">
              <el-button @click="closeDialog()">取 消</el-button>
              <el-button type="primary" @click="addDialogFormSave(this)">确 定</el-button>
            </span>
          </template>
        </el-dialog>
        <!-- 添加/编辑窗口 end -->
  </div>
</template>

<script>
import { reactive, onBeforeMount, onMounted } from 'vue'
import EupPagination from "../../../components/EupPagination.vue"
import {GetListGetPageList,ImageSoftDelete,ImageBatchSoftDelete,postimg} from "@/serviceApi/Image/Image"
import { ElMessage,ElMessageBox} from 'element-plus'
import {elConfirmDialog} from "@/common/js/comm"
import EupCrumbs from "../../../components/eup-crumbs/index.vue"
import EnumConfig from "@/enum/EnumConfig"

var CURR_VIEW_VERSION = EnumConfig.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var VIEW_VERSION = CURR_VIEW_VERSION == 'V0' ? '' : `:S:${CURR_VIEW_VERSION}`;

export default {
 name: '',
components: {
        EupPagination: EupPagination,
        EupCrumbs: EupCrumbs
    },

  setup() {

    const state = reactive({
      VIEW_VERSION: VIEW_VERSION,
       pageIndex:1,pageSize:24,pageTotal:0,deposit:[],ptyr:[],stcheckAll:false,
       query: {
                FileCategory: '0',
            },
             optionstypede: [
              {
                FileCategory : '0', label: '全部'
              },
              {
                FileCategory : '100', label: '头像'
              },
              {
                FileCategory : '200', label: '文档'
              },
              {
                FileCategory : '300', label: '证件'
              }
            ],
             optionstypedes: [

              {
                FileCategory : '100', label: '头像'
              },
              {
                FileCategory : '200', label: '文档'
              },
              {
                FileCategory : '300', label: '证件'
              }
            ],
         checkAll: false,
        checkedCities: [],
        cities: [],
        isIndeterminate: false,addDialogFormVisible:false,
        addForm:{
          FileCategory:"",file:"",
        },
        form:{},
    })
    onBeforeMount(() => {
    })
    onMounted(() => {
      getData();
    })
    const driverTypeHandle=()=>{
    }
    const getData=()=>{
        var params={
            "currentPage":state.pageIndex,
          "pageSize": state.pageSize,
          "filter.imgCategory": state.query.FileCategory =='0'? null:state.query.FileCategory,
        }
        
        GetListGetPageList(params).then((res)=>{
                if(res.code==1){
              state.cities=res.data.list
             for (let index = 0; index <state.cities.length; index++) {
               state.ptyr.push(state.cities[index].id)
             }
              state.pageTotal = res.data.total;//初始化列表数据总数
                state.deposit=[];
                 state.stcheckAll=false;
                }else{
                   ElMessage.error(res.msg)
                }
        })
    };
        const resPageData = (obj) =>{
            state.pageIndex = obj.currPage;
            state.pageSize = obj.pageSize;
        }


        const handleSearch=()=>{
          getData();
          state.pageIndex=1
        }
        const handleClickBatchDelete=()=>{
          if(state.deposit.length==0){
            return  ElMessage.error("请选择数据");
          }
          var paerms=[];
        for (let index = 0; index < state.deposit.length; index++) {
          paerms.push(state.deposit[index].imgId)          
        }
         elConfirmDialog(ElMessageBox,'此操作将永久删除该数据, 是否继续?','提示', '', ()=>{
            ImageBatchSoftDelete(paerms).then(res =>{
                if (res.code == 1){
                state.stcheckAll=false;
                state.deposit=[];
                state.checkedCities={};
                 state.isIndeterminate = false; 
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
        const handleClickAddData=()=>{
          state.addDialogFormVisible=true;
        }
         const handleCheckAllChange = (val) => {
         state.checkedCities = val ? state.cities : [];
        state.isIndeterminate = false;

        if(val){
           state.deposit=state.cities
        }
        else{
          state.deposit=[];
        }
      };
        const handleCheckedCitiesChange = (value) => {
         const checkedCount = value.length;
        state.checkAll = checkedCount === state.cities.length;
        state.isIndeterminate = checkedCount > 0 && checkedCount < state.cities.length; 
        state.deposit=value
      };
      const handleChec=(val)=>{
         elConfirmDialog(ElMessageBox,'此操作将永久删除该数据, 是否继续?','提示', '', ()=>{
            ImageSoftDelete(val).then(res =>{
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
      };
         //关闭对话框
        const closeDialog = ()=>{
           state.addDialogFormVisible = false;
           state.addForm.FileCategory=""
           state.form.portraitUrl="";
        };
        //图片 begin
        //图片上传成功回调
        const haderOnsuucess=(response, file, fileList)=> {
            console.log(response, file, fileList);

        }
        const handleRemove=(file, fileList)=> {
            state.hideUpload = fileList.length >= state.limitCount;
            state.form.portraitUrl="";
        }
        const handleChange=(file, fileList)=> {
            state.hideUpload = fileList.length >= state.limitCount;
            state.form.portraitUrl=file.url;
            state.addForm.file=file
           
        }
        //图片 end
         const addDialogFormSave=()=>{
           if(state.addForm.FileCategory==""){
             return ElMessage.error("图片类型不能为空")
           }
           if( state.addForm.file==""){
             return ElMessage.error("图片不能为空")
           }         
          var formData = new FormData();
            formData.append('File', state.addForm.file.raw);
            formData.append('FileCategory', state.addForm.FileCategory);
            postimg(formData).then(function(res) {
              if(res.code==1){
                ElMessage.success("添加成功")
                 getData();
                  state.addForm.file.raw="";
                 state.addDialogFormVisible=false;
                }
                else{
                  ElMessage.error(res.msg)
                }
            }); 
         }
    return {
     state,driverTypeHandle,resPageData,getData,handleSearch,handleClickBatchDelete,handleClickAddData,handleCheckAllChange,
     handleCheckedCitiesChange,handleChec,closeDialog,haderOnsuucess,handleChange,handleRemove,addDialogFormSave
    }
  },
}

</script>
<style scoped >
</style>