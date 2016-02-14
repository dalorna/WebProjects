$(document).ready(function () {
    $('#appName').text("Praetorian");

    $("#btn").click(function () {
        var radio = $('#PraetorianForm input[type=radio]:checked');

        $.ajax({
            url: '/Praetorian/StartGame',
            type: 'POST',
            data: {
                PlayerSideChoosen: radio.val()
            },
            dataType: "html",
            success: function (data) {
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    });
});

function ShowPositions(id, pos, img, color) {
    $(id).prepend('<img class="centerImage highlightNumber" alt=' + color + ' id="' + pos + '" src="' + img + '" draggable="true" ondragstart="drag(event)" />');
};

function allowDrop(ev) {
    ev.preventDefault();
}

function drag(ev) {
    ev.dataTransfer.setData("text", ev.target.id);
    movingPiece = ev.target;
}

function drop(ev) {
    ev.preventDefault();
    var dat = ev.dataTransfer.getData("text");
    var id = ev.target.id.substring(3, 5);
    $.ajax({
        url: '/Praetorian/PlayerMove',
        type: 'POST',
        data: {
            fromPosition: movingPiece.id,
            toPosition: id,
        },
        dataType: "html",
        success: function (data) {
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("statusPlayer: " + xhr.status);
            alert(thrownError);
        }
    });

    ev.target.appendChild(document.getElementById(dat));
}