$(document).ready(function () {
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
});