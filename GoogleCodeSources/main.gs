var mainKeeperId = "1XeKTDF21sCho3NVr0VD9Dm4dyvN1MGhwUuuyYpjesZQ";

function onOpen() {
  SpreadsheetApp.getUi()
      .createMenu('Data')
      .addItem('Show JSON', 'showJSON')
      .addToUi();
}

function showJSON()
{
  var json = convertSheetToJSON(mainKeeperId);
  showDialog("json",json);
}

function showDialog(title, text) {
  var html = HtmlService.createHtmlOutputFromFile('InfoDialogPage')
      .setSandboxMode(HtmlService.SandboxMode.IFRAME);
  html.setContent(html.getContent().replace("{text}",text+"<br>"));
  SpreadsheetApp.getUi().showModalDialog(html, title);
}

function doGet(e) {
  // http://script.google.com/macros/s/AKfycbzJSj7mfW1FQpdRPSCgWJRHIoGwFm2x7a3j-fBoz_6XA_oZ5YPK/exec?keeperId=1XeKTDF21sCho3NVr0VD9Dm4dyvN1MGhwUuuyYpjesZQ
  if (e.parameters.keeperId)
  {
    return ContentService.createTextOutput(convertSheetToJSON(e.parameters.keeperId))
           .setMimeType(ContentService.MimeType.JSON);
  }
  else
  {
    var o = {};
    o.error = 404;
    o.message = "Can't find keeper";
    return ContentService.createTextOutput(JSON.stringify(o))
           .setMimeType(ContentService.MimeType.JSON);
  }
}