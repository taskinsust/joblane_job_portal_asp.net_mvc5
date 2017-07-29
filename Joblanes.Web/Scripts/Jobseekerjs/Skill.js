$(document).ready(function() {
    $(document).on("click", "#skillAddBtn", function () {
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/AddSkill",
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
    $(document).on('click', '#saveSkillBtn', function () {

        var skillId = $(this).closest('form').find('.Skill').attr('id');
        var expYearId = $(this).closest('form').find('.expYear').attr('id');

        validateTextField(skillId, "");
        validateTextField(expYearId, "");

        var modelObj = {};
        var id = $(this).closest('form').find('.itemId').val();
        console.log(id);
        var skill = $(this).closest('form').find('.Skill').val();
        var expYear = $(this).closest('form').find('.expYear').val();
        console.log(expYear);
        var isSuccess = true;

        if (skill.length <= 0) {
            window.showErrorMessageBelowCtrl(skillId, "", false);
            window.showErrorMessageBelowCtrl(skillId, "Please enter Skill", true);
            isSuccess = false;
        }
        if (expYear.length <= 0) {

            window.showErrorMessageBelowCtrl(expYearId, "", false);
            window.showErrorMessageBelowCtrl(expYearId, "Please select", true);
            isSuccess = false;
        }

        if (isSuccess == true) {
            modelObj.Id = id;
            modelObj.Skill = skill;
            modelObj.Experence = expYear;
            $.ajax({
                type: "POST",
                url: $("body").attr("data-project-root") + "JobSeeker/SaveSkill",
                cache: false,
                async: true,
                data: { skill: modelObj },
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
    $(document).on("click", ".skillEditBtn", function () {
        var id = $(this).attr('data-val-id');
        console.log(id);
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/EditSkill",
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
    $(document).on("click", ".skillDeleteBtn", function () {
        var id = $(this).attr('data-val-id');
        if (id != "") {
            bootbox.deleteConfirm("<h3>Are you sure you want to delete this <span class='confirm-message'>Skill</span> </h3>", function (result) {
                if (result) {
                    $.ajax({
                        type: "post",
                        url: $("body").attr("data-project-root") + "JobSeeker/DelSkill",
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