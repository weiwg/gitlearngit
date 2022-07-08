
/*
 * 基础方法
 */

$(function () {
    webInit();
    IsMobileBrowser();
    noBugInCode(1);
});

function webInit() {
    //返回按钮
    $("body").on("click", ".nav-left .icon-back", function () {
        if ($(this).hasClass("icon-noevent")) {
            return;
        }
        if (!$(this).hasClass("iframe-back")) {
            if (IsNullOrEmpty(document.referrer)) {
                location.replace('/');
            } else {
                history.back();
            }
        }
    });
    //tab导航标签
    $("body").on("click", ".tab-nav-item", function () {
        $(".tab-nav-item").removeClass("tab-active");
        $(".tab-panel-item").removeClass("tab-active");
        $(this).addClass("tab-active");
        var panel = $(this).data("panel");
        if (panel) {
            $(".tab-panel-item[data-panel='" + panel + "']").addClass("tab-active");
        } else {
            $($(".tab-panel-item")[$(this).index()]).addClass("tab-active");
        }
    });

    var userAgent = GetString(navigator.userAgent).toLowerCase();
    if (/android/i.test(userAgent)) {
    }
    if (/(iphone|ipad|ipod|ios|mac)/i.test(userAgent)) {
        $("body").on("blur", "input,select", function () {
            setTimeout(function () {
                var scrollHeight = document.documentElement.scrollTop || document.body.scrollTop || 0;
                window.scrollTo(0, Math.max(scrollHeight - 1, 0));
            }, 100);
        });
    }

    //表单提交事件
    $("body").on("submit", "form", function (e) {
        if ($(this).attr("disabled")) {
            e.preventDefault();
        }
    });
    
    //select赋值
    setSelectValue();

    $("body").on("click", "a", function () {
        var item = $(this);
        var iframe = item.attr("iframe");
        var url = item.attr("href");
        if (iframe === "up") {
            showIframe(url, { item:item, showType: "up" });
            return false;
        }
        if (GetBool(item.attr("iframe"))) {
            showIframe(url, { item: item });
            return false;
        }
        return true;
    });
    closeIframeInit();

    $("body").on("click", ".nav-left .iframe-back", function () {
        closeIframeCallBack();
    });
    
    $("body").on("input propertychange", "input[type='number'][maxlength]", function () {
        var maxlength = $(this).attr("maxlength");
        if (!maxlength) { return;}
        var val = $(this).val();
        //val = val.length > maxlength ? val.substring(0, maxlength) : val;
        val = val.length > maxlength ? $(this).data("maxval") : val;
        $(this).val(val).data("maxval", val);
    });

    if ($("meta[name=gotoTop]").length > 0) {
        var top = 200, cusTop = GetInt($("meta[name=gotoTop]").data("top"));
        $("<div class='goto-top'></div>").appendTo('body');
        if ($(".navbar-block").length === 0) { $(".goto-top").css("bottom", "10px"); }
        if (cusTop > 0) { $(".goto-top").css("bottom", (cusTop + 10) + "px"); }
        if ($(this).scrollTop() < top) { $(".goto-top").fadeOut(); }
        $(window).scroll(function (e) { if ($(this).scrollTop() < top) { $(".goto-top").fadeOut(); } if ($(this).scrollTop() > top) { $(".goto-top").fadeIn(); } });
        $("body").on("click", ".goto-top", function() {$("html,body").animate({ scrollTop: "0px" }, 500);});
    }
    $('[data-toggle="tooltip"]').tooltip();


    $("body").on("click", ".header-nav-block", function () {
        var item = $(this);
        var count = GetInt(item.data("count"));
        count++;
        item.data("count", count);
        if (count > 5 || $("script[src*='/npm/eruda']").length > 0) {
            showTips("eruda已初始化");
            return;
        }
        if (item.data("timer")) {
            clearTimeout(item.data("timer"));
        }
        item.data("timer", setTimeout(function () { item.removeData("count");}, 1000));
        console.log("eruda初始化:" + count);
        if (GetInt(item.data("count")) === 5 && $("script[src*='/npm/eruda']").length === 0) {
            var script = document.createElement('script');
            script.setAttribute('src', '//cdn.jsdelivr.net/npm/eruda');
            document.head.appendChild(script);
            $("body").append("<div class='eruda'><div>");
            setTimeout(function () { eruda.init({ container: $(".eruda")[0] }); console.log("初始化eruda成功"); }, 1000);
        }
    });
}

function IsMobileBrowser() {
    var userAgent = navigator.userAgent;
    if (IsNullOrEmpty(userAgent)) {
        return;
    }
    userAgent = userAgent.toLowerCase();
    var isRepeatCookie = GetInt($().getCookie("mobile_browser_repeat"));
    if (isRepeatCookie > 2) {
        return;
    }
    if (userAgent.indexOf("windows nt") >= 0 || userAgent.indexOf("macintosh") >= 0) {
        isRepeatCookie++;
        $().setCookie("mobile_browser_repeat", isRepeatCookie, null, "/");
        if (window.location.href.indexOf("localhost") >= 0 || window.location.href.indexOf("192.168.") >= 0 || window.location.href.indexOf("locmallmb.eonup") >= 0) {
            isRepeatCookie = 99;
        }
        if (isRepeatCookie < 2) {
            redirectUrl("http://vmall.eonup.com/", "replace");
        }
    }
}

function getAjaxJson(url, getData, successCallBack, errorCallBack, beforeSendCallBack, completeCallBack) {
    var param = {
        url: url, requestType: "get", dataType: "json", async: true, data: getData,
        successCallBack: successCallBack, errorCallBack: errorCallBack, beforeSendCallBack: beforeSendCallBack, completeCallBack: completeCallBack
    };
    doAjaxMain(param);
}

function getAjaxJsonSync(url, getData, successCallBack, errorCallBack, beforeSendCallBack, completeCallBack) {
    var param = {
        url: url, requestType: "get", dataType: "json", async: false, data: getData,
        successCallBack: successCallBack, errorCallBack: errorCallBack, beforeSendCallBack: beforeSendCallBack, completeCallBack: completeCallBack
    };
    doAjaxMain(param);
}

function getAjaxJsonp(url, getData, successCallBack, errorCallBack, beforeSendCallBack, completeCallBack) {
    var param = {
        url: url, requestType: "get", dataType: "jsonp", async: true, data: getData,
        successCallBack: successCallBack, errorCallBack: errorCallBack, beforeSendCallBack: beforeSendCallBack, completeCallBack: completeCallBack
    };
    doAjaxMain(param);
}

function postAjaxJson(url, postData, successCallBack, errorCallBack, beforeSendCallBack, completeCallBack) {
    var param = {
        url: url, requestType: "post", dataType: "json", async: false, data: postData,
        successCallBack: successCallBack, errorCallBack: errorCallBack, beforeSendCallBack: beforeSendCallBack, completeCallBack: completeCallBack
    };
    doAjaxMain(param);
}

function postAjaxJsonNew(param) {
    var config = {requestType: "post", dataType: "json", async: false};
    $.extend(config, param);
    doAjaxMain(config);
}

function postFileAjax(url, postData, successCallBack, errorCallBack, beforeSendCallBack, completeCallBack) {
    var param = {
        url: url, requestType: "post", dataType: "json", async: false, data: postData,
        successCallBack: successCallBack, errorCallBack: errorCallBack, beforeSendCallBack: beforeSendCallBack, completeCallBack: completeCallBack
    };
    doAjaxMain(param);
}

function doAjax(url, requestType, dataType, async, data, successCallBack, errorCallBack, beforeSendCallBack, completeCallBack) {
    var param = {
        url: url, requestType: requestType, dataType: dataType, async: async, data: data,
        successCallBack: successCallBack, errorCallBack: errorCallBack, beforeSendCallBack: beforeSendCallBack, completeCallBack: completeCallBack
    };
    doAjaxMain(param);
}

var showLoadingCount = 0;
function doAjaxMain(param) {
    if (IsNullOrEmpty(param.url)) {
        return;
    }
    if (IsNullOrEmpty(param.requestType)) {
        param.requestType = "get";
    }
    //var contentType = "application/x-www-form-urlencoded";
    if (IsNullOrEmpty(param.dataType)) {
        param.dataType = "text";
    }
    if (IsNullOrEmpty(param.async)) {
        param.async = "false";
    }
    if (IsNullOrEmpty(param.timeout)) {
        param.timeout = 20000;
    }
    if (IsNullOrEmpty(param.isShowLoading)) {
        param.isShowLoading = false;
    }
    try {
        $.ajax({
            url: param.url,
            type: param.requestType,
            data: param.data,
            dataType: param.dataType,
            async: param.async,
            timeout: param.timeout, //超时时间设置，单位毫秒
            success: function (data) {
                if (!data.Success && data.Code === "NotLoggedIn") {
                    showConfirm(data.Msg, function () {
                        window.location.href = IsNullOrEmpty(data.Data) ? editUrlQuery(data.Data, "returnurl", encodeURI(window.location.href)) : data.Data;
                    });
                } else if (typeof param.successCallBack == "function") {
                    param.successCallBack(data);
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrown) {
                if (typeof param.errorCallBack == "function") {
                    param.errorCallBack(xmlHttpRequest, textStatus, errorThrown);
                } else {
                    if (xmlHttpRequest.statusText === "timeout") {
                        showConfirmMsg("哎，系统忙不来了，请稍后再试。");
                    } else {
                        showConfirmMsg("咦，好像出问题了，请稍后再试。\r\nstatus：" + xmlHttpRequest.status + "，msg：" + xmlHttpRequest.statusText);
                    }
                }
            },
            beforeSend: function () {
                if (typeof param.beforeSendCallBack == "function") {
                    param.beforeSendCallBack();
                }
                if (param.isShowLoading) {
                    showLoadingCount++;
                    showLoading();
                }
            },
            complete: function () {
                if (typeof param.completeCallBack == "function") {
                    param.completeCallBack();
                }
                if (param.isShowLoading) {
                    showLoadingCount--;
                    if (showLoadingCount === 0) {
                        layer.close($(".layui-m-layer.layui-open-loading").attr('index'));
                    }
                }
            }
        });
    } catch (e) {
        //alert("系统开小差了,请稍后再试");
    }
}

function noBugInCode(type) {
    if (top.location !== self.location) {
        return;
    }
    if (type === 1) {
        console.info("" +
            "      ┏┓　　　┏┓+ +\n" +
            "    ┏┛┻━━━┛┻┓ + +\n" +
            "    ┃　　　　　　　┃ 　\n" +
            "    ┃　　　━　　　┃ ++ + + +\n" +
            "    ████━█████    ┃ +\n" +
            "    ┃　　　　　　　┃ +\n" +
            "    ┃　　　┻　　　┃\n" +
            "    ┃　　　　　　　┃ + +\n" +
            "    ┗━┓　　　┏━┛\n" +
            "        ┃　　　┃\n" +
            "        ┃　　　┃ + + + +\n" +
            "        ┃　　　┃　　　　Code is far away from bug with the animal protecting\n" +
            "        ┃　　　┃ + 　　　　神兽保佑,代码无bug\n" +
            "        ┃　　　┃\n" +
            "        ┃　　　┃ + \n" +
            "        ┃ 　　 ┗━━━┓ + +\n" +
            "        ┃ 　　　　　　　┣┓\n" +
            "        ┃ 　　　　　　　┏┛\n" +
            "        ┗┓┓┏━┳┓┏┛ + + + +\n" +
            "          ┃┫┫　┃┫┫\n" +
            "          ┗┻┛　┗┻┛+ + + +\n" +
            "\n" +
            "   bug是不可能有bug的,这辈子都不可能有bug的\n" +
            "   有bug? 那一定是你电脑有问题\n" +
            "\n");

    } else {
        console.info("" +
            "                       _oo0oo_ \n" +
            "                      o8888888o \n" +
            "                      88\" . \"88 \n" +
            "                      (| -_- |) \n" +
            "                      0\\  =  /0 \n" +
            "                    ___/`---'\\___ \n" +
            "                  .' \\\\|     |\" '. \n" +
            "                 / \\\\|||  :  |||\" \\ \n" +
            "                / _||||| -:- |||||- \\ \n" +
            "               |   | \\\\\\  -  \"/ |   | \n" +
            "               | \\_|  ''\\---/''  |_/ | \n" +
            "               \\  .-\\__  '-'  ___/-. / \n" +
            "             ___'. .'  /--.--\\  `. .'___ \n" +
            "          .\"\" '<  `.___\\_<|>_/___.' >' \"\". \n" +
            "         | | :  `- \\`.;`\\ _ /`;.`/ - ` : | | \n" +
            "         \\  \\ `_.   \\_ __\\ /__ _/   .-` /  / \n" +
            "     =====`-.____`.___ \\_____/___.-`___.-'===== \n" +
            "                       `=---=' \n" +
            "     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ \n" +
            "             佛祖保佑             永无BUG  \n" +
            "     佛曰:   \n" +
            "             写字楼里写字间，写字间里程序员；   \n" +
            "             程序人员写程序，又拿程序换酒钱。   \n" +
            "             酒醒只在网上坐，酒醉还来网下眠；   \n" +
            "             酒醉酒醒日复日，网上网下年复年。   \n" +
            "             但愿老死电脑间，不愿鞠躬老板前；   \n" +
            "             奔驰宝马贵者趣，公交自行程序员。   \n" +
            "             别人笑我忒疯癫，我笑自己命太贱；   \n" +
            "             不见满街漂亮妹，哪个归得程序员？ \n" +
            "     ............................................. \n");
    }
}

/*
 * 获取URL参数
 */
function getQueryString(name) {
    var nowUrl = document.location.search.slice(1);
    var qArray = nowUrl.split("&");
    for (var i = 0; i < qArray.length; i++) {
        var vArray = qArray[i].split("=");
        if (vArray[0] === name) {
            return decodeURIComponent(vArray[1]);
        }
    }
    return "";
}

/*
 * 修改当前url参数
 */
function editLocUrlQuery(name, value) {
    var url = location.href.toString();
    return editUrlQuery(url, name, value);
}

/*
 * 批量修改当前url参数
 * array = { id: 1, no: "N100", name: "hello" };
 */
function editLocUrlQueryArray(array) {
    var url = location.href.toString();
    for (var key in array) {
        if (array.hasOwnProperty(key)) {
            url = editUrlQuery(url, key, array[key]);
        }
    }
    return url;
}

/* 
 * url 目标url 
 * name 需要替换的参数名称 
 * value 替换后的参数的值 
 * return url 参数替换后的url 
 */
function editUrlQuery(url, name, value) {
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

/* 
 * 查询条件自动赋值
 */
function setQueryData() {
    var nowUrl = document.location.search.slice(1);
    var qArray = nowUrl.split("&");
    for (var i = 0; i < qArray.length; i++) {
        var vArray = qArray[i].split("=");
        var item = $("#" + vArray[0]);
        var value = decodeURIComponent(vArray[1]);
        if (item.is('select')) {
            item.data("id", value);
        } else {
            item.val(value);
        }
    }
}

/*
 * 跳转页面
 */
function redirectUrl(url,type) {
    window.location.href = url;
    if (type === "replace") {
        window.location.replace(url);
    } else {

        window.location.href = url;
    }
}
/*
 * 延时跳转页面(秒)
 */
function delayRedirectUrl(url, time, type) {
    time = !time ? 2 * 1000 : time * 1000;
    setTimeout(function () { redirectUrl(url, type); }, time);
}
/*
 * 延时刷新页面(秒)
 */
function delayReloadUrl(time) {
    time = !time ? 1 * 1000 : time * 1000;
    setTimeout(function () { window.location.reload(); }, time);

}

/*
 * 动态加载JS
 * @param {string} url 脚本地址
 * @param {function} callback  回调函数
 */
function loadJs(url, callback) {
    var head = document.getElementsByTagName('head')[0];
    var script = document.createElement('script');
    script.type = 'text/javascript';
    script.src = url;
    if (typeof (callback) == 'function') {
        script.onload = script.onreadystatechange = function () {
            if (!this.readyState || this.readyState === "loaded" || this.readyState === "complete") {
                script.onload = script.onreadystatechange = null;
                if (typeof callback == "function") {
                    callback();
                }
            }
        };
    }
    head.appendChild(script);
}
/*
 * 动态加载CSS
 * @param {string} url 样式地址
 */
function loadCss(url) {
    var head = document.getElementsByTagName('head')[0];
    var link = document.createElement('link');
    link.type = 'text/css';
    link.rel = 'stylesheet';
    link.href = url;
    head.appendChild(link);
}

/*
 * 显示消息
 */
function showTips(msg) {
    showMsg(msg, "msg",3);
}

function showLoading() {
    layer.open({
        type: 2,
        shadeClose: false,
        content: '加载中',
        success: function (elem) {
            $(elem).addClass("layui-open-loading");
            //console.log(elem);
        }
    });
}

function showMsg(msg, showtype, type) {
    if (!showtype) {
        showtype = '';
    }
    layer.open({
        type: type,//0 （0表示信息框，1表示页面层，2表示加载层，3表示消息，4编辑框）
        className: 'layer-mystyle',
        content: msg,
        anim: 'scale',
        skin: showtype,
        time: 3
    });
}

function showConfirmMsg(msg, yesCallBack) {
    layer.open({
        content: msg,
        anim: 'scale',
        shadeClose: false,
        btn: '确定',
        yes: function (index) {
            if (typeof yesCallBack == "function") {
                yesCallBack(index);
            } else {
                layer.close(index);
            }
        },
        success: function (elem) {
            frozenBody();
        },
        end: function (elem) {
            unfreezeBody();
        }
    });
}

function showConfirm(title, yesCallBack, noCallBack) {
    layer.open({
        content: title,
        shadeClose: false,
        btn: ['确认', '取消'],
        yes: function (index) {
            if (typeof yesCallBack == "function") {
                yesCallBack(index);
            } else {
                layer.close(index);
            }
        },
        no: function (index) {
            if (typeof noCallBack == "function") {
                noCallBack(index);
            } else {
                layer.close(index);
            }
        },
        success: function (elem) {
            frozenBody();
        },
        end: function (elem) {
            unfreezeBody();
        }
    });
}

function showSimpleForm(title, html, yesCallBack, successCallBack, noCallBack) {
    showForm({
        title: title, html: html, yesCallBack: yesCallBack, successCallBack: successCallBack, noCallBack: noCallBack
    });
}

function showForm(param) {
    layer.open({
        type: IsNullOrEmpty(param.type) ? 4 : param.type,
        title: param.title,
        anim: 'scale',
        className: IsNullOrEmpty(param.className) ? 'layui-open-form' : param.className,
        shadeClose: IsNullOrEmpty(param.shadeClose) ? false : GetBool(param.shadeClose),
        content: param.html,
        btn: param.btn ? param.btn : ['确认', '取消'],
        cbtn: param.cbtn ? param.cbtn : null,
        success: function (elem) {
            if (typeof param.successCallBack == "function") {
                param.successCallBack(elem);
            }
            frozenBody();
        },
        yes: function (index) {
            if (typeof param.yesCallBack == "function") {
                param.yesCallBack(index);
            } else {
                layer.close(index);
            }
        },
        no: function (index) {
            if (typeof param.noCallBack == "function") {
                param.noCallBack(index);
            } else {
                layer.close(index);
            }
        },
        end: function (elem) {
            unfreezeBody();
        }
    });
}

function showSimpleUpForm(html, successCallBack) {
    showUpForm({
         html: html, successCallBack: successCallBack
    });
}

function showUpForm(param) {
    layer.open({
        type: 4,
        zIndex: param.zIndex,
        anim: 'up',
        className: 'layui-open-up-form' + (IsNullOrEmpty(param.className) ? "" : " " + param.className),
        //shadeClose: false,
        content: param.html,
        success: function (elem) {
            if (typeof param.successCallBack == "function") {
                param.successCallBack(elem);
            }
            frozenBody();
        },
        end: function (elem) {
            unfreezeBody();
        }
    });
}

function showIframe(url, param) {
    var iframeId = getGuid();
    var iframeUrl = editUrlQuery(url, "iframe", iframeId);
    param = param ? param : {};
    if (GetBool(param.noLockScreen)) {
        iframeUrl = editUrlQuery(iframeUrl, "nolockscreen", true);
    }
    if (param.item && !IsNullOrEmpty(param.item.attr("changeurl"))) {
        param.changeUrl = GetBool(param.item.attr("changeurl"));
    }
    if (IsNullOrEmpty(param.changeUrl)) {
        param.changeUrl = false;
    }
    var oldUrl = window.location.href;
    var iframeParam = {
        type: 1,
        content: '<iframe src="' + iframeUrl + '" frameborder="no" border="0" ></iframe>',
        anim: 'up',
        className: IsNullOrEmpty(param.className) ? 'full-screen-mask show-iframe' : param.className,
        success: function (elem) {
            $(elem).attr('data-iframeid', iframeId);
            if (param.changeUrl) {
                $(elem).attr('data-oldurl', oldUrl);
                history.replaceState({}, '', url);
            }
            //$('[data-iframeid=' + iframeid + ']').attr("data-index", $(elem).attr("index"));
            if (!GetBool(param.noLockScreen)) {
                frozenBody();
            }
        },
        end:function() {
            if ($(".show-iframe").length === 0) {
                unfreezeBody();
            }
        }
    }
    if (param.showType === "up") {
        iframeParam.className = IsNullOrEmpty(param.className) ? 'full-screen-mask up-iframe show-iframe' : param.className;
    }

    if (top.location !== self.location) {
        parent.layer.open(iframeParam);
    }
    else {
        layer.open(iframeParam);
    }
}

function closeIframeInit(param) {
    if (IsNullOrEmpty(getQueryString("iframe"))) {
        return;
    }
    $("header .icon-back").addClass("iframe-back");
    $(".navbar-block").hide();

    if (param) {
        if (typeof param.complete == "function") {
            param.complete();
        }
    }
    //监听物理返回键
    //parent.window.history.pushState(null, document.title, window.location.pathname);
    //parent.window.addEventListener("popstate", function (e) {
    //    console.log(e);
    //    closeIframeCallBack();
    //},false);
}

function closeIframeCallBack(iframeid) {
    iframeid = IsNullOrEmpty(iframeid) ? getQueryString("iframe") : iframeid;
    if (IsNullOrEmpty(iframeid)) {
        return;
    }
    var noLockScreen = GetBool(getQueryString("nolockscreen"));
    if (top.location !== self.location) {
        if (!noLockScreen && parent.$(".show-iframe").length === 1) {
            parent.unfreezeBody();
        }
        var iframe = parent.$("[data-iframeid=" + iframeid + "]");
        var index = iframe.attr('index');
        var oldUrl = iframe.data('oldurl');
        if (!IsNullOrEmpty(oldUrl)) {
            parent.history.replaceState({}, '', oldUrl);
        }
        parent.layer.close(index);
    }

}

function frozenBody() {
    var count = GetInt($('body').data("frozen"));
    count = count < 0 ? 1 : count + 1;
    $('body').data("frozen", count);
    $('body').css("overflow", "hidden");
}

function unfreezeBody() {
    var count = GetInt($('body').data("frozen"));
    count = count < 1 ? 0 : count - 1;
    $('body').data("frozen", count);
    if (count <= 0) {
        $('body').css("overflow", "auto");
    }
}

//生成随机编码
function getGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

/*
 * 金额格式(保留2位小数)后格式化成金额形式
 */
function formatCurrency(num) {
    //num = num.toString().replace(/\$|\,/g, '');
    if (isNaN(num))
        num = "0";
    num = parseFloat(num);
    var sign = (num === (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    var cents = num % 100;
    num = Math.floor(num / 100).toString();
    if (cents < 10)
        cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++)
        num = num.substring(0, num.length - (4 * i + 3)) + '' +
                num.substring(num.length - (4 * i + 3));
    return (((sign) ? '' : '-') + num + '.' + cents);
}

/*
 * 时间格式化
 * yyyy-MM-dd
 * yyyy-MM-dd HH:mm:ss
 */
function formatDate(date, format) {
    if (!date) return "";
    if (!format) format = "yyyy-MM-dd";
    date = GetDate(date);
    var dict = {
        "yyyy": date.getFullYear(),
        "yy": date.getFullYear().toString().substr(2),
        "M": date.getMonth() + 1,
        "d": date.getDate(),
        "H": date.getHours(),
        "m": date.getMinutes(),
        "s": date.getSeconds(),
        "MM": ("" + (date.getMonth() + 101)).substr(1),
        "dd": ("" + (date.getDate() + 100)).substr(1),
        "HH": ("" + (date.getHours() + 100)).substr(1),
        "mm": ("" + (date.getMinutes() + 100)).substr(1),
        "ss": ("" + (date.getSeconds() + 100)).substr(1)
    };
    return format.replace(/(yyyy|yy|MM?|dd?|HH?|ss?|mm?)/g, function () {
        return dict[arguments[0]];
    });
}

/*
 * 获取安全时间,解决苹果手机时间格式问题
 */
function getSafeDate(date) { return GetDate(date); }
function GetDate(date) {
    var dateStr;
    switch (typeof date) {
        case "string":
            if (date.indexOf("/Date(") >= 0) {
                dateStr = parseInt(date.replace("/Date(", "").replace(")/", ""));
            } else {
                dateStr = date.replace(/\-/g, "/");
            }
            break;
        case "number":
            dateStr = date;
            break;
        default:
            dateStr = date;
    }
    return new Date(dateStr);
}

/*
 * 判断字符是否为空或null
 */
function IsNullOrEmpty(str) {
    if (typeof str == "undefined" || str == null || str === "" || str.toString() === "NaN") {
        return true;
    }
    return false;
}
function GetInt(str) {
    if (IsNullOrEmpty(str) || isNaN(str)) {
        return 0;
    }
    return parseInt(str) ? parseInt(str) : 0;;
}
function GetFloat(str) {
    if (IsNullOrEmpty(str) || isNaN(str)) {
        return 0;
    }
    return parseFloat(str) ? parseFloat(str) : 0;
}
function GetBool(str) {
    if (IsNullOrEmpty(str)) {
        return false;
    }
    return str.toString().toLowerCase() === "true";
}
function GetString(str) {
    if (IsNullOrEmpty(str)) {
        return "";
    }
    return str.toString();
}
function GetDataString(data, key) {
    if (IsNullOrEmpty(data) || IsNullOrEmpty(key)) {
        return "";
    }
    return GetString(data[key]);
}
function GetData(data, key) {
    if (IsNullOrEmpty(data) || IsNullOrEmpty(key)) {
        return "";
    }
    return data[key];
}
function GetDataInt(data, key) {
    if (IsNullOrEmpty(data) || IsNullOrEmpty(key)) {
        return 0;
    }
    return GetInt(data[key]);
}
function GetDataFloat(data, key) {
    if (IsNullOrEmpty(data) || IsNullOrEmpty(key)) {
        return 0;
    }
    return GetFloat(data[key]);
}
function GetDataBool(data, key) {
    if (IsNullOrEmpty(data) || IsNullOrEmpty(key)) {
        return false;
    }
    return GetBool(data[key]);
}
function GetDataDate(data, key) {
    if (IsNullOrEmpty(data) || IsNullOrEmpty(key)) {
        return "";
    }
    return GetDate(data[key]);
} 
function GetDataFormatDate(data, key, format) {
    if (IsNullOrEmpty(data) || IsNullOrEmpty(key)) {
        return "";
    }
    return formatDate(data[key], format);
}

/*
 * 获取UNIX时间戳
 */
function GetTimestamp() {
    return Date.parse(new Date()) / 1000;
}

/*
 * 输入控制
 */
function setAllInputDisabled() {
    $("input").attr("disabled", "true");
    $("textarea").attr("disabled", "true");
    $("select").attr("disabled", "true");
    $("button").attr("disabled", "true");
}

function setAllInputAbled() {
    $("input").removeAttr("disabled");
    $("textarea").removeAttr("disabled");
    $("select").removeAttr("disabled");
    $("button").removeAttr("disabled");
}

function resetAllInput(item) {
    $(item).find("input").val("");
    $(item).find("input[type='checkbox']").prop('checked', false);
    $(item).find("select").each(function (i, dataItem) {
        $(dataItem).find("option:first").prop('selected', 'selected');
    });
}

function getFromSerialize(item) {
    setFormAbledSubmit(item);
    var data = $(item).serialize();
    reSetFormAbledSubmit(item);
    return data;
}

function setFormAbledSubmit(item) {
    $(item).find("select[disabled],input[disabled]").removeAttr("disabled").attr("setUnabled", true).attr("readonly", "readonly");
}

function reSetFormAbledSubmit(item) {
    $(item).find("select[setUnabled=true],input[setUnabled=true]").attr("disabled", "disabled").removeAttr("setUnabled");
}

/*
 * 创建select
 */
function creatSelectOption(item, data, key, text, creatOptionFun) {
    $(item).empty();
    if (!data) {
        return;
    }
    if (typeof creatOptionFun == "function") {
        $(data).each(function (i, dataItem) {
            creatOptionFun(item, GetDataString(dataItem, key), GetDataString(dataItem, text), i, dataItem);
        });
    } else {
        $(data).each(function (i, dataItem) {
            $(item).append("<option value=" + GetDataString(dataItem, key) + ">" + GetDataString(dataItem, text) + "</option>");
        });
    }
    try {
        $(item).selectpicker('refresh');
    } catch (err) { }

    //if ($(item).find("option").length <= 0) {
    //    return;
    //}
    $(item).each(function (i, dataItem) {
        var selectValue = $(dataItem).data("id");
        var selectTxt = $(dataItem).data("txt");
        if (IsNullOrEmpty(selectValue)) {
            selectValue = $(dataItem).attr("data-id");
        }
        if (IsNullOrEmpty(selectValue)) {
            selectValue = $(dataItem).val();
        }
        if (IsNullOrEmpty(selectValue)) {
            return false;
        }
        selectValue = selectValue.toString().split(',');
        if (selectValue) {
            if ($(item).find('option[value="' + selectValue + '"]').length === 0 && !IsNullOrEmpty(selectTxt)) {
                $(item).prepend("<option value=" + selectValue + ">" + selectTxt + "</option>");
            }
            try {
                $(dataItem).selectpicker('val', selectValue);
            } catch (err) {
                $(dataItem).val(selectValue);
            }
            $(dataItem).removeAttr("data-id");
            $(dataItem).removeAttr("data-txt");
        }
        return true;
    });
}

/*
 * 创建select2
 */
function creatSelectOption2(item, data, key, value) {
    $(item).empty();
    $(data).each(function () {
        var dataItem = this;
        $(item).append("<option value=" + GetDataString(dataItem, key) + ">" + GetDataString(dataItem, value) + "</option>");
    });
    $(item).selectpicker('refresh');
    var selectValue = IsNullOrEmpty($(item).data("id")) ? '' : $(item).data("id").toString().split(',');
    if (selectValue) {
        $(item).selectpicker('val', selectValue);
        $(item).removeAttr("data-id");
    }
}

/*
 * 赋值select
 */
function setSelectValue() {
    var selectList = $("select[data-id]");
    if (selectList.length > 0) {
        selectList.each(function () {
            var dataItem = this;

            if ($(dataItem).find("option").length <= 0) {
                return true;
            }
            var selectValue = $(dataItem).data("id");
            if (IsNullOrEmpty(selectValue)) {
                selectValue = $(dataItem).attr("data-id");
            }
            selectValue = selectValue.toString().split(',');
            if (selectValue) {
                if (!IsNullOrEmpty(selectValue)) {
                    try {
                        if ($(dataItem).data("mobile")) {
                            $(dataItem).val(selectValue);
                        } else {
                            $(dataItem).selectpicker("val", selectValue);
                        }
                    } catch (err) {
                        $(dataItem).val(selectValue);
                    }
                    $(dataItem).removeAttr("data-id");
                    $(dataItem).removeData("id");
                }
            }
            return true;
        });
    }
}

/*
 * 初始化checkbox
 */
function initCheckbox(item) {
    var checkList;
    var isSwitch = false;
    if (item) {
        checkList = $(item).find('input[type="checkbox"]');
    } else {
        checkList = $('input[type="checkbox"]');
    }
    if (checkList.length === 0) {
        return;
    }
    try {
        isSwitch = true;
        checkList.bootstrapSwitch();
    } catch (err) {
        isSwitch = false;
    }
    setCheckboxValue();
    checkList.each(function(i, dataItem) {
        var id = $(dataItem).attr('id');
        var val = $(dataItem).prop("checked");
        $(dataItem).after('<input id="' + id + '" name="' + id + '" type="hidden" value="' + val + '">');
        $(dataItem).attr("id", "cb" + id);
        $(dataItem).removeAttr("name");
        if (isSwitch) {
            $(dataItem).on('switchChange.bootstrapSwitch', function(event, state) {
                $("#" + id).val(state);
            });
        } else {
            $(dataItem).change(function () {
                $("#" + id).val($(this).prop("checked"));
            });
        }
    });
}

/*
 * 赋值checkbox
 */
function setCheckboxValue() {
    var checkList = $('input[type="checkbox"]');
    if (checkList.length > 0) {
        checkList.each(function (i, dataItem) {
            var checkValue = $(dataItem).data("checked");
            if (IsNullOrEmpty(checkValue)) {
                checkValue = $(dataItem).attr("data-checked");
            }
            if (!IsNullOrEmpty(checkValue)) {
                try {
                    $(dataItem).bootstrapSwitch("state", GetBool(checkValue));
                } catch (err) {
                    $(dataItem).prop("checked", GetBool(checkValue));;
                }
                $(dataItem).removeAttr("data-checked");
                $(dataItem).removeData("checked");
            }
            return true;
        });
    }
}

function GetUnitFormat(isInch) {
    var unit = { isInchUnit: false, area: "㎡", area2: "(㎡)", area3: "/㎡", length: "mm", length2: "(mm)", lengthzh: "毫米(mm)" };
    if (GetBool(isInch)) {
        unit = { isInchUnit: true, area: "千英面积", area2: "(千英面积)", area3: "/千英面积", length: "in", length2: "(in)", lengthzh: "英寸(in)" };
    }
    return unit;
}

/*
校验表单是否正确,并返回错误消息
*/
function CheckFormValid(id) {
    var errorMsg = { valid: $(id).valid() }
    if (errorMsg.valid) {
        return errorMsg;
    }
    errorMsg = ConvertToJosn($(id).validate().errorMap);
    errorMsg.valid = false;
    if (errorMsg.length > 0) {
        showTips(errorMsg[0].value);
        $("#" + errorMsg[0].key).focus();
    }
    return errorMsg;
}

/*
json: {name:'名字',age:11}
to json:[{key:'name',value:'名字'},{key:'age',value:11}]
*/
function ConvertToJosn(data) {
    var json = [];
    if (!data) {
        return json;
    }
    for (var key in data) {
        var kvJson = { key: key, value: data.hasOwnProperty(key) ? data[key] : "" }
        json.push(kvJson);
    }
    return json;
}

/*
 * 字符串部分隐藏
 * @param text:字符串
 * @param front:前面显示的字数
 * @param back:末尾显示的字数
 * @param hide:超出显示长度,字符串显示false或隐藏true
 */
function ShowShortString(text, front, back, hide) {
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

function addNavBtn(icon, url, isIframe) {
    if ($(".header-nav-block .nav-right").length <= 0) {
        $(".header-nav-block").append('<div class="nav-right"></div>');
    }
    url = IsNullOrEmpty(url) ? "javascript:;" : url;
    $(".header-nav-block .nav-right").prepend('<a class="iconfont ' + icon + '" href="' + url + '" ' + (isIframe ? 'iframe="true"' : '') + '></a>');
    return $(".header-nav-block .nav-right ." + icon);
}

function removeNavBtn(icon) {
    $(".header-nav-block .nav-right").find('.' + icon).remove();
}

function createSearchBlock(param) {
    if (IsNullOrEmpty(param.className)) {
        param.className = "my-search";
    }
    var boxName = "common-box-" + commonBlockCount + " " + param.className;
    var maskBoxName = "common-box-" + commonBlockCount + " " + param.className;
    commonBlockCount++;
    $(".header-nav-block .nav-right .icon-search").remove();
    $("section.common-box").remove();
    $(".common-box-mask").remove();
    $(".header-nav-block .nav-right").prepend('<a class="iconfont icon-search" href="javascript:;"></a>');
    $("body").append('<section class="common-box ' + boxName + ' head-drop-box"><div class="common-block search-block"><div class="search-form">' + param.html +
        '</div><div class="search-btn-block"><a href="javascript:;" class="btn btn-reset">重置</a><a href="javascript:;" class="btn btn-search">搜索</a></div></div></section>' +
        '<div class="common-box-mask ' + maskBoxName + ' full-mask-box"></div>');

    function close() {
        $(".common-box-mask" + "." + param.className).fadeOut(500);
        $(".common-box" + "." + param.className).slideUp(400, function () {
            unfreezeBody();
        });
    }
    function show() {
        $(".common-box").slideUp(500);
        $(".full-mask-box").fadeOut(500);
        $(".common-box-mask" + "." + param.className).fadeIn(500);
        $(".common-box" + "." + param.className).slideDown(400, function () {
            frozenBody();
        });
    }

    function toggle() {
        $(".common-box").not(".common-box" + "." + param.className).slideUp(500);
        $(".full-mask-box").not(".common-box-mask" + "." + param.className).fadeOut(500);
        $(".common-box-mask" + "." + param.className).fadeToggle(500);
        $(".common-box" + "." + param.className).slideToggle(400, function () {
            if ($(".common-box" + "." + param.className).is(':hidden')) {
                unfreezeBody();
            } else {
                frozenBody();
            }
        });
    }

    $("body").on("click", "section.common-box .btn-reset", function () {
        $(".search-form").find("input").val("");
        $(".search-form").find("input[type='checkbox']").prop('checked', false);
        $(".search-form").find("select").each(function (i, dataItem) {
            $(this).find("option:first").prop('selected', 'selected');
        });
        try {
            $(".search-form").find("select").selectpicker('refresh');
        } catch (err) {
        }
        if (typeof param.reset == "function") {
            param.reset();
        }
    });

    $("body").on("click", "section.common-box .btn-search", function () {
        var result = true;
        if (typeof param.search == "function") {
            result = param.search();
        }
        if (result || result == undefined) {
            close();
        }
    });
    //搜索
    $("body").on("click", "header .nav-right .icon-search", function () {
        toggle();
        try {
            //解决布局异常问题
            $(".search-form").find("select").selectpicker('refresh');
        } catch (err) { }

    });
    $("body").on("click", ".common-box-mask", function () {
        close();
    });

    if (IsNullOrEmpty(param.setQueryData)) {
        param.setQueryData = true;
    }
    if (GetBool(param.setQueryData)) {
        setQueryData();//自动设置url参数值
    }

    if (typeof param.complete == "function") {
        param.complete($("section.common-box." + param.className));
    }
    return {
        show: show,
        close: close,
        toggle: toggle
    };
}

var commonBlockCount = 0;
function createCommonBlock(param) {
    if (IsNullOrEmpty(param.className)) {
        param.className = "my-box";
    }
    var boxName = "common-box-" + commonBlockCount + " " + param.className;
    var maskBoxName = "common-box-" + commonBlockCount + " " + param.className;
    if (param.anim === "head-drop") {
        boxName += " head-drop-box";
    }else if (param.anim === "bottom-up") {
        boxName += " bottom-up-box";
        if (!param.bottomup) {
            param.bottomup = 0;
        }
    }
    commonBlockCount++;
    //$(".header-nav-block .nav-right .icon-common").remove();
    $("section.common-box").remove();
    $(".common-box-mask").remove();
    //$(".header-nav-block .nav-right").prepend('<a class="iconfont icon-alignjustify" href="javascript:;"></a>');
    $("body").append('<section class="common-box ' + boxName + '"><div class="common-block"><div class="common-form">' + param.html + '</div></div></section>');
    $("body").append('<div class="common-box-mask ' + maskBoxName + ' full-mask-box"></div>');

    function close() {
        $(".common-box-mask" + "." + param.className).fadeOut(500);
        if (param.anim === "bottom-up") {
            $(".common-box" + "." + param.className).animate({ bottom: '-100%' }, 500, function () { $(this).hide(); unfreezeBody(); });
        } else {
            $(".common-box" + "." + param.className).slideUp(500, function () { unfreezeBody();});
        }
    }

    function show() {
        $(".common-box").slideUp(500);//隐藏其他
        $(".full-mask-box").fadeOut(500);//隐藏其他
        if (param.anim === "bottom-up") {
            $(".common-box" + "." + param.className).show().animate({ bottom: param.bottomup }, 500, function () { frozenBody(); });
        } else {
            $(".common-box" + "." + param.className).slideDown(500, function () {frozenBody();});
        }
        $(".common-box-mask" + "." + param.className).fadeIn(500);
        return $("section.common-box" + "." + param.className);
    }
    function toggle() {
        $(".common-box").not(".common-box" + "." + param.className).slideUp(500);
        $(".full-mask-box").not(".common-box-mask" + "." + param.className).fadeOut(500);
        $(".common-box-mask" + "." + param.className).fadeToggle(500);

        if (param.anim === "bottom-up") {
            if ($(".common-box" + "." + param.className).is(':hidden')) {
                $(".common-box" + "." + param.className).show().animate({ bottom: param.bottomup }, 500, function () { frozenBody(); });
            } else {
                $(".common-box" + "." + param.className).animate({ bottom: '-100%' }, 500, function () { $(this).hide(); unfreezeBody(); });
            }
        } else {
            $(".common-box" + "." + param.className).slideToggle(500, function () {
                if ($(".common-box" + "." + param.className).is(':hidden')) {
                    unfreezeBody();
                } else {
                    frozenBody();
                }
            });
        }
    }

    $("body").on("click", ".common-box-mask" + "." + param.className, function () {
        close();
    });
    if (typeof param.complete == "function") {
        param.complete($("section.common-box" + "." + param.className + " .common-form"), $("section.common-box" + "." + param.className), $(".common-box-mask" + "." + param.className));
    }

    return {
        show: show,
        close: close,
        toggle: toggle
    };
}

function initDatePicker(obj, param) {
    param.preset = "date";
    initDateTimePicker(obj, param);
}

function initTimePicker(obj, param) {
    param.preset = "time";
    initDateTimePicker(obj, param);
}

function initDateTimePicker(obj, param) {
    param = param ? param : {};
    param.preset = !param.preset || IsNullOrEmpty(param.preset) ? 'datetime' : param.preset;
    var opt = {
        preset: IsNullOrEmpty(param.preset) ? 'date' : param.preset,
        dateFormat: 'yyyy-mm-dd',
        dateOrder: IsNullOrEmpty(param.dateOrder) ? 'yyyymmdd' : param.dateOrder,
        theme: 'android-ics light', //皮肤样式
        mode: 'scroller', //日期选择模式
        display: 'modal', //显示方式【modal】【inline】【bubble】【top】【bottom】
        showNow: true,
        lang: 'zh',
        timeWheels: 'HHiiss',//HH:24小时制；hh:12小时制
        timeFormat: 'HH:ii:ss',
        minDate: param.preset === "time" ? null : param.minDate ? param.minDate : new Date(), //preset: 'time' 不能设置此属性
        maxDate: param.preset === "time" ? null : param.maxDate ? param.maxDate : new Date("2099-12-31 23:59:59"), //preset: 'time' 不能设置此属性
        onShow: function (inst) {
            if (typeof param.onShow == "function") {
                param.onShow(inst);
            } else {
                if ($(".layui-m-layer").length > 0) {
                    //解决layui弹窗层高的问题
                    $(".dw-persp").css('z-index', $(".layui-m-layer").css('z-index') + 1);
                    $(".dw.dwbg").css('z-index', $(".layui-m-layer").css('z-index') + 1);
                }
            }
        },
        onSelect: function (textVale, inst) { //选中时触发事件
            if (typeof param.onSelect == "function") {
                param.onSelect(textVale, inst);
            } else {
                if (obj.length === 2) {
                    var startItem = $(obj[0]);
                    var endItem = $(obj[1]);
                    if (new Date(startItem.val()) > new Date(endItem.val())) {
                        var temp = startItem.val();
                        startItem.val(endItem.val());
                        endItem.val(temp);
                        if (IsNullOrEmpty(param.onShowCompareMsg)) {
                            param.onShowCompareMsg = "开始时间不能大于结束时间";
                        }
                        showTips(param.onShowCompareMsg);
                    }
                }
                return;
            }
        }
    };
    if (param.preset === "date") {
        obj.mobiscroll(opt);
    } else if (param.preset === "time") {
        obj.mobiscroll(opt).time(opt);
    } else {
        obj.mobiscroll(opt).datetime(opt);
    }
}

function destroyDateTimePicker(obj) {
    obj.mobiscroll('destroy');
}

//json排序 {a:"1",c:"3",b:"2"} => {a: "1", b: "2", c: "3"}
function sortJson(json) {if(!json)return;var data={};Object.keys(json).sort().forEach(function(key){data[key]=json[key];});return data;};

//key value数组排序
/*
var ary=[{id:1,name:"a"},{id:2,name:"b"}];
ary.sort(keysrt('id',true));
*/
function sortkey(key, desc) {
    return function (a, b) {
        return desc ? ~~(a[key] < b[key]) : ~~(a[key] > b[key]);
    }
}

//扩展方法
//$("form").serializeObject()
$.fn.serializeObject = function () {
    var data = {};
    $.each(this.serializeArray(), function (i, item) {
        if (item.value === '') {
            return true;
        }
        if (data[item.name]) {
            if (!data[item.name].push) {
                data[item.name] = [data[item.name]];
            }
            data[item.name].push(item.value || '');
        } else {
            data[item.name] = item.value || '';
        }
        return true;
    });
    return data;
};

$.fn.getCookie = function getCookie(name) {
    var reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    var arr = document.cookie.match(reg);
    if (arr) {
        return unescape(arr[2]);
    } else {
        return null;
    }
}

$.fn.deleteCookie = function deleteCookie(name, path) {
    var cookie = name + "=;expires=" + new Date("1970-1-1").toGMTString();
    if (!IsNullOrEmpty(path)) {
        cookie += "path=" + path + ";";
    }
    document.cookie = cookie;
}

$.fn.setCookie = function setCookie(name, value, expires, path) {
    var cookie = name + "=" + escape(value) + ";";
    if (!IsNullOrEmpty(expires)) {
        cookie += "expires=" + expires.toGMTString() + ";";
    }
    if (!IsNullOrEmpty(path)) {
        cookie += "path=" + path + ";";
    }
    document.cookie = cookie;
}

//给Number类型增加一个加法。用法:(1.2).add(0.9)/ 1.2.add(0.9) => 1.2 + 0.9
Number.prototype.add=function(arg){return accAdd(this,arg);}
//给Number类型增加一个减法。用法:1.2.sub(0.9) => 1.2 - 0.9
Number.prototype.sub=function(arg){return accSub(this,arg);}
//给Number类型增加一个乘法。用法:1.2.mul(0.9) => 1.2 * 0.9
Number.prototype.mul=function(arg){return accMul(this,arg);}
//给Number类型增加一个除法。用法:1.2.div(0.9) => 1.2 / 0.9
Number.prototype.div=function(arg){return accDiv(this,arg);}

function accAdd(h,g){var l,k,j;try{l=h.toString().split(".")[1].length}catch(i){l=0}try{k=g.toString().split(".")[1].length}catch(i){k=0}return j=Math.pow(10,Math.max(l,k)),(accMul(h,j)+accMul(g,j))/j}
function accSub(h,g){var l,k,j;try{l=h.toString().split(".")[1].length}catch(i){l=0}try{k=g.toString().split(".")[1].length}catch(i){k=0}return j=Math.pow(10,Math.max(l,k)),(accMul(h,j)-accMul(g,j))/j}
function accMul(h,g){var l=0,k=h.toString(),j=g.toString();try{l+=k.split(".")[1].length}catch(i){}try{l+=j.split(".")[1].length}catch(i){}return Number(k.replace(".",""))*Number(j.replace(".",""))/Math.pow(10,l)}
function accDiv(h,g){var l=0;try{l=h.toString().split(".")[1].length}catch(i){}try{l=g.toString().split(".")[1].length}catch(i){}return accMul(h,Math.pow(10,l))/accMul(g,Math.pow(10,l))};
//四舍五入,保留小数位。用法:(1.2356).mathRound(2) => 1.24
Number.prototype.mathRound=function(arg){return mathRound(this,arg);}
function mathRound(v, n) {return Math.round(v.mul(Math.pow(10,n)))/Math.pow(10,n);}

//数组排序
//arr.OrderBy(key,asc);
//Array.prototype.OrderBy=function(key,asc){return ArrayOrderBy(this,key,asc)};
function ArrayOrderBy(arr,key,asc){var m;asc=asc?true:false;for(var i=0;i<arr.length;i++){for(var k=0;k<arr.length;k++){if(asc&&arr[i][key]<arr[k][key]){m=arr[k];arr[k]=arr[i];arr[i]=m;}else if(!asc&&arr[i][key]>arr[k][key]){m=arr[k];arr[k]=arr[i];arr[i]=m;}}}return arr;}

function ArrayCopy(arr){return JSON.parse(JSON.stringify(arr))}

; (function () {
    var src = '//cdn.jsdelivr.net/npm/eruda';
    if (!/eruda=true/.test(window.location) && localStorage.getItem('active-eruda') !== 'true') return;
    document.write('<scr' + 'ipt src="' + src + '"></scr' + 'ipt>');
    document.write('<scr' + 'ipt>eruda.init();</scr' + 'ipt>');
})();

