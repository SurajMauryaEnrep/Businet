$(document).ready(function () {
    debugger;
    $("#ListHSN_Id").select2();
    $("#OCList #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $('#ListFilterData').val();
            var clickedrow = $(e.target).closest("tr");
            var OCCode = clickedrow.children("#ocid").text();
            var act_status = clickedrow.children("#act_status").text();
            var TaxApplicable = clickedrow.children("#TaxApplicable").text();
            if (OCCode != "" && OCCode != null) {
                window.location.href = "/BusinessLayer/OCList/EditOC/?OCId=" + OCCode + "&act_status=" + act_status + "&TaxApplicable=" + TaxApplicable + "&ListFilterData=" + ListFilterData;
            }
        }
        catch (err) {
            debugger
            alert(err.message);
        }
    });
    var PageName = sessionStorage.getItem("MenuName");
    $('#OCListPageName').text(PageName);

    $("#OCListName").select2()
      //  {
    //    ajax: {
    //        url: $("#OCListName").val(),
    //        data: function (params) {
    //            var queryParameters = {
    //                //restrictedCountry: $("#resCountry").val(),  // pass your own parameter                
    //                ddlGroup: params.term, // search term like "a" then "an"
    //                Group: params.page
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        cache: true,
    //        delay: 250,
    //        //type: 'POST',
    //        contentType: "application/json; charset=utf-8",
    //        processResults: function (data, params) {
    //            debugger;
    //            params.page = params.page || 1;
    //            return {
    //                results: $.map(data, function (val, item) {
    //                    return { id: val.ID, text: val.Name };
    //                })
    //            };
    //        }
    //    },
    //});
});
function OnClick() {
    document.getElementById("OCListName").innerHTML = "";
    $("#OCListName").css("border-color", "#ced4da");
}
    function OCDetail() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/OtherChargeSetup/OCDetail",
                data: {},
                success: function (data) {
                    debugger;
                    $("#rightPageContent").empty().html(data);
                },
            });
    } catch (err) {
        console.log("ItemSetup Error : " + err.message);
    }
}
function EditOCDetail(OCID) {
    debugger;
    sessionStorage.removeItem("EditOCCode");
    sessionStorage.setItem("EditOCCode", OCID);
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/OtherChargeSetup/OCDetail",
                //data: {},
                data: {},
                success: function (data) {
                    if (data == 'ErrorPage') {
                        ErrorPage();
                        return false;
                    }
                    debugger;
                    $("#rightPageContent").empty().html(data);
                },
            });
    } catch (err) {
        console.log("ItemSetup Error : " + err.message);
    }
}
function ErrorPage() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/GLAccountSetup/ErrorPage",
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
function FilterOCList() {
    debugger;
    var OCID = $("#OCListName").val();
    var ActStatus = $("#act_Status").val();
    var OCtype = $("#OtherChargeType").val();
    var Hsn_ID = $("#ListHSN_Id").val();
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/OCList/OCListFilter",/*Controller=ItemSetup and Fuction=GetItemList*/
        data: {
            OCID: OCID,
            ActStatus: ActStatus,
            OCtype: OCtype,
            Hsn_ID: Hsn_ID,
        },
        success: function (data) {
            debugger;
            $('#OCList').html(data);
            $('#ListFilterData').val(OCID + ',' + ActStatus + ',' + OCtype + ',' + Hsn_ID);
        },


    });
}