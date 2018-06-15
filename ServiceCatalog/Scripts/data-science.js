$(document).ready(function () {
    $("#spinner").bind("ajaxSend", function () {
        $(this).show();
    }).bind("ajaxStop", function () {
        $(this).hide();
    }).bind("ajaxError", function () {
        $(this).hide();
    });
});

$(document).ready(function () {
    $("#delete").click(function () {
        $("#spinner").show();
    });
    $("#save").click(function () {
        $("#spinner").show();
    });
});

$(document).ready(function () {
    $(".show-button").click(function (event) {
        $("#myModal").modal("show");
        $("#myModalLabel").text("Access List");
        $("#outputsModalBody").val($.trim(event.target.children[0].innerText));
    });
});

$(document).ready(function () {
    $(".show-comment-text").click(function (event) {
        $("#myModal").modal("show");
        $("#myModalLabel").text("Comments");
        $("#outputsModalBody").val($.trim(event.target.innerText));
    });
});