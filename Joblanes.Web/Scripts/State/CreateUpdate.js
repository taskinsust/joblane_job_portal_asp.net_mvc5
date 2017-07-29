$(document).on('change', '#Region', function () {

    $('#Country').empty();
    $('#Country').append("<option value=''>Select Country</option>");
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

       
    }

   
});