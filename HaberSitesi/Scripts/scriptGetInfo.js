
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
// * 
// * */






let fullurl = window.location.href;
let pathname = window.location.pathname;
let href = pathname.slice(1, fullurl.length);
let findcategory = "homepage or menus";
let newscategory = "homepage or menus";
let userbrowser = "";

if (href.includes("video") || href.includes("galeri")) {
    findcategory = href.split("/")[1];
    if (findcategory != null || findcategory != undefined) {
        newscategory = findcategory;
        console.log("news kategory", newscategory);
    }
    else if (findcategory == null || findcategory == undefined) {
        findcategory = href;
        console.log("findcategory", findcategory);
    }


}
if (href !== null || href !== "") {
    findcategory = href.split("/")[0];
    newscategory = findcategory.split("/")[0];
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
        userBrowser = "unknow"
    }


    // Data Post
    $.ajax({
        method: 'POST',
        type: 'json',
        data: {
            name: "Süleyman Çetiner",
            phone: "555-555-55-55",
            browser: userBrowser,
            userKey: localStorage.getItem("userKey") ? localStorage.getItem("userKey") : "",
            newsTitle: href,
            category: findcategory

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





