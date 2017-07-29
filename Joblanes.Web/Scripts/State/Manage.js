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
                url: $("body").attr("data-project-root") + "State/StateList",
                type: 'POST',
                data: function (d) {
                    d.name = $('#Name').val();
                    d.shortName = $('#ShortName').val();
                    d.region = $('#Region').val();
                    d.country = $('#Country').val();
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
            bootbox.deleteConfirm("<h3>Are you sure you want to delete this <span class='confirm-message'>" + name + "</span> Region?</h3>", function (result) {
                if (result) {
                    $.ajax({
                        type: "post",
                        url: $("body").attr("data-project-root") + "State/Delete",
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
                            var errorMessage = '<div class="alert alert-danger"><a class="close" data-dismiss="alert">×</a>Region Delete Failed</div>';
                            $('.customMessage').append(errorMessage);
                        }
                    });

                }
            }).css({ 'margin-top': (($(window).height() / 4)) });
        }
        return false;
    });
});

$(document).on('change', '#Region', function () {

    $('#Country').empty();
    $('#Country').append("<option value='0'>All Country</option>");
    var region = $('#Region').val();

    if (region > 0) {
        $.ajax({
            type: "POST",
            // url: '@Url.Action("LoadCountry", "CommonAjax", null)',
            url: $("body").attr("data-project-root") + "Country/LoadCountry",

            data: { regions: region },

            beforeSend: function () {
                $.blockUI({
                    timeout: 0,
                    message: '<h1><img src="/Content/Image/ajax-loader.gif" /> Processing...</h1>'
                });
            },

            success: function (response) {
                $.unblockUI();
                if (response.IsSuccess) {
                    $.each(response.returnList, function (i, v) {
                        $('#Country').append($('<option>').text(v.Text).attr('value', v.Value));
                    });
                }
            },
            complete: function () {

                $.unblockUI();
            }
        });

    }
    else {
        $('#Country').empty();
        $('#Country').append("<option value='0'>All Country</option>");


    }


});