//写新的计划
$("#writePlan").click(function () {
    if ($("#ques-title").val().length < 5) {
        $("#ques-title").removeClass("while-btn");
        $("#ques-title").addClass("ipt-error");
        var i = document.createElement("i");
        i.className = "iconfont i-warning";
        i.innerHTML += "&#xe65f";
        $(".errortip-input").append(i);
        var span = document.createElement("span");
        span.innerHTML += "目标不能少于5个字符！";
        $(".errortip-input").append(span);
        return;
    }
    else if ($("#ques-text").val().length < 5 && $(".errortip-area").html() == "") {
        var i = document.createElement("i");
        i.className = "iconfont i-warning";
        i.innerHTML += "&#xe65f";
        $(".errortip-area").append(i);

        var span = document.createElement("span");
        span.innerHTML += "计划不能少于5个字符！";
        $(".errortip-area").append(span);
        return;
    }
    var data = {};
    data.planTitle = $("#ques-title").val();
    data.planContent = $("#ques-text").val();
    data.planStartTime = $("#start-time").val();
    data.planEndTime = $("#end-time").val();
    $.ajax({
        url: "/plan/write",
        type: "post",
        dataType: "json",
        data: data,
        success: function (result) {
            if (result.boo_success) {
                $(".publishbtn").text("");
                $(".publishbtn").text("正在发布...");
                setTimeout(function () {
                    alert("hahahha");
                }, 1000);
                $(".publishbtn").text("发布");
            }
            else {

            }
        }
    });
});

//输入提示
$("#ques-title").bind("keyup", function (e) {
    if ($(this).val().length >= 5) {
        $(this).removeClass("ipt-error");
        $(this).addClass("while-btn");
        $(".errortip").empty();
    }
});
$("#ques-title").bind("blur", function () {
    if ($(this).val().length < 5 && $(".errortip-input").html() == "") {
        $("#ques-title").removeClass("while-btn");
        $("#ques-title").addClass("ipt-error");
        var i = document.createElement("i");
        i.className = "iconfont i-warning";
        i.innerHTML += "&#xe65f";
        $(".errortip-input").append(i);
        var span = document.createElement("span");
        span.innerHTML += "目标不能少于5个字符！";
        $(".errortip-input").append(span);
    }
});
$("#ques-text").bind("keyup", function () {
    if ($(this).val().length >= 5) {
        $(".errortip-area").empty();
    }
});
$("#ques-text").bind("blur", function () {
    if ($(this).val().length < 5 && $(".errortip-area").html() == "") {
        var i = document.createElement("i");
        i.className = "iconfont i-warning";
        i.innerHTML += "&#xe65f";
        $(".errortip-area").append(i);

        var span = document.createElement("span");
        span.innerHTML += "计划不能少于5个字符！";
        $(".errortip-area").append(span);
    }
});



$(document).delegate(".remove", "click", function () {
    $(this).parent().remove();
});

$(function () {
    $(".add-more").click(function () {
        var div = document.createElement("div");
        div.className = "invite_members";

        var input = document.createElement("input");
        input.className = "while-btn";
        input.placeholder = "输入邮箱地址";
        input.type = "text";
        div.appendChild(input);

        var a = document.createElement("a");
        a.className = "remove js-remove";
        a.innerHTML += "删除";
        div.appendChild(a);

        $(".add-more").before(div);
    });
});

//获取字符串的长度
function getLength(str) {
    var realLength = 0, len = str.length, charCode = -1;
    for (var i = 0; i < len; i++) {
        charCode = str.charCode(i);
        if (charCode >= 0 && charCode <= 128) realLength += 1;
        else realLength += 2;
    }
    return realLength;
}