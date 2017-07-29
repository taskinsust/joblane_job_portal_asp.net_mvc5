$(document).ready(function () {

    $(document).on("click", ".block", function () {
        var aspNetUserId = $(this).attr('data-netuser');
        if (aspNetUserId != "") {
            bootbox.confirm("<h3>Are you sure you want to <span class='confirm-message'> Block this user</span> </h3>", function (result) {
                if (result) {
                    $.ajax({
                        type: "post",
                        url: $("body").attr("data-project-root") + "Admin/BlockUser",
                        cache: false,
                        async: true,
                        data: { aspNetUserId: aspNetUserId },
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
    });

    $(document).on("click", ".unblock", function () {
        var aspNetUserId = $(this).attr('data-netuser');
        if (aspNetUserId != "") {
            bootbox.confirm("<h3>Are you sure you want to <span class='confirm-message'> UnBlock this user</span> </h3>", function (result) {
                if (result) {
                    $.ajax({
                        type: "post",
                        url: $("body").attr("data-project-root") + "Admin/UnBlockUser",
                        cache: false,
                        async: true,
                        data: { aspNetUserId: aspNetUserId },
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
    });
});