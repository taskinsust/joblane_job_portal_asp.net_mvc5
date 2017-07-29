var CUSTOM_MESSAGE_CONTAINER_CLASS = ".customMessage";
var DISPLAY_TIME_SUCCESS = 3000;
var DISPLAY_TIME_ERROR = 3000;
var DISPLAY_TIME_WARNING = 3000;
var DISPLAY_TIME_INFO = 3000;

var setTimeOutObj;



//$.fn.customMessage({
    //displayMessage: "Something went wrong",
    //displayMessageType: "danger",
    //displayTime: 10000
//});
//displayMessageType = error/e,success/s,info/i,warning/w

$.fn.customMessage = function(options) {
    var settings = $.extend({
        displayMessage: "Something went wrong",
        displayMessageType: "danger",
        displayTime: 0,
        messageHolder: ""
    }, options);


    if (settings.displayMessageType == "" || settings.displayMessageType == undefined || settings.displayMessageType.toLowerCase() == "error" || settings.displayMessageType == "e")
        settings.displayMessageType = "danger";
    else if (settings.displayMessageType=="s") {
        settings.displayMessageType = "success";
    }
    else if (settings.displayMessageType == "i") {
        settings.displayMessageType = "info";
    }
    else if (settings.displayMessageType == "w") {
        settings.displayMessageType = "warning";
    }
    //console.log(displayMessageType);
    settings.displayTime = parseInt(settings.displayTime);
    if (settings.displayTime == undefined || settings.displayTime <= 0) {
        if (settings.displayMessageType == "success") {
            settings.displayTime = DISPLAY_TIME_SUCCESS;
        }
        else if (settings.displayMessageType == "danger") {
            settings.displayTime = DISPLAY_TIME_ERROR;
        }
        else if (settings.displayMessageType == "info") {
            settings.displayTime = DISPLAY_TIME_INFO;
        }
        else if (settings.displayMessageType == "warning") {
            settings.displayTime = DISPLAY_TIME_WARNING;
        }
    }



    if (settings.displayMessage == "" || settings.displayMessage == undefined) settings.displayMessage = "Something went wrong";


    var displayMessageDiv = '<div class="alert alert-' + settings.displayMessageType + '" style="display:none"><a class="close" data-dismiss="alert">×</a>' + settings.displayMessage + '</div>';
    
    if (settings.messageHolder != "") {
        CUSTOM_MESSAGE_CONTAINER_CLASS = settings.messageHolder;
    }


    if ($(CUSTOM_MESSAGE_CONTAINER_CLASS).length > 0) {

        $(CUSTOM_MESSAGE_CONTAINER_CLASS).find(".alert-" + settings.displayMessageType).remove();

        $(CUSTOM_MESSAGE_CONTAINER_CLASS).append(displayMessageDiv).find(".alert-" + settings.displayMessageType).fadeIn(1000);

        window.clearTimeout(setTimeOutObj);

        setTimeout(function() {
            $(CUSTOM_MESSAGE_CONTAINER_CLASS).find(".alert-" + settings.displayMessageType).fadeOut(2000, function () { $(this).remove(); });
        }, settings.displayTime);

    } else {
        alert(settings.displayMessage);
    }   

}


function ShowErrorMessage(message) {
    try {
        var msg = '<div class="alert alert-danger"><a class="close" data-dismiss="alert">×</a>' + message + '</div>';
        if ($(CUSTOM_MESSAGE_CONTAINER_CLASS).length > 0) {
            $(CUSTOM_MESSAGE_CONTAINER_CLASS).append(msg);
        } else {
            alert(message);
        }
    } catch (ex) {
        alert(ex+"");
    }
}

function removeMessage() {
    var displayTime = 0;

    var childrenClass = $(CUSTOM_MESSAGE_CONTAINER_CLASS).children().attr("class");
    if (childrenClass != undefined) {
        console.log(childrenClass);
        if ((childrenClass.indexOf("alert-success") > -1)) {
            console.log((childrenClass.indexOf("alert-success") > -1));
            displayTime = DISPLAY_TIME_SUCCESS;
        } else if ((childrenClass.indexOf("alert-danger") > -1)) {
            console.log((childrenClass.indexOf("alert-danger") > -1));
            displayTime = DISPLAY_TIME_ERROR;
        } else if ((childrenClass.indexOf("alert-info") > -1)) {
            console.log((childrenClass.indexOf("alert-info") > -1));
            displayTime = DISPLAY_TIME_INFO;
        } else if ((childrenClass.indexOf("alert-warning") > -1)) {
            console.log((childrenClass.indexOf("alert-warning") > -1));
            displayTime = DISPLAY_TIME_WARNING;
        } else {
            displayTime = DISPLAY_TIME_SUCCESS;
        }
    } else {
        displayTime = DISPLAY_TIME_SUCCESS;
    }
    
    //console.log($(CUSTOM_MESSAGE_CONTAINER_CLASS).children());
    //console.log($(CUSTOM_MESSAGE_CONTAINER_CLASS).find(".alert-success"));
    //if ($(CUSTOM_MESSAGE_CONTAINER_CLASS).find(".alert-success")) {
    //    console.log("success");
    //    displayTime = DISPLAY_TIME_SUCCESS;
    //}
    //else if ($(CUSTOM_MESSAGE_CONTAINER_CLASS).find(".alert-danger")) {
    //    console.log("e");
    //    displayTime = DISPLAY_TIME_ERROR;
    //}
    //else if ($(CUSTOM_MESSAGE_CONTAINER_CLASS).find(".alert-info")) {
    //    console.log("i");
    //    displayTime = DISPLAY_TIME_INFO;
    //}
    //else if ($(CUSTOM_MESSAGE_CONTAINER_CLASS).find(".alert-warning")) {
    //    console.log("w");
    //    displayTime = DISPLAY_TIME_WARNING;
    //}
    //console.log(displayTime);

    setTimeOutObj = window.setTimeout(function () {
        $(CUSTOM_MESSAGE_CONTAINER_CLASS).find("div").first().fadeOut(3000, function () { $(this).remove(); });
        removeMessage();
    }, displayTime);
}

$(document).ready(function () {
    removeMessage();

    //var pathname = window.location.pathname;
    //pathname = pathname.replace($("body").attr("data-project-root"), "");
    //var areaName = "Administration";
    //var controllerName = "Home";
    //var actionName = "Index";

    //$.each(pathname.split("/"), function (index, value) {
    //    if (index == 0) areaName = value;
    //    else if (index == 1) controllerName = value;
    //    else if (index == 2) actionName = value;
    //});

    //$('#sidebar #accordion .panel-collapse').removeClass("in");
    //$("#sidebar .submenuitem").each(function () {
    //    //var regEx = new RegExp("/" + controllerName + "/");
    //    //var regExActions = new RegExp("/" + actionName + "/");
    //    var name = controllerName + "//" + actionName;
    //    var regEx = new RegExp("/" + name + "/");
    //    console.log(name);

    //    if (regEx.test($(this).attr("href").toString())) {

    //        $(this).parents(".panel-collapse").addClass("in");
    //        var latString = $(this).attr("href").split("/");
    //        //console.log(latString[latString.length-1]);
    //        //console.log(actionName);

    //       // var contains = (latString[latString.length - 1].indexOf(actionName) > -1); //return bool
    //        if (latString[latString.length - 1] == actionName) {
    //            $(this).css("font-weight", "bold");
    //        }

    //    }
    //});
    //console.log(window.location.pathname);
    //console.log(document.referrer);
    var pathname = window.location.pathname;
    pathname = pathname.replace($("body").attr("data-project-root"), "");
    var areaName = "Administration";
    var controllerName = "Home";
    var actionName = "Index";
    var isMultipleReferer = 0;
    var isSubMenuFound = 0;

    $.each(pathname.split("/"), function (index, value) {
        if (index == 0) areaName = value;
        else if (index == 1) controllerName = value;
        else if (index == 2) actionName = value;
    });

    var refName = document.referrer;
    refName = refName.replace("https://", "");
    refName = refName.replace("http://", "");
    var refAreaName = "Administration";
    var refControllerName = "Home";
    var refActionName = "Index";

    $.each(refName.split("/"), function (index, value) {
        if (index == 1) refAreaName = value;
        else if (index == 2) refControllerName = value;
        else if (index == 3) refActionName = value;
    });
    $.each(refActionName.split("?"), function (index, value) {
        if (index == 0) refActionName = value;
    });
    $('#sidebar #accordion .panel-collapse').removeClass("in");


    $("#sidebar .submenuitem").each(function () {
        var fullControllerActionName = controllerName + "-" + actionName + "|";    
        if ($(this).attr("data-businessId")) {
            if ($(this).attr("data-businessId").toString().toLowerCase().indexOf(fullControllerActionName.toLowerCase()) >= 0) {                
                if ($(this).attr("data-businessId").toString().toLowerCase().indexOf(actionName.toLowerCase()) >= 0) {                    
                    isMultipleReferer++;
                }
            }
        }
    });

    if (isMultipleReferer < 2) {
        $("#sidebar .submenuitem").each(function () {
            var fullControllerActionName = controllerName + "-" + actionName + "|";
            //console.log("========================");
            //console.log(fullControllerActionName);
            //console.log($(this).attr("data-businessId").toString());
            //console.log("-----------------------");
            //console.log(fullControllerActionName.toLowerCase());
            //console.log($(this).attr("data-businessId").toString().toLowerCase());
            if ($(this).attr("data-businessId")) {
                if ($(this).attr("data-businessId").toString().toLowerCase().indexOf(fullControllerActionName.toLowerCase()) >= 0) {
                    $(this).parents(".panel-collapse").addClass("in");
                    if ($(this).attr("data-businessId").toString().toLowerCase().indexOf(actionName.toLowerCase()) >= 0) {
                        $(this).css("font-weight", "bold");
                        isSubMenuFound = 1;
                    }
                }
            }
        });
    }
    else {
        $("#sidebar .submenuitem").each(function () {
            var refControllerActionName = refControllerName + "-" + refActionName + "|";
            var fullControllerActionName = controllerName + "-" + actionName + "|";
            //console.log(refControllerActionName);
            //console.log(fullControllerActionName);
            if ($(this).attr("data-businessId")) {
                if (($(this).attr("data-businessId").toString().toLowerCase().indexOf(fullControllerActionName.toLowerCase()) >= 0) && ($(this).attr("data-businessId").toString().toLowerCase().indexOf(refControllerActionName.toLowerCase()) >= 0)) {
                    $(this).parents(".panel-collapse").addClass("in");
                    if ($(this).attr("data-businessId").toString().toLowerCase().indexOf(actionName.toLowerCase()) >= 0) {
                        $(this).css("font-weight", "bold");
                        isSubMenuFound = 1;
                    }
                }
            }
        });

    }

    if (isSubMenuFound == 0) {
        $("#sidebar .submenuitem").each(function () {
            var regEx = new RegExp("/" + controllerName + "/");
            if (regEx.test($(this).attr("href").toString())) {
                $(this).parents(".panel-collapse").addClass("in");
                if ($(this).attr("href").toString().toLowerCase().indexOf(actionName.toLowerCase()) >= 0) {
                    $(this).css("font-weight", "bold");
                }
            }
        });
    }

    $('.numericInputOnly').on('input', function () {
        this.value = this.value.replace(/[^0-9.]/g, '').replace(/(\..*)\./g, '$1');
    });
});