/*
    <<< ----Commentes for change in Page---->>>
    Modified by Suraj on 16-10-2024 : Id Changed from suppcurr to Custcurr 
    <<< ----Commentes for change in Page End---->>>
*/
$(document).ready(function () {
    debugger;
    ValidationForPanNoSetAttr();
    OnClickInterBranch();
    $("#ddlcust_zone").select2();
    $("#ddlcust_group").select2();
    $("#GLAccGrp").select2();
    $("#ID_DefaultTransporter").select2();
    /*add code by  Hina on 10-01-2023  for pageload*/
    $("#Cust_State").select2();
    $("#HdnCustDistrict").val("0");
    $("#HdnCustCity").val("0");
    var custtype;
    if ($("#Export").is(":checked")) {
        custtype = "E";
    }
    if ($("#Domestic").is(":checked")) {
        custtype = "D";
    }
    if (custtype == "E") {
        $("#Cust_Country").select2();
    }
    /*Commented By Hina on 10-01-2023 to create this functionality on Pageload under Supervision of Premsir*/
    //EditAddrDetail();

    ReadCustAddress();
    ReadBranchList()
    debugger;
    var PageName = sessionStorage.getItem("PageName");
    $('#GRNDetailsPageName').text(PageName);
    sessionStorage.setItem("PageName", null);


    /*Commented By Hina on 10-01-2023 to chnge on country code*/
    //getCustCity();
    //---------------------------- Row edit Button funtionality ------------------//
    /*Remove tr in this table("#tblCustomerAddressDetail >tbody>tr") by Hina on 11-01-2024 to hit only body not row*/
    $("#tblCustomerAddressDetail >tbody").on('click', "#editBtnIcon", function (e) {

        debugger;
        /*Add code by Hina on 09-01-2024 */
        var custtype;
        if ($("#Export").is(":checked")) {
            custtype = "E";
        }
        if ($("#Domestic").is(":checked")) {
            custtype = "D";
        }
        var currentRow = $(this).closest('tr')
        var row_index = currentRow.index();

        var address = currentRow.find("#hdncustadd").text()
        var cust_pin = currentRow.find("#hdncustpin").text()
        var GSTNo = currentRow.find("#hdncustgst").text()
        var tbl_Addrcont_per = currentRow.find("#tblAddrcont_per").text()
        var tbl_Addrcont_no = currentRow.find("#tblAddrcont_no").text()

        var Statecode = GSTNo.substring(0, 2);
        var PanNum = GSTNo.substring(2, 12);
        var leftcode = GSTNo.substring(12, 15);
        /*Start Code by Hina on 11-01-2023*/
        var Country = currentRow.find("#hdncustcntry").text()
        var State = currentRow.find("#hdncuststate").text()
        var District = currentRow.find("#hdncustdist").text()
        /*End Code by Hina on 11-01-2023*/
        var City = currentRow.find("#hdncustcity").text()
        var address_id = currentRow.find("#address_id").text()
        debugger;
        var hiddenRowNo = currentRow.find("#spanBillrowId").text();
        if (currentRow.find("#chkCBA_" + hiddenRowNo).is(":checked")) {
            $("#chkCustomerBillingAddress").prop("checked", true);
        }
        else {
            $("#chkCustomerBillingAddress").prop("checked", false);
        }
        if (currentRow.find("#chkCSA_" + hiddenRowNo).is(":checked")) {
            $("#chkCustomerShippingAddress").prop("checked", true);
        }
        else {
            $("#chkCustomerShippingAddress").prop("checked", false);
        }
        var DomesticChecked = $("#Domestic").is(":checked");
        if (DomesticChecked) {
            var GstCat = $('#Gst_Cat').val();
            var GstApplicable = $('#hdnGstApplicable').val();

            if (GstApplicable != "N") {
                if (GstCat != "UR") {
                    var billingChecked = currentRow.find("#chkCBA_" + hiddenRowNo).is(":checked");
                    var shippingChecked = currentRow.find("#chkCSA_" + hiddenRowNo).is(":checked");



                    if (billingChecked) {
                        if (GstCat != "EX" && GstCat != "CO" && GstCat != "SZ") {
                            $("#GSTNumrequired").show();
                            $("#GSTMidPrt").attr("disabled", true);
                        }

                    }
                    else if (!billingChecked && shippingChecked) {
                        $("#GSTNumrequired").hide();
                        $("#GSTMidPrt").attr("disabled", false);
                    }
                    else {
                        if (GstCat != "EX" && GstCat != "CO" && GstCat != "SZ") {
                            $("#GSTNumrequired").show();
                            $("#GSTMidPrt").attr("disabled", false);
                        }
                    }
                }
            }
        }
        var col10_value = row_index
        debugger;
        //$("#hdnRow_id").val(row_index);
        $("#customer_address").val(address);
        $("#Cust_Pin").val(cust_pin);
        $("#gst_num").val(Statecode);
        $("#GSTMidPrt").val(PanNum);
        $("#GSTLastPrt").val(leftcode);
        /*Code by Hina on 11-01-2024*/
        $("#HdnCustContry").val(Country);
        $("#HdnCustState").val(State);
        $("#HdnCustDistrict").val(District);
        $("#HdnCustDistrict").val(District);
        $("#HdnCustCity").val(City);
        $("#Addr_cont_per").val(tbl_Addrcont_per);
        $("#Addr_cont_no").val(tbl_Addrcont_no);
        if (custtype == "D") {
            $("#Cust_Country").val(Country);
            $("#Cust_State").val(State).trigger("change");
        }
        else {
            $("#Cust_Country").val(Country).trigger("change");
            //$("#Supp_State").val(State).trigger("change");
        }
        /*Commented by Hina on 11-01-2024*/
        //getCustCity();
        //$("#Cust_City").val(City).trigger("change");
        debugger;
        $("#hdnRowId").val(hiddenRowNo);
        $("#UpdtAddrId").val(address_id);
        $("#divUpdate").css("display", "block");
        $('#divAddAddrDtl').css("display", "none");
        onchangeCustAddr();
        onchangeCustPin();
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
        // Removing the current row. 

        var addr_id = $(this).closest('tr')[0].cells[19].innerHTML;
        var Flag = $(this).closest('tr')[0].cells[20].innerHTML;
        debugger;
        var custId = $("#cust_ID").val();
        var IdonEdit = $("#hdnRowId").val();
        var customer_address = $("#customer_address").val();
        var customer_Pin = $("#Cust_Pin").val();
        var CustomerCity_Name = $("#Cust_City").val();
        currentRow = $(this).closest('tr');
        rowId = currentRow.find("#spanBillrowId").text();
        var address = currentRow.find("#hdncustadd").text();
        var cust_pin = currentRow.find("#hdncustpin").text();
        var City = currentRow.find("#hdncustcity").text();

        rowId = currentRow.find("#spanBillrowId").text();
        if (custId == null || custId == "") {
            $(this).closest('tr').remove();
            if (IdonEdit == rowId) {
                ClearAddressDetail();
            }
            document.getElementById("SpancustAddr").innerHTML = "";
            $("#customer_address").css('border-color', '#ced4da');
            $("#SpancustAddr").attr("style", "display: none;");
            document.getElementById("vmCustPin").innerHTML = "";
            $("#Cust_Pin").css('border-color', '#ced4da');
            $("#vmCustPin").attr("style", "display: none;");
            document.getElementById("vmCustCityname").innerHTML = "";
            $("[aria-labelledby='select2-Cust_City-container']").css('border-color', '#ced4da');
            $("#vmCustCityname").attr("style", "display: none;");


            /*  AddrIdAfterDelete();*/
            var firstactive = "N";
            var rowIdx1 = "";
            var rowIdx12 = "";
            debugger;
            var len = $("#tblCustomerAddressDetail >tbody >tr").length;
            $("#tblCustomerAddressDetail >tbody >tr").each(function (i, row) {
                debugger;
                var currentRow = $(this).closest('tr');
                if (firstactive == "N") {
                    if (len > 0) {
                        rowIdx = currentRow.find("#spanBillrowId").text();
                        if (currentRow.find("#chkCBA_" + rowIdx).is(":checked")) {
                            firstactive = "Y";
                        }
                    }
                }
            });
            $("#tblCustomerAddressDetail >tbody >tr").each(function (i, row) {
                debugger;
                var currentRow = $(this).closest('tr');
                if (firstactive == "N") {
                    rowIdx = $(this).find("#spanBillrowId").text();
                    (currentRow.find("#chkCBA_" + rowIdx).prop("checked", true));
                    $("#spanhiddenBill_" + rowIdx).text("Y");
                    firstactive = "Y";
                }
            });
            var firstactive = "N";
            debugger;
            var len = $("#tblCustomerAddressDetail >tbody >tr").length;
            $("#tblCustomerAddressDetail >tbody >tr").each(function (i, row) {
                debugger;
                var currentRow = $(this).closest('tr');
                if (firstactive == "N") {
                    if (len > 0) {
                        rowIdx = currentRow.find("#spanBillrowId").text();
                        if (currentRow.find("#chkCSA_" + rowIdx).is(":checked")) {
                            firstactive = "Y";
                        }
                    }
                }
            });
            $("#tblCustomerAddressDetail >tbody >tr").each(function (i, row) {
                debugger;
                var currentRow = $(this).closest('tr');
                if (firstactive == "N") {
                    var rowIdx12 = $(this).find("#spanShiprowId").text();
                    (currentRow.find("#chkCSA_" + rowIdx12).prop("checked", true));
                    $("#spanhiddenShip_" + rowIdx12).text("Y");
                    firstactive = "Y";
                }
            });
        }
        else {
            var currentrow = $(this).closest('tr');
            document.getElementById("SpancustAddr").innerHTML = "";
            $("#customer_address").css('border-color', '#ced4da');
            $("#SpancustAddr").attr("style", "display: none;");
            document.getElementById("vmCustPin").innerHTML = "";
            $("#Cust_Pin").css('border-color', '#ced4da');
            $("#vmCustPin").attr("style", "display: none;");
            document.getElementById("vmCustCityname").innerHTML = "";
            $("[aria-labelledby='select2-Cust_City-container']").css('border-color', '#ced4da');
            $("#vmCustCityname").attr("style", "display: none;");

            ChakeDependencyAddr(custId, addr_id, currentrow);
        }

        ReadCustAddress();
        // rowIdx--;

    });

    var FinStDt;
    var HisStDt = $("#HistoryFrom_date").val();
    if (HisStDt == "") {
        var FinStDt = $("#HiddenFinDt").val();
        $("#HistoryFrom_date").val(FinStDt);
    }
    else {
        FinStDt = $("#HistoryFrom_date").val()
        $("#HistoryFrom_date").val(FinStDt);
    }
    var HisToDt = "";
    HisToDt = $("#HistoryTo_date").val();
    var Date12 = moment().format('YYYY-MM-DD');

    $("#HistoryTo_date").val(Date12);

    if ($("#act_status").is(":checked")) {
        $("#reasonrequired").attr("style", "display: none;");

    }
    else {
        $("#reasonrequired").css("display", "inherit");
    }
    if ($("#cust_hold").is(":checked")) {

        $("#holdrequired").css("display", "inherit");
    }
    else {
        $("#holdrequired").attr("style", "display: none;");
    }
    var Disable = $("#Disable").val();
    debugger;
    if (Disable != "Disable") {
        var Custcurr;
        if ($("#Export").is(":checked")) {
            Custcurr = "E";
        }
        if ($("#Domestic").is(":checked")) {
            Custcurr = "D";
        }
        if (Custcurr == "E") {
            //var s = '<option value="0">---Select---</option>';
            //$("#Gst_Cat").html(s);
            //$("#Gst_Cat").val("---Select---");
            //$("#Gst_Cat").prop('disabled', true);
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
        var Custcurr;
        if ($("#Export").is(":checked")) {
            Custcurr = "E";
        }
        if ($("#Domestic").is(":checked")) {
            Custcurr = "D";
        }
        if (Custcurr == "E") {
            //var s = '<option value="0">---Select---</option>';
            //$("#Gst_Cat").html(s);
            //$("#Gst_Cat").val("---Select---");
            //$("#Gst_Cat").prop('disabled', true);

            //$("#RequiredGSTCategory").attr("style", "display: none;");
            //$("#GSTNumrequired").attr("style", "display: none;");
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

    //Gstvalidaction();
    debugger;
    Cmn_ByDefaultBranchData("#CustBrDetail", "#hdCustomerBranchId", "#cust_act_stat_", "Cust", "#HdnBranchActiveSatus");
    BingGLReporting();

    var Status = $("#Status").val();
    if (Status != "" && Status != null) {
        // Get the text content of the country element
        var country = $("#Cust_Country").text().trim().toLowerCase();

        // Check if the country is India and the Domestic checkbox is checked
        if ((country) === "india" && $("#Domestic").is(":checked")) {
            // Set the maximum length of the PIN input to 6
            $("#Cust_Pin").attr("maxlength", "6");

        }
        else {
            $("#Cust_Pin").attr("maxlength", "12");
        }
    }
    if ($("#Export").is(":checked")) {
        Custcurr = "E";
    }
    if ($("#Domestic").is(":checked")) {
        Custcurr = "D";
    }
   
    var gst_cat = $("#Gst_Cat").val();
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (gst_cat == "UR" || Custcurr == "E") {
        $("#GSTNumrequired").attr("style", "display: none;");

        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#SpanCustGSTMidPrt").attr("style", "display: none;");
        document.getElementById("SpanCustGSTMidPrt").innerHTML = "";

        $("#GSTLastPrt").css('border-color', "#ced4da");
        $("#SpancustGST").attr("style", "display: none;");
        document.getElementById("SpancustGST").innerHTML = "";
        $("#GSTMidPrt").val("");
        $("#GSTLastPrt").val("");
        $("#GSTMidPrt").attr("disabled", true);
        $("#GSTLastPrt").attr("disabled", true);
       
    }
    else if (gst_cat == "EX" || gst_cat == "CO" || gst_cat == "SZ" || GstApplicable =="N") {
        $("#GSTNumrequired").attr("style", "display: none;");

        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#SpanCustGSTMidPrt").attr("style", "display: none;");
        document.getElementById("SpanCustGSTMidPrt").innerHTML = "";

        $("#GSTLastPrt").css('border-color', "#ced4da");
        $("#SpancustGST").attr("style", "display: none;");
        document.getElementById("SpancustGST").innerHTML = "";
        $("#GSTMidPrt").attr("disabled", false);
        $("#GSTLastPrt").attr("disabled", false);
    }
    else {
        $("#GSTNumrequired").css("display", "inherit");
        //$("#GSTMidPrt").attr("disabled", false);
        $("#GSTMidPrt").attr("disabled", true);
        $("#GSTLastPrt").attr("disabled", false);
    }

    var create_id = $("#CreatedBy").val();
    var DisablePageLavel = $("#hdnDisable").val();
    if (create_id != "" && DisablePageLavel != "Y") {
        var gst_cat = $("#Gst_Cat").val();
        var GstApplicable = $("#Hdn_GstApplicable").text();
        if (gst_cat == "RR" && GstApplicable == "Y") {

            var panNo = $("#pan_num").val();
            if (panNo.length == 10) {
                $("#GSTMidPrt").val(panNo);
            }
            else {
                $("#GSTMidPrt").val("");
            }

        }

    }
});

function OnClickInterBranch() {
    if ($("#InterBranch").is(":checked")) {
        $("#hdn_InterBranch").val("Y");
    }
    else {
        $("#hdn_InterBranch").val("N");

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
function ReadBranchList() {
    debugger;
    var Branches = new Array();
    $("#CustBrDetail TBODY TR").each(function () {
        /*    debugger;*/
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

    var str = JSON.stringify(Branches);
    $('#hdCustomerBranchList').val(str);
}
function BingGLReporting() {
    debugger;
    $("#ID_GlRepoting_Group").select2({
        ajax: {
            type: "POST",
            url: "/BusinessLayer/CustomerDetails/GetGlReportingGrp",
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
    var glrtpID = $("#ID_GlRepoting_Group").val();
    var glrtpName = $("#ID_GlRepoting_Group option:selected").text();
    $("#hdn_GlRepoting_Group").val(glrtpID);
    $("#hdn_GlRepoting_GroupName").val(glrtpName);
}
function OnChangeDefaultTransporter() {
    var Transport = $("#ID_DefaultTransporter").val();
    $("#hdn_DefaultTransporter").val(Transport);
}
function getCustCity() {
    debugger;
    //Cmn_getCityList("cust_city");
    $("#cust_city").select2({

        ajax: {
            url: $("#CustCity_Name").val(),

            data: function (params) {
                var queryParameters = {
                    //restrictedCountry: $("#resCountry").val(),  // pass your own parameter                
                    ddlGroup: params.term, // search term like "a" then "an"
                    Group: params.page
                    //ddlCity: $("#Custlier_city").val()
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
function AddrIdAfterDelete() {
    var Addr_Id = "";
    $("#tblCustomerAddressDetail >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        Addr_Id = $(this).find("#spanBillrowId").text();
        currentRow.find("#spanShiprowId").text(Addr_Id);
    });
};
function ChakeDependencyAddr(custId, e, currentrow) {
    var addr_id = e;
    debugger;
    $.ajax({
        type: "POST",
        url: "/CustomerDetails/ChakeDependencyAddr",
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
                Exist = "Yes";
                swal("", $("#DependencyExist").text(), "warning");
            }
            else {
                var rowIdx = currentrow.find("#spanBillrowId").text();
                currentrow.find("#chkCBA_" + rowIdx).prop("checked", false);
                currentrow.find("#chkCSA_" + rowIdx).prop("checked", false);
                currentrow.hide();
                var IdonEdit = $("#hdnRowId").val();
                var customer_address = $("#customer_address").val();
                var customer_Pin = $("#Pin").val();
                var CustomerCity_Name = $("#cust_city").val();
                var address = currentrow.find("#hdncustadd").text();
                var cust_pin = currentrow.find("#hdncustpin").text();
                var City = currentrow.find("#hdncustcity").text();
                debugger;
                var IdonEdit = $("#hdnRowId").val();
                if (IdonEdit == rowIdx) {
                    ClearAddressDetail();
                }

                debugger;
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

    var rowIdx = 1;

    $("#tblCustomerAddressDetail >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var a = currentRow.find("#address_id").text();
        if (e == a) {
            currentRow.find("#hdnID").text("Delete");
        }
        if (e == a) {
            debugger;
            var firstactive = "N";
            var rowIdx1 = 1;
            var len = $("#tblCustomerAddressDetail >tbody >tr").length;
            $("#tblCustomerAddressDetail >tbody >tr").each(function (i, row) {
                var currentRow = $(this).closest('tr');
                if (firstactive == "N") {
                    if (len > 0) {
                        var billid = $(this).find("#spanBillrowId").text();
                        if (currentRow.find("#chkCBA_" + billid).is(":checked")) {
                            firstactive = "Y";
                        }
                    }
                }
            });
            var rowId = 0;
            //Comment by Hina on 6 - 12 - 2022 for bydefault biiling & shipping 
            //addres after delete on save also add ReadCustAddress() both section
            //var rowIdx1 = 1;
            var rowIdx1 = 0;
            $("#tblCustomerAddressDetail TBODY TR").each(function () {
                debugger;
                rowIdx1 = rowIdx1 + 1;
                var currentRow = $(this);
                if (firstactive == "N") {
                    var hdn_id = currentRow.find("#hdnID").text();
                    if (hdn_id != "Delete") {
                        var billid = $(this).find("#spanBillrowId").text();
                        currentRow.find("#chkCBA_" + billid).prop("checked", true);
                        $("#spanhiddenBill_" + rowIdx1).text("Y");
                        firstactive = "Y";
                        ReadCustAddress();

                    }
                }
            });
            var rowIdx1 = 1;
            var firstactiv = "N";
            var len = $("#tblCustomerAddressDetail >tbody >tr").length;
            $("#tblCustomerAddressDetail >tbody >tr").each(function (i, row) {
                var currentRow = $(this).closest('tr');
                if (firstactiv == "N") {
                    if (len > 0) {
                        var billid = $(this).find("#spanBillrowId").text();
                        if (currentRow.find("#chkCSA_" + billid).is(":checked")) {
                            firstactiv = "Y";

                        }
                    }
                }
            });
            var rowId = 0;
            //Comment by Hina on 6 - 12 - 2022 for bydefault biiling & shipping 
            //addres after delete on save also add ReadCustAddress() both section
            /*var rowIdx1 =1;*/
            var rowIdx1 = 0;
            $("#tblCustomerAddressDetail TBODY TR").each(function () {
                debugger;
                rowIdx1 = rowIdx1 + 1;
                var currentRow = $(this);
                if (firstactiv == "N") {
                    var hdn_id = currentRow.find("#hdnID").text();
                    if (hdn_id != "Delete") {
                        var billid = $(this).find("#spanBillrowId").text();
                        currentRow.find("#chkCSA_" + billid).prop("checked", true);
                        $("#spanhiddenShip_" + rowIdx1).text("Y");
                        firstactiv = "Y";
                        ReadCustAddress();
                        return false;
                    }

                }
            });
        }
    });
};

function AmtFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#ValDigit") == false) {
        return false;
    }
    return true;
}
function numValueOnly(el, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;

    if (charCode > 31 && (charCode < 48 || charCode > 58)) {
        return false;
    }
    return true;
}

function onchangecustcityOld() {

    debugger;

    GetCustDSCntr();
    cityonchangeOld();
    var CustomerCity_Name = $("#cust_city").val();
    if (CustomerCity_Name != "0") {
        $("#vmCustCityname").attr("style", "display: none;");
        $("[aria-labelledby='select2-Cust_City-container']").attr("style", "border-color: #ced4da;");
        //$("#gst_num").css('border-color', "#ced4da");
        //$("#SpancustGST").attr("style", "display: none;");
        //document.getElementById("SpancustGST").innerHTML = "";

    }

    if (document.getElementById("SpancustAddr").innerHTML == $("#valueduplicate").text()) {
        document.getElementById("SpancustAddr").innerHTML = "";
        $("#customer_address").css('border-color', '#ced4da');
        $("#SpancustAddr").attr("style", "display: none;");
        document.getElementById("vmCustCityname").innerHTML = "";
        $("[aria-labelledby='select2-Cust_City-container']").css('border-color', '#ced4da');
        $("#vmCustCityname").attr("style", "display: none;");
        document.getElementById("vmCustPin").innerHTML = "";
        $("#Cust_Pin").css('border-color', '#ced4da');
        $("#vmCustPin").attr("style", "display: none;");
    }

}
function cityonchangeOld() {

    $("#Spancustdist").attr("style", "display: none;");
    $("#district_id").attr("style", "border-color: #ced4da;");
    ;
    $("#Spancuststate").attr("style", "display: none;");
    $("#state_id").attr("style", "border-color: #ced4da;");
    ;
    $("#Spancustcntry").attr("style", "display: none;");
    $("#country_id").attr("style", "border-color: #ced4da;");
    ;
    //$("#gst_num").css('border-color', "#ced4da");
    //$("#SpancustGST").attr("style", "display: none;");
    //document.getElementById("SpancustGST").innerHTML = "";
}
function GetCustDSCntr() {
    debugger;
    var ddlCity = $("#cust_city").val();

    $.ajax({
        type: "POST",
        url: "/CustomerDetails/GetCustDSCntr",/*Controller=CustlierSetup and Fuction=GetCustport*/
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async: true,
        data: { CustCity: ddlCity, },/*Registration pass value like model*/
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
                    $("#district_id").val(arr.Table[0].district_name);
                    $("#hdCustmerDistrictID").val(arr.Table[0].district_id);
                }
                if (arr.Table1.length > 0) {
                    $("#state_id").val(arr.Table1[0].state_name);
                    $("#hdCustmerStateID").val(arr.Table1[0].state_id);
                }
                if (arr.Table2.length > 0) {
                    $("#country_id").val(arr.Table2[0].country_name);
                    $("#hdCustmerCountryID").val(arr.Table2[0].country_id);
                }
                if (arr.Table3.length > 0) {
                    $("#gst_num").val(arr.Table3[0].state_code);
                    $("#hdCustmerStateID").val(arr.Table3[0].state_id);
                }

            }
        },
        error: function (Data) {
        }
    });
};
function SaveBtnClick() {
    var btn = $("#hdnSavebtn").val(); /**Adddd this NItesh 01-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#btn_save").prop("disabled", true); /*End*/
    }
    if (validateCustomerSetupInsertform() == false) {
        $("#hdnSavebtn").val("ABC");
        return false;
    }
    //if (CheckGstNumber() == false) {
    //    return false;
    //}
    debugger;
    if (billingandShippingvalidaction() == false) {
        $("#hdnSavebtn").val("ABC");
        return false;
    }
    debugger;
    if (Gstvalidaction() == false) {
        $("#hdnSavebtn").val("ABC");
        return false;
    }

    else {
        $("#act_status").attr("disabled", false);
        $("#Custcurr").attr("disabled", false);
        //$("#suppcurr").attr("disabled", false);
        if ($("#Export").is(":checked")) {
            Custcurr = "I";
            $("#Export").attr("disabled", false);
        }
        if ($("#Domestic").is(":checked")) {
            $("#Domestic").attr("disabled", false);
        }
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");

        /***Add By Shubham Maurya on 18-11-2025 Start***/
        var FinalLicenceDetail = [];
        FinalLicenceDetail = InsertFinalLicenceDetail();
        $("#hdnLincenceDetail").val(JSON.stringify(FinalLicenceDetail));
        /***Add By Shubham Maurya on 18-11-2025 End***/
        return true;
    }
}
function validateCustomerSetupInsertform() {

    var custname = $("#cust_name").val();
    var reason = $("#inact_reason").val();
    var holdreason = $("#hold_reason").val();
    var custcityID = $("#cust_city").val();

    var active = '';
    var hold = '';
    if ($("#act_status").is(":checked")) {
        active = "Y";
    }
    else {
        active = "N";
    }
    if ($("#cust_hold").is(":checked")) {
        hold = "Y";
    }
    else {
        hold = "N";
    }

    var ValidationFlag = true;

    if (custname == '') {
        document.getElementById("vmCustname").innerHTML = $("#valueReq").text();
        $("#cust_name").css("border-color", "red");
        ValidationFlag = false;
    }
    if ($("#GLAccountNm").val() == '') {
        document.getElementById("vmGLAccountNm").innerHTML = $("#valueReq").text();
        $("#GLAccountNm").css("border-color", "red");
        ValidationFlag = false;
    }
    if ($("#cust_catg").val() == '') {
        document.getElementById("vmCustCatg").innerHTML = $("#valueReq").text();
        $("#cust_catg").css("border-color", "red");
        ValidationFlag = false;
    }
    if ($("#cust_port").val() == '') {
        document.getElementById("vmCustPort").innerHTML = $("#valueReq").text();
        $("#cust_port").css("border-color", "red");
        ValidationFlag = false;
    }
    if ($("#GLAccGrp").val() == '0') {
        $('#SpanAccGrp').text($("#valueReq").text());
        $("#SpanAccGrp").css("display", "block");
        $("[aria-labelledby='select2-GLAccGrp-container']").css("border-color", "red");
        ValidationFlag = false;
    }
    //if ($("#cust_coa").val() == "0") {
    //    document.getElementById("vmCustCoa").innerHTML = $("#valueReq").text();
    //    $("#cust_coa").css("border-color", "red");
    //    ValidationFlag = false;
    //}
    if ($("#Custcurr").val() == '0') {
        document.getElementById("vmCustCurr").innerHTML = $("#valueReq").text();
        $("#Custcurr").css("border-color", "red");
        ValidationFlag = false;
    }
    var Pricepolicy = $('#cust_pr_pol').val()/*.trim()*/;
    if (Pricepolicy != "M") {
        if ($("#cust_pr_grp").val() == '') {
            document.getElementById("vmCustPrGrp").innerHTML = $("#valueReq").text();
            $("#cust_pr_grp").css("border-color", "red");
            ValidationFlag = false;
        }
    }
    if ($("#cust_region").val() == '') {
        document.getElementById("vmCustRegion").innerHTML = $("#valueReq").text();
        $("#cust_region").css("border-color", "red");
        ValidationFlag = false;
    }
    if (active === 'Y') {
        $("#inact_reason").attr("style", "border-color: #ced4da;");
        $("#vmInactReason").attr("style", "display: none;");
        $("#reasonrequired").attr("style", "display: none;");
    }
    if (active === 'N' && reason === '') {
        $("#inact_reason").attr("style", "border-color:#ff0000;", "required");
        document.getElementById("vmInactReason").innerHTML = $("#valueReq").text();
        $("#vmInactReason").css("display", "block");
        $("#reasonrequired").css("display", "inherit");
        ValidationFlag = false;
    }
    if (hold === 'N') {
        $("#hold_reason").attr("style", "border-color: #ced4da;");
        $("#vmholdreason").attr("style", "display: none;");
        $("#holdrequired").attr("style", "display: none;");
    }
    if (hold === 'Y' && holdreason === '') {
        $("#hold_reason").attr("style", "border-color:#ff0000;", "required");
        document.getElementById("vmholdreason").innerHTML = $("#valueReq").text();
        $("#vmholdreason").css("display", "block");
        $("#holdrequired").css("display", "inherit");
        ValidationFlag = false;
    }
    var validPanNumber = ValidationPanNumber();
    if (validPanNumber == false) {
        ValidationFlag = false;
    }
    var Hdn_GstApplicable = $("#Hdn_GstApplicable").text();
    var cont_num1 = $("#cont_num1").val().length
    if (cont_num1 != null && cont_num1 != "") {
        if ($("#Export").is(":checked") == false) {
            if (Hdn_GstApplicable == "Y") {
                if (cont_num1 < 10 || cont_num1 > 10) {
                    document.getElementById("vmcont_num1").innerHTML = ($("#span_InvalidFormat").text() + " (9999999999)");
                    $("#cont_num1").css('border-color', 'red');
                    $("#vmcont_num1").attr("style", "display: block;");
                    $("#cont_num1").css("display", "inherit");
                    return false;
                }
            }
            else {
                $('#vmcont_num1').text("");
                $("#vmcont_num1").css("display", "None");
                $("#cont_num1").css('border-color', "#ced4da");
            }
        }
        else {
            $('#vmcont_num1').text("");
            $("#vmcont_num1").css("display", "None");
            $("#cont_num1").css('border-color', "#ced4da");
        }
    }
    else {
        $('#vmcont_num1').text("");
        $("#vmcont_num1").css("display", "None");
        $("#cont_num1").css('border-color', "#ced4da");
    }

    if (ValidationFlag == true) {

        /*$("#hdnCustCityId").val(custcityID);*/
        ReadCustAddress();

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
function OnChangeCustName() {
    debugger;
    var CustName = $('#cust_name').val().trim();
    if (CustName != "") {
        document.getElementById("vmCustname").innerHTML = "";
        $("#cust_name").css("border-color", "#ced4da");
        $("#GLAccountNm").val(CustName);
        document.getElementById("vmGLAccountNm").innerHTML = "";
        $("#GLAccountNm").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmCustname").innerHTML = $("#valueReq").text();
        $("#cust_name").css("border-color", "red");
    }
}
function OnChangeGLAccountName() {
    document.getElementById("vmGLAccountNm").innerHTML = "";
    $("#GLAccountNm").css("border-color", "#ced4da");
}
function Gstvalidaction() {
    debugger;
    var custtype;
    var gstNo = "";
    var GstCat = $('#Gst_Cat').val();
    var GstApplicable = $('#hdnGstApplicable').val();
    if ($("#Export").is(":checked")) {
        custtype = "E";
    }
    if ($("#Domestic").is(":checked")) {
        custtype = "D";
    }
    var ErrorLogGSt = "N";
    var billingChecked = "";
    var shippingChecked = "";
    if (custtype != "E") {
        if (GstApplicable != "N") {
            if (GstCat != "UR" && GstCat != "CO" && GstCat != "SZ" && GstCat != "EX") {
                /*  $("#tblCustomerAddressDetail >tbody >tr").each(function (i, row)*/
                $("#tblCustomerAddressDetail TBODY TR").each(function () {
                    debugger;
                    var currentrow = $(this).closest('tr');
                    debugger;
                    gstNo = currentrow.find("#hdncustgst").text();
                    hdnID = currentrow.find("#hdnID").text();
                    var BIllrowId = currentrow.find("#spanBillrowId").text();
                    if (hdnID != "Delete") {
                        if (gstNo == "") {

                            billingChecked = currentrow.find("#chkCBA_" + BIllrowId).is(":checked");
                            shippingChecked = currentrow.find("#chkCSA_" + BIllrowId).is(":checked");


                            if (billingChecked) {
                                swal("", $("#GSTNumberismissinginAddress").text(), "warning");
                                ErrorLogGSt = "Y";
                                return false;

                            }
                            else if (!billingChecked && shippingChecked) {

                                //swal("", $("#GSTNumberismissinginAddress").text(), "warning");
                                return true;
                            }
                            else {
                               // swal("", $("#GSTNumberismissinginAddress").text(), "warning");
                                ErrorLogGSt = "N";
                               // return false;
                            }


                        }
                    }
                    if (hdnID == "Delete") {
                        gstNo = "Delete";
                    }
                });
                //if (gstNo == "") {
                //    swal("", $("#GSTNumberismissinginAddress").text(), "warning");
                //    return false;
                //}
                //else {
                //    return true;
                //}
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
    if (ErrorLogGSt == "Y") {
        return false;

    }
    else {
        return true;
    }
}
function billingandShippingvalidaction() {
    debugger;
    var billing = 'N';
    var shiping = 'N';
    $("#tblCustomerAddressDetail >tbody >tr").each(function (i, row) {
        debugger;
        var BIllrowId = $(this).find("#spanBillrowId").text();
        if ($("#chkCBA_" + BIllrowId).prop("checked") == true) {
            billing = 'Y';
        }
        if ($("#chkCSA_" + BIllrowId).prop("checked") == true) {
            shiping = 'Y'
        }
    });
    if (billing == 'N' || shiping == 'N') {

        swal("", $("#AtLeastOneShippingAndBillingAddressShouldBeSetAsDefault").text(), "warning");
        return false;
    }
    else {
        return true;
    }
}
function CheckGstNumber() {
    debugger;
    var validflag = true;
    //var ValidationFlag = true;
    if ($("#Domestic").is(":checked")) {
        $("#tblCustomerAddressDetail TBODY TR").each(function () {
            var currentrow = $(this).closest('tr');
            debugger;
            var custgst = currentrow.find("#hdncustgst").text();
            //validflag = false;
            if (custgst == "") {
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

function OnChangeCat() {
    debugger;
    var cat = $('#cust_catg').val().trim();
    if (cat != "0") {
        document.getElementById("vmCustCatg").innerHTML = "";
        $("#cust_catg").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmCustCatg").innerHTML = $("#valueReq").text();
        $("#cust_catg").css("border-color", "red");
    }
}
function OnChangePort() {
    debugger;
    var Port = $('#cust_port').val().trim();
    if (Port != "0") {
        document.getElementById("vmCustPort").innerHTML = "";
        $("#cust_port").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmCustPort").innerHTML = $("#valueReq").text();
        $("#cust_port").css("border-color", "red");
    }
}
function OnChangeCoa() {
    debugger;

    var Coa = $('#cust_coa').val().trim();
    if (Coa != "0") {
        document.getElementById("vmCustCoa").innerHTML = "";
        $("#cust_coa").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmCustCoa").innerHTML = $("#valueReq").text();
        $("#cust_coa").css("border-color", "red");
    }
    $("#hdnCustCoa").val(Coa);
}
function OnChangeSalesRegion() {
    debugger;
    var Region = $('#cust_region').val().trim();
    if (Region != "0") {
        document.getElementById("vmCustRegion").innerHTML = "";
        $("#cust_region").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmCustRegion").innerHTML = $("#valueReq").text();
        $("#cust_region").css("border-color", "red");
    }
}
function OnChangePriceGroup() {
    debugger;
    var PriceGroup = $('#cust_pr_grp').val().trim();
    if (PriceGroup != "0") {
        document.getElementById("vmCustPrGrp").innerText = "";
        $("#cust_pr_grp").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmCustPrGrp").innerHTML = $("#valueReq").text();
        $("#cust_pr_grp").css("border-color", "red");
    }
}
function OnChangeGSTCat() {
    debugger;
    var gst_cat = $("#Gst_Cat").val();
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (gst_cat === "UR" || gst_cat === "EX" || gst_cat === "CO" || gst_cat === "SZ") {
        // Hide GST fields
        $("#GSTNumrequired").hide();
        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#SpanCustGSTMidPrt").hide().text("");
        $("#GSTLastPrt").css('border-color', "#ced4da");
        $("#SpancustGST").hide().text("");
        $("#hdnGstFlagValid").val("NotValidation");
    }
    if (gst_cat == "UR") {
        $("#GSTNumrequired").attr("style", "display: none;");

        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#SpanCustGSTMidPrt").attr("style", "display: none;");
        document.getElementById("SpanCustGSTMidPrt").innerHTML = "";

        $("#GSTLastPrt").css('border-color', "#ced4da");
        $("#SpancustGST").attr("style", "display: none;");
        document.getElementById("SpancustGST").innerHTML = "";
        $("#GSTMidPrt").val("");
        $("#GSTLastPrt").val("");
        $("#GSTMidPrt").attr("disabled",true);
        $("#GSTLastPrt").attr("disabled", true);
        AddressAtbleNullGstNo();
    }
    else if (gst_cat == "EX" || gst_cat == "CO" || gst_cat == "SZ")
    {
        $("#GSTNumrequired").attr("style", "display: none;");

        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#SpanCustGSTMidPrt").attr("style", "display: none;");
        document.getElementById("SpanCustGSTMidPrt").innerHTML = "";

        $("#GSTLastPrt").css('border-color', "#ced4da");
        $("#SpancustGST").attr("style", "display: none;");
        document.getElementById("SpancustGST").innerHTML = "";
        $("#GSTMidPrt").attr("disabled", false);
        $("#GSTLastPrt").attr("disabled", false);
    }
    else {
        $("#GSTNumrequired").css("display", "inherit");
        
        $("#GSTLastPrt").attr("disabled", false);

        var billingChecked = $("#chkCustomerBillingAddress").is(":checked");
        var shippingChecked = $("#chkCustomerShippingAddress").is(":checked");
        var Domestic = $("#Domestic").is(":checked");
        if (Domestic) {
            if (billingChecked) {                              
                if (GstApplicable == "Y") {
                    $("#GSTMidPrt").attr("disabled", true);
                    $("#GSTNumrequired").css("display", "inherit");
                    $("#hdnGstFlagValid").val("Validation");
                    const panInput = $("#pan_num").val().toUpperCase();
                    $("#pan_num").val(panInput); // force uppercase in input field
                    $("#GSTMidPrt").val(panInput);
                }
                else {
                    $("#GSTMidPrt").attr("disabled", false);
                    $("#GSTNumrequired").attr("style", "display: none;");
                    $("#hdnGstFlagValid").val("NotValidation");
                }                               
            }
            else if (!billingChecked && shippingChecked) {
                // Hide GST fields
                $("#GSTNumrequired").hide();
                $("#GSTMidPrt").css('border-color', "#ced4da");
                $("#SpanCustGSTMidPrt").hide().text("");

                $("#GSTLastPrt").css('border-color', "#ced4da");
                $("#SpancustGST").hide().text("");
                $("#hdnGstFlagValid").val("NotValidation");
            }
            else {
               
                $("#hdnGstFlagValid").val("NotValidation");
                var GstApplicable = $("#Hdn_GstApplicable").text();
                $("#GSTNumrequired").hide();
            }
        }
        else {
            $("#hdnGstFlagValid").val("NotValidation");
            $("#GSTNumrequired").hide();
        }

    }
}
function AddressAtbleNullGstNo() {
    $("#tblCustomerAddressDetail tbody tr").each(function () {
        var curr = $(this).closest('tr');
          curr.find("#hdncustgst").text("");

    })
}
function OnChangePricePolicy() {
    var ValidationFlag = true;
    debugger;
    var Pricepolicy = $('#cust_pr_pol').val()/*.trim()*/;
    if (Pricepolicy != "P") {
        $("#cust_pr_grp").prop('disabled', true);
        $("#cust_pr_grp").val("");
        document.getElementById("vmCustPrGrp").innerText = "";
        $("#cust_pr_grp").css("border-color", "#ced4da");
        $("#PrisegroupRequird").attr("style", "display: none;");
    }
    else {
        $("#cust_pr_grp").prop('disabled', false);
        if ($("#cust_pr_grp").val() == '') {
            $("#PrisegroupRequird").css("display", "inherit");
        }
        else {
            document.getElementById("vmCustPrGrp").innerText = "";
            $("#cust_pr_grp").css("border-color", "#ced4da");
        }

    }
}
function OnChangeReason() {
    debugger;
    document.getElementById("vmInactReason").innerHTML = "";
    $("#inact_reason").css("border-color", "#ced4da");
    $("#reasonrequired").attr("style", "display: none;");
    $("#reasonrequired").css("display", "inherit");
}
function OnActiveStatusChange() {
    debugger;
    if ($("#act_status").is(":checked")) {

        $("#inact_reason").val("");
        $("#inact_reason").prop("readonly", true);
        document.getElementById("vmInactReason").innerHTML = "";
        $("#inact_reason").css("border-color", "#ced4da");
        $("#reasonrequired").attr("style", "display: none;");
        $("#cust_hold").prop("disabled", false);
        $("#cust_hold").prop("checked", false);
        OnHoldStatusChange();

    }
    else {
        $("#inact_reason").prop("readonly", false);
        $("#inact_reason").attr("style", "border-color:#ff0000;", "required");
        document.getElementById("vmInactReason").innerHTML = $("#valueReq").text();
        $("#vmInactReason").css("display", "block");
        $("#reasonrequired").css("display", "inherit");
        $("#cust_hold").prop("disabled", true);
        $("#cust_hold").prop("checked", false);
        OnHoldStatusChange();
    }
}
function OnChangeHoldReason() {
    debugger;
    document.getElementById("vmholdreason").innerHTML = "";
    $("#hold_reason").css("border-color", "#ced4da");
    if ($("#cust_hold").is(":checked")) {

        $("#holdrequired").css("display", "inherit");
    }
    else {
        $("#holdrequired").attr("style", "display: none;");
    }
}
function OnHoldStatusChange() {
    debugger;
    if ($("#cust_hold").is(":checked")) {
        $("#hold_reason").attr("style", "border-color:#ff0000;", "required");
        document.getElementById("vmholdreason").innerHTML = $("#valueReq").text();
        $("#vmholdreason").css("display", "block");
        $("#holdrequired").css("display", "inherit");
        $("#hold_reason").prop("readonly", false);

    }
    else {
        $("#hold_reason").prop("readonly", true);
        $("#hold_reason").val("");
        document.getElementById("vmholdreason").innerHTML = "";
        $("#hold_reason").css("border-color", "#ced4da");
        $("#holdrequired").attr("style", "display: none;");
    }
}
function OnChangeCurr() {
    debugger;
    var curr = $('#Custcurr').val().trim();
    if (curr != "0") {
        document.getElementById("vmCustCurr").innerHTML = "";
        $("#Custcurr").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmCustCurr").innerHTML = $("#valueReq").text();
        $("#Custcurr").css("border-color", "red");
    }
}
function GetCustcurr() {
    debugger;
    var Custcurr;
    if ($("#Export").is(":checked")) {
        $('#cont_num1').removeClass('input-validation-error');
        $('#cont_num1').removeAttr('aria-invalid');
        $('#cont_num1').removeAttr('maxlength');
        $('#cont_num1').attr('maxlength', '50');
        $("#custinvalid").text("");
    }
    if ($("#Domestic").is(":checked")) {
        $("#custinvalid").text("*");
        $('#cont_num1').addClass('input-validation-error');
        $('#cont_num1').attr('aria-invalid');
        $('#cont_num1').attr('maxlength');
        $('#cont_num1').attr('maxlength', '10');

    }
    if ($("#Export").is(":checked")) {
        Custcurr = "I";
        //$('#suppcurr').empty();
        $('#Custcurr').empty();
    }
    if ($("#Domestic").is(":checked")) {
        Custcurr = "D";
        //$('#suppcurr').empty();
        $('#Custcurr').empty();
    }
    if (Custcurr == "I") {
        //$("#Gst_Cat").prop('disabled', true);
        //$("#RequiredGSTCategory").attr("style", "display: none;");
        $("#GSTNumrequired").attr("style", "display: none;");
        $("#DefaultCurrency").css("display", "none");
        $("#DefaultCurrency1").removeClass("col-md-6");
    }
    else {
        //$("#Gst_Cat").prop('disabled', false);
        $("#GSTNumrequired").css("display", "inherit");
        //$("#RequiredGSTCategory").css("display", "inherit");
        $("#DefaultCurrency").css("display", "block");
        $("#DefaultCurrency1").addClass("col-md-6");
    }
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/CustomerDetails/GetCurronSuppType",
        data: { Supptype: Custcurr },
        success: function (data) {
            debugger;
            var arr = [];
            arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
            if (arr.length > 0) {
                //$('#Proscurr').val(arr[0].curr_id);
                for (var i = 0; i < arr.length; i++) {
                    //$('#suppcurr').append(`<option value="${arr[i].curr_id}">${arr[i].curr_name}</option>`);
                    $('#Custcurr').append(`<option value="${arr[i].curr_id}">${arr[i].curr_name}</option>`);
                }
            }
            //$('#CurrencyDiv').html(data);
        },
    });
    /*Add Code by Hina on 10-01-2024 to get country on Custtype chng*/
    GetCountryOnChngCustType(Custcurr);
    ValidationForPanNoSetAttr();
}
function onchangeCustAddr() {
    debugger;
    var customer_address = $("#customer_address").val();
    if (customer_address != "") {
        document.getElementById("SpancustAddr").innerHTML = "";
        $("#customer_address").css('border-color', '#ced4da');
        $("#SpancustAddr").attr("style", "display: none;");
        /*Add by Hina on 24-01-2024 to validate PIN only for in case of domestic*/
        document.getElementById("vmCustCityname").innerHTML = "";
        $("[aria-labelledby='select2-Cust_City-container']").css('border-color', '#ced4da');
        $("#vmCustCityname").attr("style", "display: none;"); 
        //document.getElementById("vmCustPin").innerHTML = "";
        //$("#Cust_Pin").css('border-color', '#ced4da');
        //$("#vmCustPin").attr("style", "display: none;");
        //document.getElementById("vmCustCityname").innerHTML = "";
        //$("[aria-labelledby='select2-cust_city-container']").css('border-color', '#ced4da');
        //$("#vmCustCityname").attr("style", "display: none;");
    }
    else {
        document.getElementById("SpancustAddr").innerHTML = $("#valueReq").text();
        $("#customer_address").css('border-color', 'red');
        $("#SpancustAddr").attr("style", "display: block;");

    }
    /*Commented by Hina on 24-01-2024 to validate PIN only for in case of domestic*/
    //if (document.getElementById("vmCustPin").innerHTML == $("#valueduplicate").text()) {
    //    document.getElementById("vmCustPin").innerHTML = "";
    //    $("#Cust_Pin").css('border-color', '#ced4da');
    //    $("#vmCustPin").attr("style", "display: none;");
    //    document.getElementById("vmCustCityname").innerHTML = "";
    //    $("[aria-labelledby='select2-Cust_City-container']").css('border-color', '#ced4da');
    //    $("#vmCustCityname").attr("style", "display: none;");
    //}
}
function onchangeCustPin() {
    var Custtyp;
    if ($("#Export").is(":checked")) {
        Custtyp = "I";
    }
    if ($("#Domestic").is(":checked")) {
        Custtyp = "D";
    }
    if (Custtyp == "D") {
        var customer_Pin = $("#Cust_Pin").val();
        if (customer_Pin == "")
        {
            document.getElementById("vmCustPin").innerHTML = $("#valueReq").text();
            $("#Cust_Pin").css('border-color', 'red');
            $("#vmCustPin").attr("style", "display: block;");
        }
        else {

            document.getElementById("vmCustPin").innerHTML = "";
            $("#Cust_Pin").css('border-color', '#ced4da');
            $("#vmCustPin").attr("style", "display: none;");
        }
        if (document.getElementById("SpancustAddr").innerHTML == $("#valueduplicate").text()) {
            document.getElementById("SpancustAddr").innerHTML = "";
            $("#customer_address").css('border-color', '#ced4da');
            $("#SpancustAddr").attr("style", "display: none;");
            document.getElementById("vmCustCityname").innerHTML = "";
            $("[aria-labelledby='select2-Cust_City-container']").css('border-color', '#ced4da');
            $("#vmCustCityname").attr("style", "display: none;");
        }
    }
}
function onchangeGST() {
    debugger;
    //var custtype;
    //if ($("#Export").is(":checked")) {
    //    custtype = "I";
    //}
    //if ($("#Domestic").is(":checked")) {
    //    custtype = "D";
    //}
    var CustomerGST = $("#gst_num").val();
    var GSTNoMidPrt = $("#GSTMidPrt").val().trim();
    var GSTNoLastPrt = $("#GSTLastPrt").val().trim();

    if (GSTNoMidPrt != "" || GSTNoMidPrt == "") {
        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#SpancustGST").attr("style", "display: none;");
        document.getElementById("SpanCustGSTMidPrt").innerHTML = "";
    }
    if (GSTNoLastPrt != "" || GSTNoLastPrt == "") {
        document.getElementById("SpancustGST").innerHTML = "";
        $("#GSTLastPrt").css('border-color', "#ced4da");
        $("#SpancustGST").attr("style", "display: none;");
    }
    //if (custtype == "D") {
    //    if (GSTNoMidPrt == "") {
    //         document.getElementById("SpancustGST").innerHTML = $("#valueReq").text();
    //        //$("#gst_num").css('border-color', 'red');
    //        $("#GSTMidPrt").css('border-color', 'red');
    //        $("#SpancustGST").attr("style", "display: block;");
    //         $("#GSTNumrequired").css("display", "inherit");

    //     }
    //     else {
    //         document.getElementById("SpancustGST").innerHTML = "";
    //        //$("#gst_num").css('border-color', '#ced4da');
    //        $("#GSTMidPrt").css('border-color', "#ced4da");
    //        $("#SpancustGST").attr("style", "display: none;");
    //    }
    //    if (GSTNoLastPrt == "") {
    //        document.getElementById("SpancustGST").innerHTML = $("#valueReq").text();
    //        $("#GSTLastPrt").css('border-color', 'red');
    //        $("#SpancustGST").attr("style", "display: block;");
    //        $("#GSTNumrequired").css("display", "inherit");
    //    }
    //    else {
    //        document.getElementById("SpancustGST").innerHTML = "";
    //        //$("#gst_num").css('border-color', '#ced4da');
    //        $("#GSTLastPrt").css('border-color', "#ced4da");
    //        $("#SpancustGST").attr("style", "display: none;");
    //    }

    // }
    // else {

    //      document.getElementById("SpancustGST").innerHTML = "";
    //    //$("#gst_num").css('border-color', '#ced4da');
    //    $("#GSTMidPrt").css('border-color', "#ced4da");
    //    $("#GSTLastPrt").css('border-color', "#ced4da");
    //     $("#SpancustGST").attr("style", "display: none;");
    //     $("#GSTNumrequired").css("style", "display:none");
    //  }
}
function validatetbldt() {
    debugger;
    var customer_address = $("#customer_address").val();
    //var customer_Pin = $("#Pin").val();
    //var CustomerCity_Name = $("#cust_city").val();
    /*start add code by  Hina on 10-01-2023  for ONchangeCountry Code*/
    var CustContry_Name = $("#Cust_Country").val();
    var CustState_Name = $("#Cust_State").val();
    var CustDistrict_Name = $("#Cust_District").val();
    var CustCity_Name = $("#Cust_City").val();
    var Cust_Pin = $("#Cust_Pin").val();
    /*End add code by  Hina on 10-01-2023  for ONchangeCountry Code*/
    var CustomerGST = $("#gst_num").val();
    var GSTNoMidPrt = $("#GSTMidPrt").val().trim();
    var GSTNoLastPrt = $("#GSTLastPrt").val().trim();
    var gst_cat = $("#Gst_Cat").val();
    var custtype;
    var GstApplicable = $('#hdnGstApplicable').val();
    if ($("#Export").is(":checked")) {
        custtype = "E";
    }
    if ($("#Domestic").is(":checked")) {
        custtype = "D";
    }
    debugger;
    var valFalg = true;
    if (customer_address == "") {
        document.getElementById("SpancustAddr").innerHTML = $("#valueReq").text();
        $("#customer_address").css('border-color', 'red');
        $("#SpancustAddr").attr("style", "display: block;");
        valFalg = false;
    }
    /*start add code by  Hina on 09-01-2023  for ONchangeCountry Code**/
    if (CustContry_Name == "0") {
        document.getElementById("vmCustCountryname").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-Cust_Country-container']").css('border-color', 'red');
        $("#vmCustCountryname").attr("style", "display: block;");
        valFalg = false;
    }
    if (CustState_Name == "0") {
        document.getElementById("vmCustStatename").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-Cust_State-container']").css('border-color', 'red');
        $("#Cust_State").css('border-color', 'red');
        $("#vmCustStatename").attr("style", "display: block;");
        valFalg = false;
    }
    if (CustDistrict_Name == "0") {
        document.getElementById("vmCustDistname").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-Cust_District-container']").css('border-color', 'red');
        $("#Cust_District").css('border-color', 'red');
        $("#vmCustDistname").attr("style", "display: block;");
        valFalg = false;
    }
    if (CustCity_Name == "0") {
        document.getElementById("vmCustCityname").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-Cust_City-container']").css('border-color', 'red');
        $("#Cust_City").css('border-color', 'red');
        $("#vmCustCityname").attr("style", "display: block;");
        valFalg = false;
    }

    if (custtype == "D") {
        if (Cust_Pin == "") {
            document.getElementById("vmCustPin").innerHTML = $("#valueReq").text();
            $("#Cust_Pin").css('border-color', 'red');
            $("#vmCustPin").attr("style", "display: block;");
            valFalg = false;
        }
    }
    /*End add code by  Hina on 09-01-2023 for ONchangeCountry Code*/
    //if (customer_Pin == "") {
    //    document.getElementById("vmCustPin").innerHTML = $("#valueReq").text();
    //    $("#Pin").css('border-color', 'red');
    //    $("#vmCustPin").attr("style", "display: block;");
    //    valFalg = false;
    //}
    //if (CustomerCity_Name == "0") {
    //    document.getElementById("vmCustCityname").innerHTML = $("#valueReq").text();
    //    $("[aria-labelledby='select2-cust_city-container']").css('border-color', 'red');
    //    $("#vmCustCityname").attr("style", "display: block;");
    //    valFalg = false;
    //}
    debugger;
    onchangechkCustsuppBilling();
    var shipBillValidation = $("#hdnGstFlagValid").val();
    if (custtype != "E" && shipBillValidation != "NotValidation") {
        if (GstApplicable != "N") {
            if (gst_cat != "UR" && gst_cat != "EX" && gst_cat != "CO" && gst_cat != "SZ") {

                if (GSTNoMidPrt == "") {
                    $("#GSTMidPrt").css('border-color', 'red');
                    $("#SpanCustGSTMidPrt").attr("style", "display: block;");
                    document.getElementById("SpanCustGSTMidPrt").innerHTML = $("#valueReq").text();
                    valFalg = false;
                }
                else if (GSTNoMidPrt != "" && GSTNoMidPrt.length != "10") {
                    $("#GSTMidPrt").css('border-color', 'red');
                    $("#SpanCustGSTMidPrt").attr("style", "display: block;");
                    document.getElementById("SpanCustGSTMidPrt").innerHTML = $("#InvalidGSTNumber").text();
                    valFalg = false;
                }
            }
            if (GSTNoMidPrt != "" || GSTNoLastPrt != "") {
                if (GSTNoMidPrt == "") {
                    $("#GSTMidPrt").css('border-color', 'red');
                    $("#SpanCustGSTMidPrt").attr("style", "display: block;");
                    document.getElementById("SpanCustGSTMidPrt").innerHTML = $("#valueReq").text();
                    valFalg = false;
                }
                else if (GSTNoMidPrt != "" && GSTNoMidPrt.length != "10") {
                    $("#GSTMidPrt").css('border-color', 'red');
                    $("#SpanCustGSTMidPrt").attr("style", "display: block;");
                    document.getElementById("SpanCustGSTMidPrt").innerHTML = $("#InvalidGSTNumber").text();
                    valFalg = false;
                }
                else if (GSTNoLastPrt == "") {
                    $("#GSTLastPrt").css('border-color', 'red');
                    $("#SpancustGST").attr("style", "display: block;");
                    document.getElementById("SpancustGST").innerHTML = $("#valueReq").text();
                    valFalg = false;
                }
                else if (GSTNoLastPrt != "" && GSTNoLastPrt.length != "3") {
                    $("#GSTLastPrt").css('border-color', 'red');
                    $("#SpancustGST").attr("style", "display: block;");
                    document.getElementById("SpancustGST").innerHTML = $("#InvalidGSTNumber").text();
                    valFalg = false;
                }
            }
            else {
                if (GSTNoMidPrt != "" && GSTNoLastPrt == "") {
                    $("#GSTLastPrt").css('border-color', 'red');
                    $("#SpancustGST").attr("style", "display: block;");
                    document.getElementById("SpancustGST").innerHTML = $("#InvalidGSTNumber").text();
                    valFalg = false;
                }
                else {

                }
                if (GSTNoMidPrt == "" && GSTNoLastPrt != "") {
                    $("#GSTMidPrt").css('border-color', 'red');
                    $("#SpancustGST").attr("style", "display: block;");
                    document.getElementById("SpancustGST").innerHTML = $("#InvalidGSTNumber").text();
                    valFalg = false;
                }
                else {

                }

            }
        }
        else {
            if (GSTNoMidPrt == "" && GSTNoLastPrt != "") {
                $("#GSTMidPrt").css('border-color', 'red');
                $("#SpanCustGSTMidPrt").attr("style", "display: block;");
                document.getElementById("SpanCustGSTMidPrt").innerHTML = $("#valueReq").text();
                valFalg = false;
            }
            else if (GSTNoMidPrt != "" && GSTNoMidPrt.length != "10") {
                $("#GSTMidPrt").css('border-color', 'red');
                $("#SpanCustGSTMidPrt").attr("style", "display: block;");
                document.getElementById("SpanCustGSTMidPrt").innerHTML = $("#InvalidGSTNumber").text();
                valFalg = false;
            }
            else if (GSTNoMidPrt != "" && GSTNoLastPrt == "") {
                $("#GSTLastPrt").css('border-color', 'red');
                $("#SpancustGST").attr("style", "display: block;");
                document.getElementById("SpancustGST").innerHTML = $("#valueReq").text();
                valFalg = false;
            }
            else if (GSTNoLastPrt != "" && GSTNoLastPrt.length != "3") {
                $("#GSTLastPrt").css('border-color', 'red');
                $("#SpancustGST").attr("style", "display: block;");
                document.getElementById("SpancustGST").innerHTML = $("#InvalidGSTNumber").text();
                valFalg = false;
            }
            if (GSTNoMidPrt != "" || GSTNoLastPrt) {
                var CustGSTNum = CustomerGST + GSTNoMidPrt + GSTNoLastPrt
                $("#GSTNumber").val(CustGSTNum);
            }
            else {
                if (GSTNoMidPrt == "" || GSTNoLastPrt == "") {
                    var CustGSTNum = GSTNoMidPrt + GSTNoLastPrt
                    $("#GSTNumber").val(CustGSTNum);
                }
            }
        }
    }
    else {
        if (GSTNoMidPrt == "" && GSTNoLastPrt != "") {
            $("#GSTMidPrt").css('border-color', 'red');
            $("#SpanCustGSTMidPrt").attr("style", "display: block;");
            document.getElementById("SpanCustGSTMidPrt").innerHTML = $("#valueReq").text();
            valFalg = false;
        }
        else if (GSTNoMidPrt != "" && GSTNoMidPrt.length != "10") {
            $("#GSTMidPrt").css('border-color', 'red');
            $("#SpanCustGSTMidPrt").attr("style", "display: block;");
            document.getElementById("SpanCustGSTMidPrt").innerHTML = $("#InvalidGSTNumber").text();
            valFalg = false;
        }
        else if (GSTNoMidPrt != "" && GSTNoLastPrt == "") {
            $("#GSTLastPrt").css('border-color', 'red');
            $("#SpancustGST").attr("style", "display: block;");
            document.getElementById("SpancustGST").innerHTML = $("#valueReq").text();
            valFalg = false;
        }
        else if (GSTNoLastPrt != "" && GSTNoLastPrt.length != "3") {
            $("#GSTLastPrt").css('border-color', 'red');
            $("#SpancustGST").attr("style", "display: block;");
            document.getElementById("SpancustGST").innerHTML = $("#InvalidGSTNumber").text();
            valFalg = false;
        }
        if (GSTNoMidPrt != "" || GSTNoLastPrt) {
            var CustGSTNum = CustomerGST + GSTNoMidPrt + GSTNoLastPrt
            $("#GSTNumber").val(CustGSTNum);
        }
        else {
            if (GSTNoMidPrt == "" || GSTNoLastPrt == "") {
                var CustGSTNum = GSTNoMidPrt + GSTNoLastPrt
                $("#GSTNumber").val(CustGSTNum);
            }
        }
    }
    if (valFalg == true) {

    }
    else {
        return false;
    }
}
function AddAddressdetail() {
    var rowIdx = "";
    debugger;
    var custtype;
    if ($("#Export").is(":checked")) {
        custtype = "I";
    }
    if ($("#Domestic").is(":checked")) {
        custtype = "D";
    }
    var rowId = $("#tblCustomerAddressDetail TBODY TR:last").find("#spanBillrowId").text();
    if (rowId == "" || rowId == null) {
        rowIdx = 1;
    }
    else {
        rowIdx = parseInt(rowId) + 1;
    }

    var customer_address = $("#customer_address").val();
    var customer_Pin = $("#Cust_Pin").val();
    var CustomerCity_Name = $("#Cust_City").val();
    debugger;
    var CustomerGST = $("#gst_num").val().trim();
    var CustGSTNoMidPrt = $("#GSTMidPrt").val().trim();
    var CustGSTNoLastPrt = $("#GSTLastPrt").val().trim();
    if (CustGSTNoMidPrt != "" || CustGSTNoLastPrt != "") {
        var CustGSTNum = CustomerGST + CustGSTNoMidPrt + CustGSTNoLastPrt
        $("#hdCustmerGst").val(CustGSTNum);
    }
    else {
        if (CustGSTNoMidPrt == "" || CustGSTNoLastPrt == "") {
            var CustGSTNum = CustGSTNoMidPrt + CustGSTNoLastPrt;
            $("#hdCustmerGst").val(CustGSTNum);
        }
    }
    var CustomerBillingAddress = "";
    var CustomerShippingAddress = "";
    var district = $("#district_id").val();
    var state_id = $("#state_id").val();
    var country_id = $("#country_id").val();
    var gst_num = $("#gst_num").val();
    debugger;
    if (validatetbldt() == false) {
        return false;
    }
    var AddAddress =  addCustomerAddress();
    if (AddAddress == false) {
        return false;
    }
    else {
        return true;

    }


}
async function addCustomerAddress() {
    var CustomerBillingAddress = "";
    var CustomerShippingAddress = "";
    var district = $("#district_id").val();
    var state_id = $("#state_id").val();
    var country_id = $("#country_id").val();
    var gst_num = $("#gst_num").val();
    const isGstValid = await CheckDuplicateGstNoAsync();
    if (!isGstValid) {
        return false;
    }
    else {
        // Determine billing/shipping checkbox values
        const CustomerBillingAddress = $("#chkCustomerBillingAddress").is(":checked") ? "Y" : "N";
        const CustomerShippingAddress = $("#chkCustomerShippingAddress").is(":checked") ? "Y" : "N";

        let CustShippingAddresstd = '';
        let CustBillingAddresstd = '';

        const shippingSwitch = (id, isChecked) => `
        <td>
            <div class="custom-control custom-switch" style="margin: -5px 0px 0px 40px;">
                <input type="checkbox" class="custom-control-input col-md-3 col-sm-12 margin-switch"
                       id="chkCSA_${id}" ${isChecked ? 'checked' : ''} onclick="defshipchk(event)" onchange="defshipchk(event)">
                <label class="custom-control-label col-md-9 col-sm-12" for="chkCSA_${id}" style="padding: 3px 0px;"></label>
            </div>
        </td>
        <td hidden><span id="spanhiddenShip_${id}">${isChecked ? 'Y' : 'N'}</span></td>
        <td hidden><span id="spanShiprowId">${id}</span></td>`;

        const billingSwitch = (id, isChecked) => `
        <td>
            <div class="custom-control custom-switch" style="margin: -5px 0px 0px 40px;">
                <input type="checkbox" class="custom-control-input col-md-3 col-sm-12 margin-switch"
                       id="chkCBA_${id}" ${isChecked ? 'checked' : ''} onclick="defbillchk(event)" onchange="defbillchk(event)">
                <label class="custom-control-label col-md-9 col-sm-12" for="chkCBA_${id}" style="padding: 3px 0px;"></label>
            </div>
        </td>
        <td hidden><span id="spanhiddenBill_${id}">${isChecked ? 'Y' : 'N'}</span></td>
        <td hidden><span id="spanBillrowId">${id}</span></td>`;

        let rowId = 0;
        let cheakflag = "N";
        let cheackdefalt = "Q";

        $("#tblCustomerAddressDetail TBODY TR").each(function () {
            const billRowId = $(this).find("#spanBillrowId").text();
            if ($(this).find(`#chkCBA_${billRowId}`).is(":checked") || $(this).find(`#chkCSA_${billRowId}`).is(":checked")) {
                cheackdefalt = "P";
            }
        });

        if (cheackdefalt === "Q") {
            $("#tblCustomerAddressDetail TBODY TR").each(function () {
                const billRowId = $(this).find("#spanBillrowId").text();

                if ($(this).find(`#chkCBA_${billRowId}`).is(":checked")) {
                    $(this).find(`#chkCBA_${billRowId}`).prop("checked", false);
                    $(this).find(`#spanhiddenBill_${billRowId}`).text("Y");
                    cheakflag = "Y";
                }

                if ($(this).find(`#chkCSA_${billRowId}`).is(":checked")) {
                    $(this).find(`#chkCSA_${billRowId}`).prop("checked", false);
                    $(this).find(`#spanhiddenShip_${billRowId}`).text("Y");
                    cheakflag = "Y";
                }
            });

            if (cheakflag === "Y") {
                let firstRow = true;
                $("#tblCustomerAddressDetail TBODY TR").each(function () {
                    const billRowId = $(this).find("#spanBillrowId").text();
                    if (firstRow) {
                        $(this).find(`#chkCBA_${billRowId}`).prop("checked", true);
                        $(this).find(`#spanhiddenBill_${billRowId}`).text("Y");
                        $(this).find(`#chkCSA_${billRowId}`).prop("checked", true);
                        $(this).find(`#spanhiddenShip_${billRowId}`).text("Y");
                        firstRow = false;
                    }
                });
            }
        }

        const customer_address = $("#customer_address").val();
        const CustomerCity_Name = $("#Cust_City").val();

        if (customer_address !== "" && CustomerCity_Name !== "0") {
            if (CheckDuplicacy() === false) return false;

            let maxRowId = 0;
            $("#tblCustomerAddressDetail TBODY TR").each(function () {
                const currentRowId = parseInt($(this).find("#spanShiprowId").text(), 10);
                if (currentRowId > maxRowId) maxRowId = currentRowId;
            });
            rowId = maxRowId + 1;

            if (CustomerBillingAddress === 'Y') {
                $("#tblCustomerAddressDetail TBODY TR").each(function () {
                    const rowId = $(this).find("#spanBillrowId").text();
                    $(this).find(`#chkCBA_${rowId}`).prop("checked", false);
                    $(this).find(`#spanhiddenBill_${rowId}`).text("N");
                });
            }

            if (CustomerShippingAddress === 'Y') {
                $("#tblCustomerAddressDetail TBODY TR").each(function () {
                    const rowId = $(this).find("#spanBillrowId").text();
                    $(this).find(`#chkCSA_${rowId}`).prop("checked", false);
                    $(this).find(`#spanhiddenShip_${rowId}`).text("N");
                });
            }

            const countryname = $("#Cust_Country option:selected").text();
            const countryId = $("#Cust_Country").val();

            // Prepare address row with billing and shipping status
            $("#AddressBody").append(`
            <tr id="R${rowId}">
                <td class="red center">
                    <i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i>
                </td>
                <td class="center edit_icon">
                    <i class="fa fa-edit" aria-hidden="true" id="editBtnIcon" title="${$("#Edit").text()}"></i>
                </td>
                <td id="hdncustadd">${customer_address}</td>
                <td id="hdncntryname">${countryname}</td>
                <td hidden id="hdncustcntry">${countryId}</td>
                <td id="hdnstate">${$("#Cust_State option:selected").text()}</td>
                <td hidden id="hdncuststate">${$("#Cust_State").val()}</td>
                <td id="hdndistname">${$("#Cust_District option:selected").text()}</td>
                <td hidden id="hdncustdist">${$("#Cust_District").val()}</td>
                <td id="hdncityname">${$("#Cust_City option:selected").text()}</td>
                <td hidden id="hdncustcity">${$("#Cust_City").val()}</td>
                <td id="hdncustpin">${$("#Cust_Pin").val()}</td>
                <td id="hdncustgst">${$("#hdCustmerGst").val()}</td>
                <td id="tblAddrcont_per">${$("#Addr_cont_per").val()}</td>
                <td id="tblAddrcont_no">${$("#Addr_cont_no").val()}</td>
                ${billingSwitch(rowId, CustomerBillingAddress === 'Y')}
                ${shippingSwitch(rowId, CustomerShippingAddress === 'Y')}
                <td hidden id="address_id">${rowId}</td>
                <td hidden id="hdnID">Save</td>
            </tr>`);

            // Reset form and validation highlights
            var gstCat = $("#Gst_Cat").val();
            if (gstCat == "RR") {
                $("#customer_address, #Cust_Pin, #gst_num, #GSTLastPrt,#Addr_cont_per,#Addr_cont_no").val("");
            }
            else {
                $("#customer_address, #Cust_Pin, #gst_num, #GSTMidPrt, #GSTLastPrt,#Addr_cont_per,#Addr_cont_no").val("");
            }
            $("#SpancustAddr, #vmCustPin, #vmCustCityname").hide().text("");
            $("#customer_address, #Cust_Pin, #GSTMidPrt, #GSTLastPrt").css("border-color", "#ced4da");
            $("[aria-labelledby='select2-Cust_City-container']").css('border-color', '#ced4da');

            var custtype;
            if ($("#Export").is(":checked")) {
                custtype = "I";
            }
            if ($("#Domestic").is(":checked")) {
                custtype = "D";
            }


            // Reset dropdowns 
            $("#HdnCustState, #HdnCustDistrict, #HdnCustCity").val(0);
            if (custtype === "D") {
                $("#Cust_Country").val(countryId);
            } else {
                $("#Cust_Country").val("0").change();
            }
            $("#Cust_State, #Cust_District, #Cust_City").val("0").trigger("change");

            // Reset checkboxes
            $("#chkCustomerBillingAddress, #chkCustomerShippingAddress").prop("checked", true);
            var gst_cat = $("#Gst_Cat").val();
            var GstApplicable = $("#Hdn_GstApplicable").text();
            if (GstApplicable == "Y") {
                if (gst_cat === "UR" || gst_cat === "EX" || gst_cat === "CO" || gst_cat === "SZ") {
                    $("#GSTNumrequired").hide();
                }
                else {
                    $("#GSTNumrequired").css("display", "inherit");
                }
            }
            else {
                $("#GSTNumrequired").hide();
            }
           
        }

    }

 
}


async function CheckDuplicateGstNoAsync() {
    var GSTNum = "";
    debugger;
    var CustomerGST = $("#gst_num").val().trim();
    var CustGSTNoMidPrt = $("#GSTMidPrt").val().trim();
    var CustGSTNoLastPrt = $("#GSTLastPrt").val().trim();
    if (CustGSTNoMidPrt != "" || CustGSTNoLastPrt != "") {
        GSTNum = CustomerGST + CustGSTNoMidPrt + CustGSTNoLastPrt
       
    }
    else {
        if (CustGSTNoMidPrt == "" || CustGSTNoLastPrt == "") {
             GSTNum = CustGSTNoMidPrt + CustGSTNoLastPrt;
       
        }
    }
    if (GSTNum != "") {
        const Cust_Id = $("#cust_ID").val();
        const CustGst = GSTNum;

        const response = await $.ajax({
            type: "POST",
            url: "/BusinessLayer/CustomerDetails/CheckDependcyGstno",
            data: { Cust_Id: Cust_Id, CustGst: CustGst }
        });

        const arr = JSON.parse(response);
        if (arr.length > 0 && arr[0].Dependcy === "Use") {
            $("#SpanCustGSTMidPrt").text($("#valueduplicate").text()).show();
            $("#GSTMidPrt, #GSTLastPrt").css("border-color", "red");
            $("#GSTMidPrt, #GSTLastPrt").css("border-color", "red");
            return false;
        } else {
            $("#SpanCustGSTMidPrt").hide().text("");
            $("#GSTMidPrt, #GSTLastPrt").css("border-color", "#ced4da");
            return true;
        }
    }
    else {
        return true;
    }

}
function ReadCustAddress() {
    debugger;
    var AddressList = new Array();
    $("#tblCustomerAddressDetail TBODY TR").each(function () {

        debugger;
        var rowNum = 0;
        var ShipRowID = 0;
        var row = $(this);

        var Branch = {};

        var BIllrowId = $(this).find("#spanBillrowId").text();

        Branch.address = row.find("#hdncustadd").text();
        Branch.City = row.find("#hdncustcity").text();
        Branch.CityName = row.find("#hdncityname").text();
        Branch.District = row.find("#hdncustdist").text();
        Branch.DistrictName = row.find("#hdndistname").text();
        Branch.State = row.find("#hdncuststate").text();
        Branch.StateName = row.find("#hdnstate").text();
        Branch.Country = row.find("#hdncustcntry").text();
        Branch.CountryName = row.find("#hdncntryname").text();
        Branch.GSTNo = row.find("#hdncustgst").text();
        Branch.BillingAddress = row.find("#spanhiddenBill_" + BIllrowId).text();
        Branch.ShippingAddress = row.find("#spanhiddenShip_" + BIllrowId).text();
        Branch.address_id = row.find("#address_id").text();
        Branch.cust_pin = row.find("#hdncustpin").text();
        Branch.Flag = row.find("#hdnID").text();
        Branch.Addrcont_per = row.find("#tblAddrcont_per").text();
        Branch.Addrcont_no = row.find("#tblAddrcont_no").text();
        AddressList.push(Branch);
    });
    debugger;
    var str = JSON.stringify(AddressList);
    $('#hdCustomerAddressList').val(str);
    /*Commented By Hina on 10-01-2023 to create this functionality on Pageload under Supervision of Premsir*/
    //EditAddrDetail();
}
function defbillchk1(e) {
    debugger;
    var rowIdx = "";
    try {
        var clickedrow = $(e.target).closest("tr");
        var BillRowID = clickedrow.find("#spanBillrowId").text();
        if (clickedrow.find("#chkCBA_" + BillRowID).is(":checked")) {
            $("#tblCustomerAddressDetail TBODY TR").each(function () {
                debugger;
                rowIdx = $(this).find("#spanBillrowId").text();

                if ($(this).find("#chkCBA_" + rowIdx).prop("checked", false)) {
                    $(this).find("#spanhiddenBill_" + rowIdx).text("N");
                }
                $("#chkCBA_" + rowIdx).not(this).prop("checked", false);
            });
            clickedrow.find("#chkCBA_" + BillRowID).prop("checked", true);
            clickedrow.find("#spanhiddenBill_" + BillRowID).text("Y");
            ReadCustAddress();
        }
        else {
            clickedrow.find("#spanhiddenBill_" + BillRowID).text("N");
            ReadCustAddress();
        }
    }
    catch (err) {

    }
}
function defbillchk(e) {
    debugger;
    var rowIdx = "";
    try {
        var clickedrow = $(e.target).closest("tr");
        var BillRowID = clickedrow.find("#spanBillrowId").text();
        if (clickedrow.find("#chkCBA_" + BillRowID).is(":checked")) {
            $("#tblCustomerAddressDetail TBODY TR").each(function () {
                debugger;
                rowIdx = $(this).find("#spanBillrowId").text();
                $(this).find("#chkCBA_" + rowIdx).prop("checked", false);
                $(this).find("#spanhiddenBill_" + rowIdx).text("N");
            });
            clickedrow.find("#chkCBA_" + BillRowID).prop("checked", true);
            clickedrow.find("#spanhiddenBill_" + BillRowID).text("Y");
            ReadCustAddress();
        }
        else {
            clickedrow.find("#spanhiddenBill_" + BillRowID).text("N");
            ReadCustAddress();
        }
        clickedrow.find("#chkCBA_").prop("checked", true);
    }
    catch (err) {

    }
}
function defshipchk(e) {
    debugger;
    var rowIdx = "";
    try {
        var clickedrow = $(e.target).closest("tr");
        var ShipRowID = clickedrow.find("#spanShiprowId").text();
        if (clickedrow.find("#chkCSA_" + ShipRowID).is(":checked")) {
            $("#tblCustomerAddressDetail TBODY TR").each(function () {
                debugger;
                rowIdx = $(this).find("#spanShiprowId").text();
                $(this).find("#chkCSA_" + rowIdx).prop("checked", false);
                $(this).find("#spanhiddenShip_" + rowIdx).text("N");
            });
            clickedrow.find("#chkCSA_" + ShipRowID).prop("checked", true);
            clickedrow.find("#spanhiddenShip_" + ShipRowID).text("Y");
            ReadCustAddress();
        }
        else {
            clickedrow.find("#spanhiddenShip_" + ShipRowID).text("N");
            ReadCustAddress();
        }
    }
    catch (err) {

    }
}
function CheckDuplicacy() {
    debugger;
    var customer_address = $("#customer_address").val();

    var customer_Pin = $("#Cust_Pin").val();
    var CustomerCity_Name = $("#Cust_City").val();
    //$(this).val().toLowerCase()
    var validflg = true;
    $("#tblCustomerAddressDetail TBODY TR").each(function () {
        var currentRow = $(this).closest('tr')
        debugger;
        //CheckDuplicateOnAdd();
        var address = currentRow.find("#hdncustadd").text();
        var cust_pin = currentRow.find("#hdncustpin").text();
        var City = currentRow.find("#hdncustcity").text();
        var checkID = currentRow.find("#hdnID").text();
        if (checkID != "Delete") {
            //if (customer_address.toLowerCase() == address.toLowerCase() && customer_Pin == cust_pin && CustomerCity_Name == City) {
            if (customer_address.toLowerCase() == address.toLowerCase() && CustomerCity_Name == City) {
                document.getElementById("SpancustAddr").innerHTML = $("#valueduplicate").text();
                $("#customer_address").css('border-color', 'red');
                $("#SpancustAddr").attr("style", "display: block;");
                //document.getElementById("vmCustPin").innerHTML = $("#valueduplicate").text();
                //$("#Cust_Pin").css('border-color', 'red');
                //$("#vmCustPin").attr("style", "display: block;");
                document.getElementById("vmCustCityname").innerHTML = $("#valueduplicate").text();
                $("[aria-labelledby='select2-Cust_City-container']").css('border-color', 'red');
                $("#vmCustCityname").attr("style", "display: block;");
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
function CheckDuplicateOnUpdate() {
    debugger;
    var customer_address = $("#customer_address").val();
    var customer_Pin = $("#Cust_Pin").val();
    var CustomerCity_Name = $("#Cust_City").val();
    var RowidUpdt = $("#hdnRowId").val();

    validflg = true;
    $("#tblCustomerAddressDetail TBODY TR").each(function () {
        currentRow = $(this);
        rowId = $(this).find("#spanShiprowId").text();
        if (rowId > 0) {
            if (RowidUpdt != rowId) {
                var address = currentRow.find("#hdncustadd").text();
                var cust_pin = currentRow.find("#hdncustpin").text();
                var City = currentRow.find("#hdncustcity").text();
                var checkID = currentRow.find("#hdnID").text();
                if (checkID != "Delete") {
                    /*Commented by Hina on 24-01-2024 not validate in in case of export*/
                    //if (customer_address.toLowerCase() == address.toLowerCase() && customer_Pin == cust_pin && CustomerCity_Name == City) {
                    if (customer_address.toLowerCase() == address.toLowerCase() && CustomerCity_Name == City) {
                        document.getElementById("SpancustAddr").innerHTML = $("#valueduplicate").text();
                        $("#customer_address").css('border-color', 'red');
                        $("#SpancustAddr").attr("style", "display: block;");
                        //document.getElementById("vmCustPin").innerHTML = $("#valueduplicate").text();
                        //$("#Cust_Pin").css('border-color', 'red');
                        //$("#vmCustPin").attr("style", "display: block;");
                        document.getElementById("vmCustCityname").innerHTML = $("#valueduplicate").text();
                        $("[aria-labelledby='select2-Cust_City-container']").css('border-color', 'red');
                        $("#vmCustCityname").attr("style", "display: block;");

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
function ValidateValReqWithInvalidGSTCust() {
    var custtype = $("#HdnCustType").val();
    if (custtype == "D") {
        debugger;
        if (CustomerGST == "") {
            document.getElementById("SpancustGST").innerHTML = $("#valueReq").text();
            $("#gst_num").css('border-color', 'red');
            $("#SpancustGST").attr("style", "display: block;");
            $("#GSTNumrequired").css("display", "inherit");
            valFalg = false;
        }
        else {
            if (CustomerGST != "" && CustomerGST.length == "2") {
                $("#gst_num").css('border-color', "#ced4da");
                $("#SpancustGST").attr("style", "display: none;");
                document.getElementById("SpancustGST").innerHTML = "";

                if (GSTNoMidPrt == "") {
                    document.getElementById("SpancustGST").innerHTML = $("#valueReq").text();
                    $("#GSTMidPrt").css('border-color', 'red');
                    $("#SpancustGST").attr("style", "display: block;");
                    $("#GSTNumrequired").css("display", "inherit");
                    valFalg = false;

                }
                else {
                    if (GSTNoMidPrt != "" && GSTNoMidPrt.length == "10") {
                        $("#GSTMidPrt").css('border-color', "#ced4da");
                        $("#SpancustGST").attr("style", "display: none;");
                        document.getElementById("SpancustGST").innerHTML = "";

                        if (GSTNoLastPrt == "") {
                            document.getElementById("SpancustGST").innerHTML = $("#valueReq").text();
                            $("#GSTLastPrt").css('border-color', 'red');

                            $("#SpancustGST").attr("style", "display: block;");
                            $("#GSTNumrequired").css("display", "inherit");
                            valFalg = false;
                        }
                        else {

                            if (GSTNoLastPrt != "" && GSTNoLastPrt.length == "3") {
                                $("#GSTLastPrt").css('border-color', "#ced4da");
                                $("#SpancustGST").attr("style", "display: none;");
                                document.getElementById("SpancustGST").innerHTML = "";
                            }
                            else {
                                $("#GSTLastPrt").css('border-color', 'red');
                                $("#SpancustGST").attr("style", "display: block;");
                                document.getElementById("SpancustGST").innerHTML = $("#InvalidGSTNumber").text();
                                valFalg = false;
                            }
                        }
                    }
                    else {

                        $("#GSTMidPrt").css('border-color', 'red');
                        $("#SpancustGST").attr("style", "display: block;");
                        document.getElementById("SpancustGST").innerHTML = $("#InvalidGSTNumber").text();
                        valFalg = false;
                    }
                }

            }
            else {
                $("#gst_num").css('border-color', 'red');
                $("#SpancustGST").attr("style", "display: block;");
                document.getElementById("SpancustGST").innerHTML = $("#InvalidGSTNumber").text();
                valFalg = false;
            }
        }




    }
    else {
        document.getElementById("SpancustGST").innerHTML = "";
        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#GSTLastPrt").css('border-color', "#ced4da");
        $("#SpancustGST").attr("style", "display: none;");
        $("#GSTNumrequired").css("style", "display:none");
    }
}

function ClearAddressDetail() {
    debugger;
    $("#customer_address").val("");
    $("#Cust_Pin").val("");
    $("#gst_num").val("");
    $("#GSTMidPrt").val("");
    $("#GSTLastPrt").val("");
    /*-----------start Code bY Hina on 09-01-2023-------------------- */
    var custtype;
    if ($("#Export").is(":checked")) {
        custtype = "I";
    }
    if ($("#Domestic").is(":checked")) {
        custtype = "D";
    }
    if (custtype == "D") {
        $('#Cust_Country').val('101');

        /* $('#Cust_State').val('0');*/
        $('#Cust_District').empty();
        $('#Cust_District').append(`<option value="0">---Select---</option>`);
        $('#Cust_City').empty();
        $('#Cust_City').append(`<option value="0">---Select---</option>`);
        GetStateByCountry();
    }
    else {
        $("#Cust_Country").val("0").change();
        $("#Cust_Country").val(0);
        $("#HdnCustContry").val("0");
        $('#Cust_State').empty();
        $('#Cust_State').append(`<option value="0">---Select---</option>`);
        $('#Cust_District').empty();
        $('#Cust_District').append(`<option value="0">---Select---</option>`);
        $('#Cust_City').empty();
        $('#Cust_City').append(`<option value="0">---Select---</option>`);

    }
    /*-----------End Code bY Hina on 09-01-2023-------------------- */
    //$("#cust_city").val("0").change();
    //$("#district_id").val("");
    //$("#state_id").val("");
    //$("#country_id").val("");
    $("#divUpdate").css("display", "none");
    $('#divAddAddrDtl').css("display", "Block");
    $("#chkCustomerBillingAddress").prop("checked", true);
    $("#chkCustomerShippingAddress").prop("checked", true);

    document.getElementById("SpancustAddr").innerHTML = "";
    $("#customer_address").css('border-color', '#ced4da');
    $("#SpancustAddr").attr("style", "display: none;");
    document.getElementById("vmCustPin").innerHTML = "";
    $("#Cust_Pin").css('border-color', '#ced4da');
    $("#vmCustPin").attr("style", "display: none;");
    document.getElementById("vmCustCityname").innerHTML = "";
    $("[aria-labelledby='select2-Cust_City-container']").css('border-color', '#ced4da');
    $("#vmCustCityname").attr("style", "display: none;");
}

function EditAddrDetail() {

    debugger;

    //---------------------------- Row edit Button funtionality ------------------//
    //$("#tblCustomerAddressDetail >tbody >tr").on('click', "#editBtnIcon", function (e) {

    //    debugger;
    //    var currentRow = $(this).closest('tr')
    //    var row_index = currentRow.index();

    //    var address = currentRow.find("#hdncustadd").text()
    //    var cust_pin = currentRow.find("#hdncustpin").text()
    //    var GSTNo = currentRow.find("#hdncustgst").text()

    //    var Statecode = GSTNo.substring(0,2);
    //    var PanNum = GSTNo.substring(2,12);
    //    var leftcode = GSTNo.substring(12,15);
    //    var City = currentRow.find("#hdncustcity").text()
    //    var address_id = currentRow.find("#address_id").text()
    //    debugger;
    //    var hiddenRowNo = currentRow.find("#spanBillrowId").text();
    //    if (currentRow.find("#chkCBA_" + hiddenRowNo).is(":checked")) {
    //        $("#chkCustomerBillingAddress").prop("checked", true);
    //    }
    //    else {
    //        $("#chkCustomerBillingAddress").prop("checked", false);
    //    }
    //    if (currentRow.find("#chkCSA_" + hiddenRowNo).is(":checked")) {
    //        $("#chkCustomerShippingAddress").prop("checked", true);
    //    }
    //    else {
    //        $("#chkCustomerShippingAddress").prop("checked", false);
    //    }
    //    var col10_value = row_index
    //    debugger;
    //    //$("#hdnRow_id").val(row_index);
    //    $("#customer_address").val(address);
    //    $("#Pin").val(cust_pin);
    //    $("#gst_num").val(Statecode);
    //    $("#GSTMidPrt").val(PanNum);
    //    $("#GSTLastPrt").val(leftcode);
    //    getCustCity();
    //    $("#cust_city").val(City).trigger("change");
    //    debugger;
    //    $("#hdnRowId").val(hiddenRowNo);
    //    $("#UpdtAddrId").val(address_id);
    //    $("#divUpdate").css("display", "block");
    //    $('#divAddAddrDtl').css("display", "none");
    //    onchangeCustAddr();
    //    onchangeCustPin();
    //    onchangeGST();
    //});

}
function OnClickAddressUpdateBtn() {
    debugger;
    if (validatetbldt() == false) {
        return false;
    }
    if (CheckDuplicateOnUpdate() == false) {
        return false;
    }
    else {
        var UpdateAddr = updateCustomerAddr();
        if (UpdateAddr == false) {
            return false;
        }
        else {
            return true;
        }

    }
       

}
async function updateCustomerAddr() {
    const isGstValid = await CheckDuplicateGstNoAsync();
    if (!isGstValid) {
        return false;
    }
    else {
        var CustomerBillingAddress = "";
        var CustomerShippingAddress = "";
        if ($("#chkCustomerBillingAddress").is(":checked")) {
            CustomerBillingAddress = "Y";
        }
        else {
            CustomerBillingAddress = "N";
        }
        if ($("#chkCustomerShippingAddress").is(":checked")) {
            CustomerShippingAddress = "Y";
        }
        else {
            CustomerShippingAddress = "N";
        }
        debugger;
        if (CustomerBillingAddress == 'Y') {
            //var rowId = 0;
            $("#tblCustomerAddressDetail TBODY TR").each(function () {
                debugger;
                //rowId = rowId + 1;
                var rowId = $(this).find("#spanBillrowId").text();

                $(this).find("#chkCBA_" + rowId).prop("checked", false);
                $(this).find("#spanhiddenBill_" + rowId).text("N");
            });
        }
        if (CustomerShippingAddress == 'Y') {
            debugger;
            $("#tblCustomerAddressDetail TBODY TR").each(function () {
                var rowId = $(this).find("#spanBillrowId").text();
                $(this).find("#chkCSA_" + rowId).prop("checked", false);
                $(this).find("#spanhiddenShip_" + rowId).text("N");
            });
        }
        debugger;
        var addr_idtoUpdt = $("#UpdtAddrId").val();
        var RowidtoUpdt = $("#hdnRowId").val();
        var CustGST = $("#gst_num").val().trim();
        var GSTMidPrt = $("#GSTMidPrt").val().trim();
        var GSTLastPrt = $("#GSTLastPrt").val().trim();
        if (GSTMidPrt != "" || GSTLastPrt != "") {
            var GSTNum = CustGST + GSTMidPrt + GSTLastPrt;
            $("#hdCustmerGst").val(GSTNum);
        } else {
            if (GSTMidPrt == "" || GSTLastPrt == "") {
                var GSTNum = GSTMidPrt + GSTLastPrt;
                $("#hdCustmerGst").val(GSTNum);
            }
        }
        /* start Add code by Hina on 11-01-2024*/
        var CustType;
        if ($("#Export").is(":checked")) {
            CustType = "I";
            $("#HdnCustType").val(CustType);
            //$("#HdnCustState").val(0);

        }
        if ($("#Domestic").is(":checked")) {
            CustType = "D";
            $("#HdnCustType").val(CustType);
        }
        var Country;
        if (CustType == "D") {
            Country = $("#Cust_Country").text().trim();
        }
        else {
            Country = $("#select2-Cust_Country-container").text();
            Country = $("#Cust_Country option:selected").text();
        }
        document.getElementById("SpancustAddr").innerHTML = "";
        $("#customer_address").css('border-color', '#ced4da');
        $("#SpancustAddr").attr("style", "display: none;");
        /* End Add code by Hina on 11-01-2024*/
        var flagDup = true;
        $('#tblCustomerAddressDetail >tbody >tr').each(function () {
            var currentRow = $(this);
            debugger;

            var rowNum = $(this).find("#spanBillrowId").text();
            var address_id = currentRow.find('#address_id').text();
            //This Created and commented By Hina on 05 - 12 - 2022 and 6 - 12 - 2022
            //for check duplicacy
            //if (RowidtoUpdt != rowNum) {
            //    /*var flagDup = true;*/
            //    if (CheckDuplicateOnUpdate() == false) {
            //        flagDup = false;
            //        return false;
            //    }

            //}

            if (addr_idtoUpdt == address_id) {
                /*code arrange and changes by Hina on 10-01-2023 to get value by Id country to pin*/
                currentRow.find("#hdncustadd").text($("#customer_address").val());
                currentRow.find("#hdncntryname").text(Country);
                currentRow.find("#hdncustcntry").text($("#Cust_Country").val());
                currentRow.find("#hdnstate").text($("#select2-Cust_State-container").text());
                currentRow.find("#hdncuststate").text($("#Cust_State").val());
                currentRow.find("#hdndistname").text($("#select2-Cust_District-container").text());
                currentRow.find("#hdncustdist").text($("#Cust_District").val());
                currentRow.find("#hdncityname").text($("#select2-Cust_City-container").text());
                currentRow.find("#hdncustcity").text($("#Cust_City").val());
                currentRow.find("#hdncustpin").text($("#Cust_Pin").val());
                /*currentRow.find("#hdncustgst").text($("#gst_num").val());*/
                currentRow.find("#hdncustgst").text($("#hdCustmerGst").val());
                currentRow.find("#tblAddrcont_per").text($("#Addr_cont_per").val());
                currentRow.find("#tblAddrcont_no").text($("#Addr_cont_no").val());

                /*Commented By Hina on 10-01-2023 16:41 do the above code*/

                //currentRow.find("#hdncustadd").text($("#customer_address").val());
                //currentRow.find("#hdncustpin").text($("#Pin").val());
                //currentRow.find("#hdncityname").text($("#select2-cust_city-container").text());
                //currentRow.find("#hdndistname").text($("#district_id").val());
                //currentRow.find("#hdncustdist").text($("#hdCustmerDistrictID").val());
                //currentRow.find("#hdnstate").text($("#state_id").val());
                //currentRow.find("#hdncuststate").text($("#hdCustmerStateID").val());
                //currentRow.find("#hdncntryname").text($("#country_id").val());
                //currentRow.find("#hdncustcntry").text($("#hdCustmerCountryID").val());
                ///*currentRow.find("#hdncustgst").text($("#gst_num").val());*/
                //currentRow.find("#hdncustgst").text($("#hdCustmerGst").val());
                //currentRow.find("#hdncustcity").text($("#cust_city").val());
                if (CustomerBillingAddress == "Y") {
                    var hiddenRowNo = currentRow.find("#spanBillrowId").text();
                    currentRow.find("#chkCBA_" + hiddenRowNo).prop("checked", true);
                    $("#spanhiddenBill_" + rowNum).text("Y");
                }
                else {
                    var hiddenRowNo = currentRow.find("#spanBillrowId").text();
                    currentRow.find("#chkCBA_" + hiddenRowNo).prop("checked", false)
                    $("#spanhiddenBill_" + rowNum).text("N");
                }
                if (CustomerShippingAddress == "Y") {
                    var hiddenRowNo = currentRow.find("#spanBillrowId").text();

                    currentRow.find("#chkCSA_" + hiddenRowNo).prop("checked", true);
                    $("#spanhiddenShip_" + rowNum).text("Y");
                }
                else {
                    var hiddenRowNo = currentRow.find("#spanBillrowId").text();

                    currentRow.find("#chkCSA_" + hiddenRowNo).prop("checked", false);
                    $("#spanhiddenShip_" + rowNum).text("N");
                }
                if (CustomerBillingAddress == "Y") {

                }
                else {
                    var rowId = 0;
                    var cheakflag = "N";
                    var cheackdefalt = "Q";
                    $("#tblCustomerAddressDetail TBODY TR").each(function () {
                        var BIllrowId = currentRow.find("#spanBillrowId").text();
                        if ($(this).find("#chkCBA_" + BIllrowId).is(":checked")) {
                            cheackdefalt = "P";
                        }

                    });

                    if (cheackdefalt == "Q") {
                        debugger;
                        var rowIdx1 = 1;
                        var firstactive = "N";
                        $("#tblCustomerAddressDetail TBODY TR").each(function () {
                            debugger;
                            var currentRow = $(this);
                            if (firstactive == "N") {
                                var hdn_id = currentRow.find("#hdnID").text();
                                if (hdn_id != "Delete") {
                                    var BIllrowId = $(this).find("#spanBillrowId").text();
                                    //currentRow.find("#chkCBA_" + BIllrowId).attr("checked", true);
                                    //$("#spanhiddenBill_" + rowIdx1).text("Y");
                                    firstactive = "Y";

                                    //return false;
                                }
                            }
                        });
                    }

                }
                if (CustomerShippingAddress == "Y") {

                }
                else {
                    var rowId = 0;
                    var cheakflag = "N";
                    var cheackdefalt = "Q";
                    $("#tblCustomerAddressDetail TBODY TR").each(function () {
                        var BIllrowId = $(this).find("#spanBillrowId").text();
                        if ($(this).find("#chkCSA_" + BIllrowId).is(":checked")) {
                            cheackdefalt = "P";
                        }
                    });
                    if (cheackdefalt == "Q") {
                        debugger;
                        var rowIdx1 = 1;
                        var firstactive = "N";
                        $("#tblCustomerAddressDetail TBODY TR").each(function () {
                            debugger;
                            var currentRow = $(this);
                            if (firstactive == "N") {
                                var hdn_id = currentRow.find("#hdnID").text();
                                if (hdn_id != "Delete") {
                                    var BIllrowId = $(this).find("#spanBillrowId").text();

                                    //currentRow.find("#chkCSA_" + BIllrowId).attr("checked", true);
                                    //$("#spanhiddenShip_" + rowIdx1).text("Y");
                                    firstactive = "Y";
                                    return false;
                                }

                            }
                        });
                    }
                }
            }
        });
        /*if (flagDup == true) {*/
        $("#customer_address").val("");
        $("#Addr_cont_per").val("");
        $("#Addr_cont_no").val("");
        $("#Cust_Pin").val("");
        $("#gst_num").val("");
        var gstCat = $("#Gst_Cat").val();
        if (gstCat == "RR") {
           // $("#GSTMidPrt").val("");
        }
        else {
            $("#GSTMidPrt").val("");
        }
        $("#GSTLastPrt").val("");
        debugger;
        /*-----------start Code bY Hina on 10-01-2023-------------------- */
        var CustType = "";
        if ($("#Export").is(":checked")) {
            CustType = "E";
            $("#HdnCustType").val(CustType);
        }
        if ($("#Domestic").is(":checked")) {
            CustType = "D";
            $("#HdnCustType").val(CustType);
        }
        if (CustType == "E") {

            $("#Cust_Country").val("0").change();
            $("#Cust_State").val("0");
            $('#Cust_District').empty();
            $('#Cust_District').append(`<option value="0">---Select---</option>`);
            $('#Cust_City').empty();
            $('#Cust_City').append(`<option value="0">---Select---</option>`);

        }
        else {
            $("#Cust_State").val("0").change();
            $("#Cust_District").val("0");
            $("#Cust_City").val("0");
        }
        /*-----------End Code bY Hina on 10-01-2023-------------------- */
        //$("#cust_city").val("0").change();
        //$("#district_id").val("");
        //$("#state_id").val("");
        //$("#country_id").val("");
        $("#divUpdate").css("display", "none");
        $('#divAddAddrDtl').css("display", "Block");
        $("#chkCustomerBillingAddress").prop("checked", true);
        $("#chkCustomerShippingAddress").prop("checked", true);
        onchangechkCustsuppBilling();
        var shipBillValidation = $("#hdnGstFlagValid").val();
        var GstApplicable = $("#Hdn_GstApplicable").text();
        if (GstApplicable == "Y") {
            if (shipBillValidation != "NotValidation") {
                $("#GSTNumrequired").css("display", "inherit");
            }
        }
       
        /* }*/
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

function OnclickHistorySearchBtn() {
    debugger;
    var CustID = $("#cust_ID").val();
    var Fromdate = $("#txtFromdate").val();
    var Todate = $("#txtTodate").val();
    CustomerSetupSaleHistory(CustID, Fromdate, Todate);
}
function Approveonclick() { /**Added This Function 01-01-2024 for approve btn**/
    debugger;
    if ($("#hdnSavebtn").val() == "AllreadyclickApprovebtn") {
        $("#btn_approve").css("filter", "grayscale(100%)");
        $("#btn_approve").attr("disabled", true);
    }
    else {
        $("#hdnSavebtn").val("AllreadyclickApprovebtn");
        $("#btn_approve").css("filter", "grayscale(100%)");
    }
}

/*Code by Hina on 10-01-2024 to Bind all data in dropdown and also OnChange COUNTRY,state,district,city */
function GetCountryOnChngCustType(Custcurr) {
    debugger;

    $('#Cust_Country').empty();

    $.ajax({
        type: "POST",
        url: "/BusinessLayer/CustomerDetails/GetCountryonChngCustTyp",
        data: { CustType: Custcurr },
        success: function (data) {
            debugger;
            var arr = [];
            arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
            if (arr.length > 0) {
                for (var i = 0; i < arr.length; i++) {
                    $('#Cust_Country').append(`<option value="${arr[i].country_id}">${arr[i].country_name}</option>`);
                }
                $("#Cust_Country").select2();/*For making Serachable Dropdown */
                $('#Cust_District').empty();
                $('#Cust_District').append(`<option value="0">---Select---</option>`);
                $('#Cust_City').empty();
                $('#Cust_City').append(`<option value="0">---Select---</option>`);
                $('#Cust_Pin').val("");
                $('#gst_num').val("");
                $('#customer_address').val("");
                $('#GSTMidPrt').val("");
                $('#GSTLastPrt').val("");
                /*---To Hide Address Detail On ONChange of CustTyp-----*/
                $("#tblCustomerAddressDetail tbody tr ").each(function () { $(this).find(".deleteIcon").click() })
                GetStateByCountry();
                var hdnCountryId = $("#Cust_Country").val()

            }
        },
    });
    /*GetProspectcurr();*/
}
function ClearAllData_OnChngCustType() {
    debugger;

    //$('#customer_address').val("");
    $('#Cust_State').empty();
    $('#Cust_State').append('<option value="0">---Select---</option>')
    $('#Cust_District').empty();
    $('#Cust_District').append('<option value="0">---Select---</option>')
    $('#Cust_City').empty();
    $('#Cust_City').append('<option value="0">---Select---</option>')
    $('#Cust_Pin').val("");

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
    document.getElementById("vmCustCountryname").innerHTML = "";
    $("[aria-labelledby='select2-Cust_Country-container']").css("border-color", "#ced4da");
    GetStateByCountry();
}
function GetStateByCountry() {
    debugger;
    var CustType;
    if ($("#Export").is(":checked")) {
        CustType = "E";
        $("#HdnCustType").val(CustType);
        //$("#HdnCustState").val(0);

    }
    if ($("#Domestic").is(":checked")) {
        CustType = "D";
        $("#HdnCustType").val(CustType);

    }
    var ddlCountryID = $("#Cust_Country").val();

    $.ajax({
        type: "POST",
        url: "/CustomerDetails/GetstateOnCountry",/*Controller=CustomerSetup and Fuction=GetCustport*/
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
                    $('#Cust_State').empty();
                    $('#Cust_State').append(`<option value="0">---Select---</option>`);
                    for (var i = 0; i < arr.length; i++) {

                        $('#Cust_State').append(`<option value="${arr[i].state_id}">${arr[i].state_name}</option>`);
                    }
                    $("#Cust_State").select2();/*For making Serachable Dropdown */
                    if (CustType == "E") {
                        var hdnSTATE = $("#HdnCustState").val()
                        if (hdnSTATE == "") {
                            hdnSTATE = "0";
                        }
                        $("#Cust_State").val(hdnSTATE).trigger("change");
                        $("#HdnCustState").val("0");
                    }

                }
                else {
                    $('#Cust_State').empty();
                    $('#Cust_State').append(`<option value="0">---Select---</option>`);
                }


            }
        },
        error: function (Data) {
        }
    });
};
function OnChangeState() {
    debugger;
    $('#Cust_City').empty();
    $('#Cust_City').append(`<option value="0">---Select---</option>`);
    //$('#Cust_Pin').val("");
    GetDistrictByState();
    GstStateCodeOnChangeStatus();
    $('#vmCustStatename').text("");
    $("#vmCustStatename").css("display", "none");
    $("[aria-labelledby='select2-Cust_State-container']").css('border-color', "#ced4da");
    $("#Cust_State").css('border-color', "#ced4da");

}
function GetDistrictByState() {
    debugger;

    var ddlStateID = $("#Cust_State").val();

    $.ajax({
        type: "POST",
        url: "/CustomerDetails/GetDistrictOnState",/*Controller=CustomerSetup and Fuction=GetCustport*/
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
                    $('#Cust_District').empty();

                    for (var i = 0; i < arr.length; i++) {
                        $('#Cust_District').append(`<option value="${arr[i].district_id}">${arr[i].district_name}</option>`);
                    }
                    $("#Cust_District").select2();/*For making Serachable Dropdown */

                    var hdnDistrct = $("#HdnCustDistrict").val()
                    $("#Cust_District").val(hdnDistrct).trigger("change");
                    $("#HdnCustDistrict").val("0");
                    //$("[aria-labelledby='select2-Cust_District-container']").css('border-color', 'red');
                    //$("#Cust_District").css('border-color', 'red');
                    //$("#vmCustDistname").attr("style", "display: block;");

                }
            }
        },
        error: function (Data) {
        }
    });
};
function GstStateCodeOnChangeStatus() {
    debugger

    var stateCode = $('#Cust_State').val();
    $.ajax({
        type: "POST",
        url: "/CustomerDetails/GetStateCode",/*Controller=CustlierSetup and Fuction=GetCustport*/
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
    //var District = $("#Cust_District").val();
    //var state = $("#Cust_State").val();
    //if (state != "0") {
    //    if (District == '0') {
    //        document.getElementById("vmCustDistname").innerHTML = $("#valueReq").text();
    //        $("[aria-labelledby='select2-Cust_District-container']").css('border-color', 'red');
    //        $("#Cust_District").css('border-color', 'red');
    //        $("#vmCustDistname").attr("style", "display: block;");
    //    }
    //    else {
    $('#vmCustDistname').text("");
    $("#vmCustDistname").css("display", "none");
    $("[aria-labelledby='select2-Cust_District-container']").css('border-color', "#ced4da");
    //    }
    //}
}

function GetCityByDistrict() {
    debugger;

    var ddlDistrictID = $("#Cust_District").val();

    $.ajax({
        type: "POST",
        url: "/CustomerDetails/GetCityOnDistrict",/*Controller=CustlierSetup and Fuction=GetCustport*/
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async: true,
        data: { ddlDistrictID: ddlDistrictID, },/*Registration pass value like model*/
        success: function (data) {
            if (data == 'ErrorPage')
            {
                ErrorPage();
                return false;
            }
            /*dynamically dropdown list of all Assessment */
            debugger;
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
                if (arr.length > 0) {
                    $('#Cust_City').empty();
                    for (var i = 0; i < arr.length; i++) {
                        $('#Cust_City').append(`<option value="${arr[i].city_id}">${arr[i].city_name}</option>`);
                    }
                    $("#Cust_City").select2();/*For making Serachable Dropdown */
                    var HdnCustCity = $("#HdnCustCity").val();
                    $("#Cust_City").val(HdnCustCity).trigger("change");
                    $("#HdnCustCity").val('0');
                    //$("[aria-labelledby='select2-Cust_City-container']").css('border-color', 'red');
                    //$("#Cust_City").css('border-color', 'red');
                    //$("#vmCustCityname").attr("style", "display: block;");
                }


            }
        },
        error: function (Data) {
        }
    });
};
function OnChangeCity() {
    debugger;
    //var City = $("#Cust_City").val()
    //var state = $("#Cust_State").val();
    //if (state != "0") {
    //    if (City == "0") {
    //        document.getElementById("vmCustCityname").innerHTML = $("#valueReq").text();
    //        $("[aria-labelledby='select2-Cust_City-container']").css('border-color', 'red');
    //        $("#Cust_City").css('border-color', 'red');
    //        $("#vmCustCityname").attr("style", "display: block;");
    //    }
    //    else {
    $('#vmCustCityname').text("");
    $("#vmCustCityname").css("display", "None");
    $("[aria-labelledby='select2-Cust_City-container']").css('border-color', "#ced4da");
    //    }
    //}
    //var Status = $("#Status").val();

    //if (Status != "" && Status != null) {
    // Get the text content of the country element
    var country = $("#Cust_Country").text().trim().toLowerCase();
    // Check if the country is India and the Domestic checkbox is checked
    if ((country) === "india" && $("#Domestic").is(":checked")) {
        // Set the maximum length of the PIN input to 6
        $("#Cust_Pin").attr("maxlength", "6");

        // $("#Cust_Pin").attr("minlength", "6");
    } else {
        $("#Cust_Pin").attr("maxlength", "12");
    }
    // }


}
$("#Cust_Pin").on("keypress", function (event) {
    return OnkeyPressConNumber(event);
});
function OnkeyPressConNumber(event) {
    debugger;
    var charCode = event.which || event.keyCode;
    var char = String.fromCharCode(charCode);

    // Allow only numeric values (0-9)
    if (!/\d/.test(char)) {
        event.preventDefault(); // Block non-numeric input
    }
};
function OnchangeCustomerNUm() {
    debugger;
    var Hdn_GstApplicable = $("#Hdn_GstApplicable").text();
    var cont_num1 = $("#cont_num1").val().length
    if (cont_num1 != null && cont_num1 != "") {
        if (Hdn_GstApplicable == "Y") {
            if ($("#Export").is(":checked") == false)
            {
                if (cont_num1 < 10) {
                    document.getElementById("vmcont_num1").innerHTML = ($("#span_InvalidFormat").text() + " (9999999999)");
                    $("#cont_num1").css('border-color', 'red');
                    $("#vmcont_num1").attr("style", "display: block;");
                    $("#cont_num1").css("display", "inherit");
                    return false;
                }
            }
            else {
                $('#vmcont_num1').text("");
                $("#vmcont_num1").css("display", "None");
                $("#cont_num1").css('border-color', "#ced4da");
            }
        }
    }
    else {
        $('#vmcont_num1').text("");
        $("#vmcont_num1").css("display", "None");
        $("#cont_num1").css('border-color', "#ced4da");
    }
}

function BtnSearchSaleHistory() {

    debugger;
    var FromDate = $("#txtFromdate").val();
    var ToDate = $("#txtTodate").val();
    var cust_ID = $("#cust_ID").val();
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/CustomerDetails/GetCustomerSalesDetail",/*Controller=ItemSetup and Fuction=GetItemList*/
        data: {
            FromDate: FromDate,
            ToDate: ToDate,
            cust_ID: cust_ID,
        },
        success: function (data) {
            debugger;
            $('#CustomerSaleHistory').html(data);

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

function onchangechkCustsuppBilling() {
    debugger;
    var billingChecked = $("#chkCustomerBillingAddress").is(":checked");
    var shippingChecked = $("#chkCustomerShippingAddress").is(":checked");
    var Domestic = $("#Domestic").is(":checked");
    if (Domestic) {
        if (billingChecked) {
            var gst_cat = $("#Gst_Cat").val();

            if (gst_cat === "UR" || gst_cat === "EX" || gst_cat === "CO" || gst_cat ==="SZ") {
                // Hide GST fields
                if (gst_cat === "UR") {
                    $("#GSTMidPrt").attr("disabled", true);
                }
                else {
                    $("#GSTMidPrt").attr("disabled", false);
                }
                $("#GSTNumrequired").hide();
                $("#GSTMidPrt").css('border-color', "#ced4da");
                $("#SpanCustGSTMidPrt").hide().text("");
                $("#GSTLastPrt").css('border-color', "#ced4da");
                $("#SpancustGST").hide().text("");
                $("#hdnGstFlagValid").val("NotValidation");
            }
            else {
                // Show GST fields
                var GstApplicable = $("#Hdn_GstApplicable").text();
                if (GstApplicable == "Y") {
                    $("#GSTMidPrt").attr("disabled", true);
                    $("#GSTNumrequired").css("display", "inherit");
                    onchangePanNumber();
                }
                else {
                    $("#GSTMidPrt").attr("disabled", false);
                }
             
                $("#hdnGstFlagValid").val("Validation");
            }
        }
        else if (!billingChecked && shippingChecked) {
            // Hide GST fields
            $("#GSTNumrequired").hide();
            $("#GSTMidPrt").attr("disabled", false);
            $("#GSTMidPrt").css('border-color', "#ced4da");
            $("#SpanCustGSTMidPrt").hide().text("");

            $("#GSTLastPrt").css('border-color', "#ced4da");
            $("#SpancustGST").hide().text("");
            $("#hdnGstFlagValid").val("NotValidation");
        }
        else {
            //$("#hdnGstFlagValid").val("Validation");
            $("#hdnGstFlagValid").val("NotValidation");
            $("#GSTMidPrt").attr("disabled", false);
            var GstApplicable = $("#Hdn_GstApplicable").text();
           // if (GstApplicable == "Y") {
                //$("#GSTNumrequired").css("display", "inherit");
                $("#GSTNumrequired").hide();
           // }
        }
    }
    else {
        $("#hdnGstFlagValid").val("NotValidation");
        $("#GSTNumrequired").hide();
        $("#GSTMidPrt").attr("disabled", false);
    }



}
function ValidationForPanNoSetAttr() {
    var gst_cat = $("#Gst_Cat").val();
    if (gst_cat == "UR") {
        var placeholder = $("#span_PANNumber").text();
        $("#pan_num").attr("maxlength", "50");
        $("#pan_num").attr("placeholder", `${$("#span_PANNumber").text()}`);
        $("#PanNorequired").css("display", "none");
    }
    else {
        if ($("#Domestic").is(":checked")) {
            suppcurr = "D";
            var CompCountry = $("#hdnCompCountry").val().trim().toLowerCase();
            var CompCountryID = $("#CompCountryID").val();
            if (CompCountry == "india") {
                $("#pan_num").attr("maxlength", "10");
                $("#pan_num").attr("placeholder", "ABCDE1234F");
                $("#PanNorequired").css("display", "");
            }
        }
        else {
            var placeholder = $("#span_PANNumber").text();
            $("#pan_num").attr("maxlength", "50");
            $("#pan_num").attr("placeholder", `${$("#span_PANNumber").text()}`);
            $("#PanNorequired").css("display", "none");
        }
    }

}
function onchangePanNumber() {
    debugger;
    var gst_cat = $("#Gst_Cat").val();
    if (gst_cat == "UR" || gst_cat == "EX" || gst_cat == "CO" || gst_cat == "SZ") {
        $("#GSTMidPrt").val("");
    }
    else {
        var valid = ValidationPanNumber();
        if (valid == true) {
            $("#vmpan_num").text("");
            $("#vmpan_num").css("display", "none");
            $("#pan_num").attr("style", "border-color: #ced4da;");
            var GstApplicable = $("#Hdn_GstApplicable").text();
            if (GstApplicable == "Y") {
                const panInput = $("#pan_num").val().toUpperCase();
                $("#pan_num").val(panInput); // force uppercase in input field
                $("#GSTMidPrt").val(panInput);

                $("#GSTMidPrt").css('border-color', "#ced4da");
                $("#SpanCustGSTMidPrt").attr("style", "display: none;");
                document.getElementById("SpanCustGSTMidPrt").innerHTML = "";

                $("#GSTLastPrt").css('border-color', "#ced4da");
                $("#SpancustGST").attr("style", "display: none;");
                document.getElementById("SpancustGST").innerHTML = "";


                if ($("#tblSupplierAddressDetail > tbody > tr").length > 0) {
                    $("#tblSupplierAddressDetail > tbody > tr").each(function () {
                        const $row = $(this);
                        const gstCell = $row.find("#hdncustgst");
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
    if ($("#Domestic").is(":checked")) {
        var gst_cat = $("#Gst_Cat").val();
        if (gst_cat == "UR") {
            var placeholder = $("#span_PANNumber").text();
            $("#pan_num").attr("maxlength", "50");
            $("#pan_num").attr("placeholder", `${$("#span_PANNumber").text()}`);
            $("#PanNorequired").css("display", "none");
            $("#vmpan_num").text("");
            $("#vmpan_num").css("display", "none");
            $("#pan_num").attr("style", "border-color: #ced4da;");
        }
        else {
            var CompCountry = $("#hdnCompCountry").val().trim().toLowerCase();
            var CompCountryID = $("#CompCountryID").val();
            if (CompCountry == "india") {
                const panInput = $("#pan_num").val().toUpperCase(); // ensure uppercase
                if (panInput == "" || panInput == null) {
                    $("#vmpan_num").text($("#valueReq").text());
                    $("#vmpan_num").css("display", "block");
                    $("#pan_num").css("border-color", "red");
                    ValidationFlag = false;
                }
                else {
                    if (isValidPAN(panInput)) {
                        $("#vmpan_num").text("");
                        $("#vmpan_num").css("display", "none");
                        $("#pan_num").attr("style", "border-color: #ced4da;");
                    }
                    else {
                        $("#vmpan_num").text($("#span_InvalidPANNumber").text() + ' (' + 'ABCDE1234F' + ')');
                        $("#vmpan_num").css("display", "block");
                        $("#pan_num").css("border-color", "red");
                        ValidationFlag = false;
                    }
                }

            }
            else {
                $("#vmpan_num").text("");
                $("#vmpan_num").css("display", "none");
                $("#pan_num").attr("style", "border-color: #ced4da;");
            }
        }

    }
    else {
        $("#vmpan_num").text("");
        $("#vmpan_num").css("display", "none");
        $("#pan_num").attr("style", "border-color: #ced4da;");
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


