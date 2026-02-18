

$(document).ready(function () {
    debugger
    $("#datatable-buttons tbody tr").on("click", function (event) {
        //$("#datatable-buttons tbody tr").css("background-color", "#ffffff");
        //$(this).css("background-color", "rgba(38, 185, 154, .16)");
        //var HsnNumber = $(this).find("td:eq(1)").text();
        var HsnNumber = $(this).find("#hsnno").text();
        GetTaxDetailAgainstHSN(HsnNumber);

    });

});

function GetTaxDetailAgainstHSN(HsnNumber) {
    debugger
    var ValDigit = $("#ValDigit").text()
    if (HsnNumber != null && HsnNumber != "") {
        $.ajax({
            type: 'POST',
            url: '/BusinessLayer/HSNCode/GetTaxDetailAgainstHSN',
            data: {
                HsnNumber: HsnNumber
            },
            success: function (data) {
                debugger;
                arr = JSON.parse(data);
                $('#TaxCalculatorTbl tbody tr').remove();
                $("#BaseAmount").val("");
                $('#TotalTaxAmount').text("");
                if (arr.Table.length > 0) {
                    $("#BaseAmount").val(arr.Table[0].base_amt);
                    var TotalAmount = 0;
                    for (var i = 0; i < arr.Table.length; i++) {
                        $('#TaxCalculatorTbl tbody').append(`<tr role="row" class="odd">
                                                     <td id="taxname" width="20%">${arr.Table[i].tax_name}</td>
                                                      <td width="20%" id="taxrate">${arr.Table[i].tax_rate}%</td>
                                                      <td width="21%" id="taxlevel">${arr.Table[i].tax_level}</td>
                                                      <td width="21%" id="taxapplyonname">${arr.Table[i].tax_apply_on}</td>
                                                      <td width="18%" class="num_right" id="taxval">${parseFloat(arr.Table[i].tax_val).toFixed(ValDigit)}</td>
                                                            </tr>`);
                        TotalAmount = parseFloat(TotalAmount) + parseFloat(arr.Table[i].tax_val);

                    }
                    $('#TotalTaxAmount').text(parseFloat(TotalAmount).toFixed(ValDigit));
                }
               

            }
        })
    }


}



//---------------------------------------HSN Number----------------------------------------//
function EditHSNNumber(e,Editable) {
    document.getElementById("vmHSNNumber").innerHTML = "";
    $("#HSNNumber").css("border-color", "#ced4da");
    document.getElementById("vmHSNDescription").innerHTML = "";
    $("#HSNDescription").css("border-color", "#ced4da");
    var CurrentRow = $(e.target).closest('tr');
    //var HsnRemarks = CurrentRow.find("td:eq(3)").text();
    //var HsnNumber = CurrentRow.find("td:eq(1)").text();
    //var DBKCode = CurrentRow.find("td:eq(2)").text();
    var HsnRemarks = CurrentRow.find("#hsnrem").text();
    var HsnNumber = CurrentRow.find("#hsnno").text();
    var DBKCode = CurrentRow.find("#dbkcd").text();
    var HsnDec = CurrentRow.find("#hsndec").text();
    $("#HSNNumber").val(HsnNumber).trigger('change');
    if (Editable == "N") {
        $("#HSNNumber").attr("readonly", true);
    } else {
        $("#HSNNumber").attr("readonly", false);
    }
    $("#hdnHSNNumber").val(HsnNumber);
    $("#DBKCode").val(DBKCode);
    $("#remarks").val(HsnRemarks);//btn_saveBIN
    $("#HSNDescription").val(HsnDec);//btn_saveBIN
    $("#btn_save").val("Update");
    debugger;
    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#SaveBtnEnable").css("display", "block");
        $("#HSNNumber").attr("disabled", false);
        $("#HSNDescription").attr("disabled", false);
        $("#DBKCode").attr("disabled", false);
        $("#remarks").attr("disabled", false);
    }
}
function BinSaveBtnClick() {
    var HSnDes = $('#HSNDescription').val();
    var HSnNum = $('#HSNNumber').val();
    var validationflag = "Y";
    debugger;  
    if (HSnNum == "" || HSnNum == null) {
        document.getElementById("vmHSNNumber").innerHTML = $("#valueReq").text();
        $("#HSNNumber").css("border-color", "red");
        validationflag = "N";
    }
    else {
        document.getElementById("vmHSNNumber").innerHTML = "";
        $("#HSNNumber").css("border-color", "#ced4da");
    }
   
    if (HSnDes == "" || HSnDes == null) {
        document.getElementById("vmHSNDescription").innerHTML = $("#valueReq").text();
        $("#HSNDescription").css("border-color", "red");
        validationflag = "N";
    }
    else {
        document.getElementById("vmHSNDescription").innerHTML = "";
        $("#HSNDescription").css("border-color", "#ced4da");
    }
    if (validationflag == "N") {
        return false;
    }
    else {
        return true;
    }
    //if (CheckVallidation("HSNNumber", "vmHSNNumber") == false) {

    //    return false;
    //}
    //else
    //{
    //    return true;
    //}
  
    
}
function OnChangeHSNNumber() {
    debugger
    var HSnNum = $('#HSNNumber').val();
    if (HSnNum == "" && HSnNum == null) {
        document.getElementById("vmHSNNumber").innerHTML = $("#valueReq").text();
        $("#HSNNumber").css("border-color", "red");
    }
    else {
        document.getElementById("vmHSNNumber").innerHTML = "";
        $("#HSNNumber").css("border-color", "#ced4da");
    }
   /* CheckVallidation("HSNNumber", "vmHSNNumber");*/
}
function OnChangeHSNDes() {
    debugger
    document.getElementById("vmHSNDescription").innerHTML = "";
    $("#HSNDescription").css("border-color", "#ced4da");
}
function OnChangeHSNDescription() {
    debugger
    var HSnDes = $('#HSNDescription').val();
    if (HSnDes == "" && HSnDes == null)
    {
        document.getElementById("vmHSNDescription").innerHTML = $("#valueReq").text();
        $("#HSNDescription").css("border-color", "red");
    }
    else {
        document.getElementById("vmHSNDescription").innerHTML = "";
        $("#HSNDescription").css("border-color", "#ced4da");
    }
}


//---------------------------------------HSN Number End----------------------------------------//

function functionConfirm(e) {

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
            var currentRow = $(e.target).closest('tr');          
                /*var HsnNumber = currentRow.find("td:eq(1)").text();*/
                var HsnNumber = currentRow.find("#hsnno").text();
                $.ajax({
                    type: 'POST',
                    url: '/BusinessLayer/HSNCode/DeleteHSNDetail',
                    data: {
                        HsnNumber
                    },
                    success: function (data) {
                        if (data == "Used") {
                            swal("", $("#DependencyExist").text(), "warning");
                        }
                        if (data == "Deleted") {
                            swal("", $("#deletemsg").text(), "success");
                            currentRow.remove();   
                            $("#HSNNumber").val("");
                            $("#remarks").val("");
                            $("#DBKCode").val("");
                            $("#HSNDescription").val("");
                            $("#vmHSNNumber").val("");
                            $("#vmHSNDescription").val("");
                            document.getElementById("vmHSNNumber").innerHTML = "";
                            $("#HSNNumber").css("border-color", "#ced4da");
                            document.getElementById("vmHSNDescription").innerHTML = "";
                            $("#HSNDescription").css("border-color", "#ced4da");
                            $("#btn_save").val("Save");
                        }
                        return true;
                    }
                })
            //window.location.href = "/BusinessLayer/HSNCode/DeleteHSNDetail/?HsnNumber=" + HsnNumber;
            
        } else {
            return false;
        }
    });
    return false;
}