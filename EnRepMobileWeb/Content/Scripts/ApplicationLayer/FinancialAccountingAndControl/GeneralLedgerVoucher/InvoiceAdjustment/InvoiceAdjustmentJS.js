$(document).ready(function () {
    debugger;
    $("#BtnInvoiceDetail").prop("disabled", true);
    var t_length = $("#AdvanceDetailTbl tbody tr").length;
    if (t_length != 0) {
        $("#BtnInvoiceDetail").prop("disabled", false);
    }
    InvAdjNo = $("#InvoiceAdjustmentNo").val();
    $("#hdDoc_No").val(InvAdjNo);
    $("#entityid").select2();
    $("#InvAdjList #datatable-buttons tbody").bind("dblclick", function (e) {
        try {
            debugger;
            var ListFilterData = $("#ListFilterData").val();
            var WF_Status = $("#WF_Status").val();
            var clickedrow = $(e.target).closest("tr");
            var VouNo = clickedrow.children("#VouNumber").text().trim();
            var Voudt = clickedrow.children("#hdVouDate").text().trim();
            if (VouNo != null && VouNo != "") {
                window.location.href = "/ApplicationLayer/InvoiceAdjustment/EditVou/?VouNo=" + VouNo + "&Voudt=" + Voudt + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status;
            }
        }
        catch (err) {
            debugger
        }
    });
})

/*---------------------------List Page---------------------*/
function BtnSearch() {
    debugger;
    try {
        var EntityType = $("#EntityType").val();
        var EntityName = $("#entityid").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val().trim();

        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/InvoiceAdjustment/SearchInvoiceAdjDetail",
            data: {
                entity_type: EntityType,
                entity_id: EntityName,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#InvAdjList').html(data);
                $('#ListFilterData').val(EntityType + ',' + EntityName + ',' + Fromdate + ',' + Todate + ',' + Status);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("MTO Error : " + err.message);

    }
    ResetWF_Level();
}

function FromToDateValidation() {
    debugger;
    var FromDate = $("#txtFromdate").val();
    var ToDate = $("#txtTodate").val();
    //$("#hdFromdate").val(FromDate)
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
            debugger;
            var fromDate = new Date($("#hdFromdate").val());

            var month = (fromDate.getMonth() + 1);
            var day = fromDate.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = fromDate.getFullYear() + '-' + month + '-' + day;
            $("#txtFromdate").val(today);
            swal("", $("#fromtodatemsg").text(), "warning");
        }
    }

}
function CmnGetWorkFlowDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Vou_No = clickedrow.children("#VouNumber").text();
    var Vou_Date = clickedrow.children("#hdVouDate").text();
    var Doc_id = $("#DocumentMenuId").val();
    $("#hdDoc_No").val(Vou_No);
    var Doc_Status = clickedrow.children("#Doc_Status").text();
    Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
    GetWorkFlowDetails(Vou_No, Vou_Date, Doc_id, Doc_Status);
    var a = 1;
}
function ResetWF_Level() {
    var li_count = $("#wizard ul li").length;
    if (li_count > 0) {
        for (var y = 0; y < li_count; y++) {
            var id = parseInt(y) + 1;
            $("#a_" + id).removeClass("done");
            $("#a_" + id).removeClass("selected");
            $("#a_" + id).addClass("disabled");
        }
    }
}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = clickedrow.children("#VouNumber").text();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
}


/*---------------------------List Page---------------------*/
var ValDecDigit = $("#ValDigit").text();
function OnChangeEntityType() {
    debugger;
    var EntityType = $("#EntityType").val();
    if (EntityType == "" || EntityType == "0") {
        //document.getElementById("vmEntityType").innerHTML = $("#valueReq").text();
        //$("#EntityType").css("border-color", "red");
        //$("#vmEntityType").css("display", "block");
        var s = '<option value="0">---Select---</option>';
        $("#entityid").html(s);
    }
    else {
        $("#vmEntityType").css("display", "none");
        $("#EntityType").css("border-color", "#ced4da");
    }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/InvoiceAdjustment/GetEntity",
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async: true,
        data: { EntityType: EntityType, },/*Registration pass value like model*/
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
                var s = '<option value="0">---Select---</option>';
                for (var i = 0; i < arr.length; i++) {
                    s += '<option value="' + arr[i].id + '">' + arr[i].name + '</option>';
                }
                $("#entityid").html(s);

            }
        },
        error: function (Data) {
        }
    });
}
function OnChangeEntity() {
    debugger;
    var RateDecDigit = $("#RateDigit").text()
    var entityid = $("#entityid").val();
    if (entityid == "" || entityid == "0") {
        $('#vmentity').text($("#valueReq").text());
        $("#vmentity").css("display", "block");
        $("[aria-labelledby='select2-entityid-container']").css("border-color", "red");
    }
    else {
        $("#vmentity").css("display", "none");
        $("[aria-labelledby='select2-entityid-container']").css("border-color", "#ced4da");

    }

}
function FilterBillsDetails() {
    debugger;
    try {
        var entity_name = $("#entityid option:selected").val();
        var entity_type = $("#EntityType").val();
        var TransType = $("#TransType").val();
        var DocumentStatus = $("#DocumentStatus").val();
        var Command = $("#Command").val();
        var Flag = "N";
        if (entity_type == "" || entity_type == "0") {
            document.getElementById("vmEntityType").innerHTML = $("#valueReq").text();
            $("#EntityType").css("border-color", "red");
            $("#vmEntityType").css("display", "block");
            Flag = "Y";
        }
        if (entity_name == "" || entity_name == "0") {
            $('#vmentity').text($("#valueReq").text());
            $("#vmentity").css("display", "block");
            $("[aria-labelledby='select2-entityid-container']").css("border-color", "red");
            Flag = "Y";
        }
        if (Flag == "Y") {
            return false;
        }
        //else {
        //    $("#BtnSearch").css("display", "none");
        //    $("#EntityType").prop("disabled", true);
        //    $("#entityid").prop("disabled", true);
        //}

        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/InvoiceAdjustment/SearchBillsDetail",
            data: {
                entity_id: entity_name, entity_type: entity_type,
            },
            success: function (data) {
                debugger;
                $('#DivAdv_Bill_Detail').html(data);

                var t_length = $("#AdvanceDetailTbl tbody tr").length;
                var t_length1 = $("#BillDetailTbl tbody tr").length;
                if (t_length == 0) {
                    $("#BtnSearch").css("display", "block");
                    $("#EntityType").prop("disabled", false);
                    $("#entityid").prop("disabled", false);
                    return false;
                }
                else if (t_length1 == 0) {
                    $("#BtnSearch").css("display", "block");
                    $("#EntityType").prop("disabled", false);
                    $("#entityid").prop("disabled", false);
                    return false;
                }
                else {
                    $("#BtnSearch").css("display", "none");
                    $("#EntityType").prop("disabled", true);
                    $("#entityid").prop("disabled", true);
                    $("#BtnInvoiceDetail").prop("disabled", false);
                }
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("Grn Error : " + err.message);

    }
}

function OnClickAdvCheck(e) {
    debugger;
    var check = "";
    var CurrentRow = $(e.target).closest("tr");
    var RemAmt = cmn_ReplaceCommas(CurrentRow.find("#AdvRemBal").val());
    //var Payamount = cmn_ReplaceCommas(CurrentRow.find("#AdvAdjAmount").val());
    var PendingAmt = cmn_ReplaceCommas(CurrentRow.find("#HdAdvUnAdjAmt").val());
    if (CurrentRow.find("#AdvCheck").is(":checked")) {
        check = 'Y';
    }
    else {
        check = 'N';
    }
    if (check == 'Y') {
        CurrentRow.find("#AdvCheck").prop("checked", true);
        CurrentRow.find("#AdvAdjAmount").val(cmn_addCommas(toDecimal(PendingAmt, ValDecDigit)));
        var RemaingBal = (parseFloat(PendingAmt) - parseFloat(RemAmt));
        if (parseFloat(RemAmt) != 0) {
            CurrentRow.find("#AdvRemBal").val(cmn_addCommas(toDecimal(RemaingBal, ValDecDigit)));
        }
    }
    else {
        CurrentRow.find("#AdvCheck").prop("checked", false);
        CurrentRow.find("#AdvAdjAmount").val(parseFloat(0).toFixed(ValDecDigit));
        CurrentRow.find("#AdvRemBal").val(cmn_addCommas(toDecimal(PendingAmt, ValDecDigit)));
    }
    //if (check == 'Y') {
    //    CurrentRow.find("#AdvCheck").prop("checked", true);
    //  //  var PendingAmt1 = parseFloat(PendingAmt).toFixed(ValDecDigit);
    //    //var PendingAmt1 = toDecimal(PendingAmt, ValDecDigit);
    //    CurrentRow.find("#AdvAdjAmount").val(cmn_addCommas(toDecimal(PendingAmt, ValDecDigit)));
    //    //CurrentRow.find("#AdvAdjAmount").val(parseFloat(PendingAmt).toFixed(ValDecDigit));
    //    var RemaingBal = (parseFloat(PendingAmt) - parseFloat(RemAmt));
    //  //  var RemaingBal1 = parseFloat(RemaingBal).toFixed(ValDecDigit);
    //    //var RemaingBal1 = toDecimal(RemaingBal, ValDecDigit);
    //    CurrentRow.find("#AdvRemBal").val(cmn_addCommas(toDecimal(RemaingBal, ValDecDigit)));
    //    //CurrentRow.find("#AdvRemBal").val(parseFloat(RemaingBal).toFixed(ValDecDigit));
    //}
    //else {
    //    CurrentRow.find("#AdvCheck").prop("checked", false);
    //    CurrentRow.find("#AdvAdjAmount").val(parseFloat(0).toFixed(ValDecDigit));
    //   // var PendingAmt1 = parseFloat(PendingAmt).toFixed(ValDecDigit);
    //    //var PendingAmt1 = toDecimal(PendingAmt, ValDecDigit);
    //    CurrentRow.find("#AdvRemBal").val(cmn_addCommas(toDecimal(PendingAmt, ValDecDigit)));
    //    //CurrentRow.find("#AdvRemBal").val(parseFloat(PendingAmt).toFixed(ValDecDigit));
    //}
    CalculateAdvTotalAmt();
}
function OnChangeAdjAmount(e) {
    debugger;
    var DocId = $("#DocumentMenuId").val();
    var ValDecDigit = $("#ValDigit").text();
    var currentrow = $(e.target).closest('tr');
    var PayAmount = currentrow.find("#AdvAdjAmount").val();
    var PendingAmt = currentrow.find("#HdAdvUnAdjAmt").val();
    if (AvoidDot(PayAmount) == false) {
        PayAmount = "";
        currentrow.find("#AdvAdjAmount").val("")
    }
    else {
       // currentrow.find("#AdvAdjAmount").val(parseFloat(PayAmount).toFixed(ValDecDigit))
        currentrow.find("#AdvAdjAmount").val(toDecimal(cmn_ReplaceCommas(PayAmount), ValDecDigit))
    }
    var PendingAmt = currentrow.find("#AdvRemBal").val();
    // var RemainingAmt = currentrow.find("#AdvRemBal").val();
    debugger;
    if (PayAmount != "") {
        if (parseFloat(PayAmount) != parseFloat(0)) {
            var RemAmt = parseFloat(PendingAmt) - parseFloat(PayAmount);
            currentrow.find("#AdvAdjAmount").css("border-color", "#ced4da");
            //var test = parseFloat(PayAmount).toFixed(ValDecDigit);
            var test = toDecimal(cmn_ReplaceCommas(PayAmount), ValDecDigit);
            currentrow.find("#AdvAdjAmount").val(test);
            currentrow.find("#AdvRemBal").val(parseFloat(RemAmt).toFixed(ValDecDigit));

        }
        else {
            var test = parseFloat(0).toFixed(ValDecDigit);
            currentrow.find("#AdvAdjAmount").val(test);
            currentrow.find("#AdvRemBal").val(parseFloat(PendingAmt).toFixed(ValDecDigit));
        }
    }
    else {
        var test = parseFloat(0).toFixed(ValDecDigit);
        currentrow.find("#AdvAdjAmount").val(test);
        currentrow.find("#AdvRemBal").val(parseFloat(PendingAmt).toFixed(ValDecDigit));
    }
    CalculateAdvTotalAmt();
}
function CalculateAdvTotalAmt() {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var TotalAmount = parseFloat(0).toFixed(ValDecDigit);
    $("#AdvanceDetailTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        var PayAmount = cmn_ReplaceCommas(CurrentRow.find("#AdvAdjAmount").val());
        if (PayAmount != null && PayAmount != "") {
            TotalAmount = parseFloat(TotalAmount) + parseFloat(PayAmount);
        }
    });
 //  var TotalAmount1 = parseFloat(TotalAmount).toFixed(ValDecDigit)
    //var TotalAmount1 = toDecimal(TotalAmount, ValDecDigit);
    $("#AdvTotal").text(cmn_addCommas(toDecimal(TotalAmount, ValDecDigit)));
    //$("#AdvTotal").text(parseFloat(TotalAmount).toFixed(ValDecDigit));
}
function OnClickAllAdvCheck() {
    debugger;
    var Payamount = 0;
    var PendingAmt = 0;
    var Allcheck = "";
    if ($("#AllAdvCheck").is(":checked")) {
        Allcheck = 'Y';
    }
    else {
        Allcheck = 'N';
    }

    if (Allcheck == 'Y') {
        $("#AdvanceDetailTbl tbody tr").each(function () {
            debugger;
            var CurrentRow = $(this);
            RemAmt = cmn_ReplaceCommas(CurrentRow.find("#AdvRemBal").val());
            PendingAmt = cmn_ReplaceCommas(CurrentRow.find("#HdAdvUnAdjAmt").val());
            CurrentRow.find("#AdvCheck").prop("checked", true);
            CurrentRow.find("#AdvCheck").is(":checked")
            check = 'Y';
            if (check == 'Y') {
              //  var PendingAmt1 = parseFloat(PendingAmt).toFixed(ValDecDigit);
                var PendingAmt1 = toDecimal(cmn_ReplaceCommas(PendingAmt), ValDecDigit);
                CurrentRow.find("#AdvAdjAmount").val(cmn_addCommas(PendingAmt1));
                //CurrentRow.find("#AdvAdjAmount").val(parseFloat(PendingAmt).toFixed(ValDecDigit));
                Payamount = cmn_ReplaceCommas(CurrentRow.find("#AdvAdjAmount").val());
                var RemaingBal = (parseFloat(PendingAmt) - parseFloat(Payamount));
                //var RemaingBal1 = parseFloat(RemaingBal).toFixed(ValDecDigit)
                var RemaingBal1 = toDecimal(cmn_ReplaceCommas(RemaingBal), ValDecDigit);
                CurrentRow.find("#AdvRemBal").val(cmn_addCommas(RemaingBal1));
            }
        });
    }
    else {
        $("#AdvanceDetailTbl tbody tr").each(function () {
            debugger;
            var CurrentRow = $(this);
            CurrentRow.find("#AdvCheck").prop("checked", false);
            PendingAmt = cmn_ReplaceCommas(CurrentRow.find("#HdAdvUnAdjAmt").val());
            if (CurrentRow.find("#AdvCheck").is(":checked")) {
                check = 'Y';
            }
            else {
                check = 'N';
            }
            if (check == 'N') {

                CurrentRow.find("#AdvCheck").prop("checked", false);
                CurrentRow.find("#AdvAdjAmount").val(parseFloat(0).toFixed(ValDecDigit));
              //  var PendingAmt1 = parseFloat(PendingAmt).toFixed(ValDecDigit)
                var PendingAmt1 = toDecimal(cmn_ReplaceCommas(PendingAmt), ValDecDigit);
                CurrentRow.find("#AdvRemBal").val(cmn_addCommas(PendingAmt1));
            }
        });
    }
    CalculateAdvTotalAmt();
}

function OnClickBillCheck(e) {
    debugger;
    var check = "";
    var CurrentRow = $(e.target).closest("tr");
    $("#BillDetailTbl tbody tr").each(function () {
        debugger;
        var RemAmt = cmn_ReplaceCommas(CurrentRow.find("#InvRemBal").val());
        var PendingAmt = cmn_ReplaceCommas(CurrentRow.find("#InvUnAdjAmt").val());

        if (CurrentRow.find("#BillCheck").is(":checked")) {
            check = 'Y';
        }
        else {
            check = 'N';
        }
        if (check == 'Y') {
            CurrentRow.find("#BillCheck").prop("checked", true);
            //var PendingAmt1 = parseFloat(PendingAmt).toFixed(ValDecDigit);
            var PendingAmt1 = toDecimal(cmn_ReplaceCommas(PendingAmt), ValDecDigit);
            CurrentRow.find("#InvAdjAmt").val(cmn_addCommas(PendingAmt1));
            //CurrentRow.find("#InvAdjAmt").val(parseFloat(PendingAmt).toFixed(ValDecDigit));
            var Payamount = cmn_ReplaceCommas(CurrentRow.find("#InvAdjAmt").val());
            var RemaingBal = (parseFloat(PendingAmt) - parseFloat(Payamount));
            //var RemaingBal1 = parseFloat(RemaingBal).toFixed(ValDecDigit);
            var RemaingBal1 = toDecimal(cmn_ReplaceCommas(RemaingBal), ValDecDigit);
            CurrentRow.find("#InvRemBal").val(cmn_addCommas(RemaingBal1));
        }
        else {
            CurrentRow.find("#BillCheck").prop("checked", false);
            CurrentRow.find("#InvAdjAmt").val(parseFloat(0).toFixed(ValDecDigit));
           // var PendingAmt1 = parseFloat(PendingAmt).toFixed(ValDecDigit);
            var PendingAmt1 = toDecimal(cmn_ReplaceCommas(PendingAmt), ValDecDigit);
            CurrentRow.find("#InvRemBal").val(cmn_addCommas(PendingAmt1));
            //CurrentRow.find("#InvRemBal").val(parseFloat(PendingAmt).toFixed(ValDecDigit));
        }

    });
    OnChangeBillAdjAmount(e);
    //CalculateAdvTotalAmt();
}
function OnChangeBillAdjAmount(e) {
    debugger;
    var DocId = $("#DocumentMenuId").val();
    var ValDecDigit = $("#ValDigit").text();
    var currentrow = $(e.target).closest('tr');
    var PendingAmt = cmn_ReplaceCommas(currentrow.find("#InvUnAdjAmt").val());
    var PayAmount = cmn_ReplaceCommas(currentrow.find("#InvAdjAmt").val());
    if (AvoidDot(PayAmount) == false) {
        PayAmount = "";
        currentrow.find("#InvAdjAmt").val("")
    }
    else {
        //var PayAmount1 = parseFloat(PayAmount).toFixed(ValDecDigit);
        var PayAmount1 = toDecimal(cmn_ReplaceCommas(PayAmount), ValDecDigit);
        currentrow.find("#InvAdjAmt").val(cmn_addCommas(PayAmount1));
        //currentrow.find("#InvAdjAmt").val(parseFloat(PayAmount).toFixed(ValDecDigit))
    }
    var InvRemAmt = cmn_ReplaceCommas(currentrow.find("#InvRemBal").val());
    // var RemainingAmt = currentrow.find("#AdvRemBal").val();
    debugger;
    if (PayAmount != "") {
        if (parseFloat(PayAmount) != parseFloat(0)) {
            if (parseFloat(PayAmount) > parseFloat(PendingAmt)) {
                debugger;
                currentrow.find("#PayAmount_Error").text($("#ExceedingAmount").text());
                currentrow.find("#PayAmount_Error").css("display", "block");
                currentrow.find("#InvAdjAmt").css("border-color", "red");
            }
            else {
                var RemAmt = parseFloat(PendingAmt) - parseFloat(PayAmount)
                currentrow.find("#PayAmount_Error").css("display", "none");
                currentrow.find("#InvAdjAmt").css("border-color", "#ced4da");
               // var test = parseFloat(PayAmount).toFixed(ValDecDigit);
                var test = toDecimal(cmn_ReplaceCommas(PayAmount), ValDecDigit);
                currentrow.find("#InvAdjAmt").val(cmn_addCommas(test));
               // var RemAmt1 = parseFloat(RemAmt).toFixed(ValDecDigit)
                var RemAmt1 = toDecimal(cmn_ReplaceCommas(RemAmt), ValDecDigit);
                currentrow.find("#InvRemBal").val(cmn_addCommas(RemAmt1));
                //currentrow.find("#InvRemBal").val(parseFloat(RemAmt).toFixed(ValDecDigit));
            }
        }
        else {
            var test = parseFloat(0).toFixed(ValDecDigit);
            currentrow.find("#InvAdjAmt").val(test);
            //var PendingAmt1 = parseFloat(PendingAmt).toFixed(ValDecDigit);
            var PendingAmt1 = toDecimal(cmn_ReplaceCommas(PendingAmt), ValDecDigit);
            currentrow.find("#InvRemBal").val(cmn_addCommas(PendingAmt1));
            //currentrow.find("#InvRemBal").val(parseFloat(PendingAmt).toFixed(ValDecDigit));
        }
    }
    else {
        var test = parseFloat(0).toFixed(ValDecDigit);
        currentrow.find("#InvAdjAmt").val(test);
        //var PendingAmt1 = parseFloat(PendingAmt).toFixed(ValDecDigit);
        var PendingAmt1 = toDecimal(cmn_ReplaceCommas(PendingAmt), ValDecDigit);
        currentrow.find("#InvRemBal").val(cmn_addCommas(PendingAmt1));
        currentrow.find("#PayAmount_Error").css("display", "none");/*Add By Hina on 14-08-2024 for remove border color amd msg*/
        currentrow.find("#InvAdjAmt").css("border-color", "#ced4da");
        //currentrow.find("#InvRemBal").val(parseFloat(PendingAmt).toFixed(ValDecDigit));
    }
    CalculateInvTotalAmt();
}

function OnChangeAdvAdjAmount(e) {
    debugger;
    
    var DocId = $("#DocumentMenuId").val();
    var ValDecDigit = $("#ValDigit").text();
    var currentrow = $(e.target).closest('tr');
    var PendingAmt = cmn_ReplaceCommas(currentrow.find("#HdAdvUnAdjAmt").val());
    var PayAmount = cmn_ReplaceCommas(currentrow.find("#AdvAdjAmount").val());
    if (AvoidDot(PayAmount) == false) {
        PayAmount = "";
        currentrow.find("#AdvAdjAmount").val("")
    }
    else {
        //var PayAmount2 = parseFloat(PayAmount).toFixed(ValDigit);
        var PayAmount2 = toDecimal(cmn_ReplaceCommas(PayAmount), ValDigit);
        var PayAmount1 = cmn_addCommas(PayAmount2);
        currentrow.find("#AdvAdjAmount").val(PayAmount1);
        //currentrow.find("#AdvAdjAmount").val(parseFloat(PayAmount).toFixed(ValDecDigit))
    }
    var InvRemAmt = currentrow.find("#AdvRemBal").val();
    debugger;
    if (PayAmount != "") {
        if (parseFloat(PayAmount) != parseFloat(0)) {
            if (parseFloat(PayAmount) > parseFloat(PendingAmt)) {
                debugger;
                currentrow.find("#AdvAmount_Error").text($("#ExceedingAmount").text());
                currentrow.find("#AdvAmount_Error").css("display", "block");
                currentrow.find("#AdvAdjAmount").css("border-color", "red");
                
            }
            else {
                var RemAmt = parseFloat(PendingAmt) - parseFloat(PayAmount)
                currentrow.find("#AdvAmount_Error").css("display", "none");
                currentrow.find("#AdvAdjAmount").css("border-color", "#ced4da");
                //var test = parseFloat(PayAmount).toFixed(ValDecDigit);
                var test = toDecimal(cmn_ReplaceCommas(PayAmount), ValDecDigit);
                currentrow.find("#AdvAdjAmount").val(cmn_addCommas(test));
                //var RemAmt1 = parseFloat(RemAmt).toFixed(ValDecDigit);
                var RemAmt1 = toDecimal(cmn_ReplaceCommas(RemAmt), ValDecDigit);
                currentrow.find("#AdvRemBal").val(cmn_addCommas(RemAmt1));
            }
        }
        else {
            var test = parseFloat(0).toFixed(ValDecDigit);
            currentrow.find("#AdvAdjAmount").val(test);
           // var PendingAmt1 = parseFloat(PendingAmt).toFixed(ValDecDigit);
            var PendingAmt1 = toDecimal(cmn_ReplaceCommas(PendingAmt), ValDecDigit);
            currentrow.find("#AdvRemBal").val(cmn_addCommas(PendingAmt1));
        }
    }
    else {
        var test = parseFloat(0).toFixed(ValDecDigit);
        currentrow.find("#AdvAdjAmount").val(test);
      //  var PendingAmt1 = parseFloat(PendingAmt).toFixed(ValDecDigit);
        var PendingAmt1 = toDecimal(cmn_ReplaceCommas(PendingAmt), ValDecDigit);
        currentrow.find("#AdvRemBal").val(cmn_addCommas(PendingAmt1));
        currentrow.find("#AdvAmount_Error").css("display", "none");/*Add by Hina on 14-08-2024*/
        currentrow.find("#AdvAdjAmount").css("border-color", "#ced4da");
        //currentrow.find("#AdvRemBal").val(parseFloat(PendingAmt).toFixed(ValDecDigit));
    }
    CalculateAdvTotalAmt();
    
}

function CalculateAdvTotalAmt() {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var TotalAmount = parseFloat(0).toFixed(ValDecDigit);
    $("#AdvanceDetailTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        var PayAmount = cmn_ReplaceCommas(CurrentRow.find("#AdvAdjAmount").val());
        if (PayAmount != null && PayAmount != "") {
            TotalAmount = parseFloat(TotalAmount) + parseFloat(PayAmount);
        }
    });
    //var Total = parseFloat(TotalAmount).toFixed(ValDecDigit);
    var Total = toDecimal(cmn_ReplaceCommas(TotalAmount), ValDecDigit);
    $("#AdvTotal").text(cmn_addCommas(Total));
    //$("#AdvTotal").text(parseFloat(TotalAmount).toFixed(ValDecDigit));
}
function CalculateInvTotalAmt() {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var TotalAmount = parseFloat(0).toFixed(ValDecDigit);
    $("#BillDetailTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        var PayAmount = cmn_ReplaceCommas(CurrentRow.find("#InvAdjAmt").val());
        if (PayAmount != null && PayAmount != "") {
            TotalAmount = parseFloat(TotalAmount) + parseFloat(PayAmount);
        }
    });
   // var total = parseFloat(TotalAmount).toFixed(ValDecDigit)
    var total = toDecimal(cmn_ReplaceCommas(TotalAmount), ValDecDigit);
    $("#InvTotal").text(cmn_addCommas(total));
    //$("#InvTotal").text(parseFloat(TotalAmount).toFixed(ValDecDigit));
}
function OnClickAllBillCheck() {
    debugger;
    var Payamount = 0;
    var PendingAmt = 0;
    var Allcheck = "";
    if ($("#AllBillCheck").is(":checked")) {
        Allcheck = 'Y';
    }
    else {
        Allcheck = 'N';
    }

    if (Allcheck == 'Y') {
        $("#BillDetailTbl tbody tr").each(function () {
            debugger;
            var CurrentRow = $(this);
            var RemAmt = cmn_ReplaceCommas(CurrentRow.find("#InvRemBal").val());
            var PendingAmt = cmn_ReplaceCommas(CurrentRow.find("#InvUnAdjAmt").val());
            CurrentRow.find("#BillCheck").prop("checked", true);
            CurrentRow.find("#BillCheck").is(":checked")
            check = 'Y';
            if (check == 'Y') {
                //CurrentRow.find("#BillCheck").prop("checked", true);
                //var PendingAmt1 = parseFloat(PendingAmt).toFixed(ValDecDigit)
                var PendingAmt1 = toDecimal(cmn_ReplaceCommas(PendingAmt), ValDecDigit);
                CurrentRow.find("#InvAdjAmt").val(cmn_addCommas(PendingAmt1));
                //CurrentRow.find("#InvAdjAmt").val(parseFloat(PendingAmt).toFixed(ValDecDigit));
                Payamount = cmn_ReplaceCommas(CurrentRow.find("#InvAdjAmt").val());
                var RemaingBal = (parseFloat(PendingAmt) - parseFloat(Payamount));
               // var RemaingBal1 = parseFloat(RemaingBal).toFixed(ValDecDigit);
                var RemaingBal1 = toDecimal(cmn_ReplaceCommas(RemaingBal), ValDecDigit);
                CurrentRow.find("#InvRemBal").val(cmn_addCommas(RemaingBal1));
                //CurrentRow.find("#InvRemBal").val(parseFloat(RemaingBal).toFixed(ValDecDigit));
            }
        });
    }
    else {
        //CurrentRow.find("#BillCheck").prop("checked", false);

        $("#BillDetailTbl tbody tr").each(function () {
            debugger;
            var CurrentRow = $(this);
            CurrentRow.find("#BillCheck").prop("checked", false);
            var PendingAmt = cmn_ReplaceCommas(CurrentRow.find("#InvUnAdjAmt").val());
            var PayAmt = cmn_ReplaceCommas(CurrentRow.find("#InvAdjAmt").val());
            if (CurrentRow.find("#BillCheck").is(":checked")) {
                check = 'Y';
            }
            else {
                check = 'N';
            }
            if (check == 'N') {
                CurrentRow.find("#BillCheck").prop("checked", false);
                CurrentRow.find("#InvAdjAmt").val(parseFloat(0).toFixed(ValDecDigit));
              //  var PendingAmt1 = parseFloat(PendingAmt).toFixed(ValDecDigit);
                var PendingAmt1 = toDecimal(cmn_ReplaceCommas(PendingAmt), ValDecDigit);
                CurrentRow.find("#InvRemBal").val(cmn_addCommas(PendingAmt1));
                //CurrentRow.find("#InvRemBal").val(parseFloat(PendingAmt).toFixed(ValDecDigit));
            }
        });
    }

    CalculateInvTotalAmt();
}
function AmountFloatVal(el, evt) {
    //debugger
    if (Cmn_FloatValueonly(el, evt, "#ValDigit") == false) {
        return false;
    }
}
function OnClickInvFifoAdj() {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    //var AutopayAmt = parseFloat($("#AdvTotal").text()).toFixed(ValDecDigit);
    var AutopayAmt = toDecimal(cmn_ReplaceCommas($("#AdvTotal").text()), ValDecDigit);
    var PayAmt = parseFloat(0).toFixed(ValDecDigit);
    $("#BillDetailTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        debugger;
        var PendingAmt = cmn_ReplaceCommas(CurrentRow.find("#InvUnAdjAmt").val());
        if (parseFloat(AutopayAmt) > parseFloat(PendingAmt)) {
            //CurrentRow.find("#InvAdjAmt").val(parseFloat(PendingAmt).toFixed(ValDecDigit));
            CurrentRow.find("#InvAdjAmt").val(cmn_addCommas(toDecimal(PendingAmt, ValDecDigit)));
            AutopayAmt = parseFloat(AutopayAmt) - parseFloat(PendingAmt);
        }
        else {
            // CurrentRow.find("#InvAdjAmt").val(parseFloat(AutopayAmt).toFixed(ValDecDigit));
            CurrentRow.find("#InvAdjAmt").val(cmn_addCommas(toDecimal(AutopayAmt, ValDecDigit)));
            AutopayAmt = parseFloat(AutopayAmt) - parseFloat(AutopayAmt);
        }

        var Payamount = cmn_ReplaceCommas(CurrentRow.find("#InvAdjAmt").val());
        var RemaingBal = (parseFloat(PendingAmt) - parseFloat(Payamount))
       // CurrentRow.find("#InvRemBal").val(parseFloat(RemaingBal).toFixed(ValDecDigit));
        CurrentRow.find("#InvRemBal").val(cmn_addCommas(toDecimal(RemaingBal, ValDecDigit)));
    });
    CalculateInvTotalAmt();
}
function InsertInvAdjDetail() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 10-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }


    if (HeaderValidation() == false) {
        return false;
    }
    if (CheckAdvancePaymentValidation() == false) {/*Add By Hina sharma on 14-08-2024 */
        return false
    }
    if (CheckBillDetailValidation() == false) {
        return false
    }
    if (Adv_Bills_EqualAmnt_Valid() == false) {
        return false;
    }
    
    var FinalAdvanceDetail = [];
    FinalAdvanceDetail = InsertAdvanceDetails();
    var AdvanceDt = JSON.stringify(FinalAdvanceDetail);
    $('#hdAdvanceDetail').val(AdvanceDt);

    var FinalBillWiseAdjDetail = [];
    FinalBillWiseAdjDetail = InsertBillWiseAdjDetails();
    var BillWiseAdjDt = JSON.stringify(FinalBillWiseAdjDetail);
    $('#hdBillWiseAdjDetail').val(BillWiseAdjDt);
    debugger;
    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
    return true;
};
function HeaderValidation() {
    debugger;
    var entity_name = $("#entityid option:selected").val();
    var entity_type = $("#EntityType").val();
    $("#hdEntityType").val(entity_type);
    $("#hdentityid").val(entity_name);

    var Flag = "N";
    if (entity_type == "" || entity_type == "0") {
        document.getElementById("vmEntityType").innerHTML = $("#valueReq").text();
        $("#EntityType").css("border-color", "red");
        $("#vmEntityType").css("display", "block");
        Flag = "Y";
    }
    if (entity_name == "" || entity_name == "0") {
        $('#vmentity').text($("#valueReq").text());
        $("#vmentity").css("display", "block");
        $("[aria-labelledby='select2-entityid-container']").css("border-color", "red");
        Flag = "Y";
    }
    if (Flag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckAdvancePaymentValidation() {/*Add By Hina on 14-08-2024 for Validation On Save cz adjusted amount should not be greater than unadjustment amount*/
    debugger;
    var FlagValid = "";
    $("#AdvanceDetailTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        var PendingAmt = cmn_ReplaceCommas(CurrentRow.find("#HdAdvUnAdjAmt").val());
        var PayAmount = cmn_ReplaceCommas(CurrentRow.find("#AdvAdjAmount").val());


        if (PayAmount != "" && PayAmount != null) {
            if (parseFloat(PayAmount) > parseFloat(PendingAmt)) {
                debugger;
                CurrentRow.find("#AdvAmount_Error").text($("#ExceedingAmount").text());
                CurrentRow.find("#AdvAmount_Error").css("display", "block");
                CurrentRow.find("#AdvAdjAmount").css("border-color", "red");
                FlagValid = "Y";
            }
            else {
                CurrentRow.find("#AdvAmount_Error").css("display", "none");
                CurrentRow.find("#AdvAdjAmount").css("border-color", "#ced4da");
            }
        }
    });
    if (FlagValid === "Y") {
        return false;
    }
    else {
        return true;
    }
}
function Adv_Bills_EqualAmnt_Valid() {
    debugger;
    var ErrorFlag = "N";
    var ValDigit = $("#ValDigit").text();
    var AdvTotlAmnt = cmn_ReplaceCommas($("#AdvTotal").text());
    var BillTotAmnt = cmn_ReplaceCommas($("#InvTotal").text());
    debugger;
    if ($("#AdvanceDetailTbl >tbody >tr").length == 0) {
        swal("", $("#AdvancePaymentDetailNotFound").text(), "warning");
        ErrorFlag = "Y";
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    if (AdvTotlAmnt == "" || AdvTotlAmnt == null || AdvTotlAmnt == "NaN") {/*Add By Hina Sharma on 14-08-2024 for should not be save document with 0 value of both tables*/
        AdvTotlAmnt = 0;
    }
    if (BillTotAmnt == "" || BillTotAmnt == null || BillTotAmnt == "NaN") {/*Add By Hina Sharma on 14-08-2024*/
        BillTotAmnt = 0;
    }
    
    if (parseFloat(AdvTotlAmnt).toFixed(ValDigit) == parseFloat(BillTotAmnt).toFixed(ValDigit) && parseFloat(AdvTotlAmnt).toFixed(ValDigit) != parseFloat(0).toFixed(ValDigit) && parseFloat(BillTotAmnt).toFixed(ValDigit) != parseFloat(0).toFixed(ValDigit)) {

    }
    /*Add By Hina Sharma on 14-08-2024*/
    else if (parseFloat(AdvTotlAmnt).toFixed(ValDigit) == parseFloat(0).toFixed(ValDigit) && parseFloat(BillTotAmnt).toFixed(ValDigit) == parseFloat(0).toFixed(ValDigit))
    {
        swal("", $("#AdjustmentValueCanNotBeZero").text(), "warning");
        ErrorFlag = "Y";
    }
    else {
            swal("", $("#Advanceamountmismatchwithinvoiceamount").text(), "warning");
            ErrorFlag = "Y";
        }
   
    
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckBillDetailValidation() {
    debugger;
    //var t_length1 = $("#BillDetailTbl tbody tr").length;
    //if (t_length1 == 0) {
    //    return false;
    //}
    var ValDecDigit = $("#ValDigit").text();
    var status = "N";
    if ($("#BillDetailTbl >tbody >tr").length == 0) {
        swal("", $("#InvoiceDetailNotFound").text(), "warning");
        status = "Y";
    }
    if (status == "Y") {
        return false;
    }
    $("#BillDetailTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        var PayAmount = cmn_ReplaceCommas(CurrentRow.find("#InvAdjAmt").val());
        var PendingAmt = cmn_ReplaceCommas(CurrentRow.find("#InvUnAdjAmt").val());

        if (PayAmount != "" && PayAmount != null) {
            if (parseFloat(PayAmount) > parseFloat(PendingAmt)) {
                debugger;
                CurrentRow.find("#PayAmount_Error").text($("#ExceedingAmount").text());
                CurrentRow.find("#PayAmount_Error").css("display", "block");
                CurrentRow.find("#PayAmount").css("border-color", "red");
                status = "Y";
            }
            else {
                CurrentRow.find("#PayAmount_Error").css("display", "none");
                CurrentRow.find("#PayAmount").css("border-color", "#ced4da");
            }
        }
    });
    if (status === "Y") {
        return false;
    }
    else {
        return true;
    }
}
function InsertAdvanceDetails() {
    debugger;
    var AdvanceDetail = new Array();
    var entity_name = $("#entityid option:selected").val();
    $("#AdvanceDetailTbl TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        //var rowid = currentRow.find("#SNohf").val();
        var AccountList = {};
        AccountList.acc_id = entity_name;
        AccountList.Vou_no = currentRow.find("#VouNumber").val();
        AccountList.Vou_dt = currentRow.find("#VouDate").val();
        AccountList.Curr = currentRow.find("#HdAdvCurr").val();
        AccountList.AdvInvAmtBs = cmn_ReplaceCommas(currentRow.find("#AdvInvAmtBs").val());
        AccountList.AdvInvAmtSp = cmn_ReplaceCommas(currentRow.find("#AdvInvAmtSp").val());
        AccountList.Adv_Un_Adj_Amt = cmn_ReplaceCommas(currentRow.find("#HdAdvUnAdjAmt").val());
        AccountList.Adv_Adj_Amt = cmn_ReplaceCommas(currentRow.find("#AdvAdjAmount").val());
        AccountList.AdvRemBal = cmn_ReplaceCommas(currentRow.find("#AdvRemBal").val());

        AdvanceDetail.push(AccountList);
    });
    return AdvanceDetail;
};
function InsertBillWiseAdjDetails() {
    debugger;
    var BillsDetail = new Array();
    var entity_name = $("#entityid option:selected").val();
    var entity_type = $("#EntityType").val();
    $("#BillDetailTbl TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        var AccountList = {};
        AccountList.acc_id = entity_name;
        AccountList.Inv_no = currentRow.find("#InvoiceNumber").val();
        AccountList.Inv_dt = currentRow.find("#InvoiceDate").val();
        AccountList.Bill_no = currentRow.find("#BillNumber").val();//Added by Suraj on 03-05-2024
        AccountList.Bill_dt = currentRow.find("#BillDate").val();//Added by Suraj on 03-05-2024
        AccountList.Curr = currentRow.find("#HdInvCurr").val();
        AccountList.InvAmtBs = cmn_ReplaceCommas(currentRow.find("#InvAmtBs").val());
        AccountList.InvAmtSp = cmn_ReplaceCommas(currentRow.find("#InvAmtSp").val());
        AccountList.InvUnAdjAmt = cmn_ReplaceCommas(currentRow.find("#InvUnAdjAmt").val());
        AccountList.Inv_Paid_Amt = cmn_ReplaceCommas(currentRow.find("#InvAdjAmt").val());
        AccountList.InvRemBal = cmn_ReplaceCommas(currentRow.find("#InvRemBal").val());

        BillsDetail.push(AccountList);
    });
    return BillsDetail;
};
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
function ForwardBtnClick() {
    debugger;
    //var AdjStatus = "";
    //AdjStatus = $('#hdInvAdjStatus').val();
    //if (AdjStatus === "D" || AdjStatus === "F") {

    //    if ($("#hd_nextlevel").val() === "0") {
    //        $("#Btn_Forward").attr("data-target", "");
    //    }
    //    else {
    //        $("#Btn_Forward").attr("data-target", "#Forward_Pop");
    //        $("#Btn_Approve").attr("data-target", "");
    //    }
    //    var Doc_ID = $("#DocumentMenuId").val();
    //    $("#OKBtn_FW").attr("data-dismiss", "modal");

    //    Cmn_GetForwarderList(Doc_ID);

    //}
    //else {
    //    $("#Btn_Forward").attr("data-target", "");
    //    $("#Btn_Forward").attr('onclick', '');
    //    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //}
    //return false;

    /*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
    //$("#Btn_Approve").attr("data-target", "");
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var Voudt = $("#InvAdjDate").val();
        $.ajax({
            type: "POST",
            url: "/Common/Common/Fin_CheckFinancialYear",
            data: {
                compId: compId,
                brId: brId,
                Voudt: Voudt
            },
            success: function (data) {
                if (data == "FY Exist") { /*End to chk Financial year exist or not*/
                    var AdjStatus = "";
                    AdjStatus = $('#hdInvAdjStatus').val();
                    if (AdjStatus === "D" || AdjStatus === "F") {

                        if ($("#hd_nextlevel").val() === "0") {
                            $("#Btn_Forward").attr("data-target", "");
                        }
                        else {
                            $("#Btn_Forward").attr("data-target", "#Forward_Pop");
                            $("#Btn_Approve").attr("data-target", "");
                        }
                        var Doc_ID = $("#DocumentMenuId").val();
                        $("#OKBtn_FW").attr("data-dismiss", "modal");

                        Cmn_GetForwarderList(Doc_ID);

                    }
                    else {
                        $("#Btn_Forward").attr("data-target", "");
                        $("#Btn_Forward").attr('onclick', '');
                        $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    }
                }
                else {/* to chk Financial year exist or not*/
                    if (data == "FY Not Exist") {
                        swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");
                        //$("#Btn_Forward").attr("data-target", "");
                        $("#Forward_Pop").attr("data-target", "");
                        //$("#Btn_Approve").attr("data-target", "");
                    }
                    else {
                        swal("", $("#BooksAreClosedEntryCanNotBeMadeInThisFinancialYear").text(), "warning");
                        /* $("#Btn_Forward").attr("data-target", "");*/
                        $("#Forward_Pop").attr("data-target", "");
                        //$("#Btn_Approve").attr("data-target", "");
                    }

                }
            }
        });
    }
    catch (ex) {
        console.log(ex);
        return false;
    }
    /*End to chk Financial year exist or not*/
    return false;
}
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var VouNo = "";
    var VouDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status1 = "";
    var ModelData = "";

    docid = $("#DocumentMenuId").val();
    VouNo = $("#InvoiceAdjustmentNo").val();
    VouDate = $("#InvAdjDate").val();
    $("#hdDoc_No").val(VouNo);
    WF_Status1 = $("#WF_Status1").val();
    ModelData = (VouNo + ',' + VouDate + ',' + WF_Status1);
    Remarks = $("#fw_remarks").val();
    var ListFilterData1 = $("#ListFilterData1").val();
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            //docid = currentRow.find("td:eq(2)").text();
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    var pdfAlertEmailFilePath = 'IA_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
    GetPdfFilePathToSendonEmailAlert(VouNo, VouDate, pdfAlertEmailFilePath);

    if (fwchkval === "Forward") {
        if (fwchkval != "" && VouNo != "" && VouDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(VouNo, VouDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            window.location.href = "/ApplicationLayer/InvoiceAdjustment/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }

    debugger;
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/InvoiceAdjustment/InvoiceAdjustmentApprove?VouNo=" + VouNo + "&VouDate=" + VouDate + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1;

    }

    if (fwchkval === "Reject") {
        if (fwchkval != "" && VouNo != "" && VouDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(VouNo, VouDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            window.location.href = "/ApplicationLayer/InvoiceAdjustment/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && VouNo != "" && VouDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(VouNo, VouDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            window.location.href = "/ApplicationLayer/InvoiceAdjustment/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
}
function GetPdfFilePathToSendonEmailAlert(poNo, poDate, fileName) {
    debugger;
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/InvoiceAdjustment/SavePdfDocToSendOnEmailAlert",
        data: { poNo: poNo, poDate: poDate, fileName: fileName },
        /*dataType: "json",*/
        success: function (data) {

        }
    });
}
function OtherFunctions(StatusC, StatusName) {

}
function ForwardHistoryBtnClick() {
    debugger;
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#InvoiceAdjustmentNo").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
function RemoveSession() {
    return true
}

function OnClickCancelFlag() {
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return InsertBPDetail()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}


function approveonclick() { /**Added this Condition by Nitesh 10-01-2024 for Disable Approve btn after one Click**/
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickApprove") {
        $("#btn_approve").attr("disabled", true);
        $("#btn_approve").css("filter", "grayscale(100%)");
    }
    else {
        $("#btn_approve").css("filter", "grayscale(100%)");
        $("#hdnsavebtn").val("AllreadyclickApprove");
    }

}