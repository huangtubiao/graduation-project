$(document).ready(function () {
    tabs();

    $("#superior_param_form").ajaxForm({
        success: function (data) {
            if (data.boo_success) {
                $("#superior-success").fadeIn("fast");
                $(".modal-backdrop").css("display", "block");
                setTimeout(function () {
                    $("#superior-success").css("display", "none");
                    $(".modal-backdrop").css("display", "none");
                },1100);
            }
        }
    });
    //修改基本信息
    $("#generalInfo_param_form").ajaxForm({
        success: function (data) {
            if (data.boo_success) {
                $("#superior-success").fadeIn("fast");
                $(".modal-backdrop").css("display", "block");
                setTimeout(function () {
                    $("#superior-success").css("display", "none");
                    $(".modal-backdrop").css("display", "none");
                }, 1100);
            }
        }
    });
    //修改教育信息
    $("#education_param_form").ajaxForm({
        success: function (data) {
            if (data.boo_success) {
                $("#superior-success").fadeIn("fast");
                $(".modal-backdrop").css("display", "block");
                setTimeout(function () {
                    $("#superior-success").css("display", "none");
                    $(".modal-backdrop").css("display", "none");
                }, 1100);
            }
        }
    });

    //学校选择
    $("#school").click(function () {
        $(".modal-backdrop").css("display", "block");
        $("#myModal").addClass("in");
        $("#myModal").fadeIn("fast");
        $.get('/account/GetNearSchools', function (result) {
            var html = "";
            for (var i = 0; i < result.length; i++) {
                html += "<li>" + "<a>" + result[i] + "</a>" + "</li>";
            }
            $("#near-schools").empty();
            $("#near-schools").append(html);
        }, "json");
    });
    $(".school-show ul li a").bind("click", function () {
        $("#school").val($(this).text());
        $("#myModal").removeClass("in");
        $("#myModal").css("display", "none");
        //根据学校查询出相应院系
        GetDepartBySchool($(this).text());
        $("#myModalDepart").addClass("in");
        $("#myModalDepart").fadeIn("fast");
    });
    //附近学校的选择
    $(document).delegate("#near-schools li a", "click", function () {
        $("#school").val($(this).text());
        $("#myModal").removeClass("in");
        $("#myModal").css("display", "none");
        //根据学校查询出相应院系
        GetDepartBySchool($(this).text());
        $("#myModalDepart").addClass("in");
        $("#myModalDepart").fadeIn("fast");
    });

    //院系选择
    $("#depart").click(function () {
        $(".modal-backdrop").css("display", "block");
        $("#myModalDepart").addClass("in");
        $("#myModalDepart").fadeIn("fast");
        //根据学校查询出相应院系
        GetDepartBySchool($("#school").val());
    });
    $(document).delegate(".department-show ul li a", "click", function () {
        $("#depart").val($(this).text());
        $(".modal-backdrop").css("display", "none");
        $("#myModalDepart").removeClass("in");
        $("#myModalDepart").css("display", "none");

    });


    $(".close").click(function () {
        $(".modal-backdrop").css("display", "none");
        $("#myModal").removeClass("in");
        $("#myModal").css("display", "none");
        $("#myModalDepart").removeClass("in");
        $("#myModalDepart").css("display", "none");
    });
});

//根据学校查询出相应院系
function GetDepartBySchool(school) {
    $.get('/account/GetDepartBySchool?schoolName=' + school, function (result) {
        var html = "";
        for (var i = 0; i < result.length; i++) {
            html += "<li>" + "<a>" + result[i] + "</a>" + "</li>";
        }
        $("#depart-add").empty();
        $("#depart-add").append(html);
    }, "json");
}

//搜索学校
var searchText = "";
var timer = null;
$("#search-write").bind("keyup", function (e) {
    if (e.keyCode != 40 && e.keyCode != 38 && e.keyCode != 13) {
        searchText = $("#search-write").val();
        if (timer) {
            clearTimeout(timer);
            timer = null;
        }
        timer = setTimeout(function () {
            $.get('/account/SchoolSearch?searchText=' + searchText, function (result) {
                var html = "";
                if (result == "") {
                    $("#search-school").hide();
                    $("#school-show").css("display", "block");
                    return;
                }
                for (var i = 0; i < result.length; i++) {
                    html += "<li>" + "<a>" + result[i] + "</a>" + "</li>";
                }
                $("#search-result").html(html);
                $("#school-show").css("display", "none");
                $("#search-school").show();
            }, "json");
        }, 300);
    }
});

//搜索院系
var searchDepart = "";
var timerr = null;
$("#write-depart").bind("keyup", function (e) {
    if (e.keyCode != 40 && e.keyCode != 38 && e.keyCode != 13) {
        searchDepart = $("#write-depart").val();
        if (timer) {
            clearTimeout(timer);
            timer = null;
        }
        timer = setTimeout(function () {
            $.get('/account/DepartSearch?searchDepart=' + searchDepart + "&mySchool=" + school, function (result) {
                var html = "";
                if (result == "") {
                    $("#search-depart").hide();
                    $("#depart-add").css("display", "block");
                    return;
                }
                for (var i = 0; i < result.length; i++) {
                    html += "<li>" + "<a>" + result[i] + "</a>" + "</li>";
                }
                $("#search-depart").html(html);
                $("#depart-add").css("display", "none");
                $("#search-depart").show();
            }, "json");
        }, 300);
    }
});

//学校搜索智能提醒点击搜索
$(document).delegate("#search-result li a", "click", function () {
    $("#school").val($(this).text());
    $("#myModal").removeClass("in");
    $("#myModal").css("display", "none");
    //根据学校查询出相应院系
    GetDepartBySchool($(this).text());
    $("#myModalDepart").addClass("in");
    $("#myModalDepart").fadeIn("fast");
});