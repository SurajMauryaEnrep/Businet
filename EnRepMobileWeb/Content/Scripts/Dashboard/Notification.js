$(function () {
    getdata();
    //var chat = $.connection.notificationHub;
    //chat.client.displayCustomer = function () {
    //    getdata();
    //}
    //$.connection.hub.start().done(function () {
    //    getdata();
    //});
    //$.connection.hub.start();
});
function getdata() {
    /* Request Process changed by Suraj Maurya on 05-02-2025 to fetch from Ajax due Loader Start on ajaxStart */
    fetch('/Dashboard/Notification/GetAllUnreadNotifications', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
    })
        .then(response => response.json())  // Parse the JSON response
        .then((data) => {

            if (data == 'ErrorPage') {
                //PO_ErrorPage();
                return false;
            }
            var $div = $('#innerul');
            if (data !== null && data !== "") {
                var arr = [];
                arr = JSON.parse(data);

                var oldDataCount = $("#lblnotificationcount").html();
                $("#lblnotificationcount").html(arr.length);

                $div.empty();
                if (arr.length > 0) {
                    var rowIdx = 0;

                    for (var i = 0; i < arr.length; i++) {
                        var msglist = `<li class="nav-item" onclick="ChangenotificationRStatus(${arr[i]["row_no"]})">
<a class="dropdown-item" id='${arr[i]["row_no"]}'> <span class="image"><img src="/Content/Images/bell_icon.jpg" alt="Profile Image">
</span> <span>${arr[i]["DocName"]}</span> <span class="time">${arr[i]["TimeSpan"]}</span>
<span class="message"> ${arr[i]["msg_desc"]} </span>
</a> </li>`
                        $div.append(msglist);
                    }
                }
                //console.log(data);
            }
        })    // Handle the returned data
        .catch(error => console.log('Error:', error));  // Handle any errors
    /*------------------------------------Old Code with ajax Start-------------------------------------*/
//    $.ajax({
//        type: "POST",
//        url: "/Dashboard/Notification/GetAllUnreadNotifications",
//        data: {},
//        dataType: "json",
//        success: function (data) {

//            if (data == 'ErrorPage') {
//                //PO_ErrorPage();
//                return false;
//            }
//            //var $div = $('#anchralerts');
//            var $div = $('#innerul');
//            if (data !== null && data !== "") {
//                var arr = [];
//                arr = JSON.parse(data);

//                var oldDataCount = $("#lblnotificationcount").html();
//                $("#lblnotificationcount").html(arr.length);
//                // if (parseInt(oldDataCount) != parseInt(arr.length)) {
//                //alert(arr);
//		$div.empty();
//                if (arr.length > 0) {
//                    var rowIdx = 0;
                    
//                    for (var i = 0; i < arr.length; i++) {
//                        //$div.append('<span class="image"><img style="height:20px;width:20px;" src="/Content/Images/bell_icon.jpg" alt="." /></span>'
//                        //    + '<a id=' + arr[i]["row_no"] + ' class="message" onclick="ChangenotificationRStatus(' + arr[i]["row_no"] + ')" > ' + arr[i]["msg_desc"] + ' </span>'
//                        //    + '<br /><br />'
//                        //);
//                        var msglist = `<li class="nav-item" onclick="ChangenotificationRStatus(${arr[i]["row_no"]})">
//<a class="dropdown-item" id='${arr[i]["row_no"]}'> <span class="image"><img src="/Content/Images/bell_icon.jpg" alt="Profile Image">
//</span> <span>${arr[i]["DocName"]}</span> <span class="time">${arr[i]["TimeSpan"]}</span>
//<span class="message"> ${arr[i]["msg_desc"]} </span>
//</a> </li>`
//                        $div.append(msglist);
//                    }
//                }
//                //}
//                //else {
//                //    if (arr.length == 0) {
//                //        $div.empty();
//                //    }
//            }
//        }
//        //});
//    });
    /*------------------------------------Old Code with ajax End-------------------------------------*/

}
function ChangenotificationRStatus(id) {
    /*var id = $(this).attr("id");*/
    $.ajax({
        url: "/Dashboard/Notification/UpdateReadStatus",
        type: "POST",
        data: { id: id },
        success: function (response) {

        }
    });
    getdata();
}