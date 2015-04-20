Imports System.Net
Imports System.IO
Imports System.Xml
Imports System.Net.WebRequestMethods

Public Class oMap


    'How to extract coordinates from bing maps: http://stackoverflow.com/questions/8943787/get-latitude-longitude-from-an-address-c-sharp
    'My bing api key: An-81Wbif7kcwdVz7O47yB-qL873cvss4xlu5agBGySz9TGgL62UNqZNmKgIA5EO

    Public Function makeMap(city As String, state As String)
        'Use string builder to build url for map request
        Dim mapURL As String = mapstringBuilder(city, state)
        Dim webRequestMap As WebRequest = WebRequest.Create(mapURL)

        'get response stream
        Dim mapResponseStream As Stream = webRequestMap.GetResponse.GetResponseStream()
        Dim mapImage As Image = Image.FromStream(mapResponseStream)
        'fill image box with the map

        Return mapImage

    End Function

    Private Function mapstringBuilder(city As String, state As String)
        'this function reads the city and state text and combo boxes, builds the map url and then returns it

        Dim finalURL As String = "http://dev.virtualearth.net/REST/V1/Imagery/Map/Road/" & city & "%20" & state & "?mapLayer=TrafficFlow&key=An-81Wbif7kcwdVz7O47yB-qL873cvss4xlu5agBGySz9TGgL62UNqZNmKgIA5EO"
        Return finalURL
    End Function

    Private Function mapStringBuilderXMLPage(city As String, state As String)
        'this function reads in the city and state and builds a url that displays a xml page so that I can get
        'the coordinates for the center of the city.  We will use these coordinates to build a radius from which
        'to get photos
        'Dim xmlURL As String = "http://dev.virtualearth.net/REST/V1/Locations/US/" & city & "%20" & state & "?o=xml&key=An-81Wbif7kcwdVz7O47yB-qL873cvss4xlu5agBGySz9TGgL62UNqZNmKgIA5EO"
        Dim testURL As String = "http://dev.virtualearth.net/REST/v1/Locations/US/%20Minneapolis%20MN?o=xml&key=An-81Wbif7kcwdVz7O47yB-qL873cvss4xlu5agBGySz9TGgL62UNqZNmKgIA5EO"

        Return testURL
    End Function

    Public Function getCityLat(city As String, state As String)
        'This function gets and returns the latitude from the center of the city you search
        'At this time I am forgetting why I thought I needed "coord as double" as a parameter
        'it's not being used, and will cause an error if it's not included.  Test this with internet
        Dim mapURL As String = mapStringBuilderXMLPage(city, state)
        Dim webRequestCoordinates As WebRequest = WebRequest.Create(mapURL)
        Dim coordinateResponseStream As Stream = webRequestCoordinates.GetResponse.GetResponseStream()
        Dim xmlCoordinateReader As New XmlDocument
        xmlCoordinateReader.Load(coordinateResponseStream)
        'not sure if the following is the correct node, will need to double check to be sure
        Dim xmlCoordinateLatitude As XmlNode = xmlCoordinateReader.SelectSingleNode("//Response/ResourceSets/ResourceSet/Resources/Location/Point/Latitude")
        Dim latitude As String = xmlCoordinateLatitude.InnerText

        Return latitude

        'Dim webRequestForecast As WebRequest = WebRequest.Create(url_request_built(city, state))
        ' '' comes back as a stream, prepare a variable to handle/process stream
        'Dim forecastResponseStream As Stream = webRequestForecast.GetResponse.GetResponseStream()
        'Dim xmlWeatherReader As New XmlDocument
        'xmlWeatherReader.Load(forecastResponseStream)
        ' '' from the response stream i need to be able to read.
        'Dim xmlday As XmlNode = xmlWeatherReader.SelectSingleNode("//forecastday[" & Day & "]/title")
        'Dim daytoreturn As String = xmlday.InnerText
    End Function

    Public Function getCityLong(city As String, state As String)
        'This function gets the longitude from the center of the city and returns it
        Dim mapURL As String = mapStringBuilderXMLPage(city, state)
        Dim webRequestCoordinates As WebRequest = WebRequest.Create(mapURL)
        Dim coordinateResponseStream As Stream = webRequestCoordinates.GetResponse.GetResponseStream()
        Dim xmlCoordinateReader As New XmlDocument
        xmlCoordinateReader.Load(coordinateResponseStream)

        Dim xmlCoordinateLongitude As XmlNode = xmlCoordinateReader.SelectSingleNode("Longitude")
        Dim longitude As String = xmlCoordinateLongitude.InnerText

        Return longitude
    End Function

    Public Function coordBuilder(longitude As String, latitude As String)
        'This takes the separate long and lat in from the labels under each pictures and puts them together
        Dim fullLatLong As String = latitude & "," & longitude
        Return fullLatLong
    End Function

    Public Function urlStringBuilder(pinList As ArrayList)
        Dim count As Integer = 1
        'This turns each longlat set into a string usable by the bing api to return a map with pins
        'need to be in the format beginningurlstuff.net/imagery/pp=pp_1&pp_2&(maybe more pins)&maplayer
        'Where pp is the pushpin string of "###.###,###.###"
        For Each pin As String In pinList
            pin = pin & "pp_" & count
            count += 1
        Next

        'Now I need to build a full string, concatonating the reformatted pins
        'A regular string.join did not work because that works with arrays
        'I found the solution here: http://stackoverflow.com/questions/213295/how-do-i-create-a-comma-delimited-string-from-an-arraylist
        'Then return the list to be used in the final url

        Dim finalPinString As String = String.Join(",", CType(pinList.ToArray(Type.GetType("System.String")), String()))
        Return finalPinString



    End Function



End Class
