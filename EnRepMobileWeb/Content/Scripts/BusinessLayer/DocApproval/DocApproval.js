
$(document).ready(function () {
    debugger;

    $("#DocAppListTbody #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {

            var clickedrow = $(e.target).closest("tr");
            var br_id = clickedrow.children("#hdnbrid").text();
            var doc_id = clickedrow.children("#hdndocid").text();
            window.location.href = "/BusinessLayer/DocApproval/AddApprovalDetail?br_id=" + br_id + "-" + doc_id + "";
        }
        catch (err) {
            debugger
            alert(err.message);
        }
    });
    //$("#Doc").select2();
    var fieldDL = "#DocOnList";
    var valueDL = "#doc_searchDL";
    autocomplete(fieldDL, valueDL)
    debugger;

    if ($("#BranchName").val()=="") {
        $("#Doc").prop("disabled", true);
    }
    debugger
    //var abc = $("#BranchName").val();
   // $("#Doc").prop('disabled',true)


    $("#BranchName").on("change", function () {   
        autocompleteuser();
        $("#Doc").val("");
        var field = "#Doc";
        var value = "#doc_search";
        var branch_id = $("#BranchName").val()
        debugger;       
        autocompleteDocDetails(field, value, branch_id);
        if ($("#BranchName").val() == "0") {
            $("#Doc").prop("disabled", true);
            $("#Doc").val("");
        }
        else {
            debugger;
            $("#Doc").prop("disabled", false);
                    
        }
       
    })

    $('#DocAppTBody').on('click', '.deleteIcon', function () {
        debugger;
      
        var child = $(this).closest('tr').nextAll();
     
        child.each(function () {
            // Getting <tr> id. 
            debugger;
            var id = $(this).attr('id');
            // Getting the <p> inside the .row-index class. 
            var idx = $(this).children('.row-index').children('p');
            // Gets the row number from <tr> id. 
            var dig = parseInt(id.substring(1));
            // Modifying row index. 
            idx.html(`Row ${dig - 1}`);
            // Modifying row id. 
            $(this).attr('id', `R${dig - 1}`);
        });
        // Removing the current row. 
        $(this).closest('tr').remove();
      
        $('#UserName').empty().append('<option value="0" selected="selected">---Select---</option>');
     
        ResetDocLevelVal();
    
       
    });
  
    ResetDocLevelVal();


 
})

function FilterListDocApp() {
    debugger;
    var Br_id = $("#BranchNameDL").val();
    var Doc_id = $("#DocOnList").val();
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/DocApproval/DocAppList",/*Controller=ItemSetup and Fuction=GetItemList*/
        data: {
            Br_id: Br_id,
            Doc_id: Doc_id,

        },
        success: function (data) {
            debugger;
            $('#DocAppListTbody').html(data);
        },


    });
}
function InsertDocAppDetail() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Adddd this NItesh 02-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btnSave").css("filter", "grayscale(100%)");
        $("#btnSave").prop("disabled", true); /*End*/
    }

    var levels = [];
    var br_id = $("#BranchName").val();
    var doc_id = $("#Doc").val();
    $("#hdnBranch_id").val(br_id);
    $("#hdnDoc_id").val(doc_id);
    var ValidInfo = "N";
       
    if ($('#BranchName').val() == "") {
        ValidInfo = "Y";        
        $("#BranchName").css("border-color", "red");
        $('#vmbranchname').text($("#valueReq").text());
    }
    if ($('#Doc').val() == "" || $('#Doc').val() == "0") {
        ValidInfo = "Y";
        debugger;
        $("#Doc").css("border-color", "red");
        $("[aria-labelledby='select2-Doc-container']").css("border-color", "red");
        $('#vmdoc').text($("#valueReq").text());      
    }
    //if ($('#Level').val() == "0") {
    //    ValidInfo = "Y";
    //    $("#Level").css("border-color", "red");
    //    $('#vmlevel').text($("#valueReq").text());
    //    $("#vmlevel").css("display", "block");
    //}
    //if ($('#UserName').val() == "" || $('#UserName').val() == "0") {
    //    ValidInfo = "Y";
    //    debugger;
    //    $("#UserName").css("border-color", "red");
    //    $("[aria-labelledby='select2-UserName-container']").css("border-color", "red");
    //    $('#vmusername').text($("#valueReq").text());
    //    $("#vmusername").css("display", "block");
    //}
    if (ValidInfo == "Y") {
        return false;
    }
    
    $("#DocAppTBody tr").each(function () {
        debugger;
        var currentRow = $(this);
        //levels.push(currentRow.find("td:eq(2)").text());
        levels.push(currentRow.find("#hdnlevel").text());
    });
    if (levels.length == 0) {
        swal("", $("#Userdetailsnotfound").text(), "warning");
        return false;
    }
    if (levels.length > 0) {
        debugger;
        var a;
        var j;
        var maxlevel = Math.max(...levels);
        for (a = 1; a <= maxlevel; a++) {
            for (var k = 0; k < maxlevel; k++) {

                levels = $.grep(levels, function (elm, idx) {
                    return idx == levels.indexOf(elm)
                });

                if (a == levels[k]) {
                    j = levels[k];
                    break;
                }
            }
            if (a == j) {
                //return true;
            }
            else {
                j='N'
                break;
                }
            
        }
        if (j == 'N') {
            swal("", $("#Levelcannotbeskipped").text(), "warning");
            return false;
         
        }
       
    }

        FinalItemDetail = InsertDOCDetails();

        debugger;
        var DocuserDt = JSON.stringify(FinalItemDetail);
    $('#hdnUserlist').val(DocuserDt);

    $("#btnSave").css("filter", "grayscale(100%)");
    $("#hdnSavebtn").val("AllreadyClick");
        return true;
    
};

function InsertDOCDetails() {
    debugger;
   
    var br_id = $("#BranchName").val();
    var doc_id = $("#Doc").val();
    $("#hdnBranch_id").val(br_id);
    $("#hdnDoc_id").val(doc_id);
    var DocList = [];
    $("#DocAppTBody tr").each(function () {

        var currentRow = $(this);
        //var level = currentRow.find("td:eq(1)").text();
        //var userID = currentRow.find("td:eq(4)").text();
        //var limit = currentRow.find("td:eq(5)").text();  
        var level = currentRow.find("#levelid").text();
        var userID = currentRow.find("#hdnuser").text();
        var limit = currentRow.find("#limitid").text();

        DocList.push({ level: level, userID: userID, limit: limit, })
    });

    return DocList;
  
};

function AllowOnlyNumValue(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    else {
        return true;
    }
}

function AddUserDetails() {
    var rowIdx = 0;
    debugger;

    var ValidInfo = "N";
   
    if ($('#Level').val() == "0") {
        ValidInfo = "Y";
        $('#vmlevel').text($("#valueReq").text());
        $("#vmlevel").css("display", "block");
        $("#Level").css("border-color", "red");
    }
    if ($('#UserName').val() == "" || $('#UserName').val() == "0") {
        ValidInfo = "Y";
        $('#vmusername').text($("#valueReq").text());
        $("#vmusername").css("display", "block");
        $("#UserName").css("border-color", "red");
        $("[aria-labelledby='select2-UserName-container']").css("border-color", "red");
        
    }
    if ($('#BranchName').val() == "0") {
        ValidInfo = "Y";
       
        $("#BranchName").css("border-color", "red");
        $('#vmbranchname').text($("#valueReq").text());
        
    }
    if ($('#Doc').val() == "") {
        ValidInfo = "Y";
      
        $("#Doc").css("border-color", "red");
        $("[aria-labelledby='select2-Doc-container']").css("border-color", "red");
        $('#vmdoc').text($("#valueReq").text());
        
    }
   
    if (ValidInfo == "Y") {
        return false;
    }
    $("#BranchName").prop('disabled', true);
    $("#Doc").prop('disabled', true);

    var levelID = $("#Level").val();
    var username = $("#UserName").val();
    var limit = $("#Limit").val();
    var Title = $("#hf_deletetitle").val();
    if (levelID != "0" && username !== "0") {
        $('#DocAppTBody').append(`<tr id="R${++rowIdx}"> 
<td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
<td class=" " id="levelid">${levelID}</td>
<td class=" " hidden="hidden" id="hdnlevel">${$("#Level").val()}</td>
<td class=" " id="usernmid">${$("#UserName option:selected").text()}</td>
<td class=" " hidden="hidden" id="hdnuser">${$("#UserName").val()}</td>
<td class=" " id="limitid">${$("#Limit").val()}</td>
              </tr>`);

    
        $("#Limit").val("");
        $('#UserName').empty().append('<option value="0" selected="selected">---Select---</option>');

        
    }
 
    sortTable();
    sortTableuser();
    ResetDocLevelVal();
}


function sortTableuser() {
    var table, rows,rows1, switching, i, x, y,x1,y1, shouldSwitch;
    table = document.getElementById("tblShorting");
    switching = true;
  
    while (switching) {
   
        switching = false;
        rows = table.rows;
        rows1 = table.rows;
        for (i = 1; i < (rows.length - 1); i++) {
            debugger;
            shouldSwitch = false;
            x1 = rows1[i].getElementsByTagName("TD")[1];
            y1 = rows1[i + 1].getElementsByTagName("TD")[1];
            if (x1.innerHTML.toLowerCase() == y1.innerHTML.toLowerCase()) {
                x = rows[i].getElementsByTagName("TD")[3];
                y = rows[i + 1].getElementsByTagName("TD")[3];

                if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {

                    shouldSwitch = true;
                    break;
                }
            }
           
        }
        if (shouldSwitch) {
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
        }
    }
}
function sortTable() {
    var table, rows, switching, i, x, y,x1,y1, shouldSwitch;
    table = document.getElementById("tblShorting");
    switching = true;
    /*Make a loop that will continue until
    no switching has been done:*/
    while (switching) {
        //start by saying: no switching is done:
        switching = false;
        rows = table.rows;
        /*Loop through all table rows (except the
        first, which contains table headers):*/
        for (i = 1; i < (rows.length - 1); i++) {
            //start by saying there should be no switching:
            shouldSwitch = false;
            /*Get the two elements you want to compare,
            one from current row and one from the next:*/
            x = rows[i].getElementsByTagName("TD")[1];
            y = rows[i + 1].getElementsByTagName("TD")[1];
           
            //check if the two rows should switch place:
            if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {
              
                //if so, mark as a switch and break the loop:
                shouldSwitch = true;
                break;
            }
        }
        if (shouldSwitch) {
            /*If a switch has been marked, make the switch
            and mark that a switch has been done:*/
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
        }
    }
}

function SaveValidationDocApp() {
    var level = [];
    var maxval;
    $("#DocAppTBody tr").each(function () {
        debugger;
        var currentRow = $(this);
        //level = currentRow.find("td:eq(2)").text();
        level = currentRow.find("#hdnlevel").text();
    });

}


function OnChangeBranchName() {
    
    var val1 = $('#BranchName').val().trim();
    if (val1 != "0") {
        document.getElementById("vmbranchname").innerHTML = "";
        $("#BranchName").css("border-color", "#ced4da");
        validationFlag = false;
    }
    debugger;
    if ($('#Doc').val() == "" || $('#Doc').val() == "0") {
        if ($("#Doc").css("border-color") == 'rgb(73, 80, 87)' || $("#Doc").css("border-color") == 'rgb(255, 0, 0)') {

            $("[aria-labelledby='select2-Doc-container']").css("border-color", "red");
        }
    }
    if ($('#UserName').val() == "" ) {
        if ($("#UserName").css("border-color") == 'rgb(73, 80, 87)' || $("#UserName").css("border-color") == 'rgb(255, 0, 0)') {            
            $("[aria-labelledby='select2-UserName-container']").css("border-color", "red");
        }
    }
   
}
function OnChangeDocumentddl() {
    
    var val1 = $('#Doc').val().trim();
    if (val1 != "0") {
        document.getElementById("vmdoc").innerHTML = "";
        $("#Doc").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-Doc-container']").css("border-color", "#ced4da");
        validationFlag = false;

        autocompleteuser();
    }
}
function OnChangeLevelddl() {
    
    var val1 = $('#Level').val().trim();
    if (val1 != "0") {
        document.getElementById("vmlevel").innerHTML = "";
        $("#Level").css("border-color", "#ced4da");
        validationFlag = false;
    }
}
function OnChangeUserNameddl() {
   
    var val1 = $('#UserName').val().trim();
    if (val1 != "0") {
        document.getElementById("vmusername").innerHTML = "";
        $("#UserName").css("border-color", "#ced4da");

        $("[aria-labelledby='select2-UserName-container']").css("border-color", "#ced4da");
        validationFlag = false;
    }
}
function ResetDocLevelVal() {
    debugger;
    var level = [];// new Array();
    var maxval;
    $("#DocAppTBody tr").each(function () {
        debugger;
        var currentRow = $(this);        
        /*level.push(currentRow.find("td:eq(2)").text());*/
        level.push(currentRow.find("#hdnlevel").text());
    });    
    if (level.length >0) {
        maxval = Math.max(...level);
    }
    else {
        maxval = 0;
    }
    if (maxval == 0) {

        $("#Level option[value=" + 1 + "]").show();
        for (var k = 2; k <= 10; k++) {
            $("#Level option[value=" + k + "]").hide();
        }
        $('#Level').val("0").prop('selected', true);
    }
    else {
        for (i = 1; i <= maxval; i++) {
            $("#Level option[value=" + i + "]").show();
        }
        $("#Level option[value=" + i + "]").show();
        var j = i+1;
        for (var k = j; k <= 10; k++) {
            $("#Level option[value=" + k + "]").hide();
        }
        $('#Level').val("0").prop('selected', true);
    }
   
}



function autocompleteuser() {
    debugger;
    var DocName = $('#Doc').val().trim();
    var branch_id = $("#BranchName").val();
    $("#UserName").select2({

        ajax: {
            url: $("#UserName_search").val(),
            data: function (params) {
                var queryParameters = {
                    Search: params.term,
                    branch_id: branch_id,
                    DocName: DocName
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
                var AttriID = "";
                arr = new Array
                var i;


                $("#DocAppTBody tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var userid = "";
                    //userid = currentRow.find("td:eq(3)").text();
                    userid = currentRow.find("#usernmid").text();
                    for (var k = 0; k < data.length; k++) {
                        if (data[k].Name == userid) {
                            data[k].ID = null;
                            data[k].Name = null;
                            // j++;
                        }
                    }
                });

             
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
function autocompleteDocDetails(field, value, branch_id) {
    debugger;
    var branch_id = $("#BranchName").val();    
    $(field).select2({

        ajax: {
            url: $(value).val(),
            data: function (params) {
                var queryParameters = {
                    DocName: params.term,
                    branch_id: branch_id
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
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID, text: val.Name };
                    }),
                };
            },

        },

    });
}
function autocomplete(field, value) {
    debugger;    
    $(field).select2({

        ajax: {
            url: $(value).val(),
            data: function (params) {
                var queryParameters = {
                    DocName: params.term,                   
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
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID, text: val.Name };
                    }),
                };
            },

        },

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
            $("#hdnAction").val("Delete");
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}