var sendUser; //记录是哪一方发送了信息
var unreadnum = 0; //未读信息条数
var message;
$(function () {
    $.connection.hub.logging = true;
    var chat = $.connection.chatHub;
    chat.client.recieveMessage = function (msg) {
        switch (msg.MsgType) {
            case "1"://普通消息
                $("#discussion").append("<li>[" + msg.UserName + "]发送消息：" + msg.Message + "</li>");
                //判断用户是否在对方的聊天页面(若不在聊天页面：a.双方已经是好友，但是接受方没有在发送方聊天页面；b.新的对话聊天)
                if (msg.UserName == $('#recievename').val()) { //在发送方的聊天窗体
                    var chatUl = $("#chat_content ul");
                    for (var i = 0; i < chatUl.length; i++) {
                        if (chatUl.eq(i).css("display") == "block") {

                            var li = document.createElement("li");
                            if (sendUser == localStorage.getItem("userId")) {
                                li.className = "me";
                            } else {
                                li.className = "you";
                            }

                            chatUl.eq(i)[0].appendChild(li);
                            var divAvata = document.createElement("div");
                            divAvata.className = "chat_avata";
                            li.appendChild(divAvata);
                            var a = document.createElement("a");
                            a.innerHTML += "<img src=" + msg.UserImg + ">";
                            divAvata.appendChild(a);
                            var divInfo = document.createElement("div");
                            divInfo.className = "a_msg_info";
                            li.appendChild(divInfo);
                            var span = document.createElement("span");
                            span.innerHTML += msg.Message;
                            divInfo.appendChild(span);
                            var i = document.createElement("i");
                            i.className = "arrow_left_b";
                            divInfo.appendChild(i);
                            //滚动条到达底部
                            $("#chat_content")[0].scrollTop = $("#chat_content")[0].scrollHeight - $("#chat_content").height();
                            //插入已读数据、同时更新最近聊天信息
                            var data = {};
                            data.lastChatId = msg.lastChatId;
                            data.messageSendUserId = msg.UserName;
                            data.messageContent = msg.Message;
                            data.messageIfRead = true;
                            $.ajax({
                                url: "/message/messageIsTrue",
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

                            sendUser = "";
                        }
                    }
                } else { //不在发送方的聊天窗体
                    //最近联系人显示未读信息通知
                    unreadnum = parseInt($("#" + msg.lastChatId).find(".unread_num").text()) + 1;
                    $("#" + msg.lastChatId).find(".unread_num").text(unreadnum);
                    $("#" + msg.lastChatId).find(".theLastMsg").text(msg.Message);
                    $("#" + msg.lastChatId).find(".unread_num").css("display", "inline-block");

                    //显示信息在聊天对应窗体
                    var li = document.createElement("li");
                    li.className = "you";
                    $("." + msg.lastChatId).append(li);
                    var divAvata = document.createElement("div");
                    divAvata.className = "chat_avata";
                    li.appendChild(divAvata);
                    var a = document.createElement("a");
                    a.innerHTML += "<img src=" + msg.UserImg + ">";
                    divAvata.appendChild(a);
                    var divInfo = document.createElement("div");
                    divInfo.className = "a_msg_info";
                    li.appendChild(divInfo);
                    var span = document.createElement("span");
                    span.innerHTML += msg.Message;
                    divInfo.appendChild(span);
                    var i = document.createElement("i");
                    i.className = "arrow_left_b";
                    divInfo.appendChild(i);
                    //插入未读数据、同时更新最近聊天信息

                    //显示导航条通知信息
                    if ($(".msg_icon").text() == 0) {
                        $(".msg_icon").text("1");
                    }
                    else {
                        var data = {};
                        data.lastChatId = msg.lastChatId;
                        $.ajax({
                            url: "/message/caculateInfoNum",
                            type: "post",
                            dataType: "json",
                            data: data,
                            success: function (result) {
                                $(".msg_icon").text(result.boo_success);
                            }
                        });
                    }
                    $(".msg_icon").css("display", "block");
                }

                break;
            case "2"://跳转消息
                location.href = msg.Url;
                break;
        }

    };
    // 关闭所有客户端连接
    chat.client.StopConnect = function () {
        chat.server.leaveGroup($('#displayname').val());//断开连接前先从组中删除
        $.connection.hub.stop();
        $("#txtConnectionId").val("");
    };
    $("#btn_stop").click(function () {
        chat.server.leaveGroup($('#displayname').val());//断开连接前先从组中删除
        $.connection.hub.stop();
        $("#txtConnectionId").val("");
    });
    // 断线5秒后重连
    $.connection.hub.disconnected(function () {
        if ($.connection.hub.lastError) {
            console.log('连接断开：' + $.connection.hub.lastError.message);
        }
        $("#txtConsole").val('连接断开');
        setTimeout(function () {
            $.connection.hub.start().done(function () {
                $("#txtConsole").val('重新连接成功');
                $("#txtConnectionId").val($.connection.hub.id);
                chat.server.joinGroup($('#displayname').val());//连接后加入组
            });
        }, 5000);
    });

    $.connection.hub.start().done(function () {
        $("#txtConsole").val('连接成功');
        $("#txtConnectionId").val($.connection.hub.id);
        chat.server.joinGroup($('#displayname').val());//连接后加入组
        $('#sendmessage').click(function () {
            // Call the Send method on the hub.
            chat.server.sendToAll($('#displayname').val(), $('#message').val());
            // Clear text box and reset focus for next comment.
            $('#message').val('').focus();
        });
        $("#btn_stopall").click(function () {
            chat.server.disconnectAll();
        });
        $("#sendmessagetoGroup").click(function () {
            message = $('#message').val();
            $(".tip").css("display", "none");
            //记录是否是本人发送的信息
            sendUser = localStorage.getItem("userId");
            //显示发送信息
            var chatUl = $("#chat_content ul");
            for (var i = 0; i < chatUl.length; i++) {
                if (chatUl.eq(i).css("display") == "block") {
                    var li = document.createElement("li");
                    li.className = "me";
                    chatUl.eq(i)[0].appendChild(li);
                    var divAvata = document.createElement("div");
                    divAvata.className = "chat_avata";
                    li.appendChild(divAvata);
                    var a = document.createElement("a");
                    a.innerHTML += "<img src=" + $(".account-num img").attr("src") + ">";
                    divAvata.appendChild(a);
                    var divInfo = document.createElement("div");
                    divInfo.className = "a_msg_info";
                    li.appendChild(divInfo);
                    var span = document.createElement("span");
                    span.innerHTML += $('#message').val();
                    divInfo.appendChild(span);
                    var i = document.createElement("i");
                    i.className = "arrow_left_b";
                    divInfo.appendChild(i);

                    $("#chat_content")[0].scrollTop = $("#chat_content")[0].scrollHeight - $("#chat_content").height();
                }
            }
            //获取新加的最近聊天id
            if ($('#last-chat-id').val() == "") {
                var data = {};
                data.userId = $("#recievename").val();
                $.ajax({
                    url: "/message/getLastChatId",
                    type: "post",
                    dataType: "json",
                    data: data,
                    success: function (result) {
                        if (result.boo_success != false) {
                            $('#last-chat-id').val(result.boo_success);
                            chat.server.sendToGroup($('#recievename').val(), $('#displayname').val(), $('#message').val(), $('#displayimg').val(), $('#last-chat-id').val());
                            sendUser = "";
                            //如果是为寻找方案高手而展开的私信，则插入信息到SuperiorLetter
                            if (request("4") == "s") {
                                if (selectSperiorLetter($('#recievename').val()) == false) {
                                    addSuperiorLetter($('#recievename').val());
                                }
                            }
                            checkSperiorLetter($('#recievename').val());
                            //
                            $("#message").val("");
                        }
                        else {
                            chat.server.sendToGroup($('#recievename').val(), $('#displayname').val(), $('#message').val(), $('#displayimg').val(), $('#last-chat-id').val());
                            sendUser = "";
                            //如果是为寻找方案高手而展开的私信，则插入信息到SuperiorLetter
                            if (request("4") == "s") {
                                if (selectSperiorLetter($('#recievename').val()) == false) {
                                    addSuperiorLetter($('#recievename').val());
                                }
                            }
                            checkSperiorLetter($('#recievename').val());
                            //
                            $("#message").val("");
                        }
                    }
                });
            }
            else {
                //发送给指定的用户
                chat.server.sendToGroup($('#recievename').val(), $('#displayname').val(), $('#message').val(), $('#displayimg').val(), $('#last-chat-id').val());
                sendUser = "";
                //如果是为寻找方案高手而展开的私信，则插入信息到SuperiorLetter
                if (request("4") == "s") {
                    if (selectSperiorLetter($('#recievename').val()) == false) {
                        addSuperiorLetter($('#recievename').val());
                    }
                }
                //
                checkSperiorLetter($('#recievename').val());
                //
                $("#message").val("");
            }
        });

        //添加为联系人时执行的事件
        $("#save-friend-click").click(function () {
            //记录是否是本人发送的信息
            sendUser = localStorage.getItem("userId");
            //显示发送信息
            var chatUl = $("#chat_content ul");
            for (var i = 0; i < chatUl.length; i++) {
                if (chatUl.eq(i).css("display") == "block") {
                    var li = document.createElement("li");
                    li.className = "me";
                    chatUl.eq(i)[0].appendChild(li);
                    var divAvata = document.createElement("div");
                    divAvata.className = "chat_avata";
                    li.appendChild(divAvata);
                    var a = document.createElement("a");
                    a.innerHTML += "<img src=" + $(".account-num img").attr("src") + ">";
                    divAvata.appendChild(a);
                    var divInfo = document.createElement("div");
                    divInfo.className = "a_msg_info";
                    li.appendChild(divInfo);
                    var span = document.createElement("span");
                    span.innerHTML += "我添加你为联系人啦~";
                    divInfo.appendChild(span);
                    var i = document.createElement("i");
                    i.className = "arrow_left_b";
                    divInfo.appendChild(i);

                    $("#chat_content")[0].scrollTop = $("#chat_content")[0].scrollHeight - $("#chat_content").height();
                }
            }

            //获取新加的最近聊天id
            if ($('#last-chat-id').val() == "") {
                var data = {};
                data.userId = $("#recievename").val();
                $.ajax({
                    url: "/message/getLastChatId",
                    type: "post",
                    dataType: "json",
                    data: data,
                    success: function (result) {
                        if (result.boo_success != false) {
                            $('#last-chat-id').val(result.boo_success);
                            //发送给指定的用户
                            chat.server.sendToGroup($('#recievename').val(), $('#displayname').val(), "我添加你为联系人啦~", $('#displayimg').val(), $('#last-chat-id').val());
                            $("#message").val("");
                            sendUser = "";
                        }
                        else {
                            //发送给指定的用户
                            chat.server.sendToGroup($('#recievename').val(), $('#displayname').val(), "我添加你为联系人啦~", $('#displayimg').val(), $('#last-chat-id').val());
                            $("#message").val("");
                            sendUser = "";
                        }
                    }
                });
            } else {
                //发送给指定的用户
                chat.server.sendToGroup($('#recievename').val(), $('#displayname').val(), "我添加你为联系人啦~", $('#displayimg').val(), $('#last-chat-id').val());
                $("#message").val(""); //清空输入框内容
                sendUser = "";
            }
        });

        $("#sendtoconnid").click(function () {
            chat.server.sendToConnectID($('#recieveid').val(), $('#displayname').val(), $('#message').val());
        });
    }).fail(function () {
        console.log("连接服务器失败.")
    });

})
function isIE() { //ie?  
    if (!!window.ActiveXObject || "ActiveXObject" in window)
        return true;
    else
        return false;
}

//查看方案高手私信记录
function selectSperiorLetter(recievename) {
    var bool;
    var data = {};
    data.userId = recievename;
    $.ajax({
        url: "/projectsuperior/SelectSuperiorLetter",
        type: "post",
        dataType: "json",
        data: data,
        success: function (result) {
            if (result.boo_success) {
                bool = true;
            }
            else {
                bool = false;
            }
        }
    });
    if (bool) {
        return true;
    }
    else {
        return false;
    }
}

//添加新的方案高手私信
function addSuperiorLetter(recievename) {
    var data = {};
    data.userId = recievename;
    $.ajax({
        url: "/projectsuperior/AddSuperiorLetter",
        type: "post",
        dataType: "json",
        data: data,
        success: function (result) {
            if (result.boo_success) {
                return;
            }
            else {
                alert("系统错误");
            }
        }
    });
}

//
function checkSperiorLetter(recievename) {
    var data = {};
    data.userId = recievename;
    $.ajax({
        url: "/projectsuperior/CheckSperiorLetter",
        type: "post",
        dataType: "json",
        data: data,
        success: function (result) {
            if (result.boo_success) {
                updateSperiorLetter($('#recievename').val(), message);
            }
            else {
                return;
            }
        }
    });
}

//
function updateSperiorLetter(recievename, message) {
    var data = {};
    data.userId = recievename;
    data.message = message;
    debugger;
    $.ajax({
        url: "/projectsuperior/UpdateSperiorLetter",
        type: "post",
        dataType: "json",
        data: data,
        success: function (result) {
            if (result.boo_success) {
                return;
            }
            else {
                alert("系统错误");
            }
        }
    });
}