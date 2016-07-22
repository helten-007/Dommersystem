nordicArena.results = nordicArena.results || {};

// On load
$(function () {
	nordicArena.results.initSignalRHub();
	$("header").hide(); // Optimizing view for tablet
	$(".banner").hide();
	$(".floatright.noprint").hide();
	$("footer").hide();
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
	if (!nordicArena.common.isCurrentTournament(tournamentId))
	{
		console.log("funker ikke helt");
		return;
	}
	console.log('nordicArena.results.reloadUrl: ' + nordicArena.results.reloadUrl);
	nordicArena.common.get(nordicArena.results.reloadUrl).done(function (data) {
		$('.main-content').html(data);
	});
};