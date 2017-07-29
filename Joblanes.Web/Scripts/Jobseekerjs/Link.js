$(document).ready(function () {

    $(document).on("click", "#linkAddBtn", function () {
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/AddLink",
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

    $(document).on('click', '#saveLinkBtn', function () {
        $('.field-validation-error').remove();
        if (!$('#LinkForm').data('unobtrusiveValidation').validate()) {
            return false;
        } else {

            var linkId = $(this).closest('form').find('.Link').attr('id');

            validateTextField(linkId, "");

            var modelObj = {};
            var id = $(this).closest('form').find('.itemId').val();
            console.log(id);
            var link = $(this).closest('form').find('.Link').val();

            var isSuccess = true;

            if (link.length <= 0) {
                window.showErrorMessageBelowCtrl(linkId, "", false);
                window.showErrorMessageBelowCtrl(linkId, "Please enter Link", true);
                isSuccess = false;
            }

            if (isSuccess == true) {
                modelObj.Id = id;
                modelObj.Link = link;
                $.ajax({
                    type: "POST",
                    url: $("body").attr("data-project-root") + "JobSeeker/SaveLink",
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
        }
    });
    $(document).on("click", ".linkEditBtn", function () {
        var id = $(this).attr('data-val-id');
        console.log(id);
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/EditLink",
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
    $(document).on("click", ".linkDeleteBtn", function () {
        var id = $(this).attr('data-val-id');
        if (id != "") {
            bootbox.deleteConfirm("<h3>Are you sure you want to delete this <span class='confirm-message'>Link</span> </h3>", function (result) {
                if (result) {
                    $.ajax({
                        type: "post",
                        url: $("body").attr("data-project-root") + "JobSeeker/DelLink",
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