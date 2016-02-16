var gameState = "DEFAULTGAME";
var playerChoice = "DEFAULTGAME";

$(document).ajaxComplete(function () {
    if (gameStatus != playerChoice || gameState != "DEFAULTGAME" || gameState != "ASSASSINWIN" || gameState != "PRAETORIANWIN") {
        computerMove();
    }
});

$(document).ready(function () {
    $('#appName').text("Praetorian");

    $("#btn").click(function () {
        var radio = $('#PraetorianForm input[type=radio]:checked');
        playerChoice = radio.val();
        radio.disabled = true;

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
    if (gameState == "DEFAULTGAME" || playerChoice != gameState)
        return;

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
}

function computerMove()
{

}

function ShowBoard(data) {
    var json = JSON.parse(data);
    if (json.IsLegalMove) {
        $('.centerImage').remove();

        for (var i = 0; i < json.Pieces.length; i++) {
            if (json.Pieces[i].Piece == 100) {
                ShowPiece(json.Pieces[i].Position, json.Pieces[i].Index, "/Content/Assets/PurpleTower.png", json.Pieces[i].Color);
            }
            else if (json.Pieces[i].Color > 0 && json.Pieces[i].Color < 100) {
                ShowPiece(json.Pieces[i].Position, json.Pieces[i].Index, "/Content/Assets/Numbers/" + json.Pieces[i].Color + ".png", json.Pieces[i].Color);
            }
        }

        if (gameStatus == "ASSASSINTURN") {
            gameStatus = "PRAETORIANTURN"
        }
        else if (gameStatus == "PRAETORIANTURN") {
            gameStatus = "ASSASSINTURN";
        }
        else {
            alert('GameStatus is not changing correctly make an adjustment');
        }
        $('#txtMessage').val($('#txtMessage').val() + ShowState(json.GameState) + '\r\n');

        var sc = $('#txtMessage');
        if (sc.length)
            sc.scrollTop(sc[0].scrollHeight - sc.height());
    }
}

function ShowState(i) {
    switch (i) {
        case 0:
            return 'DEFAULTGAME';
        case 1:
            return 'ASSASSINTURN';
        case 2:
            return 'PRAETORIANTURN';
        case 3:
            return 'ASSASSINWIN';
        case 4:
            return 'PRAETORIANWIN';
        case 5:
            return 'DRAW'
    }
}