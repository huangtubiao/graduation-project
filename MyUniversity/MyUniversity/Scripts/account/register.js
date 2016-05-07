$(document).ready(function () {
    $(".modal-backdrop").css("display", "block");
    $("#school").click();
});

//注册窗体弹出
var school, department, startYear, education;
$(function () {
    var modalDiv = document.createElement("div");
    modalDiv.className = "backdrop-modalDiv modal-backdrop fade in";
    //学校选择
    $(".school-show ul li a").bind("click", function () {
        school = $(this).text();
        $(".close").click();
        //根据学校查询出相应院系
        GetDepartBySchool(school);
        $(".select-depart").click();
    });

    //院系选择
    $(".department-show ul li a").bind("click", function () {
        department = $(this).text();
        $(".close").click();
        $(".modal-backdrop").css("display", "none");

        if ($(".select-startYear").text() == "请选择入学年份") {
            $(".select-startYear").click();
        } else {
            return;
        }
    });
    
    //入学年份选择
    $(".year-show ul li a").bind("click", function () {
        startYear = $(this).text();
        $(".close").click();
        $(".modal-backdrop").css("display", "none");

        $(".select-school").text(school);
        $(".select-depart").text(department);
        $(".select-startYear").text(startYear);
    });

    $(".education-show ul li a").bind("click", function () {
        $(".select-degree").text($(this).text());
        $("#myModalEducation").css("display", "none");
        $(".modal-backdrop").css("display", "none");
    });

    //请选择学校
    $(".register-content li").click(function () {
        $(".modal-backdrop").css("display", "block");
    });

    //选择学历
    $(".myModalEducation").click(function () {
        $("#myModalEducation").addClass("in");
        $(".degree").fadeIn("fast");
    });

    //直接登录
    $("#to-login").click(function () {
        $(".phone-registered").removeClass("in");
        $(".phone-registered").css("display", "none");
        showlogin();
    });
    $("#change-phone").click(function () {
        closeRegisteredTip();
        $("#registerPhone").val("");
        $("#registerPhone").focus();
    });
});

//选择学校窗体显示的时候，查询要显示的数据（附近的学校）
$("#school").click(function () {
    $.get('/account/GetNearSchools', function (result) {
        var html = "";
        for (var i = 0; i < result.length; i++) {
            html += "<li>" + "<a>" + result[i] + "</a>" + "</li>";
        }
        $("#near-schools").empty();
        $("#near-schools").append(html);
    }, "json");
});

//返回学校上一步
function backSchool() {
    $("#myModalDepart .close").click();
    $("#school").click();
}

//返回院系上一步
function backDepart() {
    $("#myModalYear .close").click();
    $(".select-depart").click();
}

function backUrl() {
    alert("退出注册！");
}

//点击div外，div隐藏
$(document).bind("click", function (e) {
    var target = $(e.target);
    if (target[0].className == "degree modal fade in") {
        $(".modal-backdrop").css("display", "none");
        $(".degree").css("display", "none");
    }
});

$(function () {
    $(".first-step").bind("click", function () {
        $("#first-info").css("display", "none");
        $("#second-info").css("display", "block");

    });

    //返回上一步
    $("#second-info i").bind("click", function () {
        $("#second-info").css("display", "none");
        $("#first-info").css("display", "block");
    });
});

//注册
function btnRegister() {
    if ($("#registerPhone").val().length < 11 || $("#registerPhone").val().length > 11) {
        $(".modal-backdrop").css("display", "block");
        $(".effective-phone").addClass("in");
        $(".effective-phone").fadeIn("fast");
        return;
    }
    else if ($("#registerPaw").val().length < 6 || $("#registerPaw").val().length > 16) {
        $(".send-code-tip p").text("密码只能6-16位");
        $(".modal-backdrop").css("display", "block");
        $(".effective-phone").addClass("in");
        $(".effective-phone").fadeIn("fast");
        return;
    }
    $(".btn-reg a").text("注册中...");
    var data = {};
    data.userSchool = $(".select-school").text();
    data.userDepart = $(".select-depart").text();
    data.userStartYear = $(".select-startYear").text();
    data.userAccount = $("#registerPhone").val();
    data.userPaw = $("#registerPaw").val();
    $.ajax({
        url: "/account/register",
        type: "post",
        dataType: "json",
        data: data,
        success: function (result) {
            if (result.success) {
                window.location.href = "/projectsuperior/index";
            } else if (result.message == "手机号码已被注册") {
                $(".btn-reg a").text("注册");
                $(".modal-backdrop").css("display", "block");
                $(".phone-registered").addClass("in");
                $(".phone-registered").fadeIn("fast");
            }
        }
    });
}

function closeErrorTip() {
    $(".modal-backdrop").css("display", "none");
    $(".effective-phone").removeClass("in");
    $(".effective-phone").fadeOut("fast");
    if ($(".send-code-tip p").text() == "请输入有效的手机号码") {
        $("#registerPhone").focus();
    } else {
        $("#registerPaw").focus();
    }

}

function closeRegisteredTip() {
    $(".modal-backdrop").css("display", "none");
    $(".phone-registered").removeClass("in");
    $(".phone-registered").fadeOut("fast");
}

//搜索学校
var searchText = "";
var timer = null;
$("#search-write").bind("keyup", function (e) {
    if (e.keyCode != 40 && e.keyCode != 38 && e.keyCode != 13) {
        searchText = $("#search-write").val();
        if (timer) {
            clearTimeout(timer);
            timer = null;
        }
        timer = setTimeout(function () {
            $.get('/account/SchoolSearch?searchText=' + searchText, function (result) {
                var html = "";
                if (result == "") {
                    $("#search-school").hide();
                    $("#school-show").css("display", "block");
                    return;
                }
                for (var i = 0; i < result.length; i++) {
                    html += "<li>" + "<a>" + result[i] + "</a>" + "</li>";
                }
                $("#search-result").html(html);
                $("#school-show").css("display", "none");
                $("#search-school").show();
            }, "json");
        }, 300);
    }
});

//搜索院系
var searchDepart = "";
var timerr = null;
$("#write-depart").bind("keyup", function (e) {
    if (e.keyCode != 40 && e.keyCode != 38 && e.keyCode != 13) {
        searchDepart = $("#write-depart").val();
        if (timer) {
            clearTimeout(timer);
            timer = null;
        }
        timer = setTimeout(function () {
            $.get('/account/DepartSearch?searchDepart=' + searchDepart + "&mySchool=" + school, function (result) {
                var html = "";
                if (result == "") {
                    $("#search-depart").hide();
                    $("#depart-add").css("display", "block");
                    return;
                }
                for (var i = 0; i < result.length; i++) {
                    html += "<li>" + "<a>" + result[i] + "</a>" + "</li>";
                }
                $("#search-depart").html(html);
                $("#depart-add").css("display", "none");
                $("#search-depart").show();
            }, "json");
        }, 300);
    }
});

//学校搜索智能提醒点击搜索
$(document).delegate("#search-result li a", "click", function () {
    school = $(this).text();
    GetDepartBySchool(school);
    $(".close").click();
    $(".select-depart").click();
});
//根据学校查询院系的选择
$(document).delegate("#depart-add li a", "click", function () {
    department = $(this).text();
    $(".close").click();
    $(".modal-backdrop").css("display", "none");
    if ($(".select-startYear").text() == "请选择入学年份") {
        $(".select-startYear").click();
    } else {
        return;
    }
});
//搜索院系的选择
$(document).delegate("#search-depart li a", "click", function () {
    department = $(this).text();
    $(".close").click();
    $(".modal-backdrop").css("display", "none");
    if ($(".select-startYear").text() == "请选择入学年份") {
        $(".select-startYear").click();
    } else {
        return;
    }
});

//附近学校的选择
$(document).delegate("#near-schools li a", "click", function () {
    school = $(this).text();
    GetDepartBySchool(school);
    $(".close").click();
    $(".select-depart").click();
});

//根据学校查询出相应院系
function GetDepartBySchool(school) {
    $.get('/account/GetDepartBySchool?schoolName=' + school, function (result) {
        var html = "";
        for (var i = 0; i < result.length; i++) {
            html += "<li>" + "<a>" + result[i] + "</a>" + "</li>";
        }
        $("#depart-add").empty();
        $("#depart-add").append(html);
    }, "json");
}

//（封装）弹出提示窗体,使用》1、 layer.innerHTML = "登录成功！";（这里写要提示的信息）；2、 popup_windows();（弹出提示窗体）。
var layer = document.createElement("div");
layer.id = "layer";
function popupWindows() {
    var style = {
        background: "#cfeeea",
        color: "#53a375",
        borderRadius: "5px",
        position: "absolute",
        zIndex: 102,
        width: "280px",
        height: "130px",
        textAlign: "center",
        lineHeight: "130px",
        left: (window.innerWidth + window.scrollX - 200) / 2 + "px",
        top: ($(window).height() - 100) / 2 + $(document).scrollTop() + "px"
    };
    for (var i in style)
        layer.style[i] = style[i];
    if (document.getElementById("layer") == null) {
        document.body.appendChild(layer);
        setTimeout("document.body.removeChild(layer)", 2000)
    }
}