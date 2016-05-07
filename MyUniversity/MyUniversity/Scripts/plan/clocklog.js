$(function () {
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

//提交打卡日志
$(function () {
    $("#write-clock-log").click(function () {
        var data = {};
        data.planId = $("#planId").val();
        data.clocklogContent = $("#clock-log-content").val();
        $.ajax({
            url: "/plan/writeClockLog",
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result.boo_success) {
                    alert("成功");
                    location.href = "/plan/success/" + $("#planId").val();
                } else {
                    alert("提交失败！");
                }
            }
        });
    });
});