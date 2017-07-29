$(document).ready(function() {
    $(document).on("click", "#eduAddBtn", function () {
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/AddEducationalQualificationNew",
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

    $(document).on("change", ".startingYear, .passingYear", function () {
        var dateFromId = $(this).closest('form').find('.startingYear').attr('id');
        var dateToId = $(this).closest('form').find('.passingYear').attr('id');

        window.showErrorMessageBelowCtrl(dateFromId, "", false);
        window.showErrorMessageBelowCtrl(dateToId, "", false);
        var startingYear = $(this).closest('form').find('.startingYear').val();
        var passingyear = $(this).closest('form').find('.passingYear').val();
        if (startingYear.length <= 0) {
            window.showErrorMessageBelowCtrl(dateFromId, "", false);
            window.showErrorMessageBelowCtrl(dateFromId, "Please enter starting Year", true);
        }
        if (passingyear.length <= 0) {
            window.showErrorMessageBelowCtrl(dateToId, "", false);
            window.showErrorMessageBelowCtrl(dateToId, "Please enter passing Year", true);
        }
    });

    $(document).on('click', '#eduBtn', function () {
        var insId = $(this).closest('form').find('.institute').attr('id');
        var degId = $(this).closest('form').find('.degree').attr('id');
        var staId = $(this).closest('form').find('.startingYear').attr('id');
        var passId = $(this).closest('form').find('.passingYear').attr('id');
        var resId = $(this).closest('form').find('.result').attr('id');
        var fieldOfStudyId = $(this).closest('form').find('.fieldOfStudy').attr('id');
        validateTextField(insId, "");
        validateTextField(degId, "");
        validateTextField(staId, "");
        validateTextField(passId, "");
        //validateTextField(resId, "");
        validateTextField(fieldOfStudyId, "");

        var modelObj = {};
        var id = $(this).closest('form').find('.itemId').val();
        var institute = $(this).closest('form').find('.institute').val();
        var degree = $(this).closest('form').find('.degree').val();
        var startingYear = $(this).closest('form').find('.startingYear').val();
        var passingYear = $(this).closest('form').find('.passingYear').val();
        var result = $(this).closest('form').find('.result').val();
        var fieldOfStudy = $(this).closest('form').find('.fieldOfStudy').val();
        console.log(fieldOfStudy);
        var isSuccess = true;
        console.log(institute);
        if (institute.length <= 0) {
            window.showErrorMessageBelowCtrl(insId, "", false);
            window.showErrorMessageBelowCtrl(insId, "Please enter School", true);
            isSuccess = false;
        }
        if (degree.length <= 0) {

            window.showErrorMessageBelowCtrl(degId, "", false);
            window.showErrorMessageBelowCtrl(degId, "Please enter Degree", true);
            isSuccess = false;
        }
        if (fieldOfStudy.length <= 0) {

            window.showErrorMessageBelowCtrl(fieldOfStudyId, "", false);
            window.showErrorMessageBelowCtrl(fieldOfStudyId, "Please enter Field Of Study", true);
            isSuccess = false;
        }
        if (startingYear.length <= 0) {

            window.showErrorMessageBelowCtrl(staId, "", false);
            window.showErrorMessageBelowCtrl(staId, "Please enter StartingYear", true);
            isSuccess = false;
        }
        if (passingYear.length <= 0) {

            window.showErrorMessageBelowCtrl(passId, "", false);
            window.showErrorMessageBelowCtrl(passId, "Please enter PassingYear", true);
            isSuccess = false;
        }
        //if (result.length <= 0) {

        //    window.showErrorMessageBelowCtrl(resId, "", false);
        //    window.showErrorMessageBelowCtrl(resId, "Please enter Result", true);
        //    isSuccess = false;
        //}
        if (isSuccess == true) {
            modelObj.Id = id;
            modelObj.Institute = institute;
            modelObj.Degree = degree;
            modelObj.StartingYear = startingYear;
            modelObj.PassingYear = passingYear;
            modelObj.Result = result;
            modelObj.FieldOfStudy = fieldOfStudy;
            $.ajax({
                type: "POST",
                url: $("body").attr("data-project-root") + "JobSeeker/AddEducationalQualification",
                cache: false,
                async: true,
                data: { profile: modelObj },
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
    $(document).on("click", ".eduEditBtn", function () {
        var id = $(this).attr('data-val-id');
        console.log(id);
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/EditEdu",
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
    $(document).on("click", ".eduDeleteBtn", function () {
        var id = $(this).attr('data-val-id');
        if (id != "") {
            bootbox.deleteConfirm("<h3>Are you sure you want to delete <span class='confirm-message'>Educational Qualification</span> </h3>", function (result) {
                if (result) {
                    $.ajax({
                        type: "post",
                        url: $("body").attr("data-project-root") + "JobSeeker/DelEdu",
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