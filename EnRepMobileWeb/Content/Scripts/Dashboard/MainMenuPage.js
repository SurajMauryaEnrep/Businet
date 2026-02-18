/************************************************
Javascript Name:MainMenuPage
Created By:Prem
Created Date: 02-10-2020
Description: This Javascript use for the dynamic menu generation

Modified By:
Modified Date:
Description:

*************************************************/
$(document).ready(function () {
    debugger;
    var branchVal = $('#hdBranchValue').val();
    if (branchVal == "" || branchVal == "0") {
        $('#TopNavBranchList').val(0);

        var Br_ID = $('#hdBR_ID').val();
        if (Br_ID != "" && Br_ID != null) {
            $('#TopNavBranchList').val(Br_ID).prop('selected', true);
        }
    }
    else {
        $('#TopNavBranchList').val(branchVal);
    }
    $("#search_menu").keyup(function () {
        var filter = $(this).val(),
            count = 0;
        debugger;
        $("#sidebar-menu li").each(function () {
            if (filter == "") {
                $(this).css("visibility", "visible");
                $(this).fadeIn();
            } else if ($(this).text().search(new RegExp(filter, "i")) < 0) {
                $(this).css("visibility", "hidden");
                $(this).fadeOut();
            } else {
                $(this).css("visibility", "visible");
                $(this).fadeIn();
            }
        });
    });
    jQuery(document).trigger('jquery.loaded');

    //(Add/Remove My Fav)
    $('.addcart').on('contextmenu', function (e) {
        var docMenuId = this.id;
        var closestHiddenField = $(this).closest('li').find('.hidden-field').val();
        debugger;
        $.ajax({
            type: 'POST',
            url: '/DashboardHome/CheckMyFavMenu',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: JSON.stringify({ docId: this.id }),
            success: function (Objdata) {
                if (Objdata != 'exist' || closestHiddenField == "100") {
                    e.preventDefault();
                    debugger;
                    // var closestHiddenField = $(this).closest('li').find('.hidden-field').val();
                    /*alert(closestHiddenField);*/
                    if (closestHiddenField == "100") {
                        $('#hdnAddOrRemoveCart').val('remove');
                    }
                    else {
                        $('#hdnAddOrRemoveCart').val('add');
                    }
                    var contextMenu = $('#contextMenu');
                    $('#cntxt').empty();

                    contextMenu.css({
                        top: e.pageY,
                        left: e.pageX
                    }).show();
                    $('#hdnHoldFavDocId').val(e.id);
                    if ($('#hdnAddOrRemoveCart').val() == 'remove') {
                        var act = 'remove';
                        $('#cntxt').append("<li><a href='#' id='removeCart'  >Remove from Favourite</a></li>");
                        $("#removeCart").attr("onclick", "AddRemoveMyFav('remove', '" + docMenuId + "')");

                    }
                    else {
                        var act = 'add';
                        $('#cntxt').append("<li><a href='#' id='addCart'  >Add to Favourite</a></li>");
                        $("#addCart").attr("onclick", "AddRemoveMyFav('add', '" + docMenuId + "')");
                    }
                }
            }
        });
    });

    // Hide context menu on click outside
    $(document).on('click', function (e) {
        var contextMenu = $('#contextMenu');
        if (!$(e.target).closest('#contextMenu').length && !$(e.target).is('#contextMenu')) {
            contextMenu.hide();
        }
    });
});
function checkMyFavExist(docId) {
    $.ajax({
        type: 'POST',
        url: '/DashboardHome/CheckMyFavMenu',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        data: JSON.stringify({ docId: docId }),
        success: function (Objdata) {
            alert(Objdata);
            return Objdata;
        }
    });
}
function GetAllMenus() {
    //debugger;
    $('#add_menu').empty();
    var HTMLString = "";
    var ObjSearchMenu = {};
    ObjSearchMenu.search_menu = $("#search_menu").val();
    $.ajax({
        type: 'POST',
        url: '/Dashboard/Home/GetAllMenus',/*Controller=Exercise and Fuction=GetTrainerExerciseDetails*/
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        data: JSON.stringify(ObjSearchMenu),/*Registration pass value like model*/
        success: function (Objdata) {
            //debugger;
            if (Objdata !== null && Objdata !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(Objdata);/*this is use for json data braking in array type and put in a Array*/
                //alert(Objdata);
                // console.log(arr);
                var lenth = arr.Table.length;/*This is check the Data length*/

                var PresentNode = [];
                var ChildNode = [];
                var SubChildNode = [];
                var SubSubChildNode = [];

                PresentNode = arr.Table;
                ChildNode = arr.Table1;
                SubChildNode = arr.Table2;
                SubSubChildNode = arr.Table3;

                if (PresentNode.length > 0) {
                    for (x = 0; x < PresentNode.length; x++) {
                        var PreNode = [];
                        PreNode = ChildNode.filter(function (CN) { return CN.Doc_parent_ID === PresentNode[x].Doc_ID; });
                        if (PreNode.length > 0) {
                            HTMLString += '<li><a>' + PresentNode[x].Lang + '<span class="fa fa-chevron-down"></span></a>';
                            HTMLString += '<ul class="nav child_menu">';
                            for (y = 0; y < PreNode.length; y++) {
                                var ChihNode = [];
                                ChihNode = SubChildNode.filter(function (CN) { return CN.Doc_parent_ID === PreNode[y].Doc_ID; });
                                if (ChihNode.length !== 0) {
                                    if (ChihNode.length > 0) {
                                        HTMLString += '<li><a>' + PreNode[y].Lang + '<span class="fa fa-chevron-down"></span></a>';
                                        HTMLString += '<ul class="nav child_menu">';
                                        for (za = 0; za < ChihNode.length; za++) {
                                            var SubSubChihNode = [];
                                            SubSubChihNode = SubSubChildNode.filter(function (CN) { return CN.Doc_parent_ID === ChihNode[za].Doc_ID; });
                                            if (SubSubChihNode.length !== 0) {
                                                if (SubSubChihNode.length > 0) {
                                                    HTMLString += '<li><a>' + ChihNode[za].Lang + '<span class="fa fa-chevron-down"></span></a>';
                                                    HTMLString += '<ul class="nav child_menu" >';
                                                    for (ay = 0; ay < SubSubChihNode.length; ay++) {
                                                        HTMLString += '<li onclick="GetMenuPage(this);"><a>' + SubSubChihNode[ay].Lang + '</a><span hidden="hidden">' + SubSubChihNode[ay].Doc_Path + '</span><span hidden="hidden">' + SubSubChihNode[ay].Doc_ID + '</span>';
                                                        HTMLString += '</li>';
                                                    }
                                                }
                                                HTMLString += '</ul>';
                                                HTMLString += '</li>';
                                            }
                                            else {
                                                HTMLString += '<li onclick="GetMenuPage(this);"><a>' + ChihNode[za].Lang + '</a><span hidden="hidden">' + ChihNode[za].Doc_Path + '</span><span hidden="hidden">' + ChihNode[za].Doc_ID + '</span>';
                                                HTMLString += '</li>';
                                            }
                                        }
                                        HTMLString += '</ul>';
                                        HTMLString += '</li>';
                                    }
                                }
                                else {
                                    HTMLString += '<li onclick="GetMenuPage(this);"><a>' + PreNode[y].Lang + '</a><span hidden="hidden">' + PreNode[y].Doc_Path + '</span><span hidden="hidden">' + PreNode[y].Doc_ID + '</span>';
                                    HTMLString += '</li>';
                                }
                            }
                            HTMLString += '</ul>';
                            HTMLString += '</li>';
                        }
                        else {
                            HTMLString += '<li onclick="GetMenuPage(this);"><a>' + PresentNode[x].Lang + '</a><span hidden="hidden">' + PresentNode[x].Doc_Path + '</span><span hidden="hidden">' + PresentNode[x].Doc_ID + '</span>';
                            HTMLString += '</li>';
                        }
                    }
                    $('#add_menu').append(HTMLString);
                }

            }

        }
    });
}
function GetMenuPage(event) {
    debugger;
    sessionStorage.clear();
    var ListItemName = event.innerText;
    var MenuPagePath = event.childNodes[1].innerText;
    var MenuPageID = event.childNodes[2].innerText;
    sessionStorage.setItem("MenuName", ListItemName);
    sessionStorage.setItem("MenuID", MenuPageID);

    if (MenuPageID == "105101107") {
        var Branch = $("#TopNavBranchList").val();
        if (Branch == '0') {
            swal("", $("#valueReq").text(), "warning");
            //$("#spanBrch").text($("#valueReq").text());
            //$("#spanBrch").css("display", "block");
        }
        else {
            sessionStorage.setItem("BranchID", Branch);
            $("#spanBrch").css("display", "none");
            ViewMenuPage(MenuPagePath);
        }
    }
    else {
        $("#spanBrch").css("display", "none");
        ViewMenuPage(MenuPagePath);
    }
}
function ViewMenuPage(PageName) {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                //url: "/BusinessLayer/ItemGroupSetup/Index",
                url: PageName,
                data: {},
                success: function (data) {
                    debugger;
                    $("#rightPageContent").empty().html(data);
                    //DefaultItemGroupSetup();
                },
            });
    } catch (err) {
        //console.log(PFName + " Error : " + err.message);
    }
}
function GetTopNavBrchList() {
    debugger;
    $.ajax({
        type: "POST",
        url: "/Dashboard/Home/GetAllBranchList",/*Controller=ItemSetup and Fuction=Getwarehouse*/
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        data: '',/*Registration pass value like model*/
        success: function (data) {
            debugger;
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            /*dynamically dropdown list of all Assessment */
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
                var s = '<option value="0">---Select---</option>';
                for (var i = 0; i < arr.Table.length; i++) {
                    s += '<option value="' + arr.Table[i].Comp_Id + '">' + arr.Table[i].comp_nm + '</option>';
                }
                $("#TopNavBranchList").html(s);
                var Branch = sessionStorage.getItem("BranchID");
                if (Branch !== "0" && Branch !== "" && Branch !== null) {
                    $('#TopNavBranchList').val(Branch).prop('selected', true);
                    $('#TopNavBranchList').attr('disabled', true);
                    OnChangeBranch();

                }
            }
        },
        error: function (Data) {
        }
    });
}
function ErrorPage() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/ItemSetup/ErrorPage",
                data: {},
                success: function (data) {
                    debugger;
                    $("#rightPageContent").empty().html(data);
                },
            });
    } catch (err) {
        console.log("ErrorPage Error : " + err.message);
    }
}
function OnChangeBranch() {
    debugger;
    try {
        var Branch = $('#TopNavBranchList').val();
        var BranchName = $('#TopNavBranchList option:selected').text();
        $("#hdBranchValue").val(Branch);
        $.ajax({
            type: "POST",
            url: "/Dashboard/DashboardHome/OnBranchChange",
            data: {
                Branch: Branch,
                BranchName: BranchName,
            },
            success: function (data) {
                window.location.reload();
            }
        });
        //GetAllPendingDocumentDetails();
        //Get_ChartsDataList(5, "Customers", "Top Customers");
        //Get_TickersDataList("", "");
        //Get_AlertsDataList();
    } catch (err) {
        debugger;
        console.log("ItemSetup Error : " + err.message);
    }
}
function CheckBranchSelection(event) {
    debugger;
    var Branch = $('#TopNavBranchList').val();
    var MenuPageName = event.innerText;
    var MenuDocumentId = $(event).find("input").val();
    var MenuNamehref = event.href;

    $("#hdBranchValue").val(Branch);
    // sessionStorage.removeItem("MenuDocuId");
    // sessionStorage.removeItem("Menuhref");

    $.ajax({
        type: "POST",
        url: "/Dashboard/DashboardHome/SetSessionMenuDocumentId",

        data: {
            MenuDocumentId: MenuDocumentId,
            MenuPageName: MenuPageName

        },
    });

    sessionStorage.setItem("MenuName", MenuPageName);
    var testtext = sessionStorage.getItem("MenuName");
    if (Branch == "0" && MenuDocumentId.startsWith(105)) {

        //if ((MenuDocumentId != null || MenuDocumentId != "") && (MenuNamehref != null || MenuNamehref != "")) {
        var MenuID = sessionStorage.getItem("MenuDocuId");
        var MenuHref = sessionStorage.getItem("Menuhref");

        if (MenuID != null && MenuID != "" && MenuHref != null && MenuHref != "") {
            var FMID = "#" + MenuID;
            $("ul").find(FMID)[0].href = MenuHref;

            sessionStorage.setItem("MenuDocuId", MenuDocumentId)
            sessionStorage.setItem("Menuhref", MenuNamehref)
            swal("", $("#SelectBranch").text(), "warning");
            return false;
        }
        else {
            sessionStorage.setItem("MenuDocuId", MenuDocumentId)
            sessionStorage.setItem("Menuhref", MenuNamehref)
            event.href = "#";
            swal("", $("#SelectBranch").text(), "warning");
            return false;
        }
    }
    else {
        var MenuID = sessionStorage.getItem("MenuDocuId");
        var MenuHref = sessionStorage.getItem("Menuhref");

        if (MenuID != null && MenuID != "" && MenuHref != null && MenuHref != "") {

            var FMID = "#" + MenuID;
            $("ul").find(FMID)[0].href = MenuHref;

            sessionStorage.removeItem("MenuDocuId");
            sessionStorage.removeItem("Menuhref");
            return true;
        }

    }
    //}

    //if (sessionStorage.getItem("MenuName") != null) {
    //    $("MenuName").attr("href", sessionStorage.getItem("MenuNamehref"))
    //    sessionStorage.clear();
    //    event.href = "#";
    //    sessionStorage.setItem("MenuName", MenuPageName);
    //    sessionStorage.setItem("MenuNamehref", event.href)
    //    swal("", $("#SelectBranch").text(), "warning");
    //} else {
    //    event.href = "#";
    //    sessionStorage.setItem("MenuName", MenuPageName);
    //    sessionStorage.setItem("MenuNamehref", event.href)
    //    swal("", $("#SelectBranch").text(), "warning");
    //}

    //return false;



}
function avldata() {  // add this function Nitesh 21-10-2023 1432 for Available_toggle update in database by Dashbord
    debugger;
    var value = "";
    var toggle = $("#Available_toggle").val();
    if ($("#Available_toggle").is(":checked")) {
        value = "Y";
    }
    else {
        value = "N";
    }
    try {
        $.ajax({
            type: "POST",
            url: "/Dashboard/DashboardHome/togglesave",
            data: {
                value: value,
            },
            success: function (data) {
                window.location.reload();
            }
        });
    } catch (err) {
        debugger;
        console.log("ItemSetup Error : " + err.message);
    }
}
/************************************************
Created By:Sanjay
Created Date: 11-01-2024
Description: This Javascript is used to add/remove My favourite menu
*************************************************/
//$(document).ready(function () {
//    debugger;
//    $('.addcart').on('contextmenu', function (e) {
//        e.preventDefault();
//        var docId = (this.id);
//        var action = "add";
//        //check if already exist on my fav 
//        var isConfirmed = true;
//        /*$('#crtcnfmodel').show();*/
//        // Check the user's response
//        if (isConfirmed) {
//         $.ajax({
//            type: 'POST',
//            url: '/DashboardHome/CheckMyFavMenu',
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            async: false,
//            data: JSON.stringify({ docId: docId }),
//            success: function (Objdata) {
//                var msg = "";
//                var act = "";
//                var color = "btn-danger";
//                if (Objdata == "exist") {
//                    msg = "Remove from my favourite";
//                    act = "REMOVE";
//                    color = "btn-danger";
//                }
//                else {
//                    msg = "Add to my favourite";
//                    act = "Add";
//                    color = "btn-primary";
//                }
//                swal({
//                    title: $("#deltital").text() + "?",
//                    text: "",
//                    type: "warning",
//                    showCancelButton: true,
//                    confirmButtonClass: color,
//                    confirmButtonText: msg,
//                    closeOnConfirm: true
//                }, function (isConfirm) {
//                    if (isConfirm) {
//                        AddRemoveMyFav(isConfirmed, act, docId);
//                        return true;
//                    } else {
//                        return false;
//                    }
//                });
//               // var isConfirmed = confirm(msg);

//            }
//        });
//            /*alert('Successfully added to my favourite.' + docId);*/
//        }
//        else {
//            alert('Action cancelled successfully!' + docId);
//        }
//        // show remove from my fav if already exist


//        //show add to my fav if not exist
//    });
//});
function AddRemoveMyFav(action, docId) {
    debugger;
    var subItmCount = 0;
    $.ajax({
        type: 'POST',
        url: '/DashboardHome/SaveUpdateFavouriteMenuDetails',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        data: JSON.stringify({ act: action, docId: docId }),
        success: function (Objdata) {
            $('#ul_100').empty();
            $.ajax({
                type: 'POST',
                url: '/DashboardHome/GetMyFavLiItems',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: null,
                success: function (jsonData) {
                    debugger;
                    var arr = JSON.parse(jsonData);
                    $('#100').empty();
                    for (var i = 0; i < arr.length; i++) {
                        subItmCount = subItmCount + 1;
                        /*var listItemHTML = jsonData[i].listItem;*/
                        $('#ul_100').append(arr[i].listItem)
                        $('#100').html('My Favorite<input type="hidden" value="100"><span class="fa fa-chevron-down"></span>')
                    }
                    //(Add/Remove My Fav)
                    $('.addcart').on('contextmenu', function (e) {
                        var docMenuId = this.id;
                        var closestHiddenField = $(this).closest('li').find('.hidden-field').val();
                        debugger;
                        $.ajax({
                            type: 'POST',
                            url: '/DashboardHome/CheckMyFavMenu',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            data: JSON.stringify({ docId: this.id }),
                            success: function (Objdata) {
                                if (Objdata != 'exist' || closestHiddenField == "100") {
                                    e.preventDefault();
                                    debugger;
                                    // var closestHiddenField = $(this).closest('li').find('.hidden-field').val();
                                    /*alert(closestHiddenField);*/
                                    if (closestHiddenField == "100") {
                                        $('#hdnAddOrRemoveCart').val('remove');
                                    }
                                    else {
                                        $('#hdnAddOrRemoveCart').val('add');
                                    }
                                    var contextMenu = $('#contextMenu');
                                    $('#cntxt').empty();

                                    contextMenu.css({
                                        top: e.pageY,
                                        left: e.pageX
                                    }).show();
                                    $('#hdnHoldFavDocId').val(e.id);
                                    if ($('#hdnAddOrRemoveCart').val() == 'remove') {
                                        var act = 'remove';
                                        $('#cntxt').append("<li><a href='#' id='removeCart'  >Remove from Favourite</a></li>");
                                        $("#removeCart").attr("onclick", "AddRemoveMyFav('remove', '" + docMenuId + "')");

                                    }
                                    else {
                                        var act = 'add';
                                        $('#cntxt').append("<li><a href='#' id='addCart'  >Add to Favourite</a></li>");
                                        $("#addCart").attr("onclick", "AddRemoveMyFav('add', '" + docMenuId + "')");
                                    }
                                }
                            }
                        });
                    });

                }
            });
        },
        error: function (xhr, status, error) {
            console.error("Error : ", status, error);
        }
    });
    if (subItmCount < 1) {
        $('#100').html('My Favorite<input type="hidden" value="100">')
        $('.mainli').removeClass('active');
    }
    /*alert('Successfully added to my favourite.' + docId);*/
    $('#contextMenu').hide();
}
/*My Favourite END*/

