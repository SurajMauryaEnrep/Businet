$(function () {
    var
        $table = $('#tree-table'),
        rows = $table.find('tr');

    rows.each(function (index, row) {
        var
            $row = $(row),
            level = $row.data('level'),
            id = $row.data('id'),
            $columnName = $row.find('td[data-column="name"]'),
            children = $table.find('tr[data-parent="' + id + '"]');

        if (children.length) {
            debugger;
         
                var expander = $columnName.prepend('' +
                    '<span class="treegrid-expander glyphicon glyphicon-chevron-right"></span>' +
                    '');

                children.hide();
            
            expander.on('click', function (e) {
                debugger
                var $target = $(e.target);
                if ($target.hasClass('glyphicon-chevron-right')) {
                    $target
                        .removeClass('glyphicon-chevron-right')
                        .addClass('glyphicon-chevron-down');
                    children.show();
                } else {
                    if ($target.hasClass('glyphicon-chevron-down')) {
                        $target.removeClass('glyphicon-chevron-down')
                            .addClass('glyphicon-chevron-right');
                    }
                    reverseHide($table, $row);
                }
            });
        }

        $columnName.prepend('' +
            '<span class="treegrid-indent" style="width:' + 15 * level + 'px"></span>' +
            '');
    });

    // Reverse hide all elements
    reverseHide = function (table, element) {
        var
            $element = $(element),
            id = $element.data('id'),
            children = table.find('tr[data-parent="' + id + '"]');

        if (children.length) {
            children.each(function (i, e) {
                reverseHide(table, e);
            });

            $element
                .find('.glyphicon-chevron-down')
                .removeClass('glyphicon-chevron-down')
                .addClass('glyphicon-chevron-right');

            children.hide();
        }
    };
});
$(function () {
    var
        $table = $('#tree-table1'),
        rows = $table.find('tr');

    rows.each(function (index, row) {
        var
            $row = $(row),
            level = $row.data('level'),
            id = $row.data('id'),
            $columnName = $row.find('td[data-column="name"]'),
            children = $table.find('tr[data-parent="' + id + '"]');

        if (children.length) {
            debugger;
            if (level == 1) {
                var expander = $columnName.prepend('' +
                    '<span class="treegrid-expander glyphicon glyphicon-chevron-down"></span>' +
                    '');

                children.show();
            }
            else {
                var expander = $columnName.prepend('' +
                    '<span class="treegrid-expander glyphicon glyphicon-chevron-right"></span>' +
                    '');

                children.hide();
            }

            expander.on('click', function (e) {
                debugger
                var $target = $(e.target);
                if ($target.hasClass('glyphicon-chevron-right')) {
                    $target
                        .removeClass('glyphicon-chevron-right')
                        .addClass('glyphicon-chevron-down');
                    children.show();
                } else {
                    if ($target.hasClass('glyphicon-chevron-down')) {
                        $target.removeClass('glyphicon-chevron-down')
                            .addClass('glyphicon-chevron-right');
                    }
                    reverseHide($table, $row);
                }
            });
        }

        $columnName.prepend('' +
            '<span class="treegrid-indent" style="width:' + 15 * level + 'px"></span>' +
            '');
    });

    // Reverse hide all elements
    reverseHide = function (table, element) {
        var
            $element = $(element),
            id = $element.data('id'),
            children = table.find('tr[data-parent="' + id + '"]');

        if (children.length) {
            children.each(function (i, e) {
                reverseHide(table, e);
            });

            $element
                .find('.glyphicon-chevron-down')
                .removeClass('glyphicon-chevron-down')
                .addClass('glyphicon-chevron-right');

            children.hide();
        }
    };
});
$(function () {
    var
        $table = $('#tree-table2'),
        rows = $table.find('tr');

    rows.each(function (index, row) {
        var
            $row = $(row),
            level = $row.data('level'),
            id = $row.data('id'),
            $columnName = $row.find('td[data-column="name"]'),
            children = $table.find('tr[data-parent="' + id + '"]');

        if (children.length) {
            debugger;
            if (level == 1) {
                var expander = $columnName.prepend('' +
                    '<span class="treegrid-expander glyphicon glyphicon-chevron-down"></span>' +
                    '');

                children.show();
            }
            else {
                var expander = $columnName.prepend('' +
                    '<span class="treegrid-expander glyphicon glyphicon-chevron-right"></span>' +
                    '');

                children.hide();
            }

            expander.on('click', function (e) {
                debugger
                var $target = $(e.target);
                if ($target.hasClass('glyphicon-chevron-right')) {
                    $target
                        .removeClass('glyphicon-chevron-right')
                        .addClass('glyphicon-chevron-down');
                    children.show();
                } else {
                    if ($target.hasClass('glyphicon-chevron-down')) {
                        $target.removeClass('glyphicon-chevron-down')
                            .addClass('glyphicon-chevron-right');
                    }
                    reverseHide($table, $row);
                }
            });
        }

        $columnName.prepend('' +
            '<span class="treegrid-indent" style="width:' + 15 * level + 'px"></span>' +
            '');
    });

    // Reverse hide all elements
    reverseHide = function (table, element) {
        var
            $element = $(element),
            id = $element.data('id'),
            children = table.find('tr[data-parent="' + id + '"]');

        if (children.length) {
            children.each(function (i, e) {
                reverseHide(table, e);
            });

            $element
                .find('.glyphicon-chevron-down')
                .removeClass('glyphicon-chevron-down')
                .addClass('glyphicon-chevron-right');

            children.hide();
        }
    };
});