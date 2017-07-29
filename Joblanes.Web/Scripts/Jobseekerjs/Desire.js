$(document).ready(function () {

    $(document).on("click", "#desireAddBtn", function () {
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/AddDesire",
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

    $(document).on('click', '#saveDesireBtn', function () {

        var desireSalary = $(this).closest('form').find('.desireSalary').attr('id');
        var isRelocat = $(this).closest('form').find('.isRelocate').attr('id');
        var isSuccess = true;
        validateTextField(desireSalary, "");
        //validateTextField(isRelocat, "");
        var id = $(this).closest('form').find('.itemId').val();
        var desiretype = $('input:checkbox:checked.jobType').map(function () {
            return this.value;
        }).get();
        var salary = $(".desireSalary").val();
        var salaryDuration = $(".desireSalaryDuration").val();
        var isRelocate = "";
        var place = "";
        var eligibile = "";
        eligibile = $("input[name='eligibility']:checked").val();
        isRelocate = $("input[name='isRelocate']:checked").val();
        place = $("input[name='allocate']:checked").val();
        if (desiretype.length === 0) {
            $('.modal-message').html('<span>please select aleast one desired type</span>');
            isSuccess = false;
        }
        if (salary === undefined || salary == "" || salary == null || salary.length <= 0) {
            window.showErrorMessageBelowCtrl(desireSalary, "", false);
            window.showErrorMessageBelowCtrl(desireSalary, "Please enter desired salary", true);
            isSuccess = false;
        }
        //if (isRelocate.length <= 0) {
        //    window.showErrorMessageBelowCtrl(isRelocat, "", false);
        //    window.showErrorMessageBelowCtrl(isRelocat, "Please enter CompanyName", true);
        //    isSuccess = false;
        //}

        console.log(salary);
        //console.log(eligibile);
        //console.log(place);
        //console.log(salaryDuration);
        //console.log(desiretype);
        if (isSuccess) {
            $.ajax({
                type: "POST",
                url: $("body").attr("data-project-root") + "JobSeeker/SaveDesire",
                cache: false,
                async: true,
                data: { desiretype: desiretype, salary: salary, salaryDuration: salaryDuration, isRelocate: isRelocate, place: place, eligibile: eligibile, id: id },
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



    $(document).on("click", ".desireEditBtn", function () {
        var id = $(this).attr('data-val-id');
        console.log(id);
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/EditDesire",
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
    $(document).on("click", ".desireDeleteBtn", function () {
        var id = $(this).attr('data-val-id');
        if (id != "") {
            bootbox.deleteConfirm("<h3>Are you sure you want to delete this <span class='confirm-message'>Desired Jobs</span> </h3>", function (result) {
                if (result) {
                    $.ajax({
                        type: "post",
                        url: $("body").attr("data-project-root") + "JobSeeker/DelDesiredJobs",
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

    $(document).on("click", ".isRelocate", function () {
        if ($(".isRelocate").is(':checked'))
            $('input:radio[name=allocate]').prop("checked", "checked");
        else
            $('input:radio[name=allocate]').each(function () { $(this).prop('checked', false); });
    });
});