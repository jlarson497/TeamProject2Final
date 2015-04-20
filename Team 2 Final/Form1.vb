Imports System
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Xml

Public Class Form1

    Dim formMap As New oMap
    Dim formWeather As New oWeather
    Dim pushpinList As New ArrayList



    Private Sub btnGetMap_Click(sender As Object, e As EventArgs) Handles btnGetMap.Click
        Dim city As String = txtBoxCityName.Text
        Dim state As String = stateComboBox.Text
        picMap.Image = formMap.makeMap(city, state)
        lblCityLat.Text = formMap.getCityLat(city, state)
        lblCityLong.Text = formMap.getCityLong(city, state)


    End Sub



    '' as a background worker i should be able to call diffferent web request. So they should be queued up here.
    Private Sub apiBackgroundWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles apiBackgroundWorker.DoWork

    End Sub

    Private Sub txtBoxCityName_Click(sender As Object, e As EventArgs) Handles txtBoxCityName.Click
        txtBoxCityName.Clear()
    End Sub


    Private Sub btnGetWeather_Click(sender As Object, e As EventArgs) Handles btnGetWeather.Click
        Dim formWeather As New oWeather
        Dim city As String = txtBoxCityName.Text
        Dim state As String = stateComboBox.Text
        formWeather.getWeather(city, state)
        lblday.Text = formWeather.getForecastday(city, state, 1)
        lblday2.Text = formWeather.getForecastday(city, state, 3)
        lblday3.Text = formWeather.getForecastday(city, state, 5)

        forecastImageBox1.Image = formWeather.getForcastIcon(city, state, 1)
        forecastImageBox2.Image = formWeather.getForcastIcon(city, state, 3)
        forecastImageBox3.Image = formWeather.getForcastIcon(city, state, 5)

        lblDescription1.Text = formWeather.getForcastDescription(city, state, 1)
        lblDescription2.Text = formWeather.getForcastDescription(city, state, 3)
        lblDescription3.Text = formWeather.getForcastDescription(city, state, 5)


    End Sub

    'Flickr

    Dim count As Integer = 1


    '' FLCIKR API KEY 4f60a04f101ef604ead9be84856d9519
    '' FLICKR API SECRET 4e3fc31dff403d28
    Public Function city() As String
        Dim cityFromTextBox As String
        cityFromTextBox = txtBoxCityName.Text
        If cityFromTextBox.Contains(" ") Then
            cityFromTextBox.Replace(" ", "+")
            Return cityFromTextBox
        End If
    End Function

    Public Function state() As String
        Dim stateFromComboBox As String
        stateFromComboBox = CStr(stateComboBox.SelectedItem())
        Return stateFromComboBox
    End Function

    Public Function callCityAndState() As String
        'city()
        'state()
        MessageBox.Show("city and state " + city() + state())
    End Function


    Private Sub btnGetFlickr_Click(sender As Object, e As EventArgs) Handles btnGetFlickr.Click
        'flickrInfo()

        'Adding in some test data so I can test out the new map and list funcitions--- this will add some made up coordinates
        'to see if I can pull them out, add them to the list(box) and build a functioning map with pins
        lblLong1.Text = "31.045"
        lblLat1.Text = "450.123"
        lblLong2.Text = "-45.390"
        lblLat2.Text = "123.456"
        lblLong3.Text = "35.151"
        lblLat3.Text = "46.123"


    End Sub


    Private Sub flickrInfo()
        '' call city and state fuctions
        ' callCityAndState()
        '' base api method use
        ' https://api.flickr.com/services/rest/?method=flickr.photos.search
        '' api key required to make request. This is a temp key expires every 24 hours
        '&api_key=348389c8e6dfd154eeccac983fb067b3
        '&tags=St.+Paul%2C+MN&safe_search=1
        '&content_type=1
        '&lat=44.97010040
        '&lon=-93.08209991
        '&radius=10
        '&radius_units=mi
        'only asking for 3 requests per page
        '&per_page=3
        '&page=1
        '' returned in a rest -xml format
        '&format=rest"
        Try
            If txtBoxCityName.Text = "" Or stateComboBox.SelectedIndex = -1 Then
                MessageBox.Show("You must enter a City and State.")
            Else
                '' still need to rebuild using the flickr.photo.search method with custom search options similar to ones specified here.
                '' this search request is for St. Louis, MO. this particular method does not need to be authorized. and most parameters are optional.
                Dim cityName As String = txtBoxCityName.Text
                Dim requestedTest As String = "https://api.flickr.com/services/rest/?method=flickr.photos.search&api_key=c4471b9774deb717aa6c61f6b88e259a&tags=" & cityName & "&per_page=3&page=" & count & "format=rest"
                '' create the web request using the above url
                Dim flickrRequestPhoto As WebRequest = WebRequest.Create(requestedTest)
                '' store the response stream in the created variable
                Dim testResponseStream As Stream = flickrRequestPhoto.GetResponse.GetResponseStream()
                '' create a xmldocument to format the response
                Dim xmlReader As New XmlDocument
                '' select the single xml-rpc response node
                ' Dim xmlRPCNode As XmlNode
                xmlReader.Load(testResponseStream)
                ' Save the document to a file. White space is
                ' preserved (no white space). = true
                xmlReader.PreserveWhitespace = False '' set to false to preserve the same format as formatted response
                xmlReader.Save("data.xml") '' save the document in the debugger and use response to practice extracting info.
                '' need to obtain xml-rpc siblings at the following values
                ''secret path single node request = //photo/@secret this returns the location. Need to access inner text/xml by calling the innner text method
                Dim url_builder_for_photo_secret1_value_from_XML As XmlNode = xmlReader.SelectSingleNode("//photo/@secret")
                Dim url_builder_for_photo_secret2_value_from_XML As XmlNode = xmlReader.SelectSingleNode("//photo[2]/@secret")
                Dim url_builder_for_photo_secret3_value_from_XML As XmlNode = xmlReader.SelectSingleNode("//photo[3]/@secret")
                'server id'
                Dim url_builder_for_photo_serverID1_value_from_XML As XmlNode = xmlReader.SelectSingleNode("//photo/@server")
                Dim url_builder_for_photo_serverID2_value_from_XML As XmlNode = xmlReader.SelectSingleNode("//photo[2]/@server")
                Dim url_builder_for_photo_serverID3_value_from_XML As XmlNode = xmlReader.SelectSingleNode("//photo[3]/@server")
                'id'
                Dim url_builder_for_photo_ID1_valueXML As XmlNode = xmlReader.SelectSingleNode("//photo/@id")
                Dim url_builder_for_photo_ID2_valueXML As XmlNode = xmlReader.SelectSingleNode("//photo[2]/@id")
                Dim url_builder_for_photo_ID3_valueXML As XmlNode = xmlReader.SelectSingleNode("//photo[3]/@id")
                ' Dim xmlRpcElemet As XmlNodeList = xmlReader.SelectNodes("//photos/@farm")
                '' use this to request farm value
                Dim url_builder_for_farm1_valueXML As XmlNode = xmlReader.SelectSingleNode("//photo/@farm")
                Dim url_builder_for_farm2_valueXML As XmlNode = xmlReader.SelectSingleNode("//photo[2]/@farm")
                Dim url_builder_for_farm3_valueXML As XmlNode = xmlReader.SelectSingleNode("//photo[3]/@farm")
                '' had alot of trouble trying to figure out how to set this up to read each element sibling. Had to use innerxml to extract sibling information on the specified path.
                '' see example of return data at top of txtFile1
                'MessageBox.Show("Here is Farm: " + url_builder_for_farm1_valueXML.InnerXml + vbCrLf +
                ' "Here is secret:" + url_builder_for_photo_secret1_value_from_XML.InnerXml + vbCrLf _
                ' + "Server ID: " + url_builder_for_photo_serverID1_value_from_XML.InnerXml + vbCrLf +
                ' "ID :" + url_builder_for_photo_ID1_valueXML.InnerXml)
                '' confirmation that i obtained the desired values needed to request a photo.
                '' now build a url request in this format.
                ' https://farm{farm-id}.staticflickr.com/{server-id}/{id}_{secret}.jpg
                'For Each number As Integer In 1 to 3
                Dim url_PhotoRequest1 As String = String.Format("https://farm{0}.staticflickr.com/{1}/{2}_{3}.jpg", url_builder_for_farm1_valueXML.InnerXml,
                url_builder_for_photo_serverID1_value_from_XML.InnerXml, url_builder_for_photo_ID1_valueXML.InnerXml,
                url_builder_for_photo_secret1_value_from_XML.InnerXml)
                Dim url_PhotoRequest2 As String = String.Format("https://farm{0}.staticflickr.com/{1}/{2}_{3}.jpg", url_builder_for_farm2_valueXML.InnerXml,
                url_builder_for_photo_serverID2_value_from_XML.InnerXml, url_builder_for_photo_ID2_valueXML.InnerXml,
                url_builder_for_photo_secret2_value_from_XML.InnerXml)
                Dim url_PhotoRequest3 As String = String.Format("https://farm{0}.staticflickr.com/{1}/{2}_{3}.jpg", url_builder_for_farm3_valueXML.InnerXml,
                url_builder_for_photo_serverID3_value_from_XML.InnerXml, url_builder_for_photo_ID3_valueXML.InnerXml,
                url_builder_for_photo_secret3_value_from_XML.InnerXml)
                ''render this image to an image box to be displayed on a bing maps api. Or look into using Flickrs maps
                '"Copy and paste this link into a browser and see what photo is returned " + url_PhotoRequest)
                '' tried to display to messagebox , but could not click on its contents.
                Dim PhotoRequest1 As WebRequest = WebRequest.Create(url_PhotoRequest1)
                Dim photoResponseStream As Stream = PhotoRequest1.GetResponse.GetResponseStream()
                Dim flickrPhotoFromStream As Image = Image.FromStream(photoResponseStream)
                PictureBox1.Image = flickrPhotoFromStream
                Dim PhotoRequest2 As WebRequest = WebRequest.Create(url_PhotoRequest2)
                Dim photoResponseStream2 As Stream = PhotoRequest2.GetResponse.GetResponseStream()
                Dim flickrPhotoFromStream2 As Image = Image.FromStream(photoResponseStream2)
                PictureBox2.Image = flickrPhotoFromStream2
                Dim PhotoRequest3 As WebRequest = WebRequest.Create(url_PhotoRequest3)
                Dim photoResponseStream3 As Stream = PhotoRequest3.GetResponse.GetResponseStream()
                Dim flickrPhotoFromStream3 As Image = Image.FromStream(photoResponseStream3)
                PictureBox3.Image = flickrPhotoFromStream3
                'error 404 page not found
                '' need a try catch block when i reach this point.
                ''todo get the rest of the elements from the initial response.
                ''NOTE: each web request sent generates a different response of results.
                'catch any exceptions and let me know what is going wrong.
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try




    End Sub



    Private Sub btnGetMorePhotos_Click(sender As Object, e As EventArgs) Handles btnGetMorePhotos.Click
        count += 1
        flickrInfo()

    End Sub

    Private Sub btnAddPin1_Click(sender As Object, e As EventArgs) Handles btnAddPin1.Click
        Dim longlat As String = formMap.coordBuilder(lblLong1.Text, lblLat1.Text)
        lstboxCoordinates.Items.Add(longlat)
        pushpinList.Add(longlat)
    End Sub

    Private Sub btnAddPin2_Click(sender As Object, e As EventArgs) Handles btnAddPin2.Click
        Dim longlat As String = formMap.coordBuilder(lblLong2.Text, lblLat2.Text)
        lstboxCoordinates.Items.Add(longlat)
        pushpinList.Add(longlat)
    End Sub

    Private Sub btnMakePinnedMap_Click(sender As Object, e As EventArgs) Handles btnMakePinnedMap.Click
        Dim mapPinURL As String = formMap.urlStringBuilder(pushpinList)
        MessageBox.Show(mapPinURL)

    End Sub
End Class

'To do

'get lat and long from flickr photos
'add lat and long to stringbuilder for map to get static map with pushpins
'improve flickr gallery/options
'Add enumeration for weather descriptions
