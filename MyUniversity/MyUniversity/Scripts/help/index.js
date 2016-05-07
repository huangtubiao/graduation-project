//回复框
var listnode;
var replynode;
$(function () {
    $(".js-reply-ipt-default").focus(function () {
        $(this).addClass("pl-fake-focus");
    });
    $(".js-reply-ipt-default").blur(function () {
        $(this).removeClass("pl-fake-focus");
    });

    $(".help-ta").bind("click", function () {
        listnode = $(this).parents().eq(4);
        replynode = listnode.find(".reply");
        replynode.css("display", "block");
        replynode.find("textarea").select();
    });

    $(".btn-cancel").bind("click", function () {
        replynode.css("display", "none");
    });
});

//回复
function replysbumit() {
    var answerContent = replynode.find("textarea").val();
    var data = {};
    data.answerContent = answerContent;
    data.questionId = listnode.find(".questionId").val();
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
                    replynode.css("display", "none");
                }, 1000);
            }
            else if (result.boo_success == false) {
                alert("提交失败。");
            }
        }
    });
}

var totalGroups; //最新
var totalSecondGroups; //熱門
var totalThirdGroups;  //等待回復
var nowGroupNumber;  //当前所在的分页
var nowGroupSecondNumber;
var nowGroupThirdNumber;
var notiveLi;
$(document).ready(function () {
    $.ajax({
        url: "/help/ifLogin",
        type: "post",
        dataType: "json",
        success: function (result) {
            if (result.boo_success) {   
                var num = $(".wenda-listcon").length;
                for (var i = 0; i < num; i++) {
                    var img = $(".wenda-listcon").eq(i).find(".replyheadimg");
                    if (img[0].src != "") {
                        img.css({ "height": "20px", "width": "20px" });
                    }
                    var a = $(".wenda-listcon").eq(i).find(".nickname");
                    if (a.html() != "") {
                        var content = a.html() + ":";
                        a.text(content);
                    }
                }
                var replyNum = $(".reply-num");
                for (var i = 0; i < replyNum.length; i++) {
                    if (replyNum.eq(i).html() != "0") {
                        replyNum.eq(i).parents().eq(1).css("color", "#78CA99");
                    }
                }

                //获取各分页的总数量
                totalGroups = Math.ceil($("#all-newest").val() / 5);
                totalSecondGroups = Math.ceil($("#all-hot").val() / 5);
                totalThirdGroups = Math.ceil($("#all-waitReply").val() / 5);

                //判断搜索框是否为空
                var searchWords = $("#search-write").val();
                if (searchWords == "") {
                    $(".page a")[7].href = "/help/index/0/" + 2;
                    $(".page a")[8].href = "/help/index/0/" + totalGroups;
                    $(".pageSecond a")[7].href = "/help/index/1/" + 2;
                    $(".pageSecond a")[8].href = "/help/index/1/" + totalSecondGroups;
                    $(".pageThird a")[7].href = "/help/index/2/" + 2;
                    $(".pageThird a")[8].href = "/help/index/2/" + totalThirdGroups;
                }
                else if (searchWords != "") {
                    $(".page a")[3].href = "/help/index/0/" + 2 + "?" + "words=" + searchWords;
                    $(".page a")[4].href = "/help/index/0/" + 3 + "?" + "words=" + searchWords;
                    $(".page a")[5].href = "/help/index/0/" + 4 + "?" + "words=" + searchWords;
                    $(".page a")[6].href = "/help/index/0/" + 5 + "?" + "words=" + searchWords;
                    $(".page a")[7].href = "/help/index/0/" + 2 + "?" + "words=" + searchWords;
                    $(".page a")[8].href = "/help/index/0/" + totalGroups + "?" + "words=" + searchWords;

                    $(".pageSecond a")[3].href = "/help/index/1/" + 2 + "?" + "words=" + searchWords;
                    $(".pageSecond a")[4].href = "/help/index/1/" + 3 + "?" + "words=" + searchWords;
                    $(".pageSecond a")[5].href = "/help/index/1/" + 4 + "?" + "words=" + searchWords;
                    $(".pageSecond a")[6].href = "/help/index/1/" + 5 + "?" + "words=" + searchWords;
                    $(".pageSecond a")[7].href = "/help/index/1/" + 2 + "?" + "words=" + searchWords;
                    $(".pageSecond a")[8].href = "/help/index/1/" + totalSecondGroups + "?" + "words=" + searchWords;

                    $(".pageThird a")[3].href = "/help/index/2/" + 2 + "?" + "words=" + searchWords;
                    $(".pageThird a")[4].href = "/help/index/2/" + 3 + "?" + "words=" + searchWords;
                    $(".pageThird a")[5].href = "/help/index/2/" + 4 + "?" + "words=" + searchWords;
                    $(".pageThird a")[6].href = "/help/index/2/" + 5 + "?" + "words=" + searchWords;
                    $(".pageThird a")[7].href = "/help/index/2/" + 2 + "?" + "words=" + searchWords;
                    $(".pageThird a")[8].href = "/help/index/2/" + totalThirdGroups + "?" + "words=" + searchWords;
                }

                tabs();

                //判断当前url所在那个选项里,并显示出来
                var whichActive = request(3);
                if (whichActive == undefined) {
                    var t = $("#notice-tit").children("li");
                    t.eq(0).addClass("onactive");
                    $("#mod-1").css("display", "block");
                }
                if (whichActive == 0) {
                    var t = $("#notice-tit").children("li");
                    t.eq(0).addClass("onactive");
                    $("#mod-1").css("display", "block");
                }
                if (whichActive == 1) {
                    var t = $("#notice-tit").children("li");
                    t.eq(0).attr("class", "");
                    t.eq(1).addClass("onactive");
                    $("#mod-1").css("display", "none");
                    $("#mod-2").css("display", "block");

                }
                else if (whichActive == 2) {
                    var t = $("#notice-tit").children("li");
                    t.eq(2).addClass("onactive");
                    $("#mod-3").css("display", "block");
                }

                //加载数据
                if (notiveLi[0].className == "onactive") {   //最新
                    var pageA = $(".page a");
                    loadingMore(pageA, 0, totalGroups);
                    return;
                }
                if (notiveLi[1].className == "onactive") {  //热门
                    var pageA = $(".pageSecond a");
                    loadingMore(pageA, 1, totalSecondGroups);
                    return;
                }
                else if (notiveLi[2].className == "onactive") {   //等待回复
                    var pageA = $(".pageThird a");
                    loadingMore(pageA, 2, totalThirdGroups);
                    return;
                }
            }
            else if (!result.boo_success) {
                showlogin();
            }
        }
    });
});

//加载更多的数据》下一页
function loadingMore(pageA, active, totalGroups) {
    var searchWords = $("#search-write").val();
    if (searchWords == "") {
        var num = request(4);
        if (num == undefined) {
            pageA[2].className = "active";
            pageA[7].href = "/help/index/" + active + "/" + 2;
            pageA[8].href = "/help/index/" + active + "/" + totalGroups;
            return;
        }
        if (num == 1) {
            pageA[0].href = "javascript:void(0)";
            pageA[0].className = "removeCss";
            pageA[1].href = "javascript:void(0)";
            pageA[1].className = "removeCss";
            pageA[2].href = "javascript:void(0)";
            pageA[2].className = "active";

            pageA[7].href = "/help/index/" + active + "/" + (parseInt(num) + 1);   //下一页
            pageA[8].href = "/help/index/" + active + "/" + totalGroups;
            return;
        }
        if (num == 2) {
            pageA[0].href = "/help/index/" + active + "/" + 1;  //首页
            pageA[0].className = "";

            pageA[1].href = "/help/index/" + active + "/" + (num - 1);  //上一页
            pageA[1].className = "";

            pageA[2].href = "/help/index/" + active + "/" + (num - 1);
            pageA[2].className = "";
            pageA.eq(2).text(num - 1);

            pageA[3].href = "javascript:void(0)";   //目标页
            pageA[3].className = "active";
            pageA.eq(3).text(num);

            pageA[7].href = "/help/index/" + active + "/" + (parseInt(num) + 1);   //下一页
            pageA[8].href = "/help/index/" + active + "/" + totalGroups;
            return;
        }
        if (num > 2 && num < totalGroups - 1) {
            pageA[0].href = "/help/index/" + active + "/" + 1;   //首页
            pageA[0].className = "";

            pageA[1].href = "/help/index/" + active + "/" + (num - 1);  //上一页
            pageA[1].className = "";

            pageA[2].href = "/help/index/" + active + "/" + (num - 2);
            pageA[2].className = "";
            pageA.eq(2).text(num - 2);

            pageA[3].href = "/help/index/" + active + "/" + (num - 1);
            pageA[3].className = "";
            pageA.eq(3).text(num - 1);

            pageA[4].href = "javascript:void(0)";  //目标页
            pageA[4].className = "active";
            pageA.eq(4).text(num);

            pageA[5].href = "/help/index/" + active + "/" + (parseInt(num) + 1);
            pageA.eq(5).text(parseInt(num) + 1);

            pageA[6].href = "/help/index/" + active + "/" + (parseInt(num) + 2);
            pageA.eq(6).text(parseInt(num) + 2);

            pageA[7].href = "/help/index/" + active + "/" + (parseInt(num) + 1);   //下一页

            pageA[8].href = "/help/index/" + active + "/" + totalGroups;
            return;
        }
        if (num == (totalGroups - 1)) {
            pageA[0].href = "/help/index/" + active + "/" + 1;   //首页
            pageA[0].className = "";

            pageA[1].href = "/help/index/" + active + "/" + (num - 1);  //上一页
            pageA[1].className = "";

            pageA[2].href = "/help/index/" + active + "/" + (num - 3);
            pageA[2].className = "";
            pageA.eq(2).text(num - 3);

            pageA[3].href = "/help/index/" + active + "/" + (num - 2);
            pageA[3].className = "";
            pageA.eq(3).text(num - 2);

            pageA[4].href = "/help/index/" + active + "/" + (num - 1);
            pageA[4].className = "";
            pageA.eq(4).text(num - 1);

            pageA[5].href = "javascript:void(0)";  //目标页
            pageA[5].className = "active";
            pageA.eq(5).text(num);

            pageA[6].href = "/help/index/" + active + "/" + (parseInt(num) + 1);
            pageA.eq(6).text(parseInt(num) + 1);

            pageA[7].href = "/help/index/" + active + "/" + (parseInt(num) + 1);   //下一页

            pageA[8].href = "/help/index/" + active + "/" + totalGroups;
            return;
        }
        else if (num == totalGroups) {
            pageA[0].href = "/help/index/" + active + "/" + 1;   //首页
            pageA[0].className = "";

            pageA[1].href = "/help/index/" + active + "/" + (num - 1);  //上一页
            pageA[1].className = "";

            pageA[2].href = "/help/index/" + active + "/" + (num - 4);
            pageA[2].className = "";
            pageA.eq(2).text(num - 4);

            pageA[3].href = "/help/index/" + active + "/" + (num - 3);
            pageA[3].className = "";
            pageA.eq(3).text(num - 3);

            pageA[4].href = "/help/index/" + active + "/" + (num - 2);
            pageA[4].className = "";
            pageA.eq(4).text(num - 2);

            pageA[5].href = "/help/index/" + active + "/" + (num - 1);
            pageA.eq(5).text(num - 1);

            pageA[6].href = "javascript:void(0)";  //目标页
            pageA[6].className = "active";
            pageA.eq(6).text(num);

            pageA[7].href = "javascript:void(0)";   //下一页
            pageA[7].className = "removeCss";

            pageA[8].href = "javascript:void(0)";   //尾页
            pageA[8].className = "removeCss";
            return;
        }
    }
    else {   //搜索框不为空
        var num = request(4);
        num = num.substring(0, 1);
        if (num == undefined) {
            pageA[2].className = "active";
            pageA[3].href = "/help/index/" + active + "/2?" + "words=" + searchWords;
            pageA[4].href = "/help/index/" + active + "/3?" + "words=" + searchWords;
            pageA[5].href = "/help/index/" + active + "/4?" + "words=" + searchWords;
            pageA[6].href = "/help/index/" + active + "/5?" + "words=" + searchWords;
            pageA[7].href = "/help/index/" + active + "/6?" + "words=" + searchWords;
            pageA[8].href = "/help/index/" + active + "/" + totalGroups + "?words=" + searchWords;
            return;
        }
        if (num == 1) {
            pageA[0].href = "javascript:void(0)";
            pageA[0].className = "removeCss";
            pageA[1].href = "javascript:void(0)";
            pageA[1].className = "removeCss";
            pageA[2].href = "javascript:void(0)";
            pageA[2].className = "active";

            pageA[3].href = "/help/index/" + active + "/" + (parseInt(num) + 1) + "?words=" + searchWords;
            pageA[4].href = "/help/index/" + active + "/" + (parseInt(num) + 2) + "?words=" + searchWords;
            pageA[5].href = "/help/index/" + active + "/" + (parseInt(num) + 3) + "?words=" + searchWords;
            pageA[6].href = "/help/index/" + active + "/" + (parseInt(num) + 4) + "?words=" + searchWords;
            pageA[7].href = "/help/index/" + active + "/" + (parseInt(num) + 1) + "?word=" + searchWords;   //下一页
            pageA[8].href = "/help/index/" + active + "/" + totalGroups + "?word=" + searchWords;
            return;
        }
        if (num == 2) {
            pageA[0].href = "/help/index/" + active + "/" + 1 + "?words=" + searchWords;  //首页
            pageA[0].className = "";

            pageA[1].href = "/help/index/" + active + "/" + (num - 1) + "?words=" + searchWords;  //上一页
            pageA[1].className = "";

            pageA[2].href = "/help/index/" + active + "/" + (num - 1) + "?words=" + searchWords;;
            pageA[2].className = "";
            pageA.eq(2).text(num - 1);

            pageA[3].href = "javascript:void(0)";   //目标页
            pageA[3].className = "active";
            pageA.eq(3).text(num);

            pageA[4].href = "/help/index/" + active + "/" + (parseInt(num) + 1) + "?words=" + searchWords;
            pageA[5].href = "/help/index/" + active + "/" + (parseInt(num) + 2) + "?words=" + searchWords;
            pageA[6].href = "/help/index/" + active + "/" + (parseInt(num) + 3) + "?words=" + searchWords;
            pageA[7].href = "/help/index/" + active + "/" + (parseInt(num) + 1) + "?words=" + searchWords;   //下一页
            pageA[8].href = "/help/index/" + active + "/" + totalGroups + "?words=" + searchWords;
            return;
        }
        if (num > 2 && num < totalGroups - 1) {
            pageA[0].href = "/help/index/" + active + "/" + 1 + "?words=" + searchWords;  //首页
            pageA[0].className = "";

            pageA[1].href = "/help/index/" + active + "/" + (num - 1) + "?words=" + searchWords; //上一页
            pageA[1].className = "";

            pageA[2].href = "/help/index/" + active + "/" + (num - 2) + "?words=" + searchWords;
            pageA[2].className = "";
            pageA.eq(2).text(num - 2);

            pageA[3].href = "/help/index/" + active + "/" + (num - 1) + "?words=" + searchWords;
            pageA[3].className = "";
            pageA.eq(3).text(num - 1);

            pageA[4].href = "javascript:void(0)";  //目标页
            pageA[4].className = "active";
            pageA.eq(4).text(num);

            pageA[5].href = "/help/index/" + active + "/" + (parseInt(num) + 1) + "?words=" + searchWords;
            pageA.eq(5).text(parseInt(num) + 1);

            pageA[6].href = "/help/index/" + active + "/" + (parseInt(num) + 2) + "?words=" + searchWords;
            pageA.eq(6).text(parseInt(num) + 2);

            pageA[7].href = "/help/index/" + active + "/" + (parseInt(num) + 1) + "?words=" + searchWords;   //下一页

            pageA[8].href = "/help/index/" + active + "/" + totalGroups + "?words=" + searchWords;
            return;
        }
        if (num == (totalGroups - 1)) {
            pageA[0].href = "/help/index/" + active + "/" + 1 + "?words=" + searchWords;  //首页
            pageA[0].className = "";

            pageA[1].href = "/help/index/" + active + "/" + (num - 1) + "?words=" + searchWords;  //上一页
            pageA[1].className = "";

            pageA[2].href = "/help/index/" + active + "/" + (num - 3) + "?words=" + searchWords;
            pageA[2].className = "";
            pageA.eq(2).text(num - 3);

            pageA[3].href = "/help/index/" + active + "/" + (num - 2) + "?words=" + searchWords;
            pageA[3].className = "";
            pageA.eq(3).text(num - 2);

            pageA[4].href = "/help/index/" + active + "/" + (num - 1) + "?words=" + searchWords;
            pageA[4].className = "";
            pageA.eq(4).text(num - 1);

            pageA[5].href = "javascript:void(0)";  //目标页
            pageA[5].className = "active";
            pageA.eq(5).text(num);

            pageA[6].href = "/help/index/" + active + "/" + (parseInt(num) + 1) + "?words=" + searchWords;
            pageA.eq(6).text(parseInt(num) + 1);

            pageA[7].href = "/help/index/" + active + "/" + (parseInt(num) + 1) + "?words=" + searchWords;   //下一页

            pageA[8].href = "/help/index/" + active + "/" + totalGroups + "?words=" + searchWords;
            return;
        }
        else if (num == totalGroups) {
            pageA[0].href = "/help/index/" + active + "/" + 1 + "?words=" + searchWords;  //首页
            pageA[0].className = "";

            pageA[1].href = "/help/index/" + active + "/" + (num - 1) + "?words=" + searchWords; //上一页
            pageA[1].className = "";

            pageA[2].href = "/help/index/" + active + "/" + (num - 4) + "?words=" + searchWords;
            pageA[2].className = "";
            pageA.eq(2).text(num - 4);

            pageA[3].href = "/help/index/" + active + "/" + (num - 3) + "?words=" + searchWords;
            pageA[3].className = "";
            pageA.eq(3).text(num - 3);

            pageA[4].href = "/help/index/" + active + "/" + (num - 2) + "?words=" + searchWords;
            pageA[4].className = "";
            pageA.eq(4).text(num - 2);

            pageA[5].href = "/help/index/" + active + "/" + (num - 1) + "?words=" + searchWords;
            pageA.eq(5).text(num - 1);

            pageA[6].href = "javascript:void(0)";  //目标页
            pageA[6].className = "active";
            pageA.eq(6).text(num);

            pageA[7].href = "javascript:void(0)";   //下一页
            pageA[7].className = "removeCss";

            pageA[8].href = "javascript:void(0)";   //尾页
            pageA[8].className = "removeCss";
            return;
        }
    }
}

//获取当前url的参数
function request(paras) {
    var url = location.href;
    var paraString = url.substring(url.indexOf("/") + 2, url.length).split("/");
    return paraString[paras];
}

//分页
$(document).delegate(".page a", "click", function () {
    //if ($(this).attr("href") == "javascript:void(0)") {
    //    return;
    //}

    //var notiveLi = $("#notice-tit li");
    //if (notiveLi[0].className == "onactive") {
    //    if ($(this).text() == "首页") {
    //        nowGroupNumber = 1;
    //        getPageData(1, notiveLi.eq(0).children().text());
    //    }
    //    if ($(this).text() == "上一页") {
    //        nowGroupNumber = nowGroupNumber - 1;
    //        getPageData(nowGroupNumber, notiveLi.eq(0).children().text());
    //    }
    //    if ($(this).text() == "下一页") {
    //        nowGroupNumber = nowGroupNumber + 1;
    //        getPageData(nowGroupNumber, notiveLi.eq(0).children().text());
    //    }
    //    if ($(this).text() == "尾页") {
    //        nowGroupNumber = totalGroups;
    //        getPageData(totalGroups, notiveLi.eq(0).children().text());
    //    }
    //    else if($(this).text() >= 1 && $(this).text() <= totalGroups){
    //        nowGroupNumber = $(this).text();
    //        getPageData(nowGroupNumber, notiveLi.eq(0).children().text());

    //    }
    //}

});

//智能搜索提醒
var searchText = "";
var timer = null;
var img;
$("#search-write").bind("keyup", function (e) {
    if (e.keyCode != 40 && e.keyCode != 38 && e.keyCode != 13) {
        searchText = $("#search-write").val();
        if (timer) {
            clearTimeout(timer);
            timer = null;
        }
        timer = setTimeout(function () {
            $.get('/help/searchSuggest?words=' + searchText, function (result) {
                var html = "";
                if (result == "") {
                    $("#search-suggest").hide();
                    return;
                }
                img = "<i class=" + "iconfont" + ">" + "&#xe642" + "</i>";
                for (var i = 0; i < result.length; i++) {
                    html += "<li>" + img + "<a>" + result[i] + "</a>" + "</li>";
                }
                $("#search-result").html(html);
                $("#search-suggest").show();
            }, "json");
        }, 300);
    }
});

function btnSearch() {
    window.location.href = "/help/index?words=" + $("#search-write").val();
}

//显示问题信息
function showQuestions(result, nowActive) {
    var jsonObj = eval("(" + result + ")");
    for (var r in jsonObj) {
        var divList = document.createElement("div");
        divList.className = "wenda-list js-add-list";

        if (nowActive == "最新") {
            $("#mod-1").append(divList);
        }
        if (nowActive == "热门") {
            $("#mod-2").append(divList);
        }
        else if (nowActive == "等待回复") {
            $("#mod-3").append(divList);
        }

        var divListcon = document.createElement("div");
        divListcon.className = "wenda-listcon";
        divList.appendChild(divListcon);

        var divHead = document.createElement("div");
        divHead.className = "headslider";
        divListcon.appendChild(divHead);

        var a = document.createElement("a");
        a.className = "wenda-head";
        a.innerHTML += "<img src=" + jsonObj[r].userImg + ">";
        divHead.appendChild(a);

        var aNickname = document.createElement("a");
        aNickname.className = "wenda-nickname margin-right";
        aNickname.href += "/account/person/" + "5";
        aNickname.innerHTML += jsonObj[r].userName;
        divHead.appendChild(aNickname);

        var i = document.createElement("i");
        if (jsonObj[r].userSex == "女") {
            i.className = "iconfont girlsex-img";
            i.innerHTML += "&#xe639";
        }
        else {
            i.className = "iconfont sex-icon";
            i.innerHTML += "&#xe608";
        }
        divHead.appendChild(i);

        var divWendaslider = document.createElement("div");
        divWendaslider.className = "wendaslider";
        divListcon.appendChild(divWendaslider);

        var aWentime = document.createElement("a");
        aWentime.className = "wen-time";
        divWendaslider.appendChild(aWentime);

        var divHelpsum = document.createElement("div");
        divHelpsum.className = "help-sum";
        aWentime.appendChild(divHelpsum);

        var divNumbers = document.createElement("div");
        divNumbers.className = "numbers";
        divHelpsum.appendChild(divNumbers);

        var spanReplynum = document.createElement("span");
        spanReplynum.className = "reply-num";
        spanReplynum.innerHTML += jsonObj[r].questionReplyNum;
        divNumbers.appendChild(spanReplynum);

        var divHelpta = document.createElement("div");
        divHelpta.className = "help-ta";
        divHelpta.innerHTML += "帮助TA";
        divHelpsum.appendChild(divHelpta);

        var divBrowsenum = document.createElement("div");
        divBrowsenum.className = "browsenum";
        aWentime.appendChild(divBrowsenum);

        var divNumbers = document.createElement("div");
        divNumbers.className = "numbers";
        divBrowsenum.appendChild(divNumbers);

        var span = document.createElement("span");
        span.innerHTML += jsonObj[r].questionViewNum;
        divNumbers.appendChild(span);

        var div = document.createElement("div");
        div.innerHTML += "浏览";
        divBrowsenum.appendChild(div);

        var divWendaquetitle = document.createElement("div");
        divWendaquetitle.className = "wendaquetitle";
        divWendaslider.appendChild(divWendaquetitle);

        var i = document.createElement("i");
        i.className = "iconfont";
        i.innerHTML += "&#xe636";
        divWendaquetitle.appendChild(i);

        var divWendatitlecon = document.createElement("div");
        divWendatitlecon.className = "wendatitlecon";
        divWendaquetitle.appendChild(divWendatitlecon);

        var a = document.createElement("a");
        a.innerHTML += jsonObj[r].questionTitle;
        divWendatitlecon.appendChild(a);

        var divQuestion = document.createElement("div");
        divQuestion.className = "question";
        divWendaslider.appendChild(divQuestion);

        var i2 = document.createElement("i");
        i2.className = "iconfont";
        i2.innerHTML += "&#xe62d";
        divQuestion.appendChild(i2);

        var divReplydes = document.createElement("div");
        divReplydes.className = "replydes"
        divQuestion.appendChild(divReplydes);

        var aReplyuserhead = document.createElement("a");
        aReplyuserhead.className = "replyuserhead";
        aReplyuserhead.innerHTML += "<img src=" + jsonObj[r].newReplyUserImg + ">"
        divReplydes.appendChild(aReplyuserhead);

        var aNickname = document.createElement("a");
        aNickname.className = "nickname";
        aNickname.innerHTML += jsonObj[r].newReplyUserName;
        divReplydes.appendChild(aNickname);

        var spanReplycontent = document.createElement("span");
        spanReplycontent.className = "replycontent margin-left";
        spanReplycontent.innerHTML += jsonObj[r].questionNewestReply;
        divReplydes.appendChild(spanReplycontent);

        var divCommentfooter = document.createElement("div");
        divCommentfooter.className = "commentfooter";
        divWendaslider.appendChild(divCommentfooter);

        var spanFooterdepart = document.createElement("span");
        spanFooterdepart.className = "footer-depart";
        spanFooterdepart.innerHTML += jsonObj[r].userDepart;
        divCommentfooter.appendChild(spanFooterdepart);

        var divWendatime = document.createElement("div");
        divWendatime.className = "wenda-time";
        divCommentfooter.appendChild(divWendatime);

        var em = document.createElement("em");
        em.innerHTML += "提问时间：" + jsonObj[r].questionPublishedTime;
        divWendatime.appendChild(em);

        var inputQuestionId = document.createElement("input");
        inputQuestionId.type = "hidden";
        inputQuestionId.value = jsonObj[r].questionId;
        divCommentfooter.appendChild(inputQuestionId);
    }
    var allDivList = $(".js-add-list");
    for (var i = 0; i < allDivList.length; i++) {
        allDivList[i].appendChild($(".reply")[i]);
    }

    //底部分页
    var divPage = document.createElement("div");
    divPage.className = "page";
    if (nowActive == "最新") {
        $("#mod-1").append(divPage);
    }
    if (nowActive == "热门") {
        $("#mod-2").append(divPage);
    }
    else if (nowActive == "等待回复") {
        $("#mod-3").append(divPage);
    }


    var aHomePage = document.createElement("a");
    aHomePage.innerHTML += "首页";
    divPage.appendChild(aHomePage);

    var aLastPage = document.createElement("a");
    aLastPage.innerHTML += "上一页";
    divPage.appendChild(aLastPage);


    if (nowGroupNumber == 1) {  //说明点击了首页
        var aFirstPage = document.createElement("a");
        aFirstPage.innerHTML += "1";
        aFirstPage.className = "active";
        divPage.appendChild(aFirstPage);

        var aSecondPage = document.createElement("a");
        aSecondPage.innerHTML += "2";
        divPage.appendChild(aSecondPage);

        var aThirdPage = document.createElement("a");
        aThirdPage.innerHTML += "3";
        divPage.appendChild(aThirdPage);

        var aFourthPage = document.createElement("a");
        aFourthPage.innerHTML += "4";
        divPage.appendChild(aFourthPage);

        var aFifPage = document.createElement("a");
        aFifPage.innerHTML += "5";
        divPage.appendChild(aFifPage);
    }
    if (nowGroupNumber == 2) {
        var aFirstPage = document.createElement("a");
        aFirstPage.innerHTML += "1";
        divPage.appendChild(aFirstPage);

        var aSecondPage = document.createElement("a");
        aSecondPage.innerHTML += "2";
        aSecondPage.className = "active";
        divPage.appendChild(aSecondPage);

        var aThirdPage = document.createElement("a");
        aThirdPage.innerHTML += "3";
        divPage.appendChild(aThirdPage);

        var aFourthPage = document.createElement("a");
        aFourthPage.innerHTML += "4";
        divPage.appendChild(aFourthPage);

        var aFifPage = document.createElement("a");
        aFifPage.innerHTML += "5";
        divPage.appendChild(aFifPage);
    }
    if (nowGroupNumber > 2 && nowGroupNumber <= (totalGroups - 2)) {
        var aFirstPage = document.createElement("a");
        aFirstPage.innerHTML += (nowGroupNumber - 2);
        divPage.appendChild(aFirstPage);

        var aSecondPage = document.createElement("a");
        aSecondPage.innerHTML += nowGroupNumber - 1;
        divPage.appendChild(aSecondPage);

        var aActivePage = document.createElement("a");
        aActivePage.innerHTML += nowGroupNumber;
        aActivePage.className = "active";
        divPage.appendChild(aActivePage);

        var aFourthPage = document.createElement("a");
        aFourthPage.innerHTML += (parseInt(nowGroupNumber) + 1);
        divPage.appendChild(aFourthPage);

        var aFifPage = document.createElement("a");
        aFifPage.innerHTML += (parseInt(nowGroupNumber) + 2);
        divPage.appendChild(aFifPage);
    }
    if (nowGroupNumber == totalGroups - 1) {
        var aFirstPage = document.createElement("a");
        aFirstPage.innerHTML += "1";
        divPage.appendChild(aFirstPage);

        var aSecondPage = document.createElement("a");
        aSecondPage.innerHTML += "2";
        divPage.appendChild(aSecondPage);

        var aThirdPage = document.createElement("a");
        aThirdPage.innerHTML += "3";
        divPage.appendChild(aThirdPage);

        var aFourthPage = document.createElement("a");
        aFourthPage.innerHTML += "4";
        aFourthPage.className = "active";
        divPage.appendChild(aFourthPage);

        var aFifPage = document.createElement("a");
        aFifPage.innerHTML += "5";
        divPage.appendChild(aFifPage);
    }
    else if (nowGroupNumber == totalGroups) {
        var aFirstPage = document.createElement("a");
        aFirstPage.innerHTML += "1";
        divPage.appendChild(aFirstPage);

        var aSecondPage = document.createElement("a");
        aSecondPage.innerHTML += "2";
        divPage.appendChild(aSecondPage);

        var aThirdPage = document.createElement("a");
        aThirdPage.innerHTML += "3";
        divPage.appendChild(aThirdPage);

        var aFourthPage = document.createElement("a");
        aFourthPage.innerHTML += "4";
        divPage.appendChild(aFourthPage);

        var aFifPage = document.createElement("a");
        aFifPage.innerHTML += "5";
        aFifPage.className = "active";
        divPage.appendChild(aFifPage);
    }

    var aNextPage = document.createElement("a");
    aNextPage.innerHTML += "下一页";
    divPage.appendChild(aNextPage);

    var aEndPage = document.createElement("a");
    aEndPage.innerHTML += "尾页";
    divPage.appendChild(aEndPage);
}

