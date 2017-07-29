$(document).ready(function() {
    $(document).on("click", ".profileEditBtn", function () {
        var id = $(this).attr('data-val-id');
        console.log(id);
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/EditProfile",
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

    $(document).on('click', '#profileBtn', function () {
      $('.field-validation-error').remove();
        var form = $('#profileForm');
        if (!$('#profileForm').data('unobtrusiveValidation').validate()) {
            return false;
        } else {
            var isSuccess = true;
            var token = $('input[name="__RequestVerificationToken"]', form).val();
            var profile = {};
            profile.Id = $("#Id").val();
            profile.FirstName = $('#FirstName').val();
            profile.LastName = $('#LastName').val();
            profile.RegionId = $('#RegionId').val();
            profile.CountryId = $('#CountryId').val();
            profile.StateId = $('#StateId').val();
            profile.CityId = $('#CityId').val();
            profile.Gender = $('#Gender').val();
            profile.MaritalStatus = $('#MaritalStatus').val();
            profile.ContactNumber = $('#ContactNumber').val();
            profile.ContactEmail = $('#ContactEmail').val();
            var profileType = $('input[name=privacy]:checked', '#profileForm').val();
            console.log(profileType);
            if (profileType == 1) {
                profile.IsPublicResume = true;
            }
            else if (profileType == 2) {
                profile.IsPublicResume = false;
            }

            profile.FatherName = $('#FatherName').val();
            profile.MotherName = $('#MotherName').val();
            profile.Address = $('#Address').val();
            profile.ZipCode = $('#ZipCode').val();
            profile.Linkedin = $('#Linkedin').val();
            profile.Weblink = $('#Weblink').val();
            profile.Dob = $('#Dob').val();
            profile.Expertise = $('#Expertise').val();
            //profile.ProfileImage = $('#ProfileImage').val();
            //console.log("----------------->>>>" + $('#RegionId').val());

            if (isSuccess == true) {
                //event.preventDefault();
                console.log("wrong entry");
                $.ajax({
                    type: "POST",
                    url: $("body").attr("data-project-root") + "JobSeeker/BuildResume",
                    cache: false,
                    async: true,
                    data: { __RequestVerificationToken: token, profileVm: profile },
                    beforeSend: function () {
                        $.blockUI({
                            timeout: 0,
                            message: '<h1><img src="/Image/ajax-loader.gif" /> Processing...</h1>'
                        });
                    },
                    success: function (response) {
                        $.unblockUI();
                        if (response.IsSuccess) {
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

    $(document).on("click", "#delResume", function () {
        var id = $(this).attr('data-val-id');
        if (id != "") {
            bootbox.deleteConfirm("<h3>Are you sure you want to delete <span class='confirm-message'>Resume</span> </h3>", function (result) {
                if (result) {
                    $.ajax({
                        type: "post",
                        url: $("body").attr("data-project-root") + "JobSeeker/DeleteResume",
                        cache: false,
                        async: true,
                        data: { "id": id },
                        success: function (response) {
                            alert(response.Message);
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