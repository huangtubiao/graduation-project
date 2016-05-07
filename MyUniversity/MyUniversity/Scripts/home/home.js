var goHref = null; //要跳转的页面
$(document).ready(function () {
    //导航条显示通知信息
    if ($(".msg_icon").text() > 0) {
        $(".msg_icon").css("display", "block");
    }

    var userId = localStorage.getItem("userId")
    if (userId != null) {
        var data = {};
        data.userId = userId;
        $.ajax({
            url: "/Home/setLogin",
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result) {
                    return;
                }
                else {
                    return;
                }
            }
        });
    }
    //显示个人头像弹出窗的用户信息
    $.post('/account/getUserInfo', function (result) {
        if (result != null) {
            var jsonObj = eval("(" + result + ")");
            $("#js-user-name").text(jsonObj.userName);
            $("#js-user-mp").text(jsonObj.userRank);
            $("#js-user-fans").text(jsonObj.userFans);
            $(".account-num").attr("href", "/account/person/" + jsonObj.userId);
        } else {
            return;
        }
    }, "json");
});

//登录窗体弹出
function showlogin(toHref) {
    $("#login").show();
    $(".modal-backdrop").show();
    if (toHref != null) {
        goHref = toHref;
    }
}

//关闭登录弹出窗体
function offlogin() {
    $("#login").hide();
    $(".modal-backdrop").hide();
}

$(function () {
    $(".account-num").mouseover(function () {
        $(".person-select").css("display", "block");
    });
    $(".account-num").mouseout(function () {
        $(".person-select").css("display", "none");
    });
    $("#q").mouseover(function () {
        $(".person-select").css("display", "block");
    });
    $("#q").mouseout(function () {
        $(".person-select").css("display", "none");
    });

    //进入个人中心、我的设置时，判断是否已经登录
    $(".my").click(function () {
        var that = $(this).text();
        $.post('/projectsuperior/checkIfLogin', function (result) {
            if (result.boo_success) {
                if (that == "个人中心") {
                    location.href = "/account/person";
                } else {
                    location.href = "/account/setprofile";
                }
            } else {
                if (that == "个人中心") {
                    var toHref = "/account/person";
                    showlogin(toHref);
                } else {
                    var toHref = "/account/setprofile";
                    showlogin(toHref);
                }
            }
        }, "json");
    });
});

//登录
function btnLogin() {
    $("#signin-btn").text("登录中...");
    var data = {};
    data.userAccount = $(".ipt-phone").val();
    data.userPaw = $(".ipt-pwd").val();
    $.ajax({
        url: "/account/login",
        type: "get",
        dataType: "json",
        data: data,
        success: function (result) {
            if (result.success != false) {
                $("#signin-btn").text("登录");
                if (goHref == "xiaonei") {
                    $("#1 .menu-cur").click();
                    $("#login").fadeOut("fast");
                    $(".modal-backdrop").css("display", "none");

                } else {
                    if (goHref != null) {
                        location.href = goHref;
                    } else {
                        window.location.href = "/projectsuperior/index";
                    }
                }
            } else {
                if (result.message == "账号不存在") {
                    var label = document.createElement("label");
                    label.className = "error";
                    label.innerHTML += "该用户账号不存在";
                    $("#phone").after(label);
                } else if (result.message == "密码错误") {
                    var label = document.createElement("label");
                    label.className = "error";
                    label.innerHTML += "输入密码错误";
                    $("#password").after(label);
                }
            }
        }
    });
}

function tabs() {
    notiveLi = $("#notice-tit li");
    //选项卡
    var timer = null;
    var titles = $("#notice-tit").children("li");
    var divs = $("#notice-con").children(".mod");

    if (titles.length != divs.length)
        return;
    for (var i = 0; i < titles.length; i++) {
        titles[i].id = i;
        $("#" + titles[i].id).click(function () {
            var that = this;
            if (timer) {
                clearTimeout(timer);
                timer = null;
            }
            timer = setTimeout(function () {
                for (var j = 0; j < titles.length; j++) {
                    $("#" + titles[j].id).attr("class", "");
                    $("#" + divs[j].id).fadeOut("fast");
                }
                $("#" + titles[that.id].id).addClass("onactive");
                $("#" + divs[that.id].id).fadeIn("fast");
            }, 0);
            notiveLi = $("#notice-tit li");
        });
    }
}

//获取当前url的参数
function request(paras) {
    var url = location.href;
    var paraString = url.substring(url.indexOf("/") + 2, url.length).split("/");
    return paraString[paras];
}

//回到顶部效果
$(function () {
    var timer = null;
    var isTop = true;
    var clientHeight = document.documentElement.clientHeight;
    window.onscroll = function () {
        var osTop = document.documentElement.scrollTop || document.body.scrollTop;
        if (osTop >= clientHeight) {
            $("#backTop").css("display", "block");
        } else {
            $("#backTop").css("display", "none");
        }
        if (!isTop) {
            clearInterval(timer);
        }
        isTop = false;
    }
    $("#backTop").click(function () {
        timer = setInterval(function () {
            var osTop = document.documentElement.scrollTop || document.body.scrollTop;
            var ispeed = Math.floor(-osTop / 6);
            document.documentElement.scrollTop = document.body.scrollTop = osTop + ispeed;
            isTop = true;
            if (osTop == 0) {
                clearInterval(timer);
            }
        }, 30);
    });
});
