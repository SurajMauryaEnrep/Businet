$(document).ready(function () {
    $('#ddl_branch').multiselect();
    });
function GetProfitAndLoss_Details() {
    debugger;
    showLoader();
    var from_dt = $("#txtFromdate").val();
    var to_dt = $("#txtTodate").val();
    var br_list = $("#ddl_branch").val();
    var brid_list = "";
    var Flag = "N";
    var rpttype = $("#ddl_RptType").val();;

    if (br_list != null && br_list != "") {
        if (br_list.length > 0) {
            for (var i = 0; i < br_list.length; i++) {
                if (brid_list == "") {
                    brid_list = br_list[i];
                }
                else {
                    brid_list = brid_list + "," + br_list[i];
                }
            }
        }
    }
    else {
        brid_list = "";
    }

    if ( from_dt == "" || from_dt == null) {
        $("#txtFromdate").prop("style", "border-color: #ff0000;");

        $("#span_fromdt").text($("#valueReq").text());
        $("#span_fromdt").css("display", "block");
        Flag = 'Y';
    }
    else {
        $("#txtFromdate").prop("style", "border-color: #ced4da;");
        $("#span_fromdt").css("display", "none");
    }
    if (brid_list == '0' || brid_list == "" || brid_list == null) {
        $(".btn-group .multiselect").prop("style", "border-color: #ff0000;");

        $("#span_br").text($("#valueReq").text());
        $("#span_br").css("display", "block");
        Flag = 'Y';
    }
    else {
        $(".btn-group .multiselect").prop("style", "border-color: #ced4da;");
        $("#span_br").css("display", "none");
    }

    if (Flag == "Y") {
        return false
    }
    else {
        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/ProfitAndLossStatement/SearchProfitAndLossDetail",
                data: {
                    from_dt: from_dt, to_dt: to_dt, br_list: brid_list, rpt_type: rpttype,
                },
                success: function (data) {
                    debugger;

                    $('#div_profitloss').html(data);
                    $("#div_PLGeneratePDF").css("display", "");
                    hideLoader();/*Add by Hina sharma on 19-11-2024 */

                },
                error: function OnError(xhr, errorType, exception) {
                    hideLoader();
                }
            });
        } catch (err) {
            debugger;
            hideLoader();
            console.log("Profit and loss Statement Error : " + err.message);
        }
    }
}

function FromDateValidation() {
    var FromDate = $("#txtFromdate").val();
    var ToDate = $("#txtTodate").val();

    if (FromDate == "" || FromDate == null) {
        $("#txtFromdate").prop("style", "border-color: #ff0000;");

        $("#span_fromdt").text($("#valueReq").text());
        $("#span_fromdt").css("display", "block");
        Flag = 'Y';
    }
    else {
        $("#txtFromdate").prop("style", "border-color: #ced4da;");
        $("#span_fromdt").css("display", "none");

        validatefydate(FromDate, ToDate);
    }
}

function validatefydate(FromDate, ToDate) {
    debugger
    var validate = "N";
    $("#tbl_hdnfylist tbody tr").each(function () {
        var curr_row = $(this);
        var fystdt = curr_row.find("#fystdt").text();
        var fyenddt = curr_row.find("#fyenddt").text();

        var currdate = new Date();
        var fin_date = new Date(fystdt);
        var year = fin_date.getFullYear();
        var cyear = currdate.getFullYear();

        var fstdate = fystdt.replaceAll("-", "");
        var fenddate = fyenddt.replaceAll("-", "");
        var frmdate = FromDate.replaceAll("-", "");

        var yyyy = currdate.getFullYear().toString();
        var mm = (currdate.getMonth() + 1).toString(); // getMonth() is zero-based
        var dd = currdate.getDate().toString();
        var fdate = yyyy + "/" + (mm[1] ? mm : "0" + mm[0]) + "/" + (dd[1] ? dd : "0" + dd[0]);
        var fin_currdt = fdate.replaceAll("/", "-");
        //var fin_dt = fdate.replaceAll("/", "");

        var boolfromdt = "N";
        var booltodt = "N";

        if ((Date.parse(fystdt) <= Date.parse(FromDate)) && (Date.parse(fyenddt) >= Date.parse(FromDate))) {
            boolfromdt = "Y";
            finfromdt = fystdt;
            finTodt = fyenddt;
            document.getElementById("txtTodate").setAttribute('min', FromDate);
            if (year == cyear) {
                document.getElementById("txtTodate").setAttribute('max', fin_currdt);
                $("#txtTodate").val(fin_currdt);
            }
            else {
                document.getElementById("txtTodate").setAttribute('max', fyenddt);
                $("#txtTodate").val(fyenddt);
            }

        }
        if ((Date.parse(fystdt) <= Date.parse(ToDate)) && (Date.parse(fyenddt) >= Date.parse(ToDate))) {
            booltodt = "Y";
        }
        if (boolfromdt == "Y" && booltodt == "Y") {
            validate = "Y";
            fin_fromdt = fystdt;
            fin_Todt = fyenddt;

            if (fstdate <= frmdate && fenddate >= frmdate) {
                document.getElementById("txtTodate").setAttribute('min', "");
                document.getElementById("txtTodate").setAttribute('max', "");

                document.getElementById("txtTodate").setAttribute('min', FromDate);
                if (year == cyear) {
                    document.getElementById("txtTodate").setAttribute('max', fin_currdt);
                    $("#txtTodate").val(fin_currdt);
                }
                else {
                    document.getElementById("txtTodate").setAttribute('max', fyenddt);
                    $("#txtTodate").val(fyenddt);
                }
            }
            else {
                document.getElementById("txtTodate").setAttribute('min', "");
                document.getElementById("txtTodate").setAttribute('max', "");

                document.getElementById("txtTodate").setAttribute('min', fystdt);
                document.getElementById("txtTodate").setAttribute('max', fyenddt);
                $("#txtTodate").val(fyenddt);
            }
            return;
        }
    });

    if (validate != "Y") {
    }
    else {
        $("#txtTodate").val(ToDate);
    }
}
function get_glvoucherdata(evt) {
    debugger;
    var clickedrow = $(evt.target).closest("tr");
    var docid = $("#DocumentMenuId").val();
    var acc_id = clickedrow.find("#hdn_acc_id").val();
    var from_dt = $("#txtFromdate").val();
    var to_dt = $("#txtTodate").val();

    if (acc_id != "" && acc_id != null) {
        try {
            $.ajax({
                type: "POST",
                url: "/Common/Common/Cmn_GetGL_VoucherDetail",
                data: {
                    from_dt: from_dt, to_dt: to_dt, acc_id: acc_id, doc_id: docid,
                },
                success: function (data) {
                    debugger;
                    $("#hdn_acc_id_forCSV").val(acc_id);
                    $('#div_gldetails_popup').html(data);
                    //$("#datatable-buttons5_wrapper a.btn.btn-default.buttons-csv.buttons-html5").remove();
                    //$("#datatable-buttons5_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="ProfitAndLossCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                    hideLoader();

                },
                error: function OnError(xhr, errorType, exception) {
                    hideLoader();
                }
            });
        } catch (err) {
            debugger;
            hideLoader();
            console.log("Profit and loss Statement Voucher Detail Error : " + err.message);
        }
    }
}
function ProfitAndLossCSV() {
    debugger;
    var acc_id = $("#hdn_acc_id_forCSV").val();
    var from_dt = $("#txtFromdate").val();
    var to_dt = $("#txtTodate").val();

    window.location.href = "/ApplicationLayer/ProfitAndLossStatement/ExportDataInExcel?acc_id=" + acc_id + "&from_dt=" + from_dt + "&to_dt=" + to_dt;
}
function OnclickVouDetailsIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");

    var vou_no = clickedrow.find("#hdn_vouno").val();
    var voudt = clickedrow.find("#spn_voudt").text();
    var vou_dt = clickedrow.find("#hdn_voudt").val();
    var narr = clickedrow.find("#td_narr").text();

    var vochinfo = vou_no.split("_");
    if (vochinfo.length > 1) {
        var cflag = vochinfo[1];
        vou_no = vochinfo[0]
    }
    Cmn_GetVouDetails(vou_no, voudt, vou_dt, cflag, narr);
}
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
    if (dr_amt > 0) {
        amt = dr_amt;
    }
    if (cr_amt > 0) {
        amt = cr_amt;
    }
    Cmn_GetcostcenterDetails(vou_no, vou_dt, acc_id, acc_name, amt);
}
function GenerateProfitAndLossPDF() {
    debugger;
    var from_dt = $("#txtFromdate").val();
    var to_dt = $("#txtTodate").val();
    var br_list = $("#ddl_branch").val();
    var brid_list = "";
    var Flag = "N";
    var rpttype = $("#ddl_RptType").val();;

    if (br_list != null && br_list != "") {
        if (br_list.length > 0) {
            for (var i = 0; i < br_list.length; i++) {
                if (brid_list == "") {
                    brid_list = br_list[i];
                }
                else {
                    brid_list = brid_list + "," + br_list[i];
                }
            }
        }
    }
    else {
        brid_list = "";
    }

    if (from_dt == "" || from_dt == null) {
        $("#txtFromdate").prop("style", "border-color: #ff0000;");

        $("#span_fromdt").text($("#valueReq").text());
        $("#span_fromdt").css("display", "block");
        Flag = 'Y';
    }
    else {
        $("#txtFromdate").prop("style", "border-color: #ced4da;");
        $("#span_fromdt").css("display", "none");
    }
    if (brid_list == '0' || brid_list == "" || brid_list == null) {
        $(".btn-group .multiselect").prop("style", "border-color: #ff0000;");

        $("#span_br").text($("#valueReq").text());
        $("#span_br").css("display", "block");
        Flag = 'Y';
    }
    else {
        $(".btn-group .multiselect").prop("style", "border-color: #ced4da;");
        $("#span_br").css("display", "none");
    }

    if (Flag == "Y") {
        return false
    }
    else {
        window.location.href = "/ApplicationLayer/ProfitAndLossStatement/GenratePAndLPdfFile1?RptType=" + rpttype + "&from_dt=" + from_dt + "&to_dt=" + to_dt + "&br_list=" + brid_list
    }
}
