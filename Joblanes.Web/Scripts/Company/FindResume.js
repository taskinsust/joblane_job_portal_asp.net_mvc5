function dataTableRender() {
    var pageSize = 10;
    if ($("#pageSize").val() != "" && /^\d+$/.test($("#pageSize").val())) {
        pageSize = parseInt($("#pageSize").val());
    }
    else {
        $("#pageSize").val(pageSize);
    }

    var jobTitle = $("#jobTitle .checkbox:checked").map(function () {
        return this.value;
    }).get();
    var yearExp = $("#yearExp .checkbox:checked").map(function () {
        return this.value;
    }).get();
    var education = $("#education .checkbox:checked").map(function () {
        return this.value;
    }).get();
    var company = $("#company .checkbox:checked").map(function () {
        return this.value;
    }).get();

    $("#DataGrid").dataTable({
        destroy: true,
        "processing": true,
        searching: false,
        serverSide: true,
        "scrollX": true,
        "bLengthChange": false,
        "bSort": false,
        //stateSave: true,
        order: [[0, "desc"]],
        ajax: {
            url: $("body").attr("data-project-root") + "Company/FindResumeList",
            type: "POST",
            data: function (d) {
                d.what = $("#what").val();
                d.where = $("#where").val();
                d.jobTitle = jobTitle;
                d.yearExp = yearExp;
                d.education = education;
                d.company = company;

            }
        }
    });
}
$(document).ready(function () {
    dataTableRender();    
});

$(document).on("click", "#search", function () {
    dataTableRender();
});
$(document).on("click", ".checkbox", function () {
    var chck = $(this);

    chck.prop("checked", !chck.prop("checked"));
    console.log(chck.prop("checked"));
    if (chck.prop("checked")) {
        $(this).addClass("list-group-item-success");
    } else {
        $(this).removeClass("list-group-item-success");
    }
});
$(document).on("click", ".list-group-item", function() {
    var chck = $(this).find(".checkbox");
    
    chck.prop("checked", !chck.prop("checked"));
    console.log(chck.prop("checked"));
    if (chck.prop("checked")) {
        $(this).addClass("list-group-item-success");
    } else {
        $(this).removeClass("list-group-item-success");
    }
});

$(document).on("click", "#filter", function () {
    dataTableRender();
});

