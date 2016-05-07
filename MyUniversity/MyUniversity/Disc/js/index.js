define(function(require,exports,module){
    // 获取依赖
	var $ = require('$');
        Mock = require('Mock'),
        Random = Mock.Random;

    // 生成模拟数据
    Mock.mock(/dynamic/,'get',{
        "list|9": [{
            "user_name": "@cname",
            "user_link": "@url",
            'oprate': '订阅了',
            "op_aim": "@title(2)",
            "op_aim_link": "@url",
            "time": "1分钟前"
        }] 
    });
    Mock.mock(/rank\/today/,'get',{
        "list|10": [{
            "user_link": "@url",
            "user_img_url": "img/user.jpg",
            "user_name": "@cname",
            "score|1-200": 100
        }]
    });
    Mock.mock(/rank\/all/,'get',{
        "list|10": [{
            "user_link": "@url",
            "user_img_url": "img/user.jpg",
            "user_name": "@cname",
            "score|1000-5000": 100
        }]
    });

    // 获取最新动态数据
    var dynamic = {}, //最新动态
        rank_today = {}, //今日排名列表
        rank_all = {}; //综合排名列表

    // 给每个空对象添加data属性，并把数据存在list中
    var getData = function(url,type,data_name){
        $.ajax({
            url: url,
            type: type,
            dataType: 'json',
            async: false
        }).done(function(re){
            data_name.data = re;
        }).fail(function(){
            console.log('出错了！')
        });
    };

    getData('dynamic','get',dynamic);
    getData('rank/today','get',rank_today);
    getData('rank/all','get',rank_all);

    // 分数从大到小排序
    var rank_today_list = rank_today.data.list,
        rank_all_list = rank_all.data.list;

    rank_today_list.sort(function(a,b){return b.score - a.score;});
    rank_all_list.sort(function(a,b){return b.score - a.score;});

    module.exports = {
        dynamic: dynamic.data,
        rank_today: rank_today.data,
        rank_all: rank_all.data
    };
    
});