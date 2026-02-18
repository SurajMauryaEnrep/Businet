/*
 modidified by shubham maurya on 01-09-2022 17:05 on billing cheakbox all condication and attachment.
 */
/************************************************
Javascript Name:SupplierSetup
Created By:Mukesh
Created Date: 12-12-2020
Description: This Javascript use for the Supplier setup many function

Modified By: Suraj Maurya
Modified Date: 15-09-2021
Description: supplier setup modification 

*************************************************/

$(document).ready(function () {
    debugger;
    ValidationForPanNoSetAttr();
    BingGLReporting();
    Cmn_ByDefaultBranchData("#CustBrDetail", "#hdCustomerBranchId", "#cust_act_stat_", "Cust", "#HdnBranchActiveSatus");
    OnClickTdsApplicable();
    OnClickInterBranch();
    $("#GLAccGrp").select2();
    /*start add code by  Hina on 10-01-2023  for pageload*/
    $("#Supp_State").select2();
    $("#HdnDistrict").val("0");
    $("#HdnCity").val("0");
    /*End add code by  Hina on 10-01-2023  for pageload*/
    var PageName = sessionStorage.getItem("MenuName");
    $('#SupplierDetailPageName').text(PageName);
    /*Commented By Hina on 09-01-2023 to create this functionality on Pageload under Supervision of Premsir*/
    //EditSuppAddrDetail();
    ReadBranchList();
    ReadSuppAddress();
    /*Commented By Hina on 10-01-2023 to chnge on country code*/
    //getsuppCity();
    //---------------------------- Row edit Button funtionality ------------------//
    /*Remove tr in this table("#tblSupplierAddressDetail >tbody>tr") by Hina on 09-01-2024 to hit only body not row*/
    $("#tblSupplierAddressDetail >tbody").on('click', "#editBtnIcon", function (e) {

        debugger;
        /*Add code by Hina on 09-01-2024 */
        var suppcurr;
        if ($("#Import").is(":checked")) {
            suppcurr = "I";
        }
        if ($("#Local").is(":checked")) {
            suppcurr = "D";
        }

        var currentRow = $(this).closest('tr');

        //var currentRow = $(e.target).closest("tr")
        var row_index = currentRow.index();

        debugger;
        var address = currentRow.find("#suppadd").text()
        var cust_pin = currentRow.find("#supppin").text()
        var GSTNo = currentRow.find("#supgstno").text()
        var Statecode = GSTNo.substring(0, 2);
        var PanNum = GSTNo.substring(2, 12);
        var leftcode = GSTNo.substring(12, 15);
        /*Start Code by Hina on 11-01-2023*/
        var Country = currentRow.find("#hdnsuppcntry").text()
        var State = currentRow.find("#hdnsuppstate").text()
        var District = currentRow.find("#hdnsuppdist").text()
        /*End Code by Hina on 11-01-2023*/
        var City = currentRow.find("#hdnsupcity").text()
        var address_id = currentRow.find("#address_id").text()
        debugger;
        var ShiddenRowNo = currentRow.find("#spanBillrowId").text();
        if (currentRow.find("#defbilladd div input").is(":checked")) {
            $("#chkCustomerBillingAddress").prop("checked", true);
        }
        else {
            $("#chkCustomerBillingAddress").prop("checked", false);
        }
        if (currentRow.find("#address_id div input").is(":checked")) {
            $("#chkCustomerShippingAddress").prop("checked", true);
        }
        else {
            $("#chkCustomerShippingAddress").prop("checked", false);
        }
        var col10_value = row_index
        debugger;
        $("#supplier_address").val(address);
        $("#Supp_Pin").val(cust_pin);
        $("#gst_num").val(Statecode);
        $("#GSTMidPrt").val(PanNum);
        $("#GSTMidPrt").attr("disabled",true);
        $("#GSTLastPrt").val(leftcode);
        /*Code by Hina on 11-01-2024*/
        $("#HdnCountry").val(Country);
        $("#HdnState").val(State);
        $("#HdnDistrict").val(District);
        $("#HdnCity").val(City);
        if (suppcurr == "D") {
            $("#Supp_Country").val(Country);
            $("#Supp_State").val(State).trigger("change");
        }
        else {
            $("#Supp_Country").val(Country).trigger("change");
            //$("#Supp_State").val(State).trigger("change");
        }
        //$("#Supp_District").val(District);


        //getsuppCity();

        //$("#Supp_City").val(City);


        $("#hdnSRowId").val(ShiddenRowNo);
        $("#UpdtAddrId").val(address_id);
        $("#divUpdate").css("display", "block");
        $('#divAddAddrDtl').css("display", "none");

        //onchangeAddress();
        onchangePin();
        onchangeGST();
    });
    $('#AddressBody').on('click', '.deleteIcon', function () {
        debugger;

        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            debugger;
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);

        });
        debugger;
        var addr_id = $(this).closest('tr')[0].cells[16].innerHTML;
        var Flag = $(this).closest('tr')[0].cells[17].innerHTML;
        debugger;
        var SuppStatus = $("#hfSuppStatus").val();
        var custId = $("#Supp_Id").val();
        var IdonEdit = $("#hdnSRowId").val();
        var Supplier_address = $("#supplier_address").val();
        var Supplier_Pin = $("#Supp_Pin").val();
        var SupplierCity_Name = $("#Supp_City").val();
        currentRow = $(this).closest('tr');
        rowId = currentRow.find("#spanBillrowId").text();
        var addressSup = currentRow.find("#suppadd").text()
        var supp_pin = currentRow.find("#supppin").text()
        var CitySup = currentRow.find("#hdnsupcity").text()
        if ((custId == null || custId == "") && (SuppStatus == null || SuppStatus == "" || SuppStatus == "Drafted")) {
            $(this).closest('tr').remove();
            if (IdonEdit == rowId) {
                ClearSuppAddressDetail();
            }
            document.getElementById("vmSuppCityname").innerHTML = "";
            $("[aria-labelledby='select2-Supp_City-container']").css('border-color', '#ced4da');
            $("#vmSuppCityname").attr("style", "display: none;");
            document.getElementById("vmSuppPin").innerHTML = "";
            $("#Supp_Pin").css('border-color', '#ced4da');
            $("#vmSuppPin").attr("style", "display: none;");
            document.getElementById("SpanSuppAddr").innerHTML = "";
            $("#supplier_address").css('border-color', '#ced4da');
            $("#SpanSuppAddr").attr("style", "display: none;");
            /* For ResetBillingAddresToggleAfterDelete in Address Table */
            var firstactive = "N";
            var rowIdx1 = 1;
            var len = $("#tblSupplierAddressDetail >tbody >tr").length;
            $("#tblSupplierAddressDetail >tbody >tr").each(function (i, row) {
                var currentRow = $(this).closest('tr');
                if (firstactive == "N") {
                    if (len > 0) {
                        if (currentRow.find("#defbilladd div input").is(":checked")) {
                            firstactive = "Y";

                        }
                    }
                }
            });
            $("#tblSupplierAddressDetail >tbody >tr").each(function (i, row) {
                var currentRow = $(this);
                if (firstactive == "N") {
                    currentRow.find("#defbilladd div input").prop("checked", true);
                    $("#spanhiddenBill_" + rowIdx1).text("Y");
                    firstactive = "Y";
                }
            });
        }

        else {
            var currentrow = $(this).closest('tr');

            document.getElementById("vmSuppCityname").innerHTML = "";
            $("[aria-labelledby='select2-Supp_City-container']").css('border-color', '#ced4da');
            $("#vmSuppCityname").attr("style", "display: none;");
            document.getElementById("vmSuppPin").innerHTML = "";
            $("#Supp_Pin").css('border-color', '#ced4da');
            $("#vmSuppPin").attr("style", "display: none;");
            document.getElementById("SpanSuppAddr").innerHTML = "";
            $("#supplier_address").css('border-color', '#ced4da');
            $("#SpanSuppAddr").attr("style", "display: none;");

            ChakeDependencyAddr(custId, addr_id, currentrow);
        }
        ReadSuppAddress();


    });

    if ($("#act_status").is(":checked")) {
        $("#reasonrequired").attr("style", "display: none;");
    }
    else {
        $("#reasonrequired").css("display", "inherit");
    }
    if ($("#supp_hold").is(":checked")) {

        $("#holdrequired").css("display", "inherit");
    }
    else {
        $("#holdrequired").attr("style", "display: none;");
    }
    var Disable = $("#Disable").val();
    debugger;
    if (Disable != "Disable") {
        var suppcurr;
        if ($("#Import").is(":checked")) {
            suppcurr = "I";
            $("#Supp_Country").select2();
        }
        if ($("#Local").is(":checked")) {
            suppcurr = "D";
        }
        if (suppcurr != "D") {
            //$("#RequiredGSTCategory").attr("style", "display: none;");
            $("#GSTNumrequired").attr("style", "display: none;");
            $("#DefaultCurrency").css("display", "none");
            $("#DefaultCurrency1").removeClass("col-md-6");
        }
        else {
            //$("#Gst_Cat").prop('disabled', false);
            //$("#RequiredGSTCategory").css("display", "inherit");
            $("#GSTNumrequired").css("display", "inherit");
            $("#DefaultCurrency").css("display", "block");
            $("#DefaultCurrency1").addClass("col-md-6");
        }
    }
    else {
        var suppcurr;
        if ($("#Import").is(":checked")) {
            suppcurr = "I";
        }
        if ($("#Local").is(":checked")) {
            suppcurr = "D";
        }
        if (suppcurr != "D") {
            //$("#RequiredGSTCategory").attr("style", "display: none;");
            $("#GSTNumrequired").attr("style", "display: none;");
            $("#DefaultCurrency").css("display", "none");
            $("#DefaultCurrency1").removeClass("col-md-6");
        }
        else {
            //$("#Gst_Cat").prop('disabled', false);
            //$("#RequiredGSTCategory").css("display", "inherit");
            $("#GSTNumrequired").css("display", "inherit");
            $("#DefaultCurrency").css("display", "block");
            $("#DefaultCurrency1").addClass("col-md-6");
        }
    }
    debugger;
    var supp_curr;
    if ($("#Import").is(":checked")) {
        supp_curr = "I";
    }
    if ($("#Local").is(":checked")) {
        supp_curr = "D";
    }
    var gst_cat = $("#Gst_Cat").val();
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (gst_cat == "UR" || supp_curr == "E") {
        $("#GSTNumrequired").attr("style", "display: none;");

        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#SpanSuppGSTMidPrt").attr("style", "display: none;");
        document.getElementById("SpanSuppGSTMidPrt").innerHTML = "";

        $("#GSTLastPrt").css('border-color', "#ced4da");
        $("#SpanSuppGST").attr("style", "display: none;");
        document.getElementById("SpanSuppGST").innerHTML = "";

        $("#GSTMidPrt").val("");
        $("#GSTLastPrt").val("");
        $("#GSTMidPrt").attr("disabled", true);
        $("#GSTLastPrt").attr("disabled", true);

    }
    else if (gst_cat == "EX" || gst_cat == "CO" || gst_cat == "SZ" || GstApplicable == "N") {
        $("#GSTNumrequired").attr("style", "display: none;");

        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#SpanSuppGSTMidPrt").attr("style", "display: none;");
        document.getElementById("SpanSuppGSTMidPrt").innerHTML = "";

        $("#GSTLastPrt").css('border-color', "#ced4da");
        $("#SpanSuppGST").attr("style", "display: none;");
        document.getElementById("SpanSuppGST").innerHTML = "";
        $("#GSTMidPrt").attr("disabled", false);
        $("#GSTLastPrt").attr("disabled", false);
    }
    else {
        $("#GSTNumrequired").css("display", "inherit");
        $("#GSTMidPrt").attr("disabled", true);
        $("#GSTLastPrt").attr("disabled", false);
    }
    var create_id = $("#create_id").val();
    var DisablePageLavel = $("#hdnDisable").val();
    if (create_id != "" && DisablePageLavel != "Y") {
        var gst_cat = $("#Gst_Cat").val();
        var GstApplicable = $("#Hdn_GstApplicable").text();
        if (gst_cat == "RR" && GstApplicable == "Y") {

            var panNo = $("#supp_pan_no").val();
            if (panNo.length == 10) {
                $("#GSTMidPrt").val(panNo);
            }
            else {
                $("#GSTMidPrt").val("");
            }
                
        }
        
    }

});
//function upper(ustr) {
//    var str = ustr.value;
//    ustr.value = str.toUpperCase();
//}

function ValidationForPanNoSetAttr() {
    var gst_cat = $("#Gst_Cat").val();
    if (gst_cat == "UR") {
        var placeholder = $("#span_PANNumber").text();
        $("#supp_pan_no").attr("maxlength", "50");
        $("#supp_pan_no").attr("placeholder", `${$("#span_PANNumber").text()}`);
        $("#PanNorequired").css("display", "none");
    }
    else {
        if ($("#Local").is(":checked")) {
            suppcurr = "D";
            var CompCountry = $("#hdnCompCountry").val().trim().toLowerCase();
            var CompCountryID = $("#CompCountryID").val();
            if (CompCountry == "india") {
                $("#supp_pan_no").attr("maxlength", "10");
                $("#supp_pan_no").attr("placeholder", "ABCDE1234F");
                $("#PanNorequired").css("display", "");
            }
        }
        else {
            var placeholder = $("#span_PANNumber").text();
            $("#supp_pan_no").attr("maxlength", "50");
            $("#supp_pan_no").attr("placeholder", `${$("#span_PANNumber").text()}`);
            $("#PanNorequired").css("display", "none");
        }
    }
   
}
function onchangePanNumber() {
    debugger;
    var gst_cat = $("#Gst_Cat").val();
    if (gst_cat == "UR" || gst_cat == "EX" ||  gst_cat == "CO") {
        $("#GSTMidPrt").val("");
    }
    else {
        var valid = ValidationPanNumber();
        if (valid == true) {
            $("#vmsupp_pan_no").text("");
            $("#vmsupp_pan_no").css("display", "none");
            $("#supp_pan_no").attr("style", "border-color: #ced4da;");
            var GstApplicable = $("#Hdn_GstApplicable").text();
            if (GstApplicable == "Y") {
                const panInput = $("#supp_pan_no").val().toUpperCase();
                $("#supp_pan_no").val(panInput); // force uppercase in input field
                $("#GSTMidPrt").val(panInput);

                $("#GSTMidPrt").css('border-color', "#ced4da");
                $("#SpanSuppGSTMidPrt").attr("style", "display: none;");
                document.getElementById("SpanSuppGSTMidPrt").innerHTML = "";

                $("#GSTLastPrt").css('border-color', "#ced4da");
                $("#SpanSuppGST").attr("style", "display: none;");
                document.getElementById("SpanSuppGST").innerHTML = "";

                if ($("#tblSupplierAddressDetail > tbody > tr").length > 0) {
                    $("#tblSupplierAddressDetail > tbody > tr").each(function () {
                        const $row = $(this);
                        const gstCell = $row.find("#supgstno");
                        const GSTNo = gstCell.text();

                        if (GSTNo.length >= 15) {
                            const Statecode = GSTNo.substring(0, 2);
                            const leftcode = GSTNo.substring(12, 15);
                            const FullGstNo = Statecode + panInput + leftcode;
                            gstCell.text(FullGstNo);
                        }
                    });
                }
                $("#GSTMidPrt").attr("disabled", true);
            }
            else {
                $("#GSTMidPrt").attr("disabled", false);
            }
           
           

          
            return true;
        }
        else {
            $("#GSTMidPrt").attr("disabled", true);
            return false;
        }
    }
  
}
function ValidationPanNumber() {
    var ValidationFlag = true;
    if ($("#Local").is(":checked")) {
        var gst_cat = $("#Gst_Cat").val();
        if (gst_cat == "UR") {
            var placeholder = $("#span_PANNumber").text();
            $("#supp_pan_no").attr("maxlength", "50");
            $("#supp_pan_no").attr("placeholder", `${$("#span_PANNumber").text()}`);
            $("#PanNorequired").css("display", "none");
            $("#vmsupp_pan_no").text("");
            $("#vmsupp_pan_no").css("display", "none");
            $("#supp_pan_no").attr("style", "border-color: #ced4da;");
        }
        else {
            var CompCountry = $("#hdnCompCountry").val().trim().toLowerCase();
            var CompCountryID = $("#CompCountryID").val();
            if (CompCountry == "india") {
                const panInput = $("#supp_pan_no").val().toUpperCase(); // ensure uppercase
                if (panInput == "" || panInput == null) {
                    $("#vmsupp_pan_no").text($("#valueReq").text());
                    $("#vmsupp_pan_no").css("display", "block");
                    $("#supp_pan_no").css("border-color", "red");
                    ValidationFlag = false;
                }
                else {
                    if (isValidPAN(panInput)) {
                        $("#vmsupp_pan_no").text("");
                        $("#vmsupp_pan_no").css("display", "none");
                        $("#supp_pan_no").attr("style", "border-color: #ced4da;");
                    }
                    else {
                        $("#vmsupp_pan_no").text($("#span_InvalidPANNumber").text() + ' (' + 'ABCDE1234F' + ')');
                        $("#vmsupp_pan_no").css("display", "block");
                        $("#supp_pan_no").css("border-color", "red");
                        ValidationFlag = false;
                    }
                }

            }
            else {
                $("#vmsupp_pan_no").text("");
                $("#vmsupp_pan_no").css("display", "none");
                $("#supp_pan_no").attr("style", "border-color: #ced4da;");
            }
        }

    }
    else {
        $("#vmsupp_pan_no").text("");
        $("#vmsupp_pan_no").css("display", "none");
        $("#supp_pan_no").attr("style", "border-color: #ced4da;");
    }
    if (ValidationFlag == false) {
        return false;
    }
    else {
        return true;
    }
}
function isValidPAN(pan) {
    const panRegex = /^[A-Z]{5}[0-9]{4}[A-Z]{1}$/;
    return panRegex.test(pan);
}
function BingGLReporting() {
    debugger;
    $("#ID_GlRepoting_Group").select2({

        ajax: {
            type: "POST",
            url: "/BusinessLayer/SupplierDetail/GetGlReportingGrp",
            dataType: "json",

            data: function (params) {
                var queryParameters = {
                    GlRepoting_Group: params.term, // search term like "a" then "an"
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
                data = JSON.parse(data);
                params.page = params.page || 1;
                /*var paginatedData = data.slice((page - 1)  pageSize, page  pageSize);*/
                // if (page == 1) {
                if (data[0] != null) {
                    if (data[0].Val.trim() != "---Select---") {
                        var select = { ID: "0", Val: " ---Select---" };
                        data.unshift(select);
                    }
                }
                // }
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID, text: val.Val };
                    }),
                };
            },

        },

    });
}
function OnChangeGlrptfrp() {
    debugger;
    var glrtpID = $("#ID_GlRepoting_Group").val();
    var glrtpName = $("#ID_GlRepoting_Group option:selected").text();
    $("#hdn_GlRepoting_Group").val(glrtpID);
    $("#hdn_GlRepoting_GroupName").val(glrtpName);
}
function OnChangeGSTCat() {
    debugger;
    var gst_cat = $("#Gst_Cat").val();
    if (gst_cat == "UR") {
        $("#GSTNumrequired").attr("style", "display: none;");

        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#SpanSuppGSTMidPrt").attr("style", "display: none;");
        document.getElementById("SpanSuppGSTMidPrt").innerHTML = "";

        $("#GSTLastPrt").css('border-color', "#ced4da");
        $("#SpanSuppGST").attr("style", "display: none;");
        document.getElementById("SpanSuppGST").innerHTML = "";

        $("#GSTMidPrt").val("");
        $("#GSTLastPrt").val("");
        $("#GSTMidPrt").attr("disabled", true);
        $("#GSTLastPrt").attr("disabled", true);
        AddressAtbleNullGstNo();
    }
    else if (gst_cat == "EX" || gst_cat == "CO") {
        $("#GSTNumrequired").attr("style", "display: none;");

        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#SpanSuppGSTMidPrt").attr("style", "display: none;");
        document.getElementById("SpanSuppGSTMidPrt").innerHTML = "";

        $("#GSTLastPrt").css('border-color', "#ced4da");
        $("#SpanSuppGST").attr("style", "display: none;");
        document.getElementById("SpanSuppGST").innerHTML = "";
        $("#GSTMidPrt").attr("disabled", false);
        $("#GSTLastPrt").attr("disabled", false);
    }
    else {
        $("#GSTNumrequired").css("display", "inherit");
        $("#GSTMidPrt").attr("disabled", false);
        $("#GSTLastPrt").attr("disabled", false);
        ValidationForPanNoSetAttr();
      //  ValidationPanNumber();
        var valid = ValidationPanNumber();
        if (valid == true) {
            $("#vmsupp_pan_no").text("");
            $("#vmsupp_pan_no").css("display", "none");
            $("#supp_pan_no").attr("style", "border-color: #ced4da;");
            var GstApplicable = $("#Hdn_GstApplicable").text();
            if (GstApplicable == "Y") {
                if ($("#supp_pan_no").val() != "") {
                    const panInput = $("#supp_pan_no").val().toUpperCase();
                    $("#supp_pan_no").val(panInput); // force uppercase in input field
                    $("#GSTMidPrt").val(panInput);

                    if ($("#tblSupplierAddressDetail > tbody > tr").length > 0) {
                        $("#tblSupplierAddressDetail > tbody > tr").each(function () {
                            const $row = $(this);
                            const gstCell = $row.find("#supgstno");
                            const GSTNo = gstCell.text();

                            if (GSTNo.length >= 15) {
                                const Statecode = GSTNo.substring(0, 2);
                                const leftcode = GSTNo.substring(12, 15);
                                const FullGstNo = Statecode + panInput + leftcode;
                                gstCell.text(FullGstNo);
                            }
                        });
                    }
                    $("#GSTMidPrt").attr("disabled", true);
                }
               
            }
            else {
                $("#GSTMidPrt").attr("disabled", false);
            }




            return true;
        }
        else {
            $("#GSTMidPrt").attr("disabled", true);
            return false;
        }
    }
   
}
function AddressAtbleNullGstNo() {
    $("#tblSupplierAddressDetail tbody tr").each(function () {
        var curr = $(this).closest('tr');
        curr.find("#supgstno").text("");

    })
}
function OnChangeGLAccountName() {
    document.getElementById("vmGLAccountName").innerHTML = "";
    $("#GLAccountName").css("border-color", "#ced4da");
}
function getsuppCity() {
    $("#supplier_city").select2({
        ajax: {
            url: $("#SuppCity_Name").val(),
            data: function (params) {
                var queryParameters = {
                    //restrictedCountry: $("#resCountry").val(),  // pass your own parameter
                    ddlGroup: params.term, // search term like "a" then "an"
                    Group: params.page
                    //ddlCity: $("#supplier_city").val()
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
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID, text: val.Name };
                    }),
                };
            },

        },

    });

}

function OnClickTdsApplicable() {
    if ($("#TDSApplicable").is(":checked")) {
        $("#Div_TdsApplicableOn").css("display", "");
        //$("#Ddl_TdsApplicableOn").attr("disabled", false);
    } else {
        $("#Div_TdsApplicableOn").css("display", "none");
        //$("#Ddl_TdsApplicableOn").attr("disabled", true);
        //$("#Ddl_TdsApplicableOn").val("B");
    }
}
function OnClickInterBranch() {
    if ($("#InterBranch").is(":checked")) {
        $("#hdn_InterBranch").val("Y");
    }
    else {
        $("#hdn_InterBranch").val("N");
      
    }
}
function ReadBranchList() {
    debugger;
    var Branches = new Array();
    $("#CustBrDetail TBODY TR").each(function () {
        debugger;
        var row = $(this);
        var Branch = {};
        Branch.Id = row.find("#hdCustomerBranchId").val();
        var checkBoxId = "#cust_act_stat_" + Branch.Id;
        if (row.find(checkBoxId).is(":checked")) {
            Branch.BranchFlag = "Y";
        }
        else {
            Branch.BranchFlag = "N";
        }
        Branches.push(Branch);
    });
    debugger;
    var str = JSON.stringify(Branches);
    $('#hdCustomerBranchList').val(str);
}

function ReadSuppAddress() {
    debugger;
    var AddressList = new Array();
    var rowNum = 0;
    $("#tblSupplierAddressDetail TBODY TR").each(function () {
        debugger;
        var row = $(this);
        var Branch = {};
        rowNum = rowNum + 1
        //Branch.address = row.find("td:eq(2)").text();
        //Branch.City = row.find("td:eq(12)").text();
        //Branch.District = row.find("td:eq(6)").text();
        //Branch.State = row.find("td:eq(8)").text();
        //Branch.Country = row.find("td:eq(10)").text();
        //Branch.GSTNo = row.find("td:eq(11)").text();
        //Branch.BillingAddress = row.find("td:eq(14)").text();
        //Branch.pin = row.find("td:eq(3)").text();
        //Branch.address_id = row.find("td:eq(16)").text();
        //Branch.Flag = row.find("td:eq(17)").text();
        Branch.address = row.find("#suppadd").text();
        Branch.City = row.find("#hdnsupcity").text();
        Branch.CityName = row.find("#cityname").text();
        Branch.District = row.find("#hdnsuppdist").text();
        Branch.DistrictName = row.find("#distname").text();
        Branch.State = row.find("#hdnsuppstate").text();
        Branch.StateName = row.find("#statename").text();
        Branch.Country = row.find("#hdnsuppcntry").text();
        Branch.CountryName = row.find("#contryname").text();
        Branch.GSTNo = row.find("#supgstno").text();
        Branch.BillingAddress = row.find("#spanhiddenBill_" + rowNum).text();
        Branch.pin = row.find("#supppin").text();
        Branch.address_id = row.find("#address_id").text();
        Branch.Flag = row.find("#hdn_id").text();
        AddressList.push(Branch);
    });

    var str = JSON.stringify(AddressList);
    $('#hdSupplierAddressList').val(str);
    /*Commented By Hina on 09-01-2023 to create this functionality on Pageload under Supervision of Premsir*/
    //EditSuppAddrDetail();
}
function numValueOnly(el, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;

    if (charCode > 31 && (charCode < 48 || charCode > 58)) {
        return false;
    }
    return true;

}

function ChakeDependencyAddr(custId, e, currentrow) {
    var addr_id = e;
    debugger;
    $.ajax({
        type: "POST",
        url: "/SupplierDetail/ChakeDependencyAddr",
        dataType: "json",
        data: {
            addr_id: addr_id,
            custId: custId,
        },/*Registration pass value like model*/
        success: function (data) {
            debugger;
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            var DataT = data;
            if (DataT == "Detail Exists") {
                //Exist = "Yes";
                swal("", $("#DependencyExist").text(), "warning");
            }
            else {
                var rowIdx = currentrow.find("#spanBillrowId").text();
                currentrow.find("#defbilladd div input").prop("checked", false);
                currentrow.hide();
                var IdonEdit = $("#hdnSRowId").val();
                var Supplier_address = $("#supplier_address").val();
                var Supplier_Pin = $("#Supp_Pin").val();
                var SupplierCity_Name = $("#Supp_City").val();
                var addressSup = currentrow.find("#suppadd").text()
                var supp_pin = currentrow.find("#supppin").text()
                var CitySup = currentrow.find("#hdnsupcity").text()

                if (IdonEdit == rowIdx) {
                    //currentrow.hide();
                    ClearSuppAddressDetail();
                }

                FlagAfterDelete(addr_id);
                Exist = "No";
            }

        },
        error: function (Data) {
        }
    });

}
function FlagAfterDelete(e) {
    debugger;
    var firstactive = "N";
    var rowIdx = 1;
    $("#tblSupplierAddressDetail >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        //var a = currentRow.find("td:eq(16)").text();
        var a = currentRow.find("#address_id").text();
        if (e == a) {
            //currentRow.find("td:eq(17)").text("Delete");
            currentRow.find("#hdn_id").text("Delete");


        }
        if (e == a) {
            debugger;
            //  var firstactive = "N";
            var rowIdx1 = 1;
            var len = $("#tblSupplierAddressDetail >tbody >tr").length;
            $("#tblSupplierAddressDetail >tbody >tr").each(function (i, row) {
                var currentRow = $(this).closest('tr');
                if (firstactive == "N") {
                    if (len > 0) {
                        var billid = $(this).find("#spanBillrowId").text();
                        if (currentRow.find("#defbilladd div input").is(":checked")) {
                            firstactive = "Y";

                        }
                    }
                }
            });
            var rowId = 0;
            var rowIdx1 = 0;
            $("#tblSupplierAddressDetail TBODY TR").each(function () {
                debugger;
                rowIdx1 = rowIdx1 + 1;
                var currentRow = $(this);
                if (firstactive == "N") {
                    var hdn_id = currentRow.find("#hdn_id").text();
                    if (hdn_id != "Delete") {
                        currentRow.find("#defbilladd div input").prop("checked", true);
                        $("#spanhiddenBill_" + rowIdx1).text("Y");
                        firstactive = "Y";
                        ReadSuppAddress();
                        return false;
                    }

                }
            });
        }


    });
};
function SaveBtnClick() {
    var btn = $("#hdnSavebtn").val(); /**Adddd this NItesh 01-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#btn_save").prop("disabled", true); /*End*/
    }
    if (validateSupplierSetupInsertform() == false) {
        $("#hdnSavebtn").val("");
        return false;
    }
    //if (CheckSuppGstNumber() == false) {
    //    return false;
    //}
    if (OnChangeReason() == false) {
        $("#hdnSavebtn").val("");
        return false;
    }
    if (cheakbillingaddress() == false) {
        $("#hdnSavebtn").val("");
        return false;
    }
    if (Gstvalidaction() == false) {
        $("#hdnSavebtn").val("");
        return false;
    }
    else {

        debugger;
        $("#act_status").attr("disabled", false);
        $("#suppcurr").attr("disabled", false);
        if ($("#Import").is(":checked")) {
            $("#Import").attr("disabled", false);
        }
        if ($("#Local").is(":checked")) {
            $("#Local").attr("disabled", false);
        }
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");
        if ($("#Import").is(":checked")) {
            SuppType = "I";
            $("#HdnSuppType").val(SuppType);
            //$("#HdnState").val(0);
        }
        /*if ($("#Domestic").is(":checked")) {*/
        if ($("#Local").is(":checked")) {
            SuppType = "D";
            $("#HdnSuppType").val(SuppType);
        }
        /***Add By Shubham Maurya on 18-11-2025 Start***/
        var FinalLicenceDetail = [];
        FinalLicenceDetail = InsertFinalLicenceDetail();
        $("#hdnLincenceDetail").val(JSON.stringify(FinalLicenceDetail));
        /***Add By Shubham Maurya on 18-11-2025 End***/
        return true;
    }
}


function Gstvalidaction() {
    debugger;
    debugger;
    var supptype;
    var gstNo = "";
    var GstCat = $('#Gst_Cat').val();
    var GstApplicable = $('#hdnGstApplicable').val();
    if ($("#Import").is(":checked")) {
        supptype = "I";
    }
    if ($("#Local").is(":checked")) {
        supptype = "D";
    }
    if (supptype != "I") {
        if (GstApplicable != "N") {
            if (GstCat != "UR" && GstCat != "CO" && GstCat != "EX") {
                /*  $("#tblCustomerAddressDetail >tbody >tr").each(function (i, row)*/
                $("#tblSupplierAddressDetail TBODY TR").each(function () {
                    debugger;
                    var currentrow = $(this).closest('tr');
                    debugger;
                    gstNo = currentrow.find("#supgstno").text();
                    hdnID = currentrow.find("#hdn_id").text();
                    if (hdnID != "Delete") {
                        if (gstNo == "") {
                            swal("", $("#GSTNumberismissinginAddress").text(), "warning");
                            return false;
                        }
                    }
                    if (hdnID == "Delete") {
                        gstNo = "Delete";
                    }
                });
                if (gstNo == "") {
                    swal("", $("#GSTNumberismissinginAddress").text(), "warning");
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                return true;
            }
        }
        else {
            return true;
        }
    }
    else {
        return true;
    }
}
function validateSupplierSetupInsertform() {
    debugger;
    var Suppname = $("#supp_name").val();
    var reason = $("#inact_reason").val();
    var holdreason = $("#hold_reason").val();
    var active = '';
    var hold = '';
    if ($("#act_status").is(":checked")) {
        active = "Y";
    }
    else {
        active = "N";
    }
    if ($("#supp_hold").is(":checked")) {
        hold = "Y";
    }
    else {
        hold = "N";
    }
    var ValidationFlag = true;

    if (Suppname == '') {
        document.getElementById("vmSuppname").innerHTML = $("#valueReq").text();
        $("#supp_name").css("border-color", "red");
        ValidationFlag = false;
    }
    if ($("#GLAccountName").val() == '') {
        document.getElementById("vmGLAccountName").innerHTML = $("#valueReq").text();
        $("#GLAccountName").css("border-color", "red");
        ValidationFlag = false;
    }
    if ($("#supp_catg").val() == "0") {
        document.getElementById("vmSuppCatg").innerHTML = $("#valueReq").text();
        $("#supp_catg").css("border-color", "red");
        ValidationFlag = false;
    }
    if ($("#supp_port").val() == "0") {
        document.getElementById("vmSuppPort").innerHTML = $("#valueReq").text();
        $("#supp_port").css("border-color", "red");
        ValidationFlag = false;
    }
    if ($("#Supp_City").val() == "") {
        document.getElementById("vmSuppCity").innerHTML = $("#valueReq").text();
        $("#Supp_City").css("border-color", "red");
        ValidationFlag = false;
    }
    if ($("#GLAccGrp").val() == '0') {
        $('#SpanAccGrp').text($("#valueReq").text());
        $("#SpanAccGrp").css("display", "block");
        $("[aria-labelledby='select2-GLAccGrp-container']").css("border-color", "red");
        ValidationFlag = false;
    }
    //if ($("#supp_coa").val() == "0") {
    //    document.getElementById("vmSuppCoa").innerHTML = $("#valueReq").text();
    //    $("#supp_coa").css("border-color", "red");
    //    ValidationFlag = false;
    //}
    if ($("#suppcurr").val() == '0') {
        document.getElementById("vmSuppCurr").innerHTML = $("#valueReq").text();
        $("#suppcurr").css("border-color", "red");
        ValidationFlag = false;
    }
    if (active === 'Y') {
        $("#inact_reason").attr("style", "border-color: #ced4da;");
        $("#vminactreason").attr("style", "display: none;");
        $("#reasonrequired").attr("style", "display: none;");
    }
    //if (active === 'N' && reason ==='') {
    //    //$("#inact_reason").attr("style", "border-color:#ff0000;", "required");
    //    //document.getElementById("vminactreason").innerHTML = $("#valueReq").text();
    //    //$("#vminactreason").css("display", "block");
    //    //$("#reasonrequired").css("display", "inherit");
    //    ValidationFlag = false;
    //}
    if (hold === 'N') {
        $("#hold_reason").attr("style", "border-color: #ced4da;");
        $("#vmonholdreason").attr("style", "display: none;");
        $("#holdrequired").attr("style", "display: none;");
    }
    if (hold === 'Y' && holdreason === '') {
        $("#hold_reason").attr("style", "border-color:#ff0000;", "required");
        document.getElementById("vmonholdreason").innerHTML = $("#valueReq").text();
        $("#vmonholdreason").css("display", "block");
        $("#holdrequired").css("display", "inherit");
        ValidationFlag = false;
    }
    var validPanNumber = ValidationPanNumber();
    if (validPanNumber == false) {
        ValidationFlag = false;
    }
    if (ValidationFlag == true) {
        $("#supp_coa").attr("disabled", false)
        ReadSuppAddress();

        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);
        /*----- Attatchment End--------*/
        return true;
    }
    else {
        return false;
    }
}
function CheckSuppGstNumber() {
    debugger;
    var validflag = true;
    //var ValidationFlag = true;
    if ($("#Local").is(":checked")) {
        $("#tblSupplierAddressDetail TBODY TR").each(function () {
            var currentrow = $(this).closest('tr');
            debugger;
            var suppgst = currentrow.find("#supgstno").text();
            //validflag = false;
            if (suppgst == "") {
                swal("", $("#GSTNumberismissinginAddress").text(), "warning");
                validflag = false;
                return false;
            }
        });
    }
    if (validflag == false) {
        return false;
    }
    else {
        return true;
    }
}
function OnChangeAccGrp() {
    debugger;
    var GrpName = $('#GLAccGrp').val().trim();
    if (GrpName != "") {
        $("#SpanAccGrp").css("display", "none");
        $("[aria-labelledby='select2-GLAccGrp-container']").css("border-color", "#ced4da");
    }
    else {
        $('#SpanAccGrp').text($("#valueReq").text());
        $("#SpanAccGrp").css("display", "block");
        $("[aria-labelledby='select2-GLAccGrp-container']").css("border-color", "red");
    }
}
function cheakbillingaddress() {
    var billing = 'N';
    $("#tblSupplierAddressDetail >tbody >tr").each(function (i, row) {

        var currentRow = $(this);
        var rowId = "";
        //rowIdx = rowIdx + 1;
        var rowIdQ = $(this).find("#spanBillrowId").text();

        if ($("#chkCBA_" + rowIdQ).prop("checked") == true) {
            billing = 'Y';
        }
    });
    if (billing == 'N') {
        swal("", $("#AtLeastOneBillingAddressShouldBeSetAsDefault").text(), "warning");
        return false;
    }
    else {
        return true;
    }
}
function OnChangeSuppName() {
    debugger;
    var SuppName = $('#supp_name').val().trim();
    if (SuppName != "") {
        document.getElementById("vmSuppname").innerHTML = "";
        $("#supp_name").css("border-color", "#ced4da");
        $("#GLAccountName").val(SuppName);
        document.getElementById("vmGLAccountName").innerHTML = "";
        $("#GLAccountName").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmSuppname").innerHTML = $("#valueReq").text();
        $("#supp_name").css("border-color", "red");
    }
}
function OnChangeCat() {
    debugger;
    var cat = $('#supp_catg').val().trim();
    if (cat != "0") {
        document.getElementById("vmSuppCatg").innerHTML = "";
        $("#supp_catg").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmSuppCatg").innerHTML = $("#valueReq").text();
        $("#supp_catg").css("border-color", "red");
    }
}
function OnChangePort() {
    debugger;
    var Port = $('#supp_port').val().trim();
    if (Port != "0") {
        document.getElementById("vmSuppPort").innerHTML = "";
        $("#supp_port").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmSuppPort").innerHTML = $("#valueReq").text();
        $("#supp_port").css("border-color", "red");
    }
}
function OnChangeCity_OLD() {
    debugger;
    var CustomerCity_Name = $("#supplier_city").val();

    //GetsuppDSCntr();
    if (CustomerCity_Name != "0") {
        document.getElementById("vmSuppCityname").innerHTML = "";
        $("[aria-labelledby='select2-supplier_city-container']").css('border-color', '#ced4da');
        $("#vmSuppCityname").attr("style", "display: none;");

        //$("#gst_num").css('border-color', "#ced4da");
        //$("#SpanSuppGST").attr("style", "display: none;");
        //document.getElementById("SpanSuppGST").innerHTML = "";
        //document.getElementById("vmSuppPin").innerHTML = "";
        //$("#Pin").css('border-color', '#ced4da');
        //$("#vmSuppPin").attr("style", "display: none;");
        //document.getElementById("SpanSuppAddr").innerHTML = "";
        //$("#supplier_address").css('border-color', '#ced4da');
        //$("#SpanSuppAddr").attr("style", "display: none;");
    }
    if (document.getElementById("SpanSuppAddr").innerHTML == $("#valueduplicate").text()) {
        document.getElementById("SpanSuppAddr").innerHTML = "";
        $("#supplier_address").css('border-color', '#ced4da');
        $("#SpanSuppAddr").attr("style", "display: none;");
        document.getElementById("vmSuppCityname").innerHTML = "";
        $("[aria-labelledby='select2-supplier_city-container']").css('border-color', '#ced4da');
        $("#vmSuppCityname").attr("style", "display: none;");
        document.getElementById("vmSuppPin").innerHTML = "";
        $("#Supp_Pin").css('border-color', '#ced4da');
        $("#vmSuppPin").attr("style", "display: none;");
    }
}
function OnChangeCoa() {
    debugger;
    var Port = $('#supp_coa').val().trim();
    if (Port != "0") {
        document.getElementById("vmSuppCoa").innerHTML = "";
        $("#supp_coa").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmSuppCoa").innerHTML = $("#valueReq").text();
        $("#supp_coa").css("border-color", "red");
    }
}
function OnChangeCurr() {
    debugger;
    var curr = $('#suppcurr').val().trim();
    if (curr != "0") {
        document.getElementById("vmSuppCurr").innerHTML = "";
        $("#suppcurr").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmSuppCurr").innerHTML = $("#valueReq").text();
        $("#suppcurr").css("border-color", "red");
    }
}
function OnChangeReason() {

    var flag = true;
    /*var inactreason=*/
    //document.getElementById("vminactreason").innerHTML = "";
    //$("#inact_reason").css("border-color", "#ced4da");
    if ($("#act_status").is(":checked")) {
        $("#reasonrequired").attr("style", "display: none;");
        document.getElementById("vminactreason").innerHTML = "";
        $("#inact_reason").css("border-color", "#ced4da");
    }
    else {
        if ($("#inact_reason").val() == "") {
            $("#reasonrequired").css("display", "inherit");
            document.getElementById("vminactreason").innerHTML = $("#valueReq").text();;
            $("#inact_reason").css("border-color", "red");
            flag = false;
        }
        else {
            //$("#reasonrequired").attr("style", "display: none;");
            document.getElementById("vminactreason").innerHTML = "";
            $("#inact_reason").css("border-color", "#ced4da");
        }
    }
    if (flag == true) {

    }
    else {
        return false;
    }
    //$("#reasonrequired").attr("style", "display: none;");  
}
function OnActiveStatusChange() {
    debugger;
    if ($("#act_status").is(":checked")) {
        $("#inact_reason").prop("readonly", true);
        document.getElementById("vminactreason").innerHTML = "";
        $("#inact_reason").css("border-color", "#ced4da");
        $("#reasonrequired").attr("style", "display: none;");
        $("#supp_hold").prop("disabled", false);
        $("#supp_hold").prop("checked", false);
        $("#inact_reason").val("");
        OnHoldStatusChange();
    }
    else {
        $("#inact_reason").prop("readonly", false);
        //$("#inact_reason").val("");
        $("#inact_reason").attr("style", "border-color:#ff0000;", "required");
        document.getElementById("vminactreason").innerHTML = $("#valueReq").text();
        $("#vminactreason").css("display", "block");
        $("#reasonrequired").css("display", "inherit");
        $("#supp_hold").prop("disabled", true);
        $("#supp_hold").prop("checked", false);
        OnHoldStatusChange();
    }
}
function OnChangeHoldReason() {
    debugger;
    document.getElementById("vmonholdreason").innerHTML = "";
    $("#hold_reason").css("border-color", "#ced4da");
    //$("#holdrequired").attr("style", "display: none;");
    if ($("#supp_hold").is(":checked")) {

        $("#holdrequired").css("display", "inherit");
    }
    else {
        $("#holdrequired").attr("style", "display: none;");
    }
}
function OnHoldStatusChange() {
    debugger;
    if ($("#supp_hold").is(":checked")) {
        $("#hold_reason").attr("style", "border-color:#ff0000;", "required");
        document.getElementById("vmonholdreason").innerHTML = $("#valueReq").text();
        $("#vmonholdreason").css("display", "block");
        $("#holdrequired").css("display", "inherit");
        $("#hold_reason").prop("readonly", false);

    }
    else {
        $("#hold_reason").prop("readonly", true);
        document.getElementById("vmonholdreason").innerHTML = "";
        $("#hold_reason").css("border-color", "#ced4da");
        $("#holdrequired").attr("style", "display: none;");
        $("#hold_reason").val("");
    }
}
function GetsuppDSCntr() {
    debugger;
    //var Supp_Type;
    //if ($("#Import").is(":checked")) {
    //    Supp_Type = "I";
    //}
    //if ($("#Local").is(":checked")) {
    //    Supp_Type = "D";
    //}
    var ddlCity = $("#supplier_city").val();

    $.ajax({
        type: "POST",
        url: "/CustomerDetails/GetsuppDSCntr",
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async: true,
        data: { SuppCity: ddlCity, },/*Registration pass value like model*/
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            /*dynamically dropdown list of all Assessment */
            debugger;
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
                if (arr.Table.length > 0) {
                    $("#supp_district").val(arr.Table[0].district_name);
                    $("#suppdistID").val(arr.Table[0].district_id);
                }
                if (arr.Table1.length > 0) {
                    $("#supp_state").val(arr.Table1[0].state_name);
                    $("#suppstateID").val(arr.Table1[0].state_id);
                }
                if (arr.Table2.length > 0) {
                    $("#supp_country").val(arr.Table2[0].country_name);
                    $("#suppcntryID").val(arr.Table2[0].country_id);
                }

                if (arr.Table3.length > 0) {
                    $("#gst_num").val(arr.Table3[0].state_code);
                    $("#suppstateID").val(arr.Table3[0].state_id);
                }

            }
        },
        error: function (Data) {
        }
    });
};
function DisableActStat() {
    debugger;
    $("#act_status").prop("disabled", true);
    $("#supp_hold").prop("disabled", true);
    $("#inact_reason").prop("readonly", true);
    $("#hold_reason").prop("readonly", true);
}
function EnableActStat() {
    debugger;
    $("#act_status").prop("disabled", false);
    $("#supp_hold").prop("disabled", false);
    $("#inact_reason").prop("readonly", false);
    $("#hold_reason").prop("readonly", false);
}

function GetCurronSuppType() {
    debugger;
    var suppcurr;
    if ($("#Import").is(":checked")) {
        suppcurr = "I";
        $('#suppcurr').empty();
    }
    if ($("#Local").is(":checked")) {
        suppcurr = "D";
        $('#suppcurr').empty();
        //$("#GSTNumrequired").css("display", "inherit");
        //GetsuppDSCntr();
    }
    $("#HdnSuppType").val(suppcurr);
    if (suppcurr != "D") {
        //$("#Gst_Cat").prop('disabled', true);
        //$("#RequiredGSTCategory").attr("style", "display: none;");
        /*code start Add by Hina on 29-01-2024*/

        document.getElementById("vmSuppCountryname").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-Supp_Country-container']").css('border-color', 'red');
        $("#vmSuppCountryname").attr("style", "display: block;");
        document.getElementById("vmSuppPin").innerHTML = "";
        $("#Supp_Pin").css('border-color', "#ced4da");
        $("#vmSuppPin").attr("style", "display: none;");
        document.getElementById("SpanSuppGSTMidPrt").innerHTML = "";
        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#GSTMidPrt").attr("disabled", false);
        $("#SpanSuppGSTMidPrt").attr("style", "display: none;");
        /*code start End by Hina on 29-01-2024*/
        $("#GSTNumrequired").attr("style", "display: none;");
        $("#DefaultCurrency").css("display", "none");
        $("#supppinvalid").text("");
        $("#DefaultCurrency1").removeClass("col-md-6");
    }
    else {
        //$("#Gst_Cat").prop('disabled', false);
        //$("#RequiredGSTCategory").css("display", "inherit");
        /*code start Add by Hina on 29-01-2024*/
        document.getElementById("SpanSuppAddr").innerHTML = "";
        $("#supplier_address").css('border-color', "#ced4da");
        $("#SpanSuppAddr").attr("style", "display: none;");
        document.getElementById("vmSuppCountryname").innerHTML = "";
        $("[aria-labelledby='select2-Supp_Country-container']").css('border-color', "#ced4da");
        $("#vmSuppCountryname").attr("style", "display: none;");
        document.getElementById("vmSuppPin").innerHTML = "";
        $("#Supp_Pin").css('border-color', "#ced4da");
        $("#vmSuppPin").attr("style", "display: none;");
        document.getElementById("SpanSuppGSTMidPrt").innerHTML = "";
        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#SpanSuppGSTMidPrt").attr("style", "display: none;");
        /*code End Add by Hina on 29-01-2024*/
        $("#GSTNumrequired").css("display", "inherit");
        $("#DefaultCurrency").css("display", "block");
        $("#DefaultCurrency1").addClass("col-md-6");
        $("#supppinvalid").text("*");

        var Gst_cat = $("#Gst_Cat").val();
        var GstApplicable = $("#Hdn_GstApplicable").val();
        if (Gst_cat == "RR" && GstApplicable=="Y") {
            $("#GSTMidPrt").attr("disabled", true);
        }
        else {
            $("#GSTMidPrt").attr("disabled", false);
        }
        
    }
    //$("#suppcurr").val("0").change();

    $.ajax({
        type: "POST",
        url: "/BusinessLayer/SupplierDetail/GetCurronSuppType",
        data: { Supptype: (suppcurr == "I" ? "A" : suppcurr) },//modified by Suraj maurya on 07-03-2025
        success: function (data) {
            debugger;
            var arr = [];
            arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
            if (arr.length > 0) {
                //$('#Proscurr').val(arr[0].curr_id);
                for (var i = 0; i < arr.length; i++) {
                    $('#suppcurr').append(`<option value="${arr[i].curr_id}">${arr[i].curr_name}</option>`);
                }
            }
            //$('#CurrencyDiv').html(data);
        },

    });
    /*Add Code by Hina on 08-01-2024 to get country on Custtype chng*/
    GetCountryOnChngSuppType(suppcurr);
    ValidationForPanNoSetAttr();
}

function ErrorPage() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/SupplierSetup/ErrorPage",
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

function functionConfirm(event) {
    swal({
        title: $("#deltital").text() + "?",
        text: $("#deltext").text() + "!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    }, function (isConfirm) {
        if (isConfirm) {
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}

function onchangeGST() {
    //var suptype;
    //if ($("#Import").is(":checked")) {
    //    suptype = "I";
    //}
    //if ($("#Local").is(":checked")) {
    //    suptype = "D";
    //}
    var SupplierGST = $("#gst_num").val();
    var GSTNoMidPrt = $("#GSTMidPrt").val().trim();
    var GSTNoLastPrt = $("#GSTLastPrt").val().trim();

    if (GSTNoMidPrt != "" || GSTNoMidPrt == "") {
        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#SpanSuppGST").attr("style", "display: none;");
        document.getElementById("SpanSuppGSTMidPrt").innerHTML = "";
    }
    if (GSTNoLastPrt != "" || GSTNoLastPrt == "") {
        document.getElementById("SpanSuppGST").innerHTML = "";
        $("#GSTLastPrt").css('border-color', "#ced4da");
        $("#SpanSuppGST").attr("style", "display: none;");
    }
    //if (suptype == "D") {
    //    if (GSTNoMidPrt == "") {
    //        document.getElementById("SpanSuppGST").innerHTML = $("#valueReq").text();
    //        //$("#gst_num").css('border-color', 'red');
    //        $("#GSTMidPrt").css('border-color', 'red');
    //        $("#SpanSuppGST").attr("style", "display: block;");
    //        $("#GSTNumrequired").css("display", "inherit");

    //    }
    //    else {
    //        document.getElementById("SpanSuppGST").innerHTML = "";
    //        //$("#gst_num").css('border-color', '#ced4da');
    //        $("#GSTMidPrt").css('border-color', "#ced4da");
    //        $("#SpanSuppGST").attr("style", "display: none;");
    //    }
    //    if (GSTNoLastPrt == "") {
    //        document.getElementById("SpanSuppGST").innerHTML = $("#valueReq").text();
    //        $("#GSTLastPrt").css('border-color', 'red');
    //        $("#SpanSuppGST").attr("style", "display: block;");
    //        $("#GSTNumrequired").css("display", "inherit");
    //    }
    //    else {
    //        document.getElementById("SpanSuppGST").innerHTML = "";
    //        //$("#gst_num").css('border-color', '#ced4da');
    //        $("#GSTLastPrt").css('border-color', "#ced4da");
    //        $("#SpanSuppGST").attr("style", "display: none;");
    //    }

    //}
    //else {

    //    document.getElementById("SpanSuppGST").innerHTML = "";
    //    //$("#gst_num").css('border-color', '#ced4da');
    //    $("#GSTMidPrt").css('border-color', "#ced4da");
    //    $("#GSTLastPrt").css('border-color', "#ced4da");
    //    $("#SpanSuppGST").attr("style", "display: none;");
    //    $("#GSTNumrequired").css("style", "display:none");
    //}
}

function onchangePin() {
    debugger;
    var custtype;
    if ($("#Import").is(":checked")) {
        custtype = "I";
    }
    if ($("#Local").is(":checked")) {
        custtype = "D";
    }
    if (custtype == "D") {
        var customer_Pin = $("#Supp_Pin").val();
        if (customer_Pin == "") {
            document.getElementById("vmSuppPin").innerHTML = $("#valueReq").text();
            $("#Supp_Pin").css('border-color', 'red');
            $("#vmSuppPin").attr("style", "display: block;");
        }
        else {

            document.getElementById("vmSuppPin").innerHTML = "";
            $("#Supp_Pin").css('border-color', '#ced4da');
            $("#vmSuppPin").attr("style", "display: none;");
        }
    }

    else {

        document.getElementById("vmSuppPin").innerHTML = "";
        $("#Supp_Pin").css('border-color', '#ced4da');
        $("#vmSuppPin").attr("style", "display: none;");

        //document.getElementById("SpanSuppAddr").innerHTML = "";
        //$("#supplier_address").css('border-color', '#ced4da');
        //$("#SpanSuppAddr").attr("style", "display: none;");
        //document.getElementById("vmSuppCityname").innerHTML = "";
        //$("[aria-labelledby='select2-supplier_city-container']").css('border-color', '#ced4da');
        //$("#vmSuppCityname").attr("style", "display: none;");
    }
    if (document.getElementById("SpanSuppAddr").innerHTML == $("#valueduplicate").text()) {
        document.getElementById("SpanSuppAddr").innerHTML = "";
        $("#supplier_address").css('border-color', '#ced4da');
        $("#SpanSuppAddr").attr("style", "display: none;");
        document.getElementById("vmSuppCityname").innerHTML = "";
        $("[aria-labelledby='select2-Supp_City-container']").css('border-color', '#ced4da');
        $("#vmSuppCityname").attr("style", "display: none;");

    }

}

function onchangeAddress() {
    var supplier_address = $("#supplier_address").val();
    if (supplier_address == "") {
        document.getElementById("SpanSuppAddr").innerHTML = $("#valueReq").text();
        $("#supplier_address").css('border-color', 'red');
        $("#SpanSuppAddr").attr("style", "display: block;");

    }
    else {

        document.getElementById("SpanSuppAddr").innerHTML = "";
        $("#supplier_address").css('border-color', '#ced4da');
        $("#SpanSuppAddr").attr("style", "display: none;");

        //document.getElementById("vmSuppPin").innerHTML = "";
        //$("#Pin").css('border-color', '#ced4da');
        //$("#vmSuppPin").attr("style", "display: none;");
        document.getElementById("vmSuppCityname").innerHTML = "";
        $("[aria-labelledby='select2-Supp_City-container']").css('border-color', '#ced4da');
        $("#vmSuppCityname").attr("style", "display: none;");
    }
    if (document.getElementById("vmSuppPin").innerHTML == $("#valueduplicate").text()) {
        document.getElementById("vmSuppPin").innerHTML = "";
        $("#Supp_Pin").css('border-color', '#ced4da');
        $("#vmSuppPin").attr("style", "display: none;");
        document.getElementById("vmSuppCityname").innerHTML = "";
        $("[aria-labelledby='select2-Supp_City-container']").css('border-color', '#ced4da');
        $("#vmSuppCityname").attr("style", "display: none;");
    }
}
function validationAddress() {
    debugger;
    var supplier_address = $("#supplier_address").val();
    /*start add code by  Hina on 09-01-2023  for ONchangeCountry Code*/
    var SuppContry_Name = $("#Supp_Country").val();
    var SuppState_Name = $("#Supp_State").val();
    var SuppDistrict_Name = $("#Supp_District").val();
    var SuppCity_Name = $("#Supp_City").val();
    var Supp_Pin = $("#Supp_Pin").val();
    /*End add code by  Hina on 09-01-2023  for ONchangeCountry Code*/

    var SupplierGST = $("#gst_num").val();
    var GSTNoMidPrt = $("#GSTMidPrt").val().trim();
    var GSTNoLastPrt = $("#GSTLastPrt").val().trim();
    var gst_cat = $("#Gst_Cat").val();
    var GstApplicable = $('#hdnGstApplicable').val();

    var custtype;
    if ($("#Import").is(":checked")) {
        custtype = "I";
    }
    if ($("#Local").is(":checked")) {
        custtype = "D";
    }

    var valFalg = true;
    if (supplier_address == "") {
        document.getElementById("SpanSuppAddr").innerHTML = $("#valueReq").text();
        $("#supplier_address").css('border-color', 'red');
        $("#SpanSuppAddr").attr("style", "display: block;");
        valFalg = false;
    }
    /*start add code by  Hina on 09-01-2023  for ONchangeCountry Code**/
    if (SuppContry_Name == "0") {
        document.getElementById("vmSuppCountryname").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-Supp_Country-container']").css('border-color', 'red');
        $("#vmSuppCountryname").attr("style", "display: block;");
        valFalg = false;
    }
    if (SuppState_Name == "0") {
        document.getElementById("vmSuppStatename").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-Supp_State-container']").css('border-color', 'red');
        $("#Supp_State").css('border-color', 'red');
        $("#vmSuppStatename").attr("style", "display: block;");
        valFalg = false;
    }
    if (SuppDistrict_Name == "0") {
        document.getElementById("vmSuppDistname").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-Supp_District-container']").css('border-color', 'red');
        $("#Supp_District").css('border-color', 'red');
        $("#vmSuppDistname").attr("style", "display: block;");
        valFalg = false;
    }
    if (SuppCity_Name == "0") {
        document.getElementById("vmSuppCityname").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-Supp_City-container']").css('border-color', 'red');
        $("#Supp_City").css('border-color', 'red');
        $("#vmSuppCityname").attr("style", "display: block;");
        valFalg = false;
    }
    /* custytyp add by hina on 29-01-2023 to Pin not mandetory for import*/
    if (custtype == "D") {
        if (Supp_Pin == "") {
            document.getElementById("vmSuppPin").innerHTML = $("#valueReq").text();
            $("#Supp_Pin").css('border-color', 'red');
            $("#vmSuppPin").attr("style", "display: block;");
            valFalg = false;
        }
        else {
            document.getElementById("vmSuppPin").innerHTML = "";
            $("#Supp_Pin").css('border-color', "#ced4da");
            $("#vmSuppPin").attr("style", "display: none;");
        }
    }
    else {
        document.getElementById("vmSuppPin").innerHTML = "";
        $("#Supp_Pin").css('border-color', "#ced4da");
        $("#vmSuppPin").attr("style", "display: none;");
    }
    /*End add code by  Hina on 09-01-2023 for ONchangeCountry Code*/
    debugger;
    if (custtype != "I") {
        if (gst_cat != "UR") {
            if (gst_cat == "RR") 
                {
                    $("#GSTMidPrt").attr("disabled", true);
                }
                else
                {
                    $("#GSTMidPrt").attr("disabled", false);
                }
           
            $("#GSTLastPrt").attr("disabled", false);
        }
        if (GstApplicable != "N") {
            if (gst_cat != "UR" && gst_cat != "CO" && gst_cat != "EX") {

                if (GSTNoMidPrt == "") {
                    $("#GSTMidPrt").css('border-color', 'red');
                    $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
                    //document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#valueReq").text();
                    document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#valueReq").text();
                    valFalg = false;
                }
                else if (GSTNoMidPrt != "" && GSTNoMidPrt.length != "10") {
                    $("#GSTMidPrt").css('border-color', 'red');
                    $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
                    document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#InvalidGSTNumber").text();
                    valFalg = false;
                }
            }
            if (GSTNoMidPrt != "" || GSTNoLastPrt != "") {
                if (GSTNoMidPrt == "") {
                    $("#GSTMidPrt").css('border-color', 'red');
                    $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
                    document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#valueReq").text();
                    valFalg = false;
                }
                else if (GSTNoMidPrt != "" && GSTNoMidPrt.length != "10") {
                    $("#GSTMidPrt").css('border-color', 'red');
                    $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
                    document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#InvalidGSTNumber").text();
                    valFalg = false;
                }
                else if (GSTNoLastPrt == "") {
                    $("#GSTLastPrt").css('border-color', 'red');
                    $("#SpanSuppGST").attr("style", "display: block;");
                    document.getElementById("SpanSuppGST").innerHTML = $("#valueReq").text();
                    valFalg = false;
                }
                else if (GSTNoLastPrt != "" && GSTNoLastPrt.length != "3") {
                    $("#GSTLastPrt").css('border-color', 'red');
                    $("#SpanSuppGST").attr("style", "display: block;");
                    document.getElementById("SpanSuppGST").innerHTML = $("#InvalidGSTNumber").text();
                    valFalg = false;
                }
            }
            else {
                if (GSTNoMidPrt != "" && GSTNoLastPrt == "") {
                    $("#GSTLastPrt").css('border-color', 'red');
                    $("#SpanSuppGST").attr("style", "display: block;");
                    document.getElementById("SpanSuppGST").innerHTML = $("#InvalidGSTNumber").text();
                    valFalg = false;
                }
                else {

                }
                if (GSTNoMidPrt == "" && GSTNoLastPrt != "") {
                    $("#GSTMidPrt").css('border-color', 'red');
                    $("#SpanSuppGST").attr("style", "display: block;");
                    document.getElementById("SpanSuppGST").innerHTML = $("#InvalidGSTNumber").text();
                    valFalg = false;
                }
                else {

                }
            }
        }
        else {
            if (GSTNoMidPrt == "" && GSTNoLastPrt != "") {
                $("#GSTMidPrt").css('border-color', 'red');
                $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
                document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#valueReq").text();
                valFalg = false;
            }
            else if (GSTNoMidPrt != "" && GSTNoMidPrt.length != "10") {
                $("#GSTMidPrt").css('border-color', 'red');
                $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
                document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#InvalidGSTNumber").text();
                valFalg = false;
            }
            else if (GSTNoMidPrt != "" && GSTNoLastPrt == "") {
                $("#GSTLastPrt").css('border-color', 'red');
                $("#SpanSuppGST").attr("style", "display: block;");
                document.getElementById("SpanSuppGST").innerHTML = $("#valueReq").text();
                valFalg = false;
            }
            else if (GSTNoLastPrt != "" && GSTNoLastPrt.length != "3") {
                $("#GSTLastPrt").css('border-color', 'red');
                $("#SpanSuppGST").attr("style", "display: block;");
                document.getElementById("SpanSuppGST").innerHTML = $("#InvalidGSTNumber").text();
                valFalg = false;
            }
            if (GSTNoMidPrt != "" || GSTNoLastPrt) {
                var SuppGSTNum = SupplierGST + GSTNoMidPrt + GSTNoLastPrt
                $("#GSTNumber").val(SuppGSTNum);
            }
            else {
                if (GSTNoMidPrt == "" || GSTNoLastPrt == "") {
                    var SuppGSTNum = GSTNoMidPrt + GSTNoLastPrt
                    $("#GSTNumber").val(SuppGSTNum);
                }
            }
        }
    }
    /* comment and add by hina on 29-01-2023 to GST not mandetory for import*/
    else {
        document.getElementById("SpanSuppGSTMidPrt").innerHTML = "";
        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#GSTLastPrt").css('border-color', "#ced4da");
        $("#SpanSuppGSTMidPrt").attr("style", "display: none;");
        $("#GSTMidPrt").attr("disabled", true);
        $("#GSTLastPrt").attr("disabled", true);
    }
    //else {
    //    if (GSTNoMidPrt == "" && GSTNoLastPrt != "") {
    //        $("#GSTMidPrt").css('border-color', 'red');
    //        $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
    //        document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#valueReq").text();
    //        valFalg = false;
    //    }
    //    else if (GSTNoMidPrt != "" && GSTNoMidPrt.length != "10") {
    //        $("#GSTMidPrt").css('border-color', 'red');
    //        $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
    //        document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#InvalidGSTNumber").text();
    //        valFalg = false;
    //    }
    //    else if (GSTNoMidPrt != "" && GSTNoLastPrt == "") {
    //        $("#GSTLastPrt").css('border-color', 'red');
    //        $("#SpanSuppGST").attr("style", "display: block;");
    //        document.getElementById("SpanSuppGST").innerHTML = $("#valueReq").text();
    //        valFalg = false;
    //    }
    //    else if (GSTNoLastPrt != "" && GSTNoLastPrt.length != "3") {
    //        $("#GSTLastPrt").css('border-color', 'red');
    //        $("#SpanSuppGST").attr("style", "display: block;");
    //        document.getElementById("SpanSuppGST").innerHTML = $("#InvalidGSTNumber").text();
    //        valFalg = false;
    //    }
    //    if (GSTNoMidPrt != "" || GSTNoLastPrt) {
    //        var SuppGSTNum = SupplierGST + GSTNoMidPrt + GSTNoLastPrt
    //        $("#GSTNumber").val(SuppGSTNum);
    //    }
    //    else {
    //        if (GSTNoMidPrt == "" || GSTNoLastPrt == "") {
    //            var SuppGSTNum = GSTNoMidPrt + GSTNoLastPrt
    //            $("#GSTNumber").val(SuppGSTNum);
    //        }
    //    }
    //}
    if (valFalg == true) {
       
    }
    else {
        return false;
    }
}

////this function is used within Validation Address and update address
//but now is not required of value req msg in any case
//function ValidateValReqWithInvalidGST() {
//    if (suptype == "D") {
//        debugger;
//        if (SupplierGST == "") {
//            document.getElementById("SpanSuppGST").innerHTML = $("#valueReq").text();
//            $("#gst_num").css('border-color', 'red');
//            $("#SpanSuppGST").attr("style", "display: block;");
//            $("#GSTNumrequired").css("display", "inherit");
//            valFalg = false;
//        }
//        else {
//            if (SupplierGST != "" && SupplierGST.length == "2") {
//                $("#gst_num").css('border-color', "#ced4da");
//                $("#SpanSuppGST").attr("style", "display: none;");
//                document.getElementById("SpanSuppGST").innerHTML = "";

//                if (GSTNoMidPrt == "") {
//                    document.getElementById("SpanSuppGST").innerHTML = $("#valueReq").text();
//                    $("#GSTMidPrt").css('border-color', 'red');
//                    $("#SpanSuppGST").attr("style", "display: block;");
//                    $("#GSTNumrequired").css("display", "inherit");
//                    valFalg = false;

//                }
//                else {
//                    if (GSTNoMidPrt != "" && GSTNoMidPrt.length == "10") {
//                        $("#GSTMidPrt").css('border-color', "#ced4da");
//                        $("#SpanSuppGST").attr("style", "display: none;");
//                        document.getElementById("SpanSuppGST").innerHTML = "";

//                        if (GSTNoLastPrt == "") {
//                            document.getElementById("SpanSuppGST").innerHTML = $("#valueReq").text();
//                            $("#GSTLastPrt").css('border-color', 'red');

//                            $("#SpanSuppGST").attr("style", "display: block;");
//                            $("#GSTNumrequired").css("display", "inherit");
//                            valFalg = false;
//                        }
//                        else {

//                            if (GSTNoLastPrt != "" && GSTNoLastPrt.length == "3") {
//                                $("#GSTLastPrt").css('border-color', "#ced4da");
//                                $("#SpanSuppGST").attr("style", "display: none;");
//                                document.getElementById("SpanSuppGST").innerHTML = "";
//                            }
//                            else {
//                                $("#GSTLastPrt").css('border-color', 'red');
//                                $("#SpanSuppGST").attr("style", "display: block;");
//                                document.getElementById("SpanSuppGST").innerHTML = $("#InvalidGSTNumber").text();
//                                valFalg = false;
//                            }
//                        }
//                    }
//                    else {

//                        $("#GSTMidPrt").css('border-color', 'red');
//                        $("#SpanSuppGST").attr("style", "display: block;");
//                        document.getElementById("SpanSuppGST").innerHTML = $("#InvalidGSTNumber").text();
//                        valFalg = false;
//                    }
//                }

//            }
//            else {
//                $("#gst_num").css('border-color', 'red');
//                $("#SpanSuppGST").attr("style", "display: block;");
//                document.getElementById("SpanSuppGST").innerHTML = $("#InvalidGSTNumber").text();
//                valFalg = false;
//            }
//        }
//    }
//    else {
//        document.getElementById("SpanSuppGST").innerHTML = "";
//        $("#GSTMidPrt").css('border-color', "#ced4da");
//        $("#GSTLastPrt").css('border-color', "#ced4da");
//        $("#SpanSuppGST").attr("style", "display: none;");
//        $("#GSTNumrequired").css("style", "display:none");
//    }
//}
function CheckDuplicacyOnAddSup() {
    debugger;
    var Supp_address = $("#supplier_address").val();
    var Supp_Pin = $("#Supp_Pin").val();
    var SupCity_Name = $("#Supp_City").val();

    var validflg = true;

    $("#tblSupplierAddressDetail TBODY TR").each(function () {
        var currentRow = $(this).closest('tr')
        debugger;

        var addressSup = currentRow.find("#suppadd").text()
        var supp_pin = currentRow.find("#supppin").text()
        var CitySup = currentRow.find("#hdnsupcity").text()
        var checkID = currentRow.find("#hdn_id").text();
        if (checkID != "Delete") {
            /* comment by hina on 29-01-2023 to Pin not mandetory for import*/
            /*if (Supp_address.toLowerCase() == addressSup.toLowerCase() && Supp_Pin == supp_pin && SupCity_Name == CitySup) {*/
            if (Supp_address.toLowerCase() == addressSup.toLowerCase() && SupCity_Name == CitySup) {

                document.getElementById("SpanSuppAddr").innerHTML = $("#valueduplicate").text();
                $("#supplier_address").css('border-color', 'red');e
                $("#SpanSuppAddr").attr("style", "display: block;");
                /* comment by hina on 29-01-2023 to Pin not mandetory for import*/
                //document.getElementById("vmSuppPin").innerHTML = $("#valueduplicate").text();
                //$("#Supp_Pin").css('border-color', 'red');
                //$("#vmSuppPin").attr("style", "display: block;");
                document.getElementById("vmSuppCityname").innerHTML = $("#valueduplicate").text();
                $("[aria-labelledby='select2-Supp_City-container']").css('border-color', 'red');
                $("#vmSuppCityname").attr("style", "display: block;");
                validflg = false;
            }
        }

    });
    if (validflg == true) {

    }
    else {
        return false;
    }

}
function CheckDuplicateOnUpdateSupp() {
    debugger;
    var Supplier_address = $("#supplier_address").val();
    var Supplier_Pin = $("#Supp_Pin").val();
    var SupplierCity_Name = $("#Supp_City").val();
    var SRowidUpdt = $("#hdnSRowId").val();

    validflg = true;
    $("#tblSupplierAddressDetail TBODY TR").each(function () {
        currentRow = $(this);
        rowId = $(this).find("#spanBillrowId").text();
        if (rowId > 0) {
            if (SRowidUpdt != rowId) {
                var addressSup = currentRow.find("#suppadd").text()
                var supp_pin = currentRow.find("#supppin").text()
                var CitySup = currentRow.find("#hdnsupcity").text()
                var checkID = currentRow.find("#hdn_id").text();
                if (checkID != "Delete") {
                    /* comment by hina on 29-01-2023 to Pin not mandetory for import*/
                    //if (Supplier_address.toLowerCase() == addressSup.toLowerCase() && Supplier_Pin == supp_pin && SupplierCity_Name == CitySup) {
                    if (Supplier_address.toLowerCase() == addressSup.toLowerCase() && SupplierCity_Name == CitySup) {

                        document.getElementById("SpanSuppAddr").innerHTML = $("#valueduplicate").text();
                        $("#supplier_address").css('border-color', 'red');
                        $("#SpanSuppAddr").attr("style", "display: block;");
                        /* comment by hina on 29-01-2023 to Pin not mandetory for import*/
                        //document.getElementById("vmSuppPin").innerHTML = $("#valueduplicate").text();
                        //$("#Supp_Pin").css('border-color', 'red');
                        //$("#vmSuppPin").attr("style", "display: block;");
                        document.getElementById("vmSuppCityname").innerHTML = $("#valueduplicate").text();
                        $("[aria-labelledby='select2-Supp_City-container']").css('border-color', 'red');
                        $("#vmSuppCityname").attr("style", "display: block;");

                        validflg = false;
                    }
                }
            }
        }
    });
    if (validflg == false) {
        return false;
    }
}
function AddAddressdetail() {
    debugger;
    var custtype;
    if ($("#Import").is(":checked")) {
        custtype = "I";
    }
    if ($("#Local").is(":checked")) {
        custtype = "D";
    }
    var rowIdx = 0;
    debugger;
    //var TblLen = $("#tblSupplierAddressDetail TBODY TR").length;
    var rowId = $("#tblSupplierAddressDetail TBODY TR:last").find("#spanBillrowId").text();
    if (rowId == "" || rowId == null) {
        rowIdx = 1;
    }
    else {
        rowIdx = parseInt(rowId) + 1;
    }
    var supplier_address = $("#supplier_address").val();
    var customer_Pin = $("#Supp_Pin").val();
    var CustomerCity_Name = $("#Supp_City").val();
    var SupplierGST = $("#gst_num").val();
    var SuppGSTNoMidPrt = $("#GSTMidPrt").val().trim();
    var SuppGSTNoLastPrt = $("#GSTLastPrt").val().trim();
    var GstApplicable = $('#hdnGstApplicable').val();
    //if (custtype != "I") {
    //    if (GstApplicable != "N") {
    if (SuppGSTNoMidPrt != "" || SuppGSTNoLastPrt) {
        var SuppGSTNum = SupplierGST + SuppGSTNoMidPrt + SuppGSTNoLastPrt
        $("#hdSupplierGst").val(SuppGSTNum);
    }
    else {
        if (SuppGSTNoMidPrt == "" || SuppGSTNoLastPrt == "") {
            var SuppGSTNum = SuppGSTNoMidPrt + SuppGSTNoLastPrt
            $("#hdSupplierGst").val(SuppGSTNum);
        }
    }
    //}
    //}

    var CustomerBillingAddress = "";
    var district = $("#Supp_District").val();
    var state_id = $("#Supp_State").val();
    var country_id = $("#Supp_Country").val();
    var gst_num = $("#gst_num").val();

    if (validationAddress() == false) {
        return false;
    }
    if ($("#chkCustomerBillingAddress").is(":checked")) {
        CustomerBillingAddress = "Y";
    }
    else {
        CustomerBillingAddress = "N";
    }
    var CustBillingAddresstd;
    if (CustomerBillingAddress == 'Y') {
        CustBillingAddresstd = '<td id="defbilladd"><div class="custom-control custom-switch" style = "margin-bottom: 0px; margin-top: -5px; margin-left: 40px;"><input type="checkbox" class="custom-control-input col-md-3 col-sm-12 margin-switch" id="chkCBA_' + rowIdx + '" checked onclick="defbillchk(event)"><label class="custom-control-label col-md-9 col-sm-12" for="chkCBA_' + rowIdx + '" style="padding: 3px 0px;"></label></div></td><td hidden="hidden"><span id="spanhiddenBill_' + rowIdx + '">Y</span></td><td class=" " hidden="hidden"><span id="spanBillrowId">' + rowIdx + '</span></td>'
    }
    else {
        CustBillingAddresstd = '<td id="defbilladd"><div class="custom-control custom-switch" style = "margin-bottom: 0px; margin-top: -5px; margin-left: 40px;"><input type="checkbox" class="custom-control-input col-md-3 col-sm-12 margin-switch" id="chkCBA_' + rowIdx + '" onclick="defbillchk(event)"><label class="custom-control-label col-md-9 col-sm-12" for="chkCBA_' + rowIdx + '" style="padding: 3px 0px;"></label></div></td><td hidden="hidden"><span id="spanhiddenBill_' + rowIdx + '">N</span></td><td class=" " hidden="hidden"><span id="spanBillrowId">' + rowIdx + '</span></td>'
    }

    var rowId = 0;
    var cheakflag = "N";
    var cheackdefalt = "Q";
    $("#tblSupplierAddressDetail TBODY TR").each(function () {
        var BIllrowId = $(this).find("#spanBillrowId").text();
        if ($(this).find("#chkCBA_" + BIllrowId).is(":checked")) {
            cheackdefalt = "P";
        }
    });
    if (cheackdefalt == "Q") {
        $("#tblSupplierAddressDetail TBODY TR").each(function () {

            debugger;
            if ($(this).find("#defbilladd div input").is(":checked"))

                var BIllrowId = $(this).find("#spanBillrowId").text();
            $(this).find("#chkCBA_" + BIllrowId).prop("checked", false);
            $(this).find("#spanhiddenBill_" + BIllrowId).text("Y");
            cheakflag = "Y";
        });
        if (cheakflag == "Y") {
            var rowId = 0;
            $("#tblSupplierAddressDetail TBODY TR").each(function () {
                debugger;
                rowId = rowId + 1;
                if (rowId == 1) {
                    var BIllrowId = $(this).find("#spanBillrowId").text();
                    $(this).find("#chkCBA_" + BIllrowId).prop("checked", true);
                    $(this).find("#spanhiddenBill_" + BIllrowId).text("Y");
                }
                return false
            });
        }
    }
    if (supplier_address != "" && CustomerCity_Name !== "0") {
        /*Commented by Hina on 29-01-2024 to pin is not mandetory in case of import*/
        /*if (supplier_address != "" && customer_Pin!="" && CustomerCity_Name !== "0") {*/
        debugger;
        if (CheckDuplicacyOnAddSup() == false) {
            return false;
        }
        if (CustomerBillingAddress == 'Y') {
            var rowId = 0;
            $("#tblSupplierAddressDetail TBODY TR").each(function () {
                debugger;
                rowId = rowId + 1;

                $(this).find("#defbilladd div input").prop("checked", false);
                $(this).find("#spanhiddenBill_" + rowId).text("N");
                //$(this).find("#chkCBA_" + rowId).prop("checked", false);
                //$(this).find("#spanhiddenBill_" + rowId).text("N");
            });
        }
        (async function () {
            const isGstValid = await CheckDuplicateGstNoAsync();
            if (!isGstValid) {
                return false;
            }
            else {

                $('#AddressBody').append(`<tr id="R${++rowIdx}"> 
        <td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
        <td class="center edit_icon"> <i class="fa fa-edit" aria-hidden="true" id="editBtnIcon" title="${$("#Edit").text()}"></i></td>
        <td id="suppadd">${$("#supplier_address").val()}</td>
        <td id="contryname">${$("#Supp_Country option:selected").text()}</td>
        <td hidden="hidden" id="hdnsuppcntry">${$("#Supp_Country").val()}</td>
        <td id="statename">${$("#Supp_State option:selected").text()}</td>
        <td hidden="hidden" id="hdnsuppstate">${$("#Supp_State").val()}</td>
        <td id="distname">${$("#Supp_District option:selected").text()}</td>
        <td hidden="hidden" id="hdnsuppdist">${$("#Supp_District").val()}</td>
        <td id="cityname">${$("#Supp_City option:selected").text()}</td>
        <td hidden="hidden" id="hdnsupcity">${$("#Supp_City").val()}</td>
        <td id="supppin">${$("#Supp_Pin").val()}</td>
        <td id="supgstno">${$("#hdSupplierGst").val()}</td>
       
         ${CustBillingAddresstd}
          <td hidden="hidden" id="address_id">${++rowIdx}</td>
          <td hidden="hidden" id="hdn_id">Save</td>
        
        </tr>`);
                /*start add code by  Hina on 09-01-2023  for ONchangeCountry Code*/

                $("#HdnState").val(0);
                $("#HdnDistrict").val(0);
                $("#HdnCity").val(0);
          



            debugger;
                var Supp_Country = $("#Supp_Country").val();
                $("#supplier_address").val("");
                $("#Supp_Pin").val("");
                $("#gst_num").val("");
                var gstCat = $("#Gst_Cat").val();
                if (gstCat == "RR") {
                    // $("#GSTMidPrt").val("");
                }
                else {
                    $("#GSTMidPrt").val("");
                }
                $("#GSTLastPrt").val("");
            /*start add code by  Hina on 09-01-2023  for ONchangeCountry Code*/
            if (custtype == "D") {
                // $("#Supp_Country").val("101");
                $("#Supp_Country").val(Supp_Country);
            }
            else {
                $("#Supp_Country").val("0").change();
            }

            $("#Supp_State").val("0").change();
            $("#Supp_District").val("0").trigger("change");
            $("#Supp_City").val("0").change();
            /*End add code by  Hina on 09-01-2023  for ONchangeCountry Code*/

                $("#chkCustomerBillingAddress").prop("checked", true);
            //$("#chkCustomerShippingAddress").prop("checked", true);

            /*End add code by  Hina on 09-01-2023  for ONchangeCountry Code*/

            /*<td id="supgstno">${$("#gst_num").val()}</td>*/
        //ReadSuppAddress();

            }
        })();
      
            
    }
   
}
async function CheckDuplicateGstNoAsync() {
    var GSTNum = "";
  
    var SuppGST = $("#gst_num").val().trim();
    var GSTMidPrt = $("#GSTMidPrt").val().trim();
    var GSTLastPrt = $("#GSTLastPrt").val().trim();
    if (GSTMidPrt != "" || GSTLastPrt != "") {
         GSTNum = SuppGST + GSTMidPrt + GSTLastPrt
       
    }
    else {
        if (GSTMidPrt == "" || GSTLastPrt == "") {
             GSTNum = GSTMidPrt + GSTLastPrt
           
        }
    }
    if (GSTNum != "") {
        const Supp_Id = $("#Supp_Id").val();
        const SupplierGst = GSTNum;

        const response = await $.ajax({
            type: "POST",
            url: "/BusinessLayer/SupplierDetail/CheckDependcyGstno",
            data: { Supp_Id: Supp_Id, SupplierGst: SupplierGst }
        });

        const arr = JSON.parse(response);
        if (arr.length > 0 && arr[0].Dependcy === "Use") {
            $("#SpanSuppGSTMidPrt").text($("#valueduplicate").text()).show();
            $("#GSTMidPrt, #GSTLastPrt").css("border-color", "red");
            $("#GSTMidPrt, #GSTLastPrt").css("border-color", "red");
            return false;
        } else {
            $("#SpanSuppGSTMidPrt").hide().text("");
            $("#GSTMidPrt, #GSTLastPrt").css("border-color", "#ced4da");
            return true;
        }
    }
    else {
        return true;
    }
   
}
function EditSuppAddrDetail() {

    debugger;

    /*----Commented by HIna on 09-01-2023 to show on page load*/
    //---------------------------- Row edit Button funtionality ------------------//
    //$("#tblSupplierAddressDetail >tbody >tr").on('click', "#editBtnIcon", function (e) {

    //debugger;
    //var currentRow = $(this).closest('tr')

    ////var currentRow = $(e.target).closest("tr")
    //    var row_index = currentRow.index();

    //    debugger;
    //    var address = currentRow.find("#suppadd").text()
    //    var cust_pin = currentRow.find("#supppin").text()
    //    var GSTNo = currentRow.find("#supgstno").text()
    //    var Statecode = GSTNo.substring(0, 2);
    //    var PanNum = GSTNo.substring(2, 12);
    //    var leftcode = GSTNo.substring(12, 15);
    //    var Country = currentRow.find("#hdnsuppcntry").text()
    //    var State = currentRow.find("#hdnsuppstate").text()
    //    var District = currentRow.find("#hdnsuppdist").text()
    //    var City = currentRow.find("#hdnsupcity").text()
    //    var address_id = currentRow.find("#address_id").text()
    //    debugger;
    //    var ShiddenRowNo = currentRow.find("#spanBillrowId").text();
    //    if (currentRow.find("#defbilladd div input").is(":checked")) {
    //        $("#chkCustomerBillingAddress").prop("checked", true);
    //    }
    //    else {
    //        $("#chkCustomerBillingAddress").prop("checked", false);
    //    }
    //    if (currentRow.find("#address_id div input").is(":checked")) {
    //        $("#chkCustomerShippingAddress").prop("checked", true);
    //    }
    //    else {
    //        $("#chkCustomerShippingAddress").prop("checked", false);
    //    }
    //    var col10_value = row_index
    //    debugger;
    //    $("#supplier_address").val(address);
    //    $("#Supp_Pin").val(cust_pin);
    //    $("#gst_num").val(Statecode);
    //    $("#GSTMidPrt").val(PanNum);
    //    $("#GSTLastPrt").val(leftcode);
    //    $("#HdnCountry").val(Country);
    //    $("#HdnState").val(State);
    //    $("#HdnDistrict").val(District);
    //    $("#HdnCity").val(City);
    //    $("#Supp_Country").val(Country);
    //    $("#Supp_State").val(State).trigger("change");
    //    //$("#Supp_District").val(District);


    //    //getsuppCity();

    //    //$("#Supp_City").val(City);


    //    $("#hdnSRowId").val(ShiddenRowNo);
    //    $("#UpdtAddrId").val(address_id);
    //    $("#divUpdate").css("display", "block");
    //    $('#divAddAddrDtl').css("display", "none");

    //    //onchangeAddress();
    //    onchangePin();
    //    onchangeGST();
    //});

    document.getElementById("SpanSuppAddr").innerHTML = "";
    $("#supplier_address").css('border-color', 'ced4da');
    $("#SpanSuppAddr").attr("style", "display: none;");
}
function ClearSuppAddressDetail() {
    debugger;
    $("#supplier_address").val("");
    /*-----------start Code bY Hina on 09-01-2023-------------------- */
    var suppcurr;
    if ($("#Import").is(":checked")) {
        suppcurr = "I";
    }
    if ($("#Local").is(":checked")) {
        suppcurr = "D";
    }
    if (suppcurr == "D") {
        $('#Supp_Country').val('101');

        /* $('#Supp_State').val('0');*/
        $('#Supp_District').empty();
        $('#Supp_District').append(`<option value="0">---Select---</option>`);
        $('#Supp_City').empty();
        $('#Supp_City').append(`<option value="0">---Select---</option>`);
        GetStateByCountry();
    }
    else {
        $("#Supp_Country").val("0").change();
        $("#Supp_Country").val(0);
        $("#HdnContry").val("0");
        $('#Supp_State').empty();
        $('#Supp_State').append(`<option value="0">---Select---</option>`);
        $('#Supp_District').empty();
        $('#Supp_District').append(`<option value="0">---Select---</option>`);
        $('#Supp_City').empty();
        $('#Supp_City').append(`<option value="0">---Select---</option>`);

    }
    /*-----------End Code bY Hina on 09-01-2023-------------------- */
    //$("#Supp_Country").val("");
    //$("#Supp_State").val("");
    //$("#Supp_District").val("");
    //$("#Supp_City").val("");

    $("#Supp_Pin").val("");
    $("#gst_num").val("");

    $("#GSTMidPrt").val("");
    $("#GSTLastPrt").val("");



    $("#divUpdate").css("display", "none");
    $('#divAddAddrDtl').css("display", "Block");
    $("#chkCustomerBillingAddress").prop("checked", true);
    $("#chkCustomerShippingAddress").prop("checked", true);
    document.getElementById("SpanSuppAddr").innerHTML = "";
    $("#supplier_address").css('border-color', '#ced4da');
    $("#SpanSuppAddr").attr("style", "display: none;");
    document.getElementById("vmSuppPin").innerHTML = "";
    $("#Supp_Pin").css('border-color', '#ced4da');
    $("#vmSuppPin").attr("style", "display: none;");
    document.getElementById("vmSuppCityname").innerHTML = "";
    $("[aria-labelledby='select2-Supp_city-container']").css('border-color', '#ced4da');
    $("#vmSuppCityname").attr("style", "display: none;");
}
function OnClickAddressUpdateBtn() {
    debugger;
    if (validationAddress() == false) {
        return false;
    }
    if (CheckDuplicateOnUpdateSupp() == false) {
        return false;
    }
    (async function () {
        const isGstValid = await CheckDuplicateGstNoAsync();
        if (!isGstValid) {
            return false;
        }

        else {
            debugger;
            var CustomerBillingAddress = "";

            if ($("#chkCustomerBillingAddress").is(":checked")) {
                CustomerBillingAddress = "Y";
            }
            else {
                CustomerBillingAddress = "N";
            }

            debugger;
            if (CustomerBillingAddress == 'Y') {
                var rowId = 0;
                $("#tblSupplierAddressDetail TBODY TR").each(function () {
                    debugger;
                    rowId = rowId + 1;

                    $(this).find("#defbilladd div input").prop("checked", false);
                    $(this).find("#spanhiddenBill_" + rowId).text("N");
                    //$(this).find("#chkCBA_" + rowId).prop("checked", false);
                    //$(this).find("#spanhiddenBill_" + rowId).text("N");
                });
            }
            debugger;
            var addr_idtoUpdt = $("#UpdtAddrId").val();
            var SuppGST = $("#gst_num").val().trim();
            var GSTMidPrt = $("#GSTMidPrt").val().trim();
            var GSTLastPrt = $("#GSTLastPrt").val().trim();
            if (GSTMidPrt != "" || GSTLastPrt != "") {
                var GSTNum = SuppGST + GSTMidPrt + GSTLastPrt
                $("#hdSupplierGst").val(GSTNum);
            } else {
                if (GSTMidPrt == "" || GSTLastPrt == "") {
                    var GSTNum = GSTMidPrt + GSTLastPrt
                    $("#hdSupplierGst").val(GSTNum);
                }
            }

            var CustCountry = $("#Cust_Country").val()
            $('#tblSupplierAddressDetail >tbody >tr').each(function () {
                var currentRow = $(this);
                debugger;


                var address_id = currentRow.find('#address_id').text();
                if (addr_idtoUpdt == address_id) {
                    /*code arrange and changes by Hina on 08-01-2023 to get value by Id country to pin*/

                    currentRow.find("#suppadd").text($("#supplier_address").val());
                    currentRow.find("#contryname").text($("#Supp_Country option:selected").text());
                    currentRow.find("#hdnsuppcntry").text($("#Supp_Country").val());
                    currentRow.find("#statename").text($("#Supp_State option:selected").text());
                    currentRow.find("#hdnsuppstate").text($("#Supp_State").val());
                    currentRow.find("#distname").text($("#Supp_District option:selected").text());
                    currentRow.find("#hdnsuppdist").text($("#Supp_District").val());
                    currentRow.find("#cityname").text($("#Supp_City option:selected").text());
                    currentRow.find("#hdnsupcity").text($("#Supp_City").val());
                    currentRow.find("#supppin").text($("#Supp_Pin").val());


                    /*currentRow.find("#supgstno").text($("#gst_num").val());*/
                    currentRow.find("#supgstno").text($("#hdSupplierGst").val());




                    if (CustomerBillingAddress == "Y") {
                        var BIllrowId = $(this).find("#spanBillrowId").text();
                        currentRow.find("#chkCBA_" + BIllrowId).prop("checked", true);
                        $("#spanhiddenBill_" + BIllrowId).text("Y");
                    }
                    else {
                        var BIllrowId = $(this).find("#spanBillrowId").text();
                        currentRow.find("#chkCBA_" + BIllrowId).prop("checked", false);
                        $("#spanhiddenBill_" + BIllrowId).text("N");
                    }
                    if (CustomerBillingAddress == "Y") {

                    }
                    else {
                        var rowId = 0;
                        var cheakflag = "N";
                        var cheackdefalt = "Q";
                        $("#tblSupplierAddressDetail TBODY TR").each(function () {
                            var BIllrowId = $(this).find("#spanBillrowId").text();
                            if ($(this).find("#chkCBA_" + BIllrowId).is(":checked")) {
                                cheackdefalt = "P";
                            }
                        });
                        if (cheackdefalt == "Q") {
                            debugger;
                            var rowIdx1 = 1;
                            var firstactive = "N";
                            $("#tblSupplierAddressDetail TBODY TR").each(function () {
                                debugger;
                                var currentRow = $(this);
                                if (firstactive == "N") {
                                    var hdn_id = currentRow.find("#hdn_id").text();
                                    if (hdn_id != "Delete") {
                                        currentRow.find("#defbilladd div input").prop("checked", true);
                                        $("#spanhiddenBill_" + rowIdx1).text("Y");
                                        firstactive = "Y";
                                        return false;
                                    }

                                }
                            });
                        }
                    }
                }

                debugger;

                $("#HdnCountry").val(0);
                $("#HdnState").val(0);
                $("#HdnDistrict").val(0);
                $("#HdnCity").val(0);
            });
            document.getElementById("SpanSuppAddr").innerHTML = "";
            $("#supplier_address").css('border-color', '#ced4da');
            $("#SpanSuppAddr").attr("style", "display: none;");
            $("#supplier_address").val("");
            $("#Supp_Pin").val("");
            $("#gst_num").val("");
            var gstCat = $("#Gst_Cat").val();
            if (gstCat == "RR") {
                // $("#GSTMidPrt").val("");
            }
            else {
                 $("#GSTMidPrt").val("");
            }
          
            $("#GSTLastPrt").val("");
            /*-----------Start Code bY Hina on 09-01-2023-------------------- */
            debugger;
            var SuppType = "";
            if ($("#Import").is(":checked")) {
                SuppType = "I";
                $("#HdnSuppType").val(SuppType);
            }
            /*if ($("#Domestic").is(":checked")) {*/
            if ($("#Local").is(":checked")) {
                SuppType = "D";
                $("#HdnSuppType").val(SuppType);
            }
            if (SuppType == "I") {
                //$("#HdnContry").val("0").change();
                //$('#Supp_Country').empty();
                //$('#Supp_Country').append(`<option value="0">---Select---</option>`);
                $("#Supp_Country").val("0").change();
                $("#Supp_State").val("0");
                $('#Supp_District').empty();
                $('#Supp_District').append(`<option value="0">---Select---</option>`);
                $('#Supp_City').empty();
                $('#Supp_City').append(`<option value="0">---Select---</option>`);
                //$("#Supp_District").val("0");
                //$("#Supp_City").val("0");
            }
            else {
                $("#Supp_State").val("0").change();
                $("#Supp_District").val("0");
                $("#Supp_City").val("0");
            }
            /*-----------End Code bY Hina on 09-01-2023-------------------- */


            //$("#suppstateID").val("");
            debugger;
            $("#divUpdate").css("display", "none");
            $('#divAddAddrDtl').css("display", "Block");
            $("#chkCustomerBillingAddress").prop("checked", true);
            $("#chkCustomerShippingAddress").prop("checked", true);

        }
    })();
                
   
}
function defbillchk(e) {
    debugger;
    var rowIdx = "";
    try {
        var clickedrow = $(e.target).closest("tr");
        var BillRowID = clickedrow.find("#spanBillrowId").text();
        //var BillRowID = clickedrow.find("#spanhiddenBill_" + rowIdx).text();
        if (clickedrow.find("#chkCBA_" + BillRowID).is(":checked")) {
            $("#tblSupplierAddressDetail TBODY TR").each(function () {
                debugger;
                rowIdx = $(this).find("#spanBillrowId").text();

                $(this).find("#chkCBA_" + rowIdx).prop("checked", false);
                $(this).find("#spanhiddenBill_" + rowIdx).text("N");
            });
            clickedrow.find("#chkCBA_" + BillRowID).prop("checked", true);
            clickedrow.find("#spanhiddenBill_" + BillRowID).text("Y");
            ReadSuppAddress();
        }
        //else {

        //    clickedrow.find("#spanhiddenBill_" + BillRowID).text("N");
        //    ReadSuppAddress();
        //}
    }
    catch (err) {

    }
}
/**Added This Function 01-01-2024 for approve btn**/
function OnclickApproveBtn() {
    if ($("#hdnSavebtn").val() == "AllreadyclickApprovebtn") {
        $("#btn_approve").css("filter", "grayscale(100%)");
        $("#btn_approve").attr("disabled", true);
    }
    else {
        $("#hdnSavebtn").val("AllreadyclickApprovebtn");
        $("#btn_approve").css("filter", "grayscale(100%)");
    }
}

/*Code by Hina on 05-01-2024 to Bind all data in dropdown and also OnChange COUNTRY,state,district,city */
function GetCountryOnChngSuppType(suppcurr) {
    debugger;
    //var SuppType;
    //if ($("#Import").is(":checked")) {
    //    SuppType = "I";
    //    $("#HdnSuppType").val(SuppType);
    //    $('#Supp_Country').empty();

    //}
    //if ($("#Domestic").is(":checked")) {
    //    SuppType = "D";
    //    $("#HdnSuppType").val(SuppType);
    //    $('#Supp_Country').empty();
    //}
    $('#Supp_Country').empty();

    $.ajax({
        type: "POST",
        url: "/BusinessLayer/SupplierDetail/GetCountryonChngSuppTyp",
        data: { SuppType: suppcurr },
        success: function (data) {
            debugger;
            var arr = [];
            arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
            if (arr.length > 0) {
                for (var i = 0; i < arr.length; i++) {
                    $('#Supp_Country').append(`<option value="${arr[i].country_id}">${arr[i].country_name}</option>`);
                }
                $("#Supp_Country").select2();/*For making Serachable Dropdown */
                $('#Supp_District').empty();
                $('#Supp_District').append(`<option value="0">---Select---</option>`);
                $('#Supp_City').empty();
                $('#Supp_City').append(`<option value="0">---Select---</option>`);
                $('#Supp_Pin').val("");
                $('#gst_num').val("");
                $('#supplier_address').val("");
                $('#GSTMidPrt').val("");
                $('#GSTLastPrt').val("");
                /*---To Hide Address Detail On ONChange of SuppTyp-----*/
                $("#tblSupplierAddressDetail tbody tr ").each(function () { $(this).find(".deleteIcon").click() })
                GetStateByCountry();
                var hdnCountryId = $("#Supp_Country").val()

            }
        },
    });
    /*GetProspectcurr();*/
}
function ClearAllData_OnChngSuppType() {
    debugger;

    //$('#customer_address').val("");
    $('#Supp_State').empty();
    $('#Supp_State').append('<option value="0">---Select---</option>')
    $('#Supp_District').empty();
    $('#Supp_District').append('<option value="0">---Select---</option>')
    $('#Supp_City').empty();
    $('#Supp_City').append('<option value="0">---Select---</option>')
    $('#Supp_Pin').val("");

    $('#gst_num').val("");
    $('#GSTMidPrt').val("");
    $('#GSTLastPrt').val("");
    $('#remarks').val("");
    //var TransType = $("#HdnProspectType").val();
    /*if (TransType == "D") {*/
    GetStateByCountry();
    /*  }*/
}
function OnChangeCountry() {
    debugger;
    document.getElementById("vmSuppCountryname").innerHTML = "";
    $("[aria-labelledby='select2-Supp_Country-container']").css("border-color", "#ced4da");
    GetStateByCountry();
}
function GetStateByCountry() {
    debugger;
    if ($("#Import").is(":checked")) {
        SuppType = "I";
        $("#HdnSuppType").val(SuppType);
        //$("#HdnState").val(0);
        
    }
    /*if ($("#Domestic").is(":checked")) {*/
    if ($("#Local").is(":checked")) {
        SuppType = "D";
        $("#HdnSuppType").val(SuppType);

    }
    var ddlCountryID = $("#Supp_Country").val();

    $.ajax({
        type: "POST",
        url: "/SupplierDetail/GetstateOnCountry",/*Controller=SupplierSetup and Fuction=Getsuppport*/
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async: true,
        data: { ddlCountryID: ddlCountryID, },/*Registration pass value like model*/
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            /*dynamically dropdown list of all Assessment */
            debugger;
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
                if (arr.length > 0) {
                    $('#Supp_State').empty();
                    $('#Supp_State').append(`<option value="0">---Select---</option>`);
                    for (var i = 0; i < arr.length; i++) {

                        $('#Supp_State').append(`<option value="${arr[i].state_id}">${arr[i].state_name}</option>`);
                    }
                    $("#Supp_State").select2();/*For making Serachable Dropdown */
                    if (SuppType == "I") {
                        var hdnSTATE = $("#HdnState").val()
                        if (hdnSTATE == "") {
                            hdnSTATE = "0";
                        }
                        $("#Supp_State").val(hdnSTATE).trigger("change");
                        $("#HdnState").val("0");
                    }

                }
                else {
                    $('#Supp_State').empty();
                    $('#Supp_State').append(`<option value="0">---Select---</option>`);
                }


            }
        },
        error: function (Data) {
        }
    });
};
function OnChangeState() {
    debugger;
    $('#Supp_City').empty();
    $('#Supp_City').append(`<option value="0">---Select---</option>`);
    //$('#Supp_Pin').val("");
    GetDistrictByState();
    GstStateCodeOnChangeStatus();
    $('#vmSuppStatename').text("");
    $("#vmSuppStatename").css("display", "none");
    $("[aria-labelledby='select2-Supp_State-container']").css('border-color', "#ced4da");
    $("#Supp_State").css('border-color', "#ced4da");

}
function GetDistrictByState() {
    debugger;

    var ddlStateID = $("#Supp_State").val();

    $.ajax({
        type: "POST",
        url: "/SupplierDetail/GetDistrictOnState",/*Controller=SupplierSetup and Fuction=Getsuppport*/
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async: true,
        data: { ddlStateID: ddlStateID, },/*Registration pass value like model*/
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            /*dynamically dropdown list of all Assessment */
            debugger;
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
                if (arr.length > 0) {
                    $('#Supp_District').empty();

                    for (var i = 0; i < arr.length; i++) {
                        $('#Supp_District').append(`<option value="${arr[i].district_id}">${arr[i].district_name}</option>`);
                    }
                    $("#Supp_District").select2();/*For making Serachable Dropdown */

                    var hdnDistrct = $("#HdnDistrict").val();
                    var hdn_state = $("#HdnState").val();
                    var Supp_State = $("#Supp_State").val();
                    if (hdn_state == Supp_State) {
                        if (hdnDistrct != "" && hdnDistrct != null) {
                            $("#Supp_District").val(hdnDistrct).trigger("change");
                        }
                    }
                   
                   
                   // $("#HdnDistrict").val("0");
                    //$("[aria-labelledby='select2-Supp_District-container']").css('border-color', 'red');
                    //$("#Supp_District").css('border-color', 'red');
                    //$("#vmSuppDistname").attr("style", "display: block;");

                }
            }
        },
        error: function (Data) {
        }
    });
};
function GstStateCodeOnChangeStatus() {
    debugger

    var stateCode = $('#Supp_State').val();
    $.ajax({
        type: "POST",
        url: "/SupplierDetail/GetStateCode",/*Controller=SupplierSetup and Fuction=Getsuppport*/
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async: true,
        data: { stateId: stateCode, },/*Registration pass value like model*/
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            debugger;
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
                if (arr.Table.length > 0) {
                    $('#gst_num').val(arr.Table[0].state_code);
                }
            }
        },
        error: function (Data) {
        }
    });

}
function OnChangeDistrict() {
    debugger;

    GetCityByDistrict();
    //var District = $("#Supp_District").val();
    //var state = $("#Supp_State").val("")
    //if (state != "0") {
    //    if (District == '0') {
    //        document.getElementById("vmSuppDistname").innerHTML = $("#valueReq").text();
    //        $("[aria-labelledby='select2-Supp_District-container']").css('border-color', 'red');
    //        $("#Supp_District").css('border-color', 'red');
    //        $("#vmSuppDistname").attr("style", "display: block;");
    //    }
    //    else {
    $('#vmSuppDistname').text("");
    $("#vmSuppDistname").css("display", "none");
    //$("#CmnDistrict").css("border-color", "red");
    $("[aria-labelledby='select2-Supp_District-container']").css('border-color', "#ced4da");
    //    }
    //}


}

function GetCityByDistrict() {
    debugger;

    var ddlDistrictID = $("#Supp_District").val();

    $.ajax({
        type: "POST",
        url: "/SupplierDetail/GetCityOnDistrict",/*Controller=SupplierSetup and Fuction=Getsuppport*/
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async: true,
        data: { ddlDistrictID: ddlDistrictID, },/*Registration pass value like model*/
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            /*dynamically dropdown list of all Assessment */
            debugger;
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
                if (arr.length > 0) {
                    $('#Supp_City').empty();
                    for (var i = 0; i < arr.length; i++) {
                        $('#Supp_City').append(`<option value="${arr[i].city_id}">${arr[i].city_name}</option>`);
                    }
                    $("#Supp_City").select2();/*For making Serachable Dropdown */
                    var hdnCity = $("#HdnCity").val();
                    var hdn_state = $("#HdnState").val();
                    var Supp_State = $("#Supp_State").val();
                    if (hdn_state == Supp_State) {
                        if (hdnCity != null && hdnCity != "") {
                            $("#Supp_City").val(hdnCity).trigger("change");
                        }
                    }
                   // $("#HdnCity").val('0');

                    //$("[aria-labelledby='select2-Supp_City-container']").css('border-color', 'red');
                    //$("#Supp_City").css('border-color', 'red');
                    //$("#vmSuppCityname").attr("style", "display: block;");
                }


            }
        },
        error: function (Data) {
        }
    });
};
function OnChangeCity() {
    debugger;
    //var City = $("#Supp_City").val()
    //var state = $("#Supp_State").val()
    //if (state != "0") {
    //    if (City == "0") {
    //        document.getElementById("vmSuppCityname").innerHTML = $("#valueReq").text();
    //        $("[aria-labelledby='select2-Supp_City-container']").css('border-color', 'red');
    //        $("#Supp_City").css('border-color', 'red');
    //        $("#vmSuppCityname").attr("style", "display: block;");
    //    }
    //    else {
    $('#vmSuppCityname').text("");
    $("#vmSuppCityname").css("display", "None");
    $("[aria-labelledby='select2-Supp_City-container']").css('border-color', "#ced4da");
    //    }
    //}
}
function BtnSearchPurchaseHistory() {

    debugger;
    var FromDate = $("#txtFromdate").val();
    var ToDate = $("#txtTodate").val();
    var Supp_Id = $("#Supp_Id").val();
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/SupplierDetail/GetSupplierPurchaseDetail",/*Controller=ItemSetup and Fuction=GetItemList*/
        data: {
            FromDate: FromDate,
            ToDate: ToDate,
            Supp_Id: Supp_Id,
        },
        success: function (data) {
            debugger;
            $('#PurchaseHistory').html(data);

        },


    });

}
function FromToDateValidation() {
    debugger;
    var FromDate = $("#txtFromdate").val();
    var ToDate = $("#txtTodate").val();
    if ((FromDate != null || FromDate == "") && (ToDate != null || ToDate == "")) {
        if (FromDate > ToDate) {

            var now = new Date();
            var month = (now.getMonth() + 1);
            var day = now.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = now.getFullYear() + '-' + month + '-' + day;
            $("#txtTodate").val(today);

            var fromDate = new Date($("#hdFromdate").val());

            var month = (fromDate.getMonth() + 1);
            var day = fromDate.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = fromDate.getFullYear() + '-' + month + '-' + day;
            $("#txtFromdate").val(today);
            //alert('From Date cannnot be greater then To date');
            swal("", $("#fromtodatemsg").text(), "warning");
        }
    }
    else {
        //alert('please select from and to date');
    }
}
function OnClickIconBtn(e) {
    var ItmCode = $(e.target).closest('tr').find("#Hdn_item_id").val();
    ItemInfoBtnClick(ItmCode);
}
/*------------Code start Add by Shubham Maurya on 15-11-2025 to bind all licance table data-----------------*/
function AddLicensedetail() {
    var LicenseName = $("#LicenseName").val();
    var LicenseNumber = $("#LicenseNumber").val();
    var ExpiryDate = $("#ExpiryDate").val();
    var formattedDate = "";
    if (ExpiryDate && ExpiryDate.trim() !== "") {
        var parts = ExpiryDate.split("-");
        if (parts.length === 3) {
            formattedDate = parts[2] + "-" + parts[1] + "-" + parts[0];
        }
    }
    var ExpiryAlertDays = $("#ExpiryAlertDays").val();
    var CurrentDate = moment().format('YYYY-MM-DD');

    var rowIdx = 0;
    debugger;
    var rowId = $("#LicenseDetailTble TBODY TR").length;
    if (rowId == "" || rowId == null) {
        rowIdx = 1;
    }
    else {
        rowIdx = parseInt(rowId) + 1;
    }
    if (LicenseName != "" && LicenseName != null) {
        $("#LicenseDetailTble >tbody").append(`<tr id="R${rowIdx}"> 
       <td class=" red center"> <i class=" fa fa-trash" aria-hidden="true" id="DeliveryDelIconBtn" onclick="deleteLicenseDetail(event)" title="${$("#Span_Delete_Title").text()}"></i></td>
                                            <td class="center edit_icon"> <i class="fa fa-edit" aria-hidden="true" onclick="EditLicenseDetail(event)" title="${$("#Edit").text()}"></i></td>
                                            <td id="rowno" >${rowIdx}</td>
                                            <td id="srno"  hidden="hidden">${rowIdx}</td>
                                            <td id="LicenseNm">${LicenseName}</td>
                                            <td id="LicenseNum">${LicenseNumber}</td>
                                            <td id="ExpDate">${formattedDate}</td>
                                            <td id="ExpiryAlrtDays">${ExpiryAlertDays}</td>
        </tr>`);
    }
    else {
        $("#LicenseName").css("border-color", "Red");
        $('#SpanLicenseName').text($("#valueReq").text());
        $("#SpanLicenseName").css("display", "block");
        return false;
    }
    $("#LicenseName").val("");
    $("#LicenseNumber").val("");
    $("#ExpiryDate").val("");
    $("#ExpiryAlertDays").val("");
}
function InsertFinalLicenceDetail() {
    debugger;
    var LicenceList = [];
    $("#LicenseDetailTble >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var l_id = currentRow.find("#srno").text();
        var LicenseNm = currentRow.find("#LicenseNm").text();
        var LicenseNum = currentRow.find("#LicenseNum").text();
        var ExpDate1 = currentRow.find("#ExpDate").text();
        var formattedDate = "";
        if (ExpDate1 && ExpDate1.trim() !== "") {
            var parts = ExpDate1.split("-");
            if (parts.length === 3) {
                formattedDate = parts[2] + "-" + parts[1] + "-" + parts[0];
            }
        }
        var ExpiryAlrtDays = currentRow.find("#ExpiryAlrtDays").text();
        if (ExpiryAlrtDays == "") {
            ExpiryAlrtDays = 0;
        }
        LicenceList.push({
            l_id: l_id, LicenseNm: LicenseNm, LicenseNum: LicenseNum, ExpDate: formattedDate, ExpiryAlrtDays: ExpiryAlrtDays
        });
    });
    return LicenceList;
};
function deleteLicenseDetail(e) {
    debugger;
    var currentRow = $(e.target).closest("tr");
    currentRow.remove();
    SerialNoAfterDelete();
}
function EditLicenseDetail(e) {
    debugger;
    var currentRow = $(e.target).closest("tr");
    var srno = currentRow.find("#srno").text();
    var lic_name = currentRow.find("#LicenseNm").text();
    var lic_number = currentRow.find("#LicenseNum").text();
    var ExpDate = currentRow.find("#ExpDate").text();
    var parts = ExpDate.split("-");   // ["2025", "11", "15"]
    var formattedDate = parts[2] + "-" + parts[1] + "-" + parts[0];
    var ExpiryAlrtDays = currentRow.find("#ExpiryAlrtDays").text();

    $("#hdn_lid").val(srno);
    $("#LicenseName").val(lic_name);
    $("#LicenseNumber").val(lic_number);
    $("#ExpiryDate").val(formattedDate);
    $("#ExpiryAlertDays").val(ExpiryAlrtDays);

    $("#divAddlicenseDtl").attr("style", "display: none;");
    $("#divUpdateLicense").attr("style", "display: block;");
}
function OnClickLicenceUpdateBtn() {
    debugger;
    var srno = $("#hdn_lid").val();
    var LicenseName = $("#LicenseName").val();
    var LicenseNumber = $("#LicenseNumber").val();
    var ExpiryDat = $("#ExpiryDate").val();
    var formattedDate = "";
    if (ExpiryDat && ExpiryDat.trim() !== "") {
        var parts = ExpiryDat.split("-");
        if (parts.length === 3) {
            formattedDate = parts[2] + "-" + parts[1] + "-" + parts[0];
        }
    }
    var ExpiryAlertDays = $("#ExpiryAlertDays").val();
    $("#LicenseDetailTble > tbody > tr #srno:contains(" + srno + ")").closest('tr').each(function () {
        let row = $(this);
        row.find("#LicenseNm").text(LicenseName);
        row.find("#LicenseNum").text(LicenseNumber);
        row.find("#ExpDate").text(formattedDate);
        row.find("#ExpiryAlrtDays").text(ExpiryAlertDays);
    });

    $("#LicenseName").val("");
    $("#LicenseNumber").val("");
    $("#ExpiryDate").val("");
    $("#ExpiryAlertDays").val("");

    $("#divAddlicenseDtl").attr("style", "display: block;");
    $("#divUpdateLicense").attr("style", "display: none;");
}
function SerialNoAfterDelete() {
    debugger;
    var SerialNo = 0;
    $("#LicenseDetailTble >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#rowno").text(SerialNo);
    });
};
function OnChangeLicenseName() {
    $("#SpanLicenseName").css("display", "none");
    $("#LicenseName").css("border-color", "#ced4da");
}
/*------------Code End by Shubham Maurya on 15-11-2025 to bind all licance table data-----------------*/


