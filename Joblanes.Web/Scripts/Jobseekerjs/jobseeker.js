$(document).ready(function () {

    $(document).on("click", ".shortListedJob", function () {
        var jobId = $(this).attr("id");
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/JobShortlisted",
            cache: false,
            async: true,
            data: { jobId: jobId },
            beforeSend: function () {
                $.blockUI({
                    timeout: 0,
                    message: '<h1><img src="/Image/ajax-loader.gif" /> Processing...</h1>'
                });
            },
            success: function (response) {
                $.unblockUI();
                if (response.IsSuccess) {
                    console.log("success");
                    $.fn.customMessage({
                        displayMessage: response.Message,
                        displayMessageType: "success",
                    });
                } else {
                    console.log("false");
                    $.fn.customMessage({
                        displayMessage: response.Message,
                        displayMessageType: "error",
                    });
                    $("html, body").animate({ scrollTop: 0 }, "slow");
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
    });

    $(document).on("click", "#applyBtn", function () {
        var jobId = $("#Id").val();
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/JobApplied",
            cache: false,
            async: true,
            data: { jobId: jobId },
            beforeSend: function () {
                $.blockUI({
                    timeout: 0,
                    message: '<h1><img src="/Image/ajax-loader.gif" /> Processing...</h1>'
                });
            },
            success: function (response) {
                $.unblockUI();
                if (response.IsSuccess) {
                    console.log("success");
                    $("#applyBtn").prop('disabled', true);
                    $.fn.customMessage({
                        displayMessage: response.Message,
                        displayMessageType: "success",
                    });
                } else {
                    console.log("false");
                    $.fn.customMessage({
                        displayMessage: response.Message,
                        displayMessageType: "error",
                    });
                    $("html, body").animate({ scrollTop: 0 }, "slow");
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

    });

    $(document).on('click', '#profileBtn', function () {
        console.log("hello from");
        $('.field-validation-error').remove();
        var form = $('#profileForm');
        if (!$('#profileForm').data('unobtrusiveValidation').validate()) {
            return false;
        } else {
            var isSuccess = true;
            var token = $('input[name="__RequestVerificationToken"]', form).val();
            var profile = {};
            profile.FirstName = $('#FirstName').val();
            profile.LastName = $('#LastName').val();
            profile.RegionId = $('#RegionId').val();
            profile.CountryId = $('#CountryId').val();
            profile.StateId = $('#StateId').val();
            profile.CityId = $('#CityId').val();
            //profile.Gender = $('#Gender').val();
            //profile.MaritalStatus = $('#MaritalStatus').val();
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
            if (isSuccess == true) {
                //event.preventDefault();
                console.log("wrong entry");
                $.ajax({
                    type: "POST",
                    url: $("body").attr("data-project-root") + "JobSeeker/BuildResume",
                    cache: false,
                    async: true,
                    //processData: false,
                    //contentType: false,
                    data: { __RequestVerificationToken: token, profileVm: profile },
                    //dataType: 'json',
                    beforeSend: function () {
                        $.blockUI({
                            timeout: 0,
                            message: '<h1><img src="/Image/ajax-loader.gif" /> Processing...</h1>'
                        });
                    },
                    success: function (response) {
                        $.unblockUI();
                        if (response.IsSuccess) {
                            console.log("success");
                            $.fn.customMessage({
                                displayMessage: response.Message,
                                displayMessageType: "success",
                            });
                            /*Next button click --Edu data will show */

                            $.ajax({
                                type: "POST",
                                url: $("body").attr("data-project-root") + "JobSeeker/GetEducationalData",
                                cache: false,
                                async: true,
                                beforeSend: function () {
                                    $.blockUI({
                                        timeout: 0,
                                        message: '<h1><img src="/Image/ajax-loader.gif" /> Processing...</h1>'
                                    });
                                },
                                success: function (response1) {
                                    $.unblockUI();
                                    $("#profile").html(response1);
                                },
                                complete: function (response1) {
                                    $.unblockUI();

                                },
                                error: function (response1) {
                                    $.unblockUI();

                                }
                            });

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
                        $.fn.customMessage({
                            displayMessage: response.Message,
                            displayMessageType: "success",
                        });
                        /*Next button click --Edu data will show */

                        $.ajax({
                            type: "POST",
                            url: $("body").attr("data-project-root") + "JobSeeker/GetExpData",
                            cache: false,
                            async: true,
                            beforeSend: function () {
                                $.blockUI({
                                    timeout: 0,
                                    message: '<h1><img src="/Image/ajax-loader.gif" /> Processing...</h1>'
                                });
                            },
                            success: function (response1) {
                                $.unblockUI();
                                $("#profile").html(response1);
                            },
                            complete: function (response1) {
                                $.unblockUI();

                            },
                            error: function (response1) {
                                $.unblockUI();

                            }
                        });
                    } else {
                        $.fn.customMessage({
                            displayMessage: response.Message,
                            displayMessageType: "error",
                        });
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

    $(document).on('click', '#eduDelBtn', function () {
        var insId = $(this).closest('form').find('.institute').attr('id');
        var id = $(this).closest('form').find('.itemId').val();
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/DelEducationalQualification",
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
                if (response.IsSuccess) {
                    console.log(response.Message);
                    $.fn.customMessage({
                        displayMessage: response.Message,
                        displayMessageType: "success",
                    });
                } else {
                    $.fn.customMessage({
                        displayMessage: response.Message,
                        displayMessageType: "error",
                    });
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

    });

    $(document).on('click', '#expBtn', function () {
        var companyId = $(this).closest('form').find('.companyName').attr('id');
        var companyAddressId = $(this).closest('form').find('.companyAddress').attr('id');
        var designationId = $(this).closest('form').find('.designation').attr('id');
        var dateFromId = $(this).closest('form').find('.dateFrom').attr('id');
        //var dateToId = $(this).closest('form').find('.dateTo').attr('id');
        var responsibilityId = $(this).closest('form').find('.responsibility').attr('id');
        var isCurrent = false;
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
                        $.fn.customMessage({
                            displayMessage: response.Message,
                            displayMessageType: "success",
                        });
                        window.location.href = "JobSeekerProfile";
                    } else {
                        $.fn.customMessage({
                            displayMessage: response.Message,
                            displayMessageType: "error",
                        });
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

    $(document).on('click', '#expDelBtn', function () {
        var insId = $(this).closest('form').find('.institute').attr('id');
        var id = $(this).closest('form').find('.itemId').val();
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/DelExp",
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
                if (response.IsSuccess) {
                    console.log(response.Message);
                    $.fn.customMessage({
                        displayMessage: response.Message,
                        displayMessageType: "success",
                    });
                } else {
                    $.fn.customMessage({
                        displayMessage: response.Message,
                        displayMessageType: "error",
                    });
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

    });

    $(document).on('click', '#trainBtn', function () {
        var titleId = $(this).closest('form').find('.title').attr('id');
        var descriptionId = $(this).closest('form').find('.description').attr('id');
        var instituteId = $(this).closest('form').find('.institute').attr('id');
        var startDateId = $(this).closest('form').find('.startDate').attr('id');
        var closeDateId = $(this).closest('form').find('.closeDate').attr('id');

        validateTextField(titleId, "");
        validateTextField(descriptionId, "");
        validateTextField(instituteId, "");
        validateTextField(startDateId, "");
        validateTextField(closeDateId, "");

        var modelObj = {};
        var id = $(this).closest('form').find('.itemId').val();
        var title = $(this).closest('form').find('.title').val();
        var description = $(this).closest('form').find('.description').val();
        var institute = $(this).closest('form').find('.institute').val();
        console.log(institute);
        var startDate = $(this).closest('form').find('.startDate').val();
        var closeDate = $(this).closest('form').find('.closeDate').val();

        var isSuccess = true;
        if (title.length <= 0) {
            window.showErrorMessageBelowCtrl(titleId, "", false);
            window.showErrorMessageBelowCtrl(titleId, "Please enter Title", true);
            isSuccess = false;
        }
        if (description.length <= 0) {
            window.showErrorMessageBelowCtrl(descriptionId, "", false);
            window.showErrorMessageBelowCtrl(descriptionId, "Please enter Description", true);
            isSuccess = false;
        }
        if (institute.length <= 0) {
            window.showErrorMessageBelowCtrl(instituteId, "", false);
            window.showErrorMessageBelowCtrl(instituteId, "Please enter Institute", true);
            isSuccess = false;
        }
        if (startDate.length <= 0) {
            window.showErrorMessageBelowCtrl(startDateId, "", false);
            window.showErrorMessageBelowCtrl(startDateId, "Please enter StartDate", true);
            isSuccess = false;
        }
        if (closeDate.length <= 0) {
            window.showErrorMessageBelowCtrl(closeDateId, "", false);
            window.showErrorMessageBelowCtrl(closeDateId, "Please enter CloseDate", true);
            isSuccess = false;
        }
        if (isSuccess) {
            modelObj.Id = id;
            modelObj.Title = title;
            modelObj.Description = description;
            modelObj.Institute = institute;
            modelObj.StartDate = startDate;
            modelObj.CloseDate = closeDate;

            $.ajax({
                type: "POST",
                url: $("body").attr("data-project-root") + "JobSeeker/AddTraining",
                cache: false,
                async: true,
                data: { jobSeekerTrainingVm: modelObj },
                beforeSend: function () {
                    $.blockUI({
                        timeout: 0,
                        message: '<h1><img src="/Image/ajax-loader.gif" /> Processing...</h1>'
                    });
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.IsSuccess) {
                        $.fn.customMessage({
                            displayMessage: response.Message,
                            displayMessageType: "success",
                        });
                    } else {
                        $.fn.customMessage({
                            displayMessage: response.Message,
                            displayMessageType: "error",
                        });
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

    $(document).on('click', '#trainDelBtn', function () {
        var insId = $(this).closest('form').find('.institute').attr('id');
        var id = $(this).closest('form').find('.itemId').val();
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/DelTraining",
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
                if (response.IsSuccess) {
                    console.log(response.Message);
                    $.fn.customMessage({
                        displayMessage: response.Message,
                        displayMessageType: "success",
                    });
                } else {
                    $.fn.customMessage({
                        displayMessage: response.Message,
                        displayMessageType: "error",
                    });
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

    });

    function sendFile(file) {
        var formData = new FormData();
        formData.append('file', $('#f_UploadImage')[0].files[0]);
        $.ajax({
            type: 'post',
            url: $("body").attr("data-project-root") + "JobSeeker/AddPhotos",
            data: formData,
            //dataType: 'json',
            beforeSend: function () {
                $.blockUI({
                    timeout: 0,
                    message: '<h1><img src="/Image/ajax-loader.gif" /> Processing...</h1>'
                });
            },
            success: function (response) {
                $.unblockUI();
                if (response.IsSuccess) {
                    console.log("success");
                    $.fn.customMessage({
                        displayMessage: response.Message,
                        displayMessageType: "success",
                    });
                } else {
                    console.log("false");
                    $.fn.customMessage({
                        displayMessage: response.Message,
                        displayMessageType: "error",
                    });
                    $("html, body").animate({ scrollTop: 0 }, "slow");
                }
            },
            processData: false,
            contentType: false,
            complete: function (response) {
                $.unblockUI();
            },
            error: function (response) {
                $.unblockUI();
            }
        });

    }

    var _URL = window.URL || window.webkitURL;

    $("#f_UploadImage").on('change', function () {
        var file, img;
        if ((file = this.files[0])) {
            img = new Image();
            img.onload = function () {
                sendFile(file);
            };
            img.onerror = function () {
                $.fn.customMessage({
                    displayMessage: "Not a valid file:" + file.type,
                    displayMessageType: "error"
                });
            };
            img.src = _URL.createObjectURL(file);
        }
    });

    /* exp added */
    $(document).on("click", "#expId", function () {
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/GetExpData",
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
                $("#exp").html(response);
            },
            complete: function (response) {
                $.unblockUI();

            },
            error: function (response) {
                $.unblockUI();

            }
        });
    });

    /*train on change*/
    $(document).on("click", "#trainId", function () {
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "JobSeeker/GetTrainData",
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
                $("#train").html(response);
            },
            complete: function (response) {
                $.unblockUI();
            },
            error: function (response) {
                $.unblockUI();

            }
        });
    });

    $(document).on("change", "#RegionId", function () {
        var regionId = $("#RegionId").val();
        if (regionId.length <= 0) {
            window.validateDropDownField("RegionId", "RegionId", "please select region");
            $("#CountryId").html("<option selected='selected'>Select Country</option>");
            $("#StateId").html("<option selected='selected'>Select State</option>");
            $("#CityId").html("<option selected='selected'>Select City</option>");
        }
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "Country/LoadCountry",
            cache: false,
            async: true,
            data: { regions: regionId },
            beforeSend: function () {
                $.blockUI({
                    timeout: 0,
                    message: '<h1><img src="/Image/ajax-loader.gif" /> Processing...</h1>'
                });
            },
            success: function (response) {
                $.unblockUI();
                $.each(response.returnList, function (i, v) {
                    $('#CountryId').append($('<option>').text(v.Text).attr('value', v.Value));
                });
                $("#StateId").html("<option selected='selected'>Select State</option>");
                $("#CityId").html("<option selected='selected'>Select City</option>");
            },
            complete: function (response) {
                $.unblockUI();

            },
            error: function (response) {
                $.unblockUI();

            }
        });

    });

    $(document).on("change", "#CountryId", function () {
        var regionId = $("#RegionId").val();
        var countryId = $("#CountryId").val();
        if (countryId.length <= 0) {
            $("#StateId").html("<option selected='selected'>Select State</option>");
            $("#CityId").html("<option selected='selected'>Select City</option>");
        }
        //var countryId = $("#CountryId").val();
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "State/LoadState",
            cache: false,
            async: true,
            data: { region: regionId, country: countryId },
            beforeSend: function () {
                $.blockUI({
                    timeout: 0,
                    message: '<h1><img src="/Image/ajax-loader.gif" /> Processing...</h1>'
                });
            },
            success: function (response) {
                $.unblockUI();
                if (response.IsSuccess) {
                    $.each(response.returnList, function (i, v) {
                        $('#StateId').append($('<option>').text(v.Text).attr('value', v.Value));
                    });
                }
                $("#CityId").html("<option selected='selected'>Select City</option>");


            },
            complete: function (response) {
                $.unblockUI();

            },
            error: function (response) {
                $.unblockUI();

            }
        });

    });

    //long region, long country, long state
    $(document).on("change", "#StateId", function () {
        var regionId = $("#RegionId").val();
        var countryId = $("#CountryId").val();
        var stateId = $("#StateId").val();
        if (stateId.length <= 0) {
            $("#CityId").html("<option selected='selected'>Select City</option>");
        }
        $.ajax({
            type: "POST",
            url: $("body").attr("data-project-root") + "City/LoadCity",
            cache: false,
            async: true,
            data: { region: regionId, country: countryId, state: stateId },
            beforeSend: function () {
                $.blockUI({
                    timeout: 0,
                    message: '<h1><img src="/Image/ajax-loader.gif" /> Processing...</h1>'
                });
            },
            success: function (response) {
                $.unblockUI();
                if (response.IsSuccess) {
                    $.each(response.returnList, function (i, v) {
                        $('#CityId').append($('<option>').text(v.Text).attr('value', v.Value));
                    });
                }

            },
            complete: function (response) {
                $.unblockUI();

            },
            error: function (response) {
                $.unblockUI();

            }
        });

    });
});