$(document).ready(function () {
    debugger; 
    BindGLAcountName();
    //$("#ddl_accname").multiselect();
    //$("#ddl_CostCenterType").multiselect();
    $("#ddl_accname").select2();
    $("#ddl_CostCenterType").select2();
    $("#ddl_CostCenterVal").html(`<option value="0">---Select---</option>`);
    $("#ddl_CostCenterVal").select2();
    //$("#ddl_CostCenterVal").multiselect();
    //$(document).ready(function () {
    //    debugger;
    //    /*$("#datatable-buttons1_wrapper a.btn.btn-default.buttons-csv.buttons-html5.btn-sm").remove();*/

    //    $("#datatable-buttons1_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="CCReAllocationCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
    //});
});
function CCReAllocationCSV() {
    var GlAcc_id = $("#hdn_MultiselectGLAccName").val()
    var CCTyp_id = $("#hdn_MultiselectCostCenterTyp").val()
    let CC_Val_id = $("#hdn_CostCenterValId").val()
    var AllocationTyp_id = $('#ddl_AllocationType option:selected').val();
    let From_Dt = $("#txtFromdate").val();
    let To_Dt = $("#txtTodate").val();
    window.location.href = "/ApplicationLayer/CostCenterReAllocation/CCReAllocationExporttoExcelDt?GlAcc_id=" + GlAcc_id + "&CCTyp_id=" + CCTyp_id + "&CC_Val_id=" + CC_Val_id + "&AllocationTyp_id=" + AllocationTyp_id + "&From_Dt=" + From_Dt + "&To_Dt=" + To_Dt;
}
function BindGLAcountName() {
    /*$("#ddl_accname").multiselect({*/
    $("#ddl_accname").select2({
        ajax: {
            url: $("#GLListName").val(),
            data: function (params) {
                var queryParameters = {
                    //restrictedCountry: $("#resCountry").val(),  // pass your own parameter                
                    ddlGroup: params.term, // search term like "a" then "an"
                    Group: params.page
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
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
    if (ToDate != "") {
        validatefydate(FromDate, ToDate);
    }
}
function OnchngeGlAccNameMultiSelect() {
    debugger;
    var selected = [];
    var abc = "";
    $('#ddl_accname option:selected').each(function () {
        if (abc == "") {

            abc += selected[$(this).text()] = $(this).val();
        }
        else {
            abc += selected[$(this).text()] = "," + $(this).val();
        }
    });
    $("#hdn_MultiselectGLAccName").val(abc);

}
function OnchngeGlAccName() {
    debugger;

    var acc_Id = $("#ddl_accname option:selected").val();
    $("#hdn_MultiselectGLAccName").val(acc_Id);
        //if (acc_name == "0" || acc_name == null || acc_name == "") {
        //    $("#vmacc_name").text($("#valueReq").text());
        //    $("#vmacc_name").css("display", "block");
        //    $("[aria-labelledby='select2-ddl_accname-container']").css("border-color", "red");
        //}
        //else {
            /*$("#hdn_MultiselectGLAccName").val(acc_Id);*/
        //    $("#vmacc_name").css("display", "none");
        //    $("[aria-labelledby='select2-ddl_accname-container']").css("border-color", "#ced4da");
        //}
    }
function onChangeCCTypeMultiSelect() {
    debugger;
    var selected = [];
    var abc = "";
    
    $('#ddl_CostCenterType option:selected').each(function () {
        if (abc == "") {

            abc += selected[$(this).text()] = $(this).val();
            //abc += $(this).val();
            
        }
        else {
            abc += selected[$(this).text()] = "," + $(this).val();
            
        }
    });
    $("#hdn_MultiselectCostCenterTyp").val(abc);
    debugger;
    var count = abc.length;
    if (count > 0) {

        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/CostCenterReAllocation/GetCostCenterValueList",
                data: {
                    CCTypeIDS: abc
                },
                success: function (data) {
                    debugger;
                    var arr = JSON.parse(data)

                    $("#ddl_CostCenterVal").html(`<option value="0">---Select---</option>`);
                    arr.map((item) => {
                        $("#ddl_CostCenterVal").append(`<option value="${item.cc_val_id}">${item.cc_val_name}</option>`)
                        //$("#hdn_MultiselectCostCenterVal").val(abc1);
                    })
                },
                error: function OnError(xhr, errorType, exception) {
                    /*hideLoader();*/
                }
            });
        } catch (err) {
            debugger;
            /*hideLoader();*/
            console.log("Error : " + err.message);
        }
    }
    else {
        $('#ddl_CostCenterVal').children().remove().end()
        $("#ddl_CostCenterVal").html(`<option value="0">---Select---</option>`);
    }
    
}
function onChangeCCType() {
    debugger;
    var CCType_Id = $("#ddl_CostCenterType option:selected").val();
    $("#hdn_MultiselectCostCenterTyp").val(CCType_Id);
    $("#hdn_CostCenterValId").val("");
    $("#hdn_CostCenterValName").val("");
    BindCostCenterValue(CCType_Id);
}
function BindCostCenterValue(CCType_Id) {
    debugger;
   
       try {
                $.ajax({
                    type: "POST",
                    url: "/ApplicationLayer/CostCenterReAllocation/GetCostCenterValueList",
                    data: {
                        CCTypeIDS: CCType_Id
                    },
                    success: function (data) {
                        debugger;
                        var arr = JSON.parse(data)
                        $("#ddl_CostCenterVal").html(`<option value="0">---Select---</option>`);
                        arr.map((item) => {
                            $("#ddl_CostCenterVal").append(`<option value="${item.cc_val_id}">${item.cc_val_name}</option>`)
                        })
                        hideLoader();

                    },
                    error: function OnError(xhr, errorType, exception) {
                        hideLoader();
                    }
                });
       }
       catch (err)
       {
                debugger;
                hideLoader();
                console.log("Cost Center Error : " + err.message);
           
        }

    
}
function OnchngeCCVal() {
    debugger;
    var CCValId = $('#ddl_CostCenterVal option:selected').val();
    var CCvalName = $('#ddl_CostCenterVal option:selected').text();
    $("#hdn_CostCenterValId").val(CCValId);
    $("#hdn_CostCenterValName").val(CCvalName);
    //var selected = [];
    //var abc = "";
    //$('#ddl_CostCenterVal option:selected').each(function () {
    //    if (abc == "") {

    //        abc += selected[$(this).text()] = $(this).val();
    //    }
    //    else {
    //        abc += selected[$(this).text()] = "," + $(this).val();
    //    }
    //});
    //$("#hdn_MultiselectCostCenterVal").val(abc);

}


function onChangeFromDt() {
    if (CheckVallidation("txtFromdate", "span_Fromdate") == false) {

    }
}
function onChangeToDt() {
    if (CheckVallidation("txtTodate", "span_Todate") == false) {

    }
}

function SearchCostCenterReAllocationDetails() {
    debugger;
    try {

        debugger;
        var GlAcc_id = $("#hdn_MultiselectGLAccName").val()
        var CCTyp_id = $("#hdn_MultiselectCostCenterTyp").val()
        let CC_Val_id = $("#hdn_CostCenterValId").val()
        var AllocationTyp_id = $('#ddl_AllocationType option:selected').val();
        //var CCvalName = $('#ddl_AllocationType option:selected').Text();
        let From_Dt = $("#txtFromdate").val();
        let To_Dt = $("#txtTodate").val();
        //let val_dgt = $("#ValDigit").text();
        //var Flag = "N";

        try {
                $.ajax({
                    type: "POST",
                    url: "/ApplicationLayer/CostCenterReAllocation/OnSrchGetCostCenterReAllocationReport",
                    data: {
                        GlAcc_id: GlAcc_id, CCTyp_id: CCTyp_id, CC_Val_id: CC_Val_id, AllocationTyp_id: AllocationTyp_id, From_Dt: From_Dt, To_Dt: To_Dt
                    },
                    success: function (data) {
                        debugger;

                        $('#Tbl_CostCenterReAllocationDetail').html(data);
                        /*$("#datatable-buttons1_wrapper a.btn.btn-default.buttons-csv.buttons-html5.btn-sm").remove();*/
                        //$("#datatable-buttons1_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="CCReAllocationCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                        //let expence = $("#spn_TotalExpence").text();
                        //let revenue = $("#spn_TotalRevenue").text();
                        //$("#spn_TotalExpence").text(cmn_addCommas(parseFloat(CheckNullNumber(expence)).toFixed(val_dgt)));
                        //$("#spn_TotalRevenue").text(cmn_addCommas(parseFloat(CheckNullNumber(revenue)).toFixed(val_dgt)));

                        hideLoader();

                    },
                    error: function OnError(xhr, errorType, exception) {
                        //hideLoader();
                    }
                });
            } catch (err) {
                debugger;
                //hideLoader();
                console.log("Error : " + err.message);
            }
        

    }
    catch (ex) {
        console.log(ex);
        //hideLoader();
    }
}

// ----------Cost Center Section Start ------------//

function Onclick_CCbtn(flag, e) {
    debugger;
    //var TotalAmt1 = 0;//add by sm 09-12-2024
    //var Doc_ID = $("#DocumentMenuId").val();//add by sm 09-12-2024
    var ValDigit = $("#ValDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var SpanRowId = clickedrow.find("#pthdn_SpanRowId").text();
    var Int_Br_Id = clickedrow.find("#pthdn_int_br_id").text();
    var Vou_No = clickedrow.find("#pthdn_vou_no").text().trim();
    var Vou_Dt = clickedrow.find("#pthdn_vou_dt").text();
    var Vou_Typ = clickedrow.find("#pthdn_vou_type").text();
    var GLAcc_Name = clickedrow.find("#pthdn_acc_name").text();
    var GLAcc_id = clickedrow.find("#pthdn_acc_id").text();
    var Amount = clickedrow.find("#pthdn_gl_amt").text(); 
    
    var NewArr = new Array();
    /*var disableflag = ($("#txtdisable").val());*/
    var disableflag = "N";
    $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + GLAcc_id + ")").closest('tr').each(function () {
        var row = $(this);
        var List = {};
        List.GlAccount = row.find("#hdntbl_GlAccountId").text();
        List.ddl_CC_Type = row.find("#hdntbl_CCtype").text();
        List.ddl_Type_Id = row.find('#hdntbl_CCtype_Id').text();
        List.ddl_CC_Name = row.find('#hdntbl_CC_Name').text();
        List.ddl_Name_Id = row.find('#hdntbl_CCName_Id').text();
        var amount = cmn_ReplaceCommas(row.find("#hdntbl_CstAmt").text());
        //List.CC_Amount = parseFloat(amount).toFixed(ValDigit);
        List.CC_Amount = cmn_addCommas(toDecimal(amount, ValDigit));
        //TotalAmt1 = parseFloat(TotalAmt1) + parseFloat(amount);//add by sm 09-12-2024
        NewArr.push(List);
    });
    //var TotalAmt = cmn_addCommas(toDecimal(TotalAmt1, ValDigit));//add by sm 09-12-2024
   $.ajax({
        type: "POST",
        url: "/ApplicationLayer/CostCenterReAllocation/FetchCostCenterData",
       data: {
          /* Int_Br_Id: Int_Br_Id,*/
            Vou_No: Vou_No,
            Vou_Dt: Vou_Dt,
            GLAcc_id: GLAcc_id,
            Flag: flag,
            CC_rowdata: JSON.stringify(NewArr),
        },
        success: function (data) {
            debugger;
            $("#CostCenterDetailPopup").html(data);
            $("#CC_int_br_id").val(Int_Br_Id);
            $("#CC_GLAccount").val(GLAcc_Name);
            $("#hdnGLAccount_Id").val(GLAcc_id);
            $("#hdnVou_No").val(Vou_No);
            $("#hdnVou_Dt").val(Vou_Dt);
            $("#hdnVou_Typ").val(Vou_Typ);
            $("#GLAmount").val(Amount);
            $("#ddl_CC_Name").append(`<option value="0">---Select---</option>`);
            $("#hdnTable_Id").text("JVouAccDetailsTbl");
            
                
            
        },
    })
}
function SaveInsertCostCenterReAllocationDetail() {
    debugger;
    var ErrorFlag = "N"
    var CC_int_br_id = $("#CC_int_br_id").val();
    var CCVou_No = $("#hdnVou_No").val();
    var CCVou_Dt = $("#hdnVou_Dt").val();
    var Vou_type = $("#hdnVou_Typ").val();
    var CCGLAcc_id = $("#hdnGLAccount_Id").val();
    var FinalCostCntrDetails = [];
    FinalCostCntrDetails = Cmn_InsertCCDetails();
    var CCDetails = JSON.stringify(FinalCostCntrDetails);
    $('#hdn_CC_DetailList').val(CCDetails);
    
    //if (CCDetails == "[]")
    //{
    //    swal("", $("#GlamtmismatchWithCCAmt").text(), "warning");
    //    $("#CCSaveAndExitBtn").attr("data-dismiss", "");
    //    return false;
    //}
    //else {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/CostCenterReAllocation/SaveInsertReAllocationCostCenterData",
            data: {
                CC_int_br_id: CC_int_br_id,
                Vou_No: CCVou_No,
                Vou_Dt: CCVou_Dt,
                Vou_type: Vou_type,
                GLAcc_id: CCGLAcc_id,
                CCDetails: JSON.stringify(FinalCostCntrDetails),
            },
            success: function (data) {
                debugger;
                if (data == "Success") {
                    $("#tbladd > tbody > tr").remove();
                    $("#tbladdhdn > tbody >tr").remove();
                    
                    var lencount = $("#datatable-buttons1 >tbody >tr").length;
                    if (lencount > 0) {
                        $("#datatable-buttons1 >tbody >tr").each(function () {
                            debugger;
                            var currentRow = $(this);
                            var Vou_No = currentRow.find("#pthdn_vou_no").text().trim();
                            var Vou_Dt = currentRow.find("#pthdn_vou_dt").text();
                            var GLAcc_id = currentRow.find("#pthdn_acc_id").text();
                            if (Vou_No == CCVou_No && Vou_Dt == CCVou_Dt && GLAcc_id == CCGLAcc_id) {
                                //currentRow.find("#BtnCostCenterDetail").css("border-color", "green"); 
                                currentRow.find("#BtnCostCenterDetail").children().addClass("green1");
                            }
                        });
                    }
                    swal("", $("#savemsg").text(), "success");
                }
                else {
                    var lencount = $("#datatable-buttons1 >tbody >tr").length;
                    if (lencount > 0) {
                        $("#datatable-buttons1 >tbody >tr").each(function () {
                            debugger;
                            var currentRow = $(this);
                            var Vou_No = currentRow.find("#pthdn_vou_no").text().trim();
                            var Vou_Dt = currentRow.find("#pthdn_vou_dt").text();
                            var GLAcc_id = currentRow.find("#pthdn_acc_id").text();
                            if (Vou_No == CCVou_No && Vou_Dt == CCVou_Dt && GLAcc_id == CCGLAcc_id) {
                                //currentRow.find("#BtnCostCenterDetail").css("border-color", "green"); 
                                currentRow.find("#BtnCostCenterDetail").children().removeClass("green1");
                            }
                        });
                    }
                    /*swal("", $("#deletemsg").text(), "success");*/
                    swal("", $("#savemsg").text(), "success");
                }
            },
        })
    //}
}

// ----------Cost Center Section END ------------//
function OnClickShowVouDtl(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");

    var vou_no = clickedrow.find("#pthdn_vou_no").text().trim();
    var vou_date = clickedrow.find("#pthdn_vou_date").text();
    var vou_dt = clickedrow.find("#pt_vou_dt").text();
    //var narr = clickedrow.find("#td_narr").text();

    var vochinfo = vou_no.split("_");
    if (vochinfo.length > 1) {
        var cflag = vochinfo[1];
        vou_no = vochinfo[0]
    }

    Cmn_GetVouDetails(vou_no, vou_dt, vou_date, cflag, '');
}