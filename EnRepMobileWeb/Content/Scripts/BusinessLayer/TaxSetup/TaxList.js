$(document).ready(function () {
    debugger;
    $("#TaxList #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            debugger;
            var ListFilterData = $("#ListFilterData").val();
            var clickedrow = $(e.target).closest("tr");
            var TaxCode = clickedrow.children("#taxid").text();
            var ActSts = clickedrow.children("#act_status").text();
            var manual_calc = clickedrow.children("#manual_calc").text();
            var recov = clickedrow.children("#recov").text();
            window.location.href = "/BusinessLayer/TaxList/EditTax/?TaxId=" + TaxCode + "&ActSts=" + ActSts + "&manual_calc=" + manual_calc + "&recov=" + recov + "&ListFilterData=" + ListFilterData;
        }
        catch (err) {
            debugger
            alert(err.message);
        }
    });
    var PageName = sessionStorage.getItem("MenuName");
    $('#TaxListPageName').text(PageName);

       $("#TaxListDDL").select2({
        ajax: {
            url: $("#TaxListName").val(),
            data: function (params) {
                var queryParameters = {
                    //restrictedCountry: $("#resCountry").val(),  // pass your own parameter                
                    ddlGroup: params.term, // search term like "a" then "an"
                    Group: params.page
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            //type: 'POST',
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
                params.page = params.page || 1;
                return {
                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }
        },
       });

  
    
    //$("#datatable-buttons tbody tr").on("click", function (event) {
    //    $("#datatable-buttons tbody tr").css("background-color", "#ffffff");
    //    $(this).css("background-color", "rgba(38, 185, 154, .16)");
    //});
   
});


function TaxDetail() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/TaxSetup/TaxDetail",
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
function EditTaxDetail(TaxCode) {
    debugger;
    sessionStorage.removeItem("EditTaxCode");
    sessionStorage.setItem("EditTaxCode", TaxCode);
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/TaxSetup/TaxDetail",
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
function FilterTaxList() {
    debugger;
    var TaxID = $("#TaxListDDL").val();
    var ActStatus = $("#tax_act_stat").val();
    var Taxtype = $("#tax_type").val();
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/TaxList/TaxListFilter",/*Controller=ItemSetup and Fuction=GetItemList*/
        data: {
            TaxID: TaxID,
            ActStatus: ActStatus,
            Taxtype: Taxtype,
        },
        success: function (data) {
            debugger;
            $('#TaxList').html(data);
            $('#ListFilterData').val(TaxID + ',' + ActStatus + ',' + Taxtype);
        },
    });
}