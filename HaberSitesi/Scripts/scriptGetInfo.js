var userBrowser;
document.addEventListener("DOMContentLoaded", async function () {
    var width = window.outerWidth;
    var height = window.outerHeight;
    var referrer = document.referrer;
    if (referrer === "") {
        referrer = "withURI"
    }
    var fullurl = window.location.href;
    var pathName = window.location.pathname;
    var href = pathName.slice(1, fullurl.length);
    var category = "";
    var subCategory = "";
    var newsTitle = "";
    if (href.includes("video") || href.includes("galeri")) {
        category = href.split("/")[0];
        if (category != null || category != undefined) {
            subCategory = href.split("/")[1];
            newsTitle = href.split("/")[2];
        }
        else if (category == null || category == undefined) {
            category = href;
        }
    }
    else if (href !== null || href !== "") {
        category = undefined;
        newsTitle = href.split("/")[1];
        subCategory = href.split("/")[0];
    }

    if (subCategory === "" || subCategory === undefined) {
        subCategory = undefined;
    }
   
    //var ONE_SECOND = 1000;
    //var totalTime = 0;
    //var lastTime = 0;
    //setInterval(function () {
    //    if (!document.hidden) {
    //        lastTime = totalTime + ONE_SECOND;
    //    }
    //}, ONE_SECOND);
    //function formatTime(ms) {
    //    return Math.floor(ms / 1000);
    //}

    // Data Post
    $.ajax({
        method: 'POST',
        type: 'json',
        data: {
            /* browser: userBrowser,*/
            userKey: localStorage.getItem("userKey") ? localStorage.getItem("userKey") : "",
            referrer: referrer,
            url: fullurl,
            title: newsTitle,
            category: category,
            subCategory: subCategory,
            screenWidth: width,
            screenHeight: height
            //key: localStorage.getItem("userKey") ? localStorage.getItem("userKey") : null,
        },
        url: 'http://localhost:3434/',
        success: function (response) {
            if (!localStorage.getItem("userKey")) {
                localStorage.setItem("userKey", response.key);
            }
        },
        error: function (error) {
            console.log(error);
        }
    });
})