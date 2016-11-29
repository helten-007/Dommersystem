// Javascript used by the speaker Area
nordicArena.speaker = {};
nordicArena.speaker.startTimer = function () {
    nordicArena.speaker.timerStartedAt = Date.now();
    nordicArena.speaker.intervalHandle = setInterval(nordicArena.speaker.updateTimer, 100);
};

nordicArena.speaker.init = function () {
	nordicArena.speaker.initHub();
	setTimeout(function () {
		nordicArena.speaker.initHeight();
	}, 200);
};


nordicArena.speaker.initHeight = function () {
	$("footer").hide();
	$('.current-rider-container').each(function () {
		$(this).outerHeight(nordicArena.speaker.getHighestRiderCont());
	});

	var screenHeight = document.documentElement.clientHeight;
	var position = $('#next-contestants').offset();

	$('#next-contestants').outerHeight(screenHeight - position.top - 1);
	$('#side-bar').outerHeight(screenHeight - $('header').outerHeight() - 1);

};

nordicArena.speaker.getHighestRiderCont = function () {
	var maxHeight = 0;
	$('.current-rider-container').each(function () {
		var temp = $(this).outerHeight();
		if (temp > maxHeight) {
			maxHeight = temp;
		}
	});
	return maxHeight;
};

nordicArena.speaker.updateTimer = function (totalSeconds) {
    if (totalSeconds == undefined) {
        // Load seconds by calculating start time
        totalSeconds = nordicArena.speaker.secondsPerRun;
        var timePassed = (Date.now() - nordicArena.speaker.timerStartedAt);
        totalSeconds -= timePassed / 1000;
        if (totalSeconds <= 0) {
            totalSeconds = 0;
            clearInterval(nordicArena.speaker.intervalHandle);
            nordicArena.speaker.onTimerComplete();
        }
    }
    var minutes = pad(Math.floor(totalSeconds / 60), 2);
    var seconds = pad(Math.floor(totalSeconds % 60), 2);
    var tens = pad(Math.floor((totalSeconds*10)%10), 1);
    $(".speaker-timer-text").text(minutes + ":" + seconds + "." + tens);
};

nordicArena.speaker.initTimer = function (totalSeconds) {
    nordicArena.speaker.secondsPerRun = totalSeconds;
    nordicArena.speaker.updateTimer(totalSeconds);
};

nordicArena.speaker.reloadPanels = function (tournamentId) {
	if (!nordicArena.common.isCurrentTournament(tournamentId)) return;
	// Some optimization potential here... reload entire model in one request instead of three. 
    nordicArena.common.get(nordicArena.speaker.judgeStatusReloadUrl).done(function (data) {
        $("#judge-status-container").html(data);
    });
    /*nordicArena.common.get(nordicArena.speaker.runControlsReloadUrl).done(function (data) {
        $("#speaker-contestant-controls").html(data);
    });*/
    nordicArena.common.get(nordicArena.speaker.contestantListReloadUrl).done(function (data) {
        $("#current-riders").html(data);
    });
};

nordicArena.speaker.initHub = function () {
    $.connection.naHub.client.judgeStatusUpdated = nordicArena.speaker.reloadPanels;
    $.connection.hub.error(nordicArena.common.signalRError);
    $.connection.hub.start();
};

nordicArena.speaker.onTimerComplete = function () {
    var tournamentId = $("#tournamentId").val();
    $.connection.naHub.server.setCurrentRunDone(tournamentId);  
};