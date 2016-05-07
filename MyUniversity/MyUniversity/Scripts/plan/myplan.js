$(document).ready(function () {
    //监督人数和为我加油的人
    //var num = $(".plan-supervice");
    //for (var i = 0; i < num.length; i++) {
    //    if (num.eq(i).find("span").text() > 0) {
    //        num.eq(i).css("color", "#78CA99");
    //    }
    //}

    //计划是否完成
    var myPlanList = $(".wenda-list");
    for (var i = 0; i < myPlanList.length; i++) {
        if (myPlanList.eq(i).find(".plan-is-finish").val() == "value") {
            myPlanList.eq(i).find("i").html("&#xe661");
            myPlanList.eq(i).find("i").addClass("success");
        }
        else {
            myPlanList.eq(i).find("i").html("&#xe649");
            myPlanList.eq(i).find(".clock-log").css("display", "inline");
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

    //今天是否已经打卡
    for (var i = 0; i < myPlanList.length; i++) {
        if (myPlanList.eq(i).find(".today-if-clock").val() == 1) {
            myPlanList.eq(i).find(".clock-log").text("已打卡");
            myPlanList.eq(i).find(".clock-log").css({"background":"#78ca99","color":"#fff"});
        }
        else {
            myPlanList.eq(i).find(".clock-log").text("去打卡");
        }
    }
});