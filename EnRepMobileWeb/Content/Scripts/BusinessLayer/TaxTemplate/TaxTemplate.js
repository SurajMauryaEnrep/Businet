
$(document).ready(function () {
    debugger;
    $("#datatable-buttons >tbody").on("dblclick", function (e) {
        var clickedrow = $(e.target).closest("tr");
        var tmplt_id = clickedrow.find("#tmpltid").text();
        window.location.href = "/BusinessLayer/TaxTemplate/DblClickToDetail?tmplt_id=" + tmplt_id;
    })
    BindTaxTemplateTaxList("", "", "");
    OnLoadHSNNumber();
    ListRowHighLight1();
    SearchableHSN();
    const TemplateId = $("#TemplateId").val();
    if (TemplateId == "0") {
        $("#AddTaxDetails").on("click", function () {
            var Tax_Amount = $("#Tax_Amount").val();
            var len = $("#TaxCalculatorTbl tbody tr").length;
            if (parseFloat(CheckNullNumber(Tax_Amount)) > 0 || len > 0) {
                $("#TAX").attr("disabled", true);
                $("#TDS").attr("disabled", true);
            }

        });

        $("#ResetTaxDetails").on("click", function () {

            $("#TAX").attr("disabled", false);
            $("#TDS").attr("disabled", false);

        });
    }
    Cmn_ByDefaultBranchData("#CustBrDetail", "#hdCustomerBranchId", "#cust_act_stat_", "Cust", "#HdnBranchActiveSatus");
});

function SaveBtnCkick() {
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 03-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#btn_save").prop("disabled", true); /*End*/
    }
    var count = 0;
    if (CheckVallidation("TemplateName", "vmTemplateName") == false) {
        count = count + 1;
        return false;
    }
    if ($("#TaxCalculatorTbl >tbody >tr").length <= 0) {
        swal("", $("#NoTaxSelected").text(), "warning");
        count = count + 1;
        return false;
    }
    $("#TamplateStatus").attr("disabled", false);
    $("#TAX").attr("disabled", false);
    $("#TDS").attr("disabled", false);
    var FinalTaxDetail = [];
    var FinalTaxDetailForDuplicate = [];
    var FinalBranchDetail = [];
    var FinalModuleDetail = [];
    var FinalHSNDetail = [];

    FinalTaxDetail = InsertTaxDetails();
    FinalTaxDetailForDuplicate = InsertTaxDetailsForDuplicate();
    FinalBranchDetail = InsertBranchDetail();
    FinalModuleDetail = InsertModuleDetail();
    FinalHSNDetail = InsertHSNNumberDetails();


    var TaxDt = JSON.stringify(FinalTaxDetail);
    var TaxDtForDuplicate = JSON.stringify(FinalTaxDetailForDuplicate);
    var BranchDT = JSON.stringify(FinalBranchDetail);
    var ModuleDT = JSON.stringify(FinalModuleDetail);
    var HSNNumberDT = JSON.stringify(FinalHSNDetail);
    debugger
    $("#TaxDetails").val(TaxDt);
    $("#TaxDetailsForDuplicate").val(TaxDtForDuplicate);
    $("#BranchDetail").val(BranchDT);
    $("#ModuleMapping").val(ModuleDT);
    $("#HSNDetail").val(HSNNumberDT);
    if (count > 0) {
        return false;

    }
    else {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");
        return true;
    }
}
function InsertTaxDetails() {
    var TaxDetailList = new Array();    
    if ($("#TaxCalculatorTbl >tbody >tr").length > 0) {
$("#TaxCalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);       
    var ItemList = {};
    debugger;
    //ItemList.TaxID = currentRow.find("td:eq(1)").text();
    //ItemList.TaxRate = currentRow.find("td:eq(2)").text().replace('%', '');
    //ItemList.TaxLevel = currentRow.find("td:eq(3)").text();
    //ItemList.TaxValue = currentRow.find("td:eq(5)").text();
    //ItemList.TaxApplyOn = currentRow.find("td:eq(6)").text();
    ItemList.TaxID = currentRow.find("#taxid").text();
    ItemList.TaxRate = currentRow.find("#taxrate").text().replace('%', '');
    ItemList.TaxLevel = currentRow.find("#taxlevel").text();
    ItemList.TaxValue = currentRow.find("#taxval").text();
    ItemList.TaxApplyOn = currentRow.find("#taxapplyon").text();
    TaxDetailList.push(ItemList);      
   });           
        }  
    return TaxDetailList;
}
function InsertTaxDetailsForDuplicate() {
    var TaxDetailList = new Array();
    if ($("#TaxCalculatorTbl >tbody >tr").length > 0) {
        $("#TaxCalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var ItemList = {};
            debugger;
            //ItemList.TaxName = currentRow.find("td:eq(0)").text();
            //ItemList.TaxID = currentRow.find("td:eq(1)").text();
            //ItemList.TaxRate = currentRow.find("td:eq(2)").text().replace('%', '');
            //ItemList.TaxLevel = currentRow.find("td:eq(3)").text();
            //ItemList.TaxApplyOnName = currentRow.find("td:eq(4)").text();
            //ItemList.TaxValue = currentRow.find("td:eq(5)").text();
            //ItemList.TaxApplyOn = currentRow.find("td:eq(6)").text();
            ItemList.TaxName = currentRow.find("#taxname").text();
            ItemList.TaxID = currentRow.find("#taxid").text();
            ItemList.TaxRate = currentRow.find("#taxrate").text().replace('%', '');
            ItemList.TaxLevel = currentRow.find("#taxlevel").text();
            ItemList.TaxApplyOnName = currentRow.find("#taxapplyonname").text();
            ItemList.TaxValue = currentRow.find("#taxval").text();
            ItemList.TaxApplyOn = currentRow.find("#taxapplyon").text();
            TaxDetailList.push(ItemList);
        });
    }
    return TaxDetailList;
}
function InsertBranchDetail() {
    var BranchDetailList = new Array();
    if ($("#CustBrDetail >tbody >tr").length > 0) {
        $("#CustBrDetail >tbody >tr").each(function () {
            var currentRow = $(this);
            var List = {};

            var brid = currentRow.find("#hdCustomerBranchId").val();
            List.brid = brid;
            if (currentRow.find("#cust_act_stat_" + brid).is(":checked")) {
                List.ActiveStatus = "Y";
            }
            else {
                List.ActiveStatus = "N";
            }
            BranchDetailList.push(List);
        });
    }
    return BranchDetailList;
}
function InsertModuleDetail() {
    var DetailList = new Array();
    if ($("#ModuleDetail >tbody >tr").length > 0) {
        $("#ModuleDetail >tbody >tr").each(function () {
            var currentRow = $(this);
            var List = {};

            var moduleid = currentRow.find("#hdModuleId").val();
            List.moduleid = moduleid;
            if (currentRow.find("#module_act_stat_" + moduleid).is(":checked")) {
                List.ActiveStatus = "Y";
            }
            else {
                List.ActiveStatus = "N";
            }
            DetailList.push(List);
        });
    }
    return DetailList;
}
function functionConfirm(event) {
    debugger;
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
            debugger
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();

           // return true;
        } else {
            return false;
        }
    });
    return false;
}
function InsertHSNNumberDetails() {
    var DetailList = new Array();
    if ($("#datatable-buttons1 >tbody >tr").length > 0) {
        $("#datatable-buttons1 >tbody >tr").each(function () {
            var currentRow = $(this);
            var ItemList = {};
            debugger;
            /*var HSNNumber = currentRow.find("td:eq(1)").text();*/
            var HSNNumber = currentRow.find("#hsn_no").text();
            if (HSNNumber != "" && HSNNumber != null) {
                /* ItemList.HSNNumber = currentRow.find("td:eq(1)").text();*/
                ItemList.HSNNumber = currentRow.find("#hsn_no").text();
                DetailList.push(ItemList);
            }
            //DetailList.push(ItemList);
        });
    }
    return DetailList;
}

function OnChangeTamplateName() {    
    CheckVallidation("TemplateName", "vmTemplateName")
}
function OnChangeBaseAmount() {
    var DecDigit = $("#ValDigit").text();
    var AssessVal = $("#Tax_AssessableValue").val();
    if (AvoidDot(AssessVal) == false) {
        AssessVal = 0;
    }
    var BaseAmt = $("#Tax_AssessableValue").val();
    debugger;
    if (BaseAmt == "0" || BaseAmt == "") {
        debugger;
        $('#SpanBaseAmount').text($("#valueReq").text());
        $("#SpanBaseAmount").css("display", "block");
        $("#Tax_AssessableValue").css("border-color", "red");
        $("#Tax_Type").attr("disabled", true);
        $("#Tax_Percentage").attr("disabled", true);
        $("#Tax_Level").attr("disabled", true);
        $("#Tax_ApplyOn").attr("disabled", true);
        $("#Btn_AddNew").css("display", "none");
        $("#ResetTD").css("display", "none");
        $("#Tax_Type").val(0);
        $("#Tax_Level").val(0)
        $("#Tax_Percentage").val(null);
        $("#Tax_Amount").val(null);
    }
    else {
        $("#SpanBaseAmount").css("display", "none");
        $("#Tax_AssessableValue").css("border-color", "#ced4da");
        $("#Tax_Type").attr("disabled", false);
        $("#Tax_Percentage").attr("disabled", false);
        $("#Tax_Level").attr("disabled", false);
        $("#Tax_ApplyOn").attr("disabled", false);
        $("#Btn_AddNew").css("display", "block");
        $("#ResetTD").css("display", "block");
        $("#Tax_Type").val();
        $("#Tax_Level").val();
        $("#Tax_Percentage").val();
        $("#Tax_Amount").val();
    }


    var TotalTaxAmount = 0;
    if ($("#TaxCalculatorTbl >tbody >tr").length > 0) {
        $("#TaxCalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            debugger;
           //var TaxID = currentRow.find("td:eq(1)").text();
           // var TaxPec = currentRow.find("td:eq(2)").text().replace('%', '');
           // var TaxLevel = currentRow.find("td:eq(3)").text();
           // var TaxValue = currentRow.find("td:eq(5)").text();
           // var TaxApplyOn = currentRow.find("td:eq(6)").text();
            var TaxID = currentRow.find("#taxid").text();
            var TaxPec = currentRow.find("#taxrate").text().replace('%', '');
            var TaxLevel = currentRow.find("#taxlevel").text();
            var TaxValue = currentRow.find("#taxval").text();
            var TaxApplyOn = currentRow.find("#taxapplyon").text();
            var TaxAmount = 0;
            var TotalTaxAmt = 0;
            if (TaxApplyOn == "I") {
                if (TaxLevel == "1") {
                    TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                    TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                }
                else {
                    var TaxAMountColIL = parseFloat(0).toFixed(DecDigit);
                    var TaxLevelTbl = parseInt(TaxLevel) - 1;

                    $("#TaxCalculatorTbl >tbody >tr").each(function () {
                        var currentRw = $(this);
                        //var Level = currentRw.find("td:eq(3)").text();
                        //var TaxAmtLW = currentRw.find("td:eq(5)").text();
                        var Level = currentRw.find("#taxlevel").text();
                        var TaxAmtLW = currentRw.find("#taxval").text();
                        if (TaxLevelTbl == Level) {
                            TaxAMountColIL = (parseFloat(TaxAMountColIL) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
                        }
                    });
                  
                    var FinalAssAmtIL = (parseFloat(TaxAMountColIL)).toFixed(DecDigit);
                    TaxAmount = ((parseFloat(FinalAssAmtIL) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                    TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                }
            }
            if (TaxApplyOn == "C") {
                var Level = TaxLevel;
                if (TaxLevel == "1") {
                    TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                    TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                }
                else {
                    var TaxAMountCol = parseFloat(0).toFixed(DecDigit);
                    $("#TaxCalculatorTbl >tbody >tr").each(function () {
                        var currentRw = $(this);
                        //var Level = currentRw.find("td:eq(3)").text();
                        //var TaxAmtLW = currentRw.find("td:eq(5)").text();
                        var Level = currentRw.find("#taxlevel").text();
                        var TaxAmtLW = currentRw.find("#taxval").text();
                        if (TaxLevel > Level) {
                            TaxAMountCol = (parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
                        }
                    });
                   
                  
                    var FinalAssAmt = (parseFloat(AssessVal) + parseFloat(TaxAMountCol)).toFixed(DecDigit);
                    TaxAmount = ((parseFloat(FinalAssAmt) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                    TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                }
            }
            /*currentRow.find("td:eq(5)").text(TotalTaxAmt);*/
            currentRow.find("#taxval").text(TotalTaxAmt);
            TotalTaxAmount = parseFloat(TotalTaxAmount) + parseFloat(TotalTaxAmt);
          
        });
        $("#TotalTaxAmount").text(parseFloat(TotalTaxAmount).toFixed(DecDigit));
    }  
    $("#Tax_AssessableValue").val(parseFloat(AssessVal).toFixed(DecDigit));
}
function OnchangeHsnNumbar() {
    if (CheckVallidation("HSNNumber", "vmHSNNumber") == false) {
        $("[aria-labelledby='select2-HSNNumber-container']").css("border-color", "red");
    }
    else {
        $("[aria-labelledby='select2-HSNNumber-container']").css("border-color", "#ced4da");
    }
}

function ResetTaxDetails() {
    debugger;
    $("#TaxCalculatorTbl >tbody>tr").remove();
    $("#TotalTaxAmount").text("");//
    $("#Tax_Percentage").val("");
    $("#Tax_Amount").val("");
    $("#SpanTaxName").css("display", "none");
    $("#Tax_Type").css("border-color", "#ced4da");

    $("#SpanTaxPercent").css("display", "none");
    $("#Tax_Percentage").css("border-color", "#ced4da");

    $("#SpanTaxLevel").css("display", "none");
    $("#Tax_Level").css("border-color", "#ced4da");

    $("#SpanTaxAmounterr").css("display", "none");
    $("#Tax_Amount").css("border-color", "#ced4da");
    $('#Tax_ApplyOn').val("I").prop('selected', true);
    BindTaxTemplateTaxList("", "", "");

}
function ResetTaxTemplateLevelVal() {
    debugger;
    var level = [];// new Array();
    var maxval;
    if ($("#TaxCalculatorTbl tbody tr").length > 0) {
        $("#TaxCalculatorTbl tbody tr").each(function () {
            debugger;
            var currentRow = $(this);
            /*var tax_id = currentRow.find("td:eq(1)").text();*/
            var tax_id = currentRow.find("#taxid").text();
            $("#Tax_Type option[value=" + tax_id + "]").hide();
            /*level.push(currentRow.find("td:eq(3)").text());*/
            level.push(currentRow.find("#taxlevel").text());
        });
    } 
 
    if (level.length > 0) {
        maxval = Math.max(...level);
    }
    else {
        maxval = 0;
    }
    if (maxval == 0) {

        $("#Tax_Level option[value=" + 1 + "]").show();
        for (var k = 2; k <= 10; k++) {
            $("#Tax_Level option[value=" + k + "]").hide();
        }
        $('#Tax_Level').val("0").prop('selected', true);
    }
    else {
        for (i = 1; i <= maxval; i++) {
            if ((maxval - i) == 0) {
                $("#Tax_Level option[value=" + i + "]").show();
            } else {
                $("#Tax_Level option[value=" + i + "]").hide();
            }
          
        }
        $("#Tax_Level option[value=" + i + "]").show();
        var j = i + 1;
        for (var k = j; k <= 10; k++) {
            $("#Tax_Level option[value=" + k + "]").hide();
        }
        $('#Tax_Level').val("0").prop('selected', true);
    }

}
function OnclickTaxType() {
    BindTaxTemplateTaxList("", "", "");
}
function BindTaxTemplateTaxList(UserID, Sno, ItemID) {
    debugger;
    var type = "";
    if ($("#TDS").is(":checked")) {
        type = "TDS";
    } else if ($("#TAX").is(":checked")) {
        type = "TAX";
    }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/LSODetail/GetSOTaxTypeList",
        dataType: "json",
        data: { type: type},
        success: function (data) {
            if (data == 'ErrorPage') {
                LSO_ErrorPage();
                return false;
            }
            debugger;
            if (data !== null && data !== "") {
                var arr = [];
                arr = JSON.parse(data);
                var s = '<option value="0">---Select---</option>';
                for (var i = 0; i < arr.Table.length; i++) {
                    s += '<option value="' + arr.Table[i].tax_id + '" text="' + arr.Table[i].tax_name + '">' + arr.Table[i].tax_name + '</option>';
                }
                $("#Tax_Type").html(s);        
                ResetTaxTemplateLevelVal();
            }
        },
        error: function (Data) {

        }
    });
};

function AddNewHsnNumber() {
    
    if (CheckVallidation("HSNNumber", "vmHSNNumber") == false) {
        $("[aria-labelledby='select2-HSNNumber-container']").css("border-color", "red");
        return false;
    }
    else {
        $("[aria-labelledby='select2-HSNNumber-container']").css("border-color", "#ced4da");
    }
    debugger;
    var rowIdx = 0;
    var TblLen = $("#datatable-buttons1 TBODY TR").length;
    if (TblLen == 1) {
        var sno = $("#spanrowid").text();
        if (sno != "" && sno != null) {
            rowIdx = TblLen + 1;
        }
        else {
            rowIdx = TblLen;
        }
    }
    else {
        rowIdx = TblLen + 1;
    }
    var deletetext = $("#Span_Delete_Title").text();
    var t = $('#datatable-buttons1').DataTable();   
    var hsnnumber = $("#HSNNumber").val();
   
        t.row.add([
            `<td class="sorting_1"><span id="hdn_id">${rowIdx}</span></td>`,
            `<td><span id="hsn_no">${hsnnumber}</span></td>`,
            `<td>
             <i class="fa fa-trash red" aria-hidden="true" title="${deletetext}" onclick="DeleteHSNNumber(this,event)"></i>
            </td>`
        ]).draw();

   // $("#HSNNumber option[value=" + hsnnumber + "]").hide();
    $("#HSNNumber").attr("onchange","");
    $("#HSNNumber").val("0").trigger('change');
    $("#HSNNumber").attr("onchange", "OnchangeHsnNumbar()");
    resetHSNSrNumber();    
}
function resetHSNSrNumber() {
    var SrNo = 0;
    $('#datatable-buttons1 tbody tr').each(function () {
        var currentRow = $(this);
        SrNo = parseInt(SrNo) + 1;
        //currentRow.find("td:eq(0)").text(SrNo);
        currentRow.find("#hdn_id").text(SrNo);
    });
}
function DeleteHSNNumber(el, e) {
    debugger
    //var hsnnumber = $(e.target).closest('tr').find("td:eq(1)").text();
    var hsnnumber = $(e.target).closest('tr').find("#hsn_no").text();
    $("#HSNNumber option[value=" + hsnnumber + "]").show();    
    var i = el.parentNode.parentNode.rowIndex - 1;
    var table = $('#datatable-buttons1').DataTable();
    table.row(i).remove().draw(false);
    if (parseInt($('#datatable-buttons1 tbody tr td:eq(0)').text()) > 0) {
        resetHSNSrNumber();
    }    
}
function OnLoadHSNNumber() {
    $("#datatable-buttons1 >tbody >tr").each(function () {
        var currentRow = $(this);
        //var HsnNumber = currentRow.find("td:eq(1)").text();
        var HsnNumber = currentRow.find("#hsn_no").text();
        $("#HSNNumber option[value=" + HsnNumber + "]").hide();
    })
}

function FloatValueValueOnly(el, evt) {
    
    if (Cmn_FloatValueonly(el, evt, "#ValDigit") == false) {
        return false;
    }
    else {
        return true;
    }
}
function FloatValuePerOnly(el,evt) {
    if (Cmn_PercentageValueonly(el, evt) == false) {
        return false;
    } else {
        return true;
    }
}

function SearchableHSN() {
   

    $("#HSNNumber").select2({        
        templateResult: function (data) {
            debugger
            var selected = $("#HSNNumber").val();
            if (checkHSN(data, selected, "#datatable-buttons1",  "#HSNNumber") == true) {                          
                var $result = $(                   
                    '<div class="row">' +
                    '<div class="col-md-12 col-sm-12">' + data.text + '</div>' +                  
                    '</div>'
                );
                return $result;
            }
            firstEmptySelect = false;
        }
    });

}
function checkHSN(data, selected, TableNameID, ItemDDlNameID) {

    var Flag = "N";
    $(TableNameID + " tbody tr").each(function () {
        var currentRow = $(this);
        debugger;
        //var rowno = currentRow.find(HiddenSrNoID).val();
        var itemId = currentRow.find("td:eq(1)").text();
        //var itemId = currentRow.find("#hsnno").text();
        if (itemId != null) {
            if (itemId == data.id) {
                if (itemId == "0") {
                }
                else if (selected == itemId) {
                }
                else {
                    Flag = "Y";
                }
                return false;
            }
        }

    });
    debugger;
    if (Flag == "Y") {
        return false;
    }
    else {
        return true;
    }
}