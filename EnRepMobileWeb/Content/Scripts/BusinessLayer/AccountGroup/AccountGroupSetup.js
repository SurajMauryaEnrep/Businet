$(document).ready(function () {
    debugger;
    $("#parent_group_name").select2();
    $("#alt_grp_id").select2();
    //GetAllAccGrp();
   // DefaultAccGroupSetup();
    //var PageName = sessionStorage.getItem("MenuName");
    //$('#AccGrpListPageName').text(PageName);

    $(this).on('click', '.simpleTree-label', function (e) {
        debugger;
        //var Parent = this.innerText;
        var Parent = this.nextSibling.innerText;
        GetMenuData(Parent)
    });
    $("#btn_forward").attr('onclick', '');
    $("#btn_forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#btn_approve").attr('onclick', '');
    $("#btn_approve").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#btn_workflow").attr('onclick', '');
    $("#btn_workflow").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#btn_print").attr('onclick', '');
    $("#btn_print").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    HideShowgrptype_option();
});
function GetAllAccGrp() {
    debugger;
    var RequestedUrl = window.location.protocol + "//" + window.location.host + "/BusinessLayer/AccountGroupSetup/GetAllAccGrp";
    $.ajax({
        type: 'POST',
        url: RequestedUrl,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        data: JSON.stringify(),
        success: function (Objdata) {
            //debugger;
            //if (data == 'ErrorPage') {
            //    ErrorPage();
            //    return false;
            //}

            if (Objdata !== null && Objdata !== "") {
                var arr = [];
                arr = JSON.parse(Objdata);

                FinalData = []
                $.each(arr, function (i, n) {
                    FinalData.push(n);
                });

                debugger;

                var options = {
                    // Optionally provide here the jQuery element that you use as the search box for filtering the tree. simpleTree then takes control over the provided box, handling user input
                    searchBox: $('#tree_menu'),

                    // Search starts after at least 3 characters are entered in the search box
                    searchMinInputLength: 1,

                    // Number of pixels to indent each additional nesting level
                    indentSize: 25,

                    // Show child count badges?
                    childCountShow: true,

                    // Symbols for expanded and collapsed nodes that have child nodes
                    symbols: {
                        collapsed: '▶',
                        expanded: '▼'
                    },

                    // these are the CSS class names used on various occasions. If you change these names, you also need to provide the corresponding CSS class
                    css: {
                        childrenContainer: 'simpleTree-childrenContainer',
                        highlight: 'simpleTree-highlight',
                        indent: 'simpleTree-indent',
                        label: 'simpleTree-label',
                        mainContainer: 'simpleTree-mainContainer',
                        nodeContainer: 'simpleTree-nodeContainer',
                        selected: 'simpleTree-selected',
                        toggle: 'simpleTree-toggle'
                    },
                };
                debugger;
               // $('#mytree').simpleTree(options, FinalData);
                $('#mytree').val(FinalData);
            }

        }

    }); 
}
function SaveAccGrp() {
   
    InsertAccGroupDetail();
}
function EnableEditBtn() {
        debugger;
        sessionStorage.removeItem("formMode");
        sessionStorage.setItem("formMode", "Update");
        var group_name = $('#acc_group_name').val().trim();
        EditAccGroupSetup();
}
function BackBtn() {
    sessionStorage.removeItem("formMode");
    window.location.href = '/Dashboard/Home/';
}
function InsertAccGroupDetail() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 03-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#btn_save").prop("disabled", true); /*End*/
        return false;
    }
   // var FormMode = sessionStorage.getItem("formMode");
    try {
        //if (FormMode == "Update") {
        //    FormMode = "1";
        //} else {
        //    FormMode = "0"
        //}
       
            var Flag = 'N';

            if ($('#acc_group_name').val().trim() == '') {
                $("#acc_group_name").attr("style", "border-color: #ff0000;");
               /* $("[aria-labelledby='select2-parent_group_name-container']").css("border-color", "red");*/
                $("#span_acc_group_name").text($("#valueReq").text());
                $("#span_acc_group_name").css("display", "block");
                Flag = 'Y';
            
        }
        if ($('#ddlgrp_type').val().trim() == '' || $('#ddlgrp_type').val().trim() == '0') {
            $("#ddlgrp_type").attr("style", "border-color: #ff0000;");
            /* $("[aria-labelledby='select2-parent_group_name-container']").css("border-color", "red");*/
            $("#span_ddlgrp_type").text($("#valueReq").text());
            $("#span_ddlgrp_type").css("display", "block");
            Flag = 'Y';
        }
            if ($('#parent_group_name').val().trim() == '-1') {
               /* $("#parent_group_name").attr("style", "border-color: #ff0000;");*/
                $("[aria-labelledby='select2-parent_group_name-container']").css("border-color", "red");
                $("#span_acc_par_group_name").text($("#valueReq").text());
                $("#span_acc_par_group_name").css("display", "block");
                Flag = 'Y';
                return false;
            }
        if (Flag == 'Y') {
            return false;
        }
        else {
            $("#btn_save").css("filter", "grayscale(100%)");
            $("#hdnSavebtn").val("AllreadyClick");
            return true;
        }
    } catch (err) {
        console.log("InsertAccGroupDetail Error : " + err.message);
    }
}
function GetSelectedParentDetail() {
    DdlHideShowgrp_typeoption();
    $("#span_acc_par_group_name").css("display", "none");
    $("[aria-labelledby='select2-parent_group_name-container']").css("border-color", "#ced4da");

}
function OnChangeGrpType() {
    $("#span_ddlgrp_type").css("display", "none");
    $("#ddlgrp_type").css("border-color", "#ced4da");
}
function HideShowgrptype_option() {
    debugger;
    var pgrp_val = $("#parent_group_name").val();
    var grp_id = pgrp_val.substring(0, 3);
    //$("#grp_type").val("0");

    if (grp_id == "101" || grp_id == "201") {
        $('#ddlgrp_type option[value="D"]').hide();
        $('#ddlgrp_type option[value="I"]').hide();
        $('#ddlgrp_type option[value="CA"]').show();
        $('#ddlgrp_type option[value="CU"]').show();
        $('#ddlgrp_type option[value="NC"]').show();
    }
    if (grp_id == "301" || grp_id == "401") {
        $('#ddlgrp_type option[value="D"]').show();
        $('#ddlgrp_type option[value="I"]').show();
        $('#ddlgrp_type option[value="CA"]').hide();
        $('#ddlgrp_type option[value="CU"]').hide();
        $('#ddlgrp_type option[value="NC"]').hide();
    }
}
function DdlHideShowgrp_typeoption() {
    debugger;
    var pgrp_val = $("#parent_group_name").val();
    var grp_id = pgrp_val.substring(0, 3);
    $("#ddlgrp_type").val("0");

    if (grp_id == "101" || grp_id == "201") {
        $('#ddlgrp_type option[value="D"]').hide();
        $('#ddlgrp_type option[value="I"]').hide();
        $('#ddlgrp_type option[value="CA"]').show();
        $('#ddlgrp_type option[value="CU"]').show();
        $('#ddlgrp_type option[value="NC"]').show();
    }
    if (grp_id == "301" || grp_id == "401") {
        $('#ddlgrp_type option[value="D"]').show();
        $('#ddlgrp_type option[value="I"]').show();
        $('#ddlgrp_type option[value="CA"]').hide();
        $('#ddlgrp_type option[value="CU"]').hide();
        $('#ddlgrp_type option[value="NC"]').hide();
    }
    getgrp_typedata(pgrp_val);
}

function getgrp_typedata(grp_val) {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/AccountGroupSetup/get_grptype",
                data: { grp_val: grp_val},
                success: function (data) {
                    debugger;
                    var grp_type = data.trim();
                   
                    $("#ddlgrp_type").val(grp_type);
                    $("#grp_type").val(grp_type);
                    $('#ddlgrp_type option[value="' + grp_type + '"]').show();

                    if (grp_val.length == 3) {
                        $("#ddlgrp_type").removeAttr("disabled");
                        $("#ddlgrp_type").attr("onclick", "OnChangeGrpType()");
                    }
                    else {
                        $("#ddlgrp_type").attr("disabled", "disabled");
                        $("#ddlgrp_type").removeAttr("onclick");
                    }

                    OnChangeGrpType();
                },
            });
    } catch (err) {
        console.log("ItemGroupSetup Error : " + err.message);
    }
}

function DefaultAccGroupSetup() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/AccountGroup/GetDefaultAccGrp",
                data: {},
                success: function (data) {
                    debugger;
                    if (data == 'ErrorPage') {
                        ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);

                        //DisabledToolBar();

                        if (arr.Table[0].CreatedBy != null) {
                            $("#CreatedBy").text(arr.Table[0].CreatedBy);
                        }
                        if (arr.Table[0].CreatedOn != null) {
                            $("#CreatedOn").text(arr.Table[0].CreatedOn);
                        }
                        if (arr.Table[0].ModifiedBy != null) {
                            $("#ModifiedBy").text(arr.Table[0].ModifiedBy);
                        }
                        if (arr.Table[0].ModifiedOn != null) {
                            $("#ModifiedOn").text(arr.Table[0].ModifiedOn);
                        }

                        Hidevalidation();

                        $('#acc_group_name').val(arr.Table[0].acc_group_name).attr("disabled", true);
                        $('#acc_grp_id').val(arr.Table[0].acc_grp_id).attr("disabled", true);
                        $('#parent_group_name option[value=' + arr.Table[0].AccParId + ']').prop('selected', true);
                        $("#parent_group_name").attr('disabled', true);
                        $('#alternative_group_name option[value=' + arr.Table[0].alt_grp_id + ']').prop('selected', true);
                        $("#alternative_group_name").attr('disabled', true);
                        var Grptype = arr.Table[0].grp_type.toString();

                        //if (Grptype === "I") {
                        //    $("#I").prop("checked", true).attr("disabled", true);
                        //}
                        //if (Grptype === "D") {
                        //    $("#D").prop("checked", true).attr("disabled", true);
                        //}
                        //if (Grptype === "N") {
                        //    $("#N").prop("checked", true).attr("disabled", true);
                        //}

                        $('#sequence_number').val(arr.Table[0].grp_seq_no).attr("disabled", true);

                       
                    }
                    $("#btn_add_new_item").attr('onclick', 'NewAccGroupSetup()');
                    $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                    $("#btn_edit_item").attr('onclick', 'EnableEditBtn()');
                    $("#btn_edit_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                    $("#btn_delete").attr('onclick', 'DeleteAccGroup()');
                    $("#btn_delete").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                    $("#btn_clear_item").attr('onclick', 'AccGroupSetup()');
                    $("#btn_clear_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                    $("#btn_save").attr('onclick', '');
                    $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    $("#btn_back").attr('onclick', 'BackBtn()');
                    $("#btn_back").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                    
                },
            });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}
function GetMenuData(acc_grp_id) {
    debugger;
    window.location.href = "/BusinessLayer/AccountGroupSetup/AccountGroupSetupView/?AccGrpId=" + acc_grp_id;
}
function NewAccGroupSetup() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/AccountGroup/AccountGroup",
                data: {},
                success: function (data) {
                    
                    AccGrpClearScreen();
                    AccGrpEnableScreen();
                    $('#acc_grp_id').val('0')
                    $("#btn_add_new_item").attr('onclick', '');
                    $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    $("#btn_edit_item").attr('onclick', '');
                    $("#btn_edit_item").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    $("#btn_back").attr('onclick', '');
                    $("#btn_back").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    $("#btn_save").attr('onclick', 'SaveAccGrp()');
                    $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                    $("#btn_delete").attr('onclick', '');
                    $("#btn_delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    //DisabledToolBar();
                    //$('#btn_save').on('click', function () {
                    //    debugger;
                    //    InsertAccGroupDetail("Insert");
                    //});
                    //$("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                    $("#btn_clear_item").attr('onclick', 'AccGroupSetup()');
                    $("#btn_clear_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                    //$('#btn_clear_item').on('click',
                    //    function () {
                    //        AccGroupSetup();
                    //    });
                    //$("#btn_clear_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
                    $("#acc_group_name").focus();
                },
            });
    } catch (err) {
        console.log("ItemGroupSetup Error : " + err.message);
    }
}
function SavedGroupSetup() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/AccountGroup/AccountGroup",
                data: {},
                success: function (data) {

                    $("#rightPageContent").empty().html(data);
                    debugger;
                     $("#acc_group_name").focus();
                },
            });
    } catch (err) {
        console.log("ItemGroupSetup Error : " + err.message);
    }
}

function EditAccGroupSetup() {
    debugger;
    //var AccGrpId = acc_grp_id;
    AccGrpEnableScreen();
    $("#btn_add_new_item").attr('onclick', '');
    $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#btn_edit_item").attr('onclick', '');
    $("#btn_edit_item").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#btn_delete").attr('onclick', '');
    $("#btn_delete").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    $("#btn_clear_item").attr('onclick', 'AccGroupSetup()');
    $("#btn_clear_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    $("#btn_save").attr('onclick', 'InsertAccGroupDetail()');
    $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    $("#btn_back").attr('onclick', '');
    $("#btn_back").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

    //try {
    //    $("#acc_group_name").attr('disabled', false);
    //    $("#acc_group_name").focus();
    //    $("#parent_group_name").attr('disabled', false);
    //    $("#I").attr('disabled', false);
    //    $("#D").attr('disabled', false);
    //    $("#N").attr('disabled', false);
    //    $("#alternative_group_name").attr('disabled', false);
    //    $("#sequence_number").attr('disabled', false);
    //    debugger;
    //    DisabledToolBar();
    //    $('#btn_add_new_item').off('click');
    //    $("#btn_add_new_item").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //    $('#btn_edit_item').off('click');
    //    $("#btn_edit_item").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //    $('#btn_save').on('click', function () {
    //        InsertAccGroupDetail("Update");
    //    });
    //    $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    //    $('#btn_clear_item').on('click',
    //        function () {
    //            AccGroupSetup();
    //        });
    //    $("#btn_clear_item").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    //    $("#acc_group_name").focus();

    //} catch (err) {
    //    console.log("GetMenuData Error : " + err.message);
    //}

}

function AccGroupSetup() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/AccountGroup/AccountGroup",
                data: {},
                success: function (data) {
                    debugger;
                    $("#rightPageContent").empty().html(data);
                    sessionStorage.removeItem("formMode");
                    DefaultAccGroupSetup();

                },
            });
    } catch (err) {
        console.log("ItemGroupSetup Error : " + err.message);
    }
}
function OnChangeGroupName() {
    $("#acc_group_name").attr("style", "border-color: #ced4da;");
    $("#span_acc_group_name").attr("style", "display: none;");
}
function OnChangeParGroupName() {
    //$("#parent_group_name").attr("style", "border-color: #ced4da;");
    //$("#span_acc_par_group_name").attr("style", "display: none;");
    $("#span_acc_par_group_name").css("display", "none");
    $("[aria-labelledby='select2-parent_group_name-container']").css("border-color", "#ced4da");
}
function Hidevalidation() {

    $("#acc_group_name").attr("style", "border-color: #ced4da;");
    $("#span_acc_group_name").attr("style", "display: none;");
    /* $("#parent_group_name").attr("style", "border-color: #ced4da;");*/
    $("[aria-labelledby='select2-parent_group_name-container']").css("border-color", "#ced4da");
    $("#span_acc_par_group_name").attr("style", "display: none;");
};
function DeleteAccGroup() {
    debugger;
    var AccGrpID = $('#acc_grp_id').val().trim();
    try {
        swal({
            title: $("#deltital").text(),
            text: $("#deltext").text() + "!",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Yes, delete it!",
            closeOnConfirm: false
        },
            function () {
                $.ajax(
                    {
                        type: "POST",
                        url: "/BusinessLayer/AccountGroup/DeleteAccGroup",
                        data: { AccGrpID: AccGrpID },
                        success: function (data) {
                            debugger;
                            if (data == 'ErrorPage') {
                                ErrorPage();
                                return false;
                            }
                            if (data == "-1") {
                                swal("", $("#DependenciesExist").text(), "warning");
                                return false;
                            }
                            else {
                                
                                swal("", $("#deletemsg").text(), "success");
                                //NewAccGroupSetup();
                                //DefaultAccGroupSetup();
                                AccGroupSetup();
                                
                            }

                            //$("#rightPageContent").empty().html(data);
                        },
                    });
            });



    } catch (err) {
        console.log("ItemGroupSetup Error : " + err.message);
    }

}

function AccGrpClearScreen() {
    debugger;
    $('#acc_group_name').val('')
    $('#parent_group_name').val('0')
    $('#alternative_group_name').val('0')
    //$('#alternative_group_name').empty().append('<option value="0" selected="selected">---Select---</option>');
    //$("#I").prop("disabled", false);
    //$("#N").prop("checked", true);
    //$("#D").prop("disabled", false);
    $('#sequence_number').val('')
    $("#CreatedBy").text('');
    $("#CreatedOn").text('');
    $("#ModifiedBy").text('');
    $("#ModifiedOn").text('');


}
function AccGrpEnableScreen() {
    debugger;
    $("#acc_group_name").attr("disabled", false);
    $("#parent_group_name").attr('disabled', false);
    $("#alternative_group_name").attr('disabled', false);
    // $("#I").prop("disabled", false);
    //$("#N").prop("disabled", false);
    //$("#D").prop("disabled", false);
    $("#sequence_number").attr("disabled", false);

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

//function CheckGroupDependency() {
//    debugger;
//    try {
//        var PGroupID = $('#acc_groupid').val().trim();
//        if (PGroupID != "") {
//            $.ajax(
//                {
//                    type: "POST",
//                    url: "/BusinessLayer/AccountGroupSetup/Check_GroupDependency",
//                    data: { AccGroupID: PGroupID },
//                    success: function (data) {
//                        debugger;
//                        if (data == 'ErrorPage') {

//                            ErrorPage();
//                            return false;
//                        }
//                    },
//                });
//        }
//        return true;

//    } catch (err) {
//        console.log("CheckAccGroupDetail Error : " + err.message);
//    }
//}