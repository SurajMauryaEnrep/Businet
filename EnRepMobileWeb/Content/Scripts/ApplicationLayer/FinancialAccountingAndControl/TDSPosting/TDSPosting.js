$(document).ready(function () {
    TdsListDoubleClick();
    OnclickdeleteIcon();
    SetPeriod();
    //$("#TdsPostingDetailsTable thead tr:last-child th").on("click", function () {
    //    sortTable(this.cellIndex, "TdsPostingDetailsTable");
    //});
    if ($("#TdsPostingDetailsTable tbody tr").length > 0) {
        cmn_apply_datatable("#TdsPostingDetailsTable");
    }
    $("#hdDoc_No").val($("#HdnTdsId").val())
   // $(document).ready(function () {
        debugger;
       /* $("#datatable-buttons_wrapper a.btn.btn-default.buttons-csv.buttons-html5.btn-sm").remove();*/
        //$("#datatable-buttons_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="TDSPostingCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
   // });
    let gldt = $("#hdItemvouDetail").val();
    if (gldt != null && gldt != "") {
        sessionStorage.setItem("allSuppGlDetails", gldt);
    } else {
        sessionStorage.removeItem("allSuppGlDetails");
    }
    let tdsSlabdt = $("#TdsSlabDetailsToSave").val();
    if (tdsSlabdt != null && tdsSlabdt != "") {
        sessionStorage.setItem("tds_slabDetail", tdsSlabdt);
    } else {
        sessionStorage.removeItem("tds_slabDetail");
    }
    let tdsInvdt = $("#TdsSuppInvDetailsToSave").val();
    if (tdsInvdt != null && tdsInvdt != "") {
        sessionStorage.setItem("tds_SuppInvDetail", tdsInvdt);
    } else {
        sessionStorage.removeItem("tds_SuppInvDetail");
    }
});
function TDSPostingCSV() {
    debugger;
    const tds_month_val = $("#tds_month option:selected").val();
    const tds_year = $("#tds_year").val();
    const ddlStatus = $("#ddlStatus").val();
    var searchValue = $("#datatable-buttons_filter input[type=search]").val();

    window.location.href = "/ApplicationLayer/TDSPosting/TDSPostingExporttoExcelDt?tds_year=" + tds_year + "&tds_month_val=" + tds_month_val + "&ddlStatus=" + ddlStatus + "&searchValue=" + searchValue;
}
//for List
function TdsListDoubleClick() {


    $("#TdsListTable tr").on("dblclick", function () {
        const CrRow = $(this);
        const Month = CrRow.find("#td_month").text();
        const Year = CrRow.find("#td_year").text();
        const tds_id = CrRow.find("#td_tds_id").text();
        const ListFilterData1 = $("#ListFilterData1").val();
        window.location.href = "/ApplicationLayer/TDSPosting/AddTDSPostingDetail?tds_id=" + tds_id + "&Month=" + Month + "&Year=" + Year + "&ListFilterData1=" + ListFilterData1;
    });
}
//for List
function TDSListSearch() {
    const tds_month_val = $("#tds_month option:selected").val();
    const tds_year = $("#tds_year").val();
    const ddlStatus = $("#ddlStatus").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/TDSPosting/TDSListSearch",
        data: { Year: tds_year, month: tds_month_val, status: ddlStatus },
        success: function (data) {
            debugger
            $("#TDSList_Table").html(data);
            //$("#datatable-buttons_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="TDSPostingCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')

        }
    })
}

//For WorkFlow 
function CmnGetWorkFlowDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var tds_id = clickedrow.children("#td_tds_id").text();
    var tds_dt = clickedrow.children("#td_tds_dt").text();
    var Doc_id = $("#DocumentMenuId").val();
    $("#hdDoc_No").val(tds_id);
    GetWorkFlowDetails(tds_id, tds_dt, Doc_id);
    var a = 1;
}
function ForwardHistoryBtnClick() {
    debugger;
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#BankPaymentNo").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
function ForwardBtnClick() {
    debugger;
    try {
        var BPStatus = "";
        BPStatus = $('#DocumentStatus').val().trim();
        if (BPStatus === "D" || BPStatus === "F") {

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
    catch (ex) {
        console.log(ex);
        return false;
    }
    return false;
}
 function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var tds_id = "";
    var tds_dt = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";

    docid = $("#DocumentMenuId").val();
    tds_id = $("#HdnTdsId").val();
    tds_dt = $("#hdnTdsDate").val();
    WF_Status1 = $("#WF_Status1").val();
    ModelData = (tds_id + ',' + tds_dt + ',' + WF_Status1);
    $("#hdDoc_No").val(tds_id);
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
    //var pdfAlertEmailFilePath = 'BP_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
    //GetPdfFilePathToSendonEmailAlert(tds_id, tds_dt, pdfAlertEmailFilePath);

    if (fwchkval === "Forward") {
        if (fwchkval != "" && tds_id != "" && tds_dt != "" && level != "") {
            debugger;
             Cmn_InsertDocument_ForwardedDetail(tds_id, tds_dt, docid, level, forwardedto, fwchkval, Remarks/*, pdfAlertEmailFilePath*/);
            window.location.reload();
            //window.location.href = "/ApplicationLayer/TDSPosting/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/TDSPosting/ApproveTdsDetails?tds_id=" + tds_id + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && tds_id != "" && tds_dt != "") {
             Cmn_InsertDocument_ForwardedDetail(tds_id, tds_dt, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks/*, pdfAlertEmailFilePath*/);
            window.location.reload();
            //window.location.href = "/ApplicationLayer/TDSPosting/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && tds_id != "" && tds_dt != "") {
             Cmn_InsertDocument_ForwardedDetail(tds_id, tds_dt, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks/*, pdfAlertEmailFilePath*/);
            window.location.reload();
            //window.location.href = "/ApplicationLayer/TDSPosting/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData;
        }
    }
}
function ForwardHistoryBtnClick() {
    debugger;
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#HdnTdsId").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}

function OnchangeMonth() {
    SetPeriod();
    CheckVallidation("tds_month", "vm_month");


}
function SetPeriod() {
    const tds_month = $("#tds_month option:selected").text().split('-')[0];
    const tds_month_val = $("#tds_month option:selected").val();
    const tds_year = $("#tds_year").val();

    $("#hdn_Year").val(tds_year);
    $("#hdn_MonthNo").val(tds_month_val);

    if (tds_year != "0" && tds_month_val != "0") {
        var date = new Date(tds_year, GetMonthNoByName(tds_month), 1);
        debugger
        var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        $("#tds_period").val(firstDay.format("d-m-Y") + " - " + lastDay.format("d-m-Y"));
        $("#from_dt").val(firstDay.format("Y-m-d"));
        $("#to_dt").val(lastDay.format("Y-m-d"));
    }

}

function InsertTdsPostingDetail() {
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 10-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }
    if (ValidateHeader() == false) {
        return false;
    }
    if (ValidateSuppDetail() == false) {
        return false;
    }
    if (ValidateSlabDetail() == false) {
        return false;
    }

    TdsPostingSuppDetailList();
    TdsPostingSuppGLDetail();
    TdsPostingSlabDetailList();
    TdsPostingSuppInvDetailList();
    TdsPostingSuppInvSlabDetailList();

    var FinalCostCntrDetails = [];
    FinalCostCntrDetails = Cmn_InsertCCDetails();
    var CCDetails = JSON.stringify(FinalCostCntrDetails);
    $('#hdn_CC_DetailList').val(CCDetails);

    try {
        if (Cmn_CC_DtlSaveButtonClick("VoucherDetail", "dramtInBase1", "cramtInBase1", "hfPIStatus") == false) {
            return false;
        }
    }
    catch (ex) {
        console.log(ex);
        return false;
    }

    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
    return true;

}
function ValidateHeader() {
    var ErrFlag = false;
    if (CheckVallidation("tds_year", "vm_year") == false) {
        ErrFlag = true;
    }
    if (CheckVallidation("tds_month", "vm_month") == false) {
        ErrFlag = true;
    }
    if (ErrFlag == false) {
        return true;
    } else {
        return false;
    }
}

function ValidateSuppDetail() {

    const countRow = $("#TdsPostingDetailsTable tbody tr").length;
    if (countRow > 0) {
        return true;
    } else {
        swal("", $("#noitemselectedmsg").text(), "warning")
        return false;
    }

}
function ValidateSlabDetail() {


    return true;
}
function OnchangeYear() {
    SetPeriod();
    CheckVallidation("tds_year", "vm_year");
    const tds_year = $("#tds_year").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/TDSPosting/GetMonthOnBehalfYear",
        data: { tds_year },
        success: function (data) {
            if (data != "ErrorPage") {
                var Arr = JSON.parse(data);
                //let checkPending = "N";
                //if (Arr.Table1.length > 0) {
                //    if (Arr.Table1[0].tds_status == "D" || Arr.Table1[0].tds_status == "F") {
                //        checkPending = "Y";
                //    }
                //}
                var s = '<option value="0">---Select---</option>';
                //if (checkPending == "N") {
                    for (var i = 0; i < Arr.Table.length; i++) {
                        s += '<option value="' + Arr.Table[i].month_no + '">' + Arr.Table[i].month_name + '</option>';
                    }
                //}
                $("#tds_month").html(s)
            }
        }

    })

}
function GetMonthNoByName(monthName) {

    var arr = ["January", "February", "March", "April", "May", "June", "July", "August", "September"
        , "October", "November", "December"];
    return arr.indexOf(monthName);

}
function OnclickAddTdsPostingDetail() {
    if (ValidateHeader() == false) {
        return false;
    } else {
        const tds_month = $("#tds_month").val();
        const tds_year = $("#tds_year").val();
        const from_dt = $("#from_dt").val();
        const to_dt = $("#to_dt").val();

        $("#tds_month").attr("disabled", true);
        $("#tds_year").attr("disabled", true);

        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/TDSPosting/GetTdsPosting",
            data: { tds_month: tds_month, tds_year: tds_year, from_dt: from_dt, to_dt: to_dt },
            success: function (data) {
                if (data != "ErrorPage") {
                    var Arr = JSON.parse(data);
                    $("#AddTdsDetailIcon").css("display", "none");
                    debugger;
                    $("#TdsPostingDetailsTable tbody tr").remove();
                    const th_period = $("#tds_period").val();
                    $("#th_Period").text(th_period);
                    if (Arr.Table1.length > 0) {
                        $("#th_Pre_Period").text(Arr.Table1[0].pre_period);
                        $("#PreValueStartDate").text(Arr.Table1[0].st_dt);
                        $("#PreValueEndDate").text(Arr.Table1[0].end_dt);
                    }
                    $("#CurrValueStartDate").text(from_dt);
                    $("#CurrValueEndDate").text(to_dt);
                    var AllSuppGlDetails = [];
                    var suppGlDetails = [];
                    var slabDtl = [];
                    var vou_sr_no = 0;
                    var vou_tds_month = $("#tds_month option:selected").text();
                    for (var i = 0; i < Arr.Table.length; i++) {
                        //if (parseFloat(cmn_ReplaceCommas(Arr.Table[i].tds_payble)) > 0) {//for tds payble > 0
                        $("#TdsPostingDetailsTable tbody").append(`
                            <tr>
                                <td class="red center sorting_1"> <i class="fa fa-trash deleteIcon" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                                <td id="SrNo">${i + 1}</td>
                                <td hidden id="supp_id">${Arr.Table[i].supp_id}</td>
                                <td hidden id="hd_pre_net_val_bs">${Arr.Table[i].pre_net_val_bs}</td>
                                <td hidden id="hd_net_val_bs">${Arr.Table[i].net_val_bs}</td>
                                <td hidden id="hd_total_value">${Arr.Table[i].total_value}</td>
                                <td hidden id="hd_taxable_value">${Arr.Table[i].taxable_value}</td>
                                <td hidden id="hd_tds_amt">${Arr.Table[i].net_val_bs}</td>
                                <td hidden id="hd_tds_payble">${Arr.Table[i].tds_payble}</td>
                                <td hidden id="supp_acc_id">${Arr.Table[i].supp_acc_id}</td>
                                <td hidden id="supp_acc_name">${Arr.Table[i].supp_acc_name}</td>
                                <td id="supp_name" class="ItmNameBreak itmStick tditemfrz">${Arr.Table[i].supp_name}</td>
                                <td id="country_name">${Arr.Table[i].country_name}</td>
                                <td id="state_name">${Arr.Table[i].state_name}</td>
                                <td id="supp_pan_no">${Arr.Table[i].supp_pan_no}</td>
                                <td id="supp_gst_no">${Arr.Table[i].supp_gst_no}</td>                                
                                <td class="num_right">
                                <div class="col-sm-10">
                                <span id="pre_net_val_bs">${Arr.Table[i].pre_net_val_bs}</span>
                                </div>
                                <div class="col-sm-2 i_Icon ">
                                <button type="button" id="" class="calculator" onclick="OnClickPreValue(event)" data-toggle="modal" data-target="#TdsPreviousBalanceInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_TDSInformation").text()}"> </button>
                                </div>
                                </td>                                
                                <td class="num_right">
                                <div class="col-sm-10">
                                <span id="net_val_bs">${Arr.Table[i].net_val_bs}</span>
                                </div>
                                <div class="col-sm-2 i_Icon ">
                                <button type="button" id="" class="calculator" onclick="OnClickCurrValue(event)" data-toggle="modal" data-target="#TdsPreviousBalanceInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_TDSInformation").text()}"> </button>
                                </div>
                                </td>
                                <td class="num_right" hidden ></td>
                                <td class="num_right" id="total_value">${Arr.Table[i].total_value}</td>                                
                                <td class="num_right">
                                <div class="col-sm-10">
                                <span id="taxable_value">${Arr.Table[i].taxable_value}</span>
                                </div>
                                <div class="col-sm-2 i_Icon ">
                                <button type="button" id="" class="calculator" onclick="OnClickTaxableValue(event)" data-toggle="modal" data-target="#TdsTaxableValue" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_TDSInformation").text()}"> </button>
                                </div>
                                </td>                                
                                <td class="num_right" id="tds_amount" hidden>${Arr.Table[i].net_val_bs}</td>
                                <td class="num_right">
                                    <div class="col-sm-10">
                                        <span id="tds_payble">${Arr.Table[i].tds_payble}</span>
                                    </div>
                                    <div class="col-sm-2 i_Icon ">
                                        <button type="button" id="" class="calculator" onclick="OnClickTdsPaybleIcon(event);" data-toggle="modal" data-target="#TdsPaybleSlabInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_TDSInformation").text()}"> </button>
                                    </div>
                                </td>
                                <td id="td_VouNo"></td>
                                <td id="td_VouDate"></td>
                                <td id="" class="center">
                                <div class="col-sm-6 i_Icon ">${parseFloat(CheckNullNumber(Arr.Table[i].tds_payble)) > 0 ?`<button type="button" id="" onclick="Onclick_TdsVouInfo(event)" class="calculator subItmImg" data-toggle="modal" data-target="#TdsVoucherInfo" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="Voucher Detail"></i></button>`:""}
                                
                                </div>
                                <div class="col-sm-6 i_Icon ">
                                   
                                </div>
                                </td>
                            </tr>

`)
                        OnclickdeleteIcon();
                        if (parseFloat(cmn_ReplaceCommas(Arr.Table[i].tds_payble)) > 0) {
                            vou_sr_no = vou_sr_no + 1;
                            slabDtl = Arr.Table2.filter(t => t.supp_id == Arr.Table[i].supp_id);
                            suppGlDetails = CreateGLForSupp(Arr.Table[i].supp_id, Arr.Table[i].supp_acc_id, Arr.Table[i].supp_acc_name, Arr.Table[i].tds_payble, slabDtl, vou_sr_no, Arr.Table[i].supp_name, Arr.Table[i].taxable_value, vou_tds_month );
                            suppGlDetails.map((item) => {
                                AllSuppGlDetails.push(item);
                            });
                        }
                    }
                    sessionStorage.setItem("allSuppGlDetails", JSON.stringify(AllSuppGlDetails));

                    cmn_apply_datatable("#TdsPostingDetailsTable");

                    //var PaybleSuppList = Arr.Table.filter(t => parseFloat(cmn_ReplaceCommas(t.tds_payble)) > 0); //for tds payble > 0
                    //var SlabDetail = Arr.Table2.filter(t => PaybleSuppList.findIndex(j => j.supp_id == t.supp_id) > -1); //for tds payble > 0
                    sessionStorage.setItem("tds_slabDetail", JSON.stringify(Arr.Table2));
                    sessionStorage.setItem("tds_SuppInvDetail", JSON.stringify(Arr.Table3));
                    sessionStorage.setItem("tds_SuppInvSlabDetail", JSON.stringify(Arr.Table4));
                    //sessionStorage.setItem("tds_slabDetail", JSON.stringify(SlabDetail)); //for tds payble > 0

                    
                    //for (var i = 0; i < Arr.Table2.length; i++) {
                    //    $("#table_tds_slabDetail tbody").append(`<tr>
                    //                        <td id="supp_id">${Arr.Table2[i].supp_id}</td>
                    //                        <td id="tds_amt">${Arr.Table2[i].tds_amt}</td>
                    //                        <td id="tds_perc">${Arr.Table2[i].tds_perc}</td>
                    //                        <td id="tds_payble">${Arr.Table2[i].tds_payble}</td>
                    //                    </tr>`)

                    //}
                } else {
                    console.log(data);
                }
            }

        });
    }

}
function OnclickdeleteIcon() {
    $(".deleteIcon").on("click", function () {
        var Crow = $(this).closest("tr");
        const supp_id = Crow.find("#supp_id").text();
        Crow.remove();
        debugger
        

        //$("#table_tds_slabDetail tbody tr #supp_id:contains('" + supp_id + "')").closest('tr').remove();
        let slabDetail = sessionStorage.getItem("tds_slabDetail");
        if (slabDetail != null && slabDetail != "") {
            slabDetail = JSON.parse(slabDetail);
            let newslabDetail = slabDetail.filter(t => t.supp_id != supp_id);
            sessionStorage.setItem("tds_slabDetail", JSON.stringify(newslabDetail));
        }
        /*------------------- tds supplier invoice details ---------------*/
        let SuppInvDetail = sessionStorage.getItem("tds_SuppInvDetail");
        if (SuppInvDetail != null && SuppInvDetail != "") {
            SuppInvDetail = JSON.parse(SuppInvDetail);
            let newslabDetail = SuppInvDetail.filter(t => t.supp_id != supp_id);
            sessionStorage.setItem("tds_SuppInvDetail", JSON.stringify(newslabDetail));
        }
        let SuppInvSlabDetail = sessionStorage.getItem("tds_SuppInvSlabDetail");//invoice wise slab details
        if (SuppInvSlabDetail != null && SuppInvSlabDetail != "") {
            SuppInvSlabDetail = JSON.parse(SuppInvSlabDetail);
            let newslabDetail = SuppInvSlabDetail.filter(t => t.supp_id != supp_id);
            sessionStorage.setItem("tds_SuppInvSlabDetail", JSON.stringify(newslabDetail));
        }
        /*------------------- tds supplier invoice details end---------------*/
        var table = $('#TdsPostingDetailsTable').DataTable();
        table.rows(function (idx, data, node) {
            return data[2] == supp_id; // index of unique id column
        }).remove().draw(false);

        $("#TdsPostingDetailsTable tbody tr").each(function (index) {
            $(this).closest("tr").find("#SrNo").text(index + 1);
        });
        //if ($("#TdsPostingDetailsTable tbody tr").length == 0) {
        //    $("#AddTdsDetailIcon").css("display", "block");
        //    $("#tds_year").attr("disabled", false);
        //    $("#tds_month").attr("disabled", false);
        //}

    });
}

function TdsPostingSuppDetailList() {
    var arr = [];
    //$("#TdsPostingDetailsTable tbody tr").each(function (index) {
    //    var Crow = $(this).closest("tr");//
    //    var list = {};
    //    list.supp_id = Crow.find("#supp_id").text();
    //    //list.pre_val = Crow.find("#pre_net_val_bs").text();
    //    var pre_val = Crow.find("#pre_net_val_bs").text();
    //    list.pre_val = cmn_ReplaceCommas(pre_val);
    //    //list.curr_mnth_val = Crow.find("#net_val_bs").text();
    //    var curr_mnth_val = Crow.find("#net_val_bs").text();
    //    list.curr_mnth_val = cmn_ReplaceCommas(curr_mnth_val);
    //    //list.tot_val = Crow.find("#total_value").text();
    //    var tot_val = Crow.find("#total_value").text();
    //    list.tot_val = cmn_ReplaceCommas(tot_val);
    //    //list.taxable_val = Crow.find("#taxable_value").text();
    //    var taxable_val = Crow.find("#taxable_value").text();
    //    list.taxable_val = cmn_ReplaceCommas(taxable_val);
    //    //list.tds_amt = Crow.find("#tds_amount").text();
    //    var tds_amt = Crow.find("#tds_amount").text();
    //    list.tds_amt = cmn_ReplaceCommas(tds_amt);
    //    //list.tds_payable = Crow.find("#tds_payble").text();
    //    var tds_payable = Crow.find("#tds_payble").text();
    //    list.tds_payable = cmn_ReplaceCommas(tds_payable);
    //    list.dn_flag = Crow.find("#dn_flag").is(":checked") == true ? "Y" : "N";
    //    list.dn_no = Crow.find("#dn_no").text();
    //    list.dn_date = Crow.find("#dn_date").text();
    //    arr.push(list);
    //});
    //let tdsPoestingDetails = sessionStorage.getItem("tdsPoestingDetail");
    let tdsDetails = $("#TdsPostingDetailsTable").DataTable().data();
    tdsDetails.map((item) => {
        var list = {};
        list.supp_id = item[2];
        var pre_val = item[3];
        list.pre_val = cmn_ReplaceCommas(pre_val);
        var curr_mnth_val = item[4];
        list.curr_mnth_val = cmn_ReplaceCommas(curr_mnth_val);
        var tot_val = item[5];
        list.tot_val = cmn_ReplaceCommas(tot_val);
        var taxable_val = item[6];
        list.taxable_val = cmn_ReplaceCommas(taxable_val);
        var tds_amt = item[7];
        list.tds_amt = cmn_ReplaceCommas(tds_amt);
        var tds_payable = item[8];
        list.tds_payable = cmn_ReplaceCommas(tds_payable);
        list.dn_flag = "N";
        list.dn_no = "";
        list.dn_date = "";
        arr.push(list);
    })
    $("#TdsDetailsToSave").val(JSON.stringify(arr));
}
function TdsPostingSuppGLDetail() {
    var arr = [];// Work In Progress
    let tdsGlDetails = sessionStorage.getItem("allSuppGlDetails");
    if (tdsGlDetails != null && tdsGlDetails != "") {
        tdsGlDetails = JSON.parse(tdsGlDetails);
        var Compid = $("#CompID").text();
        tdsGlDetails.map((item) => {
            var list = {};
            list.comp_id = Compid;
            list.VouSrNo = item.vou_sr_no;//m
            list.GlSrNo = item.gl_sr_no;//m
            list.id = item.acc_id;//m
            list.acc_name = item.acc_name;
            list.type = "I";
            list.doctype = "D";
            list.Value = cmn_ReplaceCommas(item.value);
            list.ValueInBase = cmn_ReplaceCommas(item.value);
            list.DrAmt = cmn_ReplaceCommas(item.dr_amt_sp);//m
            list.CrAmt = cmn_ReplaceCommas(item.cr_amt_sp);//m
            list.DrAmtInBase = cmn_ReplaceCommas(item.dr_amt_bs);//m
            list.CrAmtInBase = cmn_ReplaceCommas(item.cr_amt_bs);//m
            list.Gltype = item.type;//m
            list.TransType = "";
            list.curr_id = item.curr_id; //m
            list.conv_rate = item.conv_rate;//m
            list.vou_type = item.vou_type;//m
            list.bill_no = item.bill_no || "";//m
            list.bill_date = item.bill_date || "";//m
            list.gl_narr = item.narr;//m
            arr.push(list);
        });
    }

    $("#hdItemvouDetail").val(JSON.stringify(arr));
}

function TdsPostingSlabDetailList() {
    var arr = [];
    //$("#table_tds_slabDetail tbody tr").each(function () {
    //    var Crow = $(this).closest("tr");//
    //    var list = {};
    //    list.supp_id = Crow.find("#supp_id").text();
    //    //list.tds_amt = Crow.find("#tds_amt").text();
    //    var tds_amt = Crow.find("#tds_amt").text();
    //    list.tds_amt = cmn_ReplaceCommas(tds_amt);
    //    //list.tds_perc = Crow.find("#tds_perc").text();
    //    var tds_perc = Crow.find("#tds_perc").text();
    //    list.tds_perc = cmn_ReplaceCommas(tds_perc);
    //    //list.tds_payble = Crow.find("#tds_payble").text();
    //    var tds_payble = Crow.find("#tds_payble").text();
    //    list.tds_payble = cmn_ReplaceCommas(tds_payble);
    //    arr.push(list);
    //});
    let slabDetail = sessionStorage.getItem("tds_slabDetail");
    if (slabDetail != null && slabDetail != "") {
        slabDetail = JSON.parse(slabDetail);
        slabDetail.map((item) => {
            var list = {};
            list.supp_id = item.supp_id;
            var tds_amt = item.tds_amt;
            list.tds_amt = cmn_ReplaceCommas(tds_amt);
            var tds_perc =item.tds_perc;
            list.tds_perc = cmn_ReplaceCommas(tds_perc);
            var tds_payble = item.tds_payble;
            list.tds_payble = cmn_ReplaceCommas(tds_payble);
            arr.push(list);
        })
    }

    $("#TdsSlabDetailsToSave").val(JSON.stringify(arr));
}
function TdsPostingSuppInvDetailList() {
    var arr = [];
    let SuppInvDetail = sessionStorage.getItem("tds_SuppInvDetail");
    if (SuppInvDetail != null && SuppInvDetail != "") {
        SuppInvDetail = JSON.parse(SuppInvDetail);
        SuppInvDetail.map((item) => {
            var list = {};
            list.supp_id = item.supp_id.toString();
            list.inv_no = item.inv_no.toString();
            list.inv_dt = item.inv_dt.toString();
            list.bill_no = item.bill_no.toString();
            list.bill_dt = item.bill_dt.toString();
            list.gr_val = cmn_ReplaceCommas(item.gr_val) || '0';
            list.tax_val = cmn_ReplaceCommas(item.tax_val) || '0';
            list.oc_val = cmn_ReplaceCommas(item.oc_val) || '0';
            list.net_val_bs = cmn_ReplaceCommas(item.net_val_bs) || '0';
            list.tds_val = cmn_ReplaceCommas(item.tds_val) || '0';
            arr.push(list);
        })
    }

    $("#TdsSuppInvDetailsToSave").val(JSON.stringify(arr));
}
function TdsPostingSuppInvSlabDetailList() {
    var arr = [];
    let SuppInvSlabDetail = sessionStorage.getItem("tds_SuppInvSlabDetail");
    if (SuppInvSlabDetail != null && SuppInvSlabDetail != "") {
        SuppInvSlabDetail = JSON.parse(SuppInvSlabDetail);
        SuppInvSlabDetail.map((item) => {
            var list = {};
            list.supp_id = item.supp_id.toString();
            list.inv_no = item.inv_no.toString();
            list.inv_dt = item.inv_dt.toString();
            list.bill_no = item.bill_no.toString();
            list.bill_dt = item.bill_dt.toString();
            list.inv_amt = cmn_ReplaceCommas(item.net_val_bs) || '0';
            list.taxable_value = cmn_ReplaceCommas(item.taxable_value) || '0';
            list.tds_perc = cmn_ReplaceCommas(item.tds_perc) || '0';
            list.tds_val = cmn_ReplaceCommas(item.tds_val) || '0';
            arr.push(list);
        })
    }

    $("#TdsSuppInvSlabDetailsToSave").val(JSON.stringify(arr));
}
function OnClickTdsPaybleIcon(e) {
    debugger;
    var Row = $(e.target).closest("tr");
    var suppId = Row.find("#supp_id").text();
    var suppName = Row.find("#supp_name").text();
    var tdsPayble = Row.find("#tds_payble").text();
    //var tdsPayble = cmn_ReplaceCommas(tdsPayble1);

    $("#PopTdsSuppName").val(suppName);
    $("#PopTdsPayble").val(tdsPayble);

    var TotPaybleTds = 0;
    var TotTdsAmt = 0;
    $("#TdsPaybleDtTbl tbody tr").remove();
    let slabDetail = sessionStorage.getItem("tds_slabDetail");
    if (slabDetail != null && slabDetail != "") {
        slabDetail = JSON.parse(slabDetail);
        let newslabDetail = slabDetail.filter(t => t.supp_id == suppId);
        newslabDetail.map((item,index) => {
            var tds_amt1 = item.tds_amt;
            var tds_amt = cmn_ReplaceCommas(tds_amt1);
            var tds_perc1 = item.tds_perc;
            var tds_perc = cmn_ReplaceCommas(tds_perc1);
            var tds_payble1 = item.tds_payble;
            var tds_payble = cmn_ReplaceCommas(tds_payble1);
            TotPaybleTds = parseFloat(TotPaybleTds) + parseFloat(CheckNullNumber(tds_payble));
            TotTdsAmt = parseFloat(TotTdsAmt) + parseFloat(CheckNullNumber(tds_amt));
            var tds_amt2 = cmn_addCommas(tds_amt);
            var tds_perc2 = cmn_addCommas(tds_perc);
            var tds_payble2 = cmn_addCommas(tds_payble);

            $("#TdsPaybleDtTbl tbody").append(`
            <tr>
                <td class="num_right" id="">${index + 1}</td>
                <td class="num_right" id="tds_amt">${tds_amt2}</td>
                <td class="num_right" id="tds_perc">${tds_perc2}</td>
                <td class="num_right" id="tds_payble">${tds_payble2}</td>
            </tr>
            `)
        })
    }
//    $("#table_tds_slabDetail tbody tr #supp_id:contains('" + suppId + "')").closest("tr").each(function (index) {
//        var CRow = $(this);
//        var tds_amt1 = CRow.find("#tds_amt").text();
//        var tds_amt = cmn_ReplaceCommas(tds_amt1);
//        var tds_perc1 = CRow.find("#tds_perc").text();
//        var tds_perc = cmn_ReplaceCommas(tds_perc1);
//        var tds_payble1 = CRow.find("#tds_payble").text();
//        var tds_payble = cmn_ReplaceCommas(tds_payble1);
//        TotPaybleTds = parseFloat(TotPaybleTds) + parseFloat(CheckNullNumber(tds_payble));
//        TotTdsAmt = parseFloat(TotTdsAmt) + parseFloat(CheckNullNumber(tds_amt));
//        var tds_amt2 = cmn_addCommas(tds_amt);
//        var tds_perc2 = cmn_addCommas(tds_perc);
//        var tds_payble2 = cmn_addCommas(tds_payble);

//        $("#TdsPaybleDtTbl tbody").append(`
//            <tr>
//                <td class="num_right" id="">${index + 1}</td>
//                <td class="num_right" id="tds_amt">${tds_amt2}</td>
//                <td class="num_right" id="tds_perc">${tds_perc2}</td>
//                <td class="num_right" id="tds_payble">${tds_payble2}</td>
//            </tr>
//`)
//    });
    var TotPaybleTds1 = cmn_addCommas(TotPaybleTds);
    $("#TotTdsPayble").text(TotPaybleTds1);
    //$("#TotTdsAmt").text(TotTdsAmt);

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
            $("#HdnCsvPrint").val(null);
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}

function OnClickPreValue(e) {
    debugger
    var Crow = $(e.target).closest('tr');
    var supp_id = Crow.find("#supp_id").text();
    var supp_name = Crow.find("#supp_name").text();
    var PreVlStD = $("#PreValueStartDate").text();
    var PreVlEdD = $("#PreValueEndDate").text();
    var Period = $("#th_Pre_Period").text();
    var PreVal = Crow.find("#pre_net_val_bs").text();
    var tds_id = $("#HdnTdsId").val();
    var docStatus = $("#DocumentStatus").val();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/TDSPosting/GetTdsSuppWiseInvoice",
        data: { supp_id: supp_id, PreVlStD: PreVlStD, PreVlEdD: PreVlEdD, Period: Period, PreVal: PreVal, status: docStatus, tds_id: tds_id },
        success: function (data) {
            debugger;
            $("#PopUpTdsPreviousBalanceInfo").html(data);
            $("#tdsPreBalSuppName").val(supp_name);
        }
    });
}
function OnClickCurrValue(e) {
    debugger;
    var Crow = $(e.target).closest('tr');
    var supp_id = Crow.find("#supp_id").text();
    var supp_name = Crow.find("#supp_name").text();
    var PreVlStD = $("#CurrValueStartDate").text();
    var PreVlEdD = $("#CurrValueEndDate").text();
    var Period = $("#th_Period").text();
    var PreVal = Crow.find("#net_val_bs").text();
    var tds_id = $("#HdnTdsId").val();
    var docStatus = $("#DocumentStatus").val();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/TDSPosting/GetTdsSuppWiseInvoice",
        data: { supp_id: supp_id, PreVlStD: PreVlStD, PreVlEdD: PreVlEdD, Period: Period, PreVal: PreVal, status: docStatus, tds_id: tds_id },
        success: function (data) {
            debugger;
            $("#PopUpTdsPreviousBalanceInfo").html(data);
            $("#tdsPreBalSuppName").val(supp_name);
        }
    });
}

function OnClickTaxableValue(e) {
    debugger;
    var Crow = $(e.target).closest('tr');
    var supp_id = Crow.find("#supp_id").text();
    var supp_name = Crow.find("#supp_name").text();
    var PreVlStD = $("#PreValueStartDate").text();
    var CurrVlEdD = $("#CurrValueEndDate").text();
    var Period = $("#th_Period").text();
    var taxable_value = Crow.find("#taxable_value").text();
    var year = $("#tds_year").val();
    var tds_month = $("#tds_month").val();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/TDSPosting/GetTdsSuppWiseTaxableValueDetail",
        data: {
            SuppId: supp_id, StartDate: PreVlStD, EndDate: CurrVlEdD, Year: year, Month: tds_month
            , TaxableValue: taxable_value, SuppName: supp_name
        },
        success: function (data) {
            debugger;
            $("#PopUpTdsTaxableValue").html(data);

        }
    });
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

function Onclick_TdsVouInfo(e) {
    const row = $(e.target).closest('tr');
    const supp_id = row.find("#supp_id").text();
    debugger
    let GLDetail = sessionStorage.getItem("allSuppGlDetails");
    if (GLDetail != null && GLDetail != "") {
        GLDetail = JSON.parse(GLDetail).filter(t => t.supp_id == supp_id);
    }
    var editable = $("#isDisabled").val() == "Y" ? "N" : "Y";
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/TDSPosting/GetTdsSuppWiseVoucherDetail",
        data: {
            suppId: supp_id, glDetail: JSON.stringify(GLDetail), editable: editable
        },
        success: function (data) {
            debugger;
            $("#Popup_TdsVoucherInfo").html(data);
            var tds_accs = $("#tds_acc_id option");
            $("#VoucherDetail tbody tr").each(function () {
                let row = $(this);
                let type = row.find("#type").val()
                if (type == "Tds") {
                    let acc_id = row.find("#tdhdn_GlAccId").text()
                    let GlSrNo = row.find("#td_GlSrNo").text()
                    tds_accs.each(function () {
                        let optVal = $(this).val();
                        let optText = $(this).text();
                        if (optVal != "0" && optVal != acc_id) {
                            $("#Acc_name_" + GlSrNo).append(`<option value="${optVal}">${optText}</option>`);
                        }
                    });
                    $("#Acc_name_" + GlSrNo).select2({
                        templateResult: function (data) {
                            var selected = $("#Acc_name_" + GlSrNo).val();
                            if (check(data, selected, "#VoucherDetail", "#SNohf", "#Acc_name_") == true) {
                                var classAttr = $(data.element).attr('class');
                                var hasClass = typeof classAttr != 'undefined';
                                classAttr = hasClass ? ' ' + classAttr : '';
                                var $result = $(
                                    '<div class="row">' +
                                    '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                                    '</div>'
                                );
                                return $result;
                            }
                            firstEmptySelect = false;
                        }
                    });
                }
            });
        }
    });
}

function CreateGLForSupp(supp_id, supp_acc_id, supp_acc_name, tds_payble, newSlabDtl, vou_sr_no, supp_name, pur_val, month_year) {

    let dn_narr = $("#hd_dn_narr").text();
    var GLDetail = [];
    var glRno = 1;
    var curr_id = newSlabDtl[0].bs_curr_id // To get base currency id
    var conv_rate = newSlabDtl[0].conv_rate // To get conv_rate
    /*------------------ Supp. Account Entry -----------------*/
    let acc_id_start_no = supp_acc_id.toString().substring(0, 1);
    var ccflag = "N";
    if (acc_id_start_no == "3" || acc_id_start_no == "4") {
        ccflag = "Y";
    }
    dn_narr = dn_narr.replaceAll('{supp_name}', supp_name).replaceAll('{pur_val}', pur_val).replaceAll('{month_year}', month_year);

    var rowSpan = newSlabDtl.filter(t => parseFloat(cmn_ReplaceCommas(t.tds_payble)) > 0).length + 1;

    GLDetail.push({
        rowSpan: rowSpan, gl_sr_no: glRno, acc_name: supp_acc_name, type: "TSupp", acc_id: supp_acc_id
        , dr_amt_bs: tds_payble
        , dr_amt_sp: tds_payble
        , cr_amt_bs: parseFloat(0).toFixed(cmn_ValDecDigit)
        , cr_amt_sp: parseFloat(0).toFixed(cmn_ValDecDigit)
        , ccflag: ccflag, narr: dn_narr, supp_id: supp_id, vou_sr_no: vou_sr_no
        , value: tds_payble, bill_no: "", bill_date: "", curr_id: curr_id, conv_rate: conv_rate
        , vou_no: "", vou_dt: "", vou_type: "DN", gl_type: "TSupp"
    });
    /*------------------ Supp. Account Entry End-----------------*/
    /*------------------ TDS Account Entry -----------------*/
    newSlabDtl.map((item, idx) => {
        if (parseFloat(cmn_ReplaceCommas(item.tds_payble)) > 0) {
            acc_id_start_no = item.tds_acc_id.toString().substring(0, 1);
            ccflag = "N";
            if (acc_id_start_no == "3" || acc_id_start_no == "4") {
                ccflag = "Y";
            }
            GLDetail.push({
                rowSpan: rowSpan, gl_sr_no: (glRno + 1), acc_name: item.tds_acc_name, type: "Tds", acc_id: item.tds_acc_id
                , dr_amt_bs: parseFloat(0).toFixed(cmn_ValDecDigit)
                , dr_amt_sp: parseFloat(0).toFixed(cmn_ValDecDigit)
                , cr_amt_bs: item.tds_payble
                , cr_amt_sp: item.tds_payble
                , ccflag: ccflag, narr: dn_narr, supp_id: supp_id, vou_sr_no: vou_sr_no
                , value: tds_payble, bill_no: "", bill_date: "", curr_id: curr_id, conv_rate: conv_rate
                , vou_no: "", vou_dt: "", vou_type: "DN", gl_type:"Tds"
            });
        }

    });
    return GLDetail;
}
function Cmn_SaveAndExitPopupGlDetail() {
    let GLDetail = sessionStorage.getItem("allSuppGlDetails");
    let supp_id = $("#hdn_pp_supp_id").val();
    debugger
    let arrayUpdate = [];
    $("#VoucherDetail tbody tr").each(function () {
        let row = $(this);
        let vou_sr_no = row.find("#td_vou_sr_no").text();
        let GlSrNo = row.find("#td_GlSrNo").text();
        let gl_narr = row.find("#gl_narr").text();
        let type = row.find("#type").val();
        let acc_id = row.find("#hfAccID").val();
        let acc_name = row.find("#txthfAccID").val();
        if (type == "Tds") {
            acc_id = row.find("#Acc_name_" + GlSrNo).val();
            acc_name = row.find("#Acc_name_" + GlSrNo+" option:selected").text();
        } 
        arrayUpdate.push({ vou_sr_no: vou_sr_no, GlSrNo: GlSrNo, acc_id: acc_id, acc_name: acc_name, gl_narr: gl_narr, type: type})
    });
    if (GLDetail != null && GLDetail != "") {
        GLDetail = JSON.parse(GLDetail);
        let newGLDetail = GLDetail.map(item => {
            if ((item.supp_id == supp_id && arrayUpdate.findIndex(t => t.vou_sr_no == item.vou_sr_no && t.GlSrNo == item.gl_sr_no) > -1)) {
                let update = arrayUpdate.filter(t => t.vou_sr_no == item.vou_sr_no && t.GlSrNo == item.gl_sr_no)[0];
                return {
                    ...item, acc_id: update.acc_id, acc_name: update.acc_name
                    , narr: update.gl_narr
                }
            } else {
                return item;
            }
            
        }
            
        );
        sessionStorage.setItem("allSuppGlDetails", JSON.stringify(newGLDetail));
    }
    $("#pp_GlDetailSaveAndExitBtn").attr("data-dismiss","modal")
}
function OnChangeAccountName(gl_sr_no,e) {

}
//-------------Cost Center Section----------//
function Onclick_CCbtn(flag, e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var CstDbAmt = clickedrow.find("#dramtInBase").text();
    var CstCrtAmt = clickedrow.find("#cramtInBase").text();
    var vou_sr_no = clickedrow.find("#td_vou_sr_no").text();
    var gl_sr_no = clickedrow.find("#td_GlSrNo").text();
    var RowNo = clickedrow.find("#SNohf").val();

    var GLAcc_Name = clickedrow.find("#Acc_name_" + RowNo + " option:selected").text();
    var GLAcc_id = clickedrow.find("#Acc_name_" + RowNo + " option:selected").val();
    if (GLAcc_Name == null || GLAcc_Name == "") {
        GLAcc_Name = clickedrow.find("#txthfAccID").val();
        GLAcc_id = clickedrow.find("#hfAccID").val();
    }
    var disableflag = ($("#txtdisable").val());
    var NewArr = new Array();
    $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + GLAcc_id + ")").closest('tr').each(function () {
        var row = $(this);
        let cc_vou_sr_no = row.find("#hdntbl_vou_sr_no").text();
        if (cc_vou_sr_no == vou_sr_no) {
            var List = {};
            List.GlAccount = row.find("#hdntbl_GlAccountId").text();
            List.ddl_CC_Type = row.find("#hdntbl_CCtype").text();
            List.ddl_Type_Id = row.find('#hdntbl_CCtype_Id').text();
            List.ddl_CC_Name = row.find('#hdntbl_CC_Name').text();
            List.ddl_Name_Id = row.find('#hdntbl_CCName_Id').text();
            List.vou_sr_no = row.find('#hdntbl_vou_sr_no').text();
            List.gl_sr_no = row.find('#hdntbl_gl_sr_no').text();
            var amount = row.find("#hdntbl_CstAmt").text();
            List.CC_Amount = parseFloat(cmn_ReplaceCommas(amount)).toFixed(ValDecDigit);
            NewArr.push(List);
        }
    })
    var Amt;
    if (CheckNullNumber(CstDbAmt) != "0") {
        Amt = CstDbAmt;
    }
    else {
        Amt = CstCrtAmt;
    }
    var DocMenuId = $("#DocumentMenuId").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/LocalSaleInvoice/GetCstCntrtype",
        data: {
            Flag: flag,
            Disableflag: disableflag,
            CC_rowdata: JSON.stringify(NewArr),
            DocumentMenuId: DocMenuId
        },
        success: function (data) {
            debugger;
            $("#CostCenterDetailPopup").html(data);
            $("#CC_vou_sr_no").val(vou_sr_no);
            $("#CC_gl_sr_no").val(gl_sr_no);
            $("#CC_GLAccount").val(GLAcc_Name);
            $("#hdnGLAccount_Id").val(GLAcc_id);
            $("#GLAmount").val(Amt);
            $("#ddl_CC_Name").append(`<option value="0">---Select---</option>`);
            $("#hdnTable_Id").text("VoucherDetail");
        },
    })
}
//-------------------Cost Center Section End-------------------//

