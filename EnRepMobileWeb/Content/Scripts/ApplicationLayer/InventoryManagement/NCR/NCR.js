$(document).ready(function () {
    $("#ddlSrcDocNo").select2();
});

async function GetNcrDetailsOnSerch() {
    let FromDt = $("#txtFromdate").val()
    let ToDt = $("#txtTodate").val()
    let SrcDocNo = $("#ddlSrcDocNo").val()
    let Status = $("#ddlStatus").val()
    
    $("#TableNcrData").html(`<img src="${window.location.origin}/Content/Images/loader.gif" style=" margin-left: 33vw; margin-top: 25vh; margin-bottom: 20vh;" width="50" height="50" />`);
    $.ajax({
        type: "GET",
        url: "/ApplicationLayer/NCR/SerchNCR",
        data: {
            FromDt: FromDt, ToDt: ToDt, SrcDocNo: SrcDocNo, Status: Status
        },
        success: function (data) {
            $("#TableNcrData").html(data);
        }
    })

}
function OnClickAcknowledge(e) {
    debugger;
    let row = $(e.target).closest('tr');
    let item_id = row.find("#td_item_id").val();
    let item_Name = row.find("#ddlitem_name").text().trim();
    let entity_name = row.find("#td_entity_name").text();
    let bill_no = row.find("#td_bill_no").text();
    let rej_qty = row.find("#td_rej_qty").text()
    let rwk_qty = row.find("#td_rwk_qty").text()
    let short_qty = row.find("#td_short_qty").text()
    let simp_qty = row.find("#td_simp_qty").text()
    let uom_id = row.find("#td_uom_id").val();
    let src_type = row.find("#td_src_type_id").text();
    let doc_no = row.find("#td_doc_no").text();
    let doc_dt = row.find("#td_hdn_doc_dt").text()
    let entity_id = row.find("#td_entity_id").text();
    let sr_no = row.find("#td_sr_no").text();
    var totalQuantity=0;
    $.ajax({
        type: "GET",
        url: "/ApplicationLayer/NCR/Acknowledgement",
        data: {
            item_id, uom_id, src_type, doc_no, doc_dt, entity_id
        },
        success: function (data) {
            $("#PopupAcknowledgement").html(data);

            $("#AcknowledgeItemName").val(item_Name);
            $("#AcknowledgeEntityName").val(entity_name);
            $("#AcknowledgeBillNumber").val(bill_no);
            $("#Acknowledge_rej_Quantity").val(rej_qty);
            $("#Acknowledge_rwk_Quantity").val(rwk_qty);
            $("#Acknowledge_short_Quantity").val(short_qty);
            $("#Acknowledge_smpl_Quantity").val(simp_qty);

            $("#ncr_hdn_item_id").text(item_id);
            $("#ncr_hdn_uom_id").text(uom_id);
            $("#ncr_hdn_src_type").text(src_type);
            $("#ncr_hdn_doc_no").text(doc_no);
            $("#ncr_hdn_doc_dt").text(doc_dt);
            $("#ncr_hdn_entity_id").text(entity_id);
            $("#ncr_hdn_srno").text(sr_no);
        }
    })

}
async function NcrAckSaveAndExit() {
     debugger;
     if (CheckValitaionForAcknowledgement() == true) {
         let item_id = $("#ncr_hdn_item_id").text();
         let uom_id = $("#ncr_hdn_uom_id").text();
         let src_type = $("#ncr_hdn_src_type").text();
         let doc_no = $("#ncr_hdn_doc_no").text();
         let doc_dt = $("#ncr_hdn_doc_dt").text();
         let entity_id = $("#ncr_hdn_entity_id").text();
         let ack_by = $("#AcknowledgeBy").val();
         let ack_dt = $("#AcknowledgeDate").val();
         let ack_taken = $("#ActionTaken").val();
         let remarks = $("#AcknowledgeRemarks").val();
         let SrNo = $("#ncr_hdn_srno").text();

         let data = {
             item_id: item_id, uom_id: uom_id, src_type: src_type, doc_no: doc_no
             , doc_dt: doc_dt, entity_id: entity_id, ack_by: ack_by, ack_dt: ack_dt
             , ack_taken: ack_taken, remarks: remarks
         }
         console.log("submit dismissed");
         $("#btnItemNcrAckSaveAndExit").attr("data-dismiss", "modal");
         await NcrSubmitDataAsync(SrNo, data).then(res => {
             console.log("submitted");
         }).catch(err => console.log(err.message));
     } else {
         return false;
     }
    
     
}
async function NcrSubmitDataAsync(SrNo, data) {
    return await $.ajax({
        type: "POST",
        url: "/ApplicationLayer/NCR/SaveNCRAck",
        data: {
            AckListData: data
        },
        success: function (data) {
            debugger;//expected output "Acknowledged,ACK"

            let TargetRow = $("#datatable-buttons tbody tr #td_sr_no:contains('" + SrNo + "')").closest('tr');
            let SpltData = data.split(',');
            TargetRow.find("#td_ack_status").text(SpltData[0]);
            TargetRow.find("#td_ack_status_code").text(SpltData[1]);
        }
    })
    
}
function CheckValitaionForAcknowledgement() {
    let errorFlag = "N";
    if (CheckVallidation("AcknowledgeBy","vmAcknowledgeBy") == false) {
        errorFlag = "Y";
    }
    if (CheckVallidation("AcknowledgeDate","vmAcknowledgeDate") == false) {
        errorFlag = "Y";
    }
    if (CheckVallidation("ActionTaken","vmActionTaken") == false) {
        errorFlag = "Y";
    }
    //if (CheckVallidation("AcknowledgeRemarks","vmAcknowledgeRemarks") == false) {
    //    errorFlag = "Y";
    //}
    if (errorFlag == "Y") {
        return false;
    } else {
        return true;
    }
}
function onChnageAcknowledgeBy() {
    CheckVallidation("AcknowledgeBy", "vmAcknowledgeBy");
}
function onChnageAcknowledgeDate() {
    CheckVallidation("AcknowledgeDate", "vmAcknowledgeDate");
}
function onChnageActionTaken() {
    CheckVallidation("ActionTaken", "vmActionTaken");
}
//function onChnageRemarks() {
//    CheckVallidation("AcknowledgeRemarks", "vmAcknowledgeRemarks");
//}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#td_item_id").val();
    ItemInfoBtnClick(ItmCode);
}

