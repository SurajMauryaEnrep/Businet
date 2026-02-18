////$(document).ready(function () {
////    debugger;
////    if ($("#Domestic").is(":checked")) {
////        TransportType = "D";
////        $("#HdnTransportType").val(TransportType);
////        var TransType = $("#HdnTransportType").val();
////        if (TransType == "D") {
////            /*GetStateByCountry();*/
////        }
////    }
////});
$(document).ready(function () {
    debugger;
    var gst_cat = $("#Gst_Cat").val();
    var CreateID = $("#TransCreatedBy").text();
    if (CreateID != "" && CreateID != null) {
        $("#OverSeas").attr("disabled", true)
        $("#Domestic").attr("disabled", true)
    }
    if (gst_cat == "UG") {
        $("#GSTMidPrt").attr("disabled", true)
        $("#GSTLastPrt").attr("disabled", true)
    }
    else {
        $("#GSTMidPrt").attr("disabled", true)
    }
    ValidationForPanNoSetAttr();
});
function GetCountryOnChngTransType() {
    debugger;
    ValidationForPanNoSetAttr();
    var TransportType;
    if ($("#OverSeas").is(":checked")) {
        $("#GSTNumrequired").hide();
        $("#Div_GstCatogry").css("display", "none");
        TransportType = "E";
        $("#HdnTransportType").val(TransportType);
        $('#TransportCountry').empty();
        $('#vmPin').text("");
        $("#vmPin").css("display", "none");
       
        $("#Div_pinReq").hide();
        //document.getElementById("vmPANNum").innerHTML = $("#valueReq").text();
        $("#CmnPin").css("border-color", "#ced4da");
        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#GSTMidPrt").attr("disabled",false);
        $("#SpantransportGST").attr("style", "display: none;");
        document.getElementById("SpantransportGSTMidPrt").innerHTML = "";
        ClearAllData_OnTransTypeChng();
    }
    if ($("#Domestic").is(":checked")) {
        TransportType = "D";
        $("#GSTMidPrt").attr("disabled", true);
        $("#Div_pinReq").show();
        $("#HdnTransportType").val(TransportType);
        $('#TransportCountry').empty();
        ClearAllData_OnTransTypeChng();
        $("#Div_GstCatogry").css("display", "block");
        $("#GSTNumrequired").show();
    }
    
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/TransporterSetup/GetCountryonTransMode",
        data: { TransportMode: TransportType },
        success: function (data) {
            debugger;
            var arr = [];
            arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
            if (arr.length > 0) {
                $('#TransportCountry').empty();
                $('#TransportCountry').append(`<option value="0">---Select---</option>`);

                for (var i = 0; i < arr.length; i++) {
                    $('#TransportCountry').append(`<option value="${arr[i].country_id}">${arr[i].country_name}</option>`);
                }
                $("#TransportCountry").select2();/*For making Serachable Dropdown */
                GetStateByCountry();
               var hdnCountryId= $("#TransportCountry").val()
                $("#hdnCountry_Id").val(hdnCountryId)
                var hdnCountryName = $("#TransportCountry option:selected").text()
                $("#hdnCountry_Name").val(hdnCountryName)
            }
           
        },
    });
}
function OnChangeCountry() {
    debugger;
    document.getElementById("vmTransCountry").innerHTML = "";
    $("[aria-labelledby='select2-TransportCountry-container']").css("border-color", "#ced4da");
    GetStateByCountry();
    ValidationForPanNoSetAttr();
}
function GetStateByCountry() {
    debugger;
    if ($("#OverSeas").is(":checked")) {
        TransportType = "E";
        $("#HdnTransportType").val(TransportType);
          }
    if ($("#Domestic").is(":checked")) {
        TransportType = "D";
        $("#HdnTransportType").val(TransportType);
           }
    var ddlCountryID = $("#TransportCountry").val();
    document.getElementById("vmStatename").innerHTML = "";

    $.ajax({
        type: "POST",
        url: "/TransporterSetup/GetstateOnCountry",/*Controller=SupplierSetup and Fuction=Getsuppport*/
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
                    $('#TransportState').empty();
                    $('#TransportState').append(`<option value="0">---Select---</option>`);
                    for (var i = 0; i < arr.length; i++) {

                        $('#TransportState').append(`<option value="${arr[i].state_id}">${arr[i].state_name}</option>`);
                    }
                    $("#TransportState").select2();/*For making Serachable Dropdown */
                    
                }
                

            }
        },
        error: function (Data) {
        }
    });
};

function GetDistrictByState() {
    debugger;
   
    var ddlStateID = $("#TransportState").val();
    document.getElementById("vmDistname").innerHTML = "";

    $.ajax({
        type: "POST",
        url: "/TransporterSetup/GetDistrictOnState",/*Controller=SupplierSetup and Fuction=Getsuppport*/
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
};
function OnChangeState() {
    debugger;
    GetDistrictByState();
    GstStateCodeOnChangeStatus();
    $('#vmStatename').text("");
    $("#vmStatename").css("display", "none");
   // $("[aria-labelledby='select2-TransportState-container']").css('border-color', "#ced4da");
    $("#TransportState").css('border-color', "#ced4da");
    
}
function GetCityByDistrict() {
    debugger;

    var ddlDistrictID = $("#CmnDistrict").val();
    document.getElementById("vmCityname").innerHTML = "";

    $.ajax({
        type: "POST",
        url: "/TransporterSetup/GetCityOnDistrict",/*Controller=SupplierSetup and Fuction=Getsuppport*/
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
function OnChangeDistrict() {
    debugger;
    GetCityByDistrict();
    $('#vmDistname').text("");
    $("#vmDistname").css("display", "none");
    //$("#CmnDistrict").css("border-color", "red");
    $("[aria-labelledby='select2-CmnDistrict-container']").css('border-color', "#ced4da");


}
function OnChangeCity() {
    debugger;
    
    $('#vmCityname').text("");
    $("#vmCityname").css("display", "None");
    //$("#CmnCity").css("border-color", "red");
    
    $("[aria-labelledby='select2-CmnCity-container']").css('border-color', "#ced4da");


}

function ClearAllData_OnTransTypeChng() {
    debugger;
    $('#TransporterName').val("");
    $('#Trans_Address').val("");
    $('#TransportState').empty();
    $('#TransportState').append('<option value="0">---Select---</option>')
    $('#CmnDistrict').empty();
    $('#CmnDistrict').append('<option value="0">---Select---</option>')
    $('#CmnCity').empty();
    $('#CmnCity').append('<option value="0">---Select---</option>')
    $('#CmnPin').val("");
    $('#CmnPANNumber').val("");
    $('#CmnGSTNumber').val("");
    $('#GSTMidPrt').val("");
    $('#GSTLastPrt').val(""); 
    $('#Trans_Remarks').val("");
    var TransType = $("#HdnTransportType").val();
    /*if (TransType == "D") {*/
        GetStateByCountry();
  /*  }*/
}

function SaveBtnClick() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Adddd this NItesh 01-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    var pinlength = $("#CmnPin").val();
  
        if (btn == "AllreadyClick") {
            $("#btn_save").css("filter", "grayscale(100%)");
            $("#btn_save").prop("disabled", true); /*End*/
        }
   
   
    if (validatTransportSetupInsertform() == false) {
        return false;
    }

    else {
        $("#GSTMidPrt").attr("disabled", false);
        FinalSubItemDetail = InsertSubItemDetails();
      
        var hdnCountryId = $("#TransportCountry").val()
        $("#hdnCountry_Id").val(hdnCountryId)
        var hdnCountryName = $("#TransportCountry option:selected").text()
        $("#hdnCountry_Name").val(hdnCountryName)
        //$("#btn_save").css("filter", "grayscale(100%)");
        if (pinlength.length >= 4) {
            $("#hdnSavebtn").val("AllreadyClick");
        }

        return true;
    }
    
   
   
}
function OnChangeTransporterName() {
    debugger;

    var Transname = $("#TransporterName").val().trim();
        if (Transname !="") {
            document.getElementById("vmTransporterName").innerHTML = '';
            $("#TransporterName").css("border-color", "#ced4da");
            ValidationFlag = false;
        }
    else {
            document.getElementById("vmTransporterName").innerHTML = $("#valueReq").text();
            $("#TransporterName").css("border-color", "red");
    }
}
function OnChangeTransAddress() {
    debugger;
     var TransAdd = $("#Trans_Address").val();
    if (TransAdd != "") {
        document.getElementById("vmTransAddress").innerHTML = '';
        $("#Trans_Address").css("border-color", "#ced4da");
        
    }
    else {
        document.getElementById("vmTransAddress").innerHTML = $("#valueReq").text();
       $("#Trans_Address").css("border-color", "red");
    }
}
function OnChangeTransMode() {
    debugger;
    var TransMode = $("#Trans_Mode").val();
    if (TransMode != "") {
        document.getElementById("vmTransportMode").innerHTML = '';
        $("#Trans_Mode").css("border-color", "#ced4da");

    }
    else {
        document.getElementById("vmTransportMode").innerHTML = $("#valueReq").text();
        $("#Trans_Mode").css("border-color", "red");
    }
}
function ValidationForPanNoSetAttr() {
   
    if ($("#Domestic").is(":checked")) {
        var gst_cat = $("#Gst_Cat").val();
        if (gst_cat != "UG")
        {
            var CompCountry = $("#TransportCountry option:selected").text().trim().toLowerCase();

            if (CompCountry == "india") {
                $("#CmnPANNumber").attr("maxlength", "10");
                $("#CmnPANNumber").attr("minlength", "10");
                $("#CmnPANNumber").attr("placeholder", "ABCDE1234F");
                $("#PanNorequired").css("display", "");
            }
            else {
                var placeholder = $("#span_PANNumber").text();
                $("#CmnPANNumber").attr("maxlength", "50");
                $("#CmnPANNumber").attr("minlength", "");
                $("#CmnPANNumber").attr("placeholder", `${$("#span_PANNumber").text()}`);
                $("#PanNorequired").css("display", "none");
                $("#vmPANNum").text("");
                $("#vmPANNum").css("display", "none");
                $("#CmnPANNumber").attr("style", "border-color: #ced4da;");
            }
        }
        else {
            var placeholder = $("#span_PANNumber").text();
            $("#CmnPANNumber").attr("maxlength", "50");
            $("#CmnPANNumber").attr("minlength", "");
            $("#CmnPANNumber").attr("placeholder", `${$("#span_PANNumber").text()}`);
            $("#PanNorequired").css("display", "none");
            $("#vmPANNum").text("");
            $("#vmPANNum").css("display", "none");
            $("#CmnPANNumber").attr("style", "border-color: #ced4da;");
        }

    }
    else {
        var placeholder = $("#span_PANNumber").text();
        $("#CmnPANNumber").attr("maxlength", "50");
        $("#CmnPANNumber").attr("minlength", "");
        $("#CmnPANNumber").attr("placeholder", `${$("#span_PANNumber").text()}`);
        $("#PanNorequired").css("display", "none");
    }
   

}
function OnChangePanNum() {
    debugger;
    var valid = ValidationPanNumber();
    if (valid == true) {
        $("#vmPANNum").text("");
        $("#vmPANNum").css("display", "none");
        $("#CmnPANNumber").attr("style", "border-color: #ced4da;");
        var GstApplicable = $("#Hdn_GstApplicable").text();
        
        if ($("#Domestic").is(":checked") && GstApplicable == "Y") {
            var gst_cat = $("#Gst_Cat").val();
            if (gst_cat != "UG") {
                const panInput = $("#CmnPANNumber").val().toUpperCase(); // ensure uppercase
                $("#GSTMidPrt").val(panInput);
                $("#GSTMidPrt").attr("disabled", true);
            }
            else {
                $("#GSTMidPrt").val("");
                $("#GSTMidPrt").attr("disabled", true);
            }
        }
        else {
            $("#GSTMidPrt").attr("disabled", false);
        }
        return true;
    }
    else {
        $("#GSTMidPrt").attr("disabled", true);
        return false;
    }

    //var Pannum = $("#CmnPANNumber").val().trim();
    //if (Pannum != "") {
    //    document.getElementById("vmPANNum").innerHTML = '';
    //    $("#CmnPANNumber").css("border-color", "#ced4da");
    //}
    //else {
    //    document.getElementById("vmPANNum").innerHTML = $("#valueReq").text();
    //    $("#CmnPANNumber").css("border-color", "red");
    //}
}
function ValidationPanNumber() {
    var ValidationFlag = true;
    if ($("#Domestic").is(":checked")) {
        var gst_cat = $("#Gst_Cat").val();
        if (gst_cat != "UG") {
            var CompCountry = $("#TransportCountry option:selected").text().trim().toLowerCase();

            if (CompCountry == "india") {
                const panInput = $("#CmnPANNumber").val().toUpperCase(); // ensure uppercase
                if (panInput == "" || panInput == null) {
                    $("#vmPANNum").text($("#valueReq").text());
                    $("#vmPANNum").css("display", "block");
                    $("#CmnPANNumber").css("border-color", "red");
                    ValidationFlag = false;
                }
                else {
                    if (isValidPAN(panInput)) {
                        $("#vmPANNum").text("");
                        $("#vmPANNum").css("display", "none");
                        $("#CmnPANNumber").attr("style", "border-color: #ced4da;");
                    }
                    else {
                        $("#vmPANNum").text($("#span_InvalidPANNumber").text() + ' (' + 'ABCDE1234F' + ')');
                        $("#vmPANNum").css("display", "block");
                        $("#CmnPANNumber").css("border-color", "red");
                        ValidationFlag = false;
                    }
                }

            }
            else {
                $("#vmPANNum").text("");
                $("#vmPANNum").css("display", "none");
                $("#CmnPANNumber").attr("style", "border-color: #ced4da;");
            }
        }
        else {
            var placeholder = $("#span_PANNumber").text();
            $("#CmnPANNumber").attr("maxlength", "50");
            $("#CmnPANNumber").attr("placeholder", `${$("#span_PANNumber").text()}`);
            $("#PanNorequired").css("display", "none");
            $("#vmPANNum").text("");
            $("#vmPANNum").css("display", "none");
            $("#CmnPANNumber").attr("style", "border-color: #ced4da;");
        }
      
       

    }
    else {
        var placeholder = $("#span_PANNumber").text();
        $("#CmnPANNumber").attr("maxlength", "50");
        $("#CmnPANNumber").attr("placeholder", `${$("#span_PANNumber").text()}`);
        $("#PanNorequired").css("display", "none");
        $("#vmPANNum").text("");
        $("#vmPANNum").css("display", "none");
        $("#CmnPANNumber").attr("style", "border-color: #ced4da;");
    }
    if (ValidationFlag == false) {
        return false;
    }
    else {
        return true;
    }
}
function isValidPAN(pan) {
    const panRegex = /^[A-Z]{5}[0-9]{4}[A-Z]{1}$/;
    return panRegex.test(pan);
}
function onchangeGST() {
    debugger;
    //var custtype;
    //if ($("#Export").is(":checked")) {
    //    custtype = "I";
    //}
    //if ($("#Domestic").is(":checked")) {
    //    custtype = "D";
    //}
    var CustomerGST = $("#gst_num").val();
    var GSTNoMidPrt = $("#GSTMidPrt").val().trim();
    var GSTNoLastPrt = $("#GSTLastPrt").val().trim();

    if (GSTNoMidPrt != "" || GSTNoMidPrt == "") {
        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#SpantransportGST").attr("style", "display: none;");
        document.getElementById("SpantransportGSTMidPrt").innerHTML = "";
    }
    if (GSTNoLastPrt != "" || GSTNoLastPrt == "") {
        document.getElementById("SpantransportGST").innerHTML = "";
        $("#GSTLastPrt").css('border-color', "#ced4da");
        $("#SpantransportGST").attr("style", "display: none;");
    }
    //if (custtype == "D") {
    //    if (GSTNoMidPrt == "") {
    //         document.getElementById("SpantransportGST").innerHTML = $("#valueReq").text();
    //        //$("#gst_num").css('border-color', 'red');
    //        $("#GSTMidPrt").css('border-color', 'red');
    //        $("#SpantransportGST").attr("style", "display: block;");
    //         $("#GSTNumrequired").css("display", "inherit");

    //     }
    //     else {
    //         document.getElementById("SpantransportGST").innerHTML = "";
    //        //$("#gst_num").css('border-color', '#ced4da');
    //        $("#GSTMidPrt").css('border-color', "#ced4da");
    //        $("#SpantransportGST").attr("style", "display: none;");
    //    }
    //    if (GSTNoLastPrt == "") {
    //        document.getElementById("SpantransportGST").innerHTML = $("#valueReq").text();
    //        $("#GSTLastPrt").css('border-color', 'red');
    //        $("#SpantransportGST").attr("style", "display: block;");
    //        $("#GSTNumrequired").css("display", "inherit");
    //    }
    //    else {
    //        document.getElementById("SpantransportGST").innerHTML = "";
    //        //$("#gst_num").css('border-color', '#ced4da');
    //        $("#GSTLastPrt").css('border-color', "#ced4da");
    //        $("#SpantransportGST").attr("style", "display: none;");
    //    }

    // }
    // else {

    //      document.getElementById("SpantransportGST").innerHTML = "";
    //    //$("#gst_num").css('border-color', '#ced4da');
    //    $("#GSTMidPrt").css('border-color', "#ced4da");
    //    $("#GSTLastPrt").css('border-color', "#ced4da");
    //     $("#SpantransportGST").attr("style", "display: none;");
    //     $("#GSTNumrequired").css("style", "display:none");
    //  }
}
function validatTransportSetupInsertform() {
    debugger;
    var Transtype;
    if ($("#Overseas").is(":checked")) {
        Transtype = "E";
    }
    if ($("#Domestic").is(":checked")) {
        Transtype = "D";
    }
   
    var Transname = $("#TransporterName").val();
    var reason = $("#inact_reason").val();
    var holdreason = $("#hold_reason").val();
    var custcityID = $("#cust_city").val();
    var CmnPin = $("#CmnPin").val();

  
    var hold = '';
   
    if ($("#Trans_OnHold").is(":checked")) {
        hold = "Y";
    }
    else {
        hold = "N";
    }

    var ValidationFlag = true;

    if (Transname == '') {
        document.getElementById("vmTransporterName").innerHTML = $("#valueReq").text();
        $("#TransporterName").css("border-color", "red");
        ValidationFlag = false;
    }
    
    if ($("#Trans_Mode").val() == '' || $("#Trans_Mode").val() == '0') {
        document.getElementById("vmTransportMode").innerHTML = $("#valueReq").text();
        $("#Trans_Mode").css("border-color", "red");
        ValidationFlag = false;
    }
    if ($("#Trans_Address").val() == '') {
        
        document.getElementById("vmTransAddress").innerHTML = $("#valueReq").text();
        //$('#TransAddress_Error').text($("#valueReq").text());
        
        $("#Trans_Address").css("border-color", "red");
        ValidationFlag = false;
    }
    var country = $("#TransportCountry").val();
    if (country == "0") {
        $('#SpanTransCountry').text($("#valueReq").text());
        $("#SpanTransCountry").css("display", "block");
        $("#TransportCountry").css("border-color", "red");
        //$("[aria-labelledby='select2-GLAccGrp-container']").css("border-color", "red");
        ValidationFlag = false;
    }
   
    if ($("#TransportState").val() == '0') {
        $('#vmStatename').text($("#valueReq").text());
        $("#vmStatename").css("display", "block");
        $("#TransportState").css("border-color", "red");
        ValidationFlag = false;
    }
    if ($("#CmnDistrict").val() == '0') {
        $('#vmDistname').text($("#valueReq").text());
        $("#vmDistname").css("display", "block");
        //$("#CmnDistrict").css("border-color", "red");
       // $("[aria-labelledby='select2-CmnDistrict-container']").css("border-color", "red");
        $("#CmnDistrict").css("border-color", "red");
        ValidationFlag = false;
    }
    if ($("#CmnCity").val() == '0') {
        $('#vmCityname').text($("#valueReq").text());
        $("#vmCityname").css("display", "block");
        //$("#CmnCity").css("border-color", "red");
      //  $("[aria-labelledby='select2-CmnCity-container']").css("border-color", "red");
        $("#CmnCity").css("border-color", "red");
        ValidationFlag = false;
    }
    if (Transtype == "D") {
        var gst_cat = $("#Gst_Cat").val();
        if (gst_cat != "UG") {
            var CompCountry = $("#TransportCountry option:selected").text().trim().toLowerCase();

            if (CompCountry == "india") {
                if ($("#CmnPANNumber").val() == '') {
                    $('#vmPANNum').text($("#valueReq").text());
                    $("#vmPANNum").css("display", "block");
                    //document.getElementById("vmPANNum").innerHTML = $("#valueReq").text();
                    $("#CmnPANNumber").css("border-color", "red");
                    ValidationFlag = false;
                }
                else {
                    var validPanNumber = ValidationPanNumber();
                    if (validPanNumber == false) {
                        ValidationFlag = false;
                    }
                    else {

                    }
                }
            }
           
        }
    }
    if (Transtype == "D") {
        if (CmnPin == '') {
            $('#vmPin').text($("#valueReq").text());
            $("#vmPin").css("display", "block");
            //document.getElementById("vmPANNum").innerHTML = $("#valueReq").text();
            $("#CmnPin").css("border-color", "red");
            ValidationFlag = false;
        }
    }
    var Gst_Cat = $("#Gst_Cat").val();
    var Hdn_GstApplicable = $("#Hdn_GstApplicable").text();
    if ((Gst_Cat != "UG" && Hdn_GstApplicable == "Y")) {
        if (ValidateValReqWithInvalidGST() == false) {
            // return false;
            ValidationFlag = false;

        }
    }
   
    //else {
    //    return true
    //}
    debugger;
    if (ValidationFlag == true) {

       /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);

        /*----- Attatchment End--------*/
        return true;
    }
    else {
        return false;
    }
}

function onchagepin_No() {
    var Transtype;
    if ($("#Overseas").is(":checked")) {
        Transtype = "E";
    }
    if ($("#Domestic").is(":checked")) {
        Transtype = "D";
    }
    var CmnPin = $("#CmnPin").val();
    if (Transtype == "D") {
        if (CmnPin == '' || CmnPin == null) {
            $('#vmPin').text($("#valueReq").text());
            $("#vmPin").css("display", "block");
            //document.getElementById("vmPANNum").innerHTML = $("#valueReq").text();
            $("#CmnPin").css("border-color", "red");
            ValidationFlag = false;
        }
        else {
            document.getElementById("vmPin").innerHTML = '';
            $("#CmnPin").css("border-color", "#ced4da");
        }
    }
    else {
        document.getElementById("vmPin").innerHTML = '';
        $("#CmnPin").css("border-color", "#ced4da");
    }
    
}

function ValidateValReqWithInvalidGST() {
    debugger;
    var GstApplicable = $('#hdnGstApplicable').val();
    var TransportType = "";
    if ($("#OverSeas").is(":checked")) {
        TransportType = "E";
        $("#HdnTransportType").val(TransportType);      
    }
    if ($("#Domestic").is(":checked")) {
        TransportType = "D";
        $("#HdnTransportType").val(TransportType);   
    }
    var transtyp = $("#HdnTransportType").val();
    var CustomerGST = $("#gst_num").val();
    var GSTNoMidPrt = $("#GSTMidPrt").val().trim();
    var GSTNoLastPrt = $("#GSTLastPrt").val().trim();
    var valFalg = true;
    if (TransportType != "E") {
        if (GSTNoMidPrt == "") {
            if (GSTNoMidPrt == "") {
                $("#GSTMidPrt").css('border-color', 'red');
                $("#SpantransportGSTMidPrt").attr("style", "display: block;");
                //document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#valueReq").text();
                document.getElementById("SpantransportGSTMidPrt").innerHTML = $("#valueReq").text();
                valFalg = false;
            }
            else if (GSTNoMidPrt != "" && GSTNoMidPrt.length != "10") {
                $("#GSTMidPrt").css('border-color', 'red');
                $("#SpantransportGSTMidPrt").attr("style", "display: block;");
                document.getElementById("SpantransportGSTMidPrt").innerHTML = $("#InvalidGSTNumber").text();
                valFalg = false;
            }
        }
        else {
            if (GSTNoMidPrt != "" || GSTNoLastPrt != "") {
                if (GSTNoMidPrt == "") {
                    $("#GSTMidPrt").css('border-color', 'red');
                    $("#SpantransportGSTMidPrt").attr("style", "display: block;");
                    document.getElementById("SpantransportGSTMidPrt").innerHTML = $("#valueReq").text();
                    valFalg = false;
                }
                else if (GSTNoMidPrt != "" && GSTNoMidPrt.length != "10") {
                    $("#GSTMidPrt").css('border-color', 'red');
                    $("#SpantransportGSTMidPrt").attr("style", "display: block;");
                    document.getElementById("SpantransportGSTMidPrt").innerHTML = $("#InvalidGSTNumber").text();
                    valFalg = false;
                }
                else if (GSTNoLastPrt == "") {
                    $("#GSTLastPrt").css('border-color', 'red');
                    $("#SpantransportGST").attr("style", "display: block;");
                    document.getElementById("SpantransportGST").innerHTML = $("#valueReq").text();
                    valFalg = false;
                }
                else if (GSTNoLastPrt != "" && GSTNoLastPrt.length != "3") {
                    $("#GSTLastPrt").css('border-color', 'red');
                    $("#SpantransportGST").attr("style", "display: block;");
                    document.getElementById("SpantransportGST").innerHTML = $("#InvalidGSTNumber").text();
                    valFalg = false;
                }
            }
            else {
                if (GSTNoMidPrt != "" && GSTNoLastPrt == "") {
                    $("#GSTLastPrt").css('border-color', 'red');
                    $("#SpantransportGST").attr("style", "display: block;");
                    document.getElementById("SpantransportGST").innerHTML = $("#InvalidGSTNumber").text();
                    valFalg = false;
                }
                else {

                }
                if (GSTNoMidPrt == "" && GSTNoLastPrt != "") {
                    $("#GSTMidPrt").css('border-color', 'red');
                    $("#SpantransportGSTMidPrt").attr("style", "display: block;");
                    document.getElementById("SpantransportGSTMidPrt").innerHTML = $("#InvalidGSTNumber").text();
                    valFalg = false;
                }
                else {

                }
            }
        }
    }
   
    //  //if (transtyp == "D") {  /**Commented By NItesh 19/01/2024 **/
    //    debugger;
    //if (CustomerGST == "") {

    //        document.getElementById("SpantransportGST").innerHTML = $("#valueReq").text();
    //        $("#GSTLastPrt").css('border-color', 'red');
    //        $("#SpantransportGST").attr("style", "display: block;");
    //        $("#GSTNumrequired").css("display", "inherit");
    //        valFalg = false;
    //    }
    //    else {
    //        if (CustomerGST != "" && CustomerGST.length == "2") {
    //            $("#GSTLastPrt").css('border-color', "#ced4da");
    //            $("#SpantransportGST").attr("style", "display: none;");
    //            document.getElementById("SpantransportGST").innerHTML = "";

    //            if (GSTNoMidPrt == "") {
    //                document.getElementById("SpantransportGST").innerHTML = $("#valueReq").text();
    //                $("#GSTMidPrt").css('border-color', 'red');
    //                $("#SpantransportGST").attr("style", "display: block;");
    //                $("#GSTNumrequired").css("display", "inherit");
    //                valFalg = false;

    //            }
    //            else {
    //                if (GSTNoMidPrt != "" && GSTNoMidPrt.length == "10") {
    //                    $("#GSTMidPrt").css('border-color', "#ced4da");
    //                    $("#SpantransportGSTMidPrt").attr("style", "display: none;");
    //                    document.getElementById("SpantransportGSTMidPrt").innerHTML = "";

    //                    if (GSTNoLastPrt == "") {
    //                        document.getElementById("SpantransportGST").innerHTML = $("#valueReq").text();
    //                        $("#GSTLastPrt").css('border-color', 'red');

    //                        $("#SpantransportGST").attr("style", "display: block;");
    //                        $("#GSTNumrequired").css("display", "inherit");
    //                        valFalg = false;
    //                    }
    //                    else {

    //                        if (GSTNoLastPrt != "" && GSTNoLastPrt.length == "3") {
    //                            $("#GSTLastPrt").css('border-color', "#ced4da");
    //                            $("#SpantransportGST").attr("style", "display: none;");
    //                            document.getElementById("SpantransportGST").innerHTML = "";
    //                        }
    //                        else {
    //                            $("#GSTLastPrt").css('border-color', 'red');
    //                            $("#SpantransportGST").attr("style", "display: block;");
    //                            document.getElementById("SpantransportGST").innerHTML = $("#InvalidGSTNumber").text();
    //                            valFalg = false;
    //                        }
    //                    }
    //                }
    //                else {

    //                    $("#GSTMidPrt").css('border-color', 'red');
    //                    $("#SpantransportGSTMidPrt").attr("style", "display: block;");
    //                    document.getElementById("SpantransportGSTMidPrt").innerHTML = $("#InvalidGSTNumber").text();
    //                    valFalg = false;
    //                }
    //            }

    //        }
    //        else {
    //            $("#GSTLastPrt").css('border-color', 'red');
    //            $("#SpantransportGST").attr("style", "display: block;");
    //            document.getElementById("SpantransportGST").innerHTML = $("#InvalidGSTNumber").text();
    //            valFalg = false;
    //        }
    //    }
  //  }
    //else {
    //    document.getElementById("SpantransportGST").innerHTML = "";
    //    $("#GSTMidPrt").css('border-color', "#ced4da");
    //    $("#GSTLastPrt").css('border-color', "#ced4da");
    //    $("#SpantransportGST").attr("style", "display: none;");
    //    $("#GSTNumrequired").css("style", "display:none");
    //}


    if (valFalg == false) {
        return false;
    }
    else {
        return true;
    }
}
function GstStateCodeOnChangeStatus() {
    debugger
    var stateCode = $('#TransportState').val();
    $.ajax({
        type: "POST",
        url: "/TransporterSetup/GetStateCode",/*Controller=SupplierSetup and Fuction=Getsuppport*/
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
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();
            return true;
        } else {
            return false;
        }
    });
    return false;
}
function OnChangeGSTCat() {
    debugger;
    var gst_cat = $("#Gst_Cat").val();
    if (gst_cat == "UG") {


        $("#GSTNumrequired").attr("style", "display: none;");

        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#SpantransportGSTMidPrt").attr("style", "display: none;");
        document.getElementById("SpantransportGSTMidPrt").innerHTML = "";

        $("#GSTLastPrt").css('border-color', "#ced4da");
        $("#SpantransportGST").attr("style", "display: none;");
        document.getElementById("SpantransportGST").innerHTML = "";
        $("#GSTMidPrt").val("");
        $("#GSTLastPrt").val("");
        $("#GSTMidPrt").attr("disabled", true);
        $("#GSTLastPrt").attr("disabled", true);
    }
    else {
        $("#GSTNumrequired").css("display", "inherit");
        $("#GSTMidPrt").attr("disabled", true);
        $("#GSTLastPrt").attr("disabled", false);
        var GstApplicable = $("#Hdn_GstApplicable").text();
        if (GstApplicable == "Y") {
            if ($("#CmnPANNumber").val() != "") {
                const panInput = $("#CmnPANNumber").val().toUpperCase();
                $("#CmnPANNumber").val(panInput); // force uppercase in input field
                $("#GSTMidPrt").val(panInput);
            }
        }
        GstStateCodeOnChangeStatus();
    }
    ValidationForPanNoSetAttr();
}