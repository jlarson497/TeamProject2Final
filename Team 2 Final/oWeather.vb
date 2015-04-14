Imports System.Net
Imports System.Xml
Imports System.IO

Public Class oWeather
    'weather class

    ' build a web request for 3 day forecast off of this.
    ' http://api.wunderground.com/api/ede6553743c4d570/forecast/q/MO/St_Louis.xml
    ' a function that returns the vlaue of the txtbox input


    '' the function that builds the url request from user input
    Public Function url_request_built(city As String, state As String) As String
        'Dim test_url As String = "http://api.wunderground.com/api/ede6553743c4d570/forecast/q/MO/St_Louis.xml"
        Try
            Dim ThreeDayForecastRequest As String = "http://api.wunderground.com/api/ede6553743c4d570/forecast/q" & "/" & state + "/" + city + ".xml"
            Return ThreeDayForecastRequest
        Catch problemBuildintUrl As Exception
            MessageBox.Show("Problem generating webrequest. Check that all data has been entered correctly", "Error processing request", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return 0
        End Try
    End Function


    '' as a background worker i should be able to call diffferent web request. So they should be queued up here.

    Public Sub getWeather(city As String, state As String)
        ' the test url
        ' Try
        'the web request
        Dim webRequestForecast As WebRequest = WebRequest.Create(url_request_built(city, state))
        '' comes back as a stream, prepare a variable to handle/process stream
        Dim forecastResponseStream As Stream = webRequestForecast.GetResponse.GetResponseStream()
        Dim xmlWeatherReader As New XmlDocument
        xmlWeatherReader.Load(forecastResponseStream)
        '' from the response stream i need to be able to read.

    End Sub

    Public Function getForecastday(city As String, state As String, day As Integer)
        Dim webRequestForecast As WebRequest = WebRequest.Create(url_request_built(city, state))
        '' comes back as a stream, prepare a variable to handle/process stream
        Dim forecastResponseStream As Stream = webRequestForecast.GetResponse.GetResponseStream()
        Dim xmlWeatherReader As New XmlDocument
        xmlWeatherReader.Load(forecastResponseStream)
        '' from the response stream i need to be able to read.
        Dim xmlday As XmlNode = xmlWeatherReader.SelectSingleNode("//forecastday[" & day & "]/title")
        Dim daytoreturn As String = xmlday.InnerText

        Return daytoreturn


    End Function

    Public Function getForcastIcon(city As String, state As String, day As Integer)
        Dim webRequestForecast As WebRequest = WebRequest.Create(url_request_built(city, state))
        '' comes back as a stream, prepare a variable to handle/process stream
        Dim forecastResponseStream As Stream = webRequestForecast.GetResponse.GetResponseStream()
        Dim xmlWeatherReader As New XmlDocument
        xmlWeatherReader.Load(forecastResponseStream)
        '' from the response stream i need to be able to read.
        Dim xmlIcon As XmlNode = xmlWeatherReader.SelectSingleNode("//forecastday[" & day & "]/icon_url")
        Dim icon_url As String = xmlIcon.InnerText
        Dim iconWebRequest As WebRequest = WebRequest.Create(icon_url)
        Dim webimageResonse As Stream = iconWebRequest.GetResponse.GetResponseStream()
        Dim icontoreturn As Image = Image.FromStream(webimageResonse)
        Return icontoreturn

    End Function

    Public Function getForcastDescription(city As String, state As String, day As Integer)
        Dim webRequestForecast As WebRequest = WebRequest.Create(url_request_built(city, state))
        '' comes back as a stream, prepare a variable to handle/process stream
        Dim forecastResponseStream As Stream = webRequestForecast.GetResponse.GetResponseStream()
        Dim xmlWeatherReader As New XmlDocument
        xmlWeatherReader.Load(forecastResponseStream)
        '' from the response stream i need to be able to read.
        Dim xmlDescription As XmlNode = xmlWeatherReader.SelectSingleNode("//forecastday[" & day & "]/icon")
        Dim descriptiontoreturn As String = xmlDescription.InnerText

        Return descriptiontoreturn

    End Function

End Class
