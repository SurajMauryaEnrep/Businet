
$(document).ready(function () {

    $("igst_tax_perc").select2();
    $("#igst_tax_id").select2({
        templateResult: function (data) {
            debugger
            var Selected = $("#igst_tax_id").val();
            if (checkK(data, Selected) == true) {
                var classAttr = $(data.element).attr('class');
                var hasClass = typeof classAttr != 'undefined';
                classAttr = hasClass ? ' ' + classAttr : '';
                var $result;
                $result = $(

                    '<div ' + classAttr + '">' + data.text + '</div>'
                );
                return $result;
            }
            firstEmptySelect = false;
        },
    });
    $("#rcm_igst_tax_id").select2({
        templateResult: function (data) {
            debugger
            var Selected = $("#rcm_igst_tax_id").val();
            if (checkK(data, Selected) == true) {
                var classAttr = $(data.element).attr('class');
                var hasClass = typeof classAttr != 'undefined';
                classAttr = hasClass ? ' ' + classAttr : '';
                var $result;
                $result = $(

                    '<div ' + classAttr + '">' + data.text + '</div>'
                );
                return $result;
            }
            firstEmptySelect = false;
        },
    });
    $("#sgst_tax_id").select2({
        templateResult: function (data) {
            debugger
            var Selected = $("#sgst_tax_id").val();
            if (checkK(data, Selected) == true) {
                var classAttr = $(data.element).attr('class');
                var hasClass = typeof classAttr != 'undefined';
                classAttr = hasClass ? ' ' + classAttr : '';
                var $result;
                $result = $(

                    '<div ' + classAttr + '">' + data.text + '</div>'
                );
                return $result;
            }
            firstEmptySelect = false;
        },
    });
    $("#rcm_sgst_tax_id").select2({
        templateResult: function (data) {
            debugger
            var Selected = $("#rcm_sgst_tax_id").val();
            if (checkK(data, Selected) == true) {
                var classAttr = $(data.element).attr('class');
                var hasClass = typeof classAttr != 'undefined';
                classAttr = hasClass ? ' ' + classAttr : '';
                var $result;
                $result = $(

                    '<div ' + classAttr + '">' + data.text + '</div>'
                );
                return $result;
            }
            firstEmptySelect = false;
        },
    });
    $("#cgst_tax_id").select2({
        templateResult: function (data) {
            debugger
            var Selected = $("#cgst_tax_id").val();
            if (checkK(data, Selected) == true) {
                var classAttr = $(data.element).attr('class');
                var hasClass = typeof classAttr != 'undefined';
                classAttr = hasClass ? ' ' + classAttr : '';
                var $result;
                $result = $(

                    '<div ' + classAttr + '">' + data.text + '</div>'
                );
                return $result;
            }
            firstEmptySelect = false;
        },
    });
    $("#rcm_cgst_tax_id").select2({
        templateResult: function (data) {
            debugger
            var Selected = $("#rcm_cgst_tax_id").val();
            if (checkK(data, Selected) == true) {
                var classAttr = $(data.element).attr('class');
                var hasClass = typeof classAttr != 'undefined';
                classAttr = hasClass ? ' ' + classAttr : '';
                var $result;
                $result = $(

                    '<div ' + classAttr + '">' + data.text + '</div>'
                );
                return $result;
            }
            firstEmptySelect = false;
        },
    });
    var EditData = $("#EditData").val();
    if (EditData == "Y") {
        $("#SaveBtnEnable").css("display", "block");
        $("#igst_tax_perc").attr("disabled", false);
        $("#igst_tax_id").attr("disabled", false);
        $("#sgst_tax_id").attr("disabled", false);
        $("#cgst_tax_id").attr("disabled", false);
        $("#rcm_igst_tax_id").attr("disabled", false);
        $("#rcm_sgst_tax_id").attr("disabled", false);
        $("#rcm_cgst_tax_id").attr("disabled", false);
    }
   // EditAttrName();
   // EditAttrValName();
})

function checkK(data,Selected) {
    debugger;
    var IGST = $("#igst_tax_id").val();
    var SGST = $("#sgst_tax_id option:selected").val()
    var CGST = $("#cgst_tax_id option:selected").val()
    var RCM_IGST = $("#rcm_igst_tax_id option:selected").val()
    var RCM_SGST = $("#rcm_sgst_tax_id option:selected").val()
    var RCM_CGST = $("#rcm_cgst_tax_id option:selected").val()
    arr = [];
    arr.push(IGST, SGST, CGST, RCM_IGST, RCM_SGST, RCM_CGST);
    //arr.includes(data.id)
    arr = $.grep(arr, function (n) {
        return n != Selected;
    });
    //var Flag = "N";
    if (arr.includes(data.id) == false) {
        return true;
    }

}
function CheckValidation() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 03-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#btn_save").prop("disabled", true); /*End*/
    }

    var Flag = true;
   var IgstPer = $("#igst_tax_perc").val();
    if (IgstPer != 0) {
        document.getElementById("vmigst_tax_perc").innerHTML = "";
        $("#igst_tax_perc").css("border-color", "#ced4da");
        $("#vmigst_tax_perc").css("display", "none");
    }
    else {
        document.getElementById("vmigst_tax_perc").innerHTML = $("#valueReq").text();
        $("#igst_tax_perc").css("border-color", "red");
        $("#vmigst_tax_perc").css("display", "block");
        Flag = false;
    }

    var IgstTaxId = $("#igst_tax_id").val();
    if (IgstTaxId == "0") {
        $('#vmigst_tax_id').text($("#valueReq").text());
        $("#igst_tax_id").css("border-color", "red");
        $("[aria-labelledby='select2-igst_tax_id-container']").css("border-color", "red");
        $("#vmigst_tax_id").css("display", "block");
        Flag = false;
    }

  
    var  SgstTaxId = $("#sgst_tax_id").val();
    if (SgstTaxId == "0") {
        $('#vmsgst_tax_id').text($("#valueReq").text());
        $("#sgst_tax_id").css("border-color", "red");
        $("[aria-labelledby='select2-sgst_tax_id-container']").css("border-color", "red");
        $("#vmsgst_tax_id").css("display", "block");
        Flag = false;
    }

    var CgstTaxId = $("#cgst_tax_id").val();
    if (CgstTaxId == "0") {
        $('#vmcgst_tax_id').text($("#valueReq").text());
        $("#cgst_tax_id").css("border-color", "red");
        $("[aria-labelledby='select2-cgst_tax_id-container']").css("border-color", "red");
        $("#vmcgst_tax_id").css("display", "block");
        Flag = false;
    }


    var  RCMIgstTaxId = $("#rcm_igst_tax_id").val();
    if (RCMIgstTaxId == "0") {
        $('#vmrcm_igst_tax_id').text($("#valueReq").text());
        $("#rcm_igst_tax_id").css("border-color", "red");
        $("[aria-labelledby='select2-rcm_igst_tax_id-container']").css("border-color", "red");
        $("#vmrcm_igst_tax_id").css("display", "block");
        Flag = false;
    }

    var RCMSgstTaxId = $("#rcm_sgst_tax_id").val();
    debugger;
    if (RCMIgstTaxId == "0") {
        $('#vmrcm_sgst_tax_id').text($("#valueReq").text());
        $("#rcm_sgst_tax_id").css("border-color", "red");
        $("[aria-labelledby='select2-rcm_sgst_tax_id-container']").css("border-color", "red");
        $("#vmrcm_sgst_tax_id").css("display", "block");
        Flag = false;
    }

    var  RCMCgstTaxId = $("#rcm_cgst_tax_id").val();
    debugger;
    debugger;
    if (RCMCgstTaxId == "0") {
        $('#vmrcm_cgst_tax_id').text($("#valueReq").text());
        $("#rcm_cgst_tax_id").css("border-color", "red");
        $("[aria-labelledby='select2-rcm_cgst_tax_id-container']").css("border-color", "red");
        $("#vmrcm_cgst_tax_id").css("display", "block");
        Flag = false;
    }
    if (SgstTaxId != "0") {
        if (SgstTaxId === CgstTaxId) {
            $('#vmsgst_tax_id').text($("#valueduplicate").text());
            $("#vmsgst_tax_id").css("display", "block");
            $("[aria-labelledby='select2-sgst_tax_id-container']").css("border-color", "red");
            Flag = false;
        }
        else {
            $("#vmsgst_tax_id").css("display", "none");
            $("[aria-labelledby='select2-sgst_tax_id-container']").css("border-color", "#ced4da");
        }
    }
    if (CgstTaxId != "0") {
        if (SgstTaxId === CgstTaxId) {
            $('#vmcgst_tax_id').text($("#valueduplicate").text());
            $("#vmcgst_tax_id").css("display", "block");
            $("[aria-labelledby='select2-cgst_tax_id-container']").css("border-color", "red");
            Flag = false;
        }
        else {
            $("#vmcgst_tax_id").css("display", "none");
            $("[aria-labelledby='select2-cgst_tax_id-container']").css("border-color", "#ced4da");
        }
    }
    var  RcmSgstTax_Id = $("#rcm_sgst_tax_id").val();
    if (RcmSgstTax_Id != "0") {
        if (RCMSgstTaxId === RCMCgstTaxId) {
            $('#vmrcm_sgst_tax_id').text($("#valueduplicate").text());
            $("#vmrcm_sgst_tax_id").css("display", "block");
            $("[aria-labelledby='select2-rcm_sgst_tax_id-container']").css("border-color", "red");
            Flag = false;
        }
        else {
            $("#vmrcm_sgst_tax_id").css("display", "none");
            $("[aria-labelledby='select2-rcm_sgst_tax_id-container']").css("border-color", "#ced4da");
        }
    }
    if (RCMCgstTaxId != "0") {
        if (RCMSgstTaxId === RCMCgstTaxId) {
            $('#vmrcm_cgst_tax_id').text($("#valueduplicate").text());
            $("#vmrcm_cgst_tax_id").css("display", "block");
            $("[aria-labelledby='select2-rcm_cgst_tax_id-container']").css("border-color", "red");
            Flag = false;
        }
        else {
            $("#vmrcm_cgst_tax_id").css("display", "none");
            $("[aria-labelledby='select2-rcm_cgst_tax_id-container']").css("border-color", "#ced4da");
        }
    }

    if (Flag == true) {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");
        return true;
    }
    else {
        return false;
    }
}
//function OnChangeIgstTax() {
//    debugger;
//    var SGST = $("#sgst_tax_id option:selected").text()
//    var CGST = $("#cgst_tax_id option:selected").text()
//    var RCM_IGST = $("#rcm_igst_tax_id option:selected").text()
//    var RCM_SGST = $("#rcm_sgst_tax_id option:selected").text()
//    var RCM_CGST = $("#rcm_cgst_tax_id option:selected").text()
//}
function OnChangeIgstTax() {
    debugger;
    IgstTaxId = $("#igst_tax_id").val();
    if (IgstTaxId == "0") {
        $('#vmigst_tax_id').text($("#valueReq").text());
        $("#vmigst_tax_id").css("display", "block");
        $("[aria-labelledby='select2-igst_tax_id-container']").css("border-color", "red");

    }
    else {
        $("#vmigst_tax_id").css("display", "none");
        $("[aria-labelledby='select2-igst_tax_id-container']").css("border-color", "#ced4da");
    }
}

function OnChangeSgstTax() {
    debugger;
    SgstTaxId = $("#sgst_tax_id").val();
    CgstTaxId = $("#cgst_tax_id").val();
    if (SgstTaxId == "0") {
        $('#vmsgst_tax_id').text($("#valueReq").text());
        $("#vmsgst_tax_id").css("display", "block");
        $("[aria-labelledby='select2-sgst_tax_id-container']").css("border-color", "red");
    }
    else {
        $("#vmsgst_tax_id").css("display", "none");
        $("[aria-labelledby='select2-sgst_tax_id-container']").css("border-color", "#ced4da");
    }
    if (SgstTaxId === CgstTaxId) {
        $('#vmsgst_tax_id').text($("#valueduplicate").text());
        $("#vmsgst_tax_id").css("display", "block");
        $("[aria-labelledby='select2-sgst_tax_id-container']").css("border-color", "red");
    }
    else {
        $("#vmsgst_tax_id").css("display", "none");
        $("[aria-labelledby='select2-sgst_tax_id-container']").css("border-color", "#ced4da");
    }
}
function OnChangeCgstTax() {
    debugger;
    CgstTaxId = $("#cgst_tax_id").val();
    var SGST = $("#sgst_tax_id").val();
    if (CgstTaxId == "0") {
        $('#vmcgst_tax_id').text($("#valueReq").text());
        $("#vmcgst_tax_id").css("display", "block");
        $("[aria-labelledby='select2-cgst_tax_id-container']").css("border-color", "red");

    }
    else {
        $("#vmcgst_tax_id").css("display", "none");
        $("[aria-labelledby='select2-cgst_tax_id-container']").css("border-color", "#ced4da");
    }
  
    if (SGST === CgstTaxId) {
        $('#vmcgst_tax_id').text($("#valueduplicate").text());
        $("#vmcgst_tax_id").css("display", "block");
        $("[aria-labelledby='select2-cgst_tax_id-container']").css("border-color", "red");
    }
    else {
        $("#vmcgst_tax_id").css("display", "none");
        $("[aria-labelledby='select2-cgst_tax_id-container']").css("border-color", "#ced4da");
    }
}

function OnChangeRCMIgstTax() {
    debugger;
    RcmIgstTaxId = $("#rcm_igst_tax_id").val();
    if (RcmIgstTaxId == "0") {
        $('#vmrcm_igst_tax_id').text($("#valueReq").text());
        $("#vmrcm_igst_tax_id").css("display", "block");
        $("[aria-labelledby='select2-rcm_igst_tax_id-container']").css("border-color", "red");

    }
    else {
        $("#vmrcm_igst_tax_id").css("display", "none");
        $("[aria-labelledby='select2-rcm_igst_tax_id-container']").css("border-color", "#ced4da");
    }
}
function OnChangeRCMSgstTax() {
    debugger;
    RcmSgstTaxId = $("#rcm_sgst_tax_id").val();
    RcmcgstTaxId = $("#rcm_cgst_tax_id").val();
    if (RcmSgstTaxId == "0") {
        $('#vmrcm_sgst_tax_id').text($("#valueReq").text());
        $("#vmrcm_sgst_tax_id").css("display", "block");
        $("[aria-labelledby='select2-rcm_sgst_tax_id-container']").css("border-color", "red");

    }
    else {
        $("#vmrcm_sgst_tax_id").css("display", "none");
        $("[aria-labelledby='select2-rcm_sgst_tax_id-container']").css("border-color", "#ced4da");
    }
    if (RcmSgstTaxId === RcmcgstTaxId) {
        $('#vmrcm_sgst_tax_id').text($("#valueduplicate").text());
        $("#vmrcm_sgst_tax_id").css("display", "block");
        $("[aria-labelledby='select2-rcm_sgst_tax_id-container']").css("border-color", "red");
    }
    else {
        $("#vmrcm_sgst_tax_id").css("display", "none");
        $("[aria-labelledby='select2-rcm_sgst_tax_id-container']").css("border-color", "#ced4da");
    }
}
function OnChangeRCMCgstTax() {
    debugger;
    RcmSgstTaxId = $("#rcm_sgst_tax_id").val();
    RcmCgstTaxId = $("#rcm_cgst_tax_id").val();
    if (RcmCgstTaxId == "0") {
        $('#vmrcm_cgst_tax_id').text($("#valueReq").text());
        $("#vmrcm_cgst_tax_id").css("display", "block");
        $("[aria-labelledby='select2-rcm_cgst_tax_id-container']").css("border-color", "red");

    }
    else {
        $("#vmrcm_cgst_tax_id").css("display", "none");
        $("[aria-labelledby='select2-rcm_cgst_tax_id-container']").css("border-color", "#ced4da");
    }
    if (RcmSgstTaxId === RcmCgstTaxId) {
        $('#vmrcm_cgst_tax_id').text($("#valueduplicate").text());
        $("#vmrcm_cgst_tax_id").css("display", "block");
        $("[aria-labelledby='select2-rcm_cgst_tax_id-container']").css("border-color", "red");
    }
    else {
        $("#vmrcm_cgst_tax_id").css("display", "none");
        $("[aria-labelledby='select2-rcm_cgst_tax_id-container']").css("border-color", "#ced4da");
    }
}
function OnChangeTaxPerc() {
    debugger;
    IgstPer = $("#igst_tax_perc").val();

    if (IgstPer != 0) {
        document.getElementById("vmigst_tax_perc").innerHTML = "";
        $("#igst_tax_perc").css("border-color", "#ced4da");
        DivideTaxPerc(IgstPer)

    }
    else {
        document.getElementById("vmigst_tax_perc").innerHTML = $("#valueReq").text();
        $("#igst_tax_perc").css("border-color", "red");
    }
}
function DivideTaxPerc(IgstPer) {
    debugger;
    var gst = IgstPer / 2;
    $("#sgst_tax_perc").val(gst);
    $("#cgst_tax_perc").val(gst);
}


function functionConfirm(event) {
    $("#GstBody").bind("click", function (e) {
        debugger;
        try {
            var clickedrow = $(e.target).closest("tr");
            var Tax_id = clickedrow.find("#igsttaxperc").text();
            $("#hdnigsttaxperc").val(Tax_id);
        }
        catch (err) {
            debugger
            alert(err.message);
        }
    });
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
            $("#hdnAction").val("Delete");
            $('form').submit();
            return true;
        } else {
            return false;
        }
    });
    return false;
}
function OnClickEditButton() {
    debugger;
    //var RateDecDigit = $("#RateDigit").text()
    $("#GstBody").bind("click", function (e) {
        debugger;
        try {
            $("#hdnSavebtn").val(null);
            $("#hdnTranstype").val("Update")
            var clickedrow = $(e.target).closest("tr");
            var Tax_id = clickedrow.find("#igsttaxperc").text();
            var AddNewData = $("#AddNewData").val();
            if (AddNewData == "N") {
                EditData = "Y";
            }
                         
            window.location.href = "/BusinessLayer/TaxStructure/EditTaxStructure?Tax_id=" + Tax_id + "&EditData=" + EditData;
            debugger;
            
            //if (AddNewData == "N") {
            //    $("#SaveBtnEnable").css("display", "block");
            //    $("#igst_tax_perc").attr("disabled", false);
            //    $("#igst_tax_id").attr("disabled", false);
            //    $("#sgst_tax_id").attr("disabled", false);
            //    $("#cgst_tax_id").attr("disabled", false);
            //    $("#rcm_igst_tax_id").attr("disabled", false);
            //    $("#rcm_sgst_tax_id").attr("disabled", false);
            //    $("#rcm_cgst_tax_id").attr("disabled", false);
            //}
           
        }
        catch (err) {
        }
    });
}