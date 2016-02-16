var gameState = "DEFAULTGAME";
var playerChoice = "DEFAULTGAME";

$(document).ajaxComplete(function () {
    if (gameState != playerChoice && (gameState == "DEFAULTGAME" || gameState == "ASSASSINWIN" || gameState == "PRAETORIANWIN") == false) {
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
                playerSideChoosen: radio.val()
            },
            dataType: "html",
            success: function (data) {
                ShowBoard(data, true);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    });
});

function ShowPositions(id, index, img, color) {
    $(id).prepend('<img class="centerImage highlightNumber" alt=' + color + ' id="' + index + '" src="' + img + '" draggable="true" ondragstart="drag(event)" />');
};

function ShowPositionsPost(id, index, img, color) {
    $('#' + id).prepend('<img class="centerImage highlightNumber" alt=' + color + ' id="' + index + '" src="' + img + '" draggable="true" ondragstart="drag(event)" />');
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
            playerSideChoosen: playerChoice
        },
        dataType: "html",
        success: function (data) {
            ShowBoard(data, false);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("statusPlayer: " + xhr.status);
            alert(thrownError);
        }
    });
}

function computerMove()
{
    $.ajax({
        url: '/Praetorian/ComputerMove',
        type: 'POST',
        data: {
            playerSideChoosen: playerChoice
        },
        dataType: "html",
        success: function (data) {
            ShowBoard(data, false);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("statusComputer: " + xhr.status);
            alert(thrownError);
        }
    });
}

function ShowBoard(data, gameStart) {
    var json = JSON.parse(data);
    if (json.IsLegalMove) {
        $('.centerImage').remove();

        for (var i = 0; i < json.Pieces.length; i++) {
            if (json.Pieces[i].Piece == 100) {
                ShowPositionsPost(json.Pieces[i].Position, json.Pieces[i].Index, "/Content/Assets/PurpleTower.png", "PRAETORIAN");
            }
            else if (json.Pieces[i].Piece > 0 && json.Pieces[i].Piece < 100) {
                ShowPositionsPost(json.Pieces[i].Position, json.Pieces[i].Index, "/Content/Assets/Numbers/" + json.Pieces[i].Piece + ".png", json.Pieces[i].Piece);
            }
        }

        ChangeSides(json.GameState, gameStart);
    }
}

function ChangeSides(sideMoved, gameStart) {

    if (gameStart) {
        gameState = ShowState(sideMoved);
        $('#txtMessage').val($('#txtMessage').val() + ShowState(sideMoved) + '\r\n');
    }
    else {
        if ((ShowState(sideMoved) == "DEFAULTGAME" || ShowState(sideMoved) == "ASSASSINWIN" || ShowState(sideMoved) == "PRAETORIANWIN") == false) {
            gameState = ShowState(sideMoved);
            $('#txtMessage').val($('#txtMessage').val() + ShowState(sideMoved) + '\r\n');
        }
        else {
            $('#txtMessage').val($('#txtMessage').val() + ShowState(sideMoved) + '\r\n');
            gameState = "DEFAULTGAME";
            playerChoice = "DEFAULTGAME";
        }
    }
    var sc = $('#txtMessage');
    if (sc.length)
        sc.scrollTop(sc[0].scrollHeight - sc.height());
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