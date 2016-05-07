$(document).ready(function () {
    //完成的计划显示完成状态
    var ifFinish = $(".plan-is-finish");
    for (var i = 0; i < ifFinish.length; i++) {
        if (ifFinish.eq(i).val() == "value") {
            ifFinish.eq(i).parent(0).find(".finish-state").html("&#xe661");
            ifFinish.eq(i).parent(0).find(".finish-state").addClass("success");
        }
        else {
            ifFinish.eq(i).parent(0).find(".finish-state").html("&#xe649");
        }
    }

    //显示今天是否完成打卡日志
    var ifClockLog = $(".if-clock-log");
    for (var i = 0; i < ifClockLog.length; i++) {
        if (ifClockLog.eq(i).val() == 1) {
            ifClockLog.eq(i).parent(0).find(".people-supevise").text("已完成打卡日志");
        }
        else {
            ifClockLog.eq(i).parent(0).find(".people-supevise").text("TA还没完成打卡日志哦");
        }
    }


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

    totalGroups = Math.ceil($("#allMySupervice").val() / 5);

    
});

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
                        addLoadingImg();  //添加底部加载图片
                        $("#my-supervice").fadeIn("fast");
                        trackLoad.groupNumber = 2;
                    }
                }
            });
        }
        else {   //不显示已经完成的计划
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
                        addLoadingImg();  //添加底部加载图片
                        $("#my-supervice").fadeIn("fast");
                        trackLoad.groupNumber = 2;
                    }
                }
            });
        }

    });
});

//显示我监督的计划（最热或最热）的信息
function showMySupervicePlans(result, loadMore) {
    var jsonObj = eval("(" + result + ")");
    for (var r in jsonObj) {
        var divList = document.createElement("div");
        divList.className = "wenda-list my-supervice";

        if (loadMore == "加载更多") {
            $(".loading").before(divList);
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
        a.href = "/account/person/" + jsonObj[r].userId;
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
        if (jsonObj[r].planIsFinish) {
            i.className = "iconfont success";
            i.innerHTML += "&#xe661";
        }
        else {
            i.className = "iconfont";
            i.innerHTML += "&#xe649";
        }
        divWendaquetitle.appendChild(i);

        var divWendatitlecon = document.createElement("div");
        divWendatitlecon.className = "wendatitlecon";
        divWendaquetitle.appendChild(divWendatitlecon);

        var a = document.createElement("a");
        a.innerHTML += jsonObj[r].planTilte;
        a.href = "/plan/detail/" + jsonObj[r].planId;
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
        aComeOn.href = "/plan/detail/" + jsonObj[r].planId + "/1";
        aComeOn.innerHTML += "督促TA";
        divListFooter.appendChild(aComeOn);

        var spanSupevise = document.createElement("span");
        spanSupevise.className = "people-supevise";
        if (jsonObj[r].todayIfClockLog == 1) {
            spanSupevise.innerHTML += "已完成打卡日志";
        }
        else {
            spanSupevise.innerHTML += "今天TA还没完成打卡日志哦";
        }
        divListFooter.appendChild(spanSupevise);
    }
}

//添加底部加载图片
function addLoadingImg() {
    var divLoading = document.createElement("div");
    var iLoading = document.createElement("i");
    iLoading.className = "iconfont";
    iLoading.innerHTML += "&#xe643";
    divLoading.appendChild(iLoading);
    divLoading.className = "loading";
    $("#my-supervice").append(divLoading);
}

var totalGroups;
var trackLoad = new Object();
trackLoad.groupNumber = 2;
trackLoad.loading = false;
//滚动条滚到底部数据加载更多
$(window).scroll(function () {
    if ($(window).scrollTop() + $(window).height() == $(document).height()) {
        if (trackLoad.groupNumber <= totalGroups && !trackLoad.loading) {
            trackLoad.loading = true;      // Blocks other loading data again.
            $(".loading").fadeIn("slow");
            setTimeout(function () {
                getData(trackLoad);
            }, 1000);
        }
    }
});

//获取数据
function getData(options) {
    var data = {};
    data.groupNumber = options.groupNumber;
    data.active = "我监督的";
    data.hotOrNew = "最新";
    $.ajax({
        url: "/plan/getNewHotPlan",
        type: "post",
        dataType: "json",
        data: data,
        success: function (result) {
            if (result != null) {
                $(".loading").fadeOut("slow");
                showMySupervicePlans(result, "加载更多");
                options.groupNumber++;
                options.loading = false;
            }
            else {
                return;
            }
        }
    });
}



//监督TA
$(function () {
    $(".come-on").click(function () {
        var planId = $(this).parent(0).find(".planId").val();
        location.href = "/plan/detail/" + parseInt(planId) + "?supervice=true";
    });
});
