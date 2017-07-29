function dataTableRender() {
    var pageSize = 10;
    if ($('#pageSize').val() != "" && /^\d+$/.test($('#pageSize').val())) {
        pageSize = parseInt($('#pageSize').val());
    }
    else {
        $('#pageSize').val(pageSize);
    }
    var isShort = false;
    if ($("#isShorlisted").is(":checked")) {
        isShort = true;
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
            url: $("body").attr("data-project-root") + "Company/JobApplicantList",
            type: 'POST',
            data: function (d) {
                d.jobPostId = $("#jobPostId").val();
                d.jobTitle = $("#jobTitle").val();
                d.deadlineFlag = $("#deadlineFlag").val();
                d.dateFrom = $("#dateFrom").val();
                d.dateTo = $('#dateTo').val();
                d.isShorlisted = isShort;
            }
        },
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $(".glyphicon-book").each(function () {
                alert("Hi");
                var isShort = $(this).attr("data-isShort");
                console.log(isShort);
                if (isShort === "True") {
                    var tr = $(this).parent().parent();
                    console.log(tr);
                    tr.css('background-color', 'Red');
                }
            });
        }
    });
}

$(document).ready(function () {
    

    dataTableRender();

    $(document).on("click", "#search", function () {
        dataTableRender();
    });

    $('#dateFrom').datetimepicker({
        format: "yyyy-mm-dd",
        autoclose: true,
        todayBtn: false,
        showMeridian: true,
        initialDate: new Date(),
        startView: 2,
        minView: 2,
        maxView: 4
    });

    $('#dateTo').datetimepicker({
        format: "yyyy-mm-dd",
        autoclose: true,
        todayBtn: false,
        showMeridian: true,
        initialDate: new Date(),
        startView: 2,
        minView: 2,
        maxView: 4
    });

    $(".glyphicon-book").each(function () {
        alert("Hi");
        var isShort = $(this).attr("data-isShort");
        console.log(isShort);
        if (isShort==="True") {
            var tr = $(this).parent().parent();
            console.log(tr);
            tr.css('background-color', 'Red');
        }
    });
});



$(document).on("click", ".glyphicon-star-empty,.glyphicon-star", function () {
       
    var id = $(this).attr("id");
    var isShortListed = $(this).attr("data-isShort");
    var name = $(this).attr("data-name");
    if (id != "") {
        bootbox.confirm("<h3>Are you sure you want to shortlist <span class='confirm-message'>" + name + "</span>'s Application?</h3>", function (result) {
            if (result) {
                $.ajax({
                    type: "post",
                    url: $("body").attr("data-project-root") + "Company/ShotlistApplication",
                    cache: false,
                    async: true,
                    data: { "id": id,"isShortListed":isShortListed },
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
                        dataTableRender();
                    },
                    complete: function () {
                        $.unblockUI();
                    },
                    error: function (result) {
                        var errorMessage = '<div class="alert alert-danger"><a class="close" data-dismiss="alert">×</a>Application Shortlisting  Failed</div>';
                        $('.customMessage').append(errorMessage);
                    }
                });

            }
        }).css({ 'margin-top': (($(window).height() / 4)) });
    }
    return false;
});
