$(document).ready(function () {
    Cmn_initializeMultiselect(['#ddlGatePassType']);
});
//function OnChangeEntityType_List() {
//    debugger;
//    var EntityType = $("#ddl_EntityType").val();
  
//    $("#ddl_EntityName").val(0).trigger('change');
//    BindSR_SuppCustList_List(EntityType);
//}
function OnChangeEntityType_List() {
    debugger;
    var EntityType = $("#ddl_EntityType").val();
    $("#ddl_EntityName").val(null).trigger('change');
    $("#ddl_EntityName").attr('multiple', 'multiple');;
    BindSR_SuppCustList_List(EntityType);
}

//function BindSR_SuppCustList_List(EntityType) {
//    debugger;
//    $("#ddl_EntityName").multiselect({
//        includeSelectAllOption: true,
//        enableFiltering: true,
//        enableCaseInsensitiveFiltering: true,
//        SelectedText2: true,
//        buttonWidth: '100%',
//        buttonClass: 'btn btn-default btn-custom SourceType',
//        nonSelectedText: '---All---',
//        ajax: {
//            url: $("#hfsuppcustlist2").val(),
//            data: function (params) {
//                var queryParameters = {
//                    EntityName: params.term, // search term like "a" then "an"
//                    entity_type: EntityType,
//                };
//                return queryParameters;
//            },
//            dataType: "json",
//            cache: true,
//            delay: 250,
//            //type: 'POST',
//            contentType: "application/json; charset=utf-8",
//            processResults: function (data, params) {
//                if (data == 'ErrorPage') {
//                    Error_Page();
//                    return false;
//                }
//                params.page = params.page || 1;
//                return {
//                    //results:data.results,
//                    results: $.map(data, function (val, item) {
//                        return { id: val.ID, text: val.Name };
//                    })
//                };
//            }
//        },
//    });
//}
function BindSR_SuppCustList_List(EntityType) {
    debugger;
    $.ajax({
        url: $("#hfsuppcustlist2").val(),
        type: "GET",
        data: { entity_type: EntityType },
        success: function (data) {
            debugger
            $("#ddl_EntityName").empty();
            $.each(data, function (i, val) {
                $("#ddl_EntityName").append(
                    $('<option></option>').val(val.ID).text(val.Name)
                );
            });
            Cmn_initializeMultiselect(['#ddl_EntityName']);
            $('#ddl_EntityName').multiselect('rebuild');
        }
    });
}


function SearchData() {
    debugger;
    try {
        //var Source_type = $("#ddlGatePassType").val();
        var Source_type = cmn_getddldataasstring("#ddlGatePassType");
        $("hd_SourceTypeID").val(ddlGatePassType);
        var Entity_type = $("#ddl_EntityType").val();
        //var Entity_type = cmn_getddldataasstring("#ddl_EntityType");
        $("hd_EntityTypeID").val(Entity_type);
        /* var Entity_id = $("#ddl_EntityName option:selected").val();*/
        var Entity_id = cmn_getddldataasstring("#ddl_EntityName");
       // var EntityName = $("#ddl_EntityName option:selected").text();
        var EntityName = cmn_getddldataasstring("#ddl_EntityName");
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
       
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/MISGatePassDetail/DataSearch_Search",
            data: {
                Source_type: Source_type,
                Entity_type: Entity_type,
                Entity_id: Entity_id,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
               
                $('#PartialMISGatePassDetailOutwardANDinward').html(data);
               
               
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }

        });
    } catch (err) {
        debugger;
        console.log("ItemSetup Error : " + err.message);
    }
   
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hfItemID").val();
    ItemInfoBtnClick(ItmCode);
}