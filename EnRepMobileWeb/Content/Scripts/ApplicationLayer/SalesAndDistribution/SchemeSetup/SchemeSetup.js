$(document).ready(function () {
    BindProdGrpCustPrcGrp();
 
    EnableDblClickOnSchemeList();

});

function EnableDblClickOnSchemeList() {
    $("#datatable-buttons tbody tr").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $("#ListFilterData").val();
            var WF_status = $("#WF_status").val();
            var clickedrow = $(e.target).closest("tr");
            var schm_id = clickedrow.children("#hdn_td_schm_id").text();
            if (schm_id != null && schm_id != "") {
                window.location.href = "/ApplicationLayer/SchemeSetup/EditScheme/?schm_id=" + schm_id + "&ListFilterData=" + ListFilterData + "&WF_status=" + WF_status;
            }

        }
        catch (err) {
        }
    });
}

function Select2FilterData(data,TableId,fieldId) {

    // for placeholder
    if (!data.id) {
        return data.text;
    }

    // collect grp_id already used in table
    var usedGrpIds = [];
    $("#" + TableId + " tbody tr").each(function () {
        usedGrpIds.push($(this).find("#" + fieldId).text());
    });

    // hide option if already exists in table
    if (usedGrpIds.includes(data.id.toString())) {
        return null;   // 🔥 this hides it
    }

    return data.text;
}

/*-------------------- Validations before save data -----------------------*/
function CheckFormValidation() {
    try {
        let status = $("#hdSchmStatus").val();
        if (status == "D" || status == "") {
            if (!chkPrdGrpAndCstPrGrpInOtherSchm()) {
                return false;
            }
        }
        if (CheckHeaderValidation()) {

            if (ValidateSchemeSlabData()) {

                /*------------------ Scheme Slab Detail -----------------*/
                let schmSlbDtl = SchemeSlabData();
                if (schmSlbDtl.length > 0) {
                    $("#schemeSlabDetail").val(JSON.stringify(schmSlbDtl));
                }
                else {
                    swal("", $("#SlabDataNotFound").text(), "warning");
                    return false;
                }

                /*------------------ Product Group Detail -----------------*/

                let schmPrdGrpDtl = SchemeProdGrpData();
                if (schmPrdGrpDtl.length > 0) {
                    $("#schemeProductGrpDetail").val(JSON.stringify(schmPrdGrpDtl));
                }
                else {
                    swal("", $("#ProductGroupDataNotFound").text(), "warning");
                    return false;
                }
                /*------------------ Product Group Detail End -----------------*/

                /*----------------- Customer Price Group ------------------*/

                let schmCstPrcGrpDtl = SchemeCustPriceGrpData();

                if (schmCstPrcGrpDtl.length > 0) {
                    $("#schemeCustPriceGrpDetail").val(JSON.stringify(schmCstPrcGrpDtl));
                }
                else {
                    swal("", $("#CustomerPriceGroupDataNotFound").text(), "warning");
                    return false;
                }
                /*----------------- Customer Price Group End ------------------*/
                EnableFieldsForSave();
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    }
    catch (ex) {
        console.log(ex.message);
        return false;
    }
}
function CheckHeaderValidation() {
    try {
        let scheme_name = $("#scheme_name").val();
        let valid_from = $("#valid_from").val();
        let valid_upto = $("#valid_upto").val();
        let ErrFlag = "N";
        if (scheme_name != "" && scheme_name != null) {
            RemoveValidationEffect("scheme_name", "vmscheme_name");
        }
        else {
            AddValidationEffect("scheme_name", "vmscheme_name", $("#valueReq").text());
            ErrFlag = "Y";
        }

        if (valid_from != "" && valid_from != null) {
            RemoveValidationEffect("valid_from", "vmvalid_from");
        }
        else {
            AddValidationEffect("valid_from", "vmvalid_from", $("#valueReq").text());
            ErrFlag = "Y";
        }

        if (valid_upto != "" && valid_upto != null) {
            if (moment(valid_upto) >= moment(valid_from)) {
                RemoveValidationEffect("valid_upto", "vmvalid_upto");
            } else {
                AddValidationEffect("valid_upto", "vmvalid_upto", $("#JC_InvalidDate").text());
            }
        }
        else {
            AddValidationEffect("valid_upto", "vmvalid_upto", $("#valueReq").text());
            ErrFlag = "Y";
        }


        if (ErrFlag == "Y") {
            return false;
        }
        else {
            return true;
        }
    }
    catch (ex) {
        console.log(ex.message);
        return false;
    }
}
function ValidateSchemeSlabData() {
    try {
        let ErrFlag = "N";
        let SchemeSlabTableRows = $("#TblSchemeSlabDetail tbody tr");
        SchemeSlabTableRows.each(function () {
            let Row = $(this);
            let SlabId = Row.find("#Slab").val();
            let FromQty = Row.find("#FromQuantity").val();
            let UptoQty = Row.find("#ToQuantity").val();
            let focQty = Row.find("#FOCQuantity").val();
            if (SlabId != 1) {
                if (parseFloat(CheckNullNumber(focQty)) > 0) {
                    RemoveValidationEffect("FOCQuantity", "FOCQuantityError", Row);
                }
                else {
                    AddValidationEffect("FOCQuantity", "FOCQuantityError", "Invailid Quantity", Row);
                    ErrFlag = "Y";
                }
            }
            if (parseFloat(CheckNullNumber(UptoQty)) > parseFloat(CheckNullNumber(FromQty))) {
                let NextSlabRow = SchemeSlabTableRows.find("#Slab[value=" + (parseInt(SlabId) + 1) + "]").closest("tr");
                if (NextSlabRow.length > 0) {

                    let NxtRwToQty = NextSlabRow.find("#ToQuantity").val();

                    if (NxtRwToQty == "" || parseFloat(CheckNullNumber(UptoQty)) < parseFloat(CheckNullNumber(NxtRwToQty))) {

                        RemoveValidationEffect("ToQuantity", "ToQuantityError", Row);
                        NextSlabRow.find("#FromQuantity").val((parseFloat(CheckNullNumber(UptoQty)) + 1));

                    } else {
                        AddValidationEffect("ToQuantity", "ToQuantityError", $("#InvalidRange").text(), Row);
                        ErrFlag = "Y";
                    }
                } else {
                    RemoveValidationEffect("ToQuantity", "ToQuantityError", Row);
                }

                return true;
            }
            else {
                AddValidationEffect("ToQuantity", "ToQuantityError", $("#InvalidRange").text(), Row);
                ErrFlag = "Y";
            }
        });
        if (ErrFlag == "Y") {
            return false;
        }
        else {
            return true;
        }
    }
    catch (ex) {
        console.log(ex.message);
        return false;
    }
    
}
function AddValidationEffect(fieldId, errFieldId, msg, row) {
    if (row != null && row != "") {
        row.find("#" + fieldId).css("border-color", "red");
        row.find("[aria-labelledby='select2-" + fieldId + "-container']").css("border-color", "red");
        row.find("#" + errFieldId).text(msg);
        row.find("#" + errFieldId).css("display", "block");
    }
    else {
        $("#" + fieldId).css("border-color", "red");
        $("[aria-labelledby='select2-" + fieldId + "-container']").css("border-color", "red");
        $("#" + errFieldId).text(msg);
        $("#" + errFieldId).css("display", "block");
    }
}
function RemoveValidationEffect(fieldId, errFieldId, row) {
    if (row != null && row != "") {
        row.find("#" + fieldId).css("border-color", "#ced4da");
        row.find("[aria-labelledby='select2-" + fieldId + "-container']").css("border-color", "#ced4da");
        row.find("#" + errFieldId).text("");
        row.find("#" + errFieldId).css("display", "none");
    }
    else {
        $("#" + fieldId).css("border-color", "#ced4da");
        $("[aria-labelledby='select2-" + fieldId + "-container']").css("border-color", "#ced4da");
        $("#" + errFieldId).text("");
        $("#" + errFieldId).css("display", "none");
    }
    
}
function EnableFieldsForSave() {
    $("#scheme_name").attr("disabled", false);
    $("#valid_from").attr("disabled", false);
    $("#valid_from").attr("min", "");
    $("#valid_upto").attr("disabled", false);
    $("#valid_upto").attr("min", "");
    $("#scheme_type").attr("disabled", false);
}
function chkPrdGrpAndCstPrGrpInOtherSchm() {
    var PrdGrpErrorFlag = "N";
    var CstPrcGrpErrorFlag = "N";
    var prodGrps = [];
    var CstPrcGrps = [];
    $.ajax({
        type: "Post",
        url: "/ApplicationLayer/SchemeSetup/chkPrdGrpAndCstPrGrpInOtherSchm",
        data: {
            fromDt: $("#valid_from").val(),
            uptoDt: $("#valid_upto").val(),
            scheme_id: $("#Hdn_Scheme_Id").val(),
            prodGrps: JSON.stringify(prodGrps),
            CstPrcGrps: JSON.stringify(CstPrcGrps)
        },
        async:false,
        success: function (data) {
            debugger
            var arr = JSON.parse(data);
            if (arr.Table.length > 0) {
                $("#TblSchemeProdGrpDetail tbody tr").each(function () {
                    let grpId = $(this).find("#prdGrpId").text();
                    if (arr.Table.some(x => x.prod_grp == grpId)) {
                        $(this).css("border", "2px solid red");
                        PrdGrpErrorFlag = "Y";
                    } else {
                        $(this).css("border", "1px solid #ced4da");
                    }

                });
            }
            if (arr.Table1.length > 0) {
                $("#TblSchemeCustPrcGrpDetail tbody tr").each(function () {
                    let grpId = $(this).find("#cstPrcGrpId").text();
                    if (arr.Table1.some(x => x.cust_prc_grp == grpId)) {
                        $(this).css("border", "2px solid red");
                        CstPrcGrpErrorFlag = "Y";
                    } else {
                        $(this).css("border", "1px solid #ced4da");
                    }

                });
            }
            
        },
    })
    if (PrdGrpErrorFlag == "Y" && CstPrcGrpErrorFlag == "N") {
        swal("", "Some product groups are already added in other scheme for this range.", "warning");
        return false;
    }
    else if (PrdGrpErrorFlag == "N" && CstPrcGrpErrorFlag == "Y") {
        swal("", "Some customer price groups are already added in other scheme for this range.", "warning");
        return false;
    }
    else if (PrdGrpErrorFlag == "Y" && CstPrcGrpErrorFlag == "Y") {
        swal("", "Some product groups and Customer price groups are already added in other scheme for this range.", "warning");
        return false;
    }
    else {
        return true;
    }
}
/*-------------------- Validations before save data end -----------------------*/

/*-------------------- Bind tables data to save -----------------------*/
function SchemeSlabData() {
    
    let schmSlbDtl = [];
    $("#TblSchemeSlabDetail tbody tr").each(function () {
        let Row = $(this);
        let List = {};
        let Rno = Row.find("#Slab").val();
        List.slab = Rno;
        List.fromQty = Row.find("#FromQuantity").val();
        List.toQty = Row.find("#ToQuantity").val();
        List.focQty = CheckNullNumber(Row.find("#FOCQuantity").val());
        List.remarks = Row.find("#SlabRemarks" + Rno).val();
        schmSlbDtl.push(List)
    });
    return schmSlbDtl;
}
function SchemeProdGrpData() {
    
    let schmPrdGrpDtl = [];
    $("#TblSchemeProdGrpDetail tbody tr").each(function () {
        let Row = $(this);
        let List = {};
        List.prd_grp_id = Row.find("#prdGrpId").text();
        List.prd_grp_name = Row.find("#prdGrpNm").text();
        schmPrdGrpDtl.push(List)
    });
    return schmPrdGrpDtl;
}
function SchemeCustPriceGrpData() {
    
    let schmCstPrcGrpDtl = [];

    $("#TblSchemeCustPrcGrpDetail tbody tr").each(function () {
        let Row = $(this);
        let List = {};
        List.cst_prc_grp_id = Row.find("#cstPrcGrpId").text();
        List.cst_prc_grp_name = Row.find("#cstPrcGrpNm").text();
        schmCstPrcGrpDtl.push(List)
    });
    return schmCstPrcGrpDtl;
}
/*-------------------- Bind tables data to save end----------------------*/

function AddNewRow() {
    try {
        let RowNo = $("#TblSchemeSlabDetail tbody tr").length + 1;
        let lastRow = $("#TblSchemeSlabDetail tbody tr:last-child");
        let fromQty = RowNo == 1 ? 0 : parseFloat(lastRow.find("#ToQuantity").val()) + 1;

        let AddRowAllow = true;
        if (RowNo != 1) {
            let lastRowFromQty = lastRow.find("#FromQuantity").val();
            let lastRowUptoQty = lastRow.find("#ToQuantity").val();
            if (parseFloat(CheckNullNumber(lastRowUptoQty)) > parseFloat(CheckNullNumber(lastRowFromQty))) {
                AddRowAllow = true;
            } else {
                AddRowAllow = false;
            }
        } 
        if (AddRowAllow) {

            $("#TblSchemeSlabDetail tbody tr").each(function () {
                let row = $(this);
                row.find("#td_delete").html("");
            });

            var row = `
<tr class="highlight_tr" style="background-color: rgba(38, 185, 154, 0.16);">
<td class="red center" id="td_delete">
<i class="fa fa-trash" onclick="DeleteSchemeSlab(${RowNo})" aria-hidden="true" id="delBtnIcon" title="Delete"></i>
</td>
                                                                    
<td><input id="Slab" value="${RowNo}" class="form-control num_right" autocomplete="off" type="text" name="Slab" placeholder="1" readonly=""></td>
<td><input id="FromQuantity" value="${fromQty}" class="form-control num_right" autocomplete="off" type="text" name="FromQuantity" placeholder="0000.00" readonly=""></td>
<td>
    <div class="lpo_form">
    <input id="ToQuantity" value="" class="form-control num_right" autocomplete="off" type="text" onchange="OnChangeUptoQuantity(event)" onkeypress="return OnKeyPressToQty(this,event);" name="ToQuantity" placeholder="0000.00">
    <span id="ToQuantityError" class="error-message is-visible"></span>
    </div>
</td>
<td>
    <div class="lpo_form">
    <input id="FOCQuantity" value="" class="form-control num_right" autocomplete="off" type="text" onchange="OnChangeFocQuantity(event)" onkeypress="return OnKeyPressFocQty(this,event);" name="FOCQuantity" placeholder="0000.00">
    <span id="FOCQuantityError" class="error-message is-visible"></span>
    </div>
</td>
<td><textarea id="SlabRemarks${RowNo}" class="form-control TaskDescription remarksmessage" onclick="open_popupdes('SlabRemarks${RowNo}')" onmouseover="OnMouseOver(this)" placeholder="${$("#span_remarks").text()}" title="${$("#span_remarks").text()}" maxlength="500" ></textarea></td>

</tr>`;
            $("#TblSchemeSlabDetail tbody").append(row);

        }
        
        return true;
    }
    catch (ex) {
        console.log(ex.message);
        return false;
    }
}
function DeleteSchemeSlab(SlabId) {
    try {
        debugger;
        $("#TblSchemeSlabDetail tbody tr #Slab[value=" + SlabId + "]").closest("tr").remove();
        if ((parseInt(SlabId) - 1) > 0) {
            $("#TblSchemeSlabDetail tbody tr #Slab[value=" + (parseInt(SlabId) - 1) + "]").closest("tr").each(function () {
                let row = $(this);
                row.find("#td_delete").html(`<i class="fa fa-trash" onclick="DeleteSchemeSlab(${(parseInt(SlabId) - 1)})" aria-hidden="true" id="delBtnIcon" title="Delete"></i>`);
            });
        }
        if ($("#TblSchemeSlabDetail tbody tr").length == 0 && $("#TblSchemeProdGrpDetail tbody tr").length == 0
            && $("#TblSchemeCustPrcGrpDetail tbody tr").length == 0) {
            EnableHeaderDetail();
            DisableProdGrpAndCstPrcGrp();
        }
    }
    catch (ex) {
        console.log(ex.message);
        return false;
    }
}
function DeleteSchemeProdGrp(e) {
    try {
        debugger;
        $(e.target).closest("tr").remove();
        $("#TblSchemeProdGrpDetail tbody tr").each(function (index) {
            $(this).find("#srno").text(index+1);
        });
        if ($("#TblSchemeSlabDetail tbody tr").length == 0 && $("#TblSchemeProdGrpDetail tbody tr").length == 0
            && $("#TblSchemeCustPrcGrpDetail tbody tr").length == 0) {
            EnableHeaderDetail();
            DisableProdGrpAndCstPrcGrp();
        }
    }
    catch (ex) {
        console.log(ex.message);
        return false;
    }
}
function DeleteSchemeCstPrcGrp(e) {
    try {
        debugger;
        $(e.target).closest("tr").remove();
        $("#TblSchemeCustPrcGrpDetail tbody tr").each(function (index) {
            $(this).find("#srno").text(index + 1);
        });
        if ($("#TblSchemeSlabDetail tbody tr").length == 0 && $("#TblSchemeProdGrpDetail tbody tr").length == 0
            && $("#TblSchemeCustPrcGrpDetail tbody tr").length == 0) {
            EnableHeaderDetail();
            DisableProdGrpAndCstPrcGrp();
        }
    }
    catch (ex) {
        console.log(ex.message);
        return false;
    }
}
function OnChangeUptoQuantity(e) {
    try {
        if ($("#TblSchemeSlabDetail tbody tr").length == 1) {
            DisableHeaderDetail();
            BindProdGrpCustPrcGrp();
            EnableProdGrpAndCstPrcGrp();
        }
        
        let Row = $(e.target).closest('tr');
        debugger;
        let SlabId = Row.find("#Slab").val();
        let FromQty = Row.find("#FromQuantity").val();
        let UptoQty = Row.find("#ToQuantity").val();
        if (parseFloat(CheckNullNumber(UptoQty)) > parseFloat(CheckNullNumber(FromQty))) {
            let NextSlabRow = $("#TblSchemeSlabDetail tbody tr #Slab[value=" + (parseInt(SlabId) + 1) + "]").closest("tr");
            if (NextSlabRow.length > 0) {

                let NxtRwToQty = NextSlabRow.find("#ToQuantity").val();
                if (NxtRwToQty == "" || parseFloat(CheckNullNumber(UptoQty)) < parseFloat(CheckNullNumber(NxtRwToQty))) {
                    Row.find("#ToQuantity").css("border-color", "#ced4da");
                    Row.find("#ToQuantityError").css("display", "none");
                    Row.find("#ToQuantityError").text("");
                    NextSlabRow.find("#FromQuantity").val((parseFloat(CheckNullNumber(UptoQty)) + 1));
                } else {
                    Row.find("#ToQuantity").css("border-color", "red");
                    Row.find("#ToQuantityError").css("display", "block");
                    Row.find("#ToQuantityError").text($("#InvalidRange").text());
                }
            } else {
                Row.find("#ToQuantity").css("border-color", "#ced4da");
                Row.find("#ToQuantityError").css("display", "none");
                Row.find("#ToQuantityError").text("");
            }
            
            
            return true;
        } else {
            Row.find("#ToQuantity").css("border-color", "red");
            Row.find("#ToQuantityError").css("display", "block");
            Row.find("#ToQuantityError").text($("#InvalidRange").text());
            return false;
        }
    }
    catch (ex) {
        console.log(ex.message);
        return false;
    }
}
function OnChangeFocQuantity(e) {
    try {

        let Row = $(e.target).closest('tr');
        debugger;
        let SlabId = Row.find("#Slab").val();
        let FocQty = Row.find("#FOCQuantity").val();
        if (SlabId != "1") {
            if (parseFloat(CheckNullNumber(FocQty)) > 0) {
                RemoveValidationEffect("FOCQuantity", "FOCQuantityError", Row);
            }
            else {
                AddValidationEffect("FOCQuantity", "FOCQuantityError", $("#valueReq").text(), Row);
                ErrFlag = "Y";
            }
        }
    }
    catch (ex) {
        console.log(ex.message);
        return false;
    }
}
function OnChangeSchemeName() {
    try {

        debugger;
        let scheme_nm = $("#scheme_name").val();
        if (scheme_nm != null && scheme_nm != "" ) {
            RemoveValidationEffect("scheme_name", "vmscheme_name", null);
        }
        else {
            AddValidationEffect("scheme_name", "vmscheme_name", $("#valueReq").text(), null);
            ErrFlag = "Y";
        }
    }
    catch (ex) {
        console.log(ex.message);
        return false;
    }
}
function OnChangeValidFrom() {
    try {

        debugger;
        let valid_fr = $("#valid_from").val();
        if (valid_fr != null && valid_fr != "" ) {
            RemoveValidationEffect("valid_from", "vmvalid_from", null);
            $("#valid_upto").attr("min", valid_fr);
        }
        else {
            AddValidationEffect("valid_from", "vmvalid_from", $("#valueReq").text(), null);
            ErrFlag = "Y";
        }
        let valid_to = $("#valid_upto").val();
        if (valid_to != null && valid_to != "") {
            if (moment(valid_fr) > moment(valid_to)) {
                AddValidationEffect("valid_upto", "vmvalid_upto", $("#JC_InvalidDate").text(), null);
                ErrFlag = "Y";
                DisableSlabDetailField();
            } else {
                RemoveValidationEffect("valid_upto", "vmvalid_upto", null);
                EnableSlabDetailField();
            }
        }
        
    }
    catch (ex) {
        console.log(ex.message);
        return false;
    }
}
function OnChangeValidUpTo() {
    try {

        debugger;
        let valid_to = $("#valid_upto").val();
        if (valid_to != null && valid_to != "" ) {
            RemoveValidationEffect("valid_upto", "vmvalid_upto", null);
        }
        else {
            AddValidationEffect("valid_upto", "vmvalid_upto", $("#valueReq").text(), null);
            ErrFlag = "Y";
        }
        let valid_fr = $("#valid_from").val();
        if (moment(valid_fr) > moment(valid_to)) {
            AddValidationEffect("valid_upto", "vmvalid_upto", $("#JC_InvalidDate").text(), null);
            ErrFlag = "Y";
            DisableSlabDetailField();
        } else {
            RemoveValidationEffect("valid_upto", "vmvalid_upto", null);
            EnableSlabDetailField();
        }
        
    }
    catch (ex) {
        console.log(ex.message);
        return false;
    }
}
function OnChangeSchemeType() {
    try {

        debugger;
        let schm_type = $("#scheme_type").val();
        if (schm_type == "QB") {
            $("#th_rangeFrom").text($("#span_FromQty").text());
            $("#th_rangeTo").text($("#span_ToQty").text());
        }
        else if (schm_type == "VB") {
            $("#th_rangeFrom").text($("#span_FromValue").text());
            $("#th_rangeTo").text($("#span_ToValue").text());
        }
    }
    catch (ex) {
        console.log(ex.message);
        return false;
    }
}
function OnChangeProductGroup() {
    try {

        debugger;
        let prod_grp_id = $("#prod_grp_id").val();
        if (prod_grp_id != null && prod_grp_id != "") {
            RemoveValidationEffect("prod_grp_id", "vmprod_grp_id", null);
        }
        else {
            AddValidationEffect("prod_grp_id", "vmprod_grp_id", $("#valueReq").text(), null);
            ErrFlag = "Y";
        }
    }
    catch (ex) {
        console.log(ex.message);
        return false;
    }
}
function OnChangeCustPriceGroup() {
    try {

        debugger;
        let cust_prc_grp_id = $("#cust_prc_grp_id").val();
        if (cust_prc_grp_id != null && cust_prc_grp_id != "") {
            RemoveValidationEffect("cust_prc_grp_id", "vmcust_prc_grp_id", null);
        }
        else {
            AddValidationEffect("cust_prc_grp_id", "vmcust_prc_grp_id", $("#valueReq").text(), null);
            ErrFlag = "Y";
        }
    }
    catch (ex) {
        console.log(ex.message);
        return false;
    }
}

function OnClickAddProdGroupButton() {
    try {
        let prodGrpId = $("#prod_grp_id").val();
        if (prodGrpId != "0" && prodGrpId != "") {
            let prodGrpName = $("#prod_grp_id option:selected").text();
            let RowNo = $("#TblSchemeProdGrpDetail tbody tr").length + 1;
            var row = `<tr>
                <td class="red center">
                    <i class="fa fa-trash" aria-hidden="true" onclick="DeleteSchemeProdGrp(event)" id="delBtnIcon" title="Delete"></i>
                </td>
                <td id="srno" class="sr_padding">${RowNo}</td>
                <td id="prdGrpId" hidden>${prodGrpId}</td>
                <td id="prdGrpNm" >${prodGrpName}</td>
                <td>&nbsp;</td>
            </tr>`;
            $("#TblSchemeProdGrpDetail tbody").append(row);
            $("#prod_grp_id").val(0).trigger('change');
        }
        else {
            AddValidationEffect("prod_grp_id", "vmprod_grp_id", $("#valueReq").text());
        }
    }
    catch (ex) {
        console.log(ex.message);
        return false;
    }
}
function OnClickAddCustPrcGrpButton() {
    try {

        let custPrcGrpId = $("#cust_prc_grp_id").val();
        if (custPrcGrpId != "0" && custPrcGrpId != "") {
            let custPrcGrpName = $("#cust_prc_grp_id option:selected").text();
            let RowNo = $("#TblSchemeCustPrcGrpDetail tbody tr").length + 1;
            var row = `<tr>
                <td class="red center">
                    <i class="fa fa-trash" aria-hidden="true" onclick="DeleteSchemeCstPrcGrp(event)" id="delBtnIcon" title="@Resource.Delete"></i>
                </td>
                <td id="srno" class="sr_padding">${RowNo}</td>
                <td id="cstPrcGrpId" hidden>${custPrcGrpId}</td>
                <td id="cstPrcGrpNm" >${custPrcGrpName}</td>
                <td>&nbsp;</td>
            </tr>`;
            $("#TblSchemeCustPrcGrpDetail tbody").append(row);
            $("#cust_prc_grp_id").val(0).trigger('change');
        }
        else {
            AddValidationEffect("cust_prc_grp_id", "vmcust_prc_grp_id", $("#valueReq").text());
        }
    }
    catch (ex) {
        console.log(ex.message);
        return false;
    }
    
}
function FilterItemDetail(e, tableId) {
    try {
        if (tableId == "TblSchemeSlabDetail") {
            Cmn_FilterTableData(e, tableId, [{ "FieldId": "Slab", "FieldType": "input", "SrNo": "" }]);
        }
        if (tableId == "TblSchemeProdGrpDetail") {
            Cmn_FilterTableData(e, tableId, [{ "FieldId": "prdGrpNm", "FieldType": "td", "SrNo": "" }]);
        }
        if (tableId == "TblSchemeCustPrcGrpDetail") {
            Cmn_FilterTableData(e, tableId, [{ "FieldId": "cstPrcGrpNm", "FieldType": "td", "SrNo": "" }]);
        }
    }
    catch (ex) {
        console.log(ex.message);
        return false;
    }
    
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
        debugger;
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
function OnKeyPressToQty(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    RemoveValidationEffect("ToQuantity", "ToQuantityError", clickedrow);
    return true;
}
function OnKeyPressFocQty(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    RemoveValidationEffect("FOCQuantity", "FOCQuantityError", clickedrow);
    return true;
}
function BtnSearch() {
    let prod_grp_id = $("#prod_grp_id").val();
    let cust_prc_grp_id = $("#cust_prc_grp_id").val();
    let Status = $("#ddlStatus").val();
    let act_status = $("#act_status").val();

    $.ajax({
        type: "Post",
        url: "/ApplicationLayer/SchemeSetup/SearchSchemeDetailList",
        data: {
            prod_grp_id: prod_grp_id,
            cust_prc_grp_id: cust_prc_grp_id,
            Status: Status,
            act_status: act_status
        },
        success: function (data) {
            debugger

            $("#div_SchemeSetupList").html(data);
            let ListFilterData = prod_grp_id + ',' + cust_prc_grp_id + ',' + Status + ',' + act_status;
            $("#ListFilterData").val(ListFilterData);
        },
    })
}

function BindProdGrpCustPrcGrp() {

    let valid_fr = $("#valid_from").val();
    let valid_to = $("#valid_upto").val();
    if (valid_fr != "" && valid_to != "") {
        if (moment(valid_fr) > moment(valid_to)) {
            return false;
        }
        $.ajax({
            type: "Post",
            url: "/ApplicationLayer/SchemeSetup/GetPctGrpAndCstPrcGrpList",
            data: {
                fromDt: valid_fr,
                uptoDt: valid_to,
                scheme_id: $("#Hdn_Scheme_Id").val()
            },
            async: false,
            success: function (data) {
                debugger
                var arr = JSON.parse(data);
                var opts = `<option value="0">---Select---</option>`;
                if (arr.Table.length > 0) {
                    arr.Table.map((item) => {
                        opts += `<option value="${item.item_grp_id}">${item.item_grp_name}</option>`
                    });
                }
                $("#prod_grp_id").html(opts);

                var optsCstPrcGrp = `<option value="0">---Select---</option>`;
                if (arr.Table1.length > 0) {
                    arr.Table1.map((item) => {
                        optsCstPrcGrp += `<option value="${item.setup_id}">${item.setup_val}</option>`
                    });
                }
                $("#cust_prc_grp_id").html(optsCstPrcGrp);

                $("#prod_grp_id").select2({
                    templateResult: function (data) { return Select2FilterData(data, "TblSchemeProdGrpDetail", "prdGrpId") }
                });
                $("#cust_prc_grp_id").select2({
                    templateResult: function (data) { return Select2FilterData(data, "TblSchemeCustPrcGrpDetail", "cstPrcGrpId") }
                });
            },
        })
    }
    else {
        
        DisableSlabDetailField();
        $("#prod_grp_id").select2({
            templateResult: function (data) { return Select2FilterData(data, "TblSchemeProdGrpDetail", "prdGrpId") }
        });
        $("#cust_prc_grp_id").select2({
            templateResult: function (data) { return Select2FilterData(data, "TblSchemeCustPrcGrpDetail", "cstPrcGrpId") }
        });
    }
}

function EnableSlabDetailField() {
    $("#TblSchemeSlabDetail tbody tr").each(function () {
        var row = $(this);

        let srno = row.find("#Slab").val();
        row.find("#ToQuantity").attr("disabled", false);
        row.find("#FOCQuantity").attr("disabled", false);
        row.find("#SlabRemarks" + srno).attr("readonly", false);
        row.find("#td_delete").html(`<i class="fa fa-trash" onclick="DeleteSchemeSlab(1)" aria-hidden="true" id="delBtnIcon" title="Delete"></i>`);
    });
    $("#BtnAddItem").css("display", "");
}
function DisableSlabDetailField() {
    $("#TblSchemeSlabDetail tbody tr").each(function () {
        var row = $(this);
        let srno = row.find("#Slab").val();
        row.find("#ToQuantity").attr("disabled", true);
        row.find("#FOCQuantity").attr("disabled", true);
        row.find("#SlabRemarks" + srno).attr("readonly", true);
        row.find("#td_delete").html(``);
    });
    $("#BtnAddItem").css("display", "none");

}
function DisableProdGrpAndCstPrcGrp() {
    $("#prod_grp_id").attr("disabled", true);
    $("#cust_prc_grp_id").attr("disabled", true);
    $("#BtnAddCustPrcGrp").css("display", "none");
    $("#BtnAddProdGrp").css("display", "none");
}
function EnableProdGrpAndCstPrcGrp() {
    $("#prod_grp_id").attr("disabled", false);
    $("#cust_prc_grp_id").attr("disabled", false);
    $("#BtnAddCustPrcGrp").css("display", "");
    $("#BtnAddProdGrp").css("display", "");
}

function DisableHeaderDetail() {
    $("#valid_from").attr("readonly", true);
    $("#valid_upto").attr("readonly", true);
}
function EnableHeaderDetail() {
    $("#valid_from").attr("readonly", false);
    $("#valid_upto").attr("readonly", false);
}


