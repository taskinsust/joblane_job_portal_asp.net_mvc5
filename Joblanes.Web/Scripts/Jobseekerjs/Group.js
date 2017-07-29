$(document).ready(function () {

    $(document).on("click", ".isStillMember", function () {
        if ($("#isStillMember").is(':checked')) {
            $(this).closest("#DateTo").val("");
            //$("#DateTo").val("");
            $("#isCurrentDateToGroup").hide();

        } else {
            $("#isCurrentDateToGroup").show();
        }
    });

    $(document).on("click", "#groupAddBtn", function () {
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/AddGroup",
            cache: false,
            async: true,
            beforeSend: function () {
                $.blockUI({
                    timeout: 0,
                    message: '<h1><img src="/Image/ajax-loader.gif" /> Processing...</h1>'
                });
            },
            success: function (response) {
                $.unblockUI();
                $('.modal-message').html('');
                $("#getCode").html(response);
                $("#getCodeModal").modal('show');
            },
            complete: function (response1) {
                $.unblockUI();

            },
            error: function (response1) {
                $.unblockUI();

            }
        });
    });

    $(document).on('click', '#saveGroupBtn', function () {
        $('.field-validation-error').remove();
        if (!$('#groupForm').data('unobtrusiveValidation').validate()) {
            return false;
        } else {
            console.log("enter else condition");
            var isCurrent = false;
            var title = $(this).closest('form').find('.title').val();
            var startDate = $(this).closest('form').find('.dateFrom').val();
            var closeDate = $(this).closest('form').find('.dateTo').val();
            var description = $(this).closest('form').find('.description').val();
            console.log("Description ==>" + description);
            var modelObj = {};
            var id = $(this).closest('form').find('.itemId').val();
            modelObj.Id = id;
            modelObj.Title = title;
            modelObj.DateFrom = startDate;
            modelObj.DateTo = closeDate;
            modelObj.Description = description;
            if ($(this).closest('form').find('.isStillMember').is(":checked")) {
                isCurrent = true;
            }
            modelObj.IsStillMember = isCurrent;
            $.ajax({
                type: "POST",
                url: $("body").attr("data-project-root") + "JobSeeker/SaveGroup",
                cache: false,
                async: true,
                data: { link: modelObj },
                beforeSend: function () {
                    $.blockUI({
                        timeout: 0,
                        message: '<h1><img src="/Image/ajax-loader.gif" /> Processing...</h1>'
                    });
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.IsSuccess) {
                        console.log(response.Message);
                        $("#getCodeModal").modal('toggle');
                        window.location.reload();
                    } else {
                        $('.modal-message').html(response.Message);
                    }
                    $("html, body").animate({ scrollTop: 0 }, "slow");
                },
                complete: function (response) {
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                }
            });
        }

    });
    $(document).on("click", ".groupEditBtn", function () {
        var id = $(this).attr('data-val-id');
        console.log(id);
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/EditGroup",
            cache: false,
            async: true,
            data: { id: id },
            beforeSend: function () {
                $.blockUI({
                    timeout: 0,
                    message: '<h1><img src="/Image/ajax-loader.gif" /> Processing...</h1>'
                });
            },
            success: function (response) {
                $.unblockUI();
                $("#getCode").html(response);
                $("#getCodeModal").modal('show');
            },
            complete: function (response1) {
                $.unblockUI();

            },
            error: function (response1) {
                $.unblockUI();

            }
        });
    });
    $(document).on("click", ".groupDeleteBtn", function () {
        var id = $(this).attr('data-val-id');
        if (id != "") {
            bootbox.deleteConfirm("<h3>Are you sure you want to delete this <span class='confirm-message'>Group</span> </h3>", function (result) {
                if (result) {
                    $.ajax({
                        type: "post",
                        url: $("body").attr("data-project-root") + "JobSeeker/DelGroup",
                        cache: false,
                        async: true,
                        data: { "id": id },
                        success: function (result) {
                            window.location.reload();
                        },
                        error: function (result) {
                            window.location.reload();
                        }
                    });
                }
            }).css({ 'margin-top': (($(window).height() / 4)) });
        }
        return false;
    });

});