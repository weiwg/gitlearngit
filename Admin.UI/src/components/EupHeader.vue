<!--
 * @Description: 
 * @Author: weig
 * @Date: 2021-05-24 10:24:20
 * @LastEditors: weig
 * @LastEditTime: 2021-12-29 12:27:26
-->
<template>
    <div>
        <div class="header">
            <!-- 折叠按钮 -->
            <div class="collapse-btn" @click="collapseChage">
                <i v-if="!collapse" class="el-icon-s-fold"></i>
                <i v-else class="el-icon-s-unfold"></i>
            </div>
            <div class="logo">后台管理系统</div>
            <div class="header-right">
                <div class="header-user-con">
                    <div class="user-avator">
                        <img :src="(userInfo.portraitUrl ? userInfo.portraitUrl: defaultPortraitUrl)" />
                    </div>
                    <!-- 用户名下拉菜单 -->
                    <el-dropdown class="user-name" trigger="click" @command="handleCommand">
                        <span class="el-dropdown-link">
                            {{userInfo.nickName}}
                            <i class="el-icon-caret-bottom"></i>
                        </span>
                        <template #dropdown>
                            <el-dropdown-menu>
                                <el-dropdown-item divided command="updatepassword">修改密码</el-dropdown-item>
                                <el-dropdown-item divided command="loginout">退出登录</el-dropdown-item>
                                <el-dropdown-item divided command="register" v-if="checkPermission([`api${VIEW_VERSION}:user:account:register`])">注册账号</el-dropdown-item>
                            </el-dropdown-menu>
                        </template>
                    </el-dropdown>
                </div>
            </div>
        </div>
        <el-dialog title="修改密码" v-model="editPassWordVisible" width="30%" @close="closeUpdatePWDialog()">
            <el-form ref="PWEidtInfo" :model="updatePassWord" label-width="80px" :rules="rulesForm">
                <el-form-item label="账号" prop="userName">
                    <el-input placeholder="账号" v-model="updatePassWord.userName" disabled></el-input>
                </el-form-item>
                <el-form-item label="旧密码" prop="oldPassword">
                    <el-input placeholder="请输入旧密码" v-model="updatePassWord.oldPassword" show-password></el-input>
                </el-form-item>
                <el-form-item label="新密码" prop="newPassword">
                    <el-input placeholder="请输入新密码" v-model="updatePassWord.newPassword" show-password></el-input>
                </el-form-item>
                <el-form-item label="确认密码" prop="confirmPassword">
                    <el-input placeholder="请输入密码" v-model="updatePassWord.confirmPassword" show-password></el-input>
                </el-form-item>
            </el-form>
            <template #footer>
                <span class="dialog-footer">
                    <el-button @click="closeUpdatePWDialog">取 消</el-button>
                    <el-button type="primary" @click="saveEdit">确 定</el-button>
                </span>
            </template>
        </el-dialog>
        <el-dialog title="注册账号" v-model="registerVisible" width="30%" @close="closeRegisterDialog()">
            <el-form ref="registerInfo" :model="registerInfo" label-width="80px" :rules="rulesRegisterForm">
                <el-form-item label="账号" prop="userName">
                    <el-input placeholder="请输入账号" v-model="registerInfo.userName"></el-input>
                </el-form-item>
                <el-form-item label="昵称" prop="nickName">
                    <el-input placeholder="请输入昵称" v-model="registerInfo.nickName"></el-input>
                </el-form-item>
                <el-form-item label="密码" prop="newPassword">
                    <el-input placeholder="请输入密码" v-model="registerInfo.newPassword" show-password></el-input>
                </el-form-item>
                <el-form-item label="确认密码" prop="confirmPassword">
                    <el-input placeholder="请输入密码" v-model="registerInfo.confirmPassword" show-password></el-input>
                </el-form-item>
                <el-form-item label="邮箱">
                    <el-input v-model="registerInfo.email" style="margin-right: 8px; width: 70%;"></el-input>
                </el-form-item>
                <el-form-item label="电话">
                    <el-input
                    v-model.trim="registerInfo.phone"
                    oninput="value=value.replace(/^\.+|[^\d.]/g,'')"
                    style="margin-right: 8px; width: 70%;"
                    ></el-input>
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
                        v-if="registerInfo.portraitUrl"
                        :src="registerInfo.portraitUrl"
                        style="width:100%;height:100%"
                    />
                    <i v-else class="el-icon-plus avatar-uploader-icon"></i>
                    </el-upload>
                </el-form-item>
            </el-form>
            <template #footer>
                <span class="dialog-footer">
                    <el-button @click="closeRegisterDialog">取 消</el-button>
                    <el-button type="primary" @click="saveRegister">确 定</el-button>
                </span>
            </template>
        </el-dialog>
    </div>
</template>
<script>
import { logout,UpdatePassword,Register} from "@/serviceApi/auth";
import { ElMessage } from 'element-plus'
import { delCookie,IsNullOrEmpty } from "@/common/js/comm";
import { passwordMd5 } from "@/utils/password.js";
import defaultPortraitUrl from "../assets/img/img.jpg";
import Enum from "@/enum/EnumConfig"
import { postimg } from "@/serviceApi/Image/Image";
import { VueCropper }  from "vue-cropperjs"

var CURR_VIEW_VERSION = Enum.EnumConfig.API_VIEW_VERSION.CURR_API_VIEW_VERSION;
var VIEW_VERSION = CURR_VIEW_VERSION == 'V0' ? '' : `:S:${CURR_VIEW_VERSION}`;
export default {
    data() {
        var that = this;
        //校验新旧密码是否一致
        var validateNewPassword = (rule, value, callback)=>{
            if (that.updatePassWord.oldPassword === value){
                return callback(new Error("新密码与旧密码一致！"));
            }
        };
        //校验新密码与确认密码是否一致
        var validateConfirmPassword = (rule, value, callback)=>{
            if (that.updatePassWord.newPassword != value){
                return callback(new Error("俩次输入密码不一致！"));
            }
        }
        //校验新密码与确认密码是否一致
        var validateConfirmPassword2 = (rule, value, callback)=>{
            if (that.registerInfo.newPassword != value){
                return callback(new Error("俩次输入密码不一致！"));
            }
        }
        return {
            defaultPortraitUrl: defaultPortraitUrl,
            fullscreen: false,
            // name: "linxin",
            message: 2,
            userInfo:{
                email: "",
                nickName: "",
                openId: "",
                phone: "",
                portrait: "",
                portraitUrl:"",
                userName:"",
                userStatus: 0
            },
            updatePassWord:{
                userName: "",
                oldPassword: "",
                newPassword: "",
                confirmPassword: "",

            },
            editPassWordVisible: false,
            rulesForm:{
                oldPassword: [
                    {required: true,message: "请输入旧密码！",trigger: "blur"},
                    {min: 6, max: 16, message: '密码长度6-16字符', trigger: 'blur'}
                ],
                newPassword:[
                    {required: true,message: "请输入新密码！",trigger: "blur"},
                    {min: 6, max: 16, message: '密码长度6-16字符！', trigger: 'blur'},
                    { validator: validateNewPassword, trigger: 'blur' }
                ],
                confirmPassword:[
                    {required: true,message: "请再次输入密码！",trigger: "blur"},
                    {min: 6, max: 16, message: '密码长度6-16字符！', trigger: 'blur'},
                    { validator: validateConfirmPassword, trigger: 'blur' }
                ],
            }, 
            rulesRegisterForm:{
                userName: [
                    {required: true,message: "请输入账号！",trigger: "blur"},
                    {min: 6, max: 16, message: '账号长度6-16字符', trigger: 'blur'}
                ],
                nickName: [
                    {required: true,message: "请输入昵称！",trigger: "blur"},
                    {min: 6, max: 16, message: '昵称长度6-16字符', trigger: 'blur'}
                ],
                newPassword:[
                    {required: true,message: "请输入密码！",trigger: "blur"},
                    {min: 6, max: 32, message: '密码长度6-32字符！', trigger: 'blur'},
                ],
                confirmPassword:[
                    {required: true,message: "请输入确认密码！",trigger: "blur"},
                    {min: 6, max: 32, message: '密码长度6-32字符！', trigger: 'blur'},
                    { validator: validateConfirmPassword2, trigger: 'blur' }
                ],
                email:[
                    {required: true,message: "请输入邮箱！",trigger: "blur"},
                    {min: 6, max: 32, message: '邮箱长度6-32字符！', trigger: 'blur'}
                ],
            },    
            VIEW_VERSION: VIEW_VERSION,
            registerVisible: false,
            registerInfo:{
                userName:"",
                newPassword:"",
                confirmPassword:"",
                portrait:"",
                email: "",
                phone: "",
                portraitUrl:""
            },
            frequency: 0,
            hideUpload: false,
            limitCount: 1,
        };
    },
    computed: {
        collapse() {
            return this.$store.getters.collapse;
        }
    },
    methods: {
        // 用户名下拉菜单选择事件
        handleCommand(command) {
            if (command == "loginout") {//退出
                // var url = window.location.protocol + "//" + window.location.hostname + ":" + window.location.port
                logout().then((res) => {
                    sessionStorage.setItem("token", "");
                    delCookie("ASP.NET_SessionId");
                    delCookie("SessionId");
                    sessionStorage.setItem("tagsList", "");
                    this.$router.push({name: "Login"});
                    // window.location.href = res.data;
                });
            } else if (command == "updatepassword"){//修改密码
                this.editPassWordVisible = true;
            } else if (command == "register"){//注册账号
                this.registerVisible = true;
            }
        },
        // 侧边栏折叠
        collapseChage() {
            this.$store.commit("tags/hadndleCollapse", !this.collapse);
        },
        //获取当前用户信息
        getCurrUserInfo_func(){
            let token = sessionStorage.getItem("token");
            //此方法会跑俩次，因为该页面会加载俩次，第一次未通过路由守卫，第二次通过
            //第一次进入此方法时，token为空，（此时未经过路由守卫，token可能为空或者过期等）报错，报错后再登录，客户体验不好。（处理办法，不提示错误，等通过路由守卫后重新获取token）
            //通过路由守卫那里重新获取token再次登录，再跑第二次这里，此时的token已经重新获取了
            if (token){
                    this.userInfo.email=this.$store.getters.userInfo.email;
                    this.userInfo.nickName=this.$store.getters.userInfo.nickName;
                    this.userInfo.openId=this.$store.getters.userInfo.openId;
                    this.userInfo.phone=this.$store.getters.userInfo.phone;
                    this.userInfo.portrait=this.$store.getters.userInfo.portrait;
                    this.userInfo.portraitUrl=this.$store.getters.userInfo.portraitUrl;
                    this.userInfo.userName=this.$store.getters.userInfo.userName;
                    this.userInfo.userStatus=this.$store.getters.userInfo.userStatus;
                    this.updatePassWord.userName = this.$store.getters.userInfo.userName;
                    //异步操作
                    // this.$store.dispatch("user/getPermissionPointMenuInfo");
            }
        },
        //关闭修改密码对话框
        closeUpdatePWDialog(){
            this.editPassWordVisible = false;
            this.$refs["PWEidtInfo"].resetFields();//重新初始化表单
        },
        //保存修改密码
        saveEdit(){
            if (this.updatePassWord.oldPassword == "" || this.updatePassWord.oldPassword == undefined || this.updatePassWord.oldPassword == null){
                ElMessage.error("请输入旧密码！");
                return;
            }
            if (this.updatePassWord.newPassword == "" || this.updatePassWord.newPassword == undefined || this.updatePassWord.newPassword == null){
                ElMessage.error("请输入新密码！");
                return;
            }
            if (this.updatePassWord.confirmPassword == "" || this.updatePassWord.confirmPassword == undefined || this.updatePassWord.confirmPassword == null){
                ElMessage.error("请再次输入密码！");
                return;
            }
            if (this.updatePassWord.oldPassword == this.updatePassWord.newPassword){
                ElMessage.error("新密码与旧密码一致！");
                return;
            }
            if (this.updatePassWord.confirmPassword != this.updatePassWord.newPassword){
                ElMessage.error("俩次输入密码不一致！");
                return;
            }
            let params = {
                "oldPassword": passwordMd5(this.updatePassWord.oldPassword),
                "newPassword":passwordMd5( this.updatePassWord.newPassword),
                "version": 0
            };          
            UpdatePassword(params).then((res)=>{
                if (res.msg == '修改成功' && res.msgCode == 'OK'){
                    ElMessage.success("密码修改成功！");  
                    this.editPassWordVisible = false;       
                }
            });
        },
        //关闭注册账号对话框
        closeRegisterDialog(){
            this.registerVisible = false;
            this.$refs["registerInfo"].resetFields();//重新初始化表单
        },
        //保存注册账号信息
        saveRegister(){
            if (IsNullOrEmpty(this.registerInfo.userName)){
                ElMessage.error("账号不能为空！");
                return;
            }
            if (IsNullOrEmpty(this.registerInfo.newPassword)){
                ElMessage.error("请输入密码！");
                return;
            }
            if (IsNullOrEmpty(this.registerInfo.confirmPassword)){
                ElMessage.error("请再次确认密码！");
                return;
            }
            if (this.registerInfo.newPassword != this.registerInfo.confirmPassword){
                ElMessage.error("俩次输入密码不一致！");
                return;
            }
            if (IsNullOrEmpty(this.registerInfo.email)) {
                return ElMessage.error("请输入邮箱号");
            }
            if (
                !/^([a-zA-Z0-9]+[-_.]?)+@[a-zA-Z0-9]+.[a-z]+$/.test(
                this.registerInfo.email
                )
            ) {
                return ElMessage.error("请输入正确邮箱格式");
            }
            let params = {
                "password":passwordMd5( this.registerInfo.newPassword),
                "version": 0,
                "userName": this.registerInfo.userName,
                "nickName": this.registerInfo.nickName,
                "portrait": this.registerInfo.portrait,
                "email": this.registerInfo.email,
                "phone": this.registerInfo.phone
            };       
               
            Register(params).then((res)=>{
                if (res.code == 1){
                    ElMessage.success(res.msg);  
                    this.registerVisible = false;       
                }
            });
        },
        haderOnsuucess(response, file, fileList){
            console.log(response, file, fileList);
        },
        handleRemove(file, fileList){
            this.hideUpload = fileList.length >= this.limitCount;
            this.registerInfo.portraitUrl = "";
        },
        handleChange(file, fileList){
            this.hideUpload = fileList.length >= this.limitCount;
            this.frequency += 1;
            if (this.frequency % 2 === 0) {
                var formData = new FormData();
                formData.append("File", file.raw);
                formData.append("FileCategory", "Avatar");
                var that = this;
                postimg(formData).then(function(res) {
                    if (1 == res.code) {
                        that.registerInfo.portrait = res.data.eurl;
                        that.registerInfo.portraitUrl = res.data.url;
                    } else {
                        ElMessage.error(res.msg);
                    }
                });
            }
        }
    },
    mounted() {
        if (document.body.clientWidth < 1300) {
            this.collapseChage();
        }
        this.getCurrUserInfo_func();
    },    
    components: {
        VueCropper
    },
};
</script>
<style scoped>
</style>
