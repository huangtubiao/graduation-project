$(document).ready(function () {
    tabs();
    totalGroups = Math.ceil($("#allPlans").val() / 5);
    totalSecondGroups = Math.ceil($("#supericePlans").val() / 5);

    //成功完成计划排名
    var padtop = $(".padtop");
    padtop.eq(0).find(".rankingnum").html("&#xe615");
    padtop.eq(0).find(".span-num").text("1");

    padtop.eq(1).find(".rankingnum").html("&#xe615");
    padtop.eq(1).find(".rankingnum").addClass("two");
    padtop.eq(1).find(".span-num").text("2");

    padtop.eq(2).find(".rankingnum").html("&#xe615");
    padtop.eq(2).find(".rankingnum").addClass("three");
    padtop.eq(2).find(".span-num").text("3");

    //右边的排名列表
    var userSex = $(".user-sex");
    for (var i = 0; i < userSex.length; i++) {
        if (userSex.eq(i).val() == "男") {
            userSex.eq(i).parent(0).find(".sex").addClass("sex-icon");
            userSex.eq(i).parent(0).find(".sex").html("&#xe608");
        }
        else if (userSex.eq(i).val() == "女") {
            userSex.eq(i).parent(0).find(".sex").addClass("girlsex-img");
            userSex.eq(i).parent(0).find(".sex").html("&#xe609");
        }
    }

    //是否评论或加油，有就改变颜色
    var ifReply = $(".if-reply");
    for (var i = 0; i < ifReply.length; i++) {
        if (ifReply.eq(i).find("em").text() > 0) {
            ifReply.eq(i).css("color", "#78CA99");
        }
    }
});

//获取最新、最热的计划监督
var hadSelectNewOrHot = false;
$(document).delegate(".second-select", "click", function () {
    trackLoad.groupNumber = 2;
    trackLoad.groupSecondNumber = 2;

    $(this).siblings("a").removeClass("onactive");
    $(this).addClass("onactive");

    var li = $("#notice-tit li");
    var active;
    for (var i = 0; i < li.length; i++) {
        if (li[i].className == "onactive") {
            active = li.eq(i).children().text();
        }
    }
    var data = {};
    data.groupNumber = 1;
    data.hotOrNew = $(this).text();
    data.active = active;
    $.ajax({
        url: "/plan/getNewHotPlan",
        type: "post",
        dataType: "json",
        data: data,
        success: function (result) {
            if (result != null) {
                if (active == "全部计划") {
                    $("#mod-1").hide();
                    $("#mod-1").empty();
                    showAllPlans(result, null);
                    addLoadingImg(active);  //添加底部加载图片
                    $("#mod-1").fadeIn("fast");
                }
                else if (active == "我监督的") {
                    $("#my-supervice").empty();
                    $("#my-supervice").fadeOut("fast");
                    showMySupervicePlans(result, null);
                    addLoadingImg(active);  //添加底部加载图片
                    $("#my-supervice").fadeIn("fast");
                }
            }
        }
    });
});

//显示全部计划（最热或最热）的信息
function showAllPlans(result, loadMore) {
    var jsonObj = eval("(" + result + ")");
    for (var r in jsonObj) {
        var divList = document.createElement("div");
        divList.className = "wenda-list";

        if (loadMore == "加载更多") {
            $(".loading").before(divList);
        }
        else if (loadMore == null) {
            $("#mod-1").append(divList);
        }

        var divListcon = document.createElement("div");
        divListcon.className = "wenda-listcon";
        divList.appendChild(divListcon);

        var divHead = document.createElement("div");
        divHead.className = "headslider";
        divListcon.appendChild(divHead);

        var a = document.createElement("a");
        a.className = "wenda-head";
        a.innerHTML += "<img src=" + jsonObj[r].userImg + ">"
        divHead.appendChild(a);

        var aNickname = document.createElement("a");
        aNickname.className = "wenda-nickname";
        aNickname.href += "/account/person/" + "5";
        aNickname.innerHTML += jsonObj[r].userName;
        divHead.appendChild(aNickname);

        var i = document.createElement("i");
        if (jsonObj[r].userSex == "女") {
            i.className = "iconfont girlsex-img margin-left";
            i.innerHTML += "&#xe639";
        }
        else {
            i.className = "iconfont sex-icon margin-left";
            i.innerHTML += "&#xe608";
        }
        divHead.appendChild(i);

        var divWendaslider = document.createElement("div");
        divWendaslider.className = "wendaslider";
        divListcon.appendChild(divWendaslider);

        var divWendaquetitle = document.createElement("div");
        divWendaquetitle.className = "wendaquetitle";
        divWendaslider.appendChild(divWendaquetitle);

        var i = document.createElement("i");
        i.className = "iconfont";
        i.innerHTML += "&#xe649";
        divWendaquetitle.appendChild(i);

        var divWendatitlecon = document.createElement("div");
        divWendatitlecon.className = "wendatitlecon";
        divWendaquetitle.appendChild(divWendatitlecon);

        var a = document.createElement("a");
        a.innerHTML += jsonObj[r].planTilte;
        divWendatitlecon.appendChild(a);

        var divQuestion = document.createElement("div");
        divQuestion.className = "question";
        divWendaslider.appendChild(divQuestion);

        var divReplydes = document.createElement("div");
        divReplydes.className = "replydes"
        divReplydes.innerHTML += jsonObj[r].planContent;
        divQuestion.appendChild(divReplydes);

        var divCommentfooter = document.createElement("div");
        divCommentfooter.className = "commentfooter";
        divWendaslider.appendChild(divCommentfooter);

        var spanFooterdepart = document.createElement("span");
        spanFooterdepart.className = "footer-depart";
        spanFooterdepart.innerHTML += jsonObj[r].userDepart;
        divCommentfooter.appendChild(spanFooterdepart);

        var divWendatime = document.createElement("div");
        divWendatime.className = "wenda-time";
        divCommentfooter.appendChild(divWendatime);

        var em = document.createElement("em");
        em.innerHTML += "开始时间：" + jsonObj[r].planPublishedTime;
        divWendatime.appendChild(em);

        var divListFooter = document.createElement("div");
        divListFooter.className = "list-footer";
        divListcon.appendChild(divListFooter);

        var aKeyView = document.createElement("a");
        aKeyView.className = "key-word";
        aKeyView.innerHTML += "浏览：" + jsonObj[r].planViewNum;
        divListFooter.appendChild(aKeyView);

        var aKeyComm = document.createElement("a");
        if (jsonObj[r].planCommentNum > 0) {
            aKeyComm.className = "key-word green";
        }
        else {
            aKeyComm.className = "key-word";
        }
        aKeyComm.innerHTML += "评论：" + "<em>" + jsonObj[r].planCommentNum + "</em>";
        divListFooter.appendChild(aKeyComm);

        var aKeyComeOn = document.createElement("a");
        if (jsonObj[r].planComeOnNum > 0) {
            aKeyComeOn.className = "key-word green";
        }
        else {
            aKeyComeOn.className = "key-word";
        }
        aKeyComeOn.innerHTML += "加油：" + "<em>" + jsonObj[r].planComeOnNum + "</em>";
        divListFooter.appendChild(aKeyComeOn);

        var aComeOn = document.createElement("a");
        aComeOn.className = "come-on";
        aComeOn.innerHTML += "监督TA";
        divListFooter.appendChild(aComeOn);

        var spanSupevise = document.createElement("span");
        spanSupevise.className = "people-supevise";
        spanSupevise.innerHTML += "人正在监督";
        divListFooter.appendChild(spanSupevise);

        var spanSupeviseNum = document.createElement("span");
        spanSupeviseNum.innerHTML += jsonObj[r].planSuperviseNum;
        divListFooter.appendChild(spanSupeviseNum);
    }
}

//显示我监督的计划（最热或最热）的信息
function showMySupervicePlans(result, loadMore) {
    var jsonObj = eval("(" + result + ")");
    for (var r in jsonObj) {
        var divList = document.createElement("div");
        divList.className = "wenda-list my-supervice";

        if (loadMore == "加载更多") {
            $(".loading-second").before(divList);
        }
        else if (loadMore == null) {
            $("#my-supervice").append(divList);
        }

        var divListcon = document.createElement("div");
        divListcon.className = "wenda-listcon";
        divList.appendChild(divListcon);

        var divTime = document.createElement("div");
        divTime.className = "time";
        divTime.innerHTML += jsonObj[r]._superviceTime;
        divList.appendChild(divTime);
        var iTime = document.createElement("i");
        iTime.className = "iconfont iTime";
        iTime.innerHTML += "&#xe64a";
        divList.appendChild(iTime);

        var divHead = document.createElement("div");
        divHead.className = "headslider";
        divListcon.appendChild(divHead);

        var a = document.createElement("a");
        a.className = "wenda-head";
        a.innerHTML += "<img src=" + jsonObj[r].userImg + ">"
        divHead.appendChild(a);

        var aNickname = document.createElement("a");
        aNickname.className = "wenda-nickname";
        aNickname.href += "/account/person/" + "5";
        aNickname.innerHTML += jsonObj[r].userName;
        divHead.appendChild(aNickname);

        var i = document.createElement("i");
        if (jsonObj[r].userSex == "女") {
            i.className = "iconfont girlsex-img";
            i.innerHTML += "&#xe639";
        }
        else {
            i.className = "iconfont sex-icon";
            i.innerHTML += "&#xe608";
        }
        divHead.appendChild(i);

        var divWendaslider = document.createElement("div");
        divWendaslider.className = "wendaslider";
        divListcon.appendChild(divWendaslider);

        var divWendaquetitle = document.createElement("div");
        divWendaquetitle.className = "wendaquetitle";
        divWendaslider.appendChild(divWendaquetitle);

        var i = document.createElement("i");
        i.className = "iconfont";
        i.innerHTML += "&#xe649";
        divWendaquetitle.appendChild(i);

        var divWendatitlecon = document.createElement("div");
        divWendatitlecon.className = "wendatitlecon";
        divWendaquetitle.appendChild(divWendatitlecon);

        var a = document.createElement("a");
        a.innerHTML += jsonObj[r].planTilte;
        divWendatitlecon.appendChild(a);

        var divQuestion = document.createElement("div");
        divQuestion.className = "question";
        divWendaslider.appendChild(divQuestion);

        var divReplydes = document.createElement("div");
        divReplydes.className = "replydes"
        divReplydes.innerHTML += jsonObj[r].planContent;
        divQuestion.appendChild(divReplydes);

        var divCommentfooter = document.createElement("div");
        divCommentfooter.className = "commentfooter";
        divWendaslider.appendChild(divCommentfooter);

        var spanFooterdepart = document.createElement("span");
        spanFooterdepart.className = "footer-depart";
        spanFooterdepart.innerHTML += jsonObj[r].userDepart;
        divCommentfooter.appendChild(spanFooterdepart);

        var divWendatime = document.createElement("div");
        divWendatime.className = "wenda-time";
        divCommentfooter.appendChild(divWendatime);

        var em = document.createElement("em");
        em.innerHTML += "开始时间：" + jsonObj[r].planPublishedTime;
        divWendatime.appendChild(em);

        var span = document.createElement("span");
        span.innerHTML += "你和" + (jsonObj[r].planSuperviseNum - 1) + "人在监督TA";
        divWendatime.appendChild(span);

        var divListFooter = document.createElement("div");
        divListFooter.className = "list-footer";
        divListcon.appendChild(divListFooter);

        var aComeOn = document.createElement("a");
        aComeOn.className = "come-on";
        aComeOn.innerHTML += "督促TA";
        divListFooter.appendChild(aComeOn);

        var spanSupevise = document.createElement("span");
        spanSupevise.className = "people-supevise";
        spanSupevise.innerHTML += "今天TA还没完成打卡日志哦";
        divListFooter.appendChild(spanSupevise);
    }
}

//添加底部加载图片
function addLoadingImg(active) {
    var divLoading = document.createElement("div");
    var iLoading = document.createElement("i");
    iLoading.className = "iconfont";
    iLoading.innerHTML += "&#xe643";
    divLoading.appendChild(iLoading);
    if (active == "全部计划") {
        divLoading.className = "loading";
        $("#mod-1").append(divLoading);
    }
    if (active == "我监督的") {
        divLoading.className = "loading-second";
        $("#mod-2").append(divLoading);
    }
}

var totalGroups;
var totalSecondGroups;
var trackLoad = new Object();
trackLoad.groupNumber = 2;
trackLoad.groupSecondNumber = 2;
trackLoad.loading = false;
trackLoad.loadingSecond = false;
//滚动条滚到底部数据加载更多
$(window).scroll(function () {
    if ($(window).scrollTop() + $(window).height() == $(document).height()) {
        var notiveLi = $("#notice-tit li");
        if (notiveLi[0].className == "onactive") {
            if (trackLoad.groupNumber <= totalGroups && !trackLoad.loading) {
                trackLoad.loading = true;      // Blocks other loading data again.
                $(".loading").fadeIn("slow");
                setTimeout(function () {
                    getData(trackLoad, "全部计划");
                }, 1000);
            }
        }
        else if (notiveLi[1].className == "onactive") {
            if (trackLoad.groupSecondNumber <= totalSecondGroups && !trackLoad.loadingSecond) {
                trackLoad.loadingSecond = true;      // Blocks other loading data again.
                $(".loading-second").fadeIn("slow");
                setTimeout(function () {
                    getData(trackLoad, "我监督的");
                }, 1000);
            }
        }
    }
});

//获取数据
function getData(options, Onactive) {
    var data = {};
    if (Onactive == "全部计划") {
        data.groupNumber = options.groupNumber;
    }
    else if (Onactive == "我监督的") {
        data.groupNumber = options.groupSecondNumber;
    }
    data.active = Onactive;
    var newHot = $(".second-select");
    for (var i = 0; i < newHot.length; i++) {
        if (newHot[i].className == "second-select onactive") {
            var hotOrNew = newHot.eq(i).text();
        }
    }
    data.hotOrNew = hotOrNew;
    $.ajax({
        url: "/plan/getNewHotPlan",
        type: "post",
        dataType: "json",
        data: data,
        success: function (result) {
            if (result != null) {
                if (Onactive == "全部计划") {
                    $(".loading").fadeOut("slow");
                    showAllPlans(result, "加载更多");
                    options.groupNumber++;
                    options.loading = false;
                }
                else if (Onactive == "我监督的") {
                    $(".loading-second").fadeOut("slow");
                    showMySupervicePlans(result, "加载更多");
                    options.groupSecondNumber++;
                    options.loadingSecond = false;
                }
            }
            else {
                return;
            }
        }
    });
}

//不显示已经完成的计划
$(function () {
    $(".blank-finished").click(function () {
        if ($(this).hasClass("checked")) {    //显示已完成的计划
            $(this).removeClass("checked");
            var data = {};
            data.ifShowFinish = "true";
            $.ajax({
                url: "/plan/noShowFinishPlan",
                type: "post",
                dataType: "json",
                data: data,
                success: function (result) {
                    if (result != null) {
                        $("#my-supervice").empty();
                        $("#my-supervice").fadeOut("fast");
                        showMySupervicePlans(result, null);
                        addLoadingImg(active);  //添加底部加载图片
                        $("#my-supervice").fadeIn("fast");
                    }
                }
            });
        }
        else {    //不显示已经完成的计划
            $(this).addClass("checked");
            var data = {};
            data.ifShowFinish = "false";
            $.ajax({
                url: "/plan/noShowFinishPlan",
                type: "post",
                dataType: "json",
                data: data,
                success: function (result) {
                    if (result != null) {
                        $("#my-supervice").empty();
                        $("#my-supervice").fadeOut("fast");
                        showMySupervicePlans(result, null);
                        addLoadingImg(active);  //添加底部加载图片
                        $("#my-supervice").fadeIn("fast");
                    }
                }
            });
        }

    });
});

//加油弹出窗体
var planId;
$(function () {
    $(".cheerforhim").click(function () {
        var cheerNum = $(this).find("em").text();
        var that = $(this).find("em");
        planId = $(this).parent(0).find("input").val();

        var data = {};
        data.planId = planId;
        $.ajax({
            url: "/plan/checkIfCheered",
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result.boo_success) {
                    that.text(parseInt(cheerNum) + 1);
                    var _data = {};
                    _data.planId = planId;
                    $.ajax({
                        url: "/plan/cheer",
                        type: "post",
                        dataType: "json",
                        data: _data,
                        success: function (result) {
                            if (result.boo_success) {
                                $("#cheer").fadeIn("fast");
                                $(".modal-backdrop").css("display", "block");
                            }
                            else {
                                return;
                            }
                        }
                    });
                }
                else {
                    return;
                }
            }
        });
    });
    //添加加油内容
    $(".cheer-btn").click(function () {
        var timer = null;
        var data = {};
        data.planComeOnWrite = $(".cheer-write").val();
        data.planId = planId;
        $.ajax({
            url: "/plan/updatePlanComeOn",
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result.boo_success) {
                    $(".i-success").fadeIn("fast");
                    if (timer) {
                        clearTimeout(timer);
                        timer = null;
                    }
                    timer = setTimeout(function () {
                        $("#cheer").css("display", "none");
                        $(".modal-backdrop").css("display", "none");
                        $(".i-success").css("display", "none");
                    }, 1000);
                }
                else {
                    return;
                }
            }
        });
    });
    //显示最近加油的人
    var timer = null;
    $(".cheerforhim").mouseover(function (event) {
        var that = $(this);
        if (that.find(".popover")[0] == undefined) {
            if (timer) {
                clearTimeout(timer);
                timer = null;
            }
            timer = setTimeout(function () {
                //获取最近加油的数据
                var data = {};
                data.planId = that.parent(0).find("input").val();
                $.ajax({
                    url: "/plan/getNewlyCheerUser",
                    type: "post",
                    dataType: "json",
                    data: data,
                    success: function (result) {
                        if (result != null) {
                            var divPopover = document.createElement("div");
                            divPopover.className = "popover";
                            that.append(divPopover);

                            var divTite = document.createElement("div");
                            divTite.className = "popover-title";
                            divPopover.appendChild(divTite);

                            var h = document.createElement("h3");
                            h.innerHTML += "最近加油过的同学";
                            divTite.appendChild(h);

                            var a = document.createElement("a");
                            a.href = "/plan/cheerforplan/" + that.parent(0).find("input").val();
                            a.innerHTML += "更多";
                            divTite.appendChild(a);

                            var divContent = document.createElement("div");
                            divContent.className = "popover-content";
                            divPopover.appendChild(divContent);

                            var jsonObj = eval("(" + result + ")");
                            for (var r in jsonObj) {
                                var aUser = document.createElement("a");
                                aUser.href = "/account/person/" + jsonObj[r].userId;
                                aUser.innerHTML += "<img src=" + jsonObj[r].userImg + ">";
                                divContent.appendChild(aUser);
                            }
                            that.find(".popover").fadeIn("fast");
                        }
                        else {
                            return;
                        }
                    }
                });
            }, 800);
            
        }
        else if (that.find(".popover")[0] != null) {
            that.find(".popover").css("display", "block");
        }
    });

    $(".popover").mouseover(function () {
        $(this).css("display", "block");
    });
    $(".popover").mouseout(function () {
        $(this).css("display", "none");
    });
    $(".cheerforhim").mouseout(function () {
        clearTimeout(timer);
        $(this).find(".popover").css("display", "none");
    });
});
function offcheer() {
    $("#cheer").css("display", "none");
    $(".modal-backdrop").css("display", "none");
}
function cheerByBeatEnter() {
    $(".cheer-btn").click();
}

//浏览和评论计划
$(function () {
    $(".plan-view").click(function () {
        var planId = $(this).parent(0).find("input").val();
        var data = {};
        data.planId = planId;
        $.ajax({
            url: "/plan/addPlanView",
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result.boo_success) {
                    location.href = "/plan/detail/" + planId;
                }
                else {
                    return;
                }                                                                                                   
            }
        });
    });
    $(".plan-comment").click(function () {
        location.href = "/plan/detail/" + $(this).parent(0).find("input").val();
    });
});

//监督弹出窗体
$(function () {
    $(".come-on").click(function () {
        planId = $(this).parent(0).find("input").val();

        var data = {};
        data.planId = planId;
        $.ajax({
            url: "/plan/checkIfSupervice",
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result.boo_success) {  //还没有监督该计划
                    var _data = {};
                    _data.planId = planId;
                    $.ajax({
                        url: "/plan/supervice",
                        type: "post",
                        dataType: "json",
                        data: _data,
                        success: function (result) {
                            if (result.boo_success) {
                                $("#surpervice").fadeIn("fast");
                                $(".modal-backdrop").css("display", "block");
                            }
                            else {
                                return;
                            }
                        }
                    });
                }
                else {
                    alert("你已经监督了该计划");
                }
            }
        });
    });

    $(".back-plan").click(function () {
        $("#surpervice").css("display", "none");
        $(".modal-backdrop").css("display", "none");
    });
    $(".look-all-surpervice").click(function () {
        location.href = "/plan/superviceme/" + planId;
    });
});
