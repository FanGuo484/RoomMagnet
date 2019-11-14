﻿var map;

var dataTable1;

var chart1;

var click = false;

function initMap() {
    if (click === false) {
        map = new google.maps.Map(document.getElementById('map'), {
            zoom: 11,
            center: { lat: 38.4495688, lng: -78.8689156 },
            disableDefaultUI: true
        });
    }
    else {
        var inputvalue = document.getElementById('address').value;
        if (isNaN(inputvalue)) {
            PageMethods.QueryToJsonForCityState(inputvalue, addressmap)
        }
        else {
            PageMethods.QueryToJsonForZip(inputvalue, addressmap)
        }
    }
}

function geocodeAddress() {
    click = true;
}
function addressmap(response) {
    dataTable1 = eval(response);
    dataTable1 = google.visualization.arrayToDataTable(dataTable1);
    map = new google.maps.Map(document.getElementById('map'), {
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        zoom: 11,
    });
    var opt = { minZoom: 5, maxZoom: 15 };
    map.setOptions(opt);
    var latlng = new google.maps.LatLng(38.4495688, -78.8689156);
    map.setCenter(latlng);

    var Jsonaddress = dataTable1.getDistinctValues(0);
    var cities = dataTable1.getValue(0, 1);
    var stateProvince = dataTable1.getValue(0, 2);
    var JsonZipCode = dataTable1.getValue(0, 3);

    var geocoder = new google.maps.Geocoder();
    for (var i = 0; i < Jsonaddress.length; i++) {
        var newaddress = Jsonaddress[i] + "," + cities + "," + stateProvince + "," + JsonZipCode;
        geocoder.geocode({ 'address': newaddress }, onGeocodeResponse);
    }
}
function onGeocodeResponse(response, status) {
    // the Geocode service has sent its response. We can now use it for the map
    if (status == google.maps.GeocoderStatus.OK) {
        // center the map at the location returned from the geocoding service
        map.setCenter(response[0].geometry.location);

        // set up the store names for the city to display in marker tool tip
        var storesInCity = "\n";
        for (var i = 0; i < dataTable1.getNumberOfRows(); i++) {
            if (response[0].address_components[0].long_name == dataTable1.getValue(i, 1))
                storesInCity += "\n" + dataTable1.getValue(i, 0);
        }

        var image = {
            url: 'https://cis366fanguo.s3.amazonaws.com/icon.png',
            size: new google.maps.Size(71, 71),
            origin: new google.maps.Point(0, 0),
            anchor: new google.maps.Point(17, 34),
            scaledSize: new google.maps.Size(25, 25)
        };
        // put a marker at the specified location
        var marker = new google.maps.Marker({
            map: map,
            position: response[0].geometry.location,
            animation: google.maps.Animation.DROP,
            icon: image,
            title: 'Property is near this neighborhood'
            //title: response[0].address_components[0].long_name + storesInCity
        });
        marker.addListener('click', toggleBounce);
    }
    else {
        alert('Geocode was not successful for the following reason: ' + status);
    }
}
function toggleBounce() {
    if (marker.getAnimation() !== null) {
        marker.setAnimation(null);
    } else {
        marker.setAnimation(google.maps.Animation.BOUNCE);
    }
}
window.onload = initMap;