var targetArray = [];
var queryStringArray = [];

// ==================
// === navigation ===
// ==================

// expand/collapse second level groups
$('#nav-container').on('activate.bs.scrollspy', function () {
	$('#nav-container ul').each(function() {
		if ($(this).find('.active').length != 0 || $(this).parent().hasClass('active'))
			$(this).show();
		else
			$(this).hide();
	});
})

// ===============
// === Invoker ===
// ===============

function Invoker(id) {
	var container = $('#' + id);

	if (!container.is(':visible'))
	{
		// hide all open invokers
		// $('ul.invoker:visible').slideUp('slow');

		// replace placeholder in request object and append token button

		var host = container.find('span:contains("[Host]")').text();
		container.find('span:contains("[Host]")').text(host.replace("[Host]", window.location.host))

		if ($.cookie('accessToken') == undefined) {
			$.cookie('accessToken', GetQueryStringParams('x-bwin-accessId'));
		}
		
		ReplaceToken(container, "x-bwin-accessId", $.cookie('accessToken'));
		ReplaceToken(container, "x-bwin-session-token", $.cookie('sessionToken'));
		ReplaceToken(container, "x-bwin-user-token", $.cookie('userToken'));

		ToggleTokenAlert(container);

		SplitQueryStringIntoArray(container);

		if ($('#' + id + '_editor').length) {
			var editor = ace.edit(id + "_editor");
			editor.setTheme("ace/theme/tomorrow");
			editor.setShowPrintMargin(false);
			editor.getSession().setMode("ace/mode/json");
		}

		container.slideDown('slow', function() {
			$('body').scrollspy('refresh');
		});
	}
	else
	{
		queryStringArray = []; // reset
		targetArray = [];
		container.find('.param-editor input').val('');
		container.slideUp('slow', function() {
			$('body').scrollspy('refresh');
		});
	}

	
}

function ToggleTokenAlert(container) {
	var accessTokenRequired = true;
	var sessionTokenRequired = container.find('code.invokerRequestCode span:contains(x-bwin-session-token)').length > 0;
	var userTokenRequired = container.find('code.invokerRequestCode span:contains(x-bwin-user-token)').length > 0;

	var tokenAlert = container.find('.tokenAlert');

	// accessTokenRequired will always be true, but still
	if (accessTokenRequired && !$.cookie('accessToken') ||
	    sessionTokenRequired && !$.cookie('sessionToken') ||
	    userTokenRequired && !$.cookie('userToken')) {
		tokenAlert.show();
	}
	else {
		tokenAlert.hide();
	}
}

function CallAPI(invokerId, baseUrl, method) {
	var body = '';
	if ($('#' + invokerId + '_editor').length) {
		var editor = ace.edit(invokerId + "_editor");
		body = editor.getValue();
	}

	// show loading
	var button = $('#'+invokerId).find('button.btn-primary');
	var cache = button.children();
	button.text('LOADING ...');

    $.ajax({
      url: window.location.origin + baseUrl + BuildQueryStringFromArray(),
      type: method,
      dataType: 'json',
      contentType: 'application/json; charset=UTF-8', // This is the money shot
      data: body,
      beforeSend: setHeader,
      success: function(res, status, xhr) { HandleResponse(invokerId, xhr, status) },
      error: function(jqxhr, textStatus, error) { HandleResponse(invokerId, jqxhr, textStatus); },
      complete: function() {
		// this has to be called with an each()
	    $('#' + invokerId + '-response-header').each(function(i, block) {
	      hljs.highlightBlock(block);
	    });
	    $('#' + invokerId + '-response-body').each(function(i, block) {
	      hljs.highlightBlock(block);
	    });

	    button.text('SEND').append(cache);

	    $('#' + invokerId + '-response').show();
	    $('#' + invokerId + '-clear').show();
      }
    });

    // DOM Manipulations, so better refresh scrollspy
    $('[data-spy="scroll"]').each(function () {
	  var $spy = $(this).scrollspy('refresh')
	})
}


function HandleResponse(invokerId, jqxhr, textStatus) {
	// clear before adding new response
	$('#' + invokerId + '-response-header').html('');
	$('#' + invokerId + '-response-body').html('');

	// set headers
	$('#' + invokerId + '-response-header').html(
            'HTTP/1.1 ' + jqxhr.status + " " + jqxhr.statusText + "<br>" +
            jqxhr.getAllResponseHeaders()
        );

	// set body
    $('#' + invokerId + '-response-body').html(
        JSON.stringify(jqxhr.responseJSON, null, 2)
    );

    // parse body for new tokens and display info if found
    if(jqxhr.responseJSON && jqxhr.responseJSON.sessionToken && jqxhr.responseJSON.userToken) {
    	var rememberTokens = $('#' + invokerId).find('.rememberTokens');
    	rememberTokens.find('button.remember').click(function() {
    		RememberTokens(jqxhr.responseJSON.sessionToken, jqxhr.responseJSON.userToken);
    		$('#' + invokerId).find('.rememberTokens').hide();
    	});
    	rememberTokens.show();
    }
}


function ParameterInput(invokerId, paramId, parameterName, initialValue) {
	var value = $('#' + invokerId + '_' + parameterName).val();
	var shift = 0;
	if (targetArray.length > 1)
		shift = targetArray.length;

	if (targetArray.length > paramId) {
		targetArray[paramId] = value;
	} else {
		queryStringArray[paramId - shift] = value;
	}

	// update ui
	$('#'+invokerId).find('code.invokerRequestCode').find('span.hljs-request > span').text(BuildQueryStringFromArray());
}


function RememberTokens(sessionToken, userToken) {
	var date = new Date();
	var minutes = 30;
	date.setTime(date.getTime() + (minutes * 60 * 1000));

	$.cookie('sessionToken', sessionToken, { expires: date });
	$.cookie('userToken', userToken, { expires: date });

	$('div.tokenAlert').hide();
}


function SplitQueryStringIntoArray(container) {
	queryStringArray = [];
	targetArray = [];

	var currentTarget = container.find('code.invokerRequestCode .uriTemplate').text();

	// split into parameters
	// > there might be placeholders in the url as well as in the querystring :/

	var splitIntoQuery = currentTarget.split('?');
	
	var target = splitIntoQuery[0].split('{');
	$.each(target, function() {
		$.each(this.split('}'), function() { if (this.length > 0) targetArray.push(this);});
	});

	if (splitIntoQuery.length > 1) {
		var queryString = splitIntoQuery[1].split('=');
		$.each(queryString, function() { 
			$.each(this.split('&'), function() { if (this.length > 0) queryStringArray.push(this);});
		});
	}

		// check for potential problems in documentation which will cause malfunction
	if (targetArray.length > 1 || queryStringArray.length > 1) {
		var params = container.find('.param-editor').find('div.parameterName strong');
		var counter = 1;
		var shift = 0;
		if (targetArray.length > 1)
			shift = targetArray.length;
		$.each(params, function() {

			if (targetArray.length > counter) {
				if (targetArray[counter].indexOf(this.textContent) == -1) {
					container.find('div.parameterWarning').show();
				}
			} else {
				if (queryStringArray[counter - shift].indexOf(this.textContent) == -1) {
					container.find('div.parameterWarning').show();
				}
			}
			counter += 2;
		});
	}

	container.find('code.invokerRequestCode').find('span.hljs-request > span').text(BuildQueryStringFromArray());
}

function BuildQueryStringFromArray() {
	var queryString = "";

	var arrayLength = queryStringArray.length;
	for (var i = 0; i < arrayLength; i = i+2) {
	    if (queryStringArray[i+1].length > 0 && queryStringArray[i+1].indexOf('{') !== 0) {
	    	//element found, add it to the query
	    	if (queryString.length == 0) {
	    		queryString = "?";
	    	} else {
	    		queryString += "&";
	    	}
	    	queryString +=  queryStringArray[i] + "=" + queryStringArray[i+1];
	    }
	}

	return targetArray.join('') + queryString;
}

function Clear(invokerId) {
	$('#' + invokerId + '-response-header').empty();
	$('#' + invokerId + '-response-body').empty();
	$('#' + invokerId + '-clear').hide();
	$('#' + invokerId + '-response').hide();
}


function setHeader(xhr) {
	xhr.setRequestHeader('x-bwin-accessId', $.cookie('accessToken'));
	xhr.setRequestHeader('x-bwin-session-token', $.cookie('sessionToken'));
	xhr.setRequestHeader('x-bwin-user-token', $.cookie('userToken'));
}




// ========================
// === Token Management ===
// ========================

$('#TokenManagement').on('show.bs.modal', function (e) {
	// close alert(s)
	//$('.tokenAlert').parent().parent().hide();

	$('#accessIdInput').val($.cookie('accessToken'));
	$('#sessionTokenInput').val($.cookie('sessionToken'));
	$('#userTokenInput').val($.cookie('userToken'))
})

function SaveTokens() {
	// replace in visible code-blocks
	// we have to rely on naming and structure here :/
	var visibleInvoker = $('ul.invoker:visible');
	ReplaceToken(visibleInvoker, "x-bwin-accessId", $('#accessIdInput').val());
	ReplaceToken(visibleInvoker, "x-bwin-session-token", $('#sessionTokenInput').val());
	ReplaceToken(visibleInvoker, "x-bwin-user-token", $('#userTokenInput').val());

	// save cookies *crunch* *crunch*
	var date = new Date();
	var minutes = 30;
	date.setTime(date.getTime() + (minutes * 60 * 1000));

	$.cookie('accessToken', $('#accessIdInput').val());
	$.cookie('sessionToken', $('#sessionTokenInput').val(), { expires: date });
	$.cookie('userToken', $('#userTokenInput').val(), { expires: date });

	$('#TokenManagement').modal('hide');

	// re-evalute if tokens are OK now
	ToggleTokenAlert(visibleInvoker);
}

function ReplaceToken(invoker,header,value) {
	// we have to rely on naming and structure here :/
	var b= $('<a data-toggle="modal" data-target="#TokenManagement"><span aria-hidden="true" class="glyphicon glyphicon-pencil tokenEdit"></span></a>');

	var span = invoker.find('code.invokerRequestCode').find('span:contains("'+header+'")').next();
	span.text(value);
}

function CloseAndRedirectTo(target) {
	$('#TokenManagement').modal('hide');
	window.location.href = target;
}


function GetQueryStringParams(sParam) {
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++)
    {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0].toLowerCase() == sParam.toLowerCase())
        {
            return sParameterName[1];
        }
    }
}
