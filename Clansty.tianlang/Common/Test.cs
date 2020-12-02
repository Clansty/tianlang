using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using File = System.IO.File;

namespace Clansty.tianlang
{
    public static class Test
    {
        public static void Do()
        {
            Dictionary<long, string> t=new Dictionary<long, string>
            {
                [888]="aaa",
                [111]="233"
            };
            C.WriteLn(JsonConvert.SerializeObject(t));
        }

        static DataTable XlsxToDataTable(string vFilePath)
        {
            var dataTable = new DataTable();
            try
            {
                var sldocument = new SLDocument(vFilePath);
                dataTable.TableName = sldocument.GetSheetNames()[0];
                var worksheetStatistics = sldocument.GetWorksheetStatistics();
                var startColumnIndex = worksheetStatistics.StartColumnIndex;
                var endColumnIndex = worksheetStatistics.EndColumnIndex;
                var startRowIndex = worksheetStatistics.StartRowIndex;
                var endRowIndex = worksheetStatistics.EndRowIndex;
                for (var i = startColumnIndex; i <= endColumnIndex; i++)
                {
                    var cellValueAsRstType = sldocument.GetCellValueAsRstType(1, i);
                    dataTable.Columns.Add(new DataColumn(cellValueAsRstType.GetText(), typeof(string)));
                }

                for (var j = startRowIndex + 1; j <= endRowIndex; j++)
                {
                    var dataRow = dataTable.NewRow();
                    for (var i = startColumnIndex; i <= endColumnIndex; i++)
                    {
                        dataRow[i - 1] = sldocument.GetCellValueAsString(j, i);
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Xlsx to DataTable: \n" + ex.Message);
            }

            return dataTable;
        }
    }
}