<script async defer
    src="//maps.googleapis.com/maps/api/js?key=@this.Configuration[" GoogleMapsApi: API_Key"]&callback=initialize" >
    </script >
    <script>
        function initialize() {
            var map;
            var bounds = new google.maps.LatLngBounds();
            var mapOptions = {
            mapTypeId: 'roadmap'
            };

            // Display a map on the page
            map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
            map.setTilt(45);

            // Multiple Markers
            var markers = [
            @foreach (var town in @Model.Towns)
            {
               @:['@town.Name', @town.Latitude, @town.Longitude],
            }

            ];
            // Info Window Content
            var infoWindowContent = [
                @foreach (var town in @Model.Towns)
            {
               @:['<div class="info_content">' + '<h3>@town.Name</h3>' + '<p>@town.DoctorsCount Доктори</p>' + '</div>'],
            }
            ];

            // Display multiple markers on a map
            var infoWindow = new google.maps.InfoWindow(), marker, i;

            // Loop through our array of markers & place each one on the map
            for (i = 0; i < markers.length; {
                var position = new google.maps.LatLng(markers[i][1], markers[i][2]);
                bounds.extend(position);
                marker = new google.maps.Marker({
            position: position,
                    map: map,
                    title: markers[i][0]
                });

                // Allow each marker to have an info window
                google.maps.event.addListener(marker, 'click', (function (marker, i) {
                    return function () {
            infoWindow.setContent(infoWindowContent[i][0]);
                        infoWindow.open(map, marker);
                    }
                })(marker, i));

                // Automatically center the map fitting all markers on the screen
                map.fitBounds(bounds);
            }

            // Override our map zoom level once our fitBounds function runs (Make sure it only runs once)
            var boundsListener = google.maps.event.addListener((map), 'bounds_changed', function (event) {
            this.setZoom(7);
                google.maps.event.removeListener(boundsListener);
            });
            //HTML 5 Geolocation
                if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    var pos = {
                        lat: position.coords.latitude,
                        lng: position.coords.longitude,
                    };
                    infoWindow.setPosition(pos);
                    infoWindow.setContent("ВИЕ СТЕ ТУК.");
                    infoWindow.open(map);
                    map.setCenter(pos);
                },
                () => {
                    handleLocationError(true, infoWindow, map.getCenter());
                }
            );
                } else {
            // Browser doesn't support Geolocation
            handleLocationError(false, infoWindow, map.getCenter());
                }
            };


        function handleLocationError(browserHasGeolocation, infoWindow, pos) {
            infoWindow.setPosition(pos);
            infoWindow.setContent(
                browserHasGeolocation
                ? "Error: The Geolocation service failed."
                : "Error: Your browser doesn't support geolocation."
            );
            infoWindow.open(map);
        }
    </script>