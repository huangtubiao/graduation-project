define(function(require,exports,module){

	var $ = require('$'),
        Mock = require('Mock'),
        Random = Mock.Random;
    Random.cname();
    // Random.title(2);
    Random.url();
	var data = {
		index: function(){
			Mock.mock(/dynamic/,'get',{
				"list|9": [{
        			"user_name": "@cname",
        			"user_link": "@url",
        			'oprate': '订阅了',
        			"op_aim": "将英国的历史讲座",
        			"op_aim_link": "@url",
        			"time": "1分钟前"
        		}] 
			});
		    Mock.mock(/rank/,'get',function(options){
		        return {};
		    });
		}
	};

	module.exports = data;

})