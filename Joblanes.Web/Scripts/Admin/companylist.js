$(document).ready(function() {
    function dataTableRender() {
        var pageSize = 10;
        if ($('#pageSize').val() != "" && /^\d+$/.test($('#pageSize').val())) {
            pageSize = parseInt($('#pageSize').val());
        } else {
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
                url: $("body").attr("data-project-root") + "Admin/CompanyListData",
                type: 'POST',
                data: function(d) {
                    d.name = $('#Name').val();
                    d.zip = $('#Zip').val();
                    d.companyType = $('#CompanyType').val();
                    d.contactMobile = $('#ContactMobile').val();
                    d.region = $('#Region').val();
                    d.country = $('#Country').val();
                    d.state = $('#State').val();
                    d.city = $('#City').val();
                    d.status = $('#Status').val();
                }
            }
        });
    }

    dataTableRender();

    $(document).on("click", "#search", function() {
        dataTableRender();
    });
});
$(document).on('change', '#Region', function () {

    $('#Country').empty();
    $('#Country').append("<option value=''>Select Country</option>");
    $('#State').empty();
    $('#State').append("<option value=''>Select State</option>");
    $('#City').empty();
    $('#City').append("<option value=''>Select City</option>");
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
        $('#Country').append("<option value=''>Select Country</option>");
        $('#State').empty();
        $('#State').append("<option value=''>Select State</option>");
        $('#City').empty();
        $('#City').append("<option value=''>Select City</option>");
    }


});

$(document).on('change', '#Country', function () {

    $('#State').empty();
    $('#State').append("<option value=''>Select State</option>");
    $('#City').empty();
    $('#City').append("<option value=''>Select City</option>");

    var region = $('#Region').val();
    var country = $('#Country').val();

    if (region > 0 && country > 0) {
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "State/LoadState",

            data: { region: region, country: country },

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
                        $('#State').append($('<option>').text(v.Text).attr('value', v.Value));
                    });
                }
            },
            complete: function () {

                $.unblockUI();
            }
        });
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "City/LoadCity",

            data: { region: region, country: country },

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
                        $('#City').append($('<option>').text(v.Text).attr('value', v.Value));
                    });
                }
            },
            complete: function () {

                $.unblockUI();
            }
        });

    }
    else {
        $('#State').empty();
        $('#State').append("<option value=''>Select State</option>");
        $('#City').empty();
        $('#City').append("<option value=''>Select City</option>");
    }

});


$(document).on('change', '#State', function () {

    $('#City').empty();
    $('#City').append("<option value=''>Select City</option>");
    var region = $('#Region').val();
    var country = $('#Country').val();
    var state = $('#State').val();

    if (region > 0 && country > 0 && state > 0) {
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "City/LoadCity",

            data: { region: region, country: country, state: state },

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
                        $('#City').append($('<option>').text(v.Text).attr('value', v.Value));
                    });
                }
            },
            complete: function () {

                $.unblockUI();
            }
        });

    }
    else {
        $('#City').empty();
        $('#City').append("<option value=''>Select City</option>");
    }

});