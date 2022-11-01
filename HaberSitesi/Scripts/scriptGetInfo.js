////$.ajax({
////    method: 'POST',
////    type: 'json',
////    data: {
////        name: "Süleyman Çetiner",
////        phone: "555-555-55-55"
////    },
////    url: 'http://localhost:8000/test/',
////    success: function (response) {
////        console.log(response);
////    },
////    error: function (error) {
////        console.log("hata ajax",error);
////    }
////});


let fullUrl = window.location.href;
let pathname = window.location.pathname;
let href = pathname.slice(1, fullUrl.length);
let findCategory = "HomePage or Menus";
let newsCategory ="home page or menus";
if (href !== null || href !== "") {
    findCategory = href.split("/")[0];
    newsCategory = findCategory.split("-")[0];
}



let postObj = {
    url: fullUrl,
    pathname: pathname,
    href: href,
    category: findCategory,
   
}
let post = JSON.stringify(postObj)

const url = "http://localhost:8000/test/"
let xhr = new XMLHttpRequest()

xhr.open('POST', url, true)
xhr.setRequestHeader('Content-type', 'application/json; charset=UTF-8')
xhr.send(post);

xhr.onload = function () {
    if (xhr.status === 201) {
        console.log("Post successfully created!")
    }
}