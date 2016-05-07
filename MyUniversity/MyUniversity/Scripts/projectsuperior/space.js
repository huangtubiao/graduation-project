$(document).ready(function () {
    //选项卡
    var timer = null;
    var titles = $("#notice-space").children("li");
    var divs = $("#notice-con").children(".mod");
    if (titles.length != divs.length)
        return;
    for (var i = 0; i < titles.length; i++) {
        titles[i].id = i;
        $("#" + titles[i].id).click(function () {
            var that = this;
            if (timer) {
                clearTimeout(timer);
                timer = null;
            }
            timer = setTimeout(function () {
                for (var j = 0; j < titles.length; j++) {
                    $("#" + titles[j].id).attr("class", "");
                    $("#" + divs[j].id).fadeOut("fast");
                }
                $("#" + titles[that.id].id).addClass("onactive");
                $("#" + divs[that.id].id).fadeIn("fast");
            }, 300);
        });
    }

    //判断性别，并且显示相关的性别图片
    var userSex = $(".user-sex");
    for (var i = 0; i < userSex.length; i++) {
        if (userSex.eq(i).val() == "男") {
            userSex.eq(i).parent(0).find(".sex").addClass("sex-icon");
            userSex.eq(i).parent(0).find(".sex").html("&#xe608");
        }
        else {
            userSex.eq(i).parent(0).find(".sex").addClass("girlsex-img");
            userSex.eq(i).parent(0).find(".sex").html("&#xe639");
        }
    }

    //判断是否已关注了TA
    if ($("#focus span").text() == "已关注") {
        $("#focus i").html("&#xe682");
    } else {
        $("#focus i").html("&#xe659");
    }

    //没有方案记录，显示没有图片      
    if ($(".super-note-wrap").children().length == 1) {
        $(".uncontent").css("display", "block");
    }
    if ($("#mod-3").children().length == 1) {
        $("#mod-3").find(".uncontent").css("display", "block");
    }
});


$(function () {
    //私信Ta
    $("#private-letter").click(function () {
        $.post('/projectsuperior/checkIfLogin', function (result) {
            if (result.boo_success) {
                location.href = "/message/index/" + $("#userId").val() + "/s";
            } else {
                var toHref = "/message/index/" + $("#userId").val() + "/s";
                showlogin(toHref);
            }
        }, "json");
    });

    //关注TA
    $("#focus").click(function () {
        if ($("#focus span").text() == "已关注") {
            return;
        } else {
            $.post('/projectsuperior/checkIfLogin', function (result) {
                if (result.boo_success) {
                    var data = {};
                    data.userId = $("#userId").val();
                    $.ajax({
                        url: "/projectsuperior/AddFollow",
                        type: "post",
                        dataType: "json",
                        data: data,
                        success: function (result) {
                            if (result.boo_success) {
                                $("#success-follow").fadeIn("fast");
                                $(".modal-backdrop").css("display", "block");
                                setTimeout(function () {
                                    $("#success-follow").css("display", "none");
                                    $(".modal-backdrop").css("display", "none");
                                }, 3000);
                                $("#focus i").html("&#xe682");
                                $("#focus span").text("已关注");
                                $("#fans").text(parseInt($("#fans").text()) + 1);
                            }
                            else {
                                alert("错误");
                            }
                        }
                    });
                } else {
                    showlogin();
                }
            }, "json");
        }
    });

    //点赞
    $(".favorite").click(function () {
        var that = $(this);
        var url = 'http://chaxun.1616.net/s.php?type=ip&output=json&callback=?&_=' + Math.random();
        $.getJSON(url, function (data) {
            $.get('/projectsuperior/checkIfP?ip=' + data.Ip, function (result) {
                if (result.boo_success) {
                    var data = {};
                    data.pRId = that.parent(0).find("input").val();
                    $.ajax({
                        url: "/projectsuperior/pRLove",
                        type: "post",
                        dataType: "json",
                        data: data,
                        success: function (result) {
                            if (result.boo_success) {
                                var num = parseInt(that.find("em").text()) + 1;
                                that.find("em").text(num);
                            }
                            else {
                                alert("失败");
                            }
                        }
                    });
                } else {
                    alert("用户已经点赞过");
                }
            });
        });
    });
});
