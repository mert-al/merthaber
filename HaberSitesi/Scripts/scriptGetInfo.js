

let fullUrl = window.location.href;
let pathname = window.location.pathname;
let href = pathname.slice(1, fullUrl.length);
let findCategory = "HomePage or Menus";
let newsCategory = "home page or menus";
let userBrowser = "";
if (href !== null || href !== "") {
    findCategory = href.split("/")[0];
    newsCategory = findCategory.split("-")[0];
}

if ((navigator.userAgent.indexOf("Opera") || navigator.userAgent.indexOf('OPR')) != -1) {
    userBrowser = "Opera"
}
else if (navigator.userAgent.indexOf("Edg") != -1) {
    userBrowser = "Edge"
}
else if (navigator.userAgent.indexOf("Chrome") != -1) {
    userBrowser = "Chrome"
}
else if (navigator.userAgent.indexOf("Safari") != -1) {
    userBrowser = "Safari"
}
else if (navigator.userAgent.indexOf("Firefox") != -1) {
    userBrowser = "Firefox"
}
else if ((navigator.userAgent.indexOf("MSIE") != -1) || (!!document.documentMode == true)) //IF IE > 10
{
    userBrowser = "IE"
}
else {
    userBrowser = "unknow"
}


/*
 *gerekli datalar
 * _id
 * usertokenid
 * country
 * city
 * signtime
 * category = ekonomi +
 * news = thy-bazi-yurt-disi-ucuslarini-durdurma-kararini-uzatti=1" +
 * browser = chrome + 
 * OS = windows 
 * isMobile = false
 * device = computer
 * duration = 
 * created at 
 * 
 * */



let postObj = {
    url: fullUrl,
    pathname: pathname,
    href: href,
    category: findCategory,
    browser: userBrowser,
   
}

$.ajax({
    method: 'POST',
    type: 'json',
    data: postObj,
    url: 'http://localhost:8000/test/',
    success: function (response) {
        console.log(response);
    },
    error: function (error) {
        console.log("hata ajax", error);
    }
});

//let post = JSON.stringify(postObj)

//const url = "http://localhost:8000/test/"
//let xhr = new XMLHttpRequest()

//xhr.open('POST', url, true)
//xhr.setRequestHeader('Content-type', 'application/json; charset=UTF-8')
//xhr.send(post);

//xhr.onload = function () {
//    if (xhr.status === 201) {
//        console.log("Post successfully created!")
//    }
//}