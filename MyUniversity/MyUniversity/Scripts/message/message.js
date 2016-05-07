$(function () {
    //个人中心显示窗体
    $(".account-num").mouseover(function () {
        $(".person-select").css("display", "block");
    });
    $(".account-num").mouseout(function () {
        $(".person-select").css("display", "none");
    });
    $("#q").mouseover(function () {
        $(".person-select").css("display", "block");
    });
    $("#q").mouseout(function () {
        $(".person-select").css("display", "none");
    });

    //显示最近聊天对应的对话记录
    $("#lastChat li").click(function () {
        $("#chat_content ul").css("display", "none");
        $("#chat_content").find("." + $(this)[0].id).css("display", "block");
        $(".chat-header h4").text($(this).find("h5").text());
        $(".chat-header").css("display", "block");
        //滚动条到达底部
        $("#chat_content")[0].scrollTop = $("#chat_content")[0].scrollHeight - $("#chat_content").height();
        //把接收人加入组
        $("#recievename").val($(this).find("input").val());
        //当前的最近对话id
        $("#last-chat-id").val($(this)[0].id);
        //未读信息更新为已读信息

        $(this).find(".unread_num").css("display", "none");
        $(this).find(".unread_num").text("0");
        var data = {};
        data.lastChatId = $(this)[0].id;
        $.ajax({
            url: "/message/updateMessage",
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result.boo_success) {
                    return;
                } else {
                    return;
                }
            }
        });
        //检查是否还有未读信息，若没有则导航条不显示通知信息
        var n = 0;
        var lastChatLi = $("#lastChat li");
        for (var l = 0; l < lastChatLi.length; l++) {
            if (lastChatLi.eq(l).find(".unread_num").css("display") == "block") {
                n++;
            }
        }
        if (n == 0) {
            var data = {};
            data.statu = "通知为零";
            $.ajax({
                url: "/message/changeInfo",
                type: "post",
                dataType: "json",
                data: data,
                success: function (result) {
                    if (result.boo_success) {
                        $(".msg_icon").css("display", "none");
                    } else {
                        return;
                    }
                }
            });
        }
        else {
            var data = {};
            data.statu = n;
            $.ajax({
                url: "/message/changeInfo",
                type: "post",
                dataType: "json",
                data: data,
                success: function (result) {
                    if (result.boo_success) {
                        $(".msg_icon").text(n);
                    } else {
                        return;
                    }
                }
            });
        }
        //检查接收方是否已经是好友，不是则显示添加为联系人提醒
        var friendLi = $("#friends li");
        var k = 0;
        for (var f = 0; f < friendLi.length; f++) {
            if (friendLi.eq(f)[0].className == $(this).find("input").val()) {
                k++;
            }
        }
        if (k == 0) {
            $(".save-friend").css("display", "block");
        }
        else {
            $(".save-friend").css("display", "none");
        }
    });

    //显示联系人的信息
    $(document).delegate("#friends li", "click", function () {
        var data = {};
        data.userId = $(this).attr("class");
        $.ajax({
            url: "/message/getUser",
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result != null) {
                    var jsonObj = eval("(" + result + ")");
                    $(".headpic").find("img")[0].src = jsonObj.userImg;
                    $(".user-info a").text(jsonObj.userName);
                    $(".user-info div").text(jsonObj.userMerit);
                    $(".address .user-school").text(jsonObj.userSchool);
                    $(".address .user-depart").text(jsonObj.userDepart);
                    $(".user-merit-intro").text(jsonObj.userMeritIntro)
                    $(".userinfo_layer").css("display", "block");
                    $(".user-id").val(jsonObj.userId);
                    $(".btn-info").attr("href", "/projectsuperior/space/" + jsonObj.userId);
                } else {
                    alert("系统出错！");
                }
            }
        });
    });

    //向TA发起消息
    $(".send-message").click(function () {
        $(".userinfo_layer").css("display", "none");
        $("#notice-tit #0").addClass("onactive");
        $("#notice-tit #1").removeClass("onactive");
        var user = $(".userinfo_layer").find(".user-name").text();
        //
        $(".chat-header h4").text(user);
        $(".chat-header").css("display", "block");
        $(".userinfo_layer").css("display", "none");
        $("#mod-1").css("display", "block");
        $("#mod-2").css("display", "none");
        //显示对话记录》通过联系人的id》查看是否在最近联系人里》然后显示其相应的记录对话信息
        var userId = $(this).parent(0).find(".user-id").val(); 
        var lastChat = $("#lastChat li");
        var n = 0;
        for (var i = 0; i < lastChat.length; i++) {
            if (lastChat.eq(i).find("input").val() == userId) {
                var id = lastChat.eq(i)[0].id;
                $("#chat_content").find("." + id).css("display", "block");
                n++;
            }
        }
        //若是新的第一次聊天，则创建新的聊天窗体
        if (n == 0) {
            var ul = document.createElement("ul");
            ul.className = userId;
            $(".chat-header").after(ul);
        }
        //把TA的用户id显示在“接收人id(组)”
        $("#recievename").val($(".user-id").val());
    });

    //删除联系人
    $(".delete-friend").click(function () {
        $("#warning").fadeIn("fast");
        $(".modal-backdrop").css("display", "block");
    });
    $(".w-submit a").click(function () {
        var data = {};
        data.userId = $(".user-id").val();
        $.ajax({
            url: "/message/deleteFriend",
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result.boo_success) {
                    $("#warning").css("display", "none");
                    $(".modal-backdrop").css("display", "none");
                    $(".userinfo_layer").css("display", "none");
                    $("#friends ." + $(".user-id").val()).remove();
                } else {
                    alert("删除失败");
                }
            }
        });
    });
    //取消删除联系人
    $(".w-cancel a").click(function () {
        $("#warning").css("display", "none");
        $(".modal-backdrop").css("display", "none");
    });

    //添加为联系人
    $(".btn-save-friend").click(function () {
        $("#save-friend-click").click();
        $(".save-friend").css("display", "none");
        var data = {};
        data.FriendId = $("#recievename").val();
        $.ajax({
            url: "/message/addContact",
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result != "") {
                    //在我的联系人里添加刚新加的人
                    var jsonObj = eval("(" + result + ")");
                    var li = document.createElement("li");
                    li.className = jsonObj.contactFriendId;
                    $("#friends").children().eq(0).before(li);
                    var img = document.createElement("img");
                    img.src = jsonObj.userImg;
                    li.appendChild(img);
                    var div = document.createElement("div");
                    div.className = "info";
                    li.appendChild(div);
                    var h5 = document.createElement("h5");
                    h5.innerHTML += jsonObj.userName;
                    div.appendChild(h5);
                } 
            }
        });
    });

    //是否“是否保存为联系人”提醒到达顶部时固定
    var navH = $("#chat_content").offset().top;
    $("#chat_content").scroll(function () {
        $(".save-friend").css({ "position": "absolute", "top": $(this).scrollTop() });
        if ($(this).scrollTop() == 0) {
            $(".save-friend").css("position", "relative");
        }
    })
});



$(document).ready(function () {
    var receiver = request("3").replace(/[^0-9]/ig, "");
    //显示信息未读条数
    var lastChat = $("#lastChat li");
    for (var i = 0; i < lastChat.length; i++) {
        if (lastChat.eq(i).find(".unread_num").text() != 0) {
            lastChat.eq(i).find(".unread_num").css("display", "block");
        }
        //1.判断以前是否有过聊天，有就显示以前的聊天信息2.判断有没有最近聊天，有就把最近聊天id显示
        if (lastChat.eq(i).find("input").val() == receiver) {
            $("#chat_content").find("." + lastChat.eq(i)[0].id).css("display", "block");
            $("#last-chat-id").val(lastChat.eq(i)[0].id);
        }
    }
    //导航条显示通知信息
    if ($(".msg_icon").text() > 0) {
        $(".msg_icon").css("display", "block");
    }
    //显示当前对话的联系人
    if ($(".chat-header h4").text() != "") {
        $(".chat-header").css("display", "block");
    }
    //判断正在联系人是否为好友，不是则显示“添加为联系人提醒”
    $("#recievename").val(receiver);
    if (request("3") != "send") {
        var friends = $("#friends li");
        var n = 0;
        for (var i = 0; i < friends.length; i++) {
            if (friends.eq(i)[0].className == receiver) {
                n++;
            }
        }
        if (n == 0) {
            $(".save-friend").css("display", "block");
        }
    }
    
    //判断是否有没有聊天记录，没有则提示
    if (request("3") != "send") {
        var m = 0;
        var chat_content = $("#chat_content ul");
        for (var i = 0; i < chat_content.length; i++) {
            if (chat_content.eq(i).css("display") == "block") {
                m++;
            }
        }
        if (m == 0) {
            var ul = document.createElement("ul");
            $(".save-friend").after(ul);
            var li = document.createElement("li");
            li.className = "tip";
            ul.appendChild(li);
            var a = document.createElement("a");
            a.innerHTML = "打个招呼开始你们的对话吧！";
            li.appendChild(a);
        }
    }

    notiveLi = $("#notice-tit li");
    //选项卡
    var timer = null;
    var titles = $("#notice-tit").children("li");
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
                    $("#" + divs[j].id).css("display", "none");
                }
                $("#" + titles[that.id].id).addClass("onactive");
                $("#" + divs[that.id].id).css("display", "block");
            }, 50);
            notiveLi = $("#notice-tit li");
            //
            $("#chat_content ul").css("display", "none");
            $(".userinfo_layer").css("display", "none");
            $(".chat-header").css("display", "none");
            $(".save-friend").css("display", "none");
        });
    }
});



