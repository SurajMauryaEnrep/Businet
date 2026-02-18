
$(document).ready(function () {
    EditDistrict();
    Editstate();
    EditCity();
    //onchangeselectcountry();
    //onchangeselectcountrystate();

    //$("#datatable-buttons tbody tr").on("click", function (event) {
    //    $("#datatable-buttons tbody tr").css("background-color", "#ffffff");
    //    $(this).css("background-color", "rgba(38, 185, 154, .16)");
    //});

    //$("#datatable-buttons1 tbody tr").on("click", function (event) {
    //    $("#datatable-buttons1 tbody tr").css("background-color", "#ffffff");
    //    $(this).css("background-color", "rgba(38, 185, 154, .16)");
    //});

    //$("#datatable-buttons2 tbody tr").on("click", function (event) {
    //    $("#datatable-buttons2 tbody tr").css("background-color", "#ffffff");
    //    $(this).css("background-color", "rgba(38, 185, 154, .16)");
    //});

    var field = document.getElementById("country_state");
    var value = document.getElementById("country_Name");
    autocomplete(field, value);

    var field1 = document.getElementById("state_district");
    var value1 = document.getElementById("dist_state_Name");
    autocomplete(field1, value1);

    var fieldC = document.getElementById("Cdistrictname");
    var valueC = document.getElementById("CdistrictSearch");
    autocomplete(fieldC, valueC);
    $("#requiredStateCode").attr("style", "display: none;");

})
async function autocomplete(field, value) {
    const url = $(value).val();

    $(field).select2({
        ajax: {
            transport: async function (params, success, failure) {
                try {
                    const name = params.data.term || "";
                    const query = new URLSearchParams({ Name: name }).toString();
                    const response = await fetch(`${url}?${query}`, {
                        method: 'GET',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                    });

                    if (!response.ok) throw new Error('Network response was not ok');

                    const data = await response.json();

                    success(data);
                } catch (error) {
                    console.error("Autocomplete fetch error:", error);
                    failure(error);
                }
            },
            processResults: function (data, params) {
                const pageSize = 20;
                const page = params.page || 1;
                const Fdata = data.slice((page - 1) * pageSize, page * pageSize);

                return {
                    results: $.map(Fdata, function (val) {
                        return {
                            id: val.ID,
                            text: val.Name
                        };
                    }),
                    pagination: {
                        more: (page * pageSize) < data.length
                    }
                };
            },
            delay: 250,
            cache: true
        }
    });
}

//function autocomplete(field,value) {


//    $(field).select2({

//        ajax: {
//            url: $(value).val(),
//            data: function (params) {
//                var queryParameters = {
//                    Name: params.term
//                };
//                return queryParameters;
//            },
//            dataType: "json",
//            cache: true,
//            delay: 250,
//            //type: 'POST',
//            contentType: "application/json; charset=utf-8",
//            processResults: function (data, params) {
//                debugger;
//                var pageSize = 20;
//                var page = params.page || 1;
//                var Fdata = data.slice((page - 1) * pageSize, page * pageSize)
//               // params.page = params.page || 1;
//                return {
//                    results: $.map(Fdata, function (val, Item) {
//                        return { id: val.ID, text: val.Name };
//                    }),
//                    pagination: {
//                        more: (page * pageSize) < data.length
//                    }
//                };
//            },

//        },

//    });
//}

function onchangeselectcountry() {
    debugger;
    var ddlstate = $("#state_district").val();
    //var statedistrict = $("#state_district option:selected").text();

    //var ddlstate1 = $("#country_state1").val();
    //var ddlstate1 = $("#country_state1 option:selected").text();
    $.ajax({
        type: "POST",
        url: "/AddressStructure/Getcountryonchangecity",
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async: true,
        data: { state_id: ddlstate, },/*Registration pass value like model*/
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
                    $("#country_state1").val(arr[0].country_name);
                    $("#hdncountry_id_state").val(arr[0].country_id);
                }
                else {
                    $("#country_state1").val("");
                    $("#hdncountry_id_state").val("");
                }
               
            }
        },
        error: function (Data) {
        }
    });
    //var countrystate1 = $("#country_state1").val();
    //$("#hdnAction2").val(countrystate1);
};
function onchangeselectcountrystate() {
    debugger;
    var ddldistrict = $("#Cdistrictname").val();

    $.ajax({
        type: "POST",
        url: "/AddressStructure/Getcountryonchangedistrict",
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async: true,
        data: { district_id: ddldistrict, },/*Registration pass value like model*/
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
                    $("#Ccountry_name").val(arr[0].country_name);
                    $("#hdnCcountry_id").val(arr[0].country_id);
                    $("#Cstate_name").val(arr[0].state_name);
                    $("#hdnCstate_id").val(arr[0].state_id);
                }
                else {
                    $("#Ccountry_name").val("");
                    $("#hdnCcountry_id").val("");
                    $("#Cstate_name").val("");
                    $("#hdnCstate_id").val("");
                }

            }
        },
        error: function (Data) {
        }
    });
};

function ChakeValidation() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 03-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#btn_save").prop("disabled", true); /*End*/
    }
    var stateName = $("#StateName").val().trim();
    var stateCode = $("#StateCode").val().trim();
    var hdnCountryID = $("#hdn_Country_ID").val();
    var validationFlag = true;
    if (stateName == "") {
        document.getElementById("vmstate_name").innerHTML = $("#valueReq").text();
        $("#StateName").css("border-color", "red");
        validationFlag = false;
    }
    debugger;
    var val1 = $('#country_state').val().trim();
    if (val1 == hdnCountryID) {
        if (stateCode == "") {
            document.getElementById("vmStateCode").innerHTML = $("#valueReq").text();
            $("#StateCode").css("border-color", "red");
            validationFlag = false;
        }
    }
    else {
        //if (stateCode == "") {
        //    document.getElementById("vmStateCode").innerHTML = $("#valueReq").text();
        //    $("#StateCode").css("border-color", "red");
        //    validationFlag = false;
        //}
    }
    var OnlyNum = $("#StateCode").val()
    if (OnlyNum.length == 1) {
        document.getElementById("vmStateCode").innerHTML = $("#valueReq").text();
        $("#StateCode").css("border-color", "red");
        validationFlag = false;
    }
    var country_state = $("#country_state").val().trim();
   
    if (country_state == "" || country_state == "0") {
        document.getElementById("vmcountry_state").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-country_state-container']").css("border-color", "red");
        validationFlag = false;
    }
    if (validationFlag == true) {
        $("#country_state").attr("disabled", false);
        $("#StateCode").attr("disabled", false);
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");
        return true;
    }
    else {
        return false;
    }

}
function validatedistrict() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 03-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btn_saveDistict").css("filter", "grayscale(100%)");
        $("#btn_saveDistict").prop("disabled", true); /*End*/
    }
    var stateName = $("#districtName").val().trim();
    var validationFlag = true;
    if (stateName == "") {
        document.getElementById("vmdistrict_name").innerHTML = $("#valueReq").text();
        $("#districtName").css("border-color", "red");
        validationFlag = false;
    }
    var country_state = $("#state_district").val().trim();
    if (country_state == "" || country_state == "0") {
        document.getElementById("vmstate_district").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-state_district-container']").css("border-color", "red");
        validationFlag = false;
    }
   
    if (validationFlag == true) {
        $("#state_district").attr("disabled", false);
        $("#btn_saveDistict").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");
       
        return true;
    }
    else {
        return false;
    }
}
function chakevalidateCity() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 03-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btn_savecity").css("filter", "grayscale(100%)");
        $("#btn_savecity").prop("disabled", true); /*End*/
    }
    var stateName = $("#CityName").val().trim();
    var validationFlag = true;
    if (stateName == "") {
        document.getElementById("vmcityname").innerHTML = $("#valueReq").text();
        $("#CityName").css("border-color", "red");
        validationFlag = false;
    }
    
    if (country_state == "") {
        document.getElementById("vmpin").innerHTML = $("#valueReq").text();
        $("#Pin").css("border-color", "red");
        validationFlag = false;
    }
    var country_state = $("#Cdistrictname").val().trim();
    if (country_state == "") {
        document.getElementById("vmCdistrict").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-Cdistrictname-container']").css("border-color", "red");
        validationFlag = false;
    }

    if (validationFlag == true) {
        $("#Cdistrictname").attr("disabled", false);
        $("#btn_savecity").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");
        return true;
    }
    else {
        return false;
    }
}
function OnChangeCcity() {
    var val1 = $('#CityName').val().trim();
    if (val1 != "") {
        document.getElementById("vmcityname").innerHTML = "";
        $("#CityName").css("border-color", "#ced4da");
        validationFlag = false;
    }
}
//function OnChangeCpin() {
//    var val1 = $('#Pin').val().trim();
//    if (val1 != "") {
//        document.getElementById("vmpin").innerHTML = "";
//        $("#Pin").css("border-color", "#ced4da");
//        validationFlag = false;
//    }
//}
function OnChangeCdistrict() {
    var val1 = $('#Cdistrictname').val().trim();
    var ddlstate = $("#Cdistrictname option:selected").text();
    if (val1 != "") {
        document.getElementById("vmCdistrict").innerHTML = "";
        $("[aria-labelledby='select2-Cdistrictname-container']").css("border-color", "#ced4da");
        validationFlag = false;
    }
    $("#Cdistrictid").val(ddlstate);
}

function OnChangestate_state() {
    debugger;
    var val1 = $('#StateName').val().trim();
    if (val1 != "") {
        document.getElementById("vmstate_name").innerHTML = "";
        $("#StateName").css("border-color", "#ced4da");
        validationFlag = false;
    }
}
function OnChangecountry_state(flag) {
    debugger;
    var val1 = $('#country_state').val().trim();
    var ddlstate = $("#country_state option:selected").text();
    if (val1 != "") {
        document.getElementById("vmcountry_state").innerHTML = "";
        $("[aria-labelledby='select2-country_state-container']").css("border-color", "#ced4da");
        validationFlag = false;
    }
    $("#hdncountry_id").val(ddlstate);
    debugger;
    $.ajax({
        type: "POST",
        url: "/AddressStructure/GetStateCodeOnchange",
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async: true,
        data: { Country_id: val1, },/*Registration pass value like model*/
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

                debugger;
                //var countryID = arr.val();
                if (val1 != "0") {
                    if (val1 != arr[0].country) {
                        $("#StateCode").prop("disabled", true);
                        $("#requiredStateCode").attr("style", "display: none;");
                        $("#StateCode").val(null);
                        var Country_id = arr[0].country;
                        $("#hdn_Country_ID").val(Country_id)
                    }
                    else {
                        if (flag == "Edit") {
                            $("#StateCode").prop("disabled", true);
                            $("#requiredStateCode").css("display", "inherit");
                        }
                        else {
                            $("#StateCode").prop("disabled", false);
                            $("#requiredStateCode").css("display", "inherit");
                        }
                       
                    }
                }
                else {
                    if (flag == "Edit") {
                        $("#StateCode").prop("disabled", false);
                    }
                    //$("#requiredStateCode").css("display", "inherit");
                }
                OnChangeStateCode();
                //if (arr.length > 0) {
                //    //$("#country_state1").val(arr[0].country_name);
                //    //$("#hdncountry_id_state").val(arr[0].country_id);
                //}
                //else {
                //    $("#country_state1").val("");
                //    $("#hdncountry_id_state").val("");
                //}

            }
        },
        error: function (Data) {
        }
    });
    //debugger;
    //if (val1 != "101") {
    //    $("#StateCode").prop("disabled", true);
    //    $("#requiredStateCode").attr("style", "display: none;");
    //}
    //else {
    //    $("#StateCode").prop("disabled", false);
    //    $("#requiredStateCode").css("display", "inherit");
    //}
    //OnChangeStateCode();
}
function OnChangedistrict() {
    var val1 = $('#districtName').val().trim();
    if (val1 != "") {
        document.getElementById("vmdistrict_name").innerHTML = "";
        $("#districtName").css("border-color", "#ced4da");
        validationFlag = false;
    }
}
function OnChangedistrict_state() {
    var val1 = $('#state_district').val().trim();
    var ddlstate = $("#state_district option:selected").text();
    if (val1 != "") {
        document.getElementById("vmstate_district").innerHTML = "";
        $("[aria-labelledby='select2-state_district-container']").css("border-color", "#ced4da");
        validationFlag = false;
    }
    $("#hdncountryid").val(ddlstate);
}

function EditDistrict() {

    $(".editrecorddistrict").bind("click", function (e) {
        debugger;
        try {
            var clickedrow = $(e.target).closest("tr");
            //var district_id = clickedrow.children("td:eq(1)").text();
            //var district_name = clickedrow.children("td:eq(2)").text();
            //var state_id = clickedrow.children("td:eq(3)").text();
            var district_id = clickedrow.children("#districtid").text();
            var district_name = clickedrow.children("#distname").text();
            var state_id = clickedrow.children("#state_id").text();//Modifed by Shubham Maurya on 16-12-2022 for Edit not show data//
            var state_nm = clickedrow.children("#state_nm").text();
            var country_id = clickedrow.children("#country_id").text();
            var country_nm = clickedrow.children("#country_nm").text();
           
           /* var DCountry = "<option value=" + country_id + " selected>" + country_nm + "</option>"*/
            $("#country_state1").val(country_nm);
            $("#hdncountry_id_state").val(country_id);
            $("#hdnAction1").val("UpdateDistrict");
            $("#districtName").val(district_name);
            $("#district_id_hdn").val(district_id);
            var D = "<option value=" + state_id + " selected>" + state_nm + "</option>"
            $("#state_district").append(D);
            /*$("#state_district").val(state_id).trigger("change");*/

            var AddNewData = $("#AddNewData").val();
            if (AddNewData == "N") {
                $("#SaveBtnEnableDistrict").css("display", "block");
                $("#districtName").attr("disabled", false);
                $("#state_district").attr("disabled", false);
            }
            var dependcy = clickedrow.children("#hdndependcy").text();
            if (dependcy == "NotExit") {
                $("#state_district").attr("disabled", false);
            }
            else {
                $("#state_district").attr("disabled", true);
            }

        }
        catch (err) {
            debugger
            alert(err.message);
        }

        OnChangedistrict();
        OnChangedistrict_state();

    });

}


function EditCity() {
    debugger;
    $('#City_Table').on('click', '.editrecordCity', function (e) {
   // $(".editrecordCity").bind("click", function (e) {
        debugger;
        try {
            var clickedrow = $(e.target).closest("tr");
            //var city_id = clickedrow.children("td:eq(1)").text();
            //var city_name = clickedrow.children("td:eq(2)").text();
            //var district_id = clickedrow.children("td:eq(3)").text();
            var tr = $(this).closest('tr');
        
            var city_id = clickedrow.children("#cityid").text();
            var city_name = clickedrow.children("#cityname").text();
            var district_id = clickedrow.children("#distid").text();
            var district_nm = clickedrow.children("#district_nm").text();
            var state_id = clickedrow.children("#statid").text();//Modifed by Shubham Maurya on 16-12-2022 for Edit not show data//
            var state_nm = clickedrow.children("#statnm").text();
            var countrID = clickedrow.children("#countrID").text();
            var countrname = clickedrow.children("#countrname").text();
            $("#Cstate_name").val(state_nm);
            $("#hdnCstate_id").val(state_id);
            $("#Ccountry_name").val(countrname);
            $("#hdnCcountry_id").val(countrID);
            $("#ChdnAction").val("UpdatecityAndpin");
            $("#CityName").val(city_name);
            $("#city_id_hdn").val(city_id);
            var C = "<option value=" + district_id + " selected>" + district_nm + "</option>"
            /* $("#Cdistrictname").val(district_id).trigger("change");*/
            $("#Cdistrictname").append(C);
            OnChangeCcity();
            OnChangeCdistrict();

            var AddNewData = $("#AddNewData").val();
            if (AddNewData == "N") {
                $("#SaveBtnEnableCity").css("display", "block");
                $("#CityName").attr("disabled", false);
                $("#Cdistrictname").attr("disabled", false);
            }
            var dependcy1 = clickedrow.children("#hdndependcy1").text();
            if (dependcy1 == "NotExit") {
                $("#Cdistrictname").attr("disabled", false);
            }
            else {
                $("#Cdistrictname").attr("disabled", true);
            }
        }
        catch (err) {
            debugger
            alert(err.message);
        }
        OnChangedistrict();
        OnChangedistrict_state();
    });
}
function numValueOnly(el, evt) {
    debugger;
    var charCode = (evt.which) ? evt.which : event.keyCode;
    //if (charCode < 00) {
    //    return false;
    //}
    if (charCode > 31 && (charCode < 48 || charCode > 58)) {
        return false;
    }
    var OnlyNum = $("#StateCode").val()
    if (OnlyNum.length == 1) {
        if (OnlyNum == "0") {
            if (charCode == 48) {
                return false;
            }
        }
    } 
    return true;
}
function Editstate() {
  
    $(".editrecord").bind("click", function (e) {
        debugger;
        try {
            var clickedrow = $(e.target).closest("tr");
            //var state_Id = clickedrow.children("td:eq(1)").text();
            //var state_name = clickedrow.children("td:eq(2)").text();
            //var country_id = clickedrow.children("td:eq(3)").text();
            var state_Id = clickedrow.children("#stateid").text();
            var state_name = clickedrow.children("#statename").text();
            var stateCode = clickedrow.children("#state_code").text();
            var country_id = clickedrow.children("#contryid").text();
            var country_nm = clickedrow.children("#contryname").text();//Modifed by Shubham Maurya on 16-12-2022 for Edit not show data//
            $("#hdnAction").val("UpdateState");
            $("#StateName").val(state_name);
            if (stateCode == "") {
                $("#StateCode").val(null);
            }
            else {
                $("#StateCode").val(stateCode);
            }
            var s = "<option value=" + country_id + " selected>" + country_nm+"</option>"
            $("#country_state").append(s);//.trigger('change');
            $("#state_id_hdn").val(state_Id);
            $("#hdn_Country_ID").val(country_id)
            
            clickedrow = "";
            state_Id = "";
            state_name = "";
            country_id = "";

            var AddNewData = $("#AddNewData").val();
            if (AddNewData == "N") {
                $("#SaveBtnEnable").css("display", "block");
                $("#StateName").attr("disabled", false);
                if (stateCode != "") {
                    $("#StateCode").attr("disabled", false);
                }
                $("#country_state").attr("disabled", false);
            }

          //  window.location.href = "/BusinessLayer/AddressStructure/getstatedetails/?state_id=" + state_Id;
        }
        catch (err) {
            debugger
            alert(err.message);
        }

        OnChangestate_state();
        OnChangeStateCode();
        OnChangecountry_state("Edit");
        $("#country_state").attr("disabled", true);
    });
 
}
function functionConfirm_District(event) {
    $("#District_tbody").bind("click", function (e) {
        debugger;
        try {
            var clickedrow = $(e.target).closest("tr");
            /*var district_Id = clickedrow.children("td:eq(1)").text();*/
            var district_Id = clickedrow.children("#districtid").text();
            $("#district_id_hdn").val(district_Id);
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
            $("#hdnAction1").val("DeleteDistrict");
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}
function functionConfirm_City(event) {
    $("#CityAndPin_tbody").bind("click", function (e) {
        debugger;
        try {
            var clickedrow = $(e.target).closest("tr");
            /*var city_Id = clickedrow.children("td:eq(1)").text();*/
            var city_Id = clickedrow.children("#cityid").text();
            $("#city_id_hdn").val(city_Id);
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
            $("#ChdnAction").val("DeleteCityandPin");
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}
function OnChangeStateCode() {
    
    //var val1 = $('#StateCode').val().trim();
    //if (val1 != "") {
        document.getElementById("vmStateCode").innerHTML = "";
        $("#StateCode").css("border-color", "#ced4da");
    //    validationFlag = false;
    //}
}
function functionConfirm(event) {
    $("#adrstrcountry_tbody").bind("click", function (e) {
        debugger;
        try {
            var clickedrow = $(e.target).closest("tr");
            /* var state_Id = clickedrow.children("td:eq(1)").text();*/
            var state_Id = clickedrow.children("#stateid").text();
           
            //$("#hdnAction").val("DeleteState");
            $("#state_id_hdn").val(state_Id);

            //  window.location.href = "/BusinessLayer/AddressStructure/getstatedetails/?state_id=" + state_Id;
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
            $("#hdnAction").val("DeleteState");
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}

