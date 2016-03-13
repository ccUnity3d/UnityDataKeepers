function doGet(e) {
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

function convertSheetToJSON(appId) {
  var res = GetObjectsFromAppById(appId);
  for (var i=0;i<res.length;i++)
  {
    if (res[i]["SheetAppId"])
    {
      res[i]["Items"] = GetObjectsFromAppById(res[i]["SheetAppId"]);
      delete res[i]["SheetAppId"];
    }
  }
  return JSON.stringify(res);
}
  
function GetObjectsFromAppById(appId)
{
  var app = SpreadsheetApp.openById(appId);
  var res = [];
  for (var i=0;i<app.getSheets().length;i++)
  {
    var clearTables = getClearTable(app.getSheets()[i]);
    res = res.concat(getObjects(clearTables));
  }
  return res;
}

function getObjects(table)
{
  var oo = [];
  for (var i=1;i<table.length;i++)
  {
    var o = {};
    for (var j=0;j<table[0].length;j++)
    {
      o[table[0][j]]=table[i][j];
    }
    if (o.Type)
      oo.push(o);
  }
  return oo;
}

function getClearTable(sheet)
{
  var data = sheet.getDataRange();
  if (!data)
    throw ";sajd";
  var lines = [];
  var rows = [];
  for (var i=0;i<data.getHeight();i++)
  {
    if (!data.getValues()[i])
      lines.push(i);
    else if (data.getValues()[i][0]=="!")
      lines.push(i);      
  }
  for (var i=0;i<data.getWidth();i++)
  {
    if (data.getValues()[0][i]=="!")
      rows.push(i);
  }
  var res = data.getValues();
  for (var i=lines.length-1;i>=0;i--)
    res = removeLine(res,lines[i]);
  for (var i=rows.length-1;i>=0;i--)
    res = removeRow(res,rows[i]);
  return res;
}

function removeLine(array, lineId)
{
  var data = [];
  for (var i=0;i<array.length-1;i++)
  {
    if (i<lineId)
      data[i] = array[i];
    else
      data[i] = array[i+1];
  }           
  return data;
}

function removeRow(array, rowId)
{
  var data = [];
  for (var i=0;i<array.length;i++)
  {
    data[i]=[];
    for (var j=0;j<array[0].length;j++)
    {
      if (j<=rowId)
        data[i][j]=array[i][j];
      else
        if (j>0) data[i][j-1]=array[i][j];
    }
  }           
  return data;
}