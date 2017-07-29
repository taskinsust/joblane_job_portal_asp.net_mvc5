$(document).ready(function () {

    $(document).on("change", ".dateFrom, .dateTo", function () {
        var dateFromId = $(this).closest('form').find('.dateFrom').attr('id');
        var dateToId = $(this).closest('form').find('.dateTo').attr('id');

        window.showErrorMessageBelowCtrl(dateFromId, "", false);
        window.showErrorMessageBelowCtrl(dateToId, "", false);
        var startingYear = $(this).closest('form').find('.dateFrom').val();
        var passingyear = $(this).closest('form').find('.dateTo').val();
        if (startingYear.length <= 0) {
            window.showErrorMessageBelowCtrl(dateFromId, "", false);
            window.showErrorMessageBelowCtrl(dateFromId, "Please enter dateFrom ", true);
        }
        if (passingyear.length <= 0) {
            window.showErrorMessageBelowCtrl(dateToId, "", false);
            window.showErrorMessageBelowCtrl(dateToId, "Please enter dateTo", true);
        }
    });

    $(document).on("click", ".isStillServing", function () {
        if ($("#IsStillServing").is(':checked')) {
            $("#DateTo").val("");
            $("#isCurrentDateToMillarary").hide();

        } else {
            $("#isCurrentDateToMillarary").show();
        }
    });

    $(document).on("click", "#militaryAddBtn", function () {
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/AddMilitary",
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

    $(document).on('click', '#saveMilitaryBtn', function () {

        var countryId = $(this).closest('form').find('.country').attr('id');
        var branchId = $(this).closest('form').find('.branch').attr('id');
        var rankId = $(this).closest('form').find('.rank').attr('id');
        var dateFromId = $(this).closest('form').find('.dateFrom').attr('id');
        //var dateToId = $(this).closest('form').find('.dateTo').attr('id');
        var descriptionId = $(this).closest('form').find('.description').attr('id');
        var commendationsId = $(this).closest('form').find('.commendations').attr('id');
        var isCurrent = false;
        validateTextField(countryId, "");
        validateTextField(branchId, "");
        validateTextField(rankId, "");
        validateTextField(dateFromId, "");
        //validateTextField(dateToId, "");
        validateTextField(descriptionId, "");
        validateTextField(commendationsId, "");

        var modelObj = {};
        var id = $(this).closest('form').find('.itemId').val();
        console.log(id);
        var country = $(this).closest('form').find('.country').val();
        var branch = $(this).closest('form').find('.branch').val();
        var rank = $(this).closest('form').find('.rank').val();
        var dateFrom = $(this).closest('form').find('.dateFrom').val();
        var dateTo = $(this).closest('form').find('.dateTo').val();
        var description = $(this).closest('form').find('.description').val();
        var commendations = $(this).closest('form').find('.commendations').val();
        if ($(this).closest('form').find('.isStillServing').is(":checked")) {
            isCurrent = true;
        }
        var isSuccess = true;

        if (country.length <= 0) {
            window.showErrorMessageBelowCtrl(countryId, "", false);
            window.showErrorMessageBelowCtrl(countryId, "Please select country", true);
            isSuccess = false;
        }
        if (branch.length <= 0) {
            window.showErrorMessageBelowCtrl(branchId, "", false);
            window.showErrorMessageBelowCtrl(branchId, "Please select branch", true);
            isSuccess = false;
        }
        if (rank.length <= 0) {
            window.showErrorMessageBelowCtrl(rankId, "", false);
            window.showErrorMessageBelowCtrl(rankId, "Please select rank", true);
            isSuccess = false;
        }
        if (dateFrom.length <= 0) {
            window.showErrorMessageBelowCtrl(dateFromId, "", false);
            window.showErrorMessageBelowCtrl(dateFromId, "Please select date", true);
            isSuccess = false;
        }
        //if (dateTo.length <= 0) {
        //    window.showErrorMessageBelowCtrl(dateToId, "", false);
        //    window.showErrorMessageBelowCtrl(dateToId, "Please select date", true);
        //    isSuccess = false;
        //}
        if (description.length <= 0) {
            window.showErrorMessageBelowCtrl(descriptionId, "", false);
            window.showErrorMessageBelowCtrl(descriptionId, "Please enter description", true);
            isSuccess = false;
        }
        if (commendations.length <= 0) {
            window.showErrorMessageBelowCtrl(commendationsId, "", false);
            window.showErrorMessageBelowCtrl(commendationsId, "Please enter commendations", true);
            isSuccess = false;
        }

        if (isSuccess == true) {
            modelObj.Id = id;
            modelObj.Country = country;
            modelObj.Branch = branch;
            modelObj.Rank = rank;
            modelObj.DateFrom = dateFrom;
            modelObj.DateTo = dateTo;
            modelObj.Description = description;
            modelObj.Commendations = commendations;
            modelObj.IsStillServing = isCurrent;

            $.ajax({
                type: "POST",
                url: $("body").attr("data-project-root") + "JobSeeker/SaveMilitary",
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
    $(document).on("click", ".militaryEditBtn", function () {
        var id = $(this).attr('data-val-id');
        console.log(id);
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/EditMilitary",
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
    $(document).on("click", ".militaryDeleteBtn", function () {
        var id = $(this).attr('data-val-id');
        if (id != "") {
            bootbox.deleteConfirm("<h3>Are you sure you want to delete this <span class='confirm-message'>Military</span> </h3>", function (result) {
                if (result) {
                    $.ajax({
                        type: "post",
                        url: $("body").attr("data-project-root") + "JobSeeker/DelMilitary",
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