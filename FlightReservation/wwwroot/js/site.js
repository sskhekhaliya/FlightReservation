function clearInput(e) {
    $(e).val('');
}

function getAirportName(e) {
    setTimeout(function () {
        var airportName = $("option[value='" + $(e).val() + "']")[0].text;
        $(e).next().val(airportName);
        $("#" + $(e).attr("id") + "-airport-name").text(airportName);
    }, 100);
}

function passwordVisibilty(e) {
    if ($(e).parent().children('input').attr("type") == "password") {

        $($(e).parent().children('input').attr("type", "text"));
        $($(e).parent().children('span')).text("visibility_off");

    } else {

        $($(e).parent().children('input').attr("type", "password"));
        $($(e).parent().children('span')).text("visibility");
    }
}

