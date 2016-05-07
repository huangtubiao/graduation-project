$(document).ready(function () {
    tabs();

    //没有方案记录，显示没有图片      
    if ($(".super-note-wrap").children().length == 1) {
        $(".uncontent").css("display", "block");
    }
});

var that;
$(document).delegate(".profile-li", "click", function () {
    if ($(this).find("span").text() == "真实姓名：") {
        $(".person-info h4").text("真实姓名");
        $(".person-info p").text("填写真实姓名，方便你的同学找到你");
        $(".person-text").val($(this).find("em").text());
        $(".modal-backdrop").css("display", "block");
        $(".person-info").fadeIn("fast");
        that = $(this);
    }
    if ($(this).find("span").text() == "性别：") {
        var seImg = $(".user-select-sex i");
        for (var l = 0; l < seImg.length; l++) {
            seImg.eq(l).remove();
        }

        $(".modal-backdrop").css("display", "block");
        $(".sex").fadeIn("fast");
        that = $(this);
        var i = document.createElement("i")
        i.className = "iconfont seleced";
        i.innerHTML += "&#xe68e";
        if (that.find("em").text() == "男") {
            $(".man").append(i);
        } else {
            $("#girl").append(i);
        }
    }
    if ($(this).find("span").text() == "恋爱状态：") {
        var seImg = $(".user-select-love i");
        for (var l = 0; l < seImg.length; l++) {
            seImg.eq(l).remove();
        }

        $(".modal-backdrop").css("display", "block");
        $(".love-statu").fadeIn("fast");
        that = $(this);
        var i = document.createElement("i")
        i.className = "iconfont seleced";
        i.innerHTML += "&#xe68e";
        var option = $(".user-select-love").find("a");
        for (var n = 0; n < option.length; n++) {
            if (option.eq(n).text() == that.find("em").text()) {
                option.eq(n).parent(0).append(i);
            }
        }
    }
    if ($(this).find("span").text() == "班级：") {
        $(".person-info h4").text("班级");
        $(".person-info p").text("填写班级，加入集体");
        $(".person-text").val($(this).find("em").text());
        $(".modal-backdrop").css("display", "block");
        $(".person-info").fadeIn("fast");
        that = $(this);
    }
    if ($(this).find("span").text() == "宿舍：") {
        $(".person-info h4").text("宿舍");
        $(".person-info p").text("据说填了宿舍号，有可能收到惊喜快递");
        $(".person-text").val($(this).find("em").text());
        $(".modal-backdrop").css("display", "block");
        $(".person-info").fadeIn("fast");
        that = $(this);
    }
});

//关闭性别选择窗体
function offsex() {
    $(".modal-backdrop").css("display", "none");
    $(".sex").css("display", "none");
}

//关闭恋爱状态窗体
function offlove() {
    $(".modal-backdrop").css("display", "none");
    $(".love-statu").css("display", "none");
}

//方案高手信息
function offmerit() {
    $(".modal-backdrop").css("display", "none");
    $("#person-merit").css("display", "none");
}

$(function () {
    $(".discrip i").click(function () {
        //$(".users-name").val($("#user-name").text());
        //$(".user-merit").val($(".merit").text());
        $(".user-intro").val($("#user-intro").text());
        $(".modal-backdrop").css("display", "block");
        $("#person-merit").fadeIn("fast");
    });
});

//真实姓名保存或宿舍保存
function realNameSave() {
    if ($(".person-info h4").text() == "真实姓名") {
        var data = {};
        data.realName = $(".real-name").val();
        $.ajax({
            url: "/account/UpdateRealName",
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result.boo_success) {
                    $(".modal-backdrop").css("display", "none");
                    $(".person-info").css("display", "none");
                    that.find("em").text($(".real-name").val());
                }
                else {
                    alert("提交失败");
                }
            }
        });
    } else if ($(".person-info h4").text() == "班级") {
        var data = {};
        data.userClass = $(".real-name").val();
        $.ajax({
            url: "/account/UpdateUserClass",
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result.boo_success) {
                    $(".modal-backdrop").css("display", "none");
                    $(".person-info").css("display", "none");
                    that.find("em").text($(".real-name").val());
                }
                else {
                    alert("提交失败");
                }
            }
        });
    } else if ($(".person-info h4").text() == "宿舍") {
        var data = {};
        data.userDorm = $(".real-name").val();
        $.ajax({
            url: "/account/UpdateUserDorm",
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result.boo_success) {
                    $(".modal-backdrop").css("display", "none");
                    $(".person-info").css("display", "none");
                    that.find("em").text($(".real-name").val());
                }
                else {
                    alert("提交失败");
                }
            }
        });
    }
    
}

//个人信息的修改
$(function () {
    //更改性别
    $(".user-select-sex li").click(function () {
        var usersex = $(this).find("a").text();
        var data = {};
        data.userSex = usersex;
        $.ajax({
            url: "/account/UpdateUserSex",
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result.boo_success) {
                    $(".modal-backdrop").css("display", "none");
                    $(".sex").css("display", "none");
                    that.find("em").text(usersex);
                }
                else {
                    alert("提交失败");
                }
            }
        });
    });

    //更改恋爱状态
    $(".user-select-love li").click(function () {
        var userlove = $(this).find("a").text();
        var data = {};
        data.userLove = userlove;
        $.ajax({
            url: "/account/UpdateUserLove",
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result.boo_success) {
                    $(".modal-backdrop").css("display", "none");
                    $(".love-statu").css("display", "none");
                    that.find("em").text(userlove);
                }
                else {
                    alert("提交失败");
                }
            }
        });
    });
});

//更改个性签名
function superiorSave() {
    var data = {};
    data.userIntro = $(".user-intro").val();
    $.ajax({
        url: "/account/UpdateSperior",
        type: "post",
        dataType: "json",
        data: data,
        success: function (result) {
            if (result.boo_success) {
                $(".modal-backdrop").css("display", "none");
                $("#person-merit").css("display", "none");

                $("#user-intro").text($(".user-intro").val());
            }
            else {
                alert("提交失败");
            }
        }
    });
}