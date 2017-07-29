$(document).ready(function () {

    $(document).on("click", ".isCurrent", function () {
        if ($("#isCurrentId").is(':checked')) {
            $("#isCurrentDateToExp").hide();
        } else {
            $("#isCurrentDateToExp").show();
        }
    });

    $(document).on("click", "#expAddBtn", function () {
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/AddExperienceNew",
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

    $(document).on("change", ".dateFrom", function () {
       var dateFromId = $(this).closest('form').find('.dateFrom').attr('id');
        window.showErrorMessageBelowCtrl(dateFromId, "", false);
        //validateTextField(dateFromId, "");
        var dateFrom = $(this).closest('form').find('.dateFrom').val();
        if (dateFrom.length <= 0) {
            window.showErrorMessageBelowCtrl(dateFromId, "", false);
            window.showErrorMessageBelowCtrl(dateFromId, "Please enter DateFrom", true);
        }
    });

    $(document).on('click', '#expBtn', function () {
        var companyId = $(this).closest('form').find('.companyName').attr('id');
        var companyAddressId = $(this).closest('form').find('.companyAddress').attr('id');
        var designationId = $(this).closest('form').find('.designation').attr('id');
        var dateFromId = $(this).closest('form').find('.dateFrom').attr('id');
        //var dateToId = $(this).closest('form').find('.dateTo').attr('id');
        var responsibilityId = $(this).closest('form').find('.responsibility').attr('id');
        var isCurrent = false;
        console.log("datetoId ==>" + dateFromId);
        validateTextField(companyId, "");
        validateTextField(companyAddressId, "");
        validateTextField(designationId, "");
        validateTextField(dateFromId, "");
        //validateTextField(dateToId, "");
        validateTextField(responsibilityId, "");

        var modelObj = {};
        var id = $(this).closest('form').find('.itemId').val();
        var companyName = $(this).closest('form').find('.companyName').val();
        var companyAddress = $(this).closest('form').find('.companyAddress').val();
        var designation = $(this).closest('form').find('.designation').val();
        var dateFrom = $(this).closest('form').find('.dateFrom').val();
        var dateTo = $(this).closest('form').find('.dateTo').val();
        var responsibility = $(this).closest('form').find('.responsibility').val();
        if ($(this).closest('form').find('.isCurrent').is(":checked")) {
            isCurrent = true;
        }
        var isSuccess = true;
        if (companyName.length <= 0) {
            window.showErrorMessageBelowCtrl(companyId, "", false);
            window.showErrorMessageBelowCtrl(companyId, "Please enter CompanyName", true);
            isSuccess = false;
        }
        if (companyAddress.length <= 0) {
            window.showErrorMessageBelowCtrl(companyAddressId, "", false);
            window.showErrorMessageBelowCtrl(companyAddressId, "Please enter CompanyAddress", true);
            isSuccess = false;
        }
        if (designation.length <= 0) {
            window.showErrorMessageBelowCtrl(designationId, "", false);
            window.showErrorMessageBelowCtrl(designationId, "Please enter Designation", true);
            isSuccess = false;
        }
        if (dateFrom.length <= 0) {
            window.showErrorMessageBelowCtrl(dateFromId, "", false);
            window.showErrorMessageBelowCtrl(dateFromId, "Please enter DateFrom", true);
            isSuccess = false;
        }

        if (responsibility.length <= 0) {
            window.showErrorMessageBelowCtrl(responsibilityId, "", false);
            window.showErrorMessageBelowCtrl(responsibilityId, "Please enter Responsibility", true);
            isSuccess = false;
        }
        if (isSuccess) {
            modelObj.Id = id;
            modelObj.CompanyName = companyName;
            modelObj.CompanyAddress = companyAddress;
            modelObj.Designation = designation;
            modelObj.DateFrom = dateFrom;
            modelObj.DateTo = dateTo;
            modelObj.IsCurrent = isCurrent;
            modelObj.Responsibility = responsibility;

            $.ajax({
                type: "POST",
                url: $("body").attr("data-project-root") + "JobSeeker/AddExperience",
                cache: false,
                async: true,
                data: { jobSeekerExpVm: modelObj },
                beforeSend: function () {
                    $.blockUI({
                        timeout: 0,
                        message: '<h1><img src="/Image/ajax-loader.gif" /> Processing...</h1>'
                    });
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.IsSuccess) {
                       window.location.href = "JobSeekerProfile";
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
    $(document).on("click", ".expEditBtn", function () {
        var id = $(this).attr('data-val-id');
        console.log(id);
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/EditExp",
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
    $(document).on("click", ".expDeleteBtn", function () {
        var id = $(this).attr('data-val-id');
        if (id != "") {
            bootbox.deleteConfirm("<h3>Are you sure you want to delete this <span class='confirm-message'>Experience</span> </h3>", function (result) {
                if (result) {
                    $.ajax({
                        type: "post",
                        url: $("body").attr("data-project-root") + "JobSeeker/DelExp",
                        cache: false,
                        async: true,
                        data: { "id": id },
                        success: function (result) {
                            if (result.IsSuccess) {
                                window.location.reload();
                            }
                            else {
                                window.location.reload();
                            }
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