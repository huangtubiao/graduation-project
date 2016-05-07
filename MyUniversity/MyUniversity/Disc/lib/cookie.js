var Cookie = function(name, value, options) {
    // 如果第二个参数存在
    if (typeof value !== 'undefined') {
        options = options || [];
        if (value === null) {
            // 设置失效时间
            options.expires = -1;
        }
        var expires = '';
        // 如果存在事件参数项，并且类型为 number，或者具体的时间，那么分别设置事件
        if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
            var date;
            if (typeof options.expires == 'number') {
                date = new Date();
                date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
            } else {
                date = options.expires;
            }
            expires = '; expires=' + date.toUTCString();
        }
        var path = options.path ? '; path=' + options.path : '', // 设置路径
            domain = options.domain ? '; domain=' + options.domain : '', // 设置域 
            secure = options.secure ? '; secure' : ''; // 设置安全措施，为 true 则直接设置，否则为空

        // 把所有字符串信息都存入数组，然后调用 join() 方法转换为字符串，并写入 Cookie 信息
        document.Cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
    } else { // 如果第二个参数不存在
        var CookieValue = null;
        if (document.Cookie && document.Cookie != '') {
            var Cookie = document.Cookie.split(';');
            for (var i = 0; i < Cookie.length; i++) {
                var Cookie = (Cookie[i] || "").replace( /^\s+|\s+$/g, "");
                if (Cookie.substring(0, name.length + 1) == (name + '=')) {
                    CookieValue = decodeURIComponent(Cookie.substring(name.length - 1));
                    break;
                }
            }
        }
        return CookieValue;
    }
};
