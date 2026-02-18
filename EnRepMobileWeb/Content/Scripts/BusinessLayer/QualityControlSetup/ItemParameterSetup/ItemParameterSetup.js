
$(document).ready(function () {
    GetQCItemList();
    ReplicateWith();
    $("#ItemParameterName").select2({
        matcher: matchCustom, 
    });
    $("#uom_cd").select2({
        matcher: matchCustom,
    });
    $("#ddlReplicateWith").attr("disabled", true);
    AutoGenerateSerial();
});
function matchCustom(params, data) {
    
    if (data.element != null) {
        if (data.element.className == "hide") {
            return false;
        }
        else {
            if (data.id == "0") {
                return data;
            }
            if (params.term == null) {
                return data;
            }
            if (data.text.toLowerCase().indexOf(params.term.toLowerCase()) > -1) {
                return data;
                // This includes matching the `children` how you want in nested data sets
                // return Filtered data;
            }
            else {
                return false;
            }
           
        }
    }
}
function validateSerial(input) {
    // remove non-numeric characters
    input.value = input.value.replace(/[^0-9]/g, '');

    // prevent starting with 0
    if (input.value.startsWith("0")) {
        input.value = input.value.replace(/^0+/, '');
    }

    // limit max to 999
    if (parseInt(input.value) > 999) {
        input.value = "999";
    }
}
function SortTableBySrNo() {

    var rows = $("#QCItemParameterDetailTbl tbody tr").get();

    rows.sort(function (a, b) {
        var A = parseInt($(a).find("#sr_no").text().trim());
        var B = parseInt($(b).find("#sr_no").text().trim());
        return A - B;    // ascending order  (use B - A for descending)
    });

    $.each(rows, function (index, row) {
        $("#QCItemParameterDetailTbl tbody").append(row); // reorder rows
    });
}
function AutoGenerateSerial()
{
   
    var TotalRow = 0;
    var arr = [];     // array to store sr_no values

    $("#QCItemParameterDetailTbl tbody tr").each(function () {
        var sr_no = $(this).find("#sr_no").text().trim();

        if (sr_no !== "" && !isNaN(sr_no)) {
            arr.push(parseInt(sr_no));    // store in array
        }
    });

    if (arr.length > 0) {
        var maxVal = Math.max.apply(Math, arr);   // get highest value
        TotalRow = maxVal+1;                    // push +1
    }
    else {
        if (TotalRow == "0" || TotalRow == "" || TotalRow == null || parseInt(TotalRow) == 0) {
            TotalRow = 1
        }
        else {
            TotalRow += 1;
        }
    }

  
    $("#SerialNumber").val(TotalRow);
    SortTableBySrNo();
}
var DeleteText = $("#Span_Delete_Title").text();
var EditText = $("#Edit").text();
function ReplicateWith() {
    debugger;
    var item = $("#ddlReplicateWith").val();

    $("#ddlReplicateWith").append("<option value='0'>---Select---</option>");
    $("#ddlReplicateWith").select2({
        ajax: {
            url: "/BusinessLayer/ItemParameter/BindReplicateWithlist",
            data: function (params) {
                var queryParameters = {
                    item: params.term,
                    page: params.page || 1
                };
                return queryParameters;
            },
            multiple: true,
            cache: true,
            processResults: function (data, params) {
                debugger
                var pageSize,
                    pageSize = 2000; // or whatever pagesize

                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="select2-results__group">
                     <div class="row rw">
                     <div class="col-md-9 col-xs-12 def-cursor">${$("#ItemName").text()}</div>
                     <div class="col-md-3 col-xs-12 def-cursor" id="ItemUOM">${$("#ItemUOM").text()}</div>
                     </div>
                    </strong></li></ul>`)
                }
                var page = 1;
                data = data.slice((page - 1) * pageSize, page * pageSize)
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID, text: val.Name.split(",")[0], document: val.Name.split(",")[1] };
                    }),
                };
            },
            cache: true
        },
        templateResult: function (data) {
            debugger
            var classAttr = $(data.element).attr('class');
            var hasClass = typeof classAttr != 'undefined';
            classAttr = hasClass ? ' ' + classAttr : '';
            var $result;
            $result = $(
                '<div class="row rw">' +
                '<div class="col-md-9 col-xs-12' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-3 col-xs-12' + classAttr + '">' + data.document + '</div>' +
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        },
    });
}
function OnChanngReplicateWith() {
    debugger;
    var ItemId = $("#ddlReplicateWith").val();
    $('#QCItemParameterDetailTbl >tbody >tr').remove();
    $("#ddlReplicateWith").attr("disabled", true);
    $.ajax(
        {
            type: "POST",
            url: "/BusinessLayer/ItemParameter/GetReplicateWithItemId",
            data: {
                ItemId: ItemId
            },
            success: function (data) {
                debugger;
                if (data !== null && data !== "" && data != "ErrorPage") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        for (var i = 0; i < arr.Table.length; i++) {
                            for (var i = 0; i < arr.Table.length; i++) {
                                var ParaName = arr.Table[i].ParaName;
                                var ParamTypeName = arr.Table[i].ParamTypeName;
                                var LowerValue = arr.Table[i].LowerValue;
                                var UpperValue = arr.Table[i].UpperValue;
                                var uom_name = arr.Table[i].uom_name;
                                var remarks = arr.Table[i].remarks;
                                var param_Id = arr.Table[i].param_Id;
                                var uom_id = arr.Table[i].uom_id;
                                var sr_no = arr.Table[i].sr_no;
                                onchangereplicate_item(ParaName, ParamTypeName, LowerValue, UpperValue, uom_name, remarks, param_Id, uom_id, sr_no);
                            }
                        }
                    }                 
                }                
            }
        })
}
function onchangereplicate_item(ParaName, ParamTypeName, LowerValue, UpperValue, uom_name, remarks, param_Id, uom_id, sr_no) {
    var rowIdx = 0;
    $('#QCItemParameterDetailTbl tbody').append(`<tr id="R${++rowIdx}">
 <td id="sr_no">${sr_no}</td>
<td class="red center"> <i class="fa fa-trash" aria-hidden="true" id="delBtnIcon" data-toggle="tooltip"  title="${DeleteText}"></i></td>
<td class="center edit_icon"><span id="editBtnIcon2"><i class="fa fa-edit" aria-hidden="true" id="editBtnIcon" data-toggle="tooltip" title="${EditText}"></i></span></td>
<td  id="ItemParameterNametblValue">${ParaName}</td>
<td  id="ParameterTypetblValue">${ParamTypeName}</td>
<td  id="LowerRangetblValue">${LowerValue}</td>
<td  id="UpperRangetblValue">${UpperValue}</td>

<td  id="ParamUom">${uom_name}</td>
<td  id="RemarkstblValue">${remarks}</td>
<td  hidden id="ItemParameterNametblValue1">${param_Id}</td>
<td  hidden id="hduom_cd">${uom_id}</td>
</tr>`);

    ResetParameterValues(param_Id);
    //---------------------------- Row edit Button funtionality ------------------//
    EditRow();

    //---------------------------- Row Delete Button funtionality ------------------//
    DeleteRow();
}
function GetViewDetails() {
    debugger;
    $("#QCItemParameterDetailTbl > tbody > tr").each(function () {
        var option = $(this).find("#ItemParameterNametblValue1").text();
        //$("#ItemParameterName option[value=" + option + "]").hide();
        $("#ItemParameterName option[value=" + option + "]").addClass("hide");
    });
    debugger
    if ($("#btn_save").prop("disabled") == false && $("#btn_save").val() == "Update") {
        EditRow();
        DeleteRow();
        $('#ItemParameterName').prop("disabled", false);
        $('#SerialNumber').prop("disabled", false);
        $("#detailremarks").attr("readonly", true);
        $("#divNew").css("display", "block");
    }
    else {
        $('#UpperRange').prop("readonly", true);
        $('#LowerRange').prop("readonly", true);
        $("#uom_cd").attr("disabled", true);
        $('#SerialNumber').prop("disabled", true);
        $("#divNew").css("display", "none");
        $('#ItemParameterName').prop("disabled", true);
        $("#detailremarks").attr("readonly", true);
    }
}
function SaveBtnClick() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 03-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#btn_save").prop("disabled", true); /*End*/
    }
    var flag = 'N';
    var Value = $("#ItemNameForQC").val();
    if (Value == "0" || Value == null || Value == "") {
        flag = 'Y';
        $('#QCItemNameErrorMsg').text($("#valueReq").text());
        $("[aria-labelledby='select2-ItemNameForQC-container']").css("border-color", "Red");
        $("#QCItemNameErrorMsg").css("display", "block");
        return false
    }
    else {

        $("#ItemNameForQC").css("border-color", "#ced4da");
        $('#QCItemNameErrorMsg').hide();
        var rowCount = $('#QCItemParameterDetailTbl tbody tr').length;
        if (rowCount <= 0) {
            flag = 'Y';
            swal("", $("#LeastRequired").text(), "warning"); // -------------------at least one parameter required ----------------------//
            return false;
        }
        else {
            var status = $("#status").val();
            var Items = new Array();
            $("#QCItemParameterDetailTbl TBODY TR").each(function () {
                debugger;
                var row = $(this);
                var ItemsParameter = {};
                //ItemsParameter.item_id = $("#ItemNameForQC").val();
                //ItemsParameter.param_Id = row.find("TD").eq(8).text();
                ////var test1 = row.find("TD").eq(8).text();
                ////var test2= row.find("TD").eq(9).text();
                //ItemsParameter.param_name = row.find("TD").eq(2).text();
                //ItemsParameter.para_type = row.find("TD").eq(3).text();
                //ItemsParameter.operation = "Create";//$("#Operation").val();
                ////var limit = row.find("TD").eq(4).text();
                //ItemsParameter.upper_val = row.find("TD").eq(4).text();
                //ItemsParameter.lower_val = row.find("TD").eq(5).text();
                //ItemsParameter.remarks = row.find("TD").eq(7).text();
                //ItemsParameter.uom_id = row.find("TD").eq(9).text();


                ItemsParameter.sr_no = row.children("#sr_no").text();
                ItemsParameter.item_id = $("#ItemNameForQC").val();
                ItemsParameter.param_Id = row.children("#ItemParameterNametblValue1").text();
                ItemsParameter.param_name = row.children("#ItemParameterNametblValue").text();
                ItemsParameter.para_type = row.children("#ParameterTypetblValue").text();
                ItemsParameter.operation = "Create";//$("#Operation").val();
                ItemsParameter.upper_val = row.children("#UpperRangetblValue").text();
                ItemsParameter.lower_val = row.children("#LowerRangetblValue").text();
                ItemsParameter.remarks = row.children("#RemarkstblValue").text();
                ItemsParameter.uom_id = row.children("#hduom_cd").text();

                //if (status == "Approved") {
                //    ItemsParameter.status = status;
                //}
                Items.push(ItemsParameter);


            });
            var data = JSON.stringify(Items);
            $("#hdnItemList").val(data);
            //window.location.href = "/BusinessLayer/ItemParameter/QCParameterSetupSave/?Items=" + data;
            

            //$("#QCParameterNameErrorMsg").css("display", "none");
            //$("#ItemParameterName").css("border-color", "#ced4da");
            //$('#ItemParameterName').prop("disabled", true);
            //$('#QCItemParameterDetailTbl tbody tr td i#delBtnIcon ').css('display', 'none');
            //$('#QCItemParameterDetailTbl tbody tr td i#editBtnIcon ').css('display', 'none');
            //$('#divNew').css('display', 'none');
            //$('#divUpdate').css('display', 'none');

            //GetChangeOnSaveBtnClick();


        }
        if (flag == 'Y') {
            return false;
        }
        else {
            $("#btn_save").css("filter", "grayscale(100%)");
            $("#hdnSavebtn").val("AllreadyClick");
            return true;
        }
        
    }

}
function ItemNameSelection() {
    debugger
    
    var Value = $("#ItemNameForQC").find("option:selected").val();
    if (Value == "0" || Value == "" || Value == String.empty) {
        $("#ItmUOM").val("");
        $("#ItemRefNo").val("");
        $("#ItemOEMNo").val("");
        $("#ItemSampleCode").val("");
        $('#SerialNumber').prop("disabled", true);
        $('#ItemParameterName').prop("disabled", true);
        $('#UpperRange').prop("readonly", true);
        $('#LowerRange').prop("readonly", true);
        $("#uom_cd").attr("disabled", true);
        $('#divNew').css('display', 'none');
        $('#divUpdate').css('display', 'none');

    }

    else {
        $("#QCItemNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ItemNameForQC-container']").css("border-color", "#ced4da");
        $('#ItemParameterName').prop("disabled", false);
        $('#SerialNumber').prop("disabled", false);
        $("#ddlReplicateWith").attr("disabled", false);
        $('#UpperRange').prop("readonly", true);
        $('#LowerRange').prop("readonly", true);
        $("#uom_cd").attr("disabled", true);
        $('#divNew').css('display', 'block');
        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: "/BusinessLayer/ItemParameter/ItemParaListDetails",
            data: { Value},
            dataType: "json",

            success: function (result) {
                debugger;
                for (var i = 0; i < result.length; i++) {
                    var ItemCode = result[i].item_id;
                    if (ItemCode == Value) {
                        $("#ItmUOM").val(result[i].UOMName);
                        $("#ItemRefNo").val(result[i].RefNo);
                        $("#ItemOEMNo").val(result[i].OEMNo);
                        $("#ItemSampleCode").val(result[i].SampleCode);

                    }
                }
            },
            //called on jquery ajax call failure
            error: function ajaxError(result) {
                alert(result.status + ' : ' + result.statusText);
            }
        });
    }

}
function GetQCItemList() {

    BindItemList("#ItemNameForQC", "", "", "", "", "IPS");
    GetViewDetails();
    //$.ajax(
    //    {
    //        type: "GET",
    //        url: "/BusinessLayer/ItemParameter/ItemParaListDetails",

    //        dataType: "json",
    //        success: function (data) {
    //            debugger;
    //            if (data == 'ErrorPage') {
    //                //LSO_ErrorPage();
    //                return false;
    //            }
    //            if (data !== null && data !== "") {
    //                var arr = [];
    //                arr = data;
    //                $('#ItemNameForQC').empty();
    //                if (arr.length > 0) {
    //                    $("#ItemNameForQC option").remove();
    //                    $("#ItemNameForQC optgroup").remove();
    //                    var hdnItemID = $("#hdnItemID").val();
    //                    var hdnItemName = $("#hdnItemName").val();
    //                    var ItmUOM = $("#ItmUOM").val();
    //                    $('#ItemNameForQC').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);

    //                /* $('#Textddl').append("<option value='0'>---Select---</option>");*/
    //                    if (hdnItemID != null && hdnItemID != "") {
    //                        $('#Textddl').append(`<option data-uom="0" value="0">---Select---</option>`);
    //                        $('#Textddl').append(`<option data-uom="${ItmUOM}" value="${hdnItemID}">${hdnItemName}</option>`);
    //                        $("#ItemNameForQC").val(hdnItemID).change();
    //                        $("#ItemNameForQC").attr("disabled", true);
                            
    //                    }
    //                    else {
    //                        for (var i = 0; i < arr.length; i++) {
    //                            $('#Textddl').append(`<option data-uom="${arr[i].UOMName}" value="${arr[i].item_id}">${arr[i].item_Name}</option>`);
    //                        }
    //                    }
                       
    //                    var firstEmptySelect = true;
    //                    $('#ItemNameForQC').select2({
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
                       
                       

    //                }
    //                else {
    //                    $('#ItemNameForQC').append("<option value='0'>---Select---</option>");
    //                }
    //            }
    //            GetViewDetails();
    //        },
    //    });
}
function getParaItemvalue() {

    $.ajax(
        {
            type: "GET",
            url: "/BusinessLayer/ItemParameter/ParameterDefinitionValue",
            dataType: "json",
            data: { Parmid: 0 },
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    //  LSO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = data;

                    if (arr.length > 0) {
                        $('#ItemParameterName').empty();
                        $('#ItemParameterName').append("<option value='0'>---Select---</option>");

                        $.each(data, function (index, value) {
                            $('#ItemParameterName').append('<option value="' + value.param_Id + '">' + value.param_name + '</option>');
                        });
                        $("#QCItemParameterDetailTbl > tbody > tr").each(function () {
                            var currentRow = $(this);
                            /*var paramID = currentRow.find("td:eq(7)").text();*/
                            var paramID = currentRow.find("#ItemParameterNametblValue1").text();
                            //$("#ItemParameterName option[value=" + paramID + "]").hide();
                            $("#ItemParameterName option[value=" + paramID + "]").addClass("hide");
                        });
                    }
                }
            },
        });


}

function OnchangeUom() {
    //var uom = $("#uom_cd").val();

    //if (uom == "0" || uom == null || uom == String.empty) {
    //    $('#spanUom').text($("#valueReq").text());
    //    $("#uom_cd").css("border-color", "Red");
    //    $("#spanUom").css("display", "block");
    //}
    //else {
    //    $("#spanUom").css("display", "none");
    //    $("#uom_cd").css("border-color", "#ced4da");
    //}
    var Value = $("#uom_cd").find("option:selected").val();
    if (Value) {
        $("#spanUom").css("display", "none");
        $("#uom_cd").css("border-color", "#ced4da");
        $('[aria-labelledby="select2-uom_cd-container"]').css("border-color", "#ced4da");
    }
    else { return false; }
}


function OnItemParameterSelectName() {
    debugger;
    //  $("#ItemParameterName").change(function () {
    debugger;
    // alert($('option:selected', this).text());
    //  var text = $("#ItemParameterName").find("option:selected").text();
    var Value = $("#ItemParameterName").find("option:selected").val();
    if (Value) {
        $("#QCParameterNameErrorMsg").css("display", "none");
        $("#ItemParameterName").css("border-color", "#ced4da");
        $('[aria-labelledby="select2-ItemParameterName-container"]').css("border-color", "#ced4da");
    }
    else { return false; }

    $.ajax({
        type: "GET",
        url: "/BusinessLayer/ItemParameter/ParameterDefinitionType",
        data: { Parmid: Value },
        dataType: "json",
        success: function (result) {
            //for (var i = 0; i < result.length; i++) {
                //var itemid = result[i].param_Id;
            //if (Value == itemid) {                  
            var type = JSON.parse(result);//[i].param_type;
            $("#ParameterType").val(type);

                if (type == "Qualitative") {

                    $("#UpperRange").val("0");
                    $("#LowerRange").val("0");
                    $("#uom_cd").val("0");
                    $("#UpperRange").attr('readonly', true);
                    $("#LowerRange").attr("readonly", true);
                    $("#detailremarks").attr("readonly", false);
                    $("#uom_cd").attr("disabled", true);
                    return true;
            }
            if (type == "Observative") {

                $("#UpperRange").val("0");
                $("#LowerRange").val("0");
                $("#uom_cd").val("0");
                $("#UpperRange").attr('readonly', true);
                $("#LowerRange").attr("readonly", true);
                $("#detailremarks").attr("readonly", false);
                $("#uom_cd").attr("disabled", true);
                return true;
            }
                if (type == "Quantitative") {

                    $("#UpperRange").attr('readonly', false);
                    $("#LowerRange").attr("readonly", false);
                    $("#detailremarks").attr("readonly", false);
                    $("#uom_cd").attr("disabled", false);
                    $("#UpperRange").on('keypress', function () {
                        $("#UpperRangeErrorMsg").css("display", "none");
                        $("#ComparisonErrorMsg").css("display", "none");
                        $("#UpperRange").css("border-color", "#ced4da");

                        if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
                            event.preventDefault();
                        }
                    });
                    $("#LowerRange").on('keypress', function () {
                        $("#LowerRangeErrorMsg").css("display", "none");
                        $("#LowerRange").css("border-color", "#ced4da");
                        if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
                            event.preventDefault();
                        }
                    });
                    return true;
                //}

                //}          
            }
        },
        error: function ajaxError(result) {
            alert(result.status + ' : ' + result.statusText);
        }
    });
    // });  
}
function OnSerialNumber() {
    $('#QCSerialNumberErrorMsg').text("");
    $("#SerialNumber").css("border-color", "#ced4da");

    $("#QCSerialNumberErrorMsg").css("display", "none");
}
function OnClickAddParaNewAddBtn() {
    debugger;

    var rowIdx = 0;
  var  TotalRow=  $("#SerialNumber").val();
    var ParamId = $("#ItemParameterName").val();
    var uom = $("#uom_cd").val();
    //var value = $("#ItemParameterName option:selected").text();
    var itemType = $("#ParameterType").val();
    var ur = $("#UpperRange").val();
    var lr = $("#LowerRange").val();
    if (TotalRow == "0" || TotalRow == null || TotalRow == 0 || TotalRow == "") {
        $('#QCSerialNumberErrorMsg').text($("#valueReq").text());
        $("#SerialNumber").css("border-color", "Red");

        $("#QCSerialNumberErrorMsg").css("display", "block");
        return false
    }
    if (ParamId == "0" || ParamId == null || ParamId == String.empty) {
        $('#QCParameterNameErrorMsg').text($("#valueReq").text());
        $("#ItemParameterName").css("border-color", "Red");
        $('[aria-labelledby="select2-ItemParameterName-container"]').css("border-color", "Red");
        $("#QCParameterNameErrorMsg").css("display", "block");
        return false
    }
   

    else {
        $('#QCSerialNumberErrorMsg').text("");
        $("#SerialNumber").css("border-color", "#ced4da");
      
        $("#QCSerialNumberErrorMsg").css("display", "none");
        var rowIdx = 0;
        debugger;
        var rowCount = $('#QCItemParameterDetailTbl >tbody >tr').length + 1;
        
        if (itemType == "Quantitative") {
            var ErrorFlag = "N";
            if (ur == null || ur == "" || ur == "0") {
                $('#UpperRangeErrorMsg').text($("#valueReq").text());
                $("#UpperRange").css("border-color", "Red");
                $("#UpperRangeErrorMsg").css("display", "block");
                ErrorFlag = "Y";
               // return false;
            }

            if (lr == null || lr == "" || lr == "0") {
                $('#LowerRangeErrorMsg').text($("#valueReq").text());
                $("#LowerRange").css("border-color", "Red");
                $("#LowerRangeErrorMsg").css("display", "block");
                ErrorFlag = "Y";
               // return false;
            }
            if (uom == "0" || uom == null || uom == String.empty) {
                $('#spanUom').text($("#valueReq").text());
                $("#uom_cd").css("border-color", "Red");
                $('[aria-labelledby="select2-uom_cd-container"]').css("border-color", "Red");
                $("#spanUom").css("display", "block");
                ErrorFlag = "Y";
               // return false
            }

            if (ErrorFlag == "Y") {
                return false;
            }
          

            ///----------------------
            if (parseFloat(ur) < parseFloat(lr)) {
                debugger;
                // swal("", $("#UpperRangeNeverLessThenLowerRange").text(), "success"); return false                
                $('#ComparisonErrorMsg').text($("#UpperRangeNeverLessThenLowerRange").text());
                $("#ComparisonErrorMsg").css("display", "block");
                $("#UpperRange").css("border-color", "Red");

                return false;
            }
            else {
                $("#UpperRange").css("border-color", "#ced4da");
                $('#ComparisonErrorMsg').hide();


                


                $('#QCItemParameterDetailTbl tbody').append(`<tr id="R${++rowIdx}">
<td  id="sr_no">${TotalRow}</td>
<td class="red center"> <i class="fa fa-trash" aria-hidden="true" id="delBtnIcon" data-toggle="tooltip"  title="${DeleteText}"></i></td>
<td class="center edit_icon"><span id="editBtnIcon2"><i class="fa fa-edit" aria-hidden="true" id="editBtnIcon" data-toggle="tooltip" title="${EditText}"></i></span></td>

<td  id="ItemParameterNametblValue">${$("#ItemParameterName option:selected").text()}</td>
<td  id="ParameterTypetblValue">${$("#ParameterType").val()}</td>
<td  id="LowerRangetblValue">${$("#LowerRange").val()}</td>
<td  id="UpperRangetblValue">${$("#UpperRange").val()}</td>

<td  id="ParamUom">${$("#uom_cd option:selected").text()}</td> 
<td  id="RemarkstblValue">${$("#detailremarks").val()}</td>
<td  hidden id="ItemParameterNametblValue1">${$("#ItemParameterName").val()}</td>
<td  hidden id="hduom_cd">${$("#uom_cd").val()}</td>
</tr>`);
                ResetParameterValues(ParamId);
            }
        }
        else if (itemType == "Qualitative" && ur == 0) {

            $('#QCItemParameterDetailTbl tbody').append(`<tr id="R${++rowIdx}">
<td  id="sr_no">${TotalRow}</td>
<td class="red center"> <i class="fa fa-trash" aria-hidden="true" id="delBtnIcon" data-toggle="tooltip"  title="${DeleteText}"></i></td>
<td class="center edit_icon"><span id="editBtnIcon2"><i class="fa fa-edit" aria-hidden="true" id="editBtnIcon" data-toggle="tooltip" title="${EditText}"></i></span></td>

<td  id="ItemParameterNametblValue">${$("#ItemParameterName option:selected").text()}</td>
<td  id="ParameterTypetblValue">${$("#ParameterType").val()}</td>
<td  id="LowerRangetblValue">${$("#LowerRange").val()}</td>
<td  id="UpperRangetblValue">${$("#UpperRange").val()}</td>

<td  id="ParamUom"></td> 
<td  id="RemarkstblValue">${$("#detailremarks").val()}</td>
<td  hidden id="ItemParameterNametblValue1">${$("#ItemParameterName").val()}</td>
<td  hidden id="hduom_cd">${$("#uom_cd").val()}</td>


</tr>`);
            ResetParameterValues(ParamId);
        }
        else if (itemType == "Observative" && ur == 0) {

            $('#QCItemParameterDetailTbl tbody').append(`<tr id="R${++rowIdx}">
<td  id="sr_no">${TotalRow}</td>
<td class="red center"> <i class="fa fa-trash" aria-hidden="true" id="delBtnIcon" data-toggle="tooltip"  title="${DeleteText}"></i></td>
<td class="center edit_icon"><span id="editBtnIcon2"><i class="fa fa-edit" aria-hidden="true" id="editBtnIcon" data-toggle="tooltip" title="${EditText}"></i></span></td>

<td  id="ItemParameterNametblValue">${$("#ItemParameterName option:selected").text()}</td>
<td  id="ParameterTypetblValue">${$("#ParameterType").val()}</td>
<td  id="LowerRangetblValue">${$("#LowerRange").val()}</td>
<td  id="UpperRangetblValue">${$("#UpperRange").val()}</td>

<td  id="ParamUom"></td> 
<td  id="RemarkstblValue">${$("#detailremarks").val()}</td>
<td  hidden id="ItemParameterNametblValue1">${$("#ItemParameterName").val()}</td>
<td  hidden id="hduom_cd">${$("#uom_cd").val()}</td>


</tr>`);
            ResetParameterValues(ParamId);
        }

        else {

            return false;
        }
        //---------------------------- Row edit Button funtionality ------------------//
        EditRow();
       
        //---------------------------- Row Delete Button funtionality ------------------//
        DeleteRow();
        AutoGenerateSerial();
    }

}



function ResetParameterValues(ParamId) {
    var index = $('#ItemParameterName')[0].selectedIndex;  // remove the selected index from the DOM           
    $("#ItemParameterName option[value=" + ParamId + "]").addClass("hide");
    $('#ItemParameterName').val("0").prop('selected', true).change();
    $("#ItemNameForQC").prop("disabled", true);
    $("#ItemParameterName").val(0); 
    $("#ParameterType").val("");
    $("#UpperRange").val("");
    $("#LowerRange").val("");
    $("#detailremarks").val("");
    debugger;
    $("#uom_cd").val("0").trigger('change');
    $("#UpperRange").prop("readonly", true);
    $("#LowerRange").prop("readonly", true);
    $("#detailremarks").prop("readonly", true);
    $("#uom_cd").prop("disabled", true);
}
function EditRow() {
    $("#QCItemParameterDetailTbl >tbody >tr").on('click', "#editBtnIcon", function (e) {
        debugger;
        var currentRow = $(this).closest('tr')
        var row_index = currentRow.index();
        var col1_value = currentRow.find("#editBtnIcon2").text()
      
        var col2_value = currentRow.find("#ItemParameterNametblValue").text()
        var col3_value = currentRow.find("#ParameterTypetblValue").text()
        var col5_value = currentRow.find("#LowerRangetblValue").text()
        var col4_value = currentRow.find("#UpperRangetblValue").text()
        var col6_value = currentRow.find("#RemarkstblValue").text()
        var col7_value = currentRow.find("#ItemParameterNametblValue1").text();
        var col8_value = currentRow.find("#hduom_cd").text()
        var col9_value = row_index
        var colsr_no_value = currentRow.find("#sr_no").text()

        $('#SerialNumber').val(colsr_no_value);
        $('#ItemParameterName').val(col7_value).trigger("change");//.prop('selected', true);
        $("#ParameterType").val(col3_value);
        if (col3_value == "Quantitative") {
            $("#UpperRange").prop("readonly", false);
            $("#LowerRange").prop("readonly", false);
            $("#uom_cd").attr("disabled", false);
            $("#UpperRange").val(col4_value);
            $("#LowerRange").val(col5_value);
            $("#uom_cd").val(col8_value).trigger('change.select2');
        }
        else if (col3_value == "Observative") {
            $("#UpperRange").prop("readonly", true);
            $("#LowerRange").prop("readonly", true);
            $("#uom_cd").attr("disabled", true);
            $("#UpperRange").val(col4_value);
            $("#LowerRange").val(col5_value);
            $("#uom_cd").val(col8_value).trigger('change.select2');;
        }
        else  // if (col3_value = "Qualitative") 
        {
            $("#UpperRange").prop("readonly", true);
            $("#LowerRange").prop("readonly", true);
            $("#uom_cd").attr("disabled", true);
            $("#UpperRange").val(col4_value);
            $("#LowerRange").val(col5_value);
            $("#uom_cd").val(col8_value).trigger('change.select2');;
        }
        $("#detailremarks").val(col6_value);
        $("#detailremarks").attr("readonly", false);
        $("#param_Id").val(col7_value);
        $("#comp_id").val(row_index);
        $("#ItemParameterName").prop("disabled", true);
        $('#divNew').css('display', 'none');
        $('#divUpdate').css('display', 'block');
    });
}
function DeleteRow() {
    $("#QCItemParameterDetailTbl >tbody >tr").on('click', "#delBtnIcon", function (e) {
        debugger;
        var currentRow = $(this).closest('tr')
        ResetParameterValues(optionValue);
        var optionValue = currentRow.find("#ItemParameterNametblValue1").text()
        $("#ItemParameterName option[value=" + optionValue + "]").removeClass("hide");
        $('#divNew').css('display', 'block');
        $('#divUpdate').css('display', 'none');
        $("#ItemParameterName").prop("disabled", false);
        currentRow.remove();
        var count = $("#QCItemParameterDetailTbl >tbody >tr").length;
        if (count == 0) {
            $("#ItemNameForQC").attr("disabled", false);
            $("#ddlReplicateWith").attr("disabled", false);
        }
    });
    //if ($('#QCItemParameterDetailTbl >tbody >tr').length == "0") {
    //    $("#ddlReplicateWith").attr("disabled", false);
    //}
}
function OnClickParaItemUpdateBtn() {

    debugger;
    var Row = $("#ItemParameterName").val();
    var pt = $("#ParameterType").val();
    var sr_no = $("#SerialNumber").val();
    var uom = $("#uom_cd").val();
    if (pt == "Quantitative") {
        $("#UpperRange").on('keypress', function () {
            $("#UpperRangeErrorMsg").css("display", "none");
            $("#ComparisonErrorMsg").css("display", "none");
            $("#UpperRange").css("border-color", "#ced4da");
            if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
                event.preventDefault();
            }
        });
        $("#LowerRange").on('keypress', function () {
            $("#LowerRangeErrorMsg").css("display", "none");

            $("#LowerRange").css("border-color", "#ced4da");
            if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
                event.preventDefault();
            }
        });

        var urr = $("#UpperRange").val();
        var lrr = $("#LowerRange").val();
        var ErrorFlag = "N";
        if (urr == null || urr == "" || urr == "0") {
            $('#UpperRangeErrorMsg').text($("#valueReq").text());
            $("#UpperRange").css("border-color", "Red");
            $("#UpperRangeErrorMsg").css("display", "block");
            ErrorFlag = "Y";

            return false;
        }
        if (lrr == null || lrr == "" || lrr == "0") {
            $('#LowerRangeErrorMsg').text($("#valueReq").text());
            $("#LowerRange").css("border-color", "Red");
            $("#LowerRangeErrorMsg").css("display", "block");
            ErrorFlag = "Y";
            return false;
        }
        if (parseFloat(urr) < parseFloat(lrr)) {
            $('#ComparisonErrorMsg').text($("#UpperRangeNeverLessThenLowerRange").text());
            $("#ComparisonErrorMsg").css("display", "block");
            $("#UpperRange").css("border-color", "Red");

            return false;
        }       
        if (uom == "0" || uom == null || uom == String.empty) {
            $('#spanUom').text($("#valueReq").text());
            $("#uom_cd").css("border-color", "Red");
            $('[aria-labelledby="select2-uom_cd-container"]').css("border-color", "Red");
            $("#spanUom").css("display", "block");
            return false;
        }

        if (ErrorFlag == "Y") {
            return false;
        }
        else {
            $("#UpperRange").css("border-color", "#ced4da");
            $('#ComparisonErrorMsg').hide();
            $('#UpperRangeErrorMsg').hide();
            $("#LowerRange").css("border-color", "#ced4da");
            $('#LowerRangeErrorMsg').hide();

            $("#QCItemParameterDetailTbl > tbody > tr").each(function () {
                debugger;
                var currentRow = $(this);
               /* var RID = currentRow.find("td:eq(8)").text();*/
                var RID = currentRow.find("#ItemParameterNametblValue1").text();
                var remark = $("#detailremarks").val();
                var uom_id = $("#uom_cd").val();
                var uom_name = $("#uom_cd  option:selected").text();
                if (Row == RID) {
                    currentRow.find("#sr_no").text(sr_no);
                    currentRow.find("#LowerRangetblValue").text(lrr);
                    currentRow.find("#UpperRangetblValue").text(urr);
                 
                    currentRow.find("#ParamUom").text(uom_name);
                    currentRow.find("#RemarkstblValue").text(remark);
                    currentRow.find("#hduom_cd").text(uom_id);
                }

            });
            debugger;
            $("#OnClickAddParaNewAddBtn").prop("disabled", false);
            $("#ItemParameterName").prop("disabled", false);
            //$("#ItemParameterName option[value=" + $("#ItemParameterName").val() + "]").hide();
            $('#ItemParameterName').val("0").trigger("change");//.prop('selected', true);
            $("#ParameterType").val("");
            $("#UpperRange").val("");
            $("#LowerRange").val("");
            $('#uom_cd').val("0").trigger("change").prop('selected', true);
            $("#detailremarks").val("");
            $("#UpperRange").prop("readonly", true);
            $("#LowerRange").prop("readonly", true);
            $("#uom_cd").attr("disabled", true);
            $("#detailremarks").attr("readonly", true);
            $('#divNew').css('display', 'block');
            $('#divUpdate').css('display', 'none');
        }


    }
    else {

        $("#UpperRange").css("border-color", "#ced4da");
        $('#ComparisonErrorMsg').hide();

        debugger;
        $("#QCItemParameterDetailTbl > tbody > tr").each(function () {
            var currentRow = $(this);
            var RID = currentRow.find("#ItemParameterNametblValue1").text();
            var remark = $("#detailremarks").val();
            if (Row == RID) {
                //currentRow.find("td:eq(4)").text(urr);
                //currentRow.find("td:eq(5)").text(lrr);
                currentRow.find("#sr_no").text(sr_no);
                currentRow.find("#RemarkstblValue").text(remark);
            }

        });
        $("#OnClickAddParaNewAddBtn").prop("disabled", false);
        $("#ItemParameterName").prop("disabled", false);
        // $("#ItemParameterName option[value=" + Row + "]").hide();
        //var e = $("#ItemParameterName option:selected").val(Row).hide();
        //$("#ItemParameterName option[value=" + optionValue + "]").show();
        //$("#ItemParameterName").select2("val", "0");
        //$("#ItemParameterName option[value=" + $("#ItemParameterName").val() + "]").hide();
        $('#ItemParameterName').val("0").trigger('change');//.prop('selected', true);
        //$('#ItemParameterName option: "Row"').hide();
        $("#ParameterType").val("");
        $("#UpperRange").val("");
        $("#LowerRange").val("");
        $('#uom_cd').val("0").attr('selected', true);
        $("#detailremarks").val("");
        $('#divNew').css('display', 'block');
        $('#divUpdate').css('display', 'none');

    }
    AutoGenerateSerial();
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    // var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var ItmName = "";
    
    ItmCode = $("#ItemNameForQC").val();
   // ItmName = $("#ItemNameForQC option:selected").text()
    ItemInfoBtnClick(ItmCode);
    //if (ItmCode != "" && ItmCode != null) {
    //    try {
    //        $.ajax(
    //            {
    //                type: "POST",
    //                url: "/BusinessLayer/ItemParameter/getQCItemValues",
    //                data: { ItemID: ItmCode },
    //                success: function (data) {
    //                    debugger;
    //                    if (data == 'ErrorPage') {
    //                        LSO_ErrorPage();
    //                        return false;
    //                    }
    //                    if (data !== null && data !== "") {
    //                        var arr = [];
    //                        arr = JSON.parse(data);
    //                        if (arr.Table.length > 0) {
    //                            $("#Txt_ItmOEMNo").val(arr.Table[0].item_oem_no);
    //                            $("#Txt_ItmSampCode").val(arr.Table[0].item_sam_cd);
    //                            $("#Txt_ItmRefNo").val(arr.Table[0].RefNo);
    //                            $("#Txt_ItmHSNCode").val(arr.Table[0].HSN_code);
    //                            $("#SpanItemDescription").text(ItmName);
    //                            var ImgFlag = "N";
    //                            for (var i = 0; i < arr.Table.length; i++) {
    //                                if (arr.Table[i].item_img_name != "" && arr.Table[i].item_img_name != null) {
    //                                    ImgFlag = "Y";
    //                                }
    //                            }
    //                            if (ImgFlag == "Y") {
    //                                var OL = '<ol class="carousel-indicators">';
    //                                var Div = '<div class="carousel-inner">';
    //                                for (var i = 0; i < arr.Table.length; i++) {
    //                                    var ImgName = arr.Table[i].item_img_name;
    //                                    var origin = window.location.origin + "/Attachment/ItemSetup/" + ImgName;
    //                                    if (i === 0) {
    //                                        OL += '<li data-target="#myCarousel" data-slide-to="' + i + '" class="active"> <img src="' + origin + '" />'
    //                                        Div += '<div class="carousel-item active"><img src = "' + origin + '" style = "width:100%; max-height:200px;" /></div>'
    //                                    }
    //                                    else {
    //                                        OL += '<li data-target="#myCarousel" data-slide-to="' + i + '"><img src="' + origin + '" /></li>'
    //                                        Div += '<div class="carousel-item"><img src = "' + origin + '" style = "width:100%; max-height:200px;" /></div>'
    //                                    }
    //                                }
    //                                OL += '</ol>'
    //                                Div += '</div>'

    //                                var Ach = '<a class="carousel-control-prev" href="#myCarousel" role="button" data-slide="prev"><span class="carousel-control-prev-icon" aria-hidden="true"></span><span class="sr-only">Previous</span></a>';
    //                                var Ach1 = '<a class="carousel-control-next" href="#myCarousel" role="button" data-slide="next"><span class="carousel-control-next-icon" aria-hidden="true"></span><span class="sr-only">Next</span></a>';

    //                                $("#myCarousel").html(OL + Div + Ach + Ach1);
    //                            }
    //                            else {
    //                                $("#myCarousel").html("");
    //                            }
    //                        }
    //                    }
    //                },
    //            });
    //    } catch (err) {
    //        console.log("GetMenuData Error : " + err.message);
    //    }
    //}
}
function DeleteBtnClick() {
    debugger;


    $("#ItemNameForQC").css("border-color", "#ced4da");
    $('#QCItemNameErrorMsg').hide();
    debugger;

    var itemID = $("#ItemNameForQC").val();

    if (itemID != 0) {
        try {
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
                    var hdnItemID = $("#hdnItemID").val();
                    $("#hdnDeleteCommand").attr('name', 'Command');
                    $("#hdnDeleteCommand").val("Delete");
                    $('form').submit();
                } else {
                    return false;
                }
            });
            return false;
        }
        catch (err) {
            console.log("ItemSetup Error : " + err.message);
        }


    }
    return false;
}

function onclickapprovebtn() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 03-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn=="AllReadyClickApprove") {
        $("#btn_approve").css("filter", "grayscale(100%)");
        $("#btn_approve").prop("disabled", true); /*End*/
    }
    else {
        $("#btn_approve").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllReadyClickApprove");
    }
   
}