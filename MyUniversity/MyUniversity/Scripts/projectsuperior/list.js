$(document).ready(function () {
    var range = requestUrlPara("range");
    if (range == "all") {
        $("#mod-1").css("display", "block");
    } else if(range == "my"){
        $("#mod-2").css("display", "block");
        $(".dropdown-toggle").find("span").text("校内");
        $(".now-show").text("全部");
    }

    //是否搜索到内容，有就显示“”
    if ($("#mod-1").children().length == 3) {
        $("#mod-1").find(".uncontent").css("display", "block");
    } else {
        $(".depart-superior").css("display", "block");
    }
    if ($("#mod-2").children().length == 3) {
        $("#mod-2").find(".uncontent").css("display", "block");
    } else {
        $(".depart-superior").css("display", "block");
    }

    //所搜索的词汇的搜索次数+1
    var data = {};
    data.word = $("#search-wrod").val();
    $.ajax({
        url: "/projectsuperior/superiorSearchNums",
        type: "post",
        dataType: "json",
        data: data,
        success: function (result) {
            if (result.boo_success) {
                return;
            }
            else {
                alert("系统出错");
            }
        }
    });
});

$(function () {
    //选择院系,已经显示的院系的选择
    $(".item-depart").click(function () {
        $("a").siblings(".item-depart").removeClass("selected");
        $(this).addClass("selected");
        var data = {};
        data.userDepart = $(this).text();
        userDepartment = $(this).text();
        data.nowActive = $(".dropdown-toggle span").text();
        data.searchWrite = $("#search-write").val();
        data.userSchool = $(".select-shcool").text();
        $.ajax({
            url: "/projectsuperior/selectDepartment",
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result != null) {
                    if ($(".dropdown-toggle span").text() == "校内") {
                        $("#mod-2").empty();
                        $("#mod-2").css("display", "none");
                        mySchoolGroups = Math.ceil(result[1] / 10);
                        trackLoad.groupNumber = 2;
                        hadSelectDepartment = true;
                        showSuperior(result[0], "校内", null);
                        $("#mod-2").fadeIn("fast");
                    } else {
                        $("#mod-1").empty();
                        $("#mod-1").css("display", "none");
                        AllGroups = Math.ceil(result[1] / 10);
                        trackLoad.allGroupNumber = 2;
                        hadSelectDepartment = true;
                        showSuperior(result[0], "全部", null);
                        $("#mod-1").fadeIn("fast");
                    }
                }
            }
        });
    });

    //选择查看校内或者全部的方案高手
    $(".dropdown-toggle").mouseover(function () {
        $(".range-select").css("display", "block");
        $(".searchtype_arrow").addClass("transform");
    });
    $(".dropdown-toggle").mouseout(function () {
        $(".range-select").css("display", "none");
        $(".searchtype_arrow").removeClass("transform");
    });
    $("#menu").mouseover(function () {
        $(".range-select").css("display", "block");
    });
    $("#menu").mouseout(function () {
        $(".range-select").css("display", "none");
    });
    $(".now-show").click(function () {
        var dropdown = $(this).text();
        var toggle = $(".dropdown-toggle span").text();
        $(".dropdown-toggle span").text(dropdown);
        $(this).text(toggle);
        $("#menu").css("display", "none");
        if (dropdown == "全部") {
            $("#mod-1").css("display", "none");
            $("#mod-2").fadeIn("fast");
        } else {
            $("#mod-2").css("display", "none");
            $("#mod-1").fadeIn("fast");
        }
    });

    //私信，判断是否已经登录
    $(".pri-letter").click(function () {
        var userId = $(this).siblings("input").val();
        var href = $(this).attr("href");
        $.post('/projectsuperior/checkIfLogin', function (result) {
            if (result.boo_success) {
                location.href = "/message/index/" + userId + "/s";
            } else {
                var toHref = "/message/index/" + userId + "/s";
                showlogin(toHref);
            }
        }, "json");
    });
});

//滚动条到达底部，数据加载更多的效果
$(window).scroll(function () {
    if ($(window).scrollTop() + $(window).height() == $(document).height()) {
        if ($(".dropdown-toggle span").text() == "校内") {
            if (trackLoad.groupNumber <= mySchoolGroups && !trackLoad.loading) {
                trackLoad.loading = true;      // Blocks other loading data again.
                $(".loading").fadeIn("slow");
                setTimeout(function () {
                    getData(trackLoad, "校内");
                }, 1000);
            }
        } else {
            if (trackLoad.allGroupNumber <= AllGroups && !trackLoad.loadingGirl) {
                trackLoad.loadingGirl = true;      // Blocks other loading data again.
                $(".loading-girl").fadeIn("slow");
                setTimeout(function () {
                    getData(trackLoad, "全部");
                }, 1000);
            }
        }
    }
});

//院系的选择搜索
var userDepartment = "全部院系";
var hadSelectDepartment = false;
$(function () {
    $(".department-show").delegate("a", "click", function () {
        $(".department").css("display", "none");
        $(".modal-backdrop").css("display", "none");
        var department = $(".item-depart");
        for (var i = 0; i < department.length; i++) {
            if (department.eq(i).text() == $(this).text()) {
                $("a").siblings(".item-depart").removeClass("selected");
                department.eq(i).addClass("selected");
            }
        }

        var data = {};
        data.userDepart = $(this).text();
        userDepartment = $(this).text();
        data.nowActive = "全部";
        data.searchWrite = $("#search-write").val();
        $.ajax({
            url: "/projectsuperior/selectDepartment",
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result != null) {
                    $("#mod-1").empty();
                    mySchoolGroups = Math.ceil(result[1] / 10);
                    trackLoad.groupNumber = 2;
                }
                hadSelectDepartment = true;
                showLovePeopels(result[0], "全部", null);
            }
        });
    });
});

//获取当前的URL参数
function requestUrlPara(paras) {
    var url = location.href;
    var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
    var paraObj = {}
    for (i = 0; j = paraString[i]; i++) {
        paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
    }
    var returnValue = paraObj[paras.toLowerCase()];
    if (typeof (returnValue) == "undefined") {
        return "";
    } else {
        return returnValue;
    }
}