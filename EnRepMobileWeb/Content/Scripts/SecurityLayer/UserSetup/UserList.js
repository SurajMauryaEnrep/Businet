$(document).ready(function () {

    $("#UserList #datatable-buttons tbody tr").on("click", function (event) {
        debugger;
        $("#datatable-buttons tbody tr").css("background-color", "#ffffff");
        $(this).css("background-color", "rgba(38, 185, 154, .16)");
    });

    $("#UserList #datatable-buttons tbody").bind("dblclick", function (e) {

        debugger;
        try {
            var clickedrow = $(e.target).closest("tr");
            var UserId = clickedrow.children("td:eq(1)").text();
           // alert(UserId);
            window.location.href = "/SecurityLayer/UserDetail/UserDetail/?UserId=" + UserId;
        }
        catch (err) {
            debugger
        }
    });

});