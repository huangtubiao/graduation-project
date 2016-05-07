$(document).ready(function () {
    //判断该用户是否已经是方案高手
    if ($("#user-merit").val() != "普通大学森") {
        $("#ques-title").val($("#user-merit").val());
        $("#superior-text").val($("#user-merit-intro").val());
        $(".remind").css("display", "block");
    }

    //成为方案高手验证
    $("#become-superior").validate({
        rules: {
            ques_title: {
                required: true,
                minlength: 5,
                maxlength:15
            },
            superior_text: {
                required: true,
                minlength: 5,
                maxlength:100
            }
        },
        messages: {
            ques_title: {
                required: "填写不能为空！",
                minlength: "不能少于5个字符",
                maxlength: "长度不能大于15个字符"
            },
            superior_text: {
                required: "填写不能为空！",
                minlength: "不能少于5个字符",
                maxlength: "长度不能大于100个字符"
            }
        }
    });

    

});

$(function () {
    //成为方案高手表单反馈
    $("#become-superior").ajaxForm({
        success: function (data) {
            if (data.boo_success) {
                debugger;
                location.href = "/projectsuperior/success";
            } else {
                alert("出错！");
            }
        }
    });

    //选择是否收费
    $(".if-charge a").click(function () {
        $(this).siblings("a").removeClass("onactive");
        $(this).addClass("onactive");
        $("#charge-way").focus();
    });
    $("#charge-true").click(function () {
        $(".select-charge-pop").css("opacity", "1");
        $(".modal-backdrop").css("display", "block");
    });
});

function offCharge() {
    $(".select-charge-pop").css("opacity", "0");
    $(".modal-backdrop").css("display", "none");
    $("#charge-true").removeClass("onactive");
    $("#charge-false").addClass("onactive");
}

function btnCharge() {
    $(".select-charge-pop").css("opacity", "0");
    $(".modal-backdrop").css("display", "none");
    $("#charge-true").text($("#charge-way").val());
}