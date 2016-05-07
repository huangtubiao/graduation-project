var jsonObjF1 = null;   //大家庭
var jsonObjF2 = null;
var jsonObjF3 = null;
var jsonObjF4 = null;
$(document).ready(function () {
    var li = $("#notice-tit li");
    var url = request(3);
    if (url == 0 || url == undefined) {
        li.eq(0).addClass("cur");
    }
    if (url == 1) {
        li.eq(1).addClass("cur");
        $("#mod-1").css("display", "none");
        $("#mod-3").css("display", "none");
        $("#mod-2").css("display", "block");
    }
    else if (url == 2) {
        li.eq(2).addClass("cur");
        $("#mod-1").css("display", "none");
        $("#mod-2").css("display", "none");
        $("#mod-3").css("display", "block");
    }

    //获取大家庭的信息，并加载显示
    $.post('/projectsuperior/getFamilys', function (result) {
        jsonObjF1 = eval("(" + result[0] + ")");
        jsonObjF2 = eval("(" + result[1] + ")");
        jsonObjF3 = eval("(" + result[2] + ")");
        jsonObjF4 = eval("(" + result[3] + ")");
        loadFamilyImg();
    }, "json")
});

//当滚动条到达底部时，启动 getData() 函数。
$(window).scroll(function () {
    if ($(window).scrollTop() + $(window).height() == $(document).height()) {
        var notiveLi = $("#notice-tit li");
        if (notiveLi[1].className == "cur") {
            if (trackLoad.groupNumber <= mySchoolGroups && !trackLoad.loading) {
                trackLoad.loading = true;
                $(".loading").fadeIn("slow");
                setTimeout(function () {
                    getData(trackLoad, "校内");
                }, 1000);
            }
        }
        if (notiveLi[0].className == "cur") {
            if (trackLoad.allGroupNumber <= AllGroups && !trackLoad.loadingGirl) {
                trackLoad.loadingGirl = true;
                $(".loading-girl").fadeIn("slow");
                setTimeout(function () {
                    getData(trackLoad, "全部");
                }, 1000);
            }
        }
    }
});

//院系的点击搜索（大学搜索弹出窗体）
var nowActive;
var userDepartment = "全部院系";
var hadSelectDepartment = false;
$(function () {
    $("#search-depart").delegate("a", "click", function () {
        $(".department").css("display", "none");
        $(".modal-backdrop").css("display", "none");
        $(".select-depart").text($(this).text());
        var liActive = $("#notice-tit li");
        for (var i = 0; i <= liActive.length; i++) {
            if (liActive.eq(i).attr("class") == "cur") {
                nowActive = liActive.eq(i).children("a").text(); //是要查找校内的院系还是全部院系
            }
        }
        var data = {};
        data.userDepart = $(this).text();
        userDepartment = $(this).text();
        data.nowActive = nowActive;
        data.searchWrite = $("#search-write").val();
        data.userSchool = $(".select-shcool").text();
        $.ajax({
            url: "/projectsuperior/selectDepartment",
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result != null) {
                    if (nowActive == "校内") {
                        $("#mod-2").empty();
                        mySchoolGroups = Math.ceil(result[1] / 10);
                        trackLoad.groupNumber = 2;
                    }
                    else if (nowActive == "全部") {
                        $("#mod-1").empty();
                        AllGroups = Math.ceil(result[1] / 10);
                        trackLoad.allGroupNumber = 2;
                    }
                }
                hadSelectDepartment = true;
                showSuperior(result[0], nowActive, null); //加载相应院系的方案高手
            }
        });
    });
});

//学校的点击搜索（院系搜索弹出窗体）
var userSchool = "全部大学";
$(function () {
    $("#school-list").delegate("a", "click", function () {
        $("#select-school-pop").css("display", "none");
        $(".modal-backdrop").css("display", "none");
        $(".select-shcool").text($(this).text());
        var liActive = $("#notice-tit li");
        for (var i = 0; i <= liActive.length; i++) {
            if (liActive.eq(i).attr("class") == "cur") {
                nowActive = liActive.eq(i).children("a").text(); //是要查找校内的院系还是全部院系
            }
        }
        var data = {};
        data.userSchool = $(this).text();
        userSchool = $(this).text();
        data.nowActive = nowActive;
        data.searchWrite = $("#search-write").val();
        data.userDepart = $(".select-depart").text();
        $.ajax({
            url: "/projectsuperior/selectDepartment",
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result != null) {
                    if (nowActive == "校内") {
                        $("#mod-2").empty();
                        mySchoolGroups = Math.ceil(result[1] / 10);
                        trackLoad.groupNumber = 2;
                    }
                    else if (nowActive == "全部") {
                        $("#mod-1").empty();
                        AllGroups = Math.ceil(result[1] / 10);
                        trackLoad.allGroupNumber = 2;
                    }
                }
                hadSelectDepartment = true;
                showSuperior(result[0], nowActive, null); //加载相应院系的方案高手
            }
        });
    });
});

$(function () {
    //方案高手输入搜索框css效果
    $("#search-write").mouseover(function () {
        $(this)[0].placeholder = "";
    });
    $("#search-write").mouseout(function () {
        $(this)[0].placeholder = "请输入要寻找的方案高手，如：电脑维修高手";
    });
    $("#search-write").focus(function () {
        $(this)[0].placeholder = "";
    });
    $("#search-write").blur(function () {
        $(this)[0].placeholder = "请输入要寻找的方案高手，如：电脑维修高手";
    });
});

//加载并显示大家庭头像
var n = 0;
var t;
var q = 0;  // 用于加载第二个家庭头像item
var w = 0;  // 用于加载第三个家庭头像item  
var e = 0;  // 用于加载第四个家庭头像item
function loadFamilyImg() {
    if (n < 9) {
        createFamilyImg(jsonObjF1[n]);
        $(".family-first-item").append(lifamily);
        //隐藏》显示
        n = n + 1;
        if (n == 1) {
            $(lifamily).css("background", "#78ca99");
            $(lifamily).find("img").css("display", "none");
            $(lifamily).fadeIn("slow");
            t = setTimeout("loadFamilyImg()", 100);
        } else {
            $(lifamily).fadeIn("slow");
            t = setTimeout("loadFamilyImg()", 100);
        }


    } else if (n >= 9 && n < 18) {
        createFamilyImg(jsonObjF2[q]);
        $(".family-second-item").append(lifamily);
        //隐藏》显示
        q = q + 1;
        n = n + 1;
        if (n == 17) {
            $(lifamily).css("background", "#78ca99");
            $(lifamily).find("img").css("display", "none");
            $(lifamily).fadeIn("slow");
            t = setTimeout("loadFamilyImg()", 100);
        } else {
            $(lifamily).fadeIn("slow");
            t = setTimeout("loadFamilyImg()", 100);
        }
        return;
    } else if (n >= 18 && n < 27) {
        createFamilyImg(jsonObjF3[w]);
        $(".family-third-item").append(lifamily);
        //隐藏》显示
        w = w + 1;
        n = n + 1;
        if (n == 20) {
            $(lifamily).find("img").css("display", "none");
            $(lifamily).fadeIn("slow");
            t = setTimeout("loadFamilyImg()", 100);
        }
        if (n == 23) {
            $(lifamily).find("img").css("display", "none");
            $(lifamily).fadeIn("slow");
            t = setTimeout("loadFamilyImg()", 100);
        } if (n == 26) {
            $(lifamily).css("background", "#78ca99");
            $(lifamily).find("img").css("display", "none");
            $(lifamily).fadeIn("slow");
            t = setTimeout("loadFamilyImg()", 100);
        } else {
            $(lifamily).fadeIn("slow");
            t = setTimeout("loadFamilyImg()", 100);
        }
        return;
    } else if (n >= 27 && n < 30) {
        createFamilyImg(jsonObjF3[e]);
        $(".family-forth-item").append(lifamily);
        //隐藏》显示
        e = e + 1;
        n = n + 1;
        if (n == 28) {
            $(lifamily).css("background", "#78ca99");
            $(lifamily).find("img").css("display", "none");
            $(lifamily).fadeIn("slow");
            t = setTimeout("loadFamilyImg()", 100);
        } else {
            $(lifamily).fadeIn("slow");
            t = setTimeout("loadFamilyImg()", 100);
        }
        return;
    }
}

//创建大家庭相册头像展示
var lifamily = null;
function createFamilyImg(result) {
    lifamily = document.createElement("li");
    lifamily.className = "family-sub-item";

    var a = document.createElement("a");
    a.className = "family-avator";
    a.innerHTML += "<img src=" + result.userImg + ">";
    a.onclick = function d() {
        window.open('/projectsuperior/space/' + result.userId);
    }
    lifamily.appendChild(a);

    var div = document.createElement("div");
    div.className = "js-family-info family-info family-info-s";
    lifamily.appendChild(div);

    var h3 = document.createElement("h3");
    h3.className = "family-student-nick";
    h3.innerHTML += result.userName;
    div.appendChild(h3);

    var p = document.createElement("p");
    p.className = "family-info-job";
    p.innerHTML += result.userMerit;
    div.appendChild(p);
}

//大家庭头像鼠标经过显示相关信息
$(document).mouseover(function () {
    var lis = $(".family-avator");
    for (var i = 0; i < lis.length; i++) {
        lis[i].onmouseover = showDetail;
        lis[i].onmouseout = hideDetail;
    }
    function showDetail() {
        $(this).siblings(".js-family-info").css("display", "block");
    }
    function hideDetail() {
        $(this).siblings(".js-family-info").css("display", "none");
    }
});