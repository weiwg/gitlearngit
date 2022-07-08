export function removeToken(tokenKey) {
    return window.localStorage.removeItem(tokenKey)
}

export const getLocal = (name) => {
    return localStorage.getItem(name)
}

export const setLocal = (name, value) => {
    localStorage.setItem(name, value)
}

export function GetString(str) {
    if (IsNullOrEmpty(str)) {
        return "";
    }
    return str.toString();
}

//删除指定url的参数
export function ridUrlParam(url, aParam) {
    aParam.forEach(item => {
        const fromindex = url.indexOf(`${item}=`) //必须加=号，避免参数值中包含item字符串
        if (fromindex !== -1) {
            // 通过url特殊符号，计算出=号后面的的字符数，用于生成replace正则
            const startIndex = url.indexOf('=', fromindex)
            const endIndex = url.indexOf('&', fromindex)
            const hashIndex = url.indexOf('#', fromindex)

            let reg;
            if (endIndex !== -1) { // 后面还有search参数的情况
                const num = endIndex - startIndex
                reg = new RegExp(`${item}=.{${num}}`)
                url = url.replace(reg, '')
            } else if (hashIndex !== -1) { // 有hash参数的情况
                const num = hashIndex - startIndex - 1
                reg = new RegExp(`&?${item}=.{${num}}`)
                url = url.replace(reg, '')
            } else { // search参数在最后或只有一个参数的情况
                reg = new RegExp(`&?${item}=.+`)
                url = url.replace(reg, '')
            }
        }
    });
    const noSearchParam = url.indexOf('=')
    if (noSearchParam === -1) {
        url = url.replace(/\?/, '') // 如果已经没有参数，删除？号
    }
    return url
}
/*
 * 判断字符是否为空或null
 */
export function IsNullOrEmpty(str) {
    if (typeof str == "undefined" || str == null || str === "" || str.toString() === "NaN") {
        return true;
    }
    return false;
}

//获取url指定参数
export function GetQueryValue(queryName) {
    var query = decodeURI(window.location.search.substring(1));
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == queryName) {
            return pair[1];
        }
    }
    return null;
}


/*校验邮箱格式 */
export function isEmailCode(str) {
    var reg = new RegExp("^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){1,63}[a-z0-9]+$");
    return reg.test(str);
}

/*校验邮编*/
export function isValidZipCode(str) {
    if (IsNullOrEmpty(str)) {
        return false;
    }

    var reg = /^[1-9][0-9]{5}$/;
    return reg.test(str);
}

/*校验手机格式 */
export function isValidMobile(str) {
    var reg = /^[1](3|4|5|7|8)\d{9}$|^[1](66|98|99)\d{8}$/;
    return reg.test(str);
}

/*电话有效性（固话和手机）*/
export function isValidPhoneAndMobile(str) {
    if (IsNullOrEmpty(str)) {
        return false;
    }

    var reg = /^((\([0]\d{2,3}\))|[0](\d{2,3}|\d{2,3}-))?\d{7,8}$|^[1](3|4|5|7|8)\d{9}$|^[1](66|98|99)\d{8}$/;
    return reg.test(str);
}

/*身份证校验*/
export function verifyIdCard(str) {
    var r = false;
    if (IsNullOrEmpty(str)) {
        r = false;
    }
    if (str.length == 15) {
        var reg = /^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$/;
        return reg.test(str);
    }
    if (str.length == 18) {
        var reg = /^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[A-Z])$/;
        return reg.test(str);
    }
    return r;
}

/*校验固定电话码格式 */
export function isTelCode(str) {
    var reg = /^((0\d{2,3}-\d{7,8})|(1[3584]\d{9}))$/;
    return reg.test(str);
}
export function returnFloat(value){  //保留小数
    var value=Math.round(parseFloat(value)*100)/100;
    var xsd=value.toString().split(".");
    if(xsd.length==1){
    value=value.toString()+".00";
    return value;
    }
    if(xsd.length>1){
    if(xsd[1].length<2){
    value=value.toString()+"0";
    }
    return value;
    }
 };
/*校验登录账号格式 6-16字符 数字、字母*/
export function isUserID(str) {
    var patrn = /^([a-zA-Z0-9]){6,16}$/;
    return patrn.exec(str);
}

/*转为正整数*/
export function isValidPositiveInteger(str) {
    var d = GetInt(str);
    if (d < 0) {
        d = -d;
    }
    return d;
}

/*校验密码6-16字符 不包含空格    !!!!!之所以叫这个名字是因为isPassword方法已经存在 */
export function isPasswd(str) {
    var patrn = /^([A-Za-z0-9]|[~!@#$%^&*()_=`\[\]\\{}|;':",.<>?]){6,16}$/;
    return patrn.exec(str);
}

/*密码高级验证  >>>>>  校验密码6-16字符 必须包含大小字母、数字、特称字符 */
export function isAdvancedPassword(str) {
    var patrn = /^(?=.*[0-9])(?=.*[A-Z])(?=.*[a-z])(?=.*[^a-zA-Z0-9]).{6,16}$/;
    return patrn.exec(str);
}

/*密码中级验证  >>>>>  校验密码6-16字符 必须包含字母、数字、特称字符 */
export function isIntermediatePassword(str) {
    var patrn = /^(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[^a-zA-Z0-9]).{6,16}$/;
    return patrn.exec(str);
}

/*密码初级验证  >>>>>  校验密码6-16字符 必须包含字母、数字 */
export function isPrimaryPassword(str) {
    var patrn = /^(?=.*[0-9])(?=.*[a-zA-Z]).{6,16}$/;
    return patrn.exec(str);
}

//时间戳转datetime   datet=/Date(时间戳)/
export function transferDatetime(datet) {
    //datet不能为null
    if (IsNullOrEmpty(datet)) {
        datet = "";
    }
    var date = new Date(Number(datet.substring(6, datet.length - 2)));
    Y = date.getFullYear() + '-';
    M = (date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1) + '-';
    D = p(date.getDate()) + ' ';
    h = p(date.getHours()) + ':';
    m = p(date.getMinutes()) + ':';
    s = p(date.getSeconds());
    return Y + M + D + h + m + s;
}

//获取当前时间 时间戳
export function GetTimestamp() {
    var timestamp = (new Date()).getTime();
    return timestamp;
}

/**
 * @description 时间格式化
 * @author weig
 * @param {DataTime} dataTime 
 * @param {String} format 
 */
export function formatDateTime(dataTime, format){
    let date = new Date(dataTime);
    let year = date.getFullYear();
    let month = date.getMonth() + 1;
    month = month >= 10 ? month : "0" + month;
    let day = date.getDate();
    day = day >= 10 ? day : "0" + day;
    let hour = date.getHours();
    hour = hour >= 10 ? hour : "0" + hour;
    let min = date.getMinutes();
    min = min >= 10 ? min : "0" + min;
    let sec = date.getSeconds();
    sec = sec >= 10 ? sec: "0" + sec;

    if (format == "yyyy-MM-dd HH:mm:ss"){
        return year + "-" + month + "-" + day +" " + hour + ":" + min + ":" + sec;
    } else if (format == "yyyy-MM-dd HH:mm"){
        return year + "-" + month + "-" + day +" " + hour + ":" + min;
    } else if (format == "yyyy-MM-dd"){
        return year + "-" + month + "-" + day;
    } else if (format == "HH:mm:ss"){
        return hour + ":" + min + ":" + sec;
    } else if (format == "mm:ss"){
        return min + ":" + sec;
    } else {
        return year + "/" + month + "/" + day;
    }
}

//将时间转换为时间戳
export function DateToTimestamp(timestampVal) {
    var timestamp = new Date(transferDatetime(timestampVal)).getTime();
    return timestamp;
}

/*
 * 字符串部分隐藏
 * @param text:字符串
 * @param front:前面显示的字数
 * @param back:末尾显示的字数
 * @param hide:超出显示长度,字符串显示false或隐藏true
 */
export function ShowShortString(text, front, back, hide) {
    var newText = "";
    if (IsNullOrEmpty(text)) {
        return newText;
    }
    front = front < 0 ? 0 : front;
    back = back < 0 ? 0 : back;
    if (!hide && text.length < front + back) {
        front = text.length;
        back = 0;
    }
    if (hide && text.length < front + back) {
        front = 0;
        back = 0;
    }
    newText += text.substring(0, front);
    for (var i = 0; i < text.length - back - front; i++) {
        newText += '*';
    }
    newText += text.substring(text.length - back, text.length);
    return newText;
}


/*
* url 目标url
* name 需要替换的参数名称
* value 替换后的参数的值
* return url 参数替换后的url
*/
export function editUrlQuery(url, name, value) {
    var pattern = name + '=([^&#$]*)';
    var replaceText = name + '=' + value;
    if (url.match(pattern)) {
        var tmp = '/(' + name + '=)([^&#$]*)/gi';
        tmp = url.replace(eval(tmp), replaceText);
        return tmp;
    } else {
        var point = url.match('#.*') ? url.match('#.*') : "";
        var turl = url.replace(point, "");
        if (url.match('[\?]')) {
            return turl + '&' + replaceText + point;
        } else {
            return turl + '?' + replaceText + point;
        }
    }
}


// /*
//  * 获取URL参数
//  */
// export function getQueryString(name) {
//     var nowUrl = document.location.search.slice(1);
//     var qArray = nowUrl.split("&");
//     for (var i = 0; i < qArray.length; i++) {
//         var vArray = qArray[i].split("=");
//         if (vArray[0] === name) {
//             return decodeURIComponent(vArray[1]);
//         }
//     }
//     return "";
// }

/*
 * 获取URL参数
 */
export function getSysMsgQueryString(url, name) {
    var nowUrl = url.slice(1);
    var qArray = nowUrl.split("&");
    for (var i = 0; i < qArray.length; i++) {
        var vArray = qArray[i].split("=");
        if (vArray.length > 0) {
            if (vArray[0] === name && vArray.length > 2) {

                return decodeURIComponent(vArray[1] + '=' + vArray[2]);
            } else {
                return decodeURIComponent(vArray[1]);

            }
        }

    }
    return "";
}

/*
 * 修改当前url参数
 */
export function editLocUrlQuery(name, value) {
    var url = location.href.toString();
    return editUrlQuery(url, name, value);
}


/*
获取url参数值
*/
export function getQueryString(url, name) {
    var nowUrl = url.slice(1);
    var qArray = nowUrl.split("&");
    for (var i = 0; i < qArray.length; i++) {
        var vArray = qArray[i].split("=");
        if (vArray[0].replace(/\s+/g, "") === name) {
            return decodeURIComponent(vArray[1]);
        }
    }
    return "";
}

/*
获取url参数值(稳定)
*/
export function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}


//cookie操作
export function setCookie(c_name,value,expire) {
    var date=new Date()
    date.setSeconds(date.getSeconds()+expire)
    document.cookie=c_name+ "="+escape(value)+"; expires="+date.toGMTString()
}
 
export function getCookie(c_name){
    if (document.cookie.length>0){
        let c_start=document.cookie.indexOf(c_name + "=")
        if (c_start!=-1){ 
            c_start=c_start + c_name.length+1 
            let c_end=document.cookie.indexOf(";",c_start)
            if (c_end==-1) c_end=document.cookie.length
                return unescape(document.cookie.substring(c_start,c_end))
            } 
        }
    return ""
}
 
export function delCookie(name){
    setCookie(name, "", -1)
}

export function formatDate(date) {
    var date = new Date(date);
    var YY = date.getFullYear() + '-';
    var MM = (date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1) + '-';
    var DD = (date.getDate() < 10 ? '0' + (date.getDate()) : date.getDate());
    var hh = (date.getHours() < 10 ? '0' + date.getHours() : date.getHours()) + ':';
    var mm = (date.getMinutes() < 10 ? '0' + date.getMinutes() : date.getMinutes()) + ':';
    var ss = (date.getSeconds() < 10 ? '0' + date.getSeconds() : date.getSeconds());
    return YY + MM + DD +" "+hh + mm + ss;
  }

/**
 * @description 自定义封装ElementBox确认取消对话框
 * @author weig
 * @param {Object} t ElMessageBox对象
 * @param {String} content 提示内容
 * @param {String} title 标题
 * @param {String} type 对话框类型
 * @param {Function} sureAction 确认回调
 * @param {Function} cancelAction 取消回调
 * @param {String} yesText 确定文本
 * @param {String} noText 取消文本
 */
export function elConfirmDialog (t,content, title, type, sureAction,cancelAction,yesText,noText){
    t.confirm(content,title,{
        confirmButtonText: yesText ? yesText : '确定',
        cancelButtonText: noText ? noText : "取消",
        type: type ? type : "warning"
    }).then(()=>{
        if (sureAction && typeof(sureAction) == "function"){
            sureAction();
        }
    }).catch(err =>{
        if (cancelAction && typeof(sureAction) == "function"){
            cancelAction();
        }
    });
}


