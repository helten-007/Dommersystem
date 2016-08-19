nordicArena.results = nordicArena.results || {};
var timeOut = 0;
var interval = 0;

// On load
$(function () {
	nordicArena.results.initSignalRHub();
	nordicArena.results.initScroll();

	$(".main-content").css("padding-bottom", "0em");
	if ($('#body-container').innerHeight() > $(window).height()) {
		$(".main-content").css("padding-bottom", "5em");
	}

});

nordicArena.results.initSignalRHub = function () {
	nordicArena.log("nordicArena.judge.initSignalRHub() begin");
	// Set up client functions
	$.connection.naHub.client.resultsUpdated = nordicArena.results.update;
	$.connection.hub.error(nordicArena.results.signalRError); // attach event listener
	$.connection.hub.start().done(function () {
		nordicArena.log("signalR hub successfully initialized");
		if ($("#tournamentRunning").val()) {
		}
	});
};

nordicArena.results.update = function (tournamentId) {
	clearInterval(timeOut);
	clearInterval(interval);
	if (!nordicArena.common.isCurrentTournament(tournamentId)) return;
	nordicArena.common.get(nordicArena.results.reloadUrl).done(function (data) {
		$('.main-content').html(data);
	});
	nordicArena.results.initScroll();
};

nordicArena.results.initScroll = function () {
	clearInterval(timeOut);
	timeOut = setTimeout(function () { nordicArena.results.scroll(); }, 2000);
};

nordicArena.results.scroll = function () {
	var div = $('body');
	var direction = 1;
	var minWait = 50;
	var maxWait = 3000;
	var wait = minWait;

	var myFunction = function () {
		wait = minWait;
		clearInterval(interval);
		
		var pos = div.scrollTop();
		pos += direction;
		div.scrollTop(pos);

		if (div[0].scrollHeight - div.scrollTop() == div.outerHeight() || div.scrollTop() <= 0) {
			direction *= -1;
			wait = maxWait;
		}
		interval = setInterval(myFunction, wait);
	}
	interval = setInterval(myFunction, wait);
};