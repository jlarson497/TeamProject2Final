Imports System.Net
Imports System.IO

Public Class oMap

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



End Class
