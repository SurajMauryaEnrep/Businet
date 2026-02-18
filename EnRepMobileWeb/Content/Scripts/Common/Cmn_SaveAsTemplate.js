
var btnExit1 = document.getElementById("TaxExitAndDiscard1TDS");
var span1 = document.getElementsByClassName("Closebtn")[1];
var modal2 = document.getElementById("TDS_SaveAsTemplate");
if (span1 != null) {
    span1.onclick = function () {
        debugger
        modal2.style.display = "none";

    }
    btnExit1.onclick = function () {
        modal2.style.display = "none";

    }
}

function SaveTaxTemplateFromCalc(flag) {
    if (CheckVallidation("TemplateName" + flag, "TemplateNameErrorMsg" + flag) == false) {
        return false;
    }
    if (flag == "TDS") {
        if ($("#TDS_CalculatorTbl >tbody >tr").length <= 0) {
            swal("", $("#NoTaxSelected").text(), "warning");
            return false;
        }
    } else {
        if ($("#TaxCalculatorTbl >tbody >tr").length <= 0) {
            swal("", $("#NoTaxSelected").text(), "warning");
            return false;
        }
    }
    
    var FinalTaxDetail = [];

    var FinalBranchDetail = [];
    var FinalModuleDetail = [];
    var FinalHSNDetail = [];
    var TotalDetail = [];

    FinalTaxDetail = Cmn_frTxTmplt_InsertTaxDetails(flag);
    FinalBranchDetail = Cmn_frTxTmplt_InsertBranchDetail();
    FinalModuleDetail = Cmn_frTxTmplt_InsertModuleDetail();
    FinalHSNDetail = Cmn_frTxTmplt_InsertHSNNumberDetails();


    var TaxDt = JSON.stringify(FinalTaxDetail);
    var BranchDT = JSON.stringify(FinalBranchDetail);
    var ModuleDT = JSON.stringify(FinalModuleDetail);
    var HSNNumberDT = JSON.stringify(FinalHSNDetail);

    debugger
    TotalDetail = InsertTotalDetail(TaxDt, BranchDT, ModuleDT, HSNNumberDT, flag);
    var TotalDT = JSON.stringify(TotalDetail);

    var tmplt_type = "TAX";
    if (flag == "TDS") {
        tmplt_type = "TDS"
    } 
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/TaxTemplate/SaveTaxTemplateFromTaxCalc",
        data: {
            DetailList: TotalDT, tmplt_type: tmplt_type
        },
        success: function (data) {
            debugger;
            var arr = JSON.parse(data);
            if (arr == "Save") {
                if (flag == "TDS") {
                    $("#TemplateNameTDS").css("border-color", "#ced4da");
                    $("#TemplateNameErrorMsgTDS").text("");
                    $("#TemplateNameErrorMsgTDS").css("display", "none");
                } else {
                    $("#TemplateName").css("border-color", "#ced4da");
                    $("#TemplateNameErrorMsg").text("");
                    $("#TemplateNameErrorMsg").css("display", "none");
                }
                

                
                
                if (flag == "TDS") {
                    var NewModal = document.getElementById("TDS_SaveAsTemplate");
                    NewModal.style.display = "none";
                    BindTaxTemplatelist("TDS");
                } else {
                    var NewModal = document.getElementById("SaveAsTemplate");
                    NewModal.style.display = "none";
                    BindTaxTemplatelist("TAX");
                }
            }
            if (arr == "Duplicate") {
                if (flag == "TDS") {
                    $("#TemplateNameTDS").css("border-color", "red");
                    $("#TemplateNameErrorMsgTDS").text($("#valueduplicate").text());
                    $("#TemplateNameErrorMsgTDS").css("display", "block");
                } else {
                    $("#TemplateName").css("border-color", "red");
                    $("#TemplateNameErrorMsg").text($("#valueduplicate").text());
                    $("#TemplateNameErrorMsg").css("display", "block");
                }
                
            }
        }
    })
}
function InsertTotalDetail(TaxDt, BranchDT, ModuleDT, HSNNumberDT,flag) {
    var DetailList = new Array();
    debugger
    var List = {};
    //var a = $("#DocumentMenuId").val();
    //let result = a.substring(0, 6);
    if (flag == "TDS") {
        List.BaseAmount = $("#TDS_AssessableValue").val();
        List.tmplt_name = $("#TemplateName" + flag).val();
    } else {
        List.BaseAmount = $("#Tax_AssessableValue").val();
        List.tmplt_name = $("#TemplateName").val();
    }

    List.TaxDetails = TaxDt;
    List.BranchDetail = BranchDT;
    List.ModuleMapping = ModuleDT;
    List.HSNDetail = HSNNumberDT;
    DetailList.push(List);
    return DetailList
}
function OnchangeTemplateName() {
    if (CheckVallidation("TemplateName", "TemplateNameErrorMsg") == false) {
        return false;
    }
}
function OnClickSaveAsTemplate(flag) {
    debugger
    $("#TemplateName").val("");
    $("#TemplateName" + flag).css("border-color", "#ced4da");
    $("#TemplateName"+flag).val("");
    $("#TemplateNameErrorMsg" + flag).text("");
    $("#TemplateNameErrorMsg"+flag).css("display", "none");
}

function Cmn_frTxTmplt_InsertTaxDetails(flag) {
    var TaxDetailList = new Array();
    if (flag == "TDS") {
        if ($("#TDS_CalculatorTbl >tbody >tr").length > 0) {
            $("#TDS_CalculatorTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var ItemList = {};
                ItemList.TaxID = currentRow.find("#TDS_id").text();
                ItemList.TaxRate = currentRow.find("#TDS_rate").text().replace('%', '');
                ItemList.TaxLevel = currentRow.find("#TDS_level").text();
                ItemList.TaxValue = currentRow.find("#TDS_val").text();
                ItemList.TaxApplyOn = currentRow.find("#TDS_applyon").text();
                TaxDetailList.push(ItemList);
            });
        }
    } else {
        if ($("#TaxCalculatorTbl >tbody >tr").length > 0) {
            $("#TaxCalculatorTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var ItemList = {};
                ItemList.TaxID = currentRow.find("#taxid").text();
                ItemList.TaxRate = currentRow.find("#taxrate").text().replace('%', '');
                ItemList.TaxLevel = currentRow.find("#taxlevel").text();
                ItemList.TaxValue = currentRow.find("#taxval").text();
                ItemList.TaxApplyOn = currentRow.find("#taxapplyon").text();
                TaxDetailList.push(ItemList);
            });
        }
    }
    
    return TaxDetailList;
}
function Cmn_frTxTmplt_InsertBranchDetail() {
    debugger
    var BranchDetailList = new Array();

    var List = {};
    var brid = $("#TopNavBranchList").val();
    List.brid = brid;
    List.ActiveStatus = "Y";
    BranchDetailList.push(List);

    return BranchDetailList;
}
function Cmn_frTxTmplt_InsertModuleDetail() {
    var DetailList = new Array();
    debugger
    var List = {};
    var a = $("#DocumentMenuId").val();
    let result = a.substring(0, 6);
    List.moduleid = result;
    List.ActiveStatus = "Y";
    DetailList.push(List);

    return DetailList;
}
function Cmn_frTxTmplt_InsertHSNNumberDetails() {
    var DetailList = new Array();
    return DetailList;
}
