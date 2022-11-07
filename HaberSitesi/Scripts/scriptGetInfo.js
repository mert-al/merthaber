
///*
// *gerekli datalar
// * _id +
// * usertokenid +
// * country 
// * city
// * signtime
// * category = ekonomi +
// * news = thy-bazi-yurt-disi-ucuslarini-durdurma-kararini-uzatti=1" +
// * browser = chrome + 
// * OS = windows 
// * isMobile = false
// * device = computer
// * duration = 
// * created at 
// * */




let fullurl = window.location.href;
let pathname = window.location.pathname;
let href = pathname.slice(1, fullurl.length);
let category = "";
let subcategory = "";
let userbrowser = "";
let newsTitle = "";
let data;
console.log(newsTitle);

if (href.includes("video") || href.includes("galeri")) {
    category = href.split("/")[0];

    if (category != null || category != undefined) {

        subcategory = href.split("/")[1];
        newsTitle = href.split("/")[2];
        console.log("news kategory", subcategory);
    }
    else if (category == null || category == undefined) {
        category = href;
        console.log("category", category);
    }
}
else if (href !== null || href !== "") {
    category = undefined;
    newsTitle = href.split("/")[1];
    subcategory = href.split("/")[0];
  

}


var userBrowser;
document.addEventListener("DOMContentLoaded", async function () {
    var locationURL = window.location.href

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
        userBrowser = "unknown"
    }

    // Data Post
    $.ajax({
        method: 'POST',
        type: 'json',
        data:{
            Url: fullurl,
            Title: newsTitle,
            Category: category,
            Subcategory: subcategory,
            browser: userBrowser,
            userKey: localStorage.getItem("userKey") ? localStorage.getItem("userKey") : "",
            category: category

            // key: localStorage.getItem("userKey") ? localStorage.getItem("userKey") : null,
        },
        url: 'http://localhost:3434/',
        success: function (response) {
            localStorage.setItem("userKey", response.key);
        },
        error: function (error) {
            console.log(error);
        },
        headers: {
            'Access-Control-Allow-Origin': '*',
        },
    });


})









// frontend()

//  async function frontend () {
//     var userIPV4;

//     await fetch("https://api.ipify.org/?format=json")
//         .then((result) => {
//             console.log(result)
//         });

//     console.log()

//     // Data Post
//     $.ajax({
//         url: 'http://localhost:3434/test',
//         method: 'POST',
//         type: 'json',
//         dataType: 'jsonp',
//         cors: true ,
//         contentType:'application/json',
//         secure: true,
//         headers: {
//             'Access-Control-Allow-Origin': '*',
//           },
//         data: {
//             name: "Süleyman Çetiner",
//             phone: "555-555-55-55",
//             userIP: userIPV4
//         },
//         success: function (response) {
//             console.log(response);
//         },
//         error: function (error) {
//             console.log(error);
//         }
//     });
// }





