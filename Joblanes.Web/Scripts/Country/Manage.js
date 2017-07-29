$(document).ready(function () {
    function dataTableRender() {
        var pageSize = 10;
        if ($('#pageSize').val() != "" && /^\d+$/.test($('#pageSize').val())) {
            pageSize = parseInt($('#pageSize').val());
        }
        else {
            $('#pageSize').val(pageSize);
        }
        $('#DataGrid').dataTable({
            destroy: true,
            "processing": true,
            searching: false,
            serverSide: true,
            "scrollX": true,
            "bLengthChange": false,
            "iDisplayLength": pageSize,
            //stateSave: true,
            order: [[0, "desc"]],
            ajax: {
                url: $("body").attr("data-project-root") + "Country/CountryList",
                type: 'POST',
                data: function (d) {
                    d.name = $('#Name').val();
                    d.shortName = $('#ShortName').val();
                    d.callingCode = $('#CallingCode').val();
                    d.region = $('#Region').val();
                    d.status = $('#Status').val();
                }
            }
        });
    }

    dataTableRender();

    $(document).on("click", "#search", function () {
        dataTableRender();
    });

    $(document).on("click", ".glyphicon-trash", function () {
       
        var id = $(this).attr("id");
        var name = $(this).attr("data-name");
        if (id != "") {
            bootbox.deleteConfirm("<h3>Are you sure you want to delete this <span class='confirm-message'>" + name + "</span> Country?</h3>", function (result) {
                if (result) {
                    $.ajax({
                        type: "post",
                        url: $("body").attr("data-project-root") + "Country/Delete",
                        cache: false,
                        async: true,
                        data: { "id": id },
                        beforeSend: function () {
                            $.blockUI({
                                timeout: 0,
                                message: '<h1><img src="/Content/Image/ajax-loader.gif" /> Processing...</h1>'
                            });
                        },
                        success: function (result) {
                            $.unblockUI();
                            var errorMessage = "";
                            if (result.IsSuccess) {
                                 errorMessage = '<div class="alert alert-success"><a class="close" data-dismiss="alert">×</a><strong>Success!</strong>  ' + result.Message + '</div>';
                                $('.customMessage').append(errorMessage);
                                dataTableRender();
                            }
                            else {
                                 errorMessage = '<div class="alert alert-danger"><a class="close" data-dismiss="alert">×</a>' + result.Message + '</div>';
                                $('.customMessage').append(errorMessage);
                            }
                        },
                        complete: function () {
                            $.unblockUI();
                        },
                        error: function (result) {
                            var errorMessage = '<div class="alert alert-danger"><a class="close" data-dismiss="alert">×</a>Country Delete Failed</div>';
                            $('.customMessage').append(errorMessage);
                        }
                    });

                }
            }).css({ 'margin-top': (($(window).height() / 4)) });
        }
        return false;
    });
});