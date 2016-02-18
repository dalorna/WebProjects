var gameState = "DEFAULTGAME";
var playerChoice = "DEFAULTGAME";
var boardData = null;
var questionedPiece = null;

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
                if (playerChoice == 'ASSASSINTURN')
                {
                    ShowAssassinInfo();
                }
                var btnStart = $("#btn")[0];
                btnStart.disabled = true;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    });

    $('#btnInterrogate').click(function () {

        if ($('.highlightSuspect').length != 1 || questionedPiece == null)
            return;

        $.ajax({
            url: '/Praetorian/Interrogate',
            type: 'POST',
            data: { positionQuestioned: questionedPiece },
            dataType: 'html',
            success: function (data) {
                var json = JSON.parse(data);
                boardData = json;
                ChangeSides(json.GameState, false);
                ShowSuspectsQuestioned();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    });

    $('#btnAssassinate').click(function () {

        if ($('.highlightSuspect').length != 1 || questionedPiece == null)
            return;

        $.ajax({
            url: '/Praetorian/Assassinate',
            type: 'POST',
            data: { positionKilled: questionedPiece },
            dataType: 'html',
            success: function (data) {
                var json = JSON.parse(data);
                boardData = json;
                ChangeSides(json.GameState, false);
                ShowAssassinInfo();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    });

});

function SetBoard(bData)
{
    boardData = bData;
}

function ShowPositions(id, index, img, color) {
    $(id).prepend('<img class="centerImage highlightNumber" alt=' + color + ' id="' + index + '" src="' + img + '" draggable="true" ondragstart="drag(event)" onclick="imgClick(this)" onmouseover="imgOver(this)" onmouseout="imgOut(this)" />');
};

function ShowPositionsPost(id, index, img, color, questioned) {
    var highlight = "highlightNumber";
    if (questioned)
        highlight = "highlightQuestioned";

    $('#' + id).prepend('<img class="centerImage ' + highlight + '" alt=' + color + ' id="' + index + '" src="' + img + '" draggable="true" ondragstart="drag(event)" onclick="imgClick(this)" onmouseover="imgOver(this)" onmouseout="imgOut(this)"/>');
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
    boardData = json;
    if (json.IsLegalMove) {
        $('.centerImage').remove();

        for (var i = 0; i < json.Pieces.length; i++) {
            if (json.Pieces[i].Piece == 100) {
                ShowPositionsPost(json.Pieces[i].Position, json.Pieces[i].Index, "/Content/Assets/PurpleTower.png", "PRAETORIAN", json.Pieces[i].HasBeenQuestioned);
            }
            else if (json.Pieces[i].Piece > 0 && json.Pieces[i].Piece < 100) {
                ShowPositionsPost(json.Pieces[i].Position, json.Pieces[i].Index, "/Content/Assets/Numbers/" + json.Pieces[i].Piece + ".png", json.Pieces[i].Piece, json.Pieces[i].HasBeenQuestioned);
            }
        }

        ChangeSides(json.GameState, gameStart);
    }
}

function ChangeSides(sideMoved, gameStart) {
    var side = parseInt(sideMoved);

    if (gameStart) {
        gameState = ShowState(side);
        $('#txtMessage').val($('#txtMessage').val() + ShowState(side) + '\r\n');
    }
    else {
        if ((ShowState(side) == "DEFAULTGAME" || ShowState(side) == "ASSASSINWIN" || ShowState(side) == "PRAETORIANWIN") == false) {
            gameState = ShowState(side);
            $('#txtMessage').val($('#txtMessage').val() + ShowState(side) + '\r\n');
        }
        else {
            $('#txtMessage').val($('#txtMessage').val() + ShowState(side) + '\r\n');
            gameState = "DEFAULTGAME";
            playerChoice = "DEFAULTGAME";
        }
    }
    var sc = $('#txtMessage');
    if (sc.length)
        sc.scrollTop(sc[0].scrollHeight - sc.height());
}

function ShowSuspectsQuestioned()
{
    var text = "";
    if (boardData != null) {
        for (var i = 0; i < boardData.Pieces.length; i++) {
            if (boardData.Pieces[i].Piece > 0 && boardData.Pieces[i].Piece < 100 && boardData.Pieces[i].HasBeenQuestioned) {
                text += 'Citzen: ' + boardData.Pieces[i].Piece + '\r\n';
            }
        }
    }
    $('#txtMoves').val(text);
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

function imgClick(img)
{
    if ($(img).hasClass() && $('#btnPraetorain')[0].checked)
        return;

    $('img').removeClass('highlightSuspect');
    $('img:not(.highlightQuestioned)').addClass('highlightNumber');
    
    questionedPiece = null;
    var Id = parseInt(img.id);
    if(boardData != null)
    {
        var possibleInterviews = [Id - 9, Id - 8, Id - 7, Id - 1, Id + 1, Id + 7, Id + 8, Id + 9];
        var allLines = [[]];
        for (var i = 0; i < gameLines.length; i++) {
            allLines.push(gameLines[i]);
        }
        for (var i = 0; i < diagonalLines.length; i++) {
            allLines.push(diagonalLines[i]);
        }

        if ($('#btnPraetorain')[0].checked) {

            var canQuestion = false;
            var praetorians = [];
            for (var i = 0; i < boardData.Pieces.length; i++) {
                if (boardData.Pieces[i].Piece == 100) {
                    praetorians.push(boardData.Pieces[i]);
                }
            }
            for (var a = 0; a < allLines.length; a++) {
                var line = allLines[a];
                var firstCop = $.inArray(praetorians[0].Index, line);
                var secondCop = $.inArray(praetorians[1].Index, line);
                var suspect = $.inArray(Id, line);
                if ((firstCop >= 0 && suspect >= 0) || (secondCop >= 0 && suspect >= 0)) {
                    if (firstCop >= 0 && Math.abs(firstCop - suspect) == 1) {
                        canQuestion = true;
                        break;
                    }
                    else {
                        if (Math.abs(secondCop - suspect) == 1) {
                            canQuestion = true;
                            break;
                        }
                    }
                }
            }
            if (canQuestion) {
                $(img).removeClass('highlightNumber').addClass('highlightSuspect');
                questionedPiece = boardData.Pieces[Id];
            }
        }
        else {
            var canKill = false;
            var targets = [];
            var assin;
            for (var i = 0; i < boardData.Pieces.length; i++) {
                if (boardData.Pieces[i].IsAssassin) {
                    assin = boardData.Pieces[i]
                    break;
                }
            }
            for (var i = 0; i < boardData.Pieces.length; i++) {
                if (boardData.Pieces[i].IsTarget) {
                    targets.push(boardData.Pieces[i])
                }
            }
            var isTarget = false;
            for (var i = 0; i < targets.length; i++) {
                if (targets[i].Index == Id) {
                    isTarget = true;
                    break;
                }
            }

            if(isTarget == false)
                return;

            for (var a = 0; a < allLines.length; a++) {
                var line = allLines[a];
                var firstTarget = $.inArray(targets[0].Index, line);
                var secondTargetIndex = targets[1] != null ? targets[1].Index : -100;
                var secondTarget = $.inArray(secondTargetIndex, line);

                var assassin = $.inArray(assin.Index, line);

                var targetClick;
                if (targets[0].Index == Id)
                    targetClick = firstTarget;
                else if (secondTargetIndex == Id)
                    targetClick = secondTarget;
                else {
                    return;
                }

                if ((targetClick >= 0 && assassin >= 0)) {
                    if (Math.abs(targetClick - assassin) == 1) {
                        canKill = true;
                        break;
                    }
                }
            }

            if(canKill)
            {
                $(img).removeClass('highlightNumber').addClass('highlightSuspect');
                questionedPiece = boardData.Pieces[Id];
            }
        }
    }
}

function imgOver(img)
{
    //$(img).removeClass('highlightNumber').addClass('highlightO');
}

function imgOut(img)
{
    //$(img).removeClass('highlightO').addClass('highlightNumber');
}

function aChange(rad) {
    var btnA = $('#btnAssassin');
    var btnI = $('#btnInterrogate');
    var btnAinate = $('#btnAssassinate');
    var lbl = $('#lblPlayer');

    if (btnA[0].checked) {
        btnI.addClass('hidden');
        btnAinate.removeClass('hidden');
        lbl.text('Assassin Information')
    }
    else {
        btnI.removeClass('hidden');
        btnAinate.addClass('hidden');
        lbl.text('Questioned Pieces');
    }
}

function ShowAssassinInfo()
{
    $('#txtMoves').val("");
    var count = 1;
    for (var i = 0; i < boardData.Pieces.length; i++) {
        if (boardData.Pieces[i].IsTarget) {
            $('#txtMoves').val($('#txtMoves').val() + 'Target ' + count.toString() + ': ' + boardData.Pieces[i].Piece + '\r\n');
            count++;
        }
        if (boardData.Pieces[i].IsAssassin) {
            $('#txtMoves').val($('#txtMoves').val() + 'Assassin: ' + boardData.Pieces[i].Piece + '\r\n');
        }
    }
}

var gameLines = 
[[0, 1, 2, 3, 4, 5, 6, 7],
 [8, 9, 10, 11, 12, 13, 14, 15 ],
 [16, 17, 18, 19, 20, 21, 22, 23 ],
 [24, 25, 26, 27, 28, 29, 30, 31 ],
 [32, 33, 34, 35, 36, 37, 38, 39 ],
 [40, 41, 42, 43, 44, 45, 46, 47 ],
 [48, 49, 50, 51, 52, 53, 54, 55 ],
 [56, 57, 58, 59, 60, 61, 62, 63 ],
 [0, 8, 16, 24, 32, 40, 48, 56],
 [1, 9, 17, 25, 33, 41, 49, 57],
 [2, 10, 18, 26, 34, 42, 50, 58],
 [3, 11, 19, 27, 35, 43, 51, 59],
 [4, 12, 20, 28, 36, 44, 52, 60],
 [5, 13, 21, 29, 37, 45, 53, 61],
 [6, 14, 22, 30, 38, 46, 54, 62],
 [7, 15, 23, 31, 39, 47, 55, 63]];

var diagonalLines =
[[1, 8,-100,-100,-100,-100,-100, -100 ],
 [3, 10, 17, 24,-100,-100,-100, -100 ],
 [5, 12, 19, 26, 33, 40,-100, -100 ],
 [7, 14, 21, 28, 35, 42, 49, 56 ],
 [23, 30, 37, 44, 51, 58,-100, -100 ],
 [39, 46, 53, 60,-100,-100,-100, -100 ],
 [55, 62,-100,-100,-100,-100,-100, -100 ],
 [5, 14, 23,-100,-100,-100,-100, -100 ] ,
 [3, 12, 21, 30, 39,-100, -100, -100 ],
 [1, 10, 19, 28, 37, 46, 55, -100 ],
 [8, 17, 26, 35, 44, 53, 62, -100 ],
 [24, 33, 42, 51, 60,-100,-100, -100 ],
 [40, 49, 58,-100,-100,-100,-100, -100 ],
 [2, 9, 16, -100, -100, -100, -100, -100 ],
 [4, 11, 18, 25, 32, -100, -100, -100 ],
 [6, 13, 20, 27, 34, 41, 48, -100 ],
 [15, 22, 29, 36, 43, 50, 57, -100 ],
 [31, 38, 45, 52, 59, -100, -100, -100 ],
 [47, 54, 61, -100, -100, -100, -100, -100 ],
 [48, 57, -100, -100, -100, -100, -100, -100 ],
 [32, 41, 50, 59, -100, -100, -100, -100 ],
 [16, 25, 34, 43, 52, 61, -100, -100 ],
 [0, 9, 18, 27, 36, 45, 54, 63 ],
 [2, 11, 20, 29, 38, 47, -100, -100 ],
 [4, 13, 22, 31, -100, -100, -100, -100 ],
 [6, 15, -100, -100, -100, -100, -100, -100 ]];