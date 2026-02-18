$(document).ready(function () {  
    $("#datatable-buttons tbody tr").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $('#ListFilterData').val();
            var WF_Status = $('#WF_Status').val();
            var clickedrow = $(e.target).closest("tr");
            var Gate_no = clickedrow.children("#GPassNumber").text();
            var Gate_dt = clickedrow.children("#ddlGPassDate").text();
            if (Gate_no != null && Gate_no != "" && Gate_dt != null && Gate_dt != "") {
                window.location.href = "/ApplicationLayer/GatePassOutward/DblClick/?Gate_no=" + Gate_no + "&Gate_dt=" + Gate_dt + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status;
            }
        }
        catch (err) {
            debugger
        }
    });
    $("#datatable-buttons tbody").bind("click", function (e) {

        debugger;
        var clickedrow = $(e.target).closest("tr");
        var Gate_no = clickedrow.children("#GPassNumber").text();
        var Gate_dt = clickedrow.children("#ddlGPassDate").text();
        var Doc_id = $("#DocumentMenuId").val();
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(Gate_no, Gate_dt, Doc_id, Doc_Status);
        $("#hdDoc_No").val(Gate_no);
    });
})

function OnChangeEntityType_List() {
    debugger;
    var EntityType = $("#ddl_EntityType").val();

    $("#ddl_EntityName").val(0).trigger('change');
    BindSR_SuppCustList_List(EntityType);
  
}
function BindSR_SuppCustList_List(EntityType) {
    debugger;
    var sou_type = $("#ddlsource_type" + " option:selected").val();

    $("#ddl_EntityName").select2({
        ajax: {
            url: $("#hfsuppcustlist").val(),
            data: function (params) {
                var queryParameters = {
                    EntityName: params.term, // search term like "a" then "an"
                    entity_type: EntityType,
                   // source_type: sou_type
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            //type: 'POST',
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    Error_Page();
                    return false;
                }
                params.page = params.page || 1;
                return {
                    //results:data.results,
                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }
        },
    });
}

function SearchData() {
    debugger;
    try {
        var Entity_type = $("#ddl_EntityType").val();
        $("hd_EntityTypeID").val(Entity_type);
        var Entity_id = $("#ddl_EntityName option:selected").val();
        var EntityName = $("#ddl_EntityName option:selected").text();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/GatePassOutward/DataSearch_Search",
            data: {
                Entity_type: Entity_type,
                Entity_id: Entity_id,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#ListtblGatePassOutWard').html(data);
                $('#ListFilterData').val(Entity_type + ',' + Entity_id + ',' + Fromdate + ',' + Todate + ',' + Status + ',' + EntityName);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }

        });
    } catch (err) {
        debugger;
        console.log("ItemSetup Error : " + err.message);
    }
    ResetWF_Level();
}