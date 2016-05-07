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

    //判断我的私信是否有内容
    if ($("#mod-1").children().length != 2) {
        $("#mod-1").find(".remind").css("display", "block");
    } else {
        $("#mod-1").find(".uncontent").css("display", "block");
    }
    if ($("#mod-2").children().length != 2) {
        $("#mod-2").find(".remind").css("display", "block");
    } else {
        $("#mod-2").find(".uncontent").css("display", "block");
    }
});

var that; //当前采纳方案的
$(function () {
    //判断是否已经采纳方案高手的方案，采纳显示已采纳或最佳方案高手
    var comment = $(".letter-if-comment");
    for (var i = 0; i < comment.length; i++) {
        //当comment值为0，表示最佳方案；当comment值为1时，表示方案不合；当comment值为2时，表示还没有进行采纳该方案
        if (comment.eq(i).val() == 2) {
            comment.eq(i).siblings(".project-accept").css("display", "block");
        } else if (comment.eq(i).val() == 0) {
            var h3 = document.createElement("h3");
            h3.className = "the-best-project";
            h3.innerHTML += "最佳方案";
            comment.eq(i).siblings(".wenda-listcon").before(h3);
        }
    }
    //最佳方案
    $(".love").click(function () {
        that = $(this);
        if ($(this).siblings(".letter-if-comment").val() == 2) {
            //显示经验值+20提示
            $("#add-experience").css("display", "block");
            var windowWidth = document.documentElement.clientWidth;
            var windowHeight = document.documentElement.clientHeight;
            var popupHeight = $("#add-experience").height();
            var popupWidth = $("#add-experience").width();
            $("#add-experience").css({
                "top": (windowHeight - popupHeight) / 2,
                "left": (windowWidth / 2) - (popupWidth / 2)
            });
            var img = $(this).siblings(".wenda-listcon").find("img");
            var differWidth = $("#add-experience").offset().left - img.offset().left;
            var differHeight = $("#add-experience").offset().top - img.offset().top;
            var l = img.offset().left + "px";
            var h = img.offset().top + "px";
            setTimeout(function () {
                $("#add-experience").animate({
                    left: l,
                    top: h
                });
                $("#add-experience").fadeOut();
            }, 500);
            //显示对方案高手提供的方案进行描述和评价的窗体
            setTimeout(function () {
                $(".thank-superior-pop").css("display", "block");
                $(".modal-backdrop").css("display", "block");
                $(".private_project_userImg").attr("src", that.siblings(".wenda-listcon").find("img").attr("src"));
                $(".private_project_userId").val(that.siblings(".userId").val());
            }, 1000);
            //增加经验值20
            var data = {};
            data.userId = that.parents(".wenda-listcon").find(".userId").val();
            $.ajax({
                url: "/account/UpdateUser",
                type: "post",
                dataType: "json",
                data: data,
                success: function (result) {
                    if (result.boo_success) {
                        return;
                    }
                    else {
                        alert("错误");
                    }
                }
            });
        } else {
            return;
        }
    });

    //查看关注、私信
    $(".private-letter").click(function () {
        if ($(this).text() == "关注") {
            location.href = "/projectsuperior/follow";
        } else {
            location.href = "/projectsuperior/letter";
        }
    });

    //采纳Ta的方案
    $(".project-accept").mouseover(function () {
        $(this).find("span").css("color", "#00b33b");
        $(this).find("i").css("color", "#00b33b");
    });
    $(".project-accept").mouseout(function () {
        $(this).find("span").css("color", "#78ca99");
        $(this).find("i").css("color", "#78ca99");
    });

    //方案高手提供的方案以及对Ta的评价
    //选择最佳方案还是方案不合
    $(".btn-primary").click(function () {
        $(this).siblings().removeClass("active");
        $(this).addClass("active");
    });
    $("#thank-comment-form").ajaxForm({
        success: function (data) {
            if (data.boo_success == 0 || data.success == 1) {
                $(".thank-superior-pop").css("display", "none");
                $(".modal-backdrop").css("display", "none");
                debugger;
                var project_boo = data.boo_success;
                //更新对该方案高手已经采纳Ta的方案，并设置为最佳方案，superiorLetterIfComment = 0
                var data = {};
                data.superiorLetterId = that.siblings(".letter-id").val();
                data.userId = that.siblings(".wenda-listcon").find(".userId").val();
                data.superiorLetterIfComment = project_boo;
                $.ajax({
                    url: "/projectsuperior/bestSuperior",
                    type: "post",
                    dataType: "json",
                    data: data,
                    success: function (result) {
                        if (result.boo_success) {
                            if (project_boo == 0) {
                                //最佳方案
                                that.css("display", "none");
                                var h3 = document.createElement("h3");
                                h3.className = "the-best-project";
                                h3.innerHTML += "最佳方案";
                                that.siblings(".wenda-listcon").before(h3);
                            } else {
                                //方案不合
                                that.css("display", "none");
                            }
                        }
                        else {
                            alert("系统错误");
                        }
                    }
                });
            } else {
                alert("系统错误");
            }
        }
    });
});


