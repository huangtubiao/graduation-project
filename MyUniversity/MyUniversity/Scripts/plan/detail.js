$(document).ready(function () {
    //评论者的性别显示
    var userSex = $(".user-sex");
    for (var i = 0; i < userSex.length; i++) {
        if (userSex.eq(i).val() == "男") {
            userSex.eq(i).parent(0).find("i").html("&#xe608");
            userSex.eq(i).parent(0).find("i").addClass("sex-icon");
        }
        else if (userSex.eq(i).val() == "女") {
            userSex.eq(i).parent(0).find("i").html("&#xe609");
            userSex.eq(i).parent(0).find("i").addClass("girlsex-img");
        }
    }

    //判断是页面是否来自“监督TA”
    if (request(4) == 1) {
        setTimeout(function () {
            $("#comment").select();
            startY = 0;
            endY = $("#comment").offset().top;
            winScrollTop = document.body.scrollTop;
            heightY = (endY - startY + winScrollTop) + "px";
            $("html,body").animate({ scrollTop: heightY }, 300);
        }, 200);
    }
});

$(function () {
    $(".js-placeholder").focus(function () {
        $(this).parent(0).addClass("js-pl-focus");
    });
    $(".js-placeholder").blur(function () {
        $(this).parent(0).removeClass("js-pl-focus");
    });
});

//发表评论
function comment() {
    var data = {};
    data.plancommentContent = $("#comment").val();
    data.planId = $("#planId").val();
    $.ajax({
        url: "/plan/planComment",
        type: "post",
        dataType: "json",
        data: data,
        success: function (result) {
            if (result.boo_success == false) {
                alert("提交失败");
            }
            else {
                $(".i-success").fadeIn("fast");
                $("#comment").val("");
                setTimeout(function () {
                    $(".i-success").fadeOut("fast");
                }, 1000);
                jsCreateComment(result);
            }
        }
    });
}
//创建新的评论
function jsCreateComment(result) {
    var jsonObj = eval("(" + result + ")");
    var divList = document.createElement("div");
    divList.className = "pl-list";
    $(".pl-container").append(divList);

    var divListcon = document.createElement("div");
    divListcon.className = "wenda-listcon";
    divList.appendChild(divListcon);

    var divHeadslider = document.createElement("div");
    divHeadslider.className = "headslider";
    divListcon.appendChild(divHeadslider);

    var aHead = document.createElement("a");
    aHead.className = "wenda-head";
    aHead.innerHTML += "<img src=" + jsonObj.userImg + ">";
    divHeadslider.appendChild(aHead);

    var aNickname = document.createElement("a");
    aNickname.className = "wenda-nickname";
    aNickname.href += "/account/person/" + jsonObj.userId;
    aNickname.innerHTML += jsonObj.userName;
    divHeadslider.appendChild(aNickname);

    var i = document.createElement("i");
    if (jsonObj.userSex == "男") {
        i.className = "iconfont sex-icon";
        i.innerHTML += "&#xe608";
    } else if (jsonObj.userSex == "女") {
        i.className = "iconfont sgirlsex-img";
        i.innerHTML += "&#xe609";
    }
    divHeadslider.appendChild(i);

    var divWrite = document.createElement("div");
    divWrite.className = "wendaslider pl-write";
    divListcon.appendChild(divWrite);

    var divWhat = document.createElement("div");
    divWhat.className = "pl-what";
    divWhat.innerHTML += jsonObj.planCommentContent;
    divWrite.appendChild(divWhat);

    var divTime = document.createElement("div");
    divTime.className = "pl-time";
    divTime.innerHTML += jsonObj.planCommentPublishedTime;
    divWrite.appendChild(divTime);
    return;
}

//打卡日志加载更多
var groupNum = 1;
$(function () {
    $(".lookmore").click(function () {
        var data = {};
        data.planId = $("#planId").val();
        groupNum = groupNum + 1;
        data.groupNum = groupNum;
        $.ajax({
            url: "/plan/loadingMoreClockLog",
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result != null) {
                    jsCreateClockLog(result);
                }
                else {
                
                }
            }
        });
    });
});
//创建打卡日志信息
function jsCreateClockLog(result) {
    var jsonObj = eval("(" + result + ")");
    for (var r in jsonObj) {
        var divBookin = document.createElement("div");
        divBookin.className = "bookin-day";
        $(".clock-log").append(divBookin);

        var i = document.createElement("i");
        i.className = "iconfont";
        i.innerHTML += "&#xe61d";
        divBookin.appendChild(i);

        var spanDay = document.createElement("span");
        spanDay.className = "day";
        spanDay.innerHTML += jsonObj[r].clockLogDay;
        divBookin.appendChild(spanDay);

        var spanYear = document.createElement("span");
        spanYear.className = "year";
        spanYear.innerHTML += jsonObj[r].clockLogYearMonth;
        divBookin.appendChild(spanYear);

        var divContent = document.createElement("div");
        divContent.className = "bookin-content";
        divContent.innerHTML += jsonObj[r].clockLogContent;
        divBookin.appendChild(divContent);
    }
}

//监督弹出窗体
$(function () {
    $("#supervice").click(function () {
        var data = {};
        data.planId = $("#planId").val();
        $.ajax({
            url: "/plan/checkIfSupervice",
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result.boo_success) {  //还没有监督该计划
                    var _data = {};
                    _data.planId = $("#planId").val();
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
        location.href = "/plan/superviceme/" + $("#planId").val();
    });
});

//加油弹出窗体
$(function () {
    $(".plan-cheer").click(function () {
        var cheerNum = $(this).find("em").text();
        var that = $(this).find("em");
        var data = {};
        data.planId = $("#planId").val();
        $.ajax({
            url: "/plan/checkIfCheered",
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result.boo_success) {
                    that.text(parseInt(cheerNum) + 1);
                    var _data = {};
                    _data.planId = $("#planId").val();
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
                    alert("你已经加油过啦！");
                }
            }
        });
    });
    //添加加油内容
    $(".cheer-btn").click(function () {
        var timer = null;
        var data = {};
        data.planComeOnWrite = $(".cheer-write").val();
        data.planId = $("#planId").val();
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
    $(".plan-cheer").mouseover(function (event) {
        var that = $(this);
        if (that.find(".popover")[0] == undefined) {
            if (timer) {
                clearTimeout(timer);
                timer = null;
            }
            timer = setTimeout(function () {
                //获取最近加油的数据
                var data = {};
                data.planId = $("#planId").val();
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
    $(".plan-cheer").mouseout(function () {
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

//点击评论
$(function () {
    $(".plan-comment").bind("click", function () {
        $("#comment").select();
        startY = $(this).offset().top;
        endY = $("#comment").offset().top;
        winScrollTop = document.body.scrollTop;
        heightY = (endY - startY + winScrollTop) + "px";
        $("html,body").animate({ scrollTop: heightY }, 300);
    });
});


//获取当前url的参数
function request(paras) {
    var url = location.href;
    var paraString = url.substring(url.indexOf("/") + 2, url.length).split("/");
    return paraString[paras];
}