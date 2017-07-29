$(document).ready(function () {

    $(document).on("click", "#publicationAddBtn", function () {
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/AddPublication",
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

    $(document).on('click', '#savePublicationBtn', function () {
        $('.field-validation-error').remove();
        if (!$('#publicationForm').data('unobtrusiveValidation').validate()) {
            return false;
        } else {
            console.log("enter else condition");
            var title = $(this).closest('form').find('.title').val();
            var url = $(this).closest('form').find('.url').val();
            var publishDate = $(this).closest('form').find('.publishDate').val();
            var description = $(this).closest('form').find('.description').val();
            console.log("Description ==>" + description);
            var modelObj = {};
            var id = $(this).closest('form').find('.itemId').val();
            modelObj.Id = id;
            modelObj.Title = title;
            modelObj.Url = url;
            modelObj.PublishDate = publishDate;
            modelObj.Description = description;
            $.ajax({
                type: "POST",
                url: $("body").attr("data-project-root") + "JobSeeker/SavePublication",
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
    $(document).on("click", ".publicationEditBtn", function () {
        var id = $(this).attr('data-val-id');
        console.log(id);
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/EditPublication",
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
    $(document).on("click", ".publicationDeleteBtn", function () {
        var id = $(this).attr('data-val-id');
        if (id != "") {
            bootbox.deleteConfirm("<h3>Are you sure you want to delete this <span class='confirm-message'>Publication</span> </h3>", function (result) {
                if (result) {
                    $.ajax({
                        type: "post",
                        url: $("body").attr("data-project-root") + "JobSeeker/DelPublication",
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