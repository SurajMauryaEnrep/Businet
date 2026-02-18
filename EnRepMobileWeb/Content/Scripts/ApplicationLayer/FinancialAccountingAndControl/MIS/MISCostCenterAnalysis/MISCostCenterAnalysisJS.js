/*
 Created By : Suraj Maurya
 Created On : 08-10-2024
 Description : To Show Cost Center Details Analysis
 */

function onChangeCcName() {
    if (CheckVallidation("txtCcId", "span_CcId") == false) {
        return false;
    }
    bindCostCenterValue();
}
function onChangeCcValName() {
    if (CheckVallidation("txtCcValId", "span_CcValId") == false) {
        return false;
    }
}
function onChangeFromDt() {
    if (CheckVallidation("txtFromdate", "span_Fromdate") == false) {

    }
}
function onChangeToDt() {
    if (CheckVallidation("txtTodate", "span_Todate") == false) {

    }
}
function bindCostCenterValue() {
    try {


        let cc_id = $("#txtCcId").val();
        var Flag = "N";
        debugger
        if (CheckVallidation("txtCcId", "span_CcId") == false) {
            Flag = 'Y';
        }
        if (Flag == "Y") {
            return false
        }
        else {
            showLoader();
            try {
                $.ajax({
                    type: "POST",
                    url: "/ApplicationLayer/MISCostCenterAnalysis/GetCostCenterValueList",
                    data: {
                        cc_id: cc_id
                    },
                    success: function (data) {
                        debugger;
                        var arr = JSON.parse(data)
                        $("#txtCcValId").html(`<option value="0">---Select---</option>`);
                        arr.map((item) => {
                            $("#txtCcValId").append(`<option value="${item.cc_val_id}">${item.cc_val_name}</option>`)
                        })
                        hideLoader();

                    },
                    error: function OnError(xhr, errorType, exception) {
                        hideLoader();
                    }
                });
            } catch (err) {
                debugger;
                hideLoader();
                console.log("Cost Center Analysis Error : " + err.message);
            }
        }

    }
    catch (ex) {
        console.log(ex);
        hideLoader();
    }
}
function SearchCostCenterExpRevDetails() {
    try {
        

        let cc_id = $("#txtCcId").val();
        let cc_val_id = $("#txtCcValId").val();
        let from_dt = $("#txtFromdate").val();
        let to_dt = $("#txtTodate").val();
        let val_dgt = $("#ValDigit").text();
        var Flag = "N";

        debugger
        if (CheckVallidation("txtCcId", "span_CcId") == false) {
            Flag = 'Y';
        }

        if (CheckVallidation("txtCcValId", "span_CcValId") == false) {
            Flag = 'Y';
        }

        if (CheckVallidation("txtFromdate", "span_Fromdate") == false) {
            Flag = 'Y';
        }

        if (CheckVallidation("txtTodate", "span_Todate") == false) {
            Flag = 'Y';
        }
        if (Flag == "Y") {
            hideLoader();/*Add by Hina sharma on 19-11-2024 */
            return false
        }
        else {
            showLoader();
            try {
                $.ajax({
                    type: "POST",
                    url: "/ApplicationLayer/MISCostCenterAnalysis/GetCostCenterAnalysisReport",
                    data: {
                        cc_id: cc_id, cc_val_id: cc_val_id, from_dt: from_dt, to_dt: to_dt
                    },
                    success: function (data) {
                        debugger;

                        $('#Div_CostCenterExpRevDetail').html(data);
                        let expence = $("#spn_TotalExpence").text();
                        let revenue = $("#spn_TotalRevenue").text();
                        $("#spn_TotalExpence").text(cmn_addCommas(parseFloat(CheckNullNumber(expence)).toFixed(val_dgt)));
                        $("#spn_TotalRevenue").text(cmn_addCommas(parseFloat(CheckNullNumber(revenue)).toFixed(val_dgt)));
                        
                        hideLoader();

                    },
                    error: function OnError(xhr, errorType, exception) {
                        hideLoader();
                    }
                });
            } catch (err) {
                debugger;
                hideLoader();
                console.log("Cost Center Analysis Error : " + err.message);
            }
        }

    }
    catch (ex) {
        console.log(ex);
        hideLoader();
    }
}
function onClickTransactionDetails(evt) {
    try {

        let clickedRow = $(evt.target).closest('tr');
        let acc_id = clickedRow.find("#hdn_acc_id").val();
        let cc_id = $("#txtCcId").val();
        let cc_val_id = $("#txtCcValId").val();
        let from_dt = $("#txtFromdate").val();
        let to_dt = $("#txtTodate").val();
        var Flag = "N";

        debugger
        if (CheckVallidation("txtCcId", "span_CcId") == false) {
            Flag = 'Y';
        }

        if (CheckVallidation("txtCcValId", "span_CcValId") == false) {
            Flag = 'Y';
        }

        if (CheckVallidation("txtFromdate", "span_Fromdate") == false) {
            Flag = 'Y';
        }

        if (CheckVallidation("txtTodate", "span_Todate") == false) {
            Flag = 'Y';
        }
        if (Flag == "Y") {
            return false
        }
        else {
            showLoader();
            try {
                $.ajax({
                    type: "POST",
                    url: "/ApplicationLayer/MISCostCenterAnalysis/GetCostCenterTransactionDetails",
                    data: {
                        cc_id: cc_id, cc_val_id: cc_val_id, from_dt: from_dt, to_dt: to_dt, acc_id: acc_id
                    },
                    success: function (data) {
                        debugger;

                        $('#div_PopUpMISCostCenterTransactionDetail').html(data);
                        cmn_apply_datatable("#TblTransactionDetails");

                        //$("a.btn.btn-default.buttons-csv.buttons-html5").remove();
                        //$(".dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 Csv" id="CsvData" onclick="AccountRecevablCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                        hideLoader();

                    },
                    error: function OnError(xhr, errorType, exception) {
                        hideLoader();
                    }
                });
            } catch (err) {
                debugger;
                hideLoader();
                console.log("Cost Center Analysis Error : " + err.message);
            }
        }

    }
    catch (ex) {
        console.log(ex);
        hideLoader();
    }
}
function AccountRecevablCSV() {
    var arr = [];
    $("#TblTransactionDetails >tbody >tr").each(function () {
        var currentRow = $(this);
        var list = {};
        list.vou_no = currentRow.find("#hdn_vouno").val();
        list.vou_dt = currentRow.find("#spn_voudt").text();
        list.cc_vou_amt_bs = currentRow.find("#cc_vou_amt_bs").text();
        list.cc_vou_amt_sp = currentRow.find("#cc_vou_amt_sp").text();
        list.amt_type = currentRow.find("#amt_type").text();
        list.curr_logo = currentRow.find("#curr_logo").text();
        list.conv_rate = currentRow.find("#conv_rate").text();
        list.narr = currentRow.find("#narr").text();
        arr.push(list);
    });
    var array = JSON.stringify(arr);
    $("#cc_TransactionDetail").val(array);

    $("#hdnCSVPrint").val("CsvPrint");
    var searchValue = $("#TblTransactionDetails_filter input[type=search]").val();
    $("#searchValue").val(searchValue);

    $('form').submit();
}

function OnClickVoucherDetailsIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");

    var vou_no = clickedrow.find("#hdn_vouno").val();
    var voudt = clickedrow.find("#spn_voudt").text();
    var vou_dt = clickedrow.find("#hdn_voudt").val();
    //var narr = clickedrow.find("#td_narr").text();

    var vochinfo = vou_no.split("_");
    if (vochinfo.length > 1) {
        var cflag = vochinfo[1];
        vou_no = vochinfo[0]
    }

    Cmn_GetVouDetails(vou_no, voudt, vou_dt, cflag, '');
}

//OnclickCCIconBtn : modified by Suraj maurya on 11-10-2024 to add parseFloat(CheckNullNumber(cmn_ReplaceCommas
function OnclickCCIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var vou_no = $("#GLVoucherNo").val();
    var vou_dt = $("#mwhdn_voudt").val();
    var acc_id = clickedrow.find("#hdn_accid").val();
    var acc_name = clickedrow.find("#spn_glname").text();
    var dr_amt = clickedrow.find("#spn_dramt").text();
    var cr_amt = clickedrow.find("#spn_cramt").text();
    var amt = 0;
    if (parseFloat(CheckNullNumber(cmn_ReplaceCommas(dr_amt))) > 0) { 
        amt = cmn_ReplaceCommas(dr_amt);
    }
    if (parseFloat(CheckNullNumber(cmn_ReplaceCommas(cr_amt))) > 0) {
        amt = cmn_ReplaceCommas(cr_amt);
    }
    Cmn_GetcostcenterDetails(vou_no, vou_dt, acc_id, acc_name, amt);
}
