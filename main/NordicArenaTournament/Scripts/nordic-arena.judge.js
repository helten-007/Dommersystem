﻿/* Must be loaded after nordic-arena.main */
nordicArena.judge = nordicArena.judge || {};
nordicArena.judge.const = {};
nordicArena.judge.const.bottomSafetyMargin = 10;
nordicArena.judge.const.sliderZeroThreshold = 0.1;
nordicArena.judge.const.sliderOffThreshold = 0.1;

// On load
$(function () {
	nordicArena.judge.initGui();
	//nordicArena.log("nordicArena.judge initializing hub and screen");
	nordicArena.judge.initSignalRHub();
	$("header").hide(); // Optimizing view for tablet
	$("footer").hide();
	// Prevent scrolling by dragging on background of sliders. 
	/*$(".slider-container").bind("touchstart", function (e) {
        e.preventDefault();
    });
    $(".slider-container").bind("touchmove", function (e) {
        e.preventDefault();
    });*/
	nordicArena.judge.hideBrowserChrome();
	//nordicArena.judge.initScreenHeight();
	nordicArena.judge.enableSubmitButton(true); // Oslo Games hack. Need to be able to resubmit
	//nordicArena.log("nordicArena.judge initialize end");
});

nordicArena.judge.initGui = function () {
	nordicArena.judge.selectContestant(9999);
	//nordicArena.log("nordicArena.judge initialize begin");
	nordicArena.judge.tournamentId = $("#tournamentId").val();
	nordicArena.judge.judgeId = $("#judgeId").val();
	if (nordicArena.judge.tournamentId == null) throw "Tournament ID not set";
	if (nordicArena.judge.judgeId == null) throw "Judge ID not set";
	nordicArena.judge.initSliders();
	nordicArena.judge.selectContestant(0);

	var timeoutId = setTimeout(function () {
		nordicArena.judge.initScreenHeight(timeoutId);
	}, 100);

	$('#overlay').find('#btnYes').on('click', function () {
		$('#overlay').hide();
		nordicArena.judge.submitScores(true);
	});
	$('#overlay').find('#btnNo').on('click', function () {
		$('#overlay').hide();
		nordicArena.judge.submitScores(false);
	});
};

nordicArena.judge.hideBrowserChrome = function () {
	// Attempted to fix so that chrome is removed (address bar) and more screeen real estate is revealed on Chrome for Android. Unsuccessful.
	if (nordicArena.isMobileDevice()) {
		//nordicArena.log("Mobile device detected. Queueing hideBrowserChrome()");
		setTimeout(function() {
			//nordicArena.log("Hiding browser chrome");
			window.scrollTo(0, 1);
			nordicArena.judge.initScreenHeight();
		}, 100);
	} //else //nordicArena.log("PC detected. Not hiding browser chrome.");
};

nordicArena.judge.selectContestant = function (contestantIx) {
	$(".input-container").hide();
	$(".input-container:eq(" + contestantIx + ")").show();
	$("#judge-contestant-container").find("button").toggleClass("selected", false);
	$("#judge-contestant-container").find("button:eq(" + contestantIx + ")").toggleClass("selected", true);
};

$(window).resize(function () {
	nordicArena.judge.resetHeight();
});


// Ensure the height of the bars fill the screen and that it is adjusted on change
nordicArena.judge.initScreenHeight = function (timeoutId) {
	//window.onresize = nordicArena.judge.resetHeight;
	nordicArena.judge.resetHeight();
	clearTimeout(timeoutId);
};

nordicArena.judge.reloadPanels = function (tournamentId) {
	if (!nordicArena.common.isCurrentTournament(tournamentId)) return;

	if ($('#judge-status-container').length) {
		nordicArena.judge.loadContestant();
		nordicArena.common.get(nordicArena.judge.judgeStatusReloadUrl).done(function (data) {
			$('#judge-status-container').html(data);
		});
	}
};

nordicArena.judge.initSignalRHub = function () {
	//nordicArena.log("nordicArena.judge.initSignalRHub() begin");
	// Set up client functions
	$.connection.naHub.client.judgeStatusUpdated = nordicArena.judge.reloadPanels;
	$.connection.naHub.client.currentContestantChanged = nordicArena.judge.loadContestant;
	$.connection.naHub.client.runCompleted = nordicArena.judge.onRunCompleted;
	$.connection.hub.error(nordicArena.judge.signalRError); // attach event listener
	$.connection.hub.start().done(function () {
		//nordicArena.log("signalR hub successfully initialized");
		if ($("#tournamentRunning").val()) {
		}
	});
};

nordicArena.judge.loadContestant = function () {
	//nordicArena.log("Judge Reload Page start. URL:" + nordicArena.judge.reloadUrl);
	nordicArena.common.get(nordicArena.judge.reloadUrl).done(function (data) {
		//nordicArena.log("Judge Reload Page data received");
		$("#judge-main-container").html(data);
		nordicArena.judge.initGui();
		//nordicArena.log("Judge Reload Page data done");
	});
};

nordicArena.judge.onRunCompleted = function (tournamentId) {
	//nordicArena.log("nordicArena.judge.onRunCompleted()");
	if (!nordicArena.common.isCurrentTournament(tournamentId)) return;
	nordicArena.judge.enableSubmitButton();  
};

nordicArena.judge.resetHeight = function () {
	//nordicArena.log("nordicArena.judge.resetHeight() begin");

	var height = document.documentElement.clientHeight;
	var containerTop = $(".slider").first().position().top;
	var sliderMargin = $(".slider").first().totalVerticalMargin();

	var sliderButtonHeight = $('.slider-button').first().parent().outerHeight();

	var footerHeight = $("#judging-footer").height();
	var footerMargin = $("#judging-footer").totalVerticalMargin();
	height = height - containerTop - footerHeight - sliderMargin - footerMargin - (sliderButtonHeight * 2);
	height -= nordicArena.judge.const.bottomSafetyMargin; // Remaining "unexplicable" height 
	nordicArena.log("nordicArena.judge.resetHeight() Height: " + height + " - containerTop, footerHeiht, sliderMargin, footerMargin:"  + containerTop + ","  +footerHeight + "," + sliderMargin +"," + footerMargin);
	if (height > 100) {
		$(".slider").height(height);
		//nordicArena.log("resizing to " + height);
	}
	nordicArena.judge.resizeOverlayInner();
	//nordicArena.log("nordicArena.judge.resetHeight() end");
};

nordicArena.judge.signalRError = function(error) {
	nordicArena.judge.showProgressBar(false);
	nordicArena.common.signalRError(error);
};
        
nordicArena.judge.initSliders = function () {
	var sliderCount = $("#criteriaCount").val();
	var contestantCount = $("#contestantCount").val();
	nordicArena.judge.sliderCount = sliderCount;

	for (var contIx = 0; contIx < contestantCount; contIx++) {
		var sliderData = nordicArena.judge.getSliderDataFor(contIx, "master");
		if (sliderData.value)
			nordicArena.judge.initSlider(sliderData, true);

		for (var critIx = 0; critIx < sliderCount; critIx++) {
			var sliderData = nordicArena.judge.getSliderDataFor(contIx, critIx);
			nordicArena.judge.initSlider(sliderData, true);
		}
	}
};

nordicArena.judge.validateScores = function () {
	// Check if judge has forgotten to score all contestants
	var sliderCount = $("#criteriaCount").val();
	var contestantCount = $("#contestantCount").val();
	var contestantNames = [];
	var contestantScores = [];
	var isHeadJudge = false;

	for (var contIx = 0; contIx < contestantCount; contIx++) {
		var scoreCount = 0;
		var contestantName = $("#judge-contestant-container button:eq(" + contIx + ")").text().trim();
		if (contestantName === '' || contestantName === undefined) {
			contestantName = $('#meter_' + contIx + '_master').find('.criteria-name').first().text().trim();
			contestantNames.push(contestantName);
			contestantScores.push(nordicArena.judge.getSliderDataFor(contIx, 'master').value);
			isHeadJudge = true;
		}
		// Find the number of scores actually given (non-null) for all contestantts
		for (var critIx = 0; critIx < sliderCount; critIx++) {
			var sliderData = nordicArena.judge.getSliderDataFor(contIx, critIx);
			if (sliderData.value != "") scoreCount++;
		}
		// If anyone have ZERO scores, show confirm dialog
		if (scoreCount == 0) {
			var warningText = $("#ContestantNoScoreWarning").text();
			var answer = confirm(warningText.replace("{0}", contestantName));
			return answer;
		}
	}

	if (isHeadJudge)
		nordicArena.judge.confirm(contestantNames, contestantScores);
	return !isHeadJudge;
};

nordicArena.judge.resizeOverlayInner = function () {
	var inner = $('#overlay').find('.overlay-inner-container').first();
	inner.height($(window).height() - (inner.offset().top * 2));
};

nordicArena.judge.confirm = function (conts, scores) {
	var overlay = $('#overlay');
	overlay.find('.rider-confirm-name').each(function (index) {
		$(this).find('h2').first().text(conts[index]);
	});

	overlay.find('.rider-confirm-score').each(function (index) {
		$(this).find('h2').first().text(scores[index]);
	});
	overlay.show();
	nordicArena.judge.resizeOverlayInner();

	/*var innerOverlay = $('<div></div>');
	innerOverlay.css({ 'height': '400px', 'width': '400px', 'background': 'white', 'margin': 'auto' });
	innerOverlay.append($('<div></div>').text(text));
	innerOverlay.append($('<button></button>')
		.attr('id', 'btnYes')
		.text('Ja')
		.on('click', function () {
			$('#overlay').remove();
			callback(true);
		}
	));

	innerOverlay.append($('<button></button>')
		.attr('id', 'btnNo')
		.text('Nei')
		.on('click', function () {
			$('#overlay').remove();
			callback(false);
		}
	));
	overlay.append(innerOverlay);
	$('#skel-layers-wrapper').append(overlay);*/
};

nordicArena.judge.getSliderDataFor = function(contestantIx, criteriaIx) {
	var slider = {};
	var index = "_" + contestantIx + "_" + criteriaIx;
	slider.sliderSelector = "#slider" + index;
	slider.amountSelector = "#amount" + index;
	var sliderElem = $(slider.sliderSelector);
	slider.max = nordicArena.common.parseFloat(sliderElem.attr("max"));
	slider.min = nordicArena.common.parseFloat(sliderElem.attr("min"));
	slider.step = nordicArena.common.parseFloat(sliderElem.attr("step"));
	slider.value = $(slider.amountSelector).val();
	slider.valueFloat = nordicArena.common.parseFloat(slider.value);
	return slider;
};

nordicArena.judge.initSlider = function (slider, enableIt) {
	// adding deadzone for value 0.0, and an extra zone for "no value"
	var distance = slider.max - slider.min;
	var zeroThreshold = distance * nordicArena.judge.const.sliderZeroThreshold;
	var totalThresholdBottom =  distance * (nordicArena.judge.const.sliderZeroThreshold + nordicArena.judge.const.sliderOffThreshold);
	var maxToSet = slider.max + zeroThreshold;
	var minToSet = slider.min - totalThresholdBottom;
	// Initializing value to set
	var sliderValue = slider.value;
	if (sliderValue == "") sliderValue = slider.min - totalThresholdBottom;
	var params = {
		orientation: "vertical",
		range: "min",
		min: minToSet,
		max: maxToSet,
		step: slider.step,
		value: sliderValue,
		slide: nordicArena.judge.onSliderUpdated,
		change: nordicArena.judge.onSliderUpdated,
		start: null,
		stop: null,
		disabled: !enableIt
	};
	$(slider.sliderSelector).slider(params);
	nordicArena.judge.updateLabelForSlider(slider.sliderSelector);
};

nordicArena.judge.enableSliders = function(enable) {
	if (enable) {
		$(".slider").slider("enable");
		var enableButton = $("#isCurrentRunDone").val();
		nordicArena.judge.enableSubmitButton(enableButton);
	}
	else {
		$(".slider").slider("disable");
		nordicArena.judge.enableSubmitButton(false);
	}
};

nordicArena.judge.enableSubmitButton = function(enable) {
	nordicArena.enableButton("#judge-submit", enable);
};

nordicArena.judge.showProgressBar = function (show) {
	$("progress").toggle(show);
	$("#judge-contestant-container").toggle(!show);
};

nordicArena.judge.onSliderUpdated = function (event, ui) {
	// Update text of self

	var element = this;
	var sliderId = element.id;
	var jQueryElement = $('#' + sliderId);

	setTimeout(function () {
		nordicArena.judge.updateLabelForSlider("#" + sliderId);
		var splitStr = sliderId.split('_');
		var contIx = splitStr[1];
		if (splitStr[2] == 'master') {
			var sliderCount = $("#criteriaCount").val();
			for (var index = 0; index < sliderCount; index++) {
				var slider = $("#slider_" + contIx + '_' + index);
				slider.slider("value", jQueryElement.slider('value'));
			}
		}
	}, 10); // Workaround for laggy updating of the value property
};

var intervalId;
var intervalIds = [];
var element;
var startSpeed = 200;
var speed = startSpeed;

$('.slider-button').mousedown(function () {
	element = $(this)[0];
	intervalId = setTimeout(function () {
		nordicArena.judge.adjustSliderRecursive();
	}, startSpeed);
}).bind('mouseup mouseleave', function () {
	clearTimeout(intervalId);
	nordicArena.judge.clearTimeouts();
	speed = startSpeed;
});

nordicArena.judge.clearTimeouts = function () {
	for (var i = 0; i < intervalIds.length; i++) {
		clearTimeout(intervalIds[i]);
	}
};

nordicArena.judge.adjustSliderRecursive = function () {
	nordicArena.judge.adjustSlider(element, element.value);
	intervalIds.push(setTimeout(nordicArena.judge.adjustSliderRecursive, speed));
	speed = speed > 5 ? speed * 0.9 : 5;
};

nordicArena.judge.adjustSlider = function (element, inAmount) {
	var amount = parseFloat(inAmount);
	var splitTxt = amount > 0 ? 'plus' : 'minus';
	var elem = element.id.split(splitTxt);
	var sliderId = 'slider' + elem[1];
	var slider = $("#" + sliderId);
	slider.slider("value", slider.slider("value") + amount);
	return false;
};

nordicArena.judge.updateLabelForSlider = function (sliderSelector) {
	var sliderRes = $(sliderSelector);
	var sliderLabelTag = "#" + sliderRes[0].attributes.labelref.value;
	var sliderLabelRes = $(sliderLabelTag);
	var rawVal = sliderRes.slider("value");
	// Crop the value to within the threshold
	var max = sliderRes.attr("max");
	var min = sliderRes.attr("min");
	var decimals = sliderRes.attr("decimals");
	var val = Math.max(min, Math.min(max, rawVal));
	var valStr = Globalize.format(parseFloat(val), "N" + decimals);
	var enableValue = false;
	if (nordicArena.judge.isInOffZone(rawVal, min, max)) {
		valStr = "";
		enableValue = true;
	}
	sliderLabelRes.val(valStr);
	sliderLabelRes.toggleClass("disabled", enableValue);
	var contestantIndex = sliderRes.attr("contIx");
	nordicArena.judge.updateAverageValue(contestantIndex, decimals);
};

nordicArena.judge.updateAverageValue = function (contestantIx, decimals) {
	var average = nordicArena.judge.getAverageValue(contestantIx, decimals);
	var selector = "#average_" + contestantIx;
	if (average == "NaN") average = String.fromCharCode(160);// &nbsp;
	$(selector).text(average);
	nordicArena.judge.updateCloseOpponents(average, contestantIx, decimals);
};

nordicArena.judge.getAverageValue = function (contestantIx, decimals) {
	var slidersWithValueCount = 0;
	var sum = 0;
	for (var i = 0; i < nordicArena.judge.sliderCount; i++) {
		var sliderData = nordicArena.judge.getSliderDataFor(contestantIx, i);
		if (sliderData.value != "") {
			slidersWithValueCount++;
			sum += sliderData.valueFloat;
		}
	}
	var average = sum / slidersWithValueCount;
	average = Globalize.format(parseFloat(average), "N" + decimals);
	return average;
};

nordicArena.judge.renderCloseOpponents = function (inAverage, contestantName, contestantIx) {
	nordicArena.judge.sortCloseOpponents(inAverage, contestantName);

	var hiddenElement = $('#closest-contestants_' + contestantIx + ' #hidden-contestants li').clone();
	var visibleElement = $('#closest-contestants_' + contestantIx + ' #visible-contestants');

	var score = 0.0;
	var name = "";
	var position = "";
	var array = [];
	var tempElem = {};

	hiddenElement.each(function () {
		position = $(this).find('.close-cont-pos').first().text();
		score = nordicArena.common.parseFloat($(this).find('.close-cont-score').text());
		name = $(this).find('.close-cont-name').first().text();

		if (name === contestantName)
			if (inAverage > score)
				score = inAverage;

		tempElem = {
			position: position,
			score: score,
			name: name
		};
		array.push(tempElem);
	});

	array = nordicArena.judge.sortCloseOpponents(array);
	var nameIndex = 0;
	$.each(array, function (index, value) {
		if (value.name === contestantName)
			nameIndex = index;
	});

	var newElement = $('#closest-contestants_' + contestantIx + ' #hidden-contestants').clone();
	newElement.empty();

	var index = nameIndex - 5 >= 0 ? nameIndex - 5 : 0;
	hiddenElement.each(function (i) {
		if (index < (nameIndex + 5) || i < 10) {
			$(this).find('.close-cont-pos').first().text((index + 1));
			$(this).find('.close-cont-score').text(array[index].score);
			$(this).find('.close-cont-name').first().text(array[index].name);

			if (nameIndex === index)
				$(this).css('background', '#03AAEC');

			$(this).clone().show().appendTo(newElement);
			index++;
		} else {
			return false;
		}
	});

	visibleElement.html('');
	newElement.show().children().appendTo(visibleElement);
}

nordicArena.judge.sortCloseOpponents = function (array) {
	var results = [];
	for (var i = 0; i < array.length ; i++) {
		results.push(array[i]);
	}
	results.sort(function (a, b) {
		return b.score - a.score;
	});
	return results;
};

nordicArena.judge.updateCloseOpponents = function (inAverage, contestantIx, decimals) {
	setTimeout(function () {
		var newAverage = nordicArena.judge.getAverageValue(contestantIx, decimals);
		if (inAverage == newAverage) {
			inAverage = nordicArena.common.parseFloat(inAverage); 
			
			var contestantName = $('#meter_' + contestantIx + '_master').find('.criteria-name').first().text();
			//var contestantName = $("#judge-contestant-container button:eq(" + contestantIx + ")").text().trim().split(' ')[0];
			nordicArena.judge.renderCloseOpponents(inAverage, contestantName, contestantIx);
		}
	}, 1000);
};

nordicArena.judge.isInOffZone = function (val, min, max) {
	var distance = max - min;
	var zoneStart = 0 - nordicArena.judge.const.sliderZeroThreshold * distance;    
	return val <= zoneStart;
};

nordicArena.judge.getSliderDataForDidNotSkate = function (contestantIx, criteriaIx) {
	var slider = {};
	var index = "_" + contestantIx + "_" + criteriaIx;

	slider.amountSelector = "#amount" + index;
	$(slider.amountSelector).val(-1);
};

nordicArena.judge.setDidNotSkateScore = function (contIx) {
	var sliderCount = $("#criteriaCount").val();
	var contestantCount = $("#contestantCount").val();
	for (var critIx = 0; critIx < sliderCount; critIx++) {
		var sliderData = nordicArena.judge.getSliderDataForDidNotSkate(contIx, critIx);
	}
	var sliderData = nordicArena.judge.getSliderDataForDidNotSkate(contIx, 'master');
};

nordicArena.judge.didNotSkate = function (contIx) {
	nordicArena.judge.setDidNotSkateScore(contIx);
};

nordicArena.judge.submitScores = function (ok) {
	if (ok === '' || ok === undefined)
		ok = nordicArena.judge.validateScores();
	if (ok) {
		nordicArena.judge.enableSliders(false);
		nordicArena.judge.showProgressBar(true);
		nordicArena.postForm("form", {
			success: nordicArena.judge.onSubmitScoreSuccess,
			error: nordicArena.judge.onSubmitScoreError,
			timeout: 10000
		});
	}
};

nordicArena.judge.onSubmitScoreSuccess = function (data, status, xhr) {
	nordicArena.judge.showProgressBar(false);
};

nordicArena.judge.onSubmitScoreError = function (xhr, errorText, errorThrown) {
	nordicArena.judge.showProgressBar(false);
	nordicArena.judge.enableSliders(true);
};

$.fn.extend( {
	totalVerticalMargin: function () {
		var top = parseFloat(this.css('marginTop').replace("px", ""));
		var bottom = parseFloat(this.css('marginBottom').replace("px", ""));
		return top + bottom;
	}
});