$(document).ready(function () {
    debugger;
    $('#ddl_branch').multiselect();
    Bindacc_name();
    Bindacc_group();
});
function Bindacc_name() {
    $("#acc_name").select2({
        ajax: {
            url: $("#GLListName").val(),
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
}
function Bindacc_group() {
    $("#acc_group").select2({
        ajax: {
            url: $("#GLAccGrp").val(),
            data: function (params) {
                var queryParameters = {
                    ddlGroup: params.term,
                    Group: params.page
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
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
            var today = fromDate.getFullYear() + '-' + day + '-' + month;
            $("#txtFromdate").val(today);

            swal("", $("#fromtodatemsg").text(), "warning");
        }
    }
    if (ToDate != "") {
        validatefydate(FromDate, ToDate);
    }
}
function FilterPQListData() {
    debugger;
    try {
        var Flag = "N";
        var rpt_as = $("#GLShowAs option:selected").val();
        var acc_name = $("#acc_name option:selected").val();
        var acc_group = $("#acc_group option:selected").val();
        var acc_type = $("#acc_type option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var curr = $("#curr").val();
        var br_list = $("#ddl_branch").val();

        var brid_list = cmn_multibranchlist(br_list);

        if (brid_list == '0' || brid_list == "" || brid_list == null) {
            return false;
        }

        if (rpt_as == "MW") {
            if (acc_name == "0" || acc_name == null || acc_name == "") {
                $("#vmacc_name").text($("#valueReq").text());
                $("#vmacc_name").css("display", "block");
                $("[aria-labelledby='select2-acc_name-container']").css("border-color", "red");
                hideLoader();/*Add by Hina sharma on 19-11-2024 */
                return false;
            }
            else {
                $("#vmacc_name").css("display", "none");
                $("[aria-labelledby='select2-acc_name-container']").css("border-color", "#ced4da");
                showLoader();/*Add by Hina sharma on 19-11-2024 */
            }
        }


        //showLoader();
        if (Fromdate != "" && Fromdate != null) {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/GeneralLedger/SearchGeneralLedgerDetail",
                data: {
                    acc_id: acc_name,
                    acc_group: acc_group,
                    acc_type: acc_type,
                    curr: curr,
                    Fromdate: Fromdate,
                    Todate: Todate,
                    Rpt_As: rpt_as,
                    brlist: brid_list
                },
                success: function (data) {
                    //showLoader();
                    debugger;
                    if (rpt_as == "TW") {
                        $('#div_GLtble').html(data);
                        var hdncurr_srch = $("#hdncurr_srch").text();
                        $("#curr_Name").val(hdncurr_srch);
                        //$("#hdnPDFPrint").val("Print");
                        $("a.btn.btn-default.buttons-print.btn-sm").remove();
                        $("a.btn.btn-default.buttons-pdf.buttons-html5.btn-sm").remove();
                        $(".dt-buttons").append('<button class="btn btn-default buttons-pdf buttons-html5 btn-sm" onclick="GeneralLedgerPrint()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>Print</span></button>')
                        //$(".dt-buttons").append('<button class="btn btn-default buttons-print btn-sm" onclick="GeneralLedgerPrint()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>Print</span></button>')

                        //$("#hdnCSVPrint").val("CsvPrint");
                        $("a.btn.btn-default.buttons-csv.buttons-html5.btn-sm").remove();
                        $(".dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvData" onclick="trialbalanceCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

                        $("#div_GLtble").css("display", "block")
                        $("#div_GLtbleMon").css("display", "none")
                    }
                    if (rpt_as == "MW") {
                        $('#div_GLtbleMon').html(data);
                        $("#div_GLtble").css("display", "none")
                        $("#div_GLtbleMon").css("display", "block")
                    }
                    //hideLoader();/*Add by Hina sharma on 19-11-2024 */
                },
                error: function OnError(xhr, errorType, exception) {
                    debugger;
                }
            });
        }
        else {
            swal("", $("#fromtodatemsg").text(), "warning");
        }
    } catch (err) {
        debugger;
        console.log("Grn Error : " + err.message);

    }
}
function GeneralLedgerPrint() {
    debugger;
    var arr = [];

    var rpt_as = $("#GLShowAs option:selected").val();
    var acc_name = $("#acc_name option:selected").val();
    var acc_group = $("#acc_group option:selected").val();
    var acc_type = $("#acc_type option:selected").val();
    var Fromdate = $("#txtFromdate").val();
    var Todate = $("#txtTodate").val();
    var curr = $("#curr").val();
    var br_list = $("#ddl_branch").val();
    var brid_list = "";
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

    arr.push({
        acc_id: acc_name,
        acc_group: acc_group,
        acc_type: acc_type,
        curr: curr,
        Fromdate: Fromdate,
        Todate: Todate,
        Rpt_As: rpt_as,
        brid_list: brid_list
    });

    var array = JSON.stringify(arr);
    $("#GLPrintData").val(array);
    $("#hdnCSVPrint").val(null);
    $("#hdnPDFPrint").val("Print");
    $('form').submit();

}
function trialbalanceCSV() {
    debugger;
    var arr = [];
    var rpt_as = $("#GLShowAs option:selected").val();
    var acc_name = $("#acc_name option:selected").val();
    var acc_group = $("#acc_group option:selected").val();
    var acc_type = $("#acc_type option:selected").val();
    var Fromdate = $("#txtFromdate").val();
    var Todate = $("#txtTodate").val();
    var curr = $("#curr").val();

    arr.push({
        acc_id: acc_name,
        acc_group: acc_group,
        acc_type: acc_type,
        curr: curr,
        Fromdate: Fromdate,
        Todate: Todate,
        Rpt_As: rpt_as
    });

    var array = JSON.stringify(arr);
    $("#GLPrintData").val(array);

    var br_list = $("#ddl_branch").val();

    var brid_list = cmn_multibranchlist(br_list);

    if (brid_list == '0' || brid_list == "" || brid_list == null) {
        return false;
    }

    $("#hdnBrList").val(brid_list);
    $("#hdnPDFPrint").val(null);
    $("#hdnCSVPrint").val("CsvPrint");
    var searchValue = $("#datatable-buttons5_filter input[type=search]").val();
    $("#searchValue").val(searchValue);

    $('form').submit();
}
function validatefydate(FromDate, ToDate) { /*Add changes by Hina on 14-08-2024 for show current date of fin Year */
    debugger
    var validate = "N";
    var finfromdt = "";
    var finTodt = "";
    var fin_fromdt = "";
    var fin_Todt = "";
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
        var fin_dt = fdate.replaceAll("/", "");

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
        //$("#txtFromdate").val(finfromdt);
        //$("#txtTodate").val(finTodt);
    }
    else {
        $("#txtTodate").val(ToDate);
    }
}
function OnChnage_AccName() {
    debugger;
    var showAs = $("#GLShowAs").val();/*Add by Hina on 19-11-2024 */
    if (showAs == "MW") {
        var acc_name = $("#acc_name option:selected").val();
        if (acc_name == "0" || acc_name == null || acc_name == "") {
            $("#vmacc_name").text($("#valueReq").text());
            $("#vmacc_name").css("display", "block");
            $("[aria-labelledby='select2-acc_name-container']").css("border-color", "red");
        }
        else {
            $("#HdnAccName").val($("#acc_name option:selected").text());
            $("#vmacc_name").css("display", "none");
            $("[aria-labelledby='select2-acc_name-container']").css("border-color", "#ced4da");
        }
    }
    else {
        $("#vmacc_name").css("display", "none");
        $("[aria-labelledby='select2-acc_name-container']").css("border-color", "#ced4da");
    }
}
function OnchangeCurrency() {
    var curr = $("#curr option:selected").text();
    $("#curr_Name").val(curr);

}
function onchangeShowAs() {
    debugger;
    var showAs = $("#GLShowAs").val();
    if (showAs == "TW") {

        //$("#acc_name").val("0").trigger('change');
        //$('#acc_name').val("0").prop('selected', true);

        $("#spanstr").css("display", "none");
        $("#div_GLtble").css("display", "block");
        $("#div_GLtbleMon").css("display", "none");
        $("#vmacc_name").css("display", "none");/*Add by Hina on 19-11-2024 */
        $("[aria-labelledby='select2-acc_name-container']").css("border-color", "#ced4da");/*Add by Hina on 19-11-2024 */

    } else if (showAs == "MW") {

        //$("#acc_name").val("0").trigger('change');

        $("#spanstr").removeAttr("style");
        $("#div_GLtble").css("display", "none")
        $("#div_GLtbleMon").css("display", "block")
    }
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
    Cmn_GetVouDetails(vou_no, voudt,vou_dt,cflag,narr);
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
