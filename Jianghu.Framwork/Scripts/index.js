$(function () {
    //首页新闻滚动开始
    var cur_ls = 0;         //保存当前索引，轮播到第几张
    var t_ls;               //时间句柄
    var timeout_ls = 2000;  //轮播间隔时间
    $(".buttons li").each(function (i) {
        $(this).mouseover(function () {  //鼠标放上
            clearTimeout(t_ls);
            $(".buttons li").removeClass("hover");   //先去掉所有li的class
            $(this).addClass("hover");               //再给当前li加上class
            $(".scrollbigimg").stop().animate({ top: (-i * 234) + "px" }, "normal");
            cur_ls = i;       //保存当前索引，轮播到第几张
        });
        $(this).mouseout(function () {  //鼠标离开
            t_ls = setTimeout(fn, timeout_ls);
        });
    });

    $(".buttons li").eq(0).mouseover();  //先对第一个按钮执行一次mouseover
    t_ls = setTimeout(fn, timeout_ls);
    function fn() {
        var nextIndex = cur_ls + 1;
        if (nextIndex == $(".buttons li").length) {
            nextIndex = 0;
        }
        $(".buttons li").eq(nextIndex).mouseover();
        t_ls = setTimeout(fn, timeout_ls);
    }

    $(".scrollbigimg a img").mouseover(function () {  //大图鼠标放上停止滚动
        clearTimeout(t_ls);
    })
    $(".scrollbigimg a img").mouseout(function () {   //大图鼠标离开恢复滚动
        t_ls = setTimeout(fn, timeout_ls);
    })
    //首页新闻滚动结束



    //新闻滑动门开始
    $(".lists .tit span").bind("mousemove", function () {
        $(".lists .tit span").removeClass("on");
        $(this).addClass("on");

        $(".lists .lists_con").css({ display: "none" });
        $(".lists .lists_con").eq($(this).index()).css({ display: "block" });

    })
    if ($(".lists .tit span")) {
        $(".lists .tit span").eq(0).mousemove()
    }
    //新闻滑动门结束



    //三个职业按钮开始
    $(".zhiye .zhiye_s a").bind("click", function () {
        $(".zhiye .lili").css({ display: "none" });
        $(".zhiye .lili").eq($(this).index()).css({ display: "block" });

    })
    if ($(".zhiye .zhiye_s a")) {
        $(".zhiye .zhiye_s a").eq(1).click()
    }
    //三个职业按钮结束
})