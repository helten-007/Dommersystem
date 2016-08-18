nordicArena.results = nordicArena.results || {};

// On load
$(function () {
	nordicArena.results.initSignalRHub();
	//nordicArena.results.initScroll();
	$("header").hide(); // Optimizing view for tablet
	$(".banner").hide();
	$(".floatright.noprint").hide();
	$("footer").hide();

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
	if (!nordicArena.common.isCurrentTournament(tournamentId)) return;
	nordicArena.common.get(nordicArena.results.reloadUrl).done(function (data) {
		$('.main-content').html(data);
	});
};

nordicArena.results.initScroll = function () {
	var timeOut = setTimeout(function () { nordicArena.results.scroll(); }, 2000);
	clearInterval(timeOut);
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
			console.log("VENTER!!!");
		}
		interval = setInterval(myFunction, wait);
	}
	var interval = setInterval(myFunction, wait);
};