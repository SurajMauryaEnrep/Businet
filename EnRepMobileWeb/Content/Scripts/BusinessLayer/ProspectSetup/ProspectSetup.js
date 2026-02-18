
$(document).ready(function () {
    //$("#datatable-buttons >tbody").bind("click", function (e) {
    //    debugger;
    //    var clickedrow = $(e.target).closest("tr");
    //    $("#datatable-buttons >tbody >tr").css("background-color", "#ffffff");
    //    $(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");
    //});
    var gst_cat = $("#Gst_Cat").val();
    if (gst_cat == "UR") {
        $("#GSTMidPrt").attr("disabled", true);
        $("#GSTLastPrt").attr("disabled", true);
    }
   
    $("#CmnCountry").select2();
    $("#CmnState").select2();
    $("#CmnDistrict").select2();
    $("#CmnCity").select2();
    $("#ProsListTbl").bind("dblclick", function (e) {
        debugger;
        try {
            var clickedrow = $(e.target).closest("tr");
            //var pros_id = clickedrow.children("td:eq(1)").text();
            var pros_id = clickedrow.children("#hdnprosid").text();
            var BrId = clickedrow.children("#td_BrId").text();
            if (pros_id != "" && pros_id != null) {
                window.location.href = "/BusinessLayer/ProspectSetup/dblckicktoDetail?Pros_id=" + pros_id + "&BranchId=" + BrId;
            }
        }
        catch (err) {
            debugger
            alert(err.message);
        }
    });
    var Disable = $("#Disable").val();
    debugger;
    if (Disable != "Disable") {
        var curr;
        if ($("#Overseas").is(":checked")) {
            curr = "E";
        }
        if ($("#Domestic").is(":checked")) {
            curr = "D";
        }
        if (curr != "D") {
            //$("#Gst_Cat").prop('disabled', true);
            //$("#RequiredGSTCategory").attr("style", "display: none;");
            $("#GSTNumrequired").attr("style", "display: none;");
            $("#DefaultCurrency").css("display", "none");
            $("#DefaultCurrency1").removeClass("col-md-6");
        }
        else {
            //$("#Gst_Cat").prop('disabled', false);
            //$("#RequiredGSTCategory").css("display", "inherit");
            if (gst_cat == "RR") {
                $("#GSTNumrequired").show();
            }
            else {
                $("#GSTNumrequired").hide();
            }
          //  $("#GSTNumrequired").css("display", "inherit");
            $("#DefaultCurrency").css("display", "block");
            $("#DefaultCurrency1").addClass("col-md-6");
        }
    }
    else {
        var curr;
        if ($("#Overseas").is(":checked")) {
            curr = "E";
        }
        if ($("#Domestic").is(":checked")) {
            curr = "D";
        }
        if (curr != "D") {
            //$("#Gst_Cat").prop('disabled', true);
            //$("#RequiredGSTCategory").attr("style", "display: none;");
            $("#DefaultCurrency").css("display", "none");
            $("#DefaultCurrency1").removeClass("col-md-6");
        }
        else {
            //$("#Gst_Cat").prop('disabled', false);
            //$("#RequiredGSTCategory").css("display", "inherit");
            $("#DefaultCurrency").css("display", "block");
            $("#DefaultCurrency1").addClass("col-md-6");
        }
    }
    currvalidationstar();
    /*Commented By Hina on 03-01-2024 to bind onchange of country instead of it*/
    //getCityList();
    //GetsuppDSCntr();
})
function OnChangeGSTCat() {
    debugger;
    var gst_cat = $("#Gst_Cat").val();
    if (gst_cat == "UR") {
        $("#GSTNumrequired").attr("style", "display: none;");

        $("#GSTMidPrt").attr("disabled", true);
        $("#GSTLastPrt").attr("disabled", true);
        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#SpanSuppGSTMidPrt").attr("style", "display: none;");
        document.getElementById("SpanSuppGSTMidPrt").innerHTML = "";

        $("#GSTLastPrt").css('border-color', "#ced4da");
        $("#SpanSuppGST").attr("style", "display: none;");
        document.getElementById("SpanSuppGST").innerHTML = "";

        $("#GSTMidPrt").val(null);
        $("#GSTLastPrt").val(null);
    }
    else if (gst_cat == "EX" || gst_cat == "CO")
    {
        $("#GSTNumrequired").attr("style", "display: none;");

        $("#GSTMidPrt").attr("disabled", false);
        $("#GSTLastPrt").attr("disabled", false);
        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#SpanSuppGSTMidPrt").attr("style", "display: none;");
        document.getElementById("SpanSuppGSTMidPrt").innerHTML = "";

        $("#GSTLastPrt").css('border-color', "#ced4da");
        $("#SpanSuppGST").attr("style", "display: none;");
        document.getElementById("SpanSuppGST").innerHTML = "";

        //$("#GSTMidPrt").val(null);
        //$("#GSTLastPrt").val(null);
    }
    else {
        $("#GSTMidPrt").attr("disabled", false);
        $("#GSTLastPrt").attr("disabled", false);
        $("#GSTNumrequired").css("display", "inherit");
    }
}
function CheckValidationProsDetails() {
    debugger;
    var validationFlag = "N";
    var ProspectName = $("#ProspectName").val();
    var customer_address = $("#customer_address").val();
    var Country = $("#CmnCountry").val();
    var State = $("#CmnState").val();
    var District = $("#CmnDistrict").val();
    var City = $("#CmnCity").val();
    //var city_id = $("#ddlCity").val();
    var Pin = $("#CmnPin").val();
    var prosCurr = $("#Proscurr").val();
    var Branch = $("#Fr_branch").val();
    var GST = $("#gst_num").val();
    var GSTNoMidPrt = $("#GSTMidPrt").val().trim();
    var GSTNoLastPrt = $("#GSTLastPrt").val().trim();
    var GstApplicable = $('#hdnGstApplicable').val();
    var prostype;
    if ($("#Overseas").is(":checked")) {
        prostype = "I";
    }
    if ($("#Domestic").is(":checked")) {
        prostype = "D";
    }
    debugger;
  
    if (prosCurr == "0" || prosCurr=="") {
        document.getElementById("vmProscurr").innerHTML = $("#valueReq").text();
        $("#Proscurr").css("border-color", "red");
        validationFlag = "Y";
    }
    if (ProspectName == "") {
        document.getElementById("vmProspectName").innerHTML = $("#valueReq").text();
        $("#ProspectName").css("border-color", "red");
        validationFlag = "Y";
    }   
    if (Branch == "" || Branch == "0") {
        $("#vmbranch").text($("#valueReq").text());
        $("#vmbranch").css("display", "block");
        $("#Fr_branch").css("border-color", "red");
        validationFlag = 'Y';
    }   
    if (customer_address == "") {
        document.getElementById("vmcustomer_address").innerHTML = $("#valueReq").text();
        $("#customer_address").css("border-color", "red");
        validationFlag = "Y";
    }
        if (Country == "0") {
            document.getElementById("vmCountryname").innerHTML = $("#valueReq").text();
            $("[aria-labelledby='select2-CmnCountry-container']").css("border-color", "red");

            validationFlag = "Y";
        }
        if (State == "0") {
            document.getElementById("vmStatename").innerHTML = $("#valueReq").text();
            $("#CmnState").css("border-color", "red");
            $("[aria-labelledby='select2-CmnState-container']").css("border-color", "red");
            validationFlag = "Y";
        }
        if (District == "0") {
            document.getElementById("vmDistname").innerHTML = $("#valueReq").text();
            $("#CmnDistrict").css("border-color", "red");
            $("[aria-labelledby='select2-CmnDistrict-container']").css("border-color", "red");
            validationFlag = "Y";
        }
        if (City == "0") {
            document.getElementById("vmCityname").innerHTML = $("#valueReq").text();
            $("#CmnCity").css("border-color", "red");
            $("[aria-labelledby='select2-CmnCity-container']").css("border-color", "red");
            validationFlag = "Y";
        }
    if (prostype == "D") {
        if (Pin == "") {
            document.getElementById("vmPin").innerHTML = $("#valueReq").text();
            $("#CmnPin").css("border-color", "red");
            validationFlag = "Y";
        }
    }
    
    if (prostype != "I")
    {
        var FlagGstValidation = "N";
        var Gst_Cat = $("#Gst_Cat").val();
        var Gst_Applicable = $('#hdnGstApplicable').val();
        if (Gst_Cat == "RR" && Gst_Applicable == "Y") {
            FlagGstValidation == "Y";
        }
        else {
            FlagGstValidation == "N";
        }
        if (GstApplicable != "N" && FlagGstValidation != "N") {
            if (GSTNoMidPrt == "" && GSTNoLastPrt != "") {
                $("#GSTMidPrt").css('border-color', 'red');
                $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
                document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#valueReq").text();
                validationFlag = "Y";
            }
            else if (GSTNoMidPrt != "" && GSTNoMidPrt.length != "10") {
                $("#GSTMidPrt").css('border-color', 'red');
                $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
                document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#InvalidGSTNumber").text();
                validationFlag = "Y";
            }
            else if (GSTNoMidPrt != "" && GSTNoLastPrt == "") {
                $("#GSTLastPrt").css('border-color', 'red');
                $("#SpanSuppGST").attr("style", "display: block;");
                document.getElementById("SpanSuppGST").innerHTML = $("#valueReq").text();
                validationFlag = "Y";
            }
            else if (GSTNoLastPrt != "" && GSTNoLastPrt.length != "3") {
                $("#GSTLastPrt").css('border-color', 'red');
                $("#SpanSuppGST").attr("style", "display: block;");
                document.getElementById("SpanSuppGST").innerHTML = $("#InvalidGSTNumber").text();
                validationFlag = "Y";
            }
            if (GSTNoMidPrt != "" || GSTNoLastPrt) {
                var SuppGSTNum = GST + GSTNoMidPrt + GSTNoLastPrt
                $("#GSTNumber").val(SuppGSTNum);
            }
            else {
                if (GSTNoMidPrt == "" || GSTNoLastPrt == "") {
                    var SuppGSTNum = GSTNoMidPrt + GSTNoLastPrt
                    $("#GSTNumber").val(SuppGSTNum);
                }
            }
        }
        else {
            if (GSTNoMidPrt == "" && GSTNoLastPrt != "") {
                $("#GSTMidPrt").css('border-color', 'red');
                $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
                document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#valueReq").text();
                validationFlag = "Y";
            }
            else if (GSTNoMidPrt != "" && GSTNoMidPrt.length != "10") {
                $("#GSTMidPrt").css('border-color', 'red');
                $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
                document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#InvalidGSTNumber").text();
                validationFlag = "Y";
            }
            else if (GSTNoMidPrt != "" && GSTNoLastPrt == "") {
                $("#GSTLastPrt").css('border-color', 'red');
                $("#SpanSuppGST").attr("style", "display: block;");
                document.getElementById("SpanSuppGST").innerHTML = $("#valueReq").text();
                validationFlag = "Y";
            }
            else if (GSTNoLastPrt != "" && GSTNoLastPrt.length != "3") {
                $("#GSTLastPrt").css('border-color', 'red');
                $("#SpanSuppGST").attr("style", "display: block;");
                document.getElementById("SpanSuppGST").innerHTML = $("#InvalidGSTNumber").text();
                validationFlag = "Y";
            }
            if (GSTNoMidPrt != "" || GSTNoLastPrt) {
                var SuppGSTNum = GST + GSTNoMidPrt + GSTNoLastPrt
                $("#GSTNumber").val(SuppGSTNum);
            }
            else {
                if (GSTNoMidPrt == "" || GSTNoLastPrt == "") {
                    var SuppGSTNum = GSTNoMidPrt + GSTNoLastPrt
                    $("#GSTNumber").val(SuppGSTNum);
                }
            }
        }
    }
    else {
        if (GSTNoMidPrt == "" && GSTNoLastPrt != "") {
            $("#GSTMidPrt").css('border-color', 'red');
            $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
            document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#valueReq").text();
            validationFlag = "Y";
        }
        else if (GSTNoMidPrt != "" && GSTNoMidPrt.length != "10") {
            $("#GSTMidPrt").css('border-color', 'red');
            $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
            document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#InvalidGSTNumber").text();
            validationFlag = "Y";
        }
        else if (GSTNoMidPrt != "" && GSTNoLastPrt == "") {
            $("#GSTLastPrt").css('border-color', 'red');
            $("#SpanSuppGST").attr("style", "display: block;");
            document.getElementById("SpanSuppGST").innerHTML = $("#valueReq").text();
            validationFlag = "Y";
        }
        else if (GSTNoLastPrt != "" && GSTNoLastPrt.length != "3") {
            $("#GSTLastPrt").css('border-color', 'red');
            $("#SpanSuppGST").attr("style", "display: block;");
            document.getElementById("SpanSuppGST").innerHTML = $("#InvalidGSTNumber").text();
            validationFlag = "Y";
        }
        if (GSTNoMidPrt != "" || GSTNoLastPrt) {
            var SuppGSTNum = GST + GSTNoMidPrt + GSTNoLastPrt
            $("#GSTNumber").val(SuppGSTNum);
        }
        else {
            if (GSTNoMidPrt == "" || GSTNoLastPrt == "") {
                var SuppGSTNum = GSTNoMidPrt + GSTNoLastPrt
                $("#GSTNumber").val(SuppGSTNum);
            }
        }
    }
    
    debugger;
    if (ValidateGST() == false) {
        return false;
    }
    if (validationFlag == "Y") {
        return false;
    }
    if (ValidateEmail() == false) {
        return false;
    }
    else {
        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);
        /*----- Attatchment End--------*/   
        return true;
    }
}
function ValidateGST() {
    debugger;
    var GST = $("#gst_num").val();
    var GSTNoMidPrt = $("#GSTMidPrt").val().trim();
    var GSTNoLastPrt = $("#GSTLastPrt").val().trim();
    var gst_cat = $("#Gst_Cat").val();
    var GstApplicable = $('#hdnGstApplicable').val();
    var prostype;
    if ($("#Overseas").is(":checked")) {
        prostype = "I";
    }
    if ($("#Domestic").is(":checked")) {
        prostype = "D";
    }
    var valFalg = true;
    debugger;
    if (prostype != "I") {
        if (GstApplicable != "N") {
            var FlagGstValidation = "N";
            var Gst_Cat = $("#Gst_Cat").val();
            var Gst_Applicable = $('#hdnGstApplicable').val();
            if (Gst_Cat == "RR" && Gst_Applicable == "Y") {
                FlagGstValidation == "Y";
            }
            else {
                FlagGstValidation == "N";
            }
            if (gst_cat != "UR" && FlagGstValidation != "N") {

                if (GSTNoMidPrt == "") {
                    $("#GSTMidPrt").css('border-color', 'red');
                    $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
                    //document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#valueReq").text();
                    document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#valueReq").text();
                    valFalg = false;
                }
                else if (GSTNoMidPrt != "" && GSTNoMidPrt.length != "10") {
                    $("#GSTMidPrt").css('border-color', 'red');
                    $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
                    document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#InvalidGSTNumber").text();
                    valFalg = false;
                }
                else if (GSTNoLastPrt == "") {
                    $("#GSTLastPrt").css('border-color', 'red');
                    $("#SpanSuppGST").attr("style", "display: block;");
                    document.getElementById("SpanSuppGST").innerHTML = $("#valueReq").text();
                    valFalg = false;
                }
                else if (GSTNoLastPrt != "" && GSTNoLastPrt.length != "3") {
                    $("#GSTLastPrt").css('border-color', 'red');
                    $("#SpanSuppGST").attr("style", "display: block;");
                    document.getElementById("SpanSuppGST").innerHTML = $("#InvalidGSTNumber").text();
                    valFalg = false;
                }
            }
            if (gst_cat == "UR") {
                return true;
            }
            if (valFalg == true) {
                return true;
            }
        }
        if (valFalg == true) {
            return true;
        }
    }
    if (valFalg == true) {
        return true;
    }
    else {
        return false;
    }
}
//function getCityList() {  /*Commented By Hina on 03-01-2024 to bind onchange of country instead of it*/
//    debugger;
//    Cmn_getCityList("ddlCity");
//}

function numValueOnly(el, evt) {
    debugger;
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode == 43) {
        return true;
    }
    else {
        if (charCode > 31 && (charCode < 48 || charCode > 58)) {

            return false;
        }
    }
    return true;
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
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}
//function onchangeddlCity() {/*Commented By Hina on 03-01-2024 to bind onchange of country instead of it*/
//    var city_id = $("#ddlCity").val();
//    if (city_id != "0") {
//        document.getElementById("vmddlCity").innerHTML = "";
//        $("[aria-labelledby='select2-ddlCity-container']").css("border-color", "#ced4da");
    
//    }
 
//    GetsuppDSCntr();
//}
function onchangeProsName() {
    var ProspectName = $("#ProspectName").val();
    if (ProspectName != "") {
        document.getElementById("vmProspectName").innerHTML = "";
        $("#ProspectName").css("border-color", "#ced4da");
    }
}
function onchangeAddresss() {
    var customer_address = $("#customer_address").val();
    if (customer_address != "") {
        document.getElementById("vmcustomer_address").innerHTML = "";
        $("#customer_address").css("border-color", "#ced4da");
      
    }
}
function onchangePin() {
    //Commented by Suraj on 18-07-2024 
    //var curr;
    //if ($("#Overseas").is(":checked")) {
    //    curr = "E";
    //    $('#Proscurr').empty();
    //    $("#CmnCountry").select2();
    //}
    //if ($("#Domestic").is(":checked")) {
    //    curr = "D";
    //    $('#Proscurr').empty();
    //}
   
        var Pin = $("#CmnPin").val();
        if (Pin != "") {
            document.getElementById("vmPin").innerHTML = "";
            $("#CmnPin").css("border-color", "#ced4da");

        }
   
}
function onchangeCurrency() {
    var prosCurr = $("#Proscurr").val();
    if (prosCurr != "0") {
        document.getElementById("vmProscurr").innerHTML = "";
        $("#Proscurr").css("border-color", "#ced4da");
      
    }
}
function currvalidationstar() {
    debugger;
  
    if ($("#Overseas").is(":checked")) {
       
        $("#CurrMandatory").show();
    }
    if ($("#Domestic").is(":checked")) {
      
        $("#CurrMandatory").hide();
    }
    if (gst_cat == "RR") {
        $("#GSTNumrequired").show();
    }
    else {
        $("#GSTNumrequired").hide();
    }
}
function GetProspectcurr() {
    debugger;
    var curr;
    if ($("#Overseas").is(":checked")) {
        curr = "E";
        $('#Proscurr').empty();
        $("#CmnCountry").select2();
    }
    if ($("#Domestic").is(":checked")) {
        curr = "D";
        $('#Proscurr').empty();
    }
    if (curr != "D") {
        //$("#Gst_Cat").prop('disabled', true);
        //$("#RequiredGSTCategory").attr("style", "display: none;");
        document.getElementById("vmPin").innerHTML = "";
        $("#CmnPin").css("border-color", "#ced4da");
        $("#GSTNumrequired").attr("style", "display: none;");
        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#SpanSuppGST").attr("style", "display: none;");
        document.getElementById("SpanSuppGSTMidPrt").innerHTML = "";

        $("#DefaultCurrency").css("display", "none");
        $("#PinStar_required").text("");
        $("#DefaultCurrency1").removeClass("col-md-6");
    }
    else {
        //$("#Gst_Cat").prop('disabled', false);
        $("#GSTNumrequired").css("display", "inherit");
        //$("#RequiredGSTCategory").css("display", "inherit");
        $("#DefaultCurrency").css("display", "block");
        $("#DefaultCurrency1").addClass("col-md-6");
        $("#PinStar_required").text("*");
    }
    //$("#suppcurr").val("0").change();

    $.ajax({
        type: "POST",
        url: "/BusinessLayer/ProspectSetup/GetCurronProspType",
        data: { prosType: curr },
        success: function (data) {
        debugger;
            var arr = [];
            arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
            if (arr.length > 0) {
                //$('#Proscurr').val(arr[0].curr_id);
                for (var i = 0; i < arr.length; i++) {
                    $('#Proscurr').append(`<option value="${arr[i].curr_id}">${arr[i].curr_name}</option>`);
                }
            }
            //$('#CurrencyDiv').html(data);
        }
    });
    GetCountryOnChngProspectType();
}
function onchangeGST() {

    var GST = $("#gst_num").val();
    var GSTNoMidPrt = $("#GSTMidPrt").val().trim();
    var GSTNoLastPrt = $("#GSTLastPrt").val().trim();

    if (GSTNoMidPrt != "" || GSTNoMidPrt == "") {
        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#SpanSuppGST").attr("style", "display: none;");
        document.getElementById("SpanSuppGSTMidPrt").innerHTML = "";
    }
    if (GSTNoLastPrt != "" || GSTNoLastPrt == "") {
        document.getElementById("SpanSuppGST").innerHTML = "";
        $("#GSTLastPrt").css('border-color', "#ced4da");
        $("#SpanSuppGST").attr("style", "display: none;");
    }
}
function GetsuppDSCntr() {
    debugger;
    var ddlCity = $("#ddlCity").val();
    $.ajax({
        type: "POST",
        url: "/CustomerDetails/GetsuppDSCntr",/*Controller=SupplierSetup and Fuction=Getsuppport*/
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async: true,
        data: { SuppCity: ddlCity, },/*Registration pass value like model*/
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
                //var s = '<option value="0">Choose</option>';
                if (arr.Table.length > 0) {
                    $("#District").val(arr.Table[0].district_name);
                    $("#hdnDistrictID").val(arr.Table[0].district_id);
                }
                if (arr.Table1.length > 0) {
                    $("#State").val(arr.Table1[0].state_name);
                    $("#hdnStateID").val(arr.Table1[0].state_id);
                }
                if (arr.Table2.length > 0) {
                    $("#Country").val(arr.Table2[0].country_name);
                    $("#hdnCountryID").val(arr.Table2[0].country_id);
                }
                debugger;
                if (arr.Table3.length > 0) {
                    $("#gst_num").val(arr.Table3[0].state_code);
                    $("#hdnStateID").val(arr.Table3[0].state_id);
                }
            }
        },
        error: function (Data) {
        }
    });
};
function OnChangeProsBranch() {
    debugger;
    var branch = $("#Fr_branch").val();
    $("#vmbranch").css("display", "none");
    $("#Fr_branch").css("border-color", "#ced4da");
    $("#vmbranch").text("");  
}

function FilterListProspect() {
    var branchID = $("#Fr_branch").val();
    debugger
    $.ajax({
        url: "/BusinessLayer/ProspectSetup/ProspectListFilter",
        data: { brId: branchID },
        success: function (data) {
            $("#Wh_tbody").html(data);
        }
    })
}

function ValidateEmail() {
    debugger;
    /*var validFlag = "N";*/
    var Email = $("#Email").val();
   
    var mailformat = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    //mailformat.test(Email);
    if (Email != "") {
        if (mailformat.test(Email)) {
            document.getElementById("vmEmail").innerHTML = "";
            $("#Email").css("border-color", "#ced4da");
            return true;
        }
        else {
            document.getElementById("vmEmail").innerHTML = $("#InvalidEmail").text();
                $("#Email").css("border-color", "red");
                return false;
         }
    }
    else {
        document.getElementById("vmEmail").innerHTML = "";
        $("#Email").css("border-color", "#ced4da");
    }
    
}

/*Code by Hina on 04-01-2024 to Bind all data in dropdown and also OnChange COUNTRY,state,district,city */
function GetCountryOnChngProspectType() {
    debugger;
    var ProsType;
    if ($("#Overseas").is(":checked")) {
        ProsType = "E";
        $("#HdnProspectType").val(ProsType);
        $('#CmnCountry').empty();
        ClearAllData_OnProspectTypeChng();
    }
    if ($("#Domestic").is(":checked")) {
        ProsType = "D";
        $("#HdnProspectType").val(ProsType);
        $('#CmnCountry').empty();
        ClearAllData_OnProspectTypeChng();
    }

    $.ajax({
        type: "POST",
        url: "/BusinessLayer/ProspectSetup/GetCountryonChngPros",
        data: { ProspectType: ProsType },
        success: function (data) {
            debugger;
            var arr = [];
            arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
            if (arr.length > 0) {
                for (var i = 0; i < arr.length; i++) {
                    $('#CmnCountry').append(`<option value="${arr[i].country_id}">${arr[i].country_name}</option>`);
                }
                $("#CmnCountry").select2();/*For making Serachable Dropdown */
                GetStateByCountry();
                var hdnCountryId = $("#CmnCountry").val()
                //$("#hdnCountry_Id").val(hdnCountryId)
                //var hdnCountryName = $("#CmnCountry option:selected").text()
                //$("#hdnCountry_Name").val(hdnCountryName)
            }
        },
    });
    /*GetProspectcurr();*/
}
function ClearAllData_OnProspectTypeChng() {
    debugger;
    $('#ProspectName').val("");
    $('#ContactNumber').val("");
    $('#Email').val("");
    $('#ContactPerson').val("");
    $('#CmnPANNumber').val("");
    $('#customer_address').val("");
    $('#CmnState').empty();
    $('#CmnState').append('<option value="0">---Select---</option>')
    $('#CmnDistrict').empty();
    $('#CmnDistrict').append('<option value="0">---Select---</option>')
    $('#CmnCity').empty();
    $('#CmnCity').append('<option value="0">---Select---</option>')
    $('#CmnPin').val("");
    
    $('#gst_num').val("");
    $('#GSTMidPrt').val("");
    $('#GSTLastPrt').val("");
    $('#remarks').val("");
    //var TransType = $("#HdnProspectType").val();
    /*if (TransType == "D") {*/
    GetStateByCountry();
    /*  }*/
}
function OnChangeCountry() {
    debugger;
    document.getElementById("vmCountryname").innerHTML = "";
    $("[aria-labelledby='select2-CmnCountry-container']").css("border-color", "#ced4da");
    GetStateByCountry();
}
function GetStateByCountry() {
    debugger;
    if ($("#Overseas").is(":checked")) {
        ProsType = "E";
        $("#HdnProspectType").val(ProsType);
    }
    if ($("#Domestic").is(":checked")) {
        ProsType = "D";
        $("#HdnProspectType").val(ProsType);
    }
    var ddlCountryID = $("#CmnCountry").val();

    $.ajax({
        type: "POST",
        url: "/ProspectSetup/GetstateOnCountry",/*Controller=SupplierSetup and Fuction=Getsuppport*/
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async: true,
        data: { ddlCountryID: ddlCountryID, },/*Registration pass value like model*/
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
                if (arr.length > 0) {
                    $('#CmnState').empty();
                    $('#CmnState').append(`<option value="0">---Select---</option>`);
                    for (var i = 0; i < arr.length; i++) {

                        $('#CmnState').append(`<option value="${arr[i].state_id}">${arr[i].state_name}</option>`);
                    }
                    $("#CmnState").select2();/*For making Serachable Dropdown */

                }


            }
        },
        error: function (Data) {
        }
    });
};
function OnChangeState() {
    debugger;
    GetDistrictByState();
    GstStateCodeOnChangeStatus();
    $('#vmStatename').text("");
    $("#vmStatename").css("display", "none");
    $("[aria-labelledby='select2-CmnState-container']").css('border-color', "#ced4da");
    $("#CmnState").css('border-color', "#ced4da");

}
function GetDistrictByState() {
    debugger;

    var ddlStateID = $("#CmnState").val();

    $.ajax({
        type: "POST",
        url: "/ProspectSetup/GetDistrictOnState",/*Controller=SupplierSetup and Fuction=Getsuppport*/
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async: true,
        data: { ddlStateID: ddlStateID, },/*Registration pass value like model*/
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
                if (arr.length > 0) {
                    $('#CmnDistrict').empty();
                    for (var i = 0; i < arr.length; i++) {
                        $('#CmnDistrict').append(`<option value="${arr[i].district_id}">${arr[i].district_name}</option>`);
                    }
                    $("#CmnDistrict").select2();/*For making Serachable Dropdown */
                }
            }
        },
        error: function (Data) {
        }
    });
    $('#CmnCity').empty();
    $('#CmnCity').append(`<option value="0">---Select---</option>`);
    
};
function OnChangeDistrict() {
    debugger;
    GetCityByDistrict();
    $('#vmDistname').text("");
    $("#vmDistname").css("display", "none");
    //$("#CmnDistrict").css("border-color", "red");
    $("[aria-labelledby='select2-CmnDistrict-container']").css('border-color', "#ced4da");
   
}
function GstStateCodeOnChangeStatus() {
    debugger
    
    var stateCode = $('#CmnState').val();
    $.ajax({
        type: "POST",
        url: "/ProspectSetup/GetStateCode",/*Controller=SupplierSetup and Fuction=Getsuppport*/
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async: true,
        data: { stateId: stateCode, },/*Registration pass value like model*/
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            debugger;
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
                if (arr.Table.length > 0) {
                    $('#gst_num').val(arr.Table[0].state_code);
                }
            }
        },
        error: function (Data) {
        }
    });

}
function GetCityByDistrict() {
    debugger;

    var ddlDistrictID = $("#CmnDistrict").val();

    $.ajax({
        type: "POST",
        url: "/ProspectSetup/GetCityOnDistrict",/*Controller=SupplierSetup and Fuction=Getsuppport*/
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async: true,
        data: { ddlDistrictID: ddlDistrictID, },/*Registration pass value like model*/
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
                if (arr.length > 0) {
                    $('#CmnCity').empty();
                    for (var i = 0; i < arr.length; i++) {
                        $('#CmnCity').append(`<option value="${arr[i].city_id}">${arr[i].city_name}</option>`);
                    }
                    $("#CmnCity").select2();/*For making Serachable Dropdown */

                }


            }
        },
        error: function (Data) {
        }
    });
};
function OnChangeCity() {
    debugger;

    $('#vmCityname').text("");
    $("#vmCityname").css("display", "None");
    

    $("[aria-labelledby='select2-CmnCity-container']").css('border-color', "#ced4da");


}