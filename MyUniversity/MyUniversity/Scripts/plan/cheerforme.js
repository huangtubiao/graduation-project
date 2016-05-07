var listnode;
var replynode;
$(function () {
    //回复
    $(".js-reply-ipt-default").focus(function () {
        $(this).addClass("pl-fake-focus");
    });
    $(".js-reply-ipt-default").blur(function () {
        $(this).removeClass("pl-fake-focus");
    });

    $(".cheer-reply").click(function () {
        listnode = $(this).parents().eq(3);
        replynode = listnode.find(".reply");
        replynode.css("display", "block");
        replynode.find("textarea").select();
    });

    $(".btn-cancel").bind("click", function () {
        replynode.css("display", "none");
    });
});

$(document).ready(function () {
    //显示性别相关icon
    var userSex = $(".user-sex");
    for (var i = 0; i < userSex.length; i++) {
        if (userSex.eq(i).val() == "男") {
            userSex.eq(i).prev().html("&#xe608");
            userSex.eq(i).prev().addClass("sex-icon");
        }
        else if (userSex.eq(i).val() == "女") {
            userSex.eq(i).prev().html("&#xe609");
            userSex.eq(i).prev().addClass("girlsex-img");
        }
    }

    //右边的排名列表
    var _userSex = $(".user-sex");
    for (var i = 0; i < _userSex.length; i++) {
        if (_userSex.eq(i).val() == "男") {
            _userSex.eq(i).parent(0).find(".sex").addClass("sex-icon");
            _userSex.eq(i).parent(0).find(".sex").html("&#xe608");
        }
        else if (_userSex.eq(i).val() == "女") {
            _userSex.eq(i).parent(0).find(".sex").addClass("girlsex-img");
            _userSex.eq(i).parent(0).find(".sex").html("&#xe609");
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
});