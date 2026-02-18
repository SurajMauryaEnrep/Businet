 $(document).ready(function () {
    $("#ddlissuedby").select2();
    var CurrentDate = moment().format('YYYY-MM-DD');
    $("#txtTodate").val(CurrentDate);
    debugger;
    var ReportType = $("#ReportType").val();
    if (ReportType == "EW") {
        $("#SampleTrackingTbl").css("display", "none");
    }
    $("#datatable-buttons >tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        $("#datatable-buttons >tbody >tr").css("background-color", "#ffffff");
        $(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");
    });
    BindSampleRcptItmList();
   // BindEntityData()
})

function OnClickIconBtn(e) {
    debugger;
  //  var clickedrow = $(e.target).closest("tr"); 
    //var ItmCode = $("#hf_ItemID").val();
    var ItmCode = $("#ddlItemName").val();
    ItemInfoBtnClick(ItmCode);
  
}
function OnClickIconBtnTable(e) {
    debugger;
      var clickedrow = $(e.target).closest("tr"); 
    /* var ItmCode = $("#hf_ItemID").val();*/
    var ItmCode = clickedrow.find("#hfItemID").val();
    ItemInfoBtnClick(ItmCode);

}
function onchangeIssuedby() {
    var issuedby = $("#ddlissuedby").val();
    if (issuedby != "" && issuedby != "0" && issuedby != null) {
        $("#hdnddlissuedby").val(issuedby);
    }
}
async function OnchangeSalesBy() {
    debugger;
    let ReportType = $("#ReportType").val();
    let ShowAs = "";

    // Common parameters
    let EntityType = $("#EntityType option:selected").val();
    let ItemId = $("#ddlItemName option:selected").val();
    let EntityId = $("#EntityName option:selected").val();
    let FromDate = $("#txtFromdate").val();
    let ToDate = $("#txtTodate").val();
    let Issuedby = $("#hdnddlissuedby").val() || null;

    // Reset fields by default
    $("#ddlItemName").val("0").trigger("change.select2");
    $("#EntityName").val("0").trigger("change.select2");

    // Helper function to call AJAX
    const loadData = async () => {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/SampleTracking/FilterSampleTrackingDetail",
            data: {
                ReportType,
                ItemId,
                EntityId,
                FromDate,
                ToDate,
                EntityType,
                Issuedby,
                ShowAs
            },
            success: function (data) {
                debugger;
                $("#Divtble").html(data);
            },
            error: function (xhr, errorType, exception) {
                debugger;
                console.error("Error:", exception);
            }
        });
    };

    // 🔹 Report Type: SW (Sample Wise)
    if (ReportType === "SW") {
        $("#Div_SummaryDetail").hide();
        $("#Div_Issuedby").show();

        let ValidationFlag = true;

        if ($("#ddlItemName option:selected").val() == "0") {
            $("#vmST_Item").text($("#valueReq").text());
            $("#ddlItemName, [aria-labelledby='select2-ddlItemName-container']").css("border-color", "red");
            $("#Sample_tracking tbody tr").remove();
            //ValidationFlag = false;
        }

        //if (!ValidationFlag) {
           
        //    return;
        //}

        await loadData();

        $("#EntityWiseTbl").hide();
        $("#SampleTrackingTbl").show();
        $("#InactiveRemarksReq").show();
    }

    // 🔹 Report Type: EW (Entity Wise)
    else if (ReportType === "EW") {
        $("#Div_SummaryDetail").show();
        $("#Div_Issuedby").hide();

        if ($("#SampleTrackingMISSummary").is(":checked")) ShowAs = "S";
        else if ($("#SampleTrackingMISDetail").is(":checked")) ShowAs = "D";

        await loadData();

        $("#SampleTrackingTbl").hide();
        $("#EntityWiseTbl").show();
        $("#InactiveRemarksReq").hide();
    }

    // 🔹 Report Type: PS (Product Summary?)
    else if (ReportType === "PS") {
        $("#Div_SummaryDetail").show();
        $("#Div_Issuedby").show();

        if ($("#SampleTrackingMISSummary").is(":checked")) ShowAs = "S";
        else if ($("#SampleTrackingMISDetail").is(":checked")) ShowAs = "D";

        await loadData();

        $("#SampleTrackingTbl").hide();
        $("#EntityWiseTbl").show();
        $("#InactiveRemarksReq").hide();
    }

    // 🔹 Report Type: IB or BW
    else if (ReportType === "IB" || ReportType === "BW") {
        $("#Div_SummaryDetail").hide();
        $("#Div_Issuedby").show();

        $("#Sample_tracking tbody tr").remove();

        await loadData();

        $("#EntityWiseTbl").hide();
        $("#SampleTrackingTbl").show();
        $("#InactiveRemarksReq").show();
    }
    else {

    }
}


function onchangeShowAs() {
    OnchangeSalesBy();
}
function OnChangeEntityType() {
    debugger;
    var EntityType = $("#EntityType").val();
    //if (EntityType != "0") {
    //    $("#EntityType").css("border-color", "#ced4da");
    //    $("#SpanEntityTypeErrorMsg").css("display", "none");

        //$("#EntityName").val(0).trigger('change');
        BindSR_SuppCustList(EntityType);
        //$("#EntityName").empty();
        //$("#EntityName").append('<option value="0">---Select---</option>')
   // }
    //else {
    //    $("#SpanEntityTypeErrorMsg").text($("#valueReq").text());
    //    $("#EntityType").css("border-color", "red");
    //    $("#SpanEntityTypeErrorMsg").css("display", "block");
    //    BindSR_SuppCustList(EntityType);
    //    $("#EntityName").val(0).trigger('change');
    //}
    //$("#SpanEntityNameErrorMsg").css("display", "none");
    //$("[aria-labelledby='select2-EntityName-container']").css("border-color", "#ced4da");
}
//function BindSR_SuppCustList(EntityType) {
//    debugger;
//    $("#EntityName").select2({
//        ajax: {
//            url: $("#hfsuppcustlist").val(),          
//            data: function (params) {
//                var queryParameters = {
//                    EntityName: params.term, // search term like "a" then "an"
//                    entity_type: EntityType
//                };
//                return queryParameters;
//            },
            
//            dataType: "json",
//            cache: true,
//            delay: 250,
//            //type: 'POST',
//            contentType: "application/json; charset=utf-8",
//            processResults: function (data, params) {
//                if (data == 'ErrorPage') {
//                    Error_Page();
//                    return false;
//                }
//                params.page = params.page || 1;
//                return {
//                    //results:data.results,
//                    results: $.map(data, function (val, item) {
//                        return { id: val.ID, text: val.Name };
//                    })
//                };
//            }
//        },
//    });
//}
function BindSR_SuppCustList(EntityType) {
    debugger;
    $('#EntityName').attr('multiple','multiple');
    $.ajax({
        url: $("#hfsuppcustlist").val(),
        type: "GET",
        data: { entity_type: EntityType },
        success: function (data) {
            debugger
            $("#EntityName").empty();
            $.each(data, function (i, val) {
                if (val.ID != 0) {
                    $("#EntityName").append(
                        $('<option></option>').val(val.ID).text(val.Name)
                    );
                }
            });
            Cmn_initializeMultiselect(['#EntityName']);
            $('#EntityName').multiselect('rebuild');
        }
    });
}
function BindEntityData() {
    $("#EntityName").select2({
        ajax: {
            url: $("#SearchEntityList").val(),
            data: function (params) {
                var queryParameters = {
                    EntityName: params.term // search term like "a" then "an"
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
                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }
        },
    });
}
function BindSampleRcptItmList() {
    debugger;
    DynamicSerchableItemDDL("", "#ddlItemName", "", "", "", "SR");
    //$.ajax(
    //    {
    //        type: "POST",
    //        url: "/ApplicationLayer/SampleTracking/GetST_ItemList",
    //        data: function (params) {
    //            var queryParameters = {
    //                ST_Item: params.term
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        success: function (data) {
    //            debugger;
    //            if (data == 'ErrorPage') {
    //                Error_Page();
    //                return false;
    //            }
    //            if (data !== null && data !== "") {
    //                var arr = [];
    //                arr = JSON.parse(data);
    //                if (arr.Table.length > 0) {

    //                    $('#ddlItemName').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
    //                    for (var i = 0; i < arr.Table.length; i++) {
    //                        $('#Textddl').append(`<option data-uom="${arr.Table[i].uom_name}" value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);
    //                    }
    //                    var firstEmptySelect = true;
    //                    $('#ddlItemName').select2({
    //                        templateResult: function (data) {
    //                            var UOM = $(data.element).data('uom');
    //                            var classAttr = $(data.element).attr('class');
    //                            var hasClass = typeof classAttr != 'undefined';
    //                            classAttr = hasClass ? ' ' + classAttr : '';
    //                            var $result = $(
    //                                '<div class="row">' +
    //                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
    //                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +
    //                                '</div>'
    //                            );
    //                            return $result;
    //                            firstEmptySelect = false;
    //                        }
    //                    });
    //                    debugger;
    //                }
    //            }
    //        },
    //    });
}
function OnClickIssued_ReceiptIconBtn(e,TrackingType) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount

    var EntityName = "";
    var ItemName = "";
    var EntityID = "";
    var EntityTypeCode = "";
    var ItemID = "";
    var issue_dt = "";
    var issue_date = "";
    var receive_dt = "";
    var receive_date = "";
    var other_dtl = "";
    var sr_type = "";
    var Issuedby = "";

    var fromdate = $("#txtFromdate").val();
    var todate = $("#txtTodate").val();
    var ReportType = $("#ReportType").val();
    EntityName = clickedrow.find("#EntityNameSpan").text().trim();
    ItemName = clickedrow.find("#ItemNameSpan").text().trim();
    EntityID = clickedrow.find("#hfEntityID").val();
    EntityTypeCode = clickedrow.find("#hfEntityTypeCode").val().trim();
    ItemID = clickedrow.find("#hfItemID").val().trim();
  //  issue_dt = clickedrow.find("#issue_date").text().trim();
    //other_dtl = clickedrow.find("#other_dtl").text().trim();
    //sr_type = clickedrow.find("#sr_type").text().trim();
    ST_UOM = clickedrow.find("#ST_UOM").val().trim();
    //if (issue_dt != "") {
    //    var IssueDt = issue_dt.split("-");
    //    issue_date = (IssueDt[2] + "-" + IssueDt[1] + "-" + IssueDt[0]);
    //}
    //receive_dt = clickedrow.find("#receive_date").text().trim();
    //if (receive_dt != "") {
    //    var ReceiveDt = receive_dt.split("-");
    //    receive_date = (ReceiveDt[2] + "-" + ReceiveDt[1] + "-" + ReceiveDt[0]);
    //}
    if (TrackingType === "R" || TrackingType === "RP" ) {
        $("#IRPopUpheading").text($("#span_ReceiptsDetail").text());
       
        $("#issue_rec_dateID").text("");
        $("#issue_rec_dateID").text($("#span_ReceviedDate").text());
    }
    else if (TrackingType === "I" || TrackingType === "IP") {
        
        $("#IRPopUpheading").text($("#span_IssueDetail").text());
        $("#issue_rec_dateID").text("");
        $("#issue_rec_dateID").text($("#span_issuedDate").text());
    }
    else {    
        $("#IRPopUpheading").text($("#span_ReceiptsDetail").text());
    }
    (TrackingType == "I" || TrackingType == "IP") ? $("#IssuedByTDID").show() : $("#IssuedByTDID").hide();
    Issuedby = (TrackingType == "IP" || TrackingType === "RP" || TrackingType == "BP") ? $("#hdnddlissuedby").val() : "";
    if (ItemID != "" && ItemID != null && EntityID != "" && EntityID != null) {
        try {
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/SampleTracking/GetItemIssuedReceivedDetailList",
                    data: {
                        ItemID: ItemID, EntityID: EntityID, EntityTypeCode: EntityTypeCode, Type: TrackingType,
                        issue_date: issue_date, receive_date: receive_date,
                        sr_type: sr_type, other_dtl: other_dtl, ST_UOM: ST_UOM,
                        fromdate: fromdate, todate: todate, Issuedby: Issuedby
                    },
                    dataType: "json",
                    success: function (data) {
                        debugger;
                        if (data == 'ErrorPage') {
                            return false;
                        }
                        if (TrackingType === "R" || TrackingType === "I" || TrackingType === "RP" || TrackingType === "IP") {
                            cmn_delete_datatable("#TblIssued_ReceivedSampleTracking");
                        }
                        else {
                            cmn_delete_datatable("#TblBalanceIssued_ReceivedSampleTracking");
                        }
                       
                        if (data !== null && data !== "") {
                            var arr = [];
                            arr = JSON.parse(data);
                            if (arr.Table.length > 0) {
                               
                                var rowIdx = 0;
                                var TotalAmt = parseFloat(0).toFixed(ValDecDigit);
                                if (TrackingType === "R" || TrackingType === "I" || TrackingType === "RP" || TrackingType === "IP") {
                                    $("#lblEntityName").text(EntityName);
                                    $("#lblItemName").text(ItemName);
                                    $('#TblIssued_ReceivedSampleTracking tbody tr').remove();

                                    for (var k = 0; k < arr.Table.length; k++) {
                                        TotalAmt = parseFloat(TotalAmt) + parseFloat(arr.Table[k].Amount);
                                        var TDIssuedBy = "";
                                        if (TrackingType === "I" || TrackingType === "IP") {
                                            TDIssuedBy = `<td>${arr.Table[k].Issued_Name}</td>`;
                                        } else {
                                            TDIssuedBy = `<td style="display:none;"></td>`;
                                        }
                                       

                                        $('#TblIssued_ReceivedSampleTracking tbody').append(`<tr id="R${++rowIdx}">
                                        <td width="5%">${k + 1}</td>
                                        <td hidden="hidden">${EntityName}</td>
                                        <td hidden="hidden">${ItemName}</td>
                                        <td>${arr.Table[k].DocNo}</td>
                                        <td>${arr.Table[k].DocDate}</td>                                       
                                        <td>${arr.Table[k].DocType}</td>
                                        <td>${arr.Table[k].sr_type}</td>
                                        <td>${arr.Table[k].other_dtl}</td>
                                        <td>${arr.Table[k].Issue_Recived_DATE}</td>
                                           ${TDIssuedBy}
                                       <td class="num_right">${parseFloat(arr.Table[k].Qty).toFixed(QtyDecDigit)}</td>                                                          
                                        <td class="num_right">${parseFloat(arr.Table[k].Rate).toFixed(RateDecDigit)}</td>
                                        <td class="num_right">${parseFloat(arr.Table[k].Amount).toFixed(ValDecDigit)}</td>
                            </tr>`);
                                    }
                                    $("#ST_TotalAmount").text(parseFloat(TotalAmt).toFixed(ValDecDigit));
                                }
                                else {
                                    $("#BalancelblEntityName").text(EntityName);
                                    $("#BalancelblItemName").text(ItemName);
                                    $('#TblBalanceIssued_ReceivedSampleTracking tbody tr').remove();

                                              for (var k = 0; k < arr.Table.length; k++) {
                                        TotalAmt = parseFloat(TotalAmt) + parseFloat(arr.Table[k].Amount);

                                        $('#TblBalanceIssued_ReceivedSampleTracking tbody').append(`<tr id="R${++rowIdx}">
                                        <td width="5%">${k + 1}</td>
                                        <td hidden="hidden">${EntityName}</td>
                                        <td hidden="hidden">${ItemName}</td>
                                        <td>${arr.Table[k].issue_no}</td>
                                             <td>${arr.Table[k].issueddate}</td>
                                        <td>${arr.Table[k].sr_type}</td>
                                        <td>${arr.Table[k].other_dtl}</td>
                                        <td class="num_right">${parseFloat(arr.Table[k].Qty).toFixed(QtyDecDigit)}</td>                                     
                                         <td>${arr.Table[k].DocNo}</td>
                                        <td>${arr.Table[k].Recived_DATE}</td>
                                         <td class="num_right">${parseFloat(arr.Table[k].rec_qty).toFixed(QtyDecDigit)}</td>                                                               
                                        <td class="num_right">${parseFloat(arr.Table[k].balance).toFixed(QtyDecDigit)}</td>
                                        <td>${arr.Table[k].elsp_days}</td>
                                      

                            </tr>`);
                                    }
                                   
                                }                               
                            }
                            else {
                                if (TrackingType === "R" || TrackingType === "I" || TrackingType === "RP" || TrackingType === "IP") {
                                    $('#TblIssued_ReceivedSampleTracking tbody tr').remove();
                                    $("#ST_TotalAmount").text(parseFloat(0).toFixed(ValDecDigit));
                                }
                                else {
                                    $('#TblBalanceIssued_ReceivedSampleTracking tbody tr').remove();
                                }
                            }
                            
                        }
                        if (TrackingType === "R" || TrackingType === "I" || TrackingType === "RP" || TrackingType === "IP") {
                            cmn_apply_datatable("#TblIssued_ReceivedSampleTracking");
                        }
                        else {
                            cmn_apply_datatable("#TblBalanceIssued_ReceivedSampleTracking");
                        }
                    },
                });
        } catch (err) {
            //console.log(" Error : " + err.message);
        }
    }
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
}
function BtnSearch() {
    debugger;
    FilterSampleTrackingDetailList();
}
function FilterSampleTrackingDetailList() {
    debugger;
    try {
        let ShowAs = "";
        var ReportType = $("#ReportType").val();
        if (ReportType === "EW" || ReportType === "PS") {
           

            if ($("#SampleTrackingMISSummary").is(":checked")) ShowAs = "S";
            else if ($("#SampleTrackingMISDetail").is(":checked")) ShowAs = "D";
        }
       
        if (ReportType == "SW") {
            var ValidationFlag = true;
            if ($("#ddlItemName option:selected").val() == "0") {
                document.getElementById("vmST_Item").innerHTML = $("#valueReq").text();
                $("#ddlItemName").css("border-color", "red");
                $("[aria-labelledby='select2-ddlItemName-container']").css("border-color", "red");
                ValidationFlag = false;
            }
            if (ValidationFlag == false) {
                return false;
            }
        }
     
        var EntityType = $("#EntityType option:selected").val();
        var ItemId = $("#ddlItemName option:selected").val();
        //var EntityId = $("#EntityName option:selected").val();
        var EntityId = cmn_getddldataasstring("#EntityName");
        var FromDate = $("#txtFromdate").val();
        var ToDate = $("#txtTodate").val();
        var Issuedby = $("#hdnddlissuedby").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/SampleTracking/FilterSampleTrackingDetail",
            data: {
                ReportType: ReportType,
                ItemId: ItemId,
                EntityId: EntityId,
                FromDate: FromDate,
                ToDate: ToDate,
                EntityType: EntityType,
                Issuedby: Issuedby,
                ShowAs: ShowAs
            },
            success: function (data) {
                debugger;
                if (ReportType == "EW") {
                    $('#Divtble').html(data);
                }
                else {
                    $('#Divtble').html(data);
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
function OnChangeItemName() {
    document.getElementById("vmST_Item").innerHTML = null;
    $("[aria-labelledby='select2-ddlItemName-container']").css("border-color", "#ced4da");
    $("#ddlItemName").val();
}
function Export() {

    $('#export').click(function () {
        debugger;
        var ReportType = $("#ReportType").val();
        var EntityType = $("#EntityType option:selected").val();
        var ItemId = $("#ddlItemName option:selected").val();
        var EntityId = $("#EntityName option:selected").val();
        var FromDate = $("#txtFromdate").val();
        var ToDate = $("#txtTodate").val();
        var Issuedby = "";
        Issuedby = (ReportType === "SW" || ReportType === "PS" || ReportType === "BW" || ReportType === "IB") ? $("#hdnddlissuedby").val() : "";
        let ShowAs = "";
        if (ReportType === "EW" || ReportType === "PS") {


            if ($("#SampleTrackingMISSummary").is(":checked")) ShowAs = "S";
            else if ($("#SampleTrackingMISDetail").is(":checked")) ShowAs = "D";
        }
        //var filters = EntityType + "," + EntityId + "," + ItemId + "," + FromDate + "," + ToDate + "," + ReportType + "," + Issuedby + "," + ShowAs;
        var filterArray = [
            EntityType, EntityId, ItemId, FromDate, ToDate, ReportType, Issuedby, ShowAs
        ];
        var filters = filterArray.map(x => `[${x}]`).join('_');
        var searchValue = $("#Sample_tracking_filter input[type=search]").val();
        window.location.href = "/ApplicationLayer/SampleTracking/SampleWiseExcelDownload?searchValue=" + searchValue + "&filters=" + filters + "&ReportType=" + ReportType + "&ShowAs=" + ShowAs;

    });
}