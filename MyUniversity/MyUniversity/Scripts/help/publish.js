function publish() {
    var data = {};
    data.questionTitle = $("#ques-title").val();
    data.questionContent = $("#ques-text").val();
    $.ajax({
        url: "/help/publish",
        type: "post",
        dataType: "json",
        data: data,
        success: function (result) {
            if (result.boo_success) {
                window.location.href = "/help/savesuccess";
            }
            else{
                alert("提交失败。");
            }
        }
    });
}