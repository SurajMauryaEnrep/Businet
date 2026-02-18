$(document).ready(function () {
    debugger;
    /*start Commented by Hina on 12-01-2023 to chnge all addres dtl in dropdown*/
    //getSuppCity();
    onlyphonenumber();
    $("#OrgCountry").select2();
    $("#OrgState").select2();
    $("#OrgDistrict").select2();
    $("#OrgCity").select2();
    /*start Add by Hina on 05-08-2025*/
    $('#OrgState').append('<option value="0">---Select---</option>')
    $('#OrgDistrict').append('<option value="0">---Select---</option>')
   $('#OrgCity').append('<option value="0">---Select---</option>')
    /*end Add by Hina on 05-08-2025*/
    if ($("#Branchchk").is(":checked")) {
        suppcurr = "I";
        $("#decimal").attr("style", "display: none;");
   
    }
    else {
        $("#decimal").attr("style", "display: block;");
    }
    if ($("#HeadOfficechk").is(":checked")) {
        suppcurr = "D";
        $("#vmRoleHoName").css("display", "none");
        $("#RoleHoName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-RoleHoName-container']").css("border-color", "#ced4da");
        $('#suppcurr').empty();
        $('#suppcurr').empty();
    }
    if (suppcurr != "I") {
        $("#Headoffice").attr("style", "display: none;");
    }
    else {
        $("#Headoffice").css("display", "inherit");
    }
    $(this).on('click', '.simpleTree-label', function (e) {
        debugger;

        var Comid = this.nextSibling.innerText;

        var Parent = this.innerText;
        GetMenuData(Comid)
    });
    ReadCOMPAddress();/*Add by HIna Sharma on 05-08-2025*/
    /*------------Code start Add by Hina on 04-08-2025---------------*/
    $("#tblCompanyAddressDetail >tbody").on('click', "#editBtnIcon", function (e) {

        debugger;
        /*Add code by Hina on 09-01-2024 */
        var currentRow = $(this).closest('tr')

        //var currentRow = $(e.target).closest("tr")
        var row_index = currentRow.index();

        debugger;
        var address = currentRow.find("#tblcompadd").text()
        var cust_pin = currentRow.find("#tblpin").text()
        
      
        var Country = currentRow.find("#hdntblcntryid").text()
        var State = currentRow.find("#hdntblstateid").text()
        var District = currentRow.find("#hdntbldistid").text()
       
        var City = currentRow.find("#hdntblcityid").text()
        var address_id = currentRow.find("#address_id").text()
        debugger;
        var ShiddenRowNo = currentRow.find("#spanCompDefrowId").text();
        if (currentRow.find("#defcompadd div input").is(":checked")) {
            $("#chkCompDefaultAddress").prop("checked", true);
        }
        else {
            $("#chkCompDefaultAddress").prop("checked", false);
        }
        
        var col10_value = row_index
        debugger;
        $("#customer_address").val(address);
        $("#OrgPin").val(cust_pin);
        $("#HdnCountry").val(Country);
        $("#HdnState").val(State);
        $("#HdnDistrict").val(District);
        $("#HdnCity").val(City);
        $("#hdnSrRowId").val(ShiddenRowNo);
        $("#UpdtAddrId").val(address_id);
        $("#OrgCountry").val(Country).trigger("change");
        $("#divUpdate").css("display", "block");
        $('#divAddAddrDtl').css("display", "none");

        onchangePin();
        //onchangeGST();
    });
    $('#CompAddressBody').on('click', '.deleteIcon', function () {
        debugger;

        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            debugger;
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);

        });
        debugger;
        var addr_id = $(this).closest('tr')[0].cells[16].innerHTML;
        //var Flag = $(this).closest('tr')[0].cells[17].innerHTML;
        debugger;
        //var SuppStatus = $("#hfSuppStatus").val();
        //var custId = $("#Supp_Id").val();
        var IdonEdit = $("#hdnSrRowId").val();
        var Supplier_address = $("#customer_address").val();
        var Supplier_Pin = $("#OrgPin").val();
        var SupplierCity_Name = $("#OrgCity").val();
        currentRow = $(this).closest('tr');
        rowId = currentRow.find("#spanCompDefrowId").text();
        var addressSup = currentRow.find("#tblcompadd").text()
        var supp_pin = currentRow.find("#tblpin").text()
        var CitySup = currentRow.find("#hdntblcityid").text()
       /* if ((custId == null || custId == "") && (SuppStatus == null || SuppStatus == "" || SuppStatus == "Drafted")) {*/
        $(this).closest('tr').remove();
        if (IdonEdit == rowId) {
            //$(this).closest('tr').remove();
                ClearOrgAddressDetail();
            }
        document.getElementById("vmOrgCityname").innerHTML = "";
            $("[aria-labelledby='select2-OrgCity-container']").css('border-color', '#ced4da');
        $("#vmOrgCityname").attr("style", "display: none;");
        document.getElementById("vmOrgPin").innerHTML = "";
            $("#OrgPin").css('border-color', '#ced4da');
        $("#vmOrgPin").attr("style", "display: none;");
        document.getElementById("SpancustAddr").innerHTML = "";
            $("#customer_address").css('border-color', '#ced4da');
        $("#SpancustAddr").attr("style", "display: none;");
            /* For ResetBillingAddresToggleAfterDelete in Address Table */
            var firstactive = "N";
            var rowIdx1 = 1;
        var len = $("#tblCompanyAddressDetail >tbody >tr").length;
        $("#tblCompanyAddressDetail >tbody >tr").each(function (i, row) {
                var currentRow = $(this).closest('tr');
                if (firstactive == "N") {
                    if (len > 0) {
                        if (currentRow.find("#defcompadd div input").is(":checked")) {
                            firstactive = "Y";

                        }
                    }
                }
            });
        $("#tblCompanyAddressDetail >tbody >tr").each(function (i, row) {
                var currentRow = $(this);
                if (firstactive == "N") {
                    currentRow.find("#defcompadd div input").prop("checked", true);
                    $("#spanhiddenCompDef_" + rowIdx1).text("Y");
                    firstactive = "Y";
                }
            });
        //}

        //else {
        //    var currentrow = $(this).closest('tr');

        //    document.getElementById("vmSuppCityname").innerHTML = "";
        //    $("[aria-labelledby='select2-Supp_City-container']").css('border-color', '#ced4da');
        //    $("#vmSuppCityname").attr("style", "display: none;");
        //    document.getElementById("vmSuppPin").innerHTML = "";
        //    $("#Supp_Pin").css('border-color', '#ced4da');
        //    $("#vmSuppPin").attr("style", "display: none;");
        //    document.getElementById("SpanSuppAddr").innerHTML = "";
        //    $("#supplier_address").css('border-color', '#ced4da');
        //    $("#SpanSuppAddr").attr("style", "display: none;");

        //    ChakeDependencyAddr(custId, addr_id, currentrow);
        //}
        
        ReadCOMPAddress();


    });
    /*------------Code End Add by Hina on 04-08-2025---------------*/
});
function onlyphonenumber() {
    debugger;
    $('#mobileNumber').on('input', function () {
        var maxLength = 10;/*$(this).attr('maxlength');*/
        var value = $(this).val();

        if (value.length > maxLength) {
            $(this).val(value.slice(0, maxLength));
        }
    });
}
function QtyintValueonly(el, evt) {
        if (Cmn_IntValueonly(el, evt, "#QtyDigit") == false) {
            return false;
        }
    return true;
}
function onkeypressEntityName(event) {
    debugger
    if (event.key === "/" || event.key === "\\" || event.keyCode === 191 || event.keyCode === 220 || (event.key === " " && event.target.value.length === 0)) {
        event.preventDefault();      
    }
}
function GetMenuData(Comp_Id) {
    debugger;
    window.location.href = "/FactorySettings/OrganizationSetup/OSTreeView/?Comp_Id=" + Comp_Id;
}

function getSuppCity() {
    debugger;
    //Cmn_getCityList("cust_city");
    $("#cust_city").select2({
        ajax: {
            url: $("#TxtCity_Name").val(),

            data: function (params) {
                var queryParameters = {
                    //restrictedCountry: $("#resCountry").val(),  // pass your own parameter                
                    ddlGroup: params.term, // search term like "a" then "an"
                    Group: params.page
                    //ddlCity: $("#supplier_city").val()
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            //type: 'POST',
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
                params.page = params.page || 1;
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID, text: val.Name };
                    }),
                };
            },

        },

    });
}
function onchangecustcityOld() {
    debugger;
    GetsuppDSCntr();
    //cityonchange();
    
    /* onchageCounty();*/
}
function GetsuppDSCntr() {
    debugger;
    var ddlCity = $("#cust_city").val();
    onchangecity();
    $.ajax({
        type: "POST",
        url: "/OrganizationSetup/GetsuppDSCntr",/*Controller=SupplierSetup and Fuction=Getsuppport*/
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
                if (arr.Table.length > 0) {
                    $("#district_id").val(arr.Table[0].district_name);
                    $("#hdCustmerDistrictID").val(arr.Table[0].district_id);
                }
                if (arr.Table1.length > 0) {
                    $("#state_id").val(arr.Table1[0].state_name);
                    $("#hdCustmerStateID").val(arr.Table1[0].state_id);

                }
                if (arr.Table2.length > 0) {
                    debugger;
                    $("#country_id").val(arr.Table2[0].country_name);
                    $("#hdCustmerCountryID").val(arr.Table2[0].country_id);
                    var country_id = $('#country_id').val();
                    if (country_id != "India") {
                        $("#GSTMidPrt").prop("disabled", true);
                        $("#GSTLastPrt").prop("disabled", true);
                    }
                    else {
                        $("#GSTMidPrt").prop("disabled", false);
                        $("#GSTLastPrt").prop("disabled", false);
                    }
                }
                if (arr.Table3.length > 0) {
                    $("#gst_num").val(arr.Table3[0].state_code);
                    $("#hdCustmerStateID").val(arr.Table3[0].state_id);
                }

            }
        },
        error: function (Data) {
        }
    });
   
};

function onChangeCurrency_Formet() {
    var Currency_Formet = $("#Currency_Formet").val();
    if (Currency_Formet == "0") {
        $('#vmCurrency_Formet').text($("#valueReq").text());
        $("#vmCurrency_Formet").css("display", "block");
        $("#Currency_Formet").css("border-color", "Red");
        $("[aria-labelledby='select2-Currency_Formet-container']").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#vmCurrency_Formet").css("display", "none");
        $("#Currency_Formet").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-Currency_Formet-container']").css("border-color", "#ced4da");
        //$('#Branchchk').empty();  
        $("#Currency_Formet_id").val(Currency_Formet);
    }
}

function onchangecityOld() {
    debugger;
    $("#Spancustdist").attr("style", "display: none;");
    $("#district_id").attr("style", "border-color: #ced4da;");
    ;
    $("#Spancuststate").attr("style", "display: none;");
    $("#state_id").attr("style", "border-color: #ced4da;");
    ;
    $("#Spancustcntry").attr("style", "display: none;");
    $("#country_id").attr("style", "border-color: #ced4da;");
    
    document.getElementById("vmcust_city").innerHTML = "";
    $("#cust_city").css('border-color', '#ced4da');
    $("[aria-labelledby='select2-cust_city-container']").css('border-color', '#ced4da');
    $("#vmcust_city").attr("style", "display: none;");
}
function GetCustcurr() {
    debugger;
    var Branchchk;
    if ($("#Branchchk").is(":checked")) {
        Branchchk = "B";
        $("#decimal").attr("style", "display: none;");
    }
    else {
        $("#decimal").attr("style", "display: block;");
    }
    if ($("#HeadOfficechk").is(":checked")) {
        Branchchk = "H";
        $("#vmRoleHoName").css("display", "none");
      
        $("#RoleHoName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-RoleHoName-container']").css("border-color", "#ced4da");
    }
    if (Branchchk != "B") {
        $("#Headoffice").attr("style", "display: none;");
        $("#EntityName").val(null);
        $("#Entityprefix").val(null);
        $("#ddlStartDate").val(null);
        $("#FY_EndDate").val(null);

        $("#def_lang").val("0");
        $("#CurrencyName").val("0");
        $("#cont_pers").val(null);
        $("#cont_num1").val(null);
        $("#cont_email").val(null);
        $("#cont_pers1").val(null);
        $("#cont_num2").val(null);
        $("#cont_email1").val(null);
        $("#customer_address").val(null);
        /*commented by Hina on 12-01-2024 */
        //$("#Pin").val(null);
        //$("#cust_city").val("0").trigger('change');
        //$("#district_id").val(null);
        //$("#state_id").val(null);
        //$("#country_id").val(null);

        $("#OrgCountry_id").val("0").trigger('change');
        $("#OrgState_id").val(null);
        $("#OrgDistrict_id").val(null);
        $("#OrgCity").val(null);
        $("#OrgPin").val(null);

        $("#gst_num").val(null);
        $("#GSTMidPrt").val(null);
        $("#GSTLastPrt").val(null);
        $("#pannumber").val(null);
        $("#bank_benef").val(null);
        $("#bank_name").val(null);
        $("#bank_add").val(null);
        $("#bank_acc_no").val(null);
        $("#swift_code").val(null);
        $("#swift_code").val(null);
        $("#ifsc_code").val(null);
        $("#Currency_Formet").val("0");
        $("#Currency_Formet").prop("disabled", true);
        $("#DivCurrencyFormet").css("display", "none");
        $("#ddlStartDate").prop("disabled", false)
        $("#FY_EndDate").prop("disabled", false)
        $("#CurrencyName").prop("disabled", false)
    }
    else {
        $("#Headoffice").css("display", "inherit");
        $("#EntityName").val(null);
        $("#Entityprefix").val(null);
        $("#ddlStartDate").val(null);
        $("#FY_EndDate").val(null);
     
        $("#def_lang").val("0");
        $("#CurrencyName").val("0");
        $("#cont_pers").val(null);
        $("#cont_num1").val(null);
        $("#cont_email").val(null);
        $("#cont_pers1").val(null);
        $("#cont_num2").val(null);
        $("#cont_email1").val(null);
        $("#customer_address").val(null);
        /*commented by Hina on 12-01-2024 */
        //$("#Pin").val(null);
        //$("#TxtCity_Name").val("0");
        //$("#cust_city").val("0").trigger('change');
        //$("#district_id").val(null);
        //$("#state_id").val(null);
        //$("#country_id").val(null);

        $("#OrgCountry_id").val("0").trigger('change');
        $("#OrgState_id").val(null);
        $("#OrgDistrict_id").val(null);
        $("#OrgCity").val(null);
        $("#OrgPin").val(null);

        $("#gst_num").val(null);
        $("#GSTMidPrt").val(null);
        $("#GSTLastPrt").val(null);
        $("#pannumber").val(null);
        $("#bank_benef").val(null);
        $("#bank_name").val(null);
        $("#bank_add").val(null);
        $("#bank_acc_no").val(null);
        $("#swift_code").val(null);
        $("#swift_code").val(null);
        $("#ifsc_code").val(null);
        $("#Quantity").val(null);
        $("#Quantity_Value").val(null);
        $("#Rate1").val(null);
        $("#DivCurrencyFormet").css("display", "");
        $("#Currency_Formet").prop("disabled", false);
        $("#Currency_Formet").val("0");
        $("#ddlStartDate").prop("disabled", true)
        $("#FY_EndDate").prop("disabled", true)
        $("#CurrencyName").prop("disabled", true)
    }   
}

function showPreview(event) {
    if (event.target.files.length > 0) {
        let src = URL.createObjectURL(event.target.files[0]);
        let preview = document.getElementById("file-ip-1-preview");
        preview.src = src;
        preview.style.display = "block";
    }
}
function showdigisignPreview(event) {
    debugger;
    if (event.target.files.length > 0) {
        let src = URL.createObjectURL(event.target.files[0]);
        let preview = document.getElementById("file-ip-2-preview");
        preview.src = src;
        preview.style.display = "block";
    }
}

function myImgRemove() {
    var dfltimg = $("#defltimg")[0].src;
    document.getElementById("file-ip-1-preview").src = dfltimg;
    document.getElementById("file-ip-1").value = null;
    $("#hdn_Attatchment_details").val(null);
    $("#hdnAttachment").val(null);
}
function DigiSignImgRemove() {
    var dfltimg = $("#defltimg2")[0].src;
    document.getElementById("file-ip-2-preview").src = dfltimg;
    document.getElementById("file-ip-2").value = null;
}
function CheckSOValidations() {
    debugger;
    var Branchchk;
    var ErrorFlag = "N";
    if ($("#EntityName").val() == "" || $("#EntityName").val() == null) {
        $('#vmentity').text($("#valueReq").text());
        $("#vmentity").css("display", "block");
        $("#EntityName").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        var Entity_Name_valid = $("#ValidEntity_Name").val();
        if (Entity_Name_valid == "Y") {
            document.getElementById("vmentity").innerHTML = $("#valueduplicate").text();
            $("#EntityName").css("border-color", "red");
            $("#vmentity").css("display", "block");
            ErrorFlag = "Y";
            // return false;
        }
        else {
            $("#vmentity").css("display", "none");
            $("#EntityName").css("border-color", "#ced4da");
        }
      
    }
   
    if ($("#Branchchk").is(":checked")) {
        Branchchk = "I";
        if ($("#RoleHoName").val() == "0") {
            $('#vmRoleHoName').text($("#valueReq").text());          
            $("#vmRoleHoName").css("display", "block");
            $("#RoleHoName").css("border-color", "Red");
            $("[aria-labelledby='select2-RoleHoName-container']").css("border-color", "Red");     
            ErrorFlag = "Y";
        }
        else {
            $("#vmRoleHoName").css("display", "none");
            $("#RoleHoName").css("border-color", "#ced4da");
            $("[aria-labelledby='select2-RoleHoName-container']").css("border-color", "#ced4da");
            //$('#Branchchk').empty();   
        }

        if ($("#Currency_Formet").val() == "0") {
            $('#vmCurrency_Formet').text($("#valueReq").text());
            $("#vmCurrency_Formet").css("display", "block");
            $("#Currency_Formet").css("border-color", "Red");
            $("[aria-labelledby='select2-Currency_Formet-container']").css("border-color", "Red");
            ErrorFlag = "Y";
        }
        else {
            $("#vmCurrency_Formet").css("display", "none");
            $("#Currency_Formet").css("border-color", "#ced4da");
            $("[aria-labelledby='select2-Currency_Formet-container']").css("border-color", "#ced4da");
            //$('#Branchchk').empty();   
        }
    }
    else {
            $("#vmRoleHoName").css("display", "none");
            $("#RoleHoName").css("border-color", "#ced4da");
            $("[aria-labelledby='select2-RoleHoName-container']").css("border-color", "#ced4da");
        //$('#suppcurr').empty();  
    }

   

    if ($("#ddlStartDate").val() == "" || $("#EntityName").val() == null) {
        $('#vmFY_StartDate').text($("#valueReq").text());
        $("#vmFY_StartDate").css("display", "block");
        $("#ddlStartDate").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#vmFY_StartDate").css("display", "none");
        $("#ddlStartDate").css("border-color", "#ced4da");
    }

    if ($("#FY_EndDate").val() == "" || $("#EntityName").val() == null) {
        $('#vmFY_EndDate').text($("#valueReq").text());
        $("#vmFY_EndDate").css("display", "block");
        $("#FY_EndDate").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#vmFY_EndDate").css("display", "none");
        $("#FY_EndDate").css("border-color", "#ced4da");
    }

    if ($("#def_lang").val() == "0") {
        $('#vmdef_lang').text($("#valueReq").text());
        $("#vmdef_lang").css("display", "block");
        $("#def_lang").css("border-color", "Red");
       /* $("[aria-labelledby='select2-def_lang-container']").css("border-color", "Red");*/
        ErrorFlag = "Y";
    }
    else {
        $("#vmdef_lang").css("display", "none");
        $("#def_lang").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-def_lang-container']").css("border-color", "#ced4da");
     
    }

    if ($("#CurrencyName").val() == "0" || $("#ddlStartDate").val() == "") {
        $('#vmCurrname').text($("#valueReq").text());    
        $("#vmCurrname").css("display", "block");
        $("#CurrencyName").css("border-color", "Red");
        $("[aria-labelledby='select2-CurrencyName-container']").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#vmCurrname").css("display", "none");
        $("#CurrencyName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-CurrencyName-container']").css("border-color", "#ced4da");
     
    }
 
   
    if ($("#Entityprefix").val() == "" || $("#Entityprefix").val() == null) {
        $('#vmEntityPrefix').text($("#valueReq").text());
        $("#vmEntityPrefix").css("display", "block");
        $("#Entityprefix").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        var CheckLength = CheckLengthInput();
        if (CheckLength == false) {
            ErrorFlag = "Y";
        }
        else {
            var Entity_prefix_valid = $("#ValidEntity_prefix").val();
            if (Entity_prefix_valid == "Y") {
                document.getElementById("vmEntityPrefix").innerHTML = $("#valueduplicate").text();
                $("#Entityprefix").css("border-color", "red");
                $("#vmEntityPrefix").css("display", "block");
                ErrorFlag = "Y";

            }
            else {
                $("#vmEntityPrefix").css("display", "none");
                $("#Entityprefix").css("border-color", "#ced4da");
            }

        }

    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function SaveBtnClick() {
    /* InsertSODetail();*/
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 15-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    if (InsertSODetail() == false) {
        return false;
    }
    else {
        debugger;
        /***Add By Shubham Maurya on 15-11-2025 Start***/
        var FinalLicenceDetail = [];
        FinalLicenceDetail = InsertFinalLicenceDetail();
        $("#hdnLincenceDetail").val(JSON.stringify(FinalLicenceDetail));
        /***Add By Shubham Maurya on 15-11-2025 End***/

        ReadCOMPAddress();/*Add by HIna Sharma on 05-08-2025*/
        $("#Entityprefix").attr("disabled", false)
        $("#HeadOfficechk").attr("disabled", false)
        $("#ddlStartDate").attr("Disabled", false)
        $("#FY_EndDate").attr("Disabled", false)
        $("#CurrencyName").attr("Disabled", false)
        $("#Quantity").attr("Disabled", false)
        $("#Quantity_Value").attr("Disabled", false)
        $("#Rate1").attr("Disabled", false)
        var curr_name = $("#CurrencyName option:selected").text();
        var def_lang = $("#def_lang option:selected").text();
        var cuur_id = $("#CurrencyName").val();       
        $("#curr_id").val(cuur_id);
        $("#currency_name").val(curr_name);
        $("#Language").val(def_lang);
        $("#hdnsavebtn").val("AllreadyclickSaveBtn");
        return true;
    }
   /* return false;*/
}
function InsertSODetail() {
    var Branchchk = "";
    if (CheckSOValidations() == false) {
        return false;
    }
    //if (CheckOS_Com_add_Validations() == false) {/*commenetd and modify by hina sharma on 05-08-2025*/
    //    return false;
    //}
    //if (Check_Com_GST_Validations() == false) {
    //    return false;
    //}
    if (ValidateGSTNew() == false) {
        return false;
    }
    
    if (CheckDefaultaddress() == false) {/*Add by Hina Sharma on 05-08-2025*/
        
        return false;
    }
    // Commented By Nitesh 12-10-2023 1051 not mandatory Bank Deatil
    //if (CheckOS_Bank_Detail_Validations() == false) {   
    //    return false;
    //}
    if (ValidateEmail() == false) {
        return false;
    }
    if ($("#Branchchk").is(":checked")) {
        Branchchk = "B";
    }
    else {
        Branchchk="H"
    }
    if (Branchchk != "B") {
        if (CheckOS_Decimal_Setup_Validations() == false) {
            return false;
        }
    }
}
function onchangepan() {
    document.getElementById("vmPANNumber").innerHTML = "";
    $("#pannumber").css('border-color', "#ced4da");
    $("#vmPANNumber").attr("style", "display: none;");
}
function onchangeGST1() {
    //var suptype;
    //if ($("#Import").is(":checked")) {
    //    suptype = "I";
    //}
    //if ($("#Local").is(":checked")) {
    //    suptype = "D";
    //}
    var SupplierGST = $("#gst_num").val();
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
function onchageCountyOld() {
    debugger;
    var CustomerCountry = $("#country_id").val();
    if (CustomerCountry == "India") {

    }
    else {
        $("#GSTMidPrt").prop("disabled", true);
        $("#GSTLastPrt").prop("disabled", true);
    }
}
function onchangeCustPin() {
    debugger;
    document.getElementById("vmOrgPin").innerHTML = "";
    $("#OrgPin").css('border-color', '#ced4da');
    $("#vmOrgPin").attr("style", "display: none;");
}
function CheckOS_Com_add_Validations() {
    //debugger;
    var valFalg = true;
    var customer_address = $("#customer_address").val();
    
    var Country = $("#OrgCountry").val();
    var State = $("#OrgState").val();
    var District = $("#OrgDistrict").val();
    var City = $("#OrgCity").val();
    var Pin = $("#OrgPin").val();
    var GST = $("#gst_num").val();
    var GSTNoMidPrt = $("#GSTMidPrt").val().trim();
    var GSTNoLastPrt = $("#GSTLastPrt").val().trim();
    var pannumber = $("#pannumber").val();

   if (customer_address == "" || customer_address == null ) {
        document.getElementById("SpancustAddr").innerHTML = $("#valueReq").text();
        $("#customer_address").css('border-color', 'Red');
        $("#SpancustAddr").attr("style", "display: block;");
        valFalg = false;
    }
    else {
        document.getElementById("SpancustAddr").innerHTML = "";
        $("#customer_address").css('border-color', '#ced4da');
        $("#SpancustAddr").attr("style", "display: none;");      
    }
    if (Country == "0") {
        document.getElementById("vmOrgCountryname").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-OrgCountry-container']").css("border-color", "red");
        valFalg = false;
    }
    if (State == "0") {
        document.getElementById("vmOrgStatename").innerHTML = $("#valueReq").text();
        $("#OrgState").css("border-color", "red");
        $("[aria-labelledby='select2-OrgState-container']").css("border-color", "red");
        valFalg = false;
    }
    if (District == "0") {
        document.getElementById("vmOrgDistname").innerHTML = $("#valueReq").text();
        $("#OrgDistrict").css("border-color", "red");
        $("[aria-labelledby='select2-OrgDistrict-container']").css("border-color", "red");
        valFalg = false;
    }
    if (City == "0") {
        document.getElementById("vmOrgCityname").innerHTML = $("#valueReq").text();
        $("#OrgCity").css("border-color", "red");
        $("[aria-labelledby='select2-OrgCity-container']").css("border-color", "red");
        valFalg = false;
    }
    if (Pin == "") {
        document.getElementById("vmOrgPin").innerHTML = $("#valueReq").text();
        $("#OrgPin").css("border-color", "red");
        valFalg = false;
    }
    
    if (Country == "101") {
        if (pannumber == "") {
            document.getElementById("vmPANNumber").innerHTML = $("#valueReq").text();
            $("#pannumber").css('border-color', 'Red');
            $("#vmPANNumber").attr("style", "display: block;");
            valFalg = false;
        }
        else {

            document.getElementById("vmPANNumber").innerHTML = "";
            $("#pannumber").css('border-color', '#ced4da');
            $("#vmPANNumber").attr("style", "display: none;");
        }
    }
    
    var CustomerCountry = $("#OrgCountry").val();
    if (CustomerCountry == "0" || CustomerCountry == "" || CustomerCountry == "101") {
        if (GSTNoMidPrt == "" && GSTNoLastPrt != "") {
            $("#GSTMidPrt").css('border-color', 'red');
            $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
            document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#valueReq").text();
            valFalg = false;
        }
        else if (GSTNoMidPrt != "" && GSTNoMidPrt.length != "10") {
            $("#GSTMidPrt").css('border-color', 'red');
            $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
            document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#InvalidGSTNumber").text();
            valFalg = false;
        }
        else if (GSTNoMidPrt != "" && GSTNoLastPrt == "") {
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
        debugger;
        if (ValidateGST() == false) {
            return false;
        }
    }
    else {
        if (GSTNoMidPrt != "" || GSTNoLastPrt != "") {
            if (GSTNoMidPrt == "" && GSTNoLastPrt != "") {
                $("#GSTMidPrt").css('border-color', 'red');
                $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
                document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#valueReq").text();
                valFalg = false;
            }
            else if (GSTNoMidPrt != "" && GSTNoMidPrt.length != "10") {
                $("#GSTMidPrt").css('border-color', 'red');
                $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
                document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#InvalidGSTNumber").text();
                valFalg = false;
            }
            else if (GSTNoMidPrt != "" && GSTNoLastPrt == "") {
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
            debugger;
            if (ValidateGST() == false) {
                return false;
            }
        }
    }
    if (valFalg == true) {
        debugger;
       return true;
    }
    else {
        return false;
    }
}
/*start BREAK COMPANY ADD VALidation by Hina on 05-08-2025 */
function Check_Com_add_Validations() {
   
    var valFalg = true;
    
    var customer_address = $("#customer_address").val();
    
    var Country = $("#OrgCountry").val();
    var State = $("#OrgState").val();
    var District = $("#OrgDistrict").val();
    var City = $("#OrgCity").val();
    var Pin = $("#OrgPin").val();
    
    if (customer_address == "" || customer_address == null) {
        document.getElementById("SpancustAddr").innerHTML = $("#valueReq").text();
        $("#customer_address").css('border-color', 'Red');
        $("#SpancustAddr").attr("style", "display: block;");
        valFalg = false;
    }
    else {
        document.getElementById("SpancustAddr").innerHTML = "";
        $("#customer_address").css('border-color', '#ced4da');
        $("#SpancustAddr").attr("style", "display: none;");
    }
    if (Country == "0") {
        document.getElementById("vmOrgCountryname").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-OrgCountry-container']").css("border-color", "red");
        valFalg = false;
    }
    if (State == "0") {
        document.getElementById("vmOrgStatename").innerHTML = $("#valueReq").text();
        $("#OrgState").css("border-color", "red");
        $("[aria-labelledby='select2-OrgState-container']").css("border-color", "red");
        valFalg = false;
    }
    if (District == "0") {
        document.getElementById("vmOrgDistname").innerHTML = $("#valueReq").text();
        $("#OrgDistrict").css("border-color", "red");
        $("[aria-labelledby='select2-OrgDistrict-container']").css("border-color", "red");
        valFalg = false;
    }
    if (City == "0") {
        document.getElementById("vmOrgCityname").innerHTML = $("#valueReq").text();
        $("#OrgCity").css("border-color", "red");
        $("[aria-labelledby='select2-OrgCity-container']").css("border-color", "red");
        valFalg = false;
    }
    if (Pin == "") {
        document.getElementById("vmOrgPin").innerHTML = $("#valueReq").text();
        $("#OrgPin").css("border-color", "red");
        valFalg = false;
    }
    
    if (Country == "101") {
        if (pannumber == "") {
            document.getElementById("vmPANNumber").innerHTML = $("#valueReq").text();
            $("#pannumber").css('border-color', 'Red');
            $("#vmPANNumber").attr("style", "display: block;");
            valFalg = false;
        }
        else {

            document.getElementById("vmPANNumber").innerHTML = "";
            $("#pannumber").css('border-color', '#ced4da');
            $("#vmPANNumber").attr("style", "display: none;");
        }
    }
    if (valFalg == true) {
       return true;
    }
    else {
        return false;
    }
}
function Check_Com_GST_Validations() {
    debugger;
    var valFalg = true;
    
    var pannumber = $("#pannumber").val();
    var GST = $("#gst_num").val();
    var GSTNoMidPrt = $("#GSTMidPrt").val().trim();
    var GSTNoLastPrt = $("#GSTLastPrt").val().trim();
    
    var pannumber = $("#pannumber").val();
    var Country = $("#OrgCountry").val();
    //if (Country == "101") {/*commented start by hina on 06-08-225*/
    //    if (pannumber == "") {
    //        document.getElementById("vmPANNumber").innerHTML = $("#valueReq").text();
    //        $("#pannumber").css('border-color', 'Red');
    //        $("#vmPANNumber").attr("style", "display: block;");
    //        valFalg = false;
    //    }
    //    else {

    //        document.getElementById("vmPANNumber").innerHTML = "";
    //        $("#pannumber").css('border-color', '#ced4da');
    //        $("#vmPANNumber").attr("style", "display: none;");
    //    }
    //}
    /*commented end by hina on 06-08-225*/
    var CustomerCountry = $("#OrgCountry").val();
    /* if (CustomerCountry == "0" || CustomerCountry == "" || CustomerCountry == "101") {*//*commented start by hina on 06-08-225*/
    if (GST.length > 0 || GSTNoMidPrt.length > 0 || GSTNoLastPrt.length > 0) {
        //const input = document.getElementById("gst_num");
        //const value = input.value.trim();
        //const messageText = $("#onlytwonoAllowed").text();
        //if (value.length < 2 || value.length > 2) {
        //    document.getElementById("vmgst_num").innerHTML = messageText;
        //    $("#gst_num").css("border-color", "red");
        //    $("#vmgst_num").css("display", "block");
        //    error_flag = "Y";
        //}
        //else {
        //    document.getElementById("vmEntityPrefix").innerHTML = "";
        //    $("#vmgst_num").css("display", "none");
        //    $("#gst_num").css("border-color", "#ced4da");
        //}
        if (GSTNoMidPrt.length > 0 || GSTNoLastPrt.length > 0) {
            //var CheckLength = CheckLengthGStFirstPart();
            //if (CheckLength == false) {
            //    valFalg = false;
            //}
        }

        if (GSTNoMidPrt == "" && GSTNoLastPrt != "") {
            $("#GSTMidPrt").css('border-color', 'red');
            $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
            document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#valueReq").text();
            valFalg = false;
        }
        else if (GSTNoMidPrt != "" && GSTNoMidPrt.length != "10") {
            $("#GSTMidPrt").css('border-color', 'red');
            $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
            document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#InvalidGSTNumber").text();
            valFalg = false;
        }
        else if (GSTNoMidPrt != "" && GSTNoLastPrt == "") {
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

        if (GSTNoMidPrt != "" || GSTNoLastPrt != "") {
            var SuppGSTNum = GST + GSTNoMidPrt + GSTNoLastPrt
            $("#GSTNumber").val(SuppGSTNum);
        }
        else {
            if (GSTNoMidPrt == "" || GSTNoLastPrt == "") {
                var SuppGSTNum = GSTNoMidPrt + GSTNoLastPrt
                $("#GSTNumber").val(SuppGSTNum);
            }
        }
        debugger;
        if (ValidateGST() == false) {
            return false;
        }
    }
    else {
        document.getElementById("vmgst_num").innerHTML = "";
        $("#gst_num").css('border-color', '#ced4da');
        $("#vmgst_num").attr("style", "display: none;");
    }
    //}
    //else {
    //    if (GSTNoMidPrt != "" || GSTNoLastPrt != "") {
    //        if (GSTNoMidPrt == "" && GSTNoLastPrt != "") {
    //            $("#GSTMidPrt").css('border-color', 'red');
    //            $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
    //            document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#valueReq").text();
    //            valFalg = false;
    //        }
    //        else if (GSTNoMidPrt != "" && GSTNoMidPrt.length != "10") {
    //            $("#GSTMidPrt").css('border-color', 'red');
    //            $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
    //            document.getElementById("SpanSuppGSTMidPrt").innerHTML = $("#InvalidGSTNumber").text();
    //            valFalg = false;
    //        }
    //        else if (GSTNoMidPrt != "" && GSTNoLastPrt == "") {
    //            $("#GSTLastPrt").css('border-color', 'red');
    //            $("#SpanSuppGST").attr("style", "display: block;");
    //            document.getElementById("SpanSuppGST").innerHTML = $("#valueReq").text();
    //            valFalg = false;
    //        }
    //        else if (GSTNoLastPrt != "" && GSTNoLastPrt.length != "3") {
    //            $("#GSTLastPrt").css('border-color', 'red');
    //            $("#SpanSuppGST").attr("style", "display: block;");
    //            document.getElementById("SpanSuppGST").innerHTML = $("#InvalidGSTNumber").text();
    //            valFalg = false;
    //        }
    //        if (GSTNoMidPrt != "" || GSTNoLastPrt) {
    //            var SuppGSTNum = GST + GSTNoMidPrt + GSTNoLastPrt
    //            $("#GSTNumber").val(SuppGSTNum);
    //        }
    //        else {
    //            if (GSTNoMidPrt == "" || GSTNoLastPrt == "") {
    //                var SuppGSTNum = GSTNoMidPrt + GSTNoLastPrt
    //                $("#GSTNumber").val(SuppGSTNum);
    //            }
    //        }
    //        debugger;
    //        if (ValidateGST() == false) {
    //            return false;
    //        }
    //    }
    //}/*commented start by hina on 06-08-225*/
    if (valFalg == true) {
        return true;
    }
    else {
        return false;
    }
}
/*End BREAK COMPANY ADD VALidation by Hina on 05-08-2025 */
function ValidateGST() {
    //debugger;
    var GST = $("#gst_num").val();
    var GSTNoMidPrt = $("#GSTMidPrt").val().trim();
    var GSTNoLastPrt = $("#GSTLastPrt").val().trim();

    var valFalg = true;
    debugger;
    if (GST == "") {
        $("#gst_num").css('border-color', 'red');
        $("#SpanSuppGST").attr("style", "display: block;");
        document.getElementById("SpanSuppGST").innerHTML = $("#valueReq").text();
        valFalg = false;
    }
    else if (GST != "" && GST.length != "2") {
        $("#gst_num").css('border-color', 'red');
        $("#SpanSuppGST").attr("style", "display: block;");
        document.getElementById("SpanSuppGST").innerHTML = $("#InvalidGSTNumber").text();
        valFalg = false;
    }
    if (GSTNoMidPrt == "") {
        $("#GSTMidPrt").css('border-color', 'red');
        $("#SpanSuppGSTMidPrt").attr("style", "display: block;");
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
    if (valFalg == true) {
        return true;
    }
    else {
        return false;
    }
}
//function CheckOS_Bank_Detail_Validations()
//{
//    debugger;
//    var valFalg = true;
//    var bank_benef = $("#bank_benef").val();
//    var bank_name = $("#bank_name").val();
//    var bank_add = $("#bank_add").val();
//    var bank_acc_no = $("#bank_acc_no").val();
//    var swift_code = $("#swift_code").val();
//    var ifsc_code = $("#ifsc_code").val();
//    if (bank_benef == "") {
//        document.getElementById("vmbank_benef").innerHTML = $("#valueReq").text();
//        $("#bank_benef").css('border-color', 'Red');
//        $("#vmbank_benef").attr("style", "display: block;");
//        valFalg = false;
//    }
//    else {


//        document.getElementById("vmbank_benef").innerHTML = "";
//        $("#bank_benef").css('border-color', '#ced4da');
//        $("#vmbank_benef").attr("style", "display: none;");

//    }

//    if (bank_name == "") {
//        document.getElementById("vmbank_name").innerHTML = $("#valueReq").text();
//        $("#bank_name").css('border-color', 'Red');
//        $("#vmbank_name").attr("style", "display: block;");
//        valFalg = false;
//    }
//    else {


//        document.getElementById("vmbank_name").innerHTML = "";
//        $("#bank_name").css('border-color', '#ced4da');
//        $("#vmbank_name").attr("style", "display: none;");

//    }

//    if (bank_add == "") {
//        document.getElementById("vmbank_add").innerHTML = $("#valueReq").text();
//        $("#bank_add").css('border-color', 'Red');
//        $("#vmbank_add").attr("style", "display: block;");
//        valFalg = false;
//    }
//    else {


//        document.getElementById("vmbank_add").innerHTML = "";
//        $("#bank_add").css('border-color', '#ced4da');
//        $("#vmbank_add").attr("style", "display: none;");

//    }

//    if (bank_acc_no == "") {
//        document.getElementById("vmbank_acc_no").innerHTML = $("#valueReq").text();
//        $("#bank_acc_no").css('border-color', 'Red');
//        $("#vmbank_acc_no").attr("style", "display: block;");
//        valFalg = false;
//    }
//    else {


//        document.getElementById("vmbank_acc_no").innerHTML = "";
//        $("#bank_acc_no").css('border-color', '#ced4da');
//        $("#vmbank_acc_no").attr("style", "display: none;");

//    }

//    if (swift_code == "") {
//        document.getElementById("vmswift_code").innerHTML = $("#valueReq").text();
//        $("#swift_code").css('border-color', 'Red');
//        $("#vmswift_code").attr("style", "display: block;");
//        valFalg = false;
//    }
//    else {


//        document.getElementById("vmswift_code").innerHTML = "";
//        $("#swift_code").css('border-color', '#ced4da');
//        $("#vmswift_code").attr("style", "display: none;");

//    }

//    if (ifsc_code == "") {
//        document.getElementById("vmifsc_code").innerHTML = $("#valueReq").text();
//        $("#ifsc_code").css('border-color', 'Red');
//        $("#vmifsc_code").attr("style", "display: block;");
//        valFalg = false;
//    }
//    else {


//        document.getElementById("vmifsc_code").innerHTML = "";
//        $("#ifsc_code").css('border-color', '#ced4da');
//        $("#vmifsc_code").attr("style", "display: none;");

//    }
//    if (valFalg == true) {
//        return true;
//    }
//    else {
//        return false;
//    }

//}
function CheckOS_Decimal_Setup_Validations()
{
    var valFalg = true;
    var Quantity = $("#Quantity").val();
    var Quantity_Value = $("#Quantity_Value").val();
    var Rate1 = $("#Rate1").val();
    var Weight = $("#Weightid").val();
    var Exchangeid = $("#Exchangeid").val();
    if (Quantity == "") {
        document.getElementById("vmQuantity").innerHTML = $("#valueReq").text();
        $("#Quantity").css('border-color', 'Red');
        $("#vmQuantity").attr("style", "display: block;");
        valFalg = false;
    }
    else {


        document.getElementById("vmQuantity").innerHTML = "";
        $("#Quantity").css('border-color', '#ced4da');
        $("#vmQuantity").attr("style", "display: none;");

    }

    if (Quantity_Value == "") {
        document.getElementById("vmQuantity_Value").innerHTML = $("#valueReq").text();
        $("#Quantity_Value").css('border-color', 'Red');
        $("#vmQuantity_Value").attr("style", "display: block;");
        valFalg = false;
    }
    else {


        document.getElementById("vmQuantity_Value").innerHTML = "";
        $("#Quantity_Value").css('border-color', '#ced4da');
        $("#vmQuantity_Value").attr("style", "display: none;");

    }

    if (Rate1 == "") {
        document.getElementById("vmRate").innerHTML = $("#valueReq").text();
        $("#Rate1").css('border-color', 'Red');
        $("#vmRate").attr("style", "display: block;");
        valFalg = false;
    }
    else {


        document.getElementById("vmRate").innerHTML = "";
        $("#Rate1").css('border-color', '#ced4da');
        $("#vmRate").attr("style", "display: none;");

    }
    if (Weight == "") {
        document.getElementById("vmWeight").innerHTML = $("#valueReq").text();
        $("#Weightid").css('border-color', 'Red');
        $("#vmWeight").attr("style", "display: block;");
        valFalg = false;
    }
    else {
        document.getElementById("vmWeight").innerHTML = "";
        $("#Weightid").css('border-color', '#ced4da');
        $("#vmWeight").attr("style", "display: none;");

    }
    if (Exchangeid == "") {
        document.getElementById("vmExchange").innerHTML = $("#valueReq").text();
        $("#Exchangeid").css('border-color', 'Red');
        $("#vmExchange").attr("style", "display: block;");
        valFalg = false;
    }
    else {
        document.getElementById("vmExchange").innerHTML = "";
        $("#Exchangeid").css('border-color', '#ced4da');
        $("#vmExchange").attr("style", "display: none;");

    }

    if (valFalg == true) {
        return true;
    }
    else {
        return false;
    }
}
function onchangeGST() {
    debugger;
    //var error_flag = "N";
    //var CheckLength = CheckLengthGStFirstPart();
    //if (CheckLength == false) {
    //    error_flag = "Y";
    //}
    //if (error_flag == "N") {
        var GSTMidPrt = $("#GSTMidPrt").val();
        var GSTLastPrt = $("#GSTLastPrt").val();
        if ((GSTMidPrt == "" && GSTLastPrt == "") || (GSTMidPrt == null && GSTLastPrt == null)) {
            $("#GSTNumber").val("");
    }

        $("#gst_num").css('border-color', '#ced4da');
        $("#vmgst_num").attr("style", "display: none;");
        document.getElementById("vmgst_num").innerHTML = "";
    
        $("#GSTMidPrt").css('border-color', "#ced4da");
        $("#SpanSuppGSTMidPrt").attr("style", "display: none;");
        document.getElementById("SpanSuppGSTMidPrt").innerHTML = "";

        $("#GSTLastPrt").css('border-color', "#ced4da");
        $("#SpanSuppGST").attr("style", "display: none;");
        document.getElementById("SpanSuppGST").innerHTML = "";
    //}

    //if (error_flag == "Y") {
    //    return false;
    //}
    //else {
    //    return true;
    //}
    //var GSTMidPrt = $("#GSTMidPrt").val();
    //var GSTLastPrt = $("#GSTLastPrt").val();
    //if ((GSTMidPrt == "" && GSTLastPrt == "" )|| (GSTMidPrt == null && GSTLastPrt == null)) {
    //    $("#GSTNumber").val("");
    //}
    //$("#GSTMidPrt").css('border-color', "#ced4da");
    //$("#SpanSuppGSTMidPrt").attr("style", "display: none;");
    //document.getElementById("SpanSuppGSTMidPrt").innerHTML = "";

    //$("#GSTLastPrt").css('border-color', "#ced4da");
    //$("#SpanSuppGST").attr("style", "display: none;");
    //document.getElementById("SpanSuppGST").innerHTML = "";

    
}
function ValidateAlpha(evt) {
    var keyCode = (evt.which) ? evt.which : evt.keyCode
    if ((keyCode < 65 || keyCode > 90) && (keyCode < 97 || keyCode > 123) && keyCode != 32)

        return false;
    return true;
}
function ValidateDigitOnly(evt) {
    var keyCode = (evt.which) ? evt.which : evt.keyCode;

    // Allow only digits (keyCode 48 to 57) and disallow everything else
    if (keyCode < 48 || keyCode > 57) {
        return false;
    }
}
function onchangeEntityName() {
    //$("#vmentity").css("display", "none");
    //$("#EntityName").css("border-color", "#ced4da");
    debugger;

    var error_flag = "N";
    var EntityName = checkdependcyEntityName();
    
    if (EntityName == false && $("#ValidEntity_Name").val() == "Y") {
        document.getElementById("vmentity").innerHTML = $("#valueduplicate").text();
        $("#EntityName").css("border-color", "red");
        $("#vmentity").css("display", "block");
        error_flag = "Y";
        // return false;
    }
    else {
        $("#vmentity").css("display", "none");
        $("#EntityName").css("border-color", "#ced4da");
    }
    if (error_flag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function checkdependcyEntityName() {
    debugger;
    var Branchchk = "";
    var error_flag = "N";
    var RoleHoName = "";
    if ($("#Branchchk").is(":checked")) {
        Branchchk = "B";
        RoleHoName = $("#RoleHoName").val();
        if ($("#RoleHoName").val() == "0") {
            $('#vmRoleHoName').text($("#valueReq").text());
            $("#vmRoleHoName").css("display", "block");
            $("#RoleHoName").css("border-color", "Red");
            $("[aria-labelledby='select2-RoleHoName-container']").css("border-color", "Red");
            error_flag = "Y";
        }
        else {
            $("#vmRoleHoName").css("display", "none");
            $("#RoleHoName").css("border-color", "#ced4da");
            $("[aria-labelledby='select2-RoleHoName-container']").css("border-color", "#ced4da");
            //$('#Branchchk').empty();   
        }
    }
    if ($("#HeadOfficechk").is(":checked")) {
        Branchchk = "H";
        RoleHoName = "0";
    }
   
    var flag = "EntityName";
    var Entityname = $("#EntityName").val().trim();
    var comp_id = $("#com_id").val();
    if (error_flag == "N") {
        $.ajax({
            type: "POST",
            url: "/OrganizationSetup/CheckDependcyEntityName",
            //contentType: "application/json; charset=utf-8",
            dataType: "json",

            async: false,
            data: {
                Entityname: Entityname,
                flag: flag,
                comp_id: comp_id,
                RoleHoName: RoleHoName,
                Branchchk: Branchchk
            },

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
                        if (arr.Table[0].checkDependcy == "Y") {
                            document.getElementById("vmentity").innerHTML = $("#valueduplicate").text();
                            $("#EntityName").css("border-color", "red");
                            $("#vmentity").css("display", "block");
                            error_flag = "Y";
                            $("#ValidEntity_Name").val("Y");
                            /*return false;*/
                        }
                        else {
                            document.getElementById("vmentity").innerHTML = "";
                            $("#vmentity").css("display", "none");
                            $("#EntityName").css("border-color", "#ced4da");
                            $("#ValidEntity_Name").val("N");
                        }
                    }
                }
                if (error_flag == "Y") {
                    return false;

                }
                else {
                    return true;
                }
            }

        })
    }
   
    if (error_flag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function onclickEntityName() {
    document.getElementById("vmentity").innerHTML = "";
    $("#vmentity").css("display", "none");
    $("#EntityName").css("border-color", "#ced4da");
}
function onclickHO() {
    $("#vmRoleHoName").css("display", "none");
    $("#RoleHoName").css("border-color", "#ced4da");
    $("[aria-labelledby='select2-RoleHoName-container']").css("border-color", "#ced4da");
    $('#suppcurr').empty();
}
function onclickEntityPrefix() {
    $("#vmEntityPrefix").css("display", "none");
    $("#Entityprefix").css("border-color", "#ced4da");
}
function CheckLengthInput() {
    var error_flag = "N";
    const input = document.getElementById("Entityprefix");   
    const value = input.value.trim();
    const messageText = $("#onlytwonoAllowed").text();
    if (value.length < 2 || value.length > 2) {
        document.getElementById("vmEntityPrefix").innerHTML = messageText;
        $("#Entityprefix").css("border-color", "red");
        $("#vmEntityPrefix").css("display", "block");
        error_flag = "Y";
    }
    else {
        document.getElementById("vmEntityPrefix").innerHTML = "";
        $("#vmEntityPrefix").css("display", "none");
        $("#Entityprefix").css("border-color", "#ced4da");
    }
    if (error_flag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function onchangeEntityPerfix() {
    debugger;
    var error_flag = "N";
    var CheckLength = CheckLengthInput();
    if (CheckLength == false) {
        error_flag = "Y";
    }
    if (error_flag == "N") {
        var Entity_prefix = checkdependcyEntity();
        if (Entity_prefix == false) {
            document.getElementById("vmEntityPrefix").innerHTML = $("#valueduplicate").text();
            $("#Entityprefix").css("border-color", "red");
            $("#vmEntityPrefix").css("display", "block");
            error_flag = "Y";
            // return false;
        }
        else {
            $("#vmEntityPrefix").css("display", "none");
            $("#Entityprefix").css("border-color", "#ced4da");
        }
    }
    
    if (error_flag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function checkdependcyEntity() {
    var Branchchk = "";
    var RoleHoName = "";
    if ($("#Branchchk").is(":checked")) {
        Branchchk = "B";
        RoleHoName = $("#RoleHoName").val();
    }
    if ($("#HeadOfficechk").is(":checked")) {
        Branchchk = "H";
        RoleHoName = "0";
    }
  

    var error_flag = "N";
    var flag = "Entityprefix";
    var comp_id = $("#com_id").val();
    var Entityprefix = $("#Entityprefix").val().trim();
    $.ajax({
        type: "POST",
        url: "/OrganizationSetup/CheckDependcy",
        //contentType: "application/json; charset=utf-8",
        dataType: "json",

        //async: true,
        data: {
            Entityprefix: Entityprefix, flag: flag, comp_id: comp_id, RoleHoName: RoleHoName,
            Branchchk: Branchchk },

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
                    if (arr.Table[0].checkDependcy == "Y") {
                        document.getElementById("vmEntityPrefix").innerHTML = $("#valueduplicate").text();
                        $("#Entityprefix").css("border-color", "red");
                        $("#vmEntityPrefix").css("display", "block");
                        error_flag = "Y";
                        $("#ValidEntity_prefix").val("Y");
                        /*return false;*/
                    }
                    else {
                        document.getElementById("vmEntityPrefix").innerHTML = "";
                        $("#vmEntityPrefix").css("display", "none");
                        $("#Entityprefix").css("border-color", "#ced4da");
                        $("#ValidEntity_prefix").val("N");
                    }
                }
            }
            if (error_flag == "Y") {
                return false;

            }
            else {
                return true;
            }
        }
          
    })
    if (error_flag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function OnclickDBFromDt() {
    $("#vmFY_StartDate").css("display", "none");
    $("#ddlStartDate").css("border-color", "#ced4da");
}
function OnclickDBendDte() {
    $("#vmFY_EndDate").css("display", "none");
    $("#FY_EndDate").css("border-color", "#ced4da");
}
function onclickdef_lang() {
    $("#vmdef_lang").css("display", "none");
    $("#def_lang").css("border-color", "#ced4da");
    $("[aria-labelledby='select2-def_lang-container']").css("border-color", "#ced4da");
}
function onChangecurrency() {
    $("#vmCurrname").css("display", "none");
    $("#CurrencyName").css("border-color", "#ced4da");
    $("[aria-labelledby='select2-CurrencyName-container']").css("border-color", "#ced4da");
}
function onclickHO() {
    $("#vmRoleHoName").css("display", "none");
    $("#RoleHoName").css("border-color", "#ced4da");
    $("[aria-labelledby='select2-RoleHoName-container']").css("border-color", "#ced4da");
    $('#suppcurr').empty();
}
function onkeypressbankbenef() {
    $("#vmbank_benef").css("display", "none");
    $("#bank_benef").css("border-color", "#ced4da");
    $("#vmbank_benef").css("display", "none");
   
}
function onchangepannum() {
    $("#vmPANNumber").css("display", "none");
    $("#pannumber").css("border-color", "#ced4da");
    $("#vmPANNumber").css("display", "none");
   
}
function onkeypressbankname() {
    $("#vmbank_name").css("display", "none");
    $("#bank_name").css("border-color", "#ced4da");
    $("#vmbank_name").css("display", "none");
   
}
function onkeypressAddress() {
    $("#vmbank_add").css("display", "none");
    $("#bank_add").css("border-color", "#ced4da");
    $("#vmbank_add").css("display", "none");
   
}
function onkeypressAcco() {
    $("#vmbank_acc_no").css("display", "none");
    $("#bank_acc_no").css("border-color", "#ced4da");
    $("#vmbank_acc_no").css("display", "none");
   
}
function onkeypressshiftcode() {
    $("#vmswift_code").css("display", "none");
    $("#swift_code").css("border-color", "#ced4da");
    $("#vmswift_code").css("display", "none");
   
}
function onkeypressifsccode() {
    $("#vmifsc_code").css("display", "none");
    $("#ifsc_code").css("border-color", "#ced4da");
    $("#vmifsc_code").css("display", "none");

}

function onchangeWeight() {
    $("#vmWeight").css("display", "none");
    $("#Weightid").css("border-color", "#ced4da");
    $("#vmWeight").css("display", "none");

}
function onchangeExchange() {
    $("#vmExchange").css("display", "none");
    $("#Exchangeid").css("border-color", "#ced4da");
    $("#vmExchange").css("display", "none");

}

function RqtyFloatValueonly(el, evt) {
    /*   function RqtyFloatValueonly(el, evt) {*/
    debugger;
        if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
            return false;
    }
        else {
            
            $("#vmQuantity").css("display", "none");
            $("#Quantity").css("border-color", "#ced4da");
            $("#vmQuantity").css("display", "none");
            return true;

        }
   
   
}

function onchagevalue(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    else {
        $("#vmQuantity_Value").css("display", "none");
        $("#Quantity_Value").css("border-color", "#ced4da");
        $("#vmQuantity_Value").css("display", "none");
        return true;
    }
   
}
function rate(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    else {
        $("#vmRate").css("display", "none");
        $("#Rate1").css("border-color", "#ced4da");
        $("#vmRate").css("display", "none");
        return true;
    }
   
}
function onchangeCustAddr() {
    var customer_address = $("#customer_address").val();
        document.getElementById("SpancustAddr").innerHTML = "";
        $("#customer_address").css('border-color', '#ced4da');
        $("#SpancustAddr").attr("style", "display: none;");  
}

/*Document Setup Start*/
function CheckValidation()
{
    debugger;
    var valFalg = true;
    var Doc_name = $("#Doc_name").val();

    if ( Doc_name =="0") {
      

        document.getElementById("vmDoc_name").innerHTML = $("#valueReq").text();
        $("#Doc_name").css('border-color', 'red');
        $("#vmDoc_name").attr("style", "display: block;");
        valFalg == false;
    }
    else {
        $("[aria-labelledby='select2-Doc_name-container']").css("border-color", "#ced4da");
        $("#Doc_name").css('border-color', '#ced4da');
        $("#vmDoc_name").attr("style", "display: none;");
    }
    if (valFalg == true) {
        return true;
    }
    else {
        return false;
    }
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
function onclickEmail() {
    document.getElementById("vmEmail").innerHTML = "";
    $("#Email").css("border-color", "#ced4da");
}

function ValidateEmail1() {
    debugger;
    /*var validFlag = "N";*/
    var Email = $("#Email1").val();

    var mailformat = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    //mailformat.test(Email);
    if (Email != "") {
        if (mailformat.test(Email)) {
            document.getElementById("vmEmail1").innerHTML = "";
            $("#Email1").css("border-color", "#ced4da");
            return true;
        }
        else {
            document.getElementById("vmEmail1").innerHTML = $("#InvalidEmail").text();
            $("#Email1").css("border-color", "red");
            return false;
        }
    }
    else {
        document.getElementById("vmEmail1").innerHTML = "";
        $("#Email1").css("border-color", "#ced4da");
    }

}
function onclickEmail1() {
    document.getElementById("vmEmail1").innerHTML = "";
    $("#Email1").css("border-color", "#ced4da");
}
function OnchangeHOGetRole() { // add this function Nitesh 21-10-2023 1426 for headoffice onchnge
    debugger;
    var Entity_name = $("#EntityName").val();
    if (Entity_name != "" && Entity_name != null) {
        onchangeEntityName();
    }
    var Headoffice_id = $("#RoleHoName").val();

    $.ajax({
        type: "POST",
        url: "/OrganizationSetup/GetFinst_dt_End_dt_bs_curr",
        //contentType: "application/json; charset=utf-8",
        dataType: "json",

        //async: true,
        data: { Headoffice_id: Headoffice_id, },

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
                    $("#ddlStartDate").val(arr.Table[0].startdate);
                    $("#FY_EndDate").val(arr.Table[0].enddate);
                    $("#curr_id").val(arr.Table[0].bs_curr_id);
                    $("#CurrencyName").val(arr.Table[0].bs_curr_id);
                    $("#currency_name").text(arr.Table[0].bs_curr_nm);

                    $("#vmCurrname").css("display", "none");
                    $("#CurrencyName").css("border-color", "#ced4da");
                    $("[aria-labelledby='select2-CurrencyName-container']").css("border-color", "#ced4da");

                    $("#vmFY_EndDate").css("display", "none");
                    $("#FY_EndDate").css("border-color", "#ced4da");

                    $("#vmFY_StartDate").css("display", "none");
                    $("#ddlStartDate").css("border-color", "#ced4da");

                    $("#vmCurrname").css("display", "none");
                    $("#CurrencyName").css("border-color", "#ced4da");
                    $("[aria-labelledby='select2-CurrencyName-container']").css("border-color", "#ced4da");
                }
            }
        },
        error: function (Data) {
        }
    });
}

/*Code by Hina on 12-01-2024 to Bind all data in dropdown and also OnChange COUNTRY,state,district,city */
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
        url: "/BusinessLayer/OrganizationSetup/GetCountryonChngPros",
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
    var CustomerCountry = $("#OrgCountry").val();
    var pannumber = $("#pannumber").val();
    if (CustomerCountry == "101") {
        
        $("#GSTNumrequired").css("display", "inherit");
        $("#pannumberrequired").css("display", "inherit");
    }
    else {
        
        $("#GSTNumrequired").attr("style", "display: none;");
        $("#pannumberrequired").attr("style", "display: none;");
        //$("#GSTMidPrt").prop("disabled", true);
        //$("#GSTLastPrt").prop("disabled", true);
    }
    document.getElementById("vmOrgCountryname").innerHTML = "";
    $("[aria-labelledby='select2-OrgCountry-container']").css("border-color", "#ced4da");
    /*Commenetd and modify by Hina Sharma on 04-08-2025*/
    //$('#OrgState').empty();
    //$('#OrgState').append(`<option value="0">---Select---</option>`);
    //$('#OrgDistrict').empty();
    //$('#OrgDistrict').append(`<option value="0">---Select---</option>`);
    //$('#OrgCity').empty();
    //$('#OrgCity').append(`<option value="0">---Select---</option>`);
    //$("#OrgPin").val("");
    //$("#pannumber").val("");
    //$("#gst_num").val("");
    //$("#GSTMidPrt").val("");
    //$("#GSTLastPrt").val("");
    //$("#GSTNumber").val("");
    //$("#MSME_Number").val("");
    //if (pannumber == "") {
    //    pannumber = "";
    //}
    GetStateByCountry();
}
function GetStateByCountry() {
    debugger;
    //if ($("#Overseas").is(":checked")) {
    //    ProsType = "E";
    //    $("#HdnProspectType").val(ProsType);
    //}
    //if ($("#Domestic").is(":checked")) {
    //    ProsType = "D";
    //    $("#HdnProspectType").val(ProsType);
    //}
    var ddlCountryID = $("#OrgCountry").val();

    $.ajax({
        type: "POST",
        url: "/OrganizationSetup/GetstateOnCountry",/*Controller=SupplierSetup and Fuction=Getsuppport*/
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
                    $('#OrgState').empty();
                    $('#OrgState').append(`<option value="0">---Select---</option>`);
                    for (var i = 0; i < arr.length; i++) {

                        $('#OrgState').append(`<option value="${arr[i].state_id}">${arr[i].state_name}</option>`);
                    }
                    $("#OrgState").select2();/*For making Serachable Dropdown */
                    var hdnSTATE = $("#HdnState").val()
                    if (hdnSTATE == "") {
                        hdnSTATE = "0";
                    }
                    $("#OrgState").val(hdnSTATE).trigger("change");
                    //$("#HdnState").val("0");
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
    //GstStateCodeOnChangeStatus();/*COMMENETD BY HINA SHARMA ON 06-08-2025*/
    $('#vmOrgStatename').text("");
    $("#vmOrgStatename").css("display", "none");
    $("[aria-labelledby='select2-OrgState-container']").css('border-color', "#ced4da");
    $("#OrgState").css('border-color', "#ced4da");

}
function GetDistrictByState() {
    debugger;

    var ddlStateID = $("#OrgState").val();

    $.ajax({
        type: "POST",
        url: "/OrganizationSetup/GetDistrictOnState",/*Controller=SupplierSetup and Fuction=Getsuppport*/
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
                    $('#OrgDistrict').empty();
                    for (var i = 0; i < arr.length; i++) {
                        $('#OrgDistrict').append(`<option value="${arr[i].district_id}">${arr[i].district_name}</option>`);
                    }
                    $("#OrgDistrict").select2();/*For making Serachable Dropdown */
                    /*---------------Add by Hina on 04-08-2025------------ */
                    var hdnDistrct = $("#HdnDistrict").val();
                    var hdn_state = $("#HdnState").val();
                    var Org_State = $("#OrgState").val();
                    if (hdn_state == Org_State) {
                        if (hdnDistrct != "" && hdnDistrct != null) {
                            $("#OrgDistrict").val(hdnDistrct).trigger("change");
                        }
                    }
                }
            }
        },
        error: function (Data) {
        }
    });
    /*commented by Hina on 04-08-2025 to bind data in table*/
    //$('#OrgCity').empty();
    //$('#OrgCity').append(`<option value="0">---Select---</option>`);

};
function OnChangeDistrict() {
    debugger;
    GetCityByDistrict();
    $('#vmOrgDistname').text("");
    $("#vmOrgDistname").css("display", "none");
    //$("#CmnDistrict").css("border-color", "red");
    $("[aria-labelledby='select2-OrgDistrict-container']").css('border-color', "#ced4da");

}
function GstStateCodeOnChangeStatus() {
    debugger

    var stateCode = $('#OrgState').val();
    $.ajax({
        type: "POST",
        url: "/OrganizationSetup/GetStateCode",/*Controller=SupplierSetup and Fuction=Getsuppport*/
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

    var ddlDistrictID = $("#OrgDistrict").val();

    $.ajax({
        type: "POST",
        url: "/OrganizationSetup/GetCityOnDistrict",/*Controller=SupplierSetup and Fuction=Getsuppport*/
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
                    $('#OrgCity').empty();
                    for (var i = 0; i < arr.length; i++) {
                        $('#OrgCity').append(`<option value="${arr[i].city_id}">${arr[i].city_name}</option>`);
                    }
                    $("#OrgCity").select2();/*For making Serachable Dropdown */

                    /*---------------Add by Hina on 04-08-2025------------ */
                    var hdnCity = $("#HdnCity").val();
                    var hdn_state = $("#HdnState").val();
                    var Org_State = $("#OrgState").val();
                    if (hdn_state == Org_State) {
                        if (hdnCity != null && hdnCity != "") {
                            $("#OrgCity").val(hdnCity).trigger("change");
                        }
                    }
                }


            }
        },
        error: function (Data) {
        }
    });
};
function OnChangeCity() {
    debugger;
    $('#vmOrgCityname').text("");
    $("#vmOrgCityname").css("display", "None");
    $("[aria-labelledby='select2-OrgCity-container']").css('border-color', "#ced4da");


}

/*------------Code start Add by Hina Sharma on 02-08-2025 to bind all address table data-----------------*/

function CheckDuplicacyOnAddOrg() {
    debugger;
    var Comp_address = $("#customer_address").val();
    var Comp_Pin = $("#OrgPin").val();
    var CompCity_Name = $("#OrgCity").val();

    var validflg = true;

    $("#tblCompanyAddressDetail TBODY TR").each(function () {
        var currentRow = $(this).closest('tr')
        debugger;

        var addressCOMP = currentRow.find("#tblcompadd").text()
        var supp_pin = currentRow.find("#tblpin").text()
        var CityCOMP = currentRow.find("#hdntblcityid").text()
        var checkID = currentRow.find("#hdn_id").text();
        if (checkID != "Delete") {
            if (Comp_address.toLowerCase() == addressCOMP.toLowerCase() && CompCity_Name == CityCOMP) {

               document.getElementById("SpancustAddr").innerHTML = $("#valueduplicate").text();
               $("#customer_address").css('border-color', 'red');
               $("#SpancustAddr").attr("style", "display: block;");
                
               document.getElementById("vmOrgCityname").innerHTML = $("#valueduplicate").text();
               $("[aria-labelledby='select2-OrgCity-container']").css('border-color', 'red');
               $("#vmOrgCityname").attr("style", "display: block;");
                validflg = false;
            }
        }

    });
    if (validflg == true) {

    }
    else {
        return false;
    }

}
function CheckDuplicateOnUpdateOrg() {
    debugger;
    var Comp_address = $("#customer_address").val();
    var Comp_Pin = $("#OrgPin").val();
    var CompCity_Name = $("#OrgCity").val();
    var SRowidUpdt = $("#hdnSrRowId").val();

    validflg = true;
    $("#tblCompanyAddressDetail TBODY TR").each(function () {
        currentRow = $(this);
        rowId = $(this).find("#spanCompDefrowId").text();
        if (rowId > 0) {
            if (SRowidUpdt != rowId) {
                var addressCOMP = currentRow.find("#tblcompadd").text()
                var supp_pin = currentRow.find("#tblpin").text()
                var CityCOMP = currentRow.find("#hdntblcityid").text()
                var checkID = currentRow.find("#hdn_id").text();
                if (checkID != "Delete") {
                    
                    if (Comp_address.toLowerCase() == addressCOMP.toLowerCase() && CompCity_Name == CityCOMP) {

                        document.getElementById("SpancustAddr").innerHTML = $("#valueduplicate").text();
                        $("#customer_address").css('border-color', 'red');
                        $("#SpancustAddr").attr("style", "display: block;");
                       
                        document.getElementById("vmOrgCityname").innerHTML = $("#valueduplicate").text();
                        $("[aria-labelledby='select2-OrgCity-container']").css('border-color', 'red');
                        $("#vmOrgCityname").attr("style", "display: block;");

                        validflg = false;
                    }
                }
            }
        }
    });
    if (validflg == false) {
        return false;
    }
}
function AddAddressdetail() {
    debugger;
    
    var rowIdx = 0;
    debugger;
    //var TblLen = $("#tblCompanyAddressDetail TBODY TR").length;
    var rowId = $("#tblCompanyAddressDetail TBODY TR:last").find("#spanCompDefrowId").text();
    if (rowId == "" || rowId == null) {
        rowIdx = 1;
    }
    else {
        rowIdx = parseInt(rowId) + 1;
    }
    var Org_address = $("#customer_address").val();
    var Org_Pin = $("#OrgPin").val();
    var Org_City = $("#OrgCity").val();
    var GstApplicable = $('#hdnGstApplicable').val();
    var CompDefaultAddress = "";
    var Comp_district = $("#OrgDistrict").val();
    var Comp_state_id = $("#OrgState").val();
    var Comp_country_id = $("#OrgCountry").val();

    if (Check_Com_add_Validations() == false) {
        return false;
    }
    if ($("#chkCompDefaultAddress").is(":checked")) {
        CompDefaultAddress = "Y";
    }
    else {
        CompDefaultAddress = "N";
    }
    var CompDefAddresstd;
    if (CompDefaultAddress == 'Y') {
        CompDefAddresstd = '<td id="defcompadd"><div class="custom-control custom-switch" style = "margin-bottom: 0px; margin-top: -5px; margin-left: 40px;"><input type="checkbox" class="custom-control-input col-md-3 col-sm-12 margin-switch" id="chkCCA_' + rowIdx + '" checked onclick="defcompchk(event)"><label class="custom-control-label col-md-9 col-sm-12" for="chkCCA_' + rowIdx + '" style="padding: 3px 0px;"></label></div></td><td hidden="hidden"><span id="spanhiddenCompDef_' + rowIdx + '">Y</span></td><td class=" " hidden="hidden"><span id="spanCompDefrowId">' + rowIdx + '</span></td>'
    }
    else {
        CompDefAddresstd = '<td id="defcompadd"><div class="custom-control custom-switch" style = "margin-bottom: 0px; margin-top: -5px; margin-left: 40px;"><input type="checkbox" class="custom-control-input col-md-3 col-sm-12 margin-switch" id="chkCCA_' + rowIdx + '" onclick="defcompchk(event)"><label class="custom-control-label col-md-9 col-sm-12" for="chkCCA_' + rowIdx + '" style="padding: 3px 0px;"></label></div></td><td hidden="hidden"><span id="spanhiddenCompDef_' + rowIdx + '">N</span></td><td class=" " hidden="hidden"><span id="spanCompDefrowId">' + rowIdx + '</span></td>'
    }

    var rowId = 0;
    var cheakflag = "N";
    var cheackdefalt = "Q";
    $("#tblCompanyAddressDetail TBODY TR").each(function () {
        var BIllrowId = $(this).find("#spanCompDefrowId").text();
        if ($(this).find("#chkCCA_" + BIllrowId).is(":checked")) {
            cheackdefalt = "P";
        }
    });
    if (cheackdefalt == "Q") {
        $("#tblCompanyAddressDetail TBODY TR").each(function () {

            debugger;
            if ($(this).find("#defcompadd div input").is(":checked"))

                var BIllrowId = $(this).find("#spanCompDefrowId").text();
            $(this).find("#chkCCA_" + BIllrowId).prop("checked", false);
            $(this).find("#spanhiddenCompDef_" + BIllrowId).text("Y");
            cheakflag = "Y";
        });
        if (cheakflag == "Y") {
            var rowId = 0;
            $("#tblCompanyAddressDetail TBODY TR").each(function () {
                debugger;
                rowId = rowId + 1;
                if (rowId == 1) {
                    var BIllrowId = $(this).find("#spanCompDefrowId").text();
                    $(this).find("#chkCCA_" + BIllrowId).prop("checked", true);
                    $(this).find("#spanhiddenCompDef_" + BIllrowId).text("Y");
                }
                return false
            });
        }
    }
    if (Org_address != "" && Org_City !== "0") {
       
        debugger;
        if (CheckDuplicacyOnAddOrg() == false) {
            return false;
        }
        if (CompDefaultAddress == 'Y') {
            var rowId = 0;
            $("#tblCompanyAddressDetail TBODY TR").each(function () {
                debugger;
                rowId = rowId + 1;

                $(this).find("#defcompadd div input").prop("checked", false);
                $(this).find("#spanhiddenCompDef_" + rowId).text("N");
            });
        }
        //(async function () {
        //    const isGstValid = await CheckDuplicateGstNoAsync();
        //    if (!isGstValid) {
        //        return false;
        //    }
        //    else {

                $('#CompAddressBody').append(`<tr id="R${++rowIdx}"> 
        <td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
        <td class="center edit_icon"> <i class="fa fa-edit" aria-hidden="true" id="editBtnIcon" title="${$("#Edit").text()}"></i></td>
        <td id="tblcompadd">${$("#customer_address").val()}</td>
        <td hidden="hidden" id="comp_address_id"></td>
        <td id="tblcontryname">${$("#OrgCountry option:selected").text()}</td>
        <td hidden="hidden" id="hdntblcntryid">${$("#OrgCountry").val()}</td>
        <td id="tblstatename">${$("#OrgState option:selected").text()}</td>
        <td hidden="hidden" id="hdntblstateid">${$("#OrgState").val()}</td>
        <td id="tbldistname">${$("#OrgDistrict option:selected").text()}</td>
        <td hidden="hidden" id="hdntbldistid">${$("#OrgDistrict").val()}</td>
        <td id="tblcityname">${$("#OrgCity option:selected").text()}</td>
        <td hidden="hidden" id="hdntblcityid">${$("#OrgCity").val()}</td>
        <td id="tblpin">${$("#OrgPin").val()}</td>
        
       
         ${CompDefAddresstd}
          <td hidden="hidden" id="address_id">${++rowIdx}</td>
          <td hidden="hidden" id="hdn_id">Save</td>
        
        </tr>`);
               
                $("#HdnCountry").val(0);
                $("#HdnState").val(0);
                $("#HdnDistrict").val(0);
                $("#HdnCity").val(0);
                var Supp_Country = $("#OrgCountry").val();
                $("#customer_address").val("");
                $("#OrgPin").val("");
                $("#OrgCountry").val("0").change();
                $("#OrgState").val("0").change();
                $("#OrgDistrict").val("0").trigger("change");
                $("#OrgCity").val("0").change();
                $("#chkCompDefaultAddress").prop("checked", true);
                

                

        //    }
        //})();


    }

}
async function CheckDuplicateGstNoAsync() {
    var GSTNum = "";

    var SuppGST = $("#gst_num").val().trim();
    var GSTMidPrt = $("#GSTMidPrt").val().trim();
    var GSTLastPrt = $("#GSTLastPrt").val().trim();
    if (GSTMidPrt != "" || GSTLastPrt != "") {
        GSTNum = SuppGST + GSTMidPrt + GSTLastPrt

    }
    else {
        if (GSTMidPrt == "" || GSTLastPrt == "") {
            GSTNum = GSTMidPrt + GSTLastPrt

        }
    }
    if (GSTNum != "") {
        const Supp_Id = $("#Supp_Id").val();
        const SupplierGst = GSTNum;

        const response = await $.ajax({
            type: "POST",
            url: "/BusinessLayer/SupplierDetail/CheckDependcyGstno",
            data: { Supp_Id: Supp_Id, SupplierGst: SupplierGst }
        });

        const arr = JSON.parse(response);
        if (arr.length > 0 && arr[0].Dependcy === "Use") {
            $("#SpanSuppGSTMidPrt").text($("#valueduplicate").text()).show();
            $("#GSTMidPrt, #GSTLastPrt").css("border-color", "red");
            $("#GSTMidPrt, #GSTLastPrt").css("border-color", "red");
            return false;
        } else {
            $("#SpanSuppGSTMidPrt").hide().text("");
            $("#GSTMidPrt, #GSTLastPrt").css("border-color", "#ced4da");
            return true;
        }
    }
    else {
        return true;
    }

}

function ClearOrgAddressDetail() {
    debugger;
    $("#customer_address").val("");
    $("#OrgCountry").val("0").change();
    $("#OrgCountry").val(0);
    $("#HdnContry").val("0");
    $('#OrgState').empty();
    $('#OrgState').append(`<option value="0">---Select---</option>`);
    $('#OrgDistrict').empty();
    $('#OrgDistrict').append(`<option value="0">---Select---</option>`);
    $('#OrgCity').empty();
    $('#OrgCity').append(`<option value="0">---Select---</option>`);
    $("#OrgPin").val("");
    
    $("#divUpdate").css("display", "none");
    $('#divAddAddrDtl').css("display", "Block");
    $("#chkCompDefaultAddress").prop("checked", true);
    document.getElementById("SpancustAddr").innerHTML = "";
    $("#customer_address").css('border-color', '#ced4da');
    $("#SpancustAddr").attr("style", "display: none;");
    document.getElementById("vmOrgPin").innerHTML = "";
    $("#OrgPin").css('border-color', '#ced4da');
    $("#vmOrgPin").attr("style", "display: none;");
    document.getElementById("vmOrgCityname").innerHTML = "";
    $("[aria-labelledby='select2-Orgcity-container']").css('border-color', '#ced4da');
    $("#vmOrgCityname").attr("style", "display: none;");
}
function OnClickAddressUpdateBtn() {
    debugger;
    //if (CheckOS_Com_add_Validations() == false) {/*commenetd and modify by hina sharma on 05-08-2025*/
    //    return false;
    //}
    if (Check_Com_add_Validations() == false) {
        return false;
    }
    if (CheckDuplicateOnUpdateOrg() == false) {
        return false;
    }
    //(async function () {
    //    const isGstValid = await CheckDuplicateGstNoAsync();
    //    if (!isGstValid) {
    //        return false;
    //    }

    //    else {
            debugger;
    var CompDefaultAddress = "";

    if ($("#chkCompDefaultAddress").is(":checked")) {
        CompDefaultAddress = "Y";
            }
            else {
        CompDefaultAddress = "N";
            }

            debugger;
    if (CompDefaultAddress == 'Y') {
                var rowId = 0;
                $("#tblCompanyAddressDetail TBODY TR").each(function () {
                    debugger;
                    rowId = rowId + 1;

                    $(this).find("#defcompadd div input").prop("checked", false);
                    $(this).find("#spanhiddenCompDef_" + rowId).text("N");
                    
                });
            }
            debugger;
            var addr_idtoUpdt = $("#UpdtAddrId").val();
            
            var CustCountry = $("#OrgCountry").val()
            $('#tblCompanyAddressDetail >tbody >tr').each(function () {
                var currentRow = $(this);
                debugger;


                var address_id = currentRow.find('#address_id').text();
                if (addr_idtoUpdt == address_id) {
                    currentRow.find("#tblcompadd").text($("#customer_address").val());
                    currentRow.find("#tblcontryname").text($("#OrgCountry option:selected").text());
                    currentRow.find("#hdntblcntryid").text($("#OrgCountry").val());
                    currentRow.find("#tblstatename").text($("#OrgState option:selected").text());
                    currentRow.find("#hdntblstateid").text($("#OrgState").val());
                    currentRow.find("#tbldistname").text($("#OrgDistrict option:selected").text());
                    currentRow.find("#hdntbldistid").text($("#OrgDistrict").val());
                    currentRow.find("#tblcityname").text($("#OrgCity option:selected").text());
                    currentRow.find("#hdntblcityid").text($("#OrgCity").val());
                    currentRow.find("#tblpin").text($("#OrgPin").val());
                  
                    if (CompDefaultAddress == "Y") {
                        var BIllrowId = $(this).find("#spanCompDefrowId").text();
                        currentRow.find("#chkCCA_" + BIllrowId).prop("checked", true);
                        $("#spanhiddenCompDef_" + BIllrowId).text("Y");
                    }
                    else {
                        var BIllrowId = $(this).find("#spanCompDefrowId").text();
                        currentRow.find("#chkCCA_" + BIllrowId).prop("checked", false);
                        $("#spanhiddenCompDef_" + BIllrowId).text("N");
                    }
                    if (CompDefaultAddress == "Y") {

                    }
                    else {
                        var rowId = 0;
                        var cheakflag = "N";
                        var cheackdefalt = "Q";
                        $("#tblCompanyAddressDetail TBODY TR").each(function () {
                            var BIllrowId = $(this).find("#spanCompDefrowId").text();
                            if ($(this).find("#chkCCA_" + BIllrowId).is(":checked")) {
                                cheackdefalt = "P";
                            }
                        });
                        if (cheackdefalt == "Q") {
                            debugger;
                            var rowIdx1 = 1;
                            var firstactive = "N";
                            $("#tblCompanyAddressDetail TBODY TR").each(function () {
                                debugger;
                                var currentRow = $(this);
                                if (firstactive == "N") {
                                    var hdn_id = currentRow.find("#hdn_id").text();
                                    if (hdn_id != "Delete") {
                                        currentRow.find("#defcompadd div input").prop("checked", true);
                                        $("#spanhiddenCompDef_" + rowIdx1).text("Y");
                                        firstactive = "Y";
                                        return false;
                                    }

                                }
                            });
                        }
                    }
                }

                debugger;

                $("#HdnCountry").val(0);
                $("#HdnState").val(0);
                $("#HdnDistrict").val(0);
                $("#HdnCity").val(0);
            });

            document.getElementById("SpancustAddr").innerHTML = "";
            $("#customer_address").css('border-color', '#ced4da');
            $("#SpancustAddr").attr("style", "display: none;");
            document.getElementById("vmOrgCityname").innerHTML = "";
            $("#OrgCity").css('border-color', '#ced4da');
            $("#vmOrgCityname").attr("style", "display: none;");
            $("#customer_address").val("");
            $("#OrgPin").val("");
            $("#OrgCountry").val("0").change();
    $("#OrgState").val("0");
    $('#OrgState').empty();
    $('#OrgState').append(`<option value="0">---Select---</option>`);
            $('#OrgDistrict').empty();
            $('#OrgDistrict').append(`<option value="0">---Select---</option>`);
            $('#OrgCity').empty();
            $('#OrgCity').append(`<option value="0">---Select---</option>`);
           
            $("#divUpdate").css("display", "none");
            $('#divAddAddrDtl').css("display", "Block");
            $("#chkCompDefaultAddress").prop("checked", true);
            

    //    }
    //})();


}
function defcompchk(e) {
    debugger;
    var rowIdx = "";
    try {
        var clickedrow = $(e.target).closest("tr");
        var BillRowID = clickedrow.find("#spanCompDefrowId").text();
        //var BillRowID = clickedrow.find("#spanhiddenBill_" + rowIdx).text();
        if (clickedrow.find("#chkCCA_" + BillRowID).is(":checked")) {
            $("#tblCompanyAddressDetail TBODY TR").each(function () {
                debugger;
                rowIdx = $(this).find("#spanCompDefrowId").text();

                $(this).find("#chkCCA_" + rowIdx).prop("checked", false);
                $(this).find("#spanhiddenCompDef_" + rowIdx).text("N");
            });
            clickedrow.find("#chkCCA_" + BillRowID).prop("checked", true);
            clickedrow.find("#spanhiddenCompDef_" + BillRowID).text("Y");
            ReadCOMPAddress();
        }
        else {
            clickedrow.find("#chkCCA_" + BillRowID).prop("checked", false);
            clickedrow.find("#spanhiddenCompDef_" + BillRowID).text("N");
            ReadCOMPAddress();
        }
        
    }
    catch (err) {

    }
}
function ReadCOMPAddress() {
    debugger;
    var AddressList = new Array();
    var rowNum = 0;
    $("#tblCompanyAddressDetail TBODY TR").each(function () {
        debugger;
        var row = $(this);
        var List = {};
        rowNum = rowNum + 1
        
        List.address = row.find("#tblcompadd").text();
        List.comp_add_id = row.find("#comp_address_id").text();
        List.City = row.find("#hdntblcityid").text();
        List.CityName = row.find("#tblcityname").text();
        List.District = row.find("#hdntbldistid").text();
        List.DistrictName = row.find("#tbldistname").text();
        List.State = row.find("#hdntblstateid").text();
        List.StateName = row.find("#tblstatename").text();
        List.Country = row.find("#hdntblcntryid").text();
        List.CountryName = row.find("#tblcontryname").text();
        //Branch.GSTNo = row.find("#supgstno").text();
        List.DefAddress = row.find("#spanhiddenCompDef_" + rowNum).text();
        List.pin = row.find("#tblpin").text();
        List.address_id = row.find("#address_id").text();
        List.Flag = row.find("#hdn_id").text();
        AddressList.push(List);
    });
    var str = JSON.stringify(AddressList);
    $('#hdOrgAddressList').val(str);
    
}
function onchangePin() {
    debugger;
        var customer_Pin = $("#OrgPin").val();
        if (customer_Pin == "") {
            document.getElementById("vmOrgPin").innerHTML = $("#valueReq").text();
            $("#OrgPin").css('border-color', 'red');
            $("#vmOrgPin").attr("style", "display: block;");
        }
        else {

            document.getElementById("vmOrgPin").innerHTML = "";
            $("#OrgPin").css('border-color', '#ced4da');
            $("#vmOrgPin").attr("style", "display: none;");
        }
    
    if (document.getElementById("SpancustAddr").innerHTML == $("#valueduplicate").text()) {
        document.getElementById("SpancustAddr").innerHTML = "";
        $("#customer_address").css('border-color', '#ced4da');
        $("#SpancustAddr").attr("style", "display: none;");
        document.getElementById("vmOrgCityname").innerHTML = "";
        $("[aria-labelledby='select2-OrgCity-container']").css('border-color', '#ced4da');
        $("#vmOrgCityname").attr("style", "display: none;");

    }

}
function CheckDefaultaddress() {
    var billing = 'N';
    $("#tblCompanyAddressDetail >tbody >tr").each(function (i, row) {

        var currentRow = $(this);
        var rowId = "";
        //rowIdx = rowIdx + 1;
        var rowIdQ = $(this).find("#spanCompDefrowId").text();

        if ($("#chkCCA_" + rowIdQ).prop("checked") == true) {
            billing = 'Y';
        }
    });
    if (billing == 'N') {
        swal("", $("#AtLeastOneAddressShouldBeSetAsDefault").text(), "warning");
        return false;
    }
    else {
        return true;
    }
}
function FlagAfterDelete(e) {
    debugger;
    var firstactive = "N";
    var rowIdx = 1;
    $("#tblCompanyAddressDetail >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        //var a = currentRow.find("td:eq(16)").text();
        var a = currentRow.find("#address_id").text();
        if (e == a) {
            //currentRow.find("td:eq(17)").text("Delete");
            currentRow.find("#hdn_id").text("Delete");


        }
        if (e == a) {
            debugger;
            //  var firstactive = "N";
            var rowIdx1 = 1;
            var len = $("#tblCompanyAddressDetail >tbody >tr").length;
            $("#tblCompanyAddressDetail >tbody >tr").each(function (i, row) {
                var currentRow = $(this).closest('tr');
                if (firstactive == "N") {
                    if (len > 0) {
                        var billid = $(this).find("#spanCompDefrowId").text();
                        if (currentRow.find("#defcompadd div input").is(":checked")) {
                            firstactive = "Y";

                        }
                    }
                }
            });
            var rowId = 0;
            var rowIdx1 = 0;
            $("#tblCompanyAddressDetail TBODY TR").each(function () {
                debugger;
                rowIdx1 = rowIdx1 + 1;
                var currentRow = $(this);
                if (firstactive == "N") {
                    var hdn_id = currentRow.find("#hdn_id").text();
                    if (hdn_id != "Delete") {
                        currentRow.find("#defcompadd div input").prop("checked", true);
                        $("#spanhiddenCompDef_" + rowIdx1).text("Y");
                        firstactive = "Y";
                        ReadCOMPAddress();
                        return false;
                    }

                }
            });
        }


    });
};
function ChakeDependencyAddr(custId, e, currentrow) {
    var addr_id = e;
    debugger;
    $.ajax({
        type: "POST",
        url: "/SupplierDetail/ChakeDependencyAddr",
        dataType: "json",
        data: {
            addr_id: addr_id,
            custId: custId,
        },/*Registration pass value like model*/
        success: function (data) {
            debugger;
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            var DataT = data;
            if (DataT == "Detail Exists") {
                //Exist = "Yes";
                swal("", $("#DependencyExist").text(), "warning");
            }
            else {
                var rowIdx = currentrow.find("#spanCompDefrowId").text();
                currentrow.find("#defcompadd div input").prop("checked", false);
                currentrow.hide();
                var IdonEdit = $("#hdnSrRowId").val();
                var Supplier_address = $("#customer_address").val();
                var Supplier_Pin = $("#OrgPin").val();
                var SupplierCity_Name = $("#OrgCity").val();
                var addressSup = currentrow.find("#tblcompadd").text()
                var supp_pin = currentrow.find("#tblpin").text()
                var CitySup = currentrow.find("#hdntblcityid").text()

                if (IdonEdit == rowIdx) {
                    //currentrow.hide();
                    ClearOrgAddressDetail();
                }

                FlagAfterDelete(addr_id);
                Exist = "No";
            }

        },
        error: function (Data) {
        }
    });

}
function CheckLengthGStFirstPart() {
    debugger;
    var error_flag = "N";
    const input = document.getElementById("gst_num");
    const value = input.value.trim();
    const messageText = $("#onlytwodigitsAllowed").text();
    if (/*value.length < 2 || value.length > 2*/(value.length < 2 && value.length > 0) || value.length==0) {
        $("#gst_num").css('border-color', 'red');
        $("#SpanSuppGST").attr("style", "display: block;");
        document.getElementById("SpanSuppGST").innerHTML = $("#valueReq").text();
        error_flag = "Y";

        //document.getElementById("vmgst_num").innerHTML = messageText;
        //$("#gst_num").css("border-color", "red");
        //$("#vmgst_num").css("display", "block");
        //error_flag = "Y";
    }
    else {
        $("#gst_num").css('border-color', '#ced4da');
        $("#SpanSuppGST").attr("style", "display: none;");
        document.getElementById("SpanSuppGST").innerHTML = $("#valueReq").text();
        error_flag = "Y";

        //document.getElementById("vmEntityPrefix").innerHTML = "";
        //$("#vmgst_num").css("display", "none");
        //$("#gst_num").css("border-color", "#ced4da");

    }
    if (error_flag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function ValidateGSTNew() {
    //debugger;
    var GST = $("#gst_num").val();
    var GSTNoMidPrt = $("#GSTMidPrt").val().trim();
    var GSTNoLastPrt = $("#GSTLastPrt").val().trim();

    var valFalg = true;
    debugger;
    
    if (
        (GST && GST.length !== 2) ||
        (GSTNoMidPrt && GSTNoMidPrt.length !== 10) ||
        (GSTNoLastPrt && GSTNoLastPrt.length !== 3) ||
        !GST || !GSTNoMidPrt || !GSTNoLastPrt
    ) {
        $("#gst_num, #GSTMidPrt, #GSTLastPrt").css('border-color', 'red');
        $("#SpanSuppGST").show().html($("#InvalidGSTNumber").text());
        valFalg = false;
    } else {
        $("#gst_num, #GSTMidPrt, #GSTLastPrt").css('border-color', '');
        $("#SpanSuppGST").hide().html("");
    }
    if (GST == "" && GSTNoMidPrt == "" && GSTNoLastPrt == "") {
        $("#gst_num, #GSTMidPrt, #GSTLastPrt").css('border-color', '');
        $("#SpanSuppGST").hide().html("");
        var valFalg = true;
    }
    if (valFalg == true)
    {
        if (GSTNoMidPrt != "" || GSTNoLastPrt != "") {
            var CompGSTNum = GST + GSTNoMidPrt + GSTNoLastPrt
            $("#GSTNumber").val(CompGSTNum);
        }
        else {
            if (GSTNoMidPrt == "" || GSTNoLastPrt == "") {
                var CompGSTNum = GSTNoMidPrt + GSTNoLastPrt
                $("#GSTNumber").val(CompGSTNum);
            }
        }
        return true;
    }
    else {
        return false;
    }
}
/*------------Code end Add by Hina Sharma on 02-08-2025 to bind all address table data-----------------*/
/*------------Code start Add by Shubham Maurya on 15-11-2025 to bind all licance table data-----------------*/
function AddLicensedetail() {
    var LicenseName = $("#LicenseName").val();
    var LicenseNumber = $("#LicenseNumber").val();
    var ExpiryDate = $("#ExpiryDate").val();
    //var parts = ExpiryDate.split("-");   // ["2025", "11", "15"]
    //var formattedDate = parts[2] + "-" + parts[1] + "-" + parts[0];
    var formattedDate = "";
    if (ExpiryDate && ExpiryDate.trim() !== "") {
        var parts = ExpiryDate.split("-");
        if (parts.length === 3) {
            formattedDate = parts[2] + "-" + parts[1] + "-" + parts[0];
        }
    }
    var ExpiryAlertDays = $("#ExpiryAlertDays").val();
    var CurrentDate = moment().format('YYYY-MM-DD');

    var rowIdx = 0;
    debugger;
    var rowId = $("#LicenseDetailTble TBODY TR").length;
    if (rowId == "" || rowId == null) {
        rowIdx = 1;
    }
    else {
        rowIdx = parseInt(rowId) + 1;
    }
    if (LicenseName != "" && LicenseName != null) {
        $("#LicenseDetailTble >tbody").append(`<tr id="R${rowIdx}"> 
       <td class=" red center"> <i class=" fa fa-trash" aria-hidden="true" id="DeliveryDelIconBtn" onclick="deleteLicenseDetail(event)" title="${$("#Span_Delete_Title").text()}"></i></td>
                                            <td class="center edit_icon"> <i class="fa fa-edit" aria-hidden="true" onclick="EditLicenseDetail(event)" title="${$("#Edit").text()}"></i></td>
                                            <td id="rowno" >${rowIdx}</td>
                                            <td id="srno"  hidden="hidden">${rowIdx}</td>
                                            <td id="LicenseNm">${LicenseName}</td>
                                            <td id="LicenseNum">${LicenseNumber}</td>
                                            <td id="ExpDate">${formattedDate}</td>
                                            <td id="ExpiryAlrtDays">${ExpiryAlertDays}</td>
        </tr>`);
    }
    else {
        $("#LicenseName").css("border-color", "Red");
        $('#SpanLicenseName').text($("#valueReq").text());
        $("#SpanLicenseName").css("display", "block");
        return false;
    }
    $("#LicenseName").val("");
    $("#LicenseNumber").val("");
    $("#ExpiryDate").val("");
    $("#ExpiryAlertDays").val("");
}
function InsertFinalLicenceDetail() {
    debugger;
    var LicenceList = [];
    $("#LicenseDetailTble >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var l_id = currentRow.find("#srno").text();
        var LicenseNm = currentRow.find("#LicenseNm").text();
        var LicenseNum = currentRow.find("#LicenseNum").text();
        var ExpDate1 = currentRow.find("#ExpDate").text();
        //var parts = ExpDate1.split("-");   // ["2025", "11", "15"]
        //var formattedDate1 = parts[2] + "-" + parts[1] + "-" + parts[0];
        var formattedDate = "";
        if (ExpDate1 && ExpDate1.trim() !== "") {
            var parts = ExpDate1.split("-");
            if (parts.length === 3) {
                formattedDate = parts[2] + "-" + parts[1] + "-" + parts[0];
            }
        }
        var ExpiryAlrtDays = currentRow.find("#ExpiryAlrtDays").text();
        if(ExpiryAlrtDays == ""){
            ExpiryAlrtDays = 0;
        }
        LicenceList.push({
            l_id: l_id, LicenseNm: LicenseNm, LicenseNum: LicenseNum, ExpDate: formattedDate, ExpiryAlrtDays: ExpiryAlrtDays
        });
    });
    return LicenceList;
};
function deleteLicenseDetail(e) {
    debugger;
    var currentRow = $(e.target).closest("tr");
    currentRow.remove();
    SerialNoAfterDelete();
}
function EditLicenseDetail(e) {
    debugger;
    var currentRow = $(e.target).closest("tr");
    var srno = currentRow.find("#srno").text();
    var lic_name = currentRow.find("#LicenseNm").text();
    var lic_number = currentRow.find("#LicenseNum").text();
    var ExpDate = currentRow.find("#ExpDate").text();
    var parts = ExpDate.split("-");   // ["2025", "11", "15"]
    var formattedDate = parts[2] + "-" + parts[1] + "-" + parts[0];
    var ExpiryAlrtDays = currentRow.find("#ExpiryAlrtDays").text();

    $("#hdn_lid").val(srno);
    $("#LicenseName").val(lic_name);
    $("#LicenseNumber").val(lic_number);
    $("#ExpiryDate").val(formattedDate);
    $("#ExpiryAlertDays").val(ExpiryAlrtDays);

    $("#divAddlicenseDtl").attr("style", "display: none;");
    $("#divUpdateLicense").attr("style", "display: block;");
}
function OnClickLicenceUpdateBtn() {
    debugger;
    var srno = $("#hdn_lid").val();
    var LicenseName = $("#LicenseName").val();
    var LicenseNumber = $("#LicenseNumber").val();
    var ExpiryDat = $("#ExpiryDate").val();
    //var parts = ExpiryDat.split("-");   // ["2025", "11", "15"]
    //var ExpiryDate = parts[2] + "-" + parts[1] + "-" + parts[0];
    var formattedDate = "";
    if (ExpiryDat && ExpiryDat.trim() !== "") {
        var parts = ExpiryDat.split("-");
        if (parts.length === 3) {
            formattedDate = parts[2] + "-" + parts[1] + "-" + parts[0];
        }
    }
    var ExpiryAlertDays = $("#ExpiryAlertDays").val();

    $("#LicenseDetailTble > tbody > tr #srno:contains(" + srno + ")").closest('tr').each(function () {
        let row = $(this);
        row.find("#LicenseNm").text(LicenseName);
        row.find("#LicenseNum").text(LicenseNumber);
        row.find("#ExpDate").text(formattedDate);
        row.find("#ExpiryAlrtDays").text(ExpiryAlertDays);
    });

    $("#LicenseName").val("");
    $("#LicenseNumber").val("");
    $("#ExpiryDate").val("");
    $("#ExpiryAlertDays").val("");

    $("#divAddlicenseDtl").attr("style", "display: block;");
    $("#divUpdateLicense").attr("style", "display: none;");
}
function SerialNoAfterDelete() {
    debugger;
    var SerialNo = 0;
    $("#LicenseDetailTble >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#rowno").text(SerialNo);
    });
};
function OnChangeLicenseName() {
    $("#SpanLicenseName").css("display", "none");
    $("#LicenseName").css("border-color", "#ced4da");
}
/*------------Code End by Shubham Maurya on 15-11-2025 to bind all licance table data-----------------*/