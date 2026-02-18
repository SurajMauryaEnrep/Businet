/************************************************
Javascript Name:Domestic Purchase Order List
Created By:Rahul
Created Date: 23-03-2021
Description: This Javascript use for the DPO many function

Modified By:
Modified Date:
Description:
*************************************************/
$(document).ready(function () {
    debugger;
    RemoveSession();
    $('#Btn_AddNewPO').on("click", function () {
        debugger;
        RemoveSessionNew();
    });
    $("#datatable-buttons1 >tbody").bind("dblclick", function (e) {
        debugger;
        var ListFilterData = $("#ListFilterData").val();
        var DocumentMenuId = $("#DocumentMenuId").val();
        var WF_status = $("#WF_status").val();
        var clickedrow = $(e.target).closest("tr");
        var LPO_No = clickedrow.children("#OrderNo").text();
        var LPO_Date = clickedrow.children("#OrderDt").text();
        var ListItem_type = clickedrow.children("#ListItem_type").text();
        if (LPO_No != "" && LPO_No != null) {
            location.href = "/ApplicationLayer/DPO/DoubleClickOnList/?DocNo=" + LPO_No + "&DocDate=" + LPO_Date + "&ListFilterData=" + ListFilterData + "&DocumentMenuId=" + DocumentMenuId + "&WF_status=" + WF_status + "&ListItem_type=" + ListItem_type;
        }
        
    });
    $("#datatable-buttons1 >tbody").bind("click", function (e) {
        
        var clickedrow = $(e.target).closest("tr");
        var PO_No = clickedrow.children("#OrderNo").text();
        var PO_Date = clickedrow.children("#OrderDt").text();
        var Supp_name = clickedrow.children("#SuppName").text();
        var Doc_id = $("#DocumentMenuId").val();

        $("#hdDoc_No").val(PO_No);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetPurchaseOrderDetails(PO_No, PO_Date);
        GetWorkFlowDetails(PO_No, PO_Date, Doc_id, Doc_Status);
        $("#hdpo_no").val(PO_No);
        $("#hdpo_dt").val(PO_Date);
        $("#hdsupp_name").val(Supp_name);
    });
    BindSupplierPOList();
    //cmn_delete_datatable("#PoListPageData");
    //cmn_apply_datatable("#PoListPageData");
    //cmn_delete_datatable("#tbl_po_list");
    //cmn_apply_datatable("#tbl_po_list");
});
function RemoveSession() {
    sessionStorage.removeItem("EditLPONo");
    sessionStorage.removeItem("EditLPODate");
}
function RemoveSessionNew() {
    sessionStorage.removeItem("TaxCalcDetails");
    sessionStorage.removeItem("LPO_No");
    sessionStorage.removeItem("POTransType");
    sessionStorage.removeItem("POitemList");
}
function BtnSearch() {
    debugger;
    FilterDPOList();
    ResetWF_Level();
}
function FilterDPOList() {
    debugger;
    var ValDecDigit = $("#ValDigit").text();///Amount
    try {
        var SuppId = $("#POListSupplier option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        var item_type = $("#idItemtype").val();
        var DocumentMenuId = $("#DocumentMenuId").val();
        $("#datatable-buttons2")[0].id = "dttbl1";
        $.ajax({
            type: "POST", 
            url: "/ApplicationLayer/DPO/SearchDPODetail",
            data: {
                SuppId: SuppId,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status,
                DocumentMenuId: DocumentMenuId,
                item_type: item_type
            },
            success: function (data) {
                debugger;
              
                $('#tbodyDPOList').html(data);
                $("#dttbl1")[0].id = "datatable-buttons2";
                //cmn_apply_datatable("#datatable-buttons");
                $('#ListFilterData').val(SuppId + ',' + Fromdate + ',' + Todate + ',' + Status + ',' + item_type);
                $("#TxtOrderVal").val(parseFloat(0).toFixed(ValDecDigit));
                $("#TxtGRNVal").val(parseFloat(0).toFixed(ValDecDigit));
                $("#TxtPendingOrderVal").val(parseFloat(0).toFixed(ValDecDigit));
                $("#TxtInvoiceVal").val(parseFloat(0).toFixed(ValDecDigit));
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("ItemSetup Error : " + err.message);

    }
}

function DPODetail() {
    debugger;
    try {
        location.href = "/ApplicationLayer/DPO/DPODetail";
    } catch (err) {
        console.log("PO Error : " + err.message);
    }
}
function BindSupplierPOList() {
    var Branch = sessionStorage.getItem("BranchID");
    $("#POListSupplier").select2({
        ajax: {
            url: $("#SuppNameList").val(),
            data: function (params) {
                var queryParameters = {
                    SuppName: params.term, // search term like "a" then "an"
                    SrcType: $("#SrcType").val(), // search term like "a" then "an"
                    SuppPage: params.page,
                    BrchID: Branch
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    ErrorPage();
                    return false;
                }
                params.page = params.page || 1;
                return {
                    //results:data.results,
                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }
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
function GetPurchaseOrderDetails(PONo, PODate) {
    var ValDecDigit = $("#ValDigit").text();///Amount
    if (PONo != null && PONo != "" && PODate != null && PODate != "") {
        sessionStorage.setItem("ShowLoader", "N");
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/DPO/GetPO_Detail",
            data: { PO_No: PONo, PO_Date: PODate },
            dataType: "json",
            success: function (data) {
                if (data == 'ErrorPage') {
                    ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                    var arr = [];
                    //debugger;
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        $("#TxtOrderVal").val(parseFloat(arr.Table[0].PONetVal).toFixed(ValDecDigit));
                        $("#TxtGRNVal").val(parseFloat(arr.Table[0].GRNNetVal).toFixed(ValDecDigit));
                        $("#TxtPendingOrderVal").val(parseFloat(arr.Table[0].PendingOrdVal).toFixed(ValDecDigit));
                        $("#TxtInvoiceVal").val(parseFloat(arr.Table[0].Inv_NetVal).toFixed(ValDecDigit));
                    }
                    else {
                        $("#TxtOrderVal").val(parseFloat(0).toFixed(ValDecDigit));
                        $("#TxtGRNVal").val(parseFloat(0).toFixed(ValDecDigit));
                        $("#TxtPendingOrderVal").val(parseFloat(0).toFixed(ValDecDigit));
                        $("#TxtInvoiceVal").val(parseFloat(0).toFixed(ValDecDigit));
                    }
                }
                else {
                    $("#TxtOrderVal").val(parseFloat(0).toFixed(ValDecDigit));
                    $("#TxtGRNVal").val(parseFloat(0).toFixed(ValDecDigit));
                    $("#TxtPendingOrderVal").val(parseFloat(0).toFixed(ValDecDigit));
                    $("#TxtInvoiceVal").val(parseFloat(0).toFixed(ValDecDigit));
                }
            }
        });
    }
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

function OnClickOrderTrackingBtn(e) {
    debugger;
    //var PONo = $("#hdpo_no").val(); /// Commented By Nitesh 13102023 1521 for not data is correct in onclick of tracking Button
    //var PODate = $("#hdpo_dt").val();
    //var SuppName = $("#hdsupp_name").val();
    $("#Table_POTracking tbody tr").remove();/*Added This Line Nitesh 19-12-2023 */
    var currentrow = $(e.target).closest('tr');
    var PONo = currentrow.find("#OrderNo").text();
    var PODate = moment(currentrow.find("#OrderDt").text()).format('YYYY-MM-DD');;
    var SuppName = currentrow.find("#SuppName").text();
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (PONo != null && PONo != "" && PODate != null && PODate != "") {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/DPO/GetPOTrackingDetail",
            data: { PO_No: PONo, PO_Date: PODate, SuppName: SuppName, DocumentMenuId: DocumentMenuId },
            //dataType: "json",
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    //LSO_ErrorPage();
                    return false;
                }
                $("#trackingpo").html(data);
                cmn_apply_datatable("#Table_POTracking");
                $("#PurchaseOrderNumber").val(PONo);
                $("#OrderDate").val(PODate);
                $("#SupplierName").val(SuppName);
            }
        });
    }
}
function GetPendingDocument() {
    debugger;
    var Docid = $("#DocumentMenuId").val();
    var SrcType = $("#SrcType").val();
    $("#datatable-buttons1")[0].id = "dttbl1";
    //cmn_delete_datatable("#POPendingSourceDocuments");
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/DPO/GetPendingDocument",
            data: {
                Docid: Docid,
                SrcType: SrcType
            },
            success: function (data) {
                debugger;
                $('#POPendingSourceDocument').html(data);
              /*  cmn_apply_datatable("#datatable-buttons2");*/
                $("#dttbl1")[0].id = "datatable-buttons1";

            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }

        });
    } catch (err) {
        debugger;
        console.log("ItemSetup Error : " + err.message);

    }
}
function GetDataPending(ev) {
    debugger;
    var curr = $(ev.target).closest('tr');
    var Doc_no = curr.find("#Pending_OrderNo").text();
    var Doc_dt = curr.find("#Pending_OrderDt").text();
    var Doc_id = $("#DocumentMenuId").val();

    if (Doc_no != "" && Doc_no != null) {
        location.href = "/ApplicationLayer/DPO/DoubleClickOnList/?DocNo=" + LPO_No + "&DocDate=" + LPO_Date + "&ListFilterData=" + ListFilterData + "&DocumentMenuId=" + DocumentMenuId + "&WF_status=" + WF_status;
    }
}
function OnClickPOItemDetail(e,flag) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var doc_no = clickedrow.find("#Pending_OrderNo").text().trim();
    var doc_dt = clickedrow.find("#Pending_OrderDt").text().trim();
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/DPO/GetPendingDocumentitemDetail",
            data: {
                doc_no: doc_no,
                doc_dt: doc_dt,
                flag: flag
            },
            success: function (data) {
                $('#ItemInfoMIList1').html(data);

            },

        });
    }
    catch (err) {
        debugger;
        console.log("ItemSetup Error : " + err.message);

    }
}