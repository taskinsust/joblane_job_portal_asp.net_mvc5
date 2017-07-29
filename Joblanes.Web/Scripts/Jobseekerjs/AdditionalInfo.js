$(document).ready(function () {

    $(document).on("click", "#additionalAddBtn", function () {
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/AddAdditionalInfo",
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

    $(document).on('click', '#saveAdditionalInfoBtn', function () {
        $('.field-validation-error').remove();
        if (!$('#additionalInfoForm').data('unobtrusiveValidation').validate()) {
            return false;
        } else {
            console.log("enter else condition");
            var description = $(this).closest('form').find('.description').val();
            var modelObj = {};
            var id = $(this).closest('form').find('.itemId').val();
            modelObj.Id = id;
            modelObj.Description = description;
            $.ajax({
                type: "POST",
                url: $("body").attr("data-project-root") + "JobSeeker/SaveAdditionalInfo",
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
    $(document).on("click", ".aiEditBtn", function () {
        var id = $(this).attr('data-val-id');
        console.log(id);
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/EditAdditionalInfo",
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
    $(document).on("click", ".aiDeleteBtn", function () {
        var id = $(this).attr('data-val-id');
        if (id != "") {
            bootbox.deleteConfirm("<h3>Are you sure you want to delete this <span class='confirm-message'>Additional Info</span> </h3>", function (result) {
                if (result) {
                    $.ajax({
                        type: "post",
                        url: $("body").attr("data-project-root") + "JobSeeker/DelAdditionalInfo",
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