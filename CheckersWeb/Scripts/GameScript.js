var movingPiece;
var gameStatus = "STARTGAME";

$(document).ajaxComplete(function () {
    if (gameStatus == "BLACKTURN") {
        BlackMove();
    }
});

$(document).ready(function () {
    $("#btn").click(function () {
        $.ajax({
            url: '/Checkers/Index',
            type: 'POST',
            data: {
                colorMovingPiece: "BLACKPAWN",
                fromPosition: "",
                toPosition: "",
                gameState: "DEFAULTGAME"
            },
            dataType: "html",
            success: function (data) {
                $('.centerImage').remove();
                gameStatus = "STARTGAME";
                $('#txtMessage').val('');
                ShowBoard(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    });
});

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
        url: '/Checkers/Index',
        type: 'POST',
        data: {
            colorMovingPiece: movingPiece.alt,
            fromPosition: movingPiece.id,
            toPosition: id,
            gameState: "WHITETURN"

        },
        dataType: "html",
        success: function (data) {
            ShowBoard(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("statusPlayer: " + xhr.status);
            alert(thrownError);
        }
    });
}

function BlackMove() {
    $.ajax({
        url: '/Checkers/Index',
        type: 'POST',
        data: {
            colorMovingPiece: "NULL",
            fromPosition: "65",
            toPosition: "65",
            gameState: "BLACKTURN"

        },
        dataType: "html",
        success: function (data) {
            ShowBoard(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("statusComputer: " + xhr.status);
            alert(thrownError);
        }
    });
}

function ShowBoard(data) {
    var json = JSON.parse(data);
    if (json.IsLegalMove) {
        $('.centerImage').remove();
        for (var i = 0; i < json.Pieces.length; i++) {
            if (json.Pieces[i].Color == 1) {
                ShowPiece(json.Pieces[i].Position, json.Pieces[i].Index, "/Content/Assets/BlackPiece.png", json.Pieces[i].Color);
            }
            else if (json.Pieces[i].Color == 2) {
                ShowPiece(json.Pieces[i].Position, json.Pieces[i].Index, "/Content/Assets/WhitePiece.png", json.Pieces[i].Color);
            }
            else if (json.Pieces[i].Color == 3) {
                ShowPiece(json.Pieces[i].Position, json.Pieces[i].Index, "/Content/Assets/BlackKing.png", json.Pieces[i].Color);
            }
            else if (json.Pieces[i].Color == 4) {
                ShowPiece(json.Pieces[i].Position, json.Pieces[i].Index, "/Content/Assets/WhiteKing.png", json.Pieces[i].Color);
            }
        }

        if(gameStatus == "WHITETURN")
        {
            gameStatus = "BLACKTURN"
        }
        else if (gameStatus == "BLACKTURN" || gameStatus == "STARTGAME")
        {
            gameStatus = "WHITETURN";
        }
        $('#txtMessage').val($('#txtMessage').val() + ShowState(json.GameState) + '\r\n');

        var sc = $('#txtMessage');
        if (sc.length)
            sc.scrollTop(sc[0].scrollHeight - sc.height());
    }
}

function ShowPiece(id, pos, img, color) {
    $('#' + id).removeClass('highlight').prepend('<img class="centerImage" alt=' + color + ' id="' + pos + '" src="' + img + '" draggable="true" ondragstart="drag(event)" />');

};

function ShowBlack(id, pos, img, color) {
    $(id).addClass('highlight').prepend('<img class="centerImage" alt=' + color + ' id="' + pos + '" src="' + img + '" draggable="true" ondragstart="drag(event)" />');
};

function ShowState(i)
{
    switch(i)
    {
        case 0:
            return 'DEFAULTGAME';
        case 1:
            return 'BLACKTURN';
        case 2:
            return 'WHITETURN';
        case 3:
            return 'BLACKWIN';
        case 4:
            return 'WHITEWIN';
        case 5:
            return 'DRAW'
    }
}