var startY, endY, winScrollTop, heightY;

//帮助TA
function answer() {
    var data = {};
    data.answerContent = $(".comment-write").val();
    data.questionId = $("#questionId").attr("value");
    $.ajax({
        url: "/help/answer",
        type: "post",
        dataType: "json",
        data: data,
        success: function (result) {
            if (result.boo_success == "pleaseLogin") {
                $("#login").show();
                $(".modal-backdrop").show();
            }
            if (result.boo_success == true) {
                $(".success-tips").css("display", "block");
                setTimeout(function () {
                    $(".success-tips").css("display", "none");
                }, 1000);
                location.reload();
            }
            else if (result.boo_success == false) {
                alert("提交失败。");
            }
        }
    });
}

$(function () {
    $(".hd-help-ta").bind("click", function () {
        $(".comment-write").select();
        startY = $(this).offset().top;
        endY = $(".comment-write").offset().top;
        winScrollTop = document.body.scrollTop;
        heightY = (endY - startY + winScrollTop) + "px";
        $("html,body").animate({ scrollTop: heightY }, 300);
    });
});

var hdComments, r, answerReply, recoverWho;
$(function () {
    //对帮助者的评论内容进行评论
    $(".reply-item-reply").bind("click", function () {
        hdComments = $(this).parents().eq(4);
        answerReply = $(this).parents().eq(3);
        r = answerReply.find(".reply-to-reply");
        r.css("display", "block");
        answerReply.find("textarea").select();
        recoverWho = "回复 " + answerReply.find(".reply-author-name").html() + ":";
        var textarea = answerReply.find("textarea");
        textarea.val(recoverWho); //innerHTML += recoverWho;
        
        startY = $(this).offset().top;
        endY = r.offset().top;
        winScrollTop = document.body.scrollTop;
        heightY = (endY - startY + winScrollTop) + "px";
        $("html,body").animate({ scrollTop: heightY }, 300);

    });

    $(".js-reply-ipt-default").focus(function () {
        $(this).addClass("pl-fake-focus");
    });
    $(".js-reply-ipt-default").blur(function () {
        $(this).removeClass("pl-fake-focus");
    });

    //对帮助者回复
    $(".js-reply-item-reply").bind("click", function () {
        hdComments = $(this).parents().eq(6);
        answerReply = hdComments.find(".answer-reply");
        r = hdComments.find(".reply-to-reply");
        r.css("display", "block");
        hdComments.find("textarea").select();
        
        startY = $(this).offset().top;
        endY = r.offset().top;
        winScrollTop = document.body.scrollTop;
        heightY = (endY - startY + winScrollTop) + "px";
        $("html,body").animate({ scrollTop: heightY }, 300);

    });

    //取消评论
    $(".btn-cancel").bind("click", function () {  
        var textareaBox = $(this).parents().eq(3);
        textareaBox.find(".textarea-wrap").val("");
        textareaBox.css("display", "none");
    });

});

//对帮助者的评论内容进行评论
function replyReplySubmit() {
    var data = {};
    data.reAnswerContent = r.find("textarea").val();
    data.answerId = hdComments.find(".answerId").val();
    data.questionUserId = hdComments.find(".questionUserId").val();
    data.recoverWho = recoverWho;
    $.ajax({
        url: "/help/recoverAnswer",
        type: "post",
        dataType: "json",
        data: data,
        success: function (result) {
            if (result.boo_success == "pleaseLogin") {
                $("#login").show();
                $(".modal-backdrop").show();
            }
            if (result.boo_success == true) {
                r.find(".success-tips").css("display", "block");
                setTimeout(function () {
                    r.css("display", "none");
                }, 2000);
                setTimeout(function () {
                    r.find(".success-tips").css("display", "none");
                }, 2000);

                var replyItemDiv = document.createElement("div");    //显示回复的信息
                replyItemDiv.className = "reply-item reply-user-img";

                var a = document.createElement("a");
                a.className = "reply-item-author";
                a.innerHTML += "<img src=" + result.reAnswerUserImg + ">";
                replyItemDiv.appendChild(a);

                var innerItemDiv = document.createElement("div");
                innerItemDiv.className = "reply-inner-item";
                replyItemDiv.appendChild(innerItemDiv);

                var p = document.createElement("p");
                innerItemDiv.appendChild(p);

                var authorNameA = document.createElement("a");
                authorNameA.className = "reply-author-name";
                authorNameA.innerHTML += result.reAnswerUserName;
                p.appendChild(authorNameA);

                var recoverTextSpan = document.createElement("span");
                if (result.answerUserName == "") {
                    recoverTextSpan.className = "recover-text";
                }
                else {
                   recoverTextSpan.className = "recover-text _recover-text";
                }
                recoverTextSpan.innerHTML += "回复";
                p.appendChild(recoverTextSpan);

                var answerUserNameA = document.createElement("a");
                answerUserNameA.className = "reply-author-name answerUserName";
                answerUserNameA.innerHTML += result.answerUserName;
                p.appendChild(answerUserNameA);

                var itemContentP = document.createElement("p");
                itemContentP.className = "reply-item-c";
                itemContentP.innerHTML += data.reAnswerContent.replace(recoverWho, "");
                innerItemDiv.appendChild(itemContentP);

                var itemFootDiv = document.createElement("div");
                itemFootDiv.className = "reply-item-foot";
                innerItemDiv.appendChild(itemFootDiv);

                var indexSpan = document.createElement("span");
                indexSpan.className = "reply-item-index r";
                indexSpan.innerHTML += "#1";
                itemFootDiv.appendChild(indexSpan);
                var spanSecond = document.createElement("span");
                spanSecond.innerHTML += "0秒前";
                itemFootDiv.appendChild(spanSecond);

                answerReply[0].insertBefore(replyItemDiv, r[0]);

                //清除字符串的值
                recoverWho = null;
            }
            else if (result.boo_success == false) {
                alert("提交失败。");
            }
        }
    });
}

$(document).ready(function () {
    var answerUserName = $(".answerUserName");
    for (var i=0; i < answerUserName.length; i++) {
        if (answerUserName.eq(i)[0].text != "") {
            answerUserName.eq(i).prev().css("display", "inline-block");
        }
    }
});