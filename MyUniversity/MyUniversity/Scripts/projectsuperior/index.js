/**************方案高手各个模块的公共js******************/
$(document).ready(function () {
    //显示方案高手的性别
    var userSexs = $(".userSex");
    for (var i = 0; i < userSexs.length; i++) {
        if (userSexs.eq(i).val() == "男") {
            var iImg = userSexs.eq(i).prev();
            iImg.removeClass("girlsex-img");
            iImg.addClass("sex-icon");
            iImg.empty();
            iImg.html("&#xe608");
        }
    }

    //获取分页的总数量
    mySchoolGroups = Math.ceil($("#mySchoolGroups").val() / 10);
    AllGroups = Math.ceil($("#allGroups").val() / 10);

    //判断经验是否为0，不为0显示颜色
    var expNum = $(".ys .number");
    for (var i = 0; i < expNum.length; i++) {
        if (expNum.eq(i).text() > 0) {
            expNum.eq(i).parent(0).css("color", "#78CA99");
        }
    }

    //如果私信通知为零则不显示通知
    if ($(".superior-remind").text() != 0) {
        $(".superior-remind").css("display", "block");
    }

    //左边导航的选项卡
    var timer = null;
    var titles = $("#notice-tit").children("li");
    var divs = $("#notice-con").children(".mod");
    if (titles.length != divs.length)
        return;
    for (var i = 0; i < titles.length; i++) {
        titles[i].id = i;
        $("#" + titles[i].id).click(function () {

            var that = this;
            if (that.id == 1) {
                $.post('/projectsuperior/checkIfLogin', function (result) {
                    if (result.boo_success) {
                        for (var j = 0; j < titles.length; j++) {
                            $("#" + titles[j].id).attr("class", "");
                            $("#" + divs[j].id).css("display", "none");
                        }
                        $("#" + titles[that.id].id).addClass("cur");
                        $("#" + divs[that.id].id).fadeIn("fast");
                       
                        trackLoad.groupNumber = 1;
                        $("#mod-2 .wenda-list").remove();
                        getData(trackLoad, "校内");
                        //显示我所在的学校(定位)
                        $.post('/projectsuperior/getMySchoolName', function (result) {
                            $(".select-shcool").text(result);
                            $(".select-shcool").addClass("xiaonei");
                        }, "json");
                        //显示（校内或是全部标题）
                        $(".now-selected").text("校内");
                        //动态显示已经登录（导航条改为已经登录）
                        $.post('/projectsuperior/getMyInfo', function (result) {
                            $(".header-unlogin").empty();
                            $("#chat").css("display", "block");
                            $("#myImg").css("display", "block");
                            $("#myImg img").attr("src", result);
                            $(".header-unlogin").append($("#chat"));
                            $(".header-unlogin").append($("#myImg"));
                        }, "json");
                    } else {
                        showlogin("xiaonei");
                    }
                }, "json");
            } else {
                for (var j = 0; j < titles.length; j++) {
                    $("#" + titles[j].id).attr("class", "");
                    $("#" + divs[j].id).css("display", "none");
                }
                $("#" + titles[that.id].id).addClass("cur");
                $("#" + divs[that.id].id).fadeIn("fast");

                $(".select-shcool").text("全部大学");
                $(".select-shcool").removeClass("xiaonei");
                //显示（校内或是全部标题）
                $(".now-selected").text("全部");
            }
        });
    }

    //去看看
    $(".userMerit").click(function () {
        window.open('/projectsuperior/space/' + $(this).siblings("input").val());
    });

    //我的关注、私信，检查是否已经登录
    $(".private-letter").click(function () {
        if ($(this).text() == "关注") {
            var toHref = "/projectsuperior/follow";
        } else {
            var toHref = "/projectsuperior/letter";
        }
        $.post('/projectsuperior/checkIfLogin', function (result) {
            if (result.boo_success) {
                location.href = toHref;
            } else {
                showlogin(toHref);
            }
        }, "json");
    });

    //私信
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

    //案多多
    $("#project-more").click(function () {
        $.post('/projectsuperior/checkIfLogin', function (result) {
            if (result.boo_success) {
                location.href = "/projectsuperior/top";
            } else {
                var toHref = "/projectsuperior/top";
                showlogin(toHref);
            }
        }, "json");
    });

    //成为方案高手
    $(".to_become_sperior").click(function () {
        $.post('/projectsuperior/checkIfLogin', function (result) {
            if (result.boo_success) {
                location.href = "/projectsuperior/become";
            } else {
                var toHref = "/projectsuperior/become";
                showlogin(toHref);
            }
        }, "json");
    });
});

//左边导航菜单效果
$(function () {
    $(".menu-cur").css("top", $(".cur").position().top);
    $("#notice-tit li").hover(function () {
        $(".menu-cur").css("top", $(this).position().top);
    }, function () {
        $(".menu-cur").css("top", $(".cur").position().top);
    });
});

//显示院系的选择
function showDepartment() {
    $(".department").fadeIn("fast");
    $(".modal-backdrop").css("display", "block");
    if ($(".now-selected").text() == "全部") {
        $.post('/projectsuperior/getAllDepart', function (result) {
            var html = "";
            html += "<li>" + "<a>全部院系</a>" + "</li>";
            for (var i = 0; i < result.length; i++) {
                html += "<li>" + "<a>" + result[i] + "</a>" + "</li>";
            }
            $("#search-depart").empty();
            $("#search-depart").append(html);
        }, "json");
    } else {
        $.post('/projectsuperior/getMySchoolDepart', function (result) {
            var html = "";
            html += "<li>" + "<a>全部院系</a>" + "</li>";
            for (var i = 0; i < result.length; i++) {
                html += "<li>" + "<a>" + result[i] + "</a>" + "</li>";
            }
            $("#search-depart").empty();
            $("#search-depart").append(html);
        }, "json");
    }
    //list页面
    //if (requestUrlPara("range") == "all") {
    //    $.post('/projectsuperior/getAllDepart', function (result) {
    //        var html = "";
    //        html += "<li>" + "<a>全部院系</a>" + "</li>";
    //        for (var i = 0; i < result.length; i++) {
    //            html += "<li>" + "<a>" + result[i] + "</a>" + "</li>";
    //        }
    //        $("#search-depart").empty();
    //        $("#search-depart").append(html);
    //    }, "json");
    //} else {
    //    $.post('/projectsuperior/getMySchoolDepart', function (result) {
    //        var html = "";
    //        html += "<li>" + "<a>全部院系</a>" + "</li>";
    //        for (var i = 0; i < result.length; i++) {
    //            html += "<li>" + "<a>" + result[i] + "</a>" + "</li>";
    //        }
    //        $("#search-depart").empty();
    //        $("#search-depart").append(html);
    //    }, "json");
    //}
}

//显示学校的选择
function showSchool() {
    //判断是否校内的
    if ($(".now-selected").text() == "校内") {
        return;
    } else {
        $("#select-school-pop").fadeIn("fast");
        $(".modal-backdrop").css("display", "block");
        if ($(".now-selected").text() == "全部") {
            $.post('/projectsuperior/getAllSchool', function (result) {
                var html = "";
                html += "<li>" + "<a>全部学校</a>" + "</li>";
                for (var i = 0; i < result.length; i++) {
                    html += "<li>" + "<a>" + result[i] + "</a>" + "</li>";
                }
                $("#school-list").empty();
                $("#school-list").append(html);
            }, "json");
        } else {
            return;
        }
        if ($(".dropdown-toggle span").text() == "全部") {
            $.post('/projectsuperior/getAllSchool', function (result) {
                var html = "";
                html += "<li>" + "<a>全部学校</a>" + "</li>";
                for (var i = 0; i < result.length; i++) {
                    html += "<li>" + "<a>" + result[i] + "</a>" + "</li>";
                }
                $("#school-list").empty();
                $("#school-list").append(html);
            }, "json");
        } else {
            $.post('/projectsuperior/getMySchool', function (result) {
                var html = "";
                html += "<li>" + "<a>全部学校</a>" + "</li>";
                for (var i = 0; i < result.length; i++) {
                    html += "<li>" + "<a>" + result[i] + "</a>" + "</li>";
                }
                $("#school-list").empty();
                $("#school-list").append(html);
            }, "json");
        }
    }
}

//关闭院系的选择
function offDepartment() {
    $(".department").css("display", "none");
    $(".modal-backdrop").css("display", "none");
}

//关闭学校的选择
function offSchool() {
    $("#select-school-pop").css("display", "none");
    $(".modal-backdrop").css("display", "none");
}

//选择学校弹出窗》搜索学校
var searchText = "";
var timer = null;
$("#write-school").bind("keyup", function (e) {
    if (e.keyCode != 40 && e.keyCode != 38 && e.keyCode != 13) {
        searchText = $("#write-school").val();
        if (timer) {
            clearTimeout(timer);
            timer = null;
        }
        timer = setTimeout(function () {
            $.get('/account/SchoolSearch?searchText=' + searchText, function (result) {
                var html = "";
                if (result == "") {
                    $("#search-school").hide();
                    $("#school-list").css("display", "block");
                    return;
                }
                for (var i = 0; i < result.length; i++) {
                    html += "<li>" + "<a>" + result[i] + "</a>" + "</li>";
                }
                $("#school-result").html(html);
                $("#school-list").css("display", "none");
                $("#search-school").show();
            }, "json");
        }, 300);
    }
});

//学校搜索结果点击搜索
$(document).delegate("#school-result li a", "click", function () {
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

//院系搜索结果点击搜索
$(document).delegate("#depart-result li a", "click", function () {
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

//选择院系弹出窗》搜索院系
$("#write-depart").bind("keyup", function (e) {
    if (e.keyCode != 40 && e.keyCode != 38 && e.keyCode != 13) {
        searchText = $("#write-depart").val();
        if (timer) {
            clearTimeout(timer);
            timer = null;
        }
        timer = setTimeout(function () {
            $.get('/account/SearchDepart?searchText=' + searchText, function (result) {
                var html = "";
                if (result == "") {
                    $("#search-depart-list").hide();
                    $("#search-depart").css("display", "block");
                    return;
                }
                for (var i = 0; i < result.length; i++) {
                    html += "<li>" + "<a>" + result[i] + "</a>" + "</li>";
                }
                $("#depart-result").html(html);
                $("#search-depart").css("display", "none");
                $("#search-depart-list").show();
            }, "json");
        }, 300);
    }
});

//显示加载方案高手
function showSuperior(result, nowActive, loadMore) {
    var jsonObj = eval("(" + result + ")");
    for (var r in jsonObj) {
        var divList = document.createElement("div");
        divList.className = "wenda-list";
        if (loadMore == "加载更多") {
            if (nowActive == "校内") {
                $(".loading").before(divList);
            }
            else if (nowActive == "全部") {
                $(".loading-girl").before(divList);
            }
        }
        else if (loadMore == null) {
            if (nowActive == "校内") {
                $("#mod-2").append(divList);
            }
            else if (nowActive == "全部") {
                $("#mod-1").append(divList);
            }
        }
        var divListcon = document.createElement("div");
        divListcon.className = "wenda-listcon";
        divList.appendChild(divListcon);

        var divHead = document.createElement("div");
        divHead.className = "headslider";
        divListcon.appendChild(divHead);

        var a = document.createElement("a");
        a.className = "superior-head";
        a.href = "/account/person/" + jsonObj[r].userId;
        a.innerHTML += "<img src=" + jsonObj[r].userImg + ">"
        divHead.appendChild(a);

        var aNickname = document.createElement("a");
        aNickname.className = "wenda-nickname";
        aNickname.href += "/account/person/" + jsonObj[r].userId;
        var i = document.createElement("i");

        if (jsonObj[r].userSex == "女") {
            i.className = "iconfont girlsex-img";
            i.innerHTML += "&#xe639";
        }
        else {
            i.className = "iconfont sex-icon";
            i.innerHTML += "&#xe608";
        }
        aNickname.innerHTML += jsonObj[r].userName;
        aNickname.appendChild(i);
        divHead.appendChild(aNickname);

        var divSlider = document.createElement("div");
        divSlider.className = "wendaslider";
        divListcon.appendChild(divSlider);

        var aExperience = document.createElement("a");
        aExperience.className = "experience-nums";
        aExperience.href = "/projectsuperior/space/" + jsonObj[r].userId;
        divSlider.appendChild(aExperience);

        var divYs = document.createElement("div");
        if (jsonObj[r].userRank > 0) {
            divYs.className = "ys green";
        }
        else {
            divYs.className = "ys";
        }
        aExperience.appendChild(divYs);

        var divNumber = document.createElement("div");
        divNumber.className = "number";
        divNumber.innerHTML += jsonObj[r].userRank;
        divYs.appendChild(divNumber);

        var div = document.createElement("div");
        div.innerHTML += "经验";
        divYs.appendChild(div);

        var divBrowsenum = document.createElement("div");
        divBrowsenum.className = "browsenum";
        aExperience.appendChild(divBrowsenum);

        var divNumber = document.createElement("div");
        divNumber.className = "number";
        divNumber.innerHTML += jsonObj[r].userVisitedNum;
        divBrowsenum.appendChild(divNumber);

        var div = document.createElement("div");
        div.innerHTML += "访问";
        divBrowsenum.appendChild(div);

        var divAbility = document.createElement("div");
        divAbility.className = "ability";
        divSlider.appendChild(divAbility);

        var iMerit = document.createElement("i");
        iMerit.className = "iconfont";
        iMerit.innerHTML += "&#xe63f";
        divAbility.appendChild(iMerit);

        var a = document.createElement("a");
        a.className = "userMerit";
        a.onclick = function d() {
            window.open('/projectsuperior/space/' + jsonObj[r].userId);
        }
        a.innerHTML += jsonObj[r].userMerit;
        divAbility.appendChild(a);

        var questionDiv = document.createElement("div");
        questionDiv.className = "question";
        divSlider.appendChild(questionDiv);

        var i = document.createElement("i");
        i.className = "iconfont";
        i.innerHTML += "&#xe62c";
        questionDiv.appendChild(i);

        var replydesDiv = document.createElement("div");
        replydesDiv.className = "replydes";
        questionDiv.appendChild(replydesDiv);

        var replycontentSpan = document.createElement("span");
        replycontentSpan.className = "replycontent";
        replycontentSpan.innerHTML += jsonObj[r].userMeritIntro;
        replydesDiv.appendChild(replycontentSpan);

        var divDepart = document.createElement("div");
        divDepart.className = "department-rank";
        divSlider.appendChild(divDepart);

        var aSchool = document.createElement("a");
        aSchool.innerHTML += jsonObj[r].userSchool;
        divDepart.appendChild(aSchool);

        var aDepart = document.createElement("a");
        aDepart.className = "depart";
        aDepart.innerHTML += jsonObj[r].userDepart;
        divDepart.appendChild(aDepart);

        var aTag = document.createElement("a");
        aTag.className = "list-tag pri-letter";
        divDepart.appendChild(aTag);
        var iHelp = document.createElement("i");
        iHelp.className = "iconfont";
        iHelp.innerHTML += "&#xe63b";
        aTag.appendChild(iHelp);
        aTag.innerHTML += "私信";

        var input = document.createElement("input");
        input.type = "hidden";
        input.value = jsonObj[r].userId;
        divDepart.appendChild(input);
    }
    if (hadSelectDepartment) {
        addLoadingImg(nowActive);
    }
}
//为数据列表添加加载的图片
function addLoadingImg(nowActive) {
    var divLoading = document.createElement("div");
    var iLoading = document.createElement("i");
    iLoading.className = "iconfont";
    iLoading.innerHTML += "&#xe643";
    divLoading.appendChild(iLoading);
    if (nowActive == "校内") {
        divLoading.className = "loading";
        $("#mod-2").append(divLoading);
    }
    if (nowActive == "全部") {
        divLoading.className = "loading-girl";
        $("#mod-1").append(divLoading);
    }
    hadSelectDepartment = false;
}

//智能搜索提醒
var searchText = "";
var timer = null;
var img;
$("#search-write").bind("keyup", function (e) {
    if (e.keyCode != 40 && e.keyCode != 38 && e.keyCode != 13) {
        searchText = $("#search-write").val();
        if (timer) {
            clearTimeout(timer);
            timer = null;
        }
        timer = setTimeout(function () {
            $.get('/projectsuperior/searchSuggest?searchText=' + searchText, function (result) {
                var html = "";
                if (result == "") {
                    $("#search-suggest").hide();
                    return;
                }
                img = "<i class=" + "iconfont" + ">" + "&#xe642" + "</i>";
                for (var i = 0; i < result.length; i++) {
                    html += "<li>" + img + "<a>" + result[i] + "</a>" + "</li>";
                }
                $("#search-result").html(html);
                $("#search-suggest").find("span").text(searchText);
                $("#search-suggest").show();
            }, "json");
        }, 300);
    }
});
//搜索》键盘上下键控制
var index = -1;
document.documentElement.onkeydown = function (e) {
    e = window.event || e;
    if (e.keyCode == 40) {   //下键操作
        if (++index == $("#search-result li").length) { //判断加一操作后index值是否超出列表数目界限
            index = -1; //超出的话就将index值变为初值
            $("#search-write").val(searchText); //并将文本框中的值设为用户用于搜索的值
            $("#search-result li").removeClass("resultLiHover");
        }
        else {
            $("#search-write").val($($("#search-result li a")[index]).text());
            $($("#search-result li")[index]).siblings().removeClass("resultLiHover").end().addClass("resultLiHover");
        }
    }
    if (e.keyCode == 38) {  //上键操作
        if (--index == -1) {  //判断自减后是否已移到文本框
            $("#search-write").val(searchText);
            $("#search-result li").removeClass("resultLiHover");
        }
        else if (index == -2) {  //判断index值是否超出列表数目界限
            index = $("#search-result li").length - 1;
            $("#search-write").val($($("#search-result li a")[index]).text());
            $($("#search-result li")[index]).siblings().removeClass("resultLiHover").end().addClass("resultLiHover");
        }
        else {
            $("#search-write").val($($("#search-result li a")[index]).text());
            $($("#search-result li")[index]).siblings().removeClass("resultLiHover").end().addClass("resultLiHover");
        }
    }
}

$(document).bind("click", function () {
    $("#search-suggest").hide();
});
//搜索智能提醒点击搜索
$(document).delegate("#search-result li", "click", function () {
    var keyword = $(this).children("a").text();
    $("#search-write").attr("value", keyword);
    $("#search").click();
});

//点击搜索
function btnSearch() {
    var li = $("#notice-tit li");
    if (li.length == 0) {
        if ($(".dropdown-toggle").find("span").text() == "全部") {
            var range = "all";
        } else {
            var range = "my";
        }
    }
    for (var i = 0; i < li.length; i++) {
        if (li.eq(i).hasClass("cur")) {
            if (li.eq(i).find(".nav-title").text() == "全部") {
                var range = "all";
            } else {
                var range = "my";
            }
        }
    }
    if ($("#search-write").val() == "") {
        location.href = "/projectsuperior/index";
    }
    else {
        location.href = '/projectsuperior/list?search=' + $("#search-write").val() + '&range=' + range;
    }
}

//点击回车搜索
function searchByBeatEnter() {
    btnSearch();
    $("#search-suggest").css("display", "none");
}

//滚动条到达底部，数据加载更多的效果
var mySchoolGroups;
var AllGroups;
var trackLoad = new Object();
trackLoad.groupNumber = 2;
trackLoad.allGroupNumber = 2;
trackLoad.loading = false;
trackLoad.loadingGirl = false;

function getData(options, whatOnactive) {   //whatOnactive=校内或全部
    var data = {};
    if (whatOnactive == "校内") {
        data.groupNumber = options.groupNumber;
    }
    else if (whatOnactive == "全部") {
        data.groupNumber = options.allGroupNumber;
    }
    data.whatOnactive = whatOnactive;
    data.userDepartment = userDepartment;
    data.userSchool = userSchool;
    data.searchWrite = $("#search-write").val();
    $.ajax({
        url: "/projectsuperior/getData",
        type: "post",
        dataType: "json",
        data: data,
        success: function (result) {
            if (result != null) {
                showSuperior(result, whatOnactive, "加载更多");
                if (whatOnactive == "校内") {
                    $(".loading").fadeOut("slow");
                    options.groupNumber++;
                    options.loading = false;
                }
                else if (whatOnactive == "全部") {
                    $(".loading-girl").fadeOut("slow");
                    options.allGroupNumber++;
                    options.loadingGirl = false;
                }
            }
            else {
                return;
            }
        }
    });
}

//热门搜索
var hotSearch = null;
$(document).delegate(".panel-body .hot-label", "click", function () {
    $("#search-write").val($(this).text());
    btnSearch();
});


