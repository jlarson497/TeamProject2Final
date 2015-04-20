﻿Imports System.Net
Imports System.IO
Imports System.Xml

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

    Private Function getCityLat(city As String, state As String, coord As Double)
        'This function gets and returns the latitude from the center of the city you search
        'At this time I am forgetting why I thought I needed "coord as double" as a parameter
        'it's not being used, and will cause an error if it's not included.  Test this with internet
        Dim mapURL As String = mapstringBuilder(city, state)
        Dim webRequestCoordinates As WebRequest = WebRequest.Create(mapURL)
        Dim coordinateResponseStream As Stream = webRequestCoordinates.GetResponse.GetResponseStream()
        Dim xmlCoordinateReader As New XmlDocument
        xmlCoordinateReader.Load(coordinateResponseStream)
        'not sure if the following is the correct node, will need to double check to be sure
        Dim xmlCoordinateLatitude As XmlNode = xmlCoordinateReader.SelectSingleNode("//Point/Latitude")
        Dim latitude As Double = xmlCoordinateLatitude.InnerText

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

    Private Function getCityLong(city As String, state As String, coord As Double)
        'This function gets the longitude from the center of the city and returns it
        Dim mapURL As String = mapstringBuilder(city, state)
        Dim webRequestCoordinates As WebRequest = WebRequest.Create(mapURL)
        Dim coordinateResponseStream As Stream = webRequestCoordinates.GetResponse.GetResponseStream()
        Dim xmlCoordinateReader As New XmlDocument
        xmlCoordinateReader.Load(coordinateResponseStream)

        Dim xmlCoordinateLongitude As XmlNode = xmlCoordinateReader.SelectSingleNode("//Point/Longitude")
        Dim longitude As Double = xmlCoordinateLongitude.InnerText

        Return longitude
    End Function

    Public Function coordBuilder(longitude As String, latitude As String)
        'This takes the separate long and lat in from the labels under each pictures and puts them together
        Dim fullLongLat As String = longitude & "," & latitude
        Return fullLongLat
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




    End Function



End Class
