<template>
  <div>
    <!-- 面包屑 begin -->
    <eup-crumbs
      icon="el-icon-s-custom"
      firstCrumbs="用户管理"
      secondCrumbs="用户列表"
    />
    <!-- 面包屑 end -->

    <!-- 内容区域 begin-->
    <div class="container">
      <div class="handle-box">
        <el-form
          ref="state.query"
          :model="state.query"
          :inline="true"
          v-if="checkPermission([`api${state.VIEW_VERSION}:User:UserInfo:GetPage`])"
        >
          <!-- <el-form-item>
                        <eup-search :fields="state.fields" @click="onSearch" />
                    </el-form-item> -->
          <el-form-item label="用户名">
            <el-input
              v-model="state.query.userName"
              placeholder="用户名"
              class="handle-input mr10"
            ></el-input>
          </el-form-item>
          <el-form-item label="昵称">
            <el-input
              v-model="state.query.nickName"
              placeholder="昵称"
              class="handle-input mr10"
            ></el-input>
          </el-form-item>
          <el-form-item label="手机">
            <el-input
              v-model="state.query.phone"
              placeholder="手机"
              class="handle-input mr10"
            ></el-input>
          </el-form-item>
          <el-form-item label="邮箱">
            <el-input
              v-model="state.query.email"
              placeholder="邮箱"
              class="handle-input mr10"
            ></el-input>
          </el-form-item>
          <el-button type="primary" icon="el-icon-search" @click="handleSearch"
            >搜索</el-button
          >
        </el-form>
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
        <el-table-column
          type="selection"
          width="60"
          align="center"
        ></el-table-column>
        <el-table-column
          prop="num"
          label="序号"
          width="60"
          align="center"
        ></el-table-column>
        <el-table-column
          prop="nickName"
          label="昵称"
          min-width="140"
          align="center"
        ></el-table-column>
        <el-table-column
          prop="userName"
          label="用户名"
          min-width="100"
          align="center"
        ></el-table-column>
        <el-table-column
          prop="email"
          label="邮箱"
          min-width="130"
          align="center"
        ></el-table-column>
        <el-table-column
          prop="phone"
          label="电话"
          min-width="110"
          align="center"
        ></el-table-column>
        <el-table-column
          prop="lastLoginIp"
          label="上次登陆IP地址"
          min-width="140"
          align="center"
        ></el-table-column>
        <el-table-column
          prop="lastLoginDate"
          label="上次登陆日期"
          min-width="160"
          align="center"
        ></el-table-column>
        <el-table-column
          prop="createDate"
          label="创建时间"
          min-width="160"
          align="center"
        ></el-table-column>
        <el-table-column
          label="操作"
          min-width="180"
          align="center"
          fixed="right"
          v-if="
            checkPermission([
              `api${state.VIEW_VERSION}:User:UserInfo:Update`,
              `api${state.VIEW_VERSION}:User:UserInfo:Get`,
            ])
          "
        >
          <template #default="scope">
            <el-button
              v-if="checkPermission([`api${state.VIEW_VERSION}:User:UserInfo:Update`])"
              type="text"
              icon="el-icon-edit"
              @click="handleEdit(scope.$index, scope.row)"
              >编辑</el-button
            >
            <el-button
              v-if="checkPermission([`api${state.VIEW_VERSION}:User:UserInfo:Get`])"
              type="text"
              icon="el-icon-check"
              @click="handleClickDetails(scope.row)"
              >查看详情</el-button
            >
          </template>
        </el-table-column>
      </el-table>
      <!-- 分页 begin -->
      <EupPagination
        :current-page="state.pageIndex"
        :pagesizes="[10, 20, 50, 100]"
        :pagesize="state.pageSize"
        :total="state.pageTotal"
        @getPageData="getData"
        @resPageData="resPageData"
      >
      </EupPagination>
      <!-- 分页 end -->
    </div>
    <!-- 内容区域 end-->

    <!-- 编辑弹出框 -->
    <div>
      <el-dialog
        title="编辑"
        v-model="state.editVisible"
        :close-on-click-modal="false"
        width="35%"
        destroy-on-close
      >
        <el-form ref="form" :model="state.form" label-width="70px">
          <el-form-item label="昵称">
            <el-input v-model="state.form.nickName"></el-input>
          </el-form-item>
          <el-form-item label="用户名">
            <el-input v-model="state.form.userName" :disabled="true"></el-input>
          </el-form-item>
           <el-form-item label="登录密码">
            <el-button type="success" @click="handleClickResetPassword"
              >重置登录密码</el-button
            >
          </el-form-item>
          <el-form-item label="支付密码">
            <el-button type="success" @click="handleClickPayPassword"
              >重置支付密码</el-button
            >
          </el-form-item>
          <el-form-item label="邮箱">
            <el-input v-model="state.form.email" :disabled="true" style="margin-right: 8px; width: 70%;"></el-input>
              <el-button type="success" @click="handleClickResetemail">重置邮箱</el-button>
          </el-form-item>
          <el-form-item label="电话">
            <el-input
              v-model.trim="state.form.phone"
              oninput="value=value.replace(/^\.+|[^\d.]/g,'')"
              :disabled="true"
              style="margin-right: 8px; width: 70%;"
            ></el-input>
            <el-button type="success" @click="handleClickResetphone">重置电话</el-button>
          </el-form-item>
          <el-form-item label="头像" prop="Portrait">
            <el-upload
              action="#"
              :on-success="haderOnsuucess"
              list-type="picture-card"
              :on-change="handleChange"
              :on-remove="handleRemove"
            >
              <img
                v-if="state.form.portraitUrl"
                :src="state.form.portraitUrl"
                style="width:100%;height:100%"
              />
              <i v-else class="el-icon-plus avatar-uploader-icon"></i>
            </el-upload>
          </el-form-item>
         
        </el-form>
        <template #footer>
          <span class="dialog-footer">
            <el-button @click="state.editVisible = false">取 消</el-button>
            <el-button type="primary" @click="saveEdit">确 定</el-button>
          </span>
        </template>
      </el-dialog>
    </div>

    <!-- 编辑密码弹出框 -->
    <el-dialog
      title="重置登录密码"
      v-model="state.editInnerVisible"
      width="30%"
      :close-on-click-modal="false"
    >
      <el-form
        ref="state.PWEidtInfo"
        :model="state.PWEidtInfo"
        label-width="70px"
      >
        <el-form-item label="新密码">
          <el-input
            placeholder="请输入密码"
            v-model="state.PWEidtInfo.newPassword"
             maxlength="16"
            show-password
          ></el-input>
        </el-form-item>
        <el-form-item label="确认密码">
          <el-input
            placeholder="请输入密码"
            v-model="state.PWEidtInfo.confirmPassword"
             maxlength="16"
            show-password
          ></el-input>
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button type="primary" @click="handleClickSavePassword"
            >确 定</el-button
          >
        </span>
      </template>
    </el-dialog>
    <!-- 编辑支付密码弹出框 -->
    <el-dialog
      title="重置支付密码"
      v-model="state.editPayPassword"
      width="30%"
      :close-on-click-modal="false"
    >
      <el-form
        ref="state.PWEidtInfo"
        :model="state.PWEiPayPassword"
        label-width="70px"
      >
        <el-form-item label="新密码">
          <el-input
            placeholder="请输入密码"
            v-model="state.PWEiPayPassword.newPassword"
            maxlength="6"
            show-password
            oninput="value=value.replace(/[^0-9.]/g,'')"
          ></el-input>
        </el-form-item>
        <el-form-item label="确认密码">
          <el-input
            placeholder="请输入密码"
            v-model="state.PWEiPayPassword.confirmPassword"
            maxlength="6"
            show-password
            oninput="value=value.replace(/[^0-9.]/g,'')"
          ></el-input>
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button type="primary" @click="handleClickSavePayPassword"
            >确 定</el-button
          >
        </span>
      </template>
    </el-dialog>
    <!-- 编辑手机弹出框 -->
    <el-dialog
      title="编辑"
      v-model="state.editPhome"
      width="30%"
      :close-on-click-modal="false"
    >
      <el-form
        ref="state.PWEidtPhome"
        :model="state.PWEidtPhome"
        label-width="70px"
      >
        <el-form-item label="手机号">
          <el-input
            placeholder="请输入手机号"
             maxlength="11"
            v-model="state.PWEidtPhome.Phome"
          ></el-input>
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button type="primary" @click="handleClickSavePhome"
            >确 定</el-button
          >
        </span>
      </template>
    </el-dialog>
    <!-- 编辑邮箱弹出框 -->
    <el-dialog
      title="编辑"
      v-model="state.editEmail"
      width="30%"
      :close-on-click-modal="false"
    >
      <el-form
        ref="state.PWEidtEmail"
        :model="state.PWEidtEmail"
        label-width="70px"
      >
        <el-form-item label="邮箱号">
          <el-input
            placeholder="请输入邮箱"
            v-model="state.PWEidtEmail.Email"
          ></el-input>
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button type="primary" @click="handleClickSaveEmail"
            >确 定</el-button
          >
        </span>
      </template>
    </el-dialog>
    <!-- 详情弹出框 -->
    <el-dialog
      title="详情"
      v-model="state.detailsVisible"
      width="30%"
      :close-on-click-modal="false"
    >
      <el-form ref="form" :model="state.form" label-width="70px">
        <el-form-item label="昵称">
          <el-input
            v-model="state.form.nickName"
            autocomplete="off"
            :disabled="true"
          ></el-input>
        </el-form-item>
        <el-form-item label="用户名">
          <el-input
            v-model="state.form.userName"
            autocomplete="off"
            :disabled="true"
          ></el-input>
        </el-form-item>
        <el-form-item label="邮箱">
          <el-input
            v-model="state.form.email"
            autocomplete="off"
            :disabled="true"
          ></el-input>
        </el-form-item>
        <el-form-item label="电话">
          <el-input
            v-model="state.form.phone"
            autocomplete="off"
            :disabled="true"
          ></el-input>
        </el-form-item>
        <el-form-item label="头像">
          <el-image
            style="width: 100px; height: 100px"
            :src="state.form.portraitUrl"
            :preview-src-list="[state.form.portraitUrl]"
          ></el-image>
        </el-form-item>
      </el-form>
    </el-dialog>
  </div>
</template>

<script>
import { reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import { VueCropper }  from "vue-cropperjs"
import {
  getUserList,
  updateUserInfo,
  resetPassword,
  ResetPhone,
  ResetEmail,
  resetSysPayPassword,
} from "@/serviceApi/user/userList";
import { postimg } from "@/serviceApi/Image/Image";
import { passwordMd5 } from "@/utils/password.js";
import EupPagination from "../../../components/EupPagination.vue";
import EupCrumbs from "../../../components/eup-crumbs/index.vue";
import EnumConfig from '../../../enum/EnumConfig'

var CURR_VIEW_VERSION = EnumConfig.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var VIEW_VERSION = CURR_VIEW_VERSION == 'V0' ? '' : `:S:${CURR_VIEW_VERSION}`;
export default {
  name: "UserList",
  components: {
    EupPagination: EupPagination,
    EupCrumbs: EupCrumbs,
    VueCropper: VueCropper
  },
  setup() {
    const state = reactive({
      query: {
        userName: "",
        nickName: "",
        phone: "",
        email: "",
      },
      VIEW_VERSION: VIEW_VERSION,
      pageIndex: 1,
      pageSize: 10,
      tableData: [],
      multipleSelection: [],
      delList: [],
      editVisible: false,
      pageTotal: 0,
      form: {},
      idx: -1,
      frequency: 0,
      PWEidtEmail: {
        Email: "",
        userId: "",
      },
      PWEidtPhome: {
        Phome: "",
        userId: "",
      },
      PWEiPayPassword: {
        newPassword: "",
        confirmPassword: "",
        userId: "",
      },
      editPhome: false,
      editEmail: false,
      loading: false,
      limitCount: 1,
      editInnerVisible: false,
      PWEidtInfo: {
        newPassword: "",
        confirmPassword: "",
        userId: "",
      },
      detailsVisible: false,
      dynamicFilter: {},
      hideUpload: false,
      editPayPassword: false,
    });
    onMounted(() => {
      getData();
    });
    //获取表单信息
    const getData = () => {
      var params = {
        currentPage: state.pageIndex,
        pageSize: state.pageSize,
        "Filter.UserId":state.query.userId,
        "Filter.UserName":state.query.userName,
        "Filter.NickName":state.query.nickName,
        "Filter.Phone":state.query.phone,
        "Filter.Email":state.query.email,
        dynamicFilter: state.dynamicFilter,
      };
      state.loading = true;
      getUserList(params).then(function(res) {
        if (res.code == 1) {
          state.pageTotal = res.data.total; //初始化列表数据总数
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
    };
    // 多选操作
    const handleSelectionChange = (val) => {
      state.multipleSelection = val;
    };
    // 触发搜索按钮
    const handleSearch = () => {
      state.pageIndex = 1;
      getData();
    };
    // 编辑操作
    const handleEdit = (index, row) => {
      state.form = {};
      state.idx = index;
      var strRow = JSON.stringify(row);
      state.form = JSON.parse(strRow);
      state.editVisible = true;
      state.hideUpload = false;
      if (
        row.portraitUrl == undefined ||
        row.portraitUrl == null ||
        row.portraitUrl == ""
      ) {
        //同一条记录，图片删除后恢复旧的图片地址，而不是空的，因为该记录未上传到后台
        state.form.portraitUrl = "";
      } else {
        state.form.portraitUrl = row.portraitUrl;
      }
    };
    // 保存编辑
    const saveEdit = () => {
      state.editVisible = false;
      if (state.form.nickName == "") {
        ElMessage.error("昵称不能为空");
        return;
      }

      var para = JSON.stringify(state.form);
      para = JSON.parse(para);
      var params = {
        userId: para.userId,
        openId: para.openId,
        nickName: para.nickName,
        portrait: para.portrait,
        version: 0,
      };
      updateUserInfo(params).then((res) => {
        if (res.code == 1) {
          ElMessage.success(`修改第 ${state.idx + 1} 行成功`);
          getData();
        } else {
          ElMessage.error(res.msg);
        }
      });
    };

    //图片 begin
    //图片上传成功回调
    function haderOnsuucess(response, file, fileList) {
      console.log(response, file, fileList);
    }
    function handleRemove(file, fileList) {
      state.hideUpload = fileList.length >= state.limitCount;
      state.form.portraitUrl = "";
    }
    function handleChange(file, fileList) {
      state.hideUpload = fileList.length >= state.limitCount;
      state.frequency += 1;
      if (state.frequency % 2 === 0) {
          var formData = new FormData();
        formData.append("File", file.raw);
        formData.append("FileCategory", "Avatar");
        postimg(formData).then(function(res) {
          if (1 == res.code) {
            state.form.portrait = res.data.eurl;
            state.form.portraitUrl = res.data.url;
          } else {
            ElMessage.error(res.msg);
          }
        });
      }
    }
    //图片 end

    //点击重置密码
    const handleClickResetPassword = () => {
      state.editInnerVisible = true;
      //每次打开对话窗时都重新初始化
      state.PWEidtInfo.newPassword = "";
      state.PWEidtInfo.confirmPassword = "";
      state.PWEidtInfo.userId = state.form.userId;
    };
    //保存密码
    const handleClickSavePassword = () => {
      state.editInnerVisible = false;
      if (state.PWEidtInfo.newPassword == "") {
        ElMessage.error("新密码不能为空");
        return;
      }
      if (state.PWEidtInfo.confirmPassword == "") {
        ElMessage.error("确认密码不能为空");
        return;
      }
      if (state.PWEidtInfo.newPassword != state.PWEidtInfo.confirmPassword) {
        ElMessage.error("新密码与确认密码不一致");
        return;
      }
      var para = JSON.stringify(state.PWEidtInfo);
      para = JSON.parse(para);
      var params = {
        userId: para.userId,
        newPassword:passwordMd5(para.newPassword),
        version: 0,
      };
      resetPassword(params)
        .then((res) => {
          if (res.code == 1) {
            ElMessage.success("密码重置成功");
          } else {
            ElMessage.error(res.msg);
          }
        })
        .catch((err) => {
          ElMessage.error("重置密码失败," + err);
        });
    };
    //查看
    const handleClickDetails = (row) => {
      state.detailsVisible = true;
      state.form = row;
    };

    /**
     * @description 子组件返回分页数据
     * @author weig
     * @param {Object} obj
     */
    const resPageData = (obj) => {
      state.pageIndex = obj.currPage;
      state.pageSize = obj.pageSize;
    };
    const handleClickSaveEmail = () => {
      if (state.PWEidtEmail.Email == "") {
        return ElMessage.error("请输入邮箱号");
      }
      if (
        !/^([a-zA-Z0-9]+[-_.]?)+@[a-zA-Z0-9]+.[a-z]+$/.test(
          state.PWEidtEmail.Email
        )
      ) {
        return ElMessage.error("请输入正确邮箱格式");
      }
      state.PWEidtEmail.userId = state.form.userId;
      var parems = {
        userId: state.PWEidtEmail.userId,
        email: state.PWEidtEmail.Email,
        newAccount: "",
      };
      ResetEmail(parems).then((res) => {
        if (res.code == 1) {
          state.editEmail = false;
          ElMessage.success("邮箱重置成功");
          getData();
        } else {
          ElMessage.error(res.msg);
        }
      });
    };
    const handleClickSavePhome = () => {
      if (state.PWEidtPhome.Phome == "") {
        return ElMessage.error("请输入手机号");
      }
      if (!/^1(3|4|5|6|7|8|9)\d{9}$/.test(state.PWEidtPhome.Phome)) {
        return ElMessage.error("请输入正确手机格式");
      }
      state.PWEidtPhome.userId = state.form.userId;
      var parems = {
        userId: state.PWEidtPhome.userId,
        phone: state.PWEidtPhome.Phome,
      };
      ResetPhone(parems).then((res) => {
        if (res.code == 1) {
          state.editPhome = false;
          ElMessage.success("手机号重置成功");
          getData();
        } else {
          ElMessage.error(res.msg);
        }
      });
    };
    const handleClickResetemail = () => {
      state.editEmail = true;
      state.PWEidtEmail.Email = "";
    };
    const handleClickResetphone = () => {
      state.editPhome = true;
      state.PWEidtPhome.Phome = "";
    };
    const handleClickPayPassword = () => {
      state.editPayPassword = true;
      //每次打开对话窗时都重新初始化
      state.PWEiPayPassword.newPassword = "";
      state.PWEiPayPassword.confirmPassword = "";
      state.PWEiPayPassword.userId = state.form.userId;
    };
    const handleClickSavePayPassword = () => {
      
      if (state.PWEiPayPassword.newPassword != state.PWEiPayPassword.confirmPassword) {
        return ElMessage.error("两次密码输入不一致");
      }
      if (/^(\d)\1{2}(\d)\2{2}$|^(\d)\1{5}$|^(?!(^\d{6}$))/.test(state.PWEiPayPassword.newPassword)) {
        return ElMessage.error("新密码格式错误,请输入6位不连续的数字");
      }
      let str = '0123456789_9876543210';
      if(str.indexOf(state.PWEiPayPassword.confirmPassword) > -1){
         return ElMessage.error("密码不能设置太简单");
      }
      var params = {
        userId: state.PWEiPayPassword.userId,
        newPassword: passwordMd5(state.PWEiPayPassword.confirmPassword),
      };
  resetSysPayPassword(params).then((res) => {
          if(res.code==1){
            state.editPayPassword = false;
          ElMessage.success(res.msg);
          getData();
          }
          else{
            ElMessage.error(res.msg);
          }
      });
    };
    return {
      state,
      handleSelectionChange,
      handleClickPayPassword,
      handleClickSavePayPassword,
      handleClickSaveEmail,
      handleClickResetphone,
      handleClickResetemail,
      handleClickSavePhome,
      handleSearch,
      handleEdit,
      haderOnsuucess,
      handleRemove,
      handleChange,
      saveEdit,
      handleClickResetPassword,
      handleClickSavePassword,
      handleClickDetails,
      getData,
      resPageData,
      // onSearch
    };
  },
};
</script>
<style>
</style>
