
$(document).ready(function () {
    Cmn_rowHighLight();
    $(".remarksmessage").attr('onmouseover', 'OnMouseOver(this)');
    $(".itmfrz >tbody").bind("click", function (e) {
        //debugger;
        var clickedrow = $(e.target).closest("tr");
        $(".itmfrz >tbody >tr").css("background-color", "#ffffff");
        $(".itmfrz >tbody >tr > #tditem").css("background-color", "#ffffff");
        $(".itmfrz >tbody >tr > #tditem").css("background-image", "");
        $(".itmfrz >tbody >tr > .tditemfrz").css("background-color", "#ffffff");
        $(".itmfrz >tbody >tr > .tditemfrz").css("background-image", "");

        $(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");
        clickedrow.find("#tditem").css("background-color", "rgba(210, 239, 233)");
        clickedrow.find(".tditemfrz").css("background-color", "rgba(220, 244, 239)");
    });

    $(".RowHighLightMultiTable >tbody").bind("click", function (e) {
        var clickedrow = $(e.target).closest("tr");
        //debugger;
        $(e.target).closest("tr").parent().find("tr").css("background-color", "#ffffff");
        var hfopid = $(e.target).closest("tr").find("#hfopid").val();
        $(clickedrow).parent().find("tr").each(function () {
            var tdlen = $(this).find("td").length;
            if (tdlen == 6) {
                $(this).find("td:eq(2),td:eq(3),td:eq(4),td:eq(5)").css("background-color", "");
            }
        });
        $(clickedrow).parent().find("tr #hfopid[value=" + hfopid + "]").closest("tr").css("background-color", "rgba(38, 185, 154, .16)");

        if ($(clickedrow).find("td").length == 6) {
            if ($(e.target).closest("td").prop("rowspan") == 1) {
                $(clickedrow).find("td:eq(2),td:eq(3),td:eq(4),td:eq(5)").css("background-color", "rgba(38, 185, 154, .115)");
            }

        }
        else {
            $(clickedrow).css("background-color", "rgba(38, 185, 154, .24)");
        }
        //$(clickedrow).find("#ItmInfoBtnIcon").focus();
        //clickdownAndUp();
    });
    $(".rowhighlighttbl >tbody").bind("click", function (e) {
        var clickedrow = $(e.target).closest("tr");
        //debugger;
        $(".rowhighlighttbl >tbody >tr").css("background-color", "#ffffff");
        $(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");      
    });

    /*--------------------Created by Suraj on 10-10-2024 for Row HighLight in case Multi Table Cost Center Details ---------------------*/
    /*Used in : Cmn_PartialCostCenterDetailDisplay.cshtml*/
    $(".rowhighlightMultiTable tbody").bind("click", function (e) {
        var clickedrow = $(e.target).closest("tr");
        //debugger;
        if (clickedrow[0].className == "parent-row") {
            $(".rowhighlighttblNew tbody tr").css("background-color", "#ffffff");
            $(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");
            clickedrow.find("table tr").css("background-color", "rgba(38, 185, 154, .0)");
        }
        
    });

    $(".rowhighlighttblInnerTable tbody").bind("click", function (e) {
        var clickedrow = $(e.target).closest('.parent-row');
        var cRow = $(e.target).closest('tr');
        //debugger;
        if (cRow[0].className == "inner-row") {
            $(".rowhighlighttblNew tbody tr").css("background-color", "#ffffff");
            $(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");
            $(clickedrow).find("table tr").css("background-color", "rgba(38, 185, 154, 0)");
            $(cRow).css("background-color", "rgba(38, 185, 154, 0.16)");
        }
    });
    /*--------------------Created by Suraj on 10-10-2024 for Cost Center Details ---------------------*/
});
